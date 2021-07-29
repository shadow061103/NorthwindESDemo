namespace NorthwindDemo.Repository.Models.ES
{
    public class OrderDetailsESModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }

        public virtual ProductsESModel Product { get; set; }
    }
}