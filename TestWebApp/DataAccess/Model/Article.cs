namespace DataAccess.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceId { get; set; }
        public Price Price { get; set; }
        public int Amount { get; set; }
        public byte[] ImageData { get; set; }
        public ArtType Type { get; set; }
        public bool Active { get; set; }
        public bool IsInVending { get; set; }
        public int VendingSlot { get; set; }
        public int VendingMachineNumber { get; set; }
    }
}