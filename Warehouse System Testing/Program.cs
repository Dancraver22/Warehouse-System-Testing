using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace WarehousePro
{
    public class Program
    {
        static void Main(string[] args)
        {
            var inventory = new InventoryManager();
            inventory.SetupDatabase();

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n" + new string('=', 40));
                Console.WriteLine("    WAREHOUSE MANAGEMENT SYSTEM");
                Console.WriteLine(new string('=', 40));
                Console.WriteLine("1. Process Scans (Remove Stock)");
                Console.WriteLine("2. Insert/Update Product");
                Console.WriteLine("3. View Dashboard (Audit)");
                Console.WriteLine("4. Delete Specific Item (By SKU)");
                Console.WriteLine("5. CLEAR ALL DATA (Reset System)");
                Console.WriteLine("6. Exit");
                Console.Write("\nSelect Option: ");

                switch (Console.ReadLine())
                {
                    case "1": inventory.StartScanner(); break;
                    case "2": inventory.AddNewProductUI(); break;
                    case "3": inventory.DisplayDashboard(); break;
                    case "4": inventory.DeleteProductUI(); break;
                    case "5": inventory.ResetDatabaseUI(); break;
                    case "6": running = false; break;
                    default: Console.WriteLine("[!] Invalid selection."); break;
                }
            }
        }
    }

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
            ID = id.ToUpper(); Name = name.ToUpper(); Stock = stock;
            ReorderPoint = reorder; SafetyStock = safety; CurrentPrice = price;
            UpdateStatus(); // Set initial status
        }

        public void UpdateStatus()
        {
            if (Stock <= (ReorderPoint + SafetyStock))
                Status = "CRITICAL: REORDER NOW";
            else
                Status = "Stable";
        }
    }

    public class InventoryManager
    {
        private List<Item> _items = new List<Item>();
        private string _dbPath = "Data Source=warehouse.db";

        public void SetupDatabase()
        {
            using (var connection = new SqliteConnection(_dbPath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Inventory (
                        ID TEXT PRIMARY KEY, Name TEXT, Stock INTEGER, 
                        ReorderPoint INTEGER, SafetyStock INTEGER, Price REAL
                    );";
                command.ExecuteNonQuery();
            }
            LoadFromSQL();
        }

        // --- 1. PROCESS SCANS ---
        public void StartScanner()
        {
            Console.WriteLine("\n[SCANNER] Process Scans");
            Console.WriteLine("1. Process Scan");
            Console.WriteLine("2. Return to Main Menu");
            Console.Write("Select Option: ");

            if (Console.ReadLine() == "2")
                return;

            Console.Write("\n[SCANNER] Enter SKU: ");
            string inputID = Console.ReadLine() ?? "";
            var item = _items.FirstOrDefault(i => i.ID == inputID.ToUpper());

            if (item != null)
            {
                Console.WriteLine($"Found: {item.Name} | Current Stock: {item.Stock}");
                Console.Write("Amount to remove: ");
                if (int.TryParse(Console.ReadLine(), out int qty))
                {
                    ProcessTransaction(item, qty);
                }
            }
            else { Console.WriteLine("[!] SKU Not Found."); }
        }

        private void ProcessTransaction(Item item, int amount)
        {
            if (amount > item.Stock)
            {
                Console.WriteLine("\n[!!!] TRANSACTION DENIED [!!!]");
                Console.WriteLine($"Only {item.Stock} available. Cannot remove {amount}.");
                item.Status = "DENIED: STOCKOUT PREVENTED";
                return;
            }

            item.Stock -= amount;
            item.UpdateStatus();
            UpdateStockInSQL(item.ID, item.Stock);
            Console.WriteLine($">>> Success! {item.Name} updated.");
        }

        // --- 2. ADD/UPDATE PRODUCT ---
        public void AddNewProductUI()
        {
            Console.WriteLine("\n--- PRODUCT ENTRY ---");
            Console.WriteLine("1. Add/Update Product");
            Console.WriteLine("2. Return to Main Menu");
            Console.Write("Select Option: ");

            if (Console.ReadLine() == "2")
                return;

            try
            {
                Console.WriteLine("\n--- PRODUCT ENTRY ---");
                Console.Write("SKU: "); string id = (Console.ReadLine() ?? "").ToUpper();
                Console.Write("Name: "); string name = Console.ReadLine() ?? "";
                Console.Write("Stock: "); int stock = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Reorder Point: "); int reorder = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Safety Stock: "); int safety = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("Price: "); double price = double.Parse(Console.ReadLine() ?? "0");

                var newItem = new Item(id, name, stock, reorder, safety, price);

                using (var connection = new SqliteConnection(_dbPath))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT OR REPLACE INTO Inventory VALUES ($id, $name, $stock, $reorder, $safety, $price)";
                    command.Parameters.AddWithValue("$id", newItem.ID);
                    command.Parameters.AddWithValue("$name", newItem.Name);
                    command.Parameters.AddWithValue("$stock", newItem.Stock);
                    command.Parameters.AddWithValue("$reorder", newItem.ReorderPoint);
                    command.Parameters.AddWithValue("$safety", newItem.SafetyStock);
                    command.Parameters.AddWithValue("$price", newItem.CurrentPrice);
                    command.ExecuteNonQuery();
                }

                // Refresh list
                LoadFromSQL();
                Console.WriteLine(">>> Database Updated Successfully.");
            }
            catch { Console.WriteLine("[!] Error: Invalid data format entered."); }
        }

        // --- 3. DELETE SPECIFIC ID ---
        public void DeleteProductUI()
        {
            Console.Write("\nEnter SKU to DELETE: ");
            string id = (Console.ReadLine() ?? "").ToUpper();

            using (var connection = new SqliteConnection(_dbPath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Inventory WHERE ID = $id";
                command.Parameters.AddWithValue("$id", id);
                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    Console.WriteLine($">>> {id} permanently removed from database.");
                    LoadFromSQL();
                }
                else { Console.WriteLine("[!] SKU not found in database."); }
            }
        }

        // --- 4. CLEAR ALL DATA ---
        public void ResetDatabaseUI()
        {
            Console.WriteLine("\n[WARNING] This will wipe ALL warehouse data.");
            Console.Write("Type 'CONFIRM' to proceed: ");
            if (Console.ReadLine() == "CONFIRM")
            {
                using (var connection = new SqliteConnection(_dbPath))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "DELETE FROM Inventory";
                    command.ExecuteNonQuery();
                }
                _items.Clear();
                Console.WriteLine(">>> System Reset Complete. Database is empty.");
            }
            else { Console.WriteLine("Reset cancelled."); }
        }

        // --- UTILS ---
        private void UpdateStockInSQL(string itemID, int newStock)
        {
            using (var connection = new SqliteConnection(_dbPath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE Inventory SET Stock = $stock WHERE ID = $id";
                command.Parameters.AddWithValue("$stock", newStock);
                command.Parameters.AddWithValue("$id", itemID);
                command.ExecuteNonQuery();
            }
        }

        public void LoadFromSQL()
        {
            _items.Clear();
            using (var connection = new SqliteConnection(_dbPath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Inventory";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _items.Add(new Item(
                            reader.GetString(0), reader.GetString(1), reader.GetInt32(2),
                            reader.GetInt32(3), reader.GetInt32(4), reader.GetDouble(5)));
                    }
                }
            }
        }

        public void DisplayDashboard()
        {
            Console.WriteLine("\n{0,-10} | {1,-15} | {2,-6} | {3}", "ID", "NAME", "STOCK", "STATUS");
            Console.WriteLine(new string('-', 50));
            foreach (var item in _items)
                Console.WriteLine($"{item.ID,-10} | {item.Name,-15} | {item.Stock,-6} | {item.Status}");
        }
    }
}