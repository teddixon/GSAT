using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.IO;
using System.Drawing;

namespace GSATLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class Webcams
    {
        private static Color STAMP_FONT_COLOR = Color.Red;

        /// <summary>
        /// GetImage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Tuple<byte[], string>> GetImage(int id)
        {
            string sQS = "";
            string sURL = "";
            string sUID = "guest";
            string sPWD = "";
            bool addStamp = false;

            sQS = id.ToString();

            // TODO: Move this camera data to database and cache it in App as it doesn't change often.
            switch (sQS)
            {
                case "1": // I-95 sout of Oakland park
                    {
                        sURL = "https://fl511.com/map/Cctv/511--10";
                        addStamp = true;
                        break;
                    }
                case "2": // The 95 south of Sheridan
                    {
                        sUID = ""; // optional
                        sPWD = ""; // optional
                        sURL = "https://fl511.com/map/Cctv/493--10";
                        addStamp = false;
                        break;
                    }
                case "3": //  I-95 in Broward
                    {
                        sURL = "https://fl511.com/map/Cctv/513--10";
                        addStamp = false;
                        break;
                    }
                case "4": // I-95 south of Sunrise Blvd
                    {
                        sURL = "https://fl511.com/map/Cctv/508--10";
                        addStamp = false;
                        break;
                    }
                case "5": // I-95 south of Sunrise Blvd
                    {
                        sURL = "https://fl511.com/map/Cctv/520--10";
                        addStamp = false;
                        break;
                    }
                case "6": // I-95 south of Sunrise Blvd
                    {
                        sURL = "https://fl511.com/map/Cctv/5049-CCTV--8";
                        addStamp = false;
                        break;
                    }
            }

            // Create credentials object.	
            System.Net.NetworkCredential objCredential;
            objCredential = new System.Net.NetworkCredential(sUID, sPWD, "");

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sURL);

                // Set some reasonable limits on resources used by this request
                httpWebRequest.MaximumAutomaticRedirections = 4;
                httpWebRequest.MaximumResponseHeadersLength = 4;

                // Set credentials
                httpWebRequest.Credentials = objCredential;

                // Get Response
                var httpWebResponse = await httpWebRequest.GetResponseAsync();

                // Check for empty by otherwise valid reply
                if (httpWebResponse.ContentLength < 1)
                {
                    httpWebResponse.Close();
                    throw new Exception("0 Bytes returned in image request from camera."); // Only English for now.
                }

                // Use Binary Reader to put response into byte array
                using (BinaryReader reader = new BinaryReader(httpWebResponse.GetResponseStream()))
                {
                    Byte[] blob = reader.ReadBytes((int)httpWebResponse.ContentLength);
                    string ct = httpWebResponse.ContentType;
                    httpWebResponse.Close();
                    if (addStamp)
                    {
                        var img = AddDateTimeStamp(BlobToBitmap(blob));
                        // Only dealing with JPGS in the app so KISS for performance.
                        blob = BitmapToBlob(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                        img.Dispose();
                        // Return the doctored byte array in the IActionResult
                        return new Tuple<byte[], string>(blob, ct);
                    }
                    else
                        // Return the raw byte array in the IActionResult
                        return new Tuple<byte[], string>(blob, ct);
                }
            }
            catch (Exception ex)
            {
                var img = CamOffline(640, 800, ex.Message);
                var blob = BitmapToBlob(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                img.Dispose();
                return new Tuple<byte[], string>(blob, "image/jpg");
            }
        }

        #region Private 

        /// <summary>
        /// Method to return Generic Not Found Picture BitMap
        /// </summary>
        /// <returns>Generic Not Found Picture as System.Drawing.BitMap</returns>
        private static Bitmap CamOffline(int width, int height, string errMessage)
        {
            Graphics g;
            Bitmap img = new Bitmap(height, width);
            g = Graphics.FromImage(img);
            g.Clear(System.Drawing.Color.White);
            Pen pRed5 = new Pen(System.Drawing.Color.Gainsboro, 1);
            Pen pBlack = new Pen(System.Drawing.Color.Black, 1);
            g.DrawLine(pRed5, 0, 0, height, width);
            g.DrawLine(pRed5, 0, width, height, 0);
            g.DrawRectangle(pBlack, new System.Drawing.Rectangle(0, 0, height - 1, width - 1));
            g.DrawString("Camera Offline", new System.Drawing.Font("Arial", 24), Brushes.Red, 200, 220);

            int deltaY = 0;
            int chunkSize = 80;
            List<string> mm = Enumerable.Range(0, errMessage.Length / chunkSize).Select(i => errMessage.Substring(i * chunkSize, chunkSize)).ToList();
            foreach (string s in mm)
            {
                g.DrawString(s, new Font("Arial", 10), Brushes.Red, 15, 310 + deltaY);
                deltaY += 25;
            }
            return img;
        }

        /// <summary>
        /// AddDateTimeStampry>
        /// <param name="blob"></param>
        /// <returns></returns>
        private static Bitmap AddDateTimeStamp(Bitmap bmp)
        {
            Graphics g = System.Drawing.Graphics.FromImage(bmp);
            // Get Time from Portal Setting and check if local Daylight Savings Time.    
            DateTime dt = DateTime.Now.ToLocalTime(); // UtcNow.AddMinutes(().TimeZoneOffset);
            // Check for local daylight savings time.
            if (System.DateTime.Now.IsDaylightSavingTime()) dt = dt.AddHours(1);
            
            // Add the stamp
            g.DrawString(dt.ToLongDateString() + " " + dt.ToLongTimeString(), 
                new Font("Arial", 14, FontStyle.Regular), 
                new SolidBrush(STAMP_FONT_COLOR), 
                new PointF(50, 50));

            // Add silly test message for fun
            g.DrawString("Ted says - Welcome to Broward County! Fasten your seatbelts.",
                new Font("Arial", 14, FontStyle.Regular),
                new SolidBrush(STAMP_FONT_COLOR),
                new PointF(50, 70));

            return bmp;
        }

        /// <summary>
        /// BlobToBitmap
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        private static Bitmap BlobToBitmap(byte[] blob)
        {
            using var ms = new MemoryStream(blob);
            Bitmap bmp = new System.Drawing.Bitmap(ms);
            return bmp;
        }

        /// <summary>
        /// BitmapToBlob
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="imageFormat"></param>
        /// <returns></returns>
        private static byte[] BitmapToBlob(Bitmap img, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            using var ms = new MemoryStream();
            img.Save(ms, imageFormat);
            return ms.ToArray();
        }

        /// <summary>
        /// GetImageCodecInfo (e.g. from "image/jpg")
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static System.Drawing.Imaging.ImageCodecInfo GetImageCodecInfo(string mimeType)
        {
            System.Drawing.Imaging.ImageCodecInfo codecInfo = null;
            var encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < encoders.Length; ++i)
            {
                if (encoders[i].MimeType == mimeType)
                    codecInfo = encoders[i];
            }
            return codecInfo;
        }

        #endregion

    }
}
