using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Web.Models;

namespace Warehouse.Web.Services
{
    public class InventoryManager
    {
        private List<Item> _items = new List<Item>();
        private string _dbPath = "Data Source=warehouse.db";

        public InventoryManager()
        {
            SetupDatabase();
        }

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

        // Get all items
        public List<Item> GetAllItems()
        {
            return _items;
        }

        // Get item by ID
        public Item? GetItemById(string id)
        {
            return _items.FirstOrDefault(i => i.ID == id.ToUpper());
        }

        // Add or update product
        public void AddOrUpdateProduct(string id, string name, int stock, int reorder, int safety, double price)
        {
            try
            {
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

                LoadFromSQL();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding/updating product: {ex.Message}");
            }
        }

        // Process transaction (remove stock)
        public bool ProcessTransaction(string id, int amount)
        {
            var item = GetItemById(id);
            if (item == null)
                return false;

            if (amount > item.Stock)
                return false;

            item.Stock -= amount;
            item.UpdateStatus();
            UpdateStockInSQL(item.ID, item.Stock);
            return true;
        }

        // Delete product
        public bool DeleteProduct(string id)
        {
            using (var connection = new SqliteConnection(_dbPath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Inventory WHERE ID = $id";
                command.Parameters.AddWithValue("$id", id.ToUpper());
                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    LoadFromSQL();
                    return true;
                }
                return false;
            }
        }

        // Reset database
        public void ResetDatabase()
        {
            using (var connection = new SqliteConnection(_dbPath))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Inventory";
                command.ExecuteNonQuery();
            }
            _items.Clear();
        }

        // Private utilities
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

        private void LoadFromSQL()
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
    }
}
