using System;

namespace AOI.Models
{
    [Serializable]
    public class CartLine
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
