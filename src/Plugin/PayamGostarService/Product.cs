using System.Runtime.Serialization;

namespace Septa.PayamGostar.ServiceLayer.Contract.DataContracts
{
    // warning: it's better to rename the name of class, it may be confilicted with Nop.Core.Domain.Catalog.Product

    /// <summary>
    /// Represents a product from Payam Gostar
    /// </summary>
    [DataContract]
    public class Product
    {
        /// <summary>
        /// Product's code - used for identifying product
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// product's unit price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// products name
        /// </summary>
        public string Name { get; set; }
    }
}
