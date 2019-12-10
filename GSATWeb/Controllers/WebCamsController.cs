using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Net;
using System.IO;
using System.Text;
using System.Drawing;

namespace GSATWeb.Controllers
{
    /// <summary>
    /// Web Cameras API Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WebCamsController : ControllerBase
    {
        #region Fields

        private readonly ILogger<WebCamsController> _logger;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public WebCamsController(ILogger<WebCamsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get by Camera ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Tuple<byte[], string> x = await GSATLibrary.Webcams.GetImage(id);
            return File(x.Item1, x.Item2);
        }

        #endregion
    }
}
