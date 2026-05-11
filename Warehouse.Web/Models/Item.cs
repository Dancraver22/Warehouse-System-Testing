namespace Warehouse.Web.Models
{
    public class Item
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public int ReorderPoint { get; set; }
        public int SafetyStock { get; set; }
        public double CurrentPrice { get; set; }
        public string Status { get; set; } = "Stable";

        public Item(string id, string name, int stock, int reorder, int safety, double price)
        {
            ID = id.ToUpper();
            Name = name.ToUpper();
            Stock = stock;
            ReorderPoint = reorder;
            SafetyStock = safety;
            CurrentPrice = price;
            UpdateStatus();
        }

        public void UpdateStatus()
        {
            if (Stock <= (ReorderPoint + SafetyStock))
                Status = "CRITICAL: REORDER NOW";
            else
                Status = "Stable";
        }
    }
}
