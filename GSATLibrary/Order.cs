using System;

namespace GSATLibrary
{
    /// <summary>
    /// Food Order Class
    /// </summary>
    public class Order
    {
        private Guid _orderNumber;

        /// <summary>
        /// Order Number Property
        /// </summary>
        public Guid OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; }
        }

        /// <summary>
        /// Default constructor with automatically generated Guid
        /// </summary>
        public Order()
        {
            _orderNumber = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor allowing specification of OrderNumber
        /// </summary>
        /// <param name="orderNumber"></param>
        public Order(Guid orderNumber)
        {
            _orderNumber = orderNumber;
        }
    }
}
