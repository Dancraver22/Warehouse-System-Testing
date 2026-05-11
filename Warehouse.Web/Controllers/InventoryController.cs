using Microsoft.AspNetCore.Mvc;
using Warehouse.Web.Models;
using Warehouse.Web.Services;

namespace Warehouse.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryManager _inventoryManager;

        public InventoryController()
        {
            _inventoryManager = new InventoryManager();
        }

        // Dashboard - View all products
        public IActionResult Dashboard()
        {
            var items = _inventoryManager.GetAllItems();
            return View(items);
        }

        // Add/Edit Product - GET
        public IActionResult AddProduct()
        {
            return View();
        }

        // Add/Edit Product - POST
        [HttpPost]
        public IActionResult AddProduct(string id, string name, int stock, int reorderPoint, int safetyStock, double price)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Error = "SKU and Name are required.";
                return View();
            }

            try
            {
                _inventoryManager.AddOrUpdateProduct(id, name, stock, reorderPoint, safetyStock, price);
                TempData["Success"] = $"Product {id} added/updated successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // Process Scan - GET
        public IActionResult ProcessScan()
        {
            return View();
        }

        // Process Scan - POST
        [HttpPost]
        public IActionResult ProcessScan(string sku, int amount)
        {
            if (string.IsNullOrWhiteSpace(sku) || amount <= 0)
            {
                ViewBag.Error = "Please enter a valid SKU and amount.";
                return View();
            }

            var item = _inventoryManager.GetItemById(sku);
            if (item == null)
            {
                ViewBag.Error = $"Product with SKU '{sku}' not found.";
                return View();
            }

            if (amount > item.Stock)
            {
                ViewBag.Error = $"Insufficient stock! Only {item.Stock} available.";
                return View();
            }

            if (_inventoryManager.ProcessTransaction(sku, amount))
            {
                TempData["Success"] = $"Successfully removed {amount} units from {item.Name}";
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Error = "Transaction failed.";
                return View();
            }
        }

        // Delete Product - GET
        public IActionResult DeleteProduct(string id)
        {
            var item = _inventoryManager.GetItemById(id);
            if (item == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("Dashboard");
            }
            return View(item);
        }

        // Delete Product - POST
        [HttpPost]
        public IActionResult DeleteProductConfirm(string id)
        {
            if (_inventoryManager.DeleteProduct(id))
            {
                TempData["Success"] = $"Product {id} deleted successfully!";
                return RedirectToAction("Dashboard");
            }
            else
            {
                TempData["Error"] = "Failed to delete product.";
                return RedirectToAction("Dashboard");
            }
        }

        // Reset Database - GET
        public IActionResult Reset()
        {
            return View();
        }

        // Reset Database - POST
        [HttpPost]
        public IActionResult ResetConfirm()
        {
            _inventoryManager.ResetDatabase();
            TempData["Success"] = "Database reset successfully. All data has been cleared.";
            return RedirectToAction("Dashboard");
        }
    }
}
