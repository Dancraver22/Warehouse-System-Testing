# Warehouse Management System

A comprehensive .NET 10 console application for managing warehouse inventory, stock tracking, and product management with SQLite database integration.

## Features

✅ **Process Scans** - Remove stock from inventory by SKU  
✅ **Insert/Update Products** - Add new products or update existing ones  
✅ **View Dashboard** - Real-time audit of all inventory items  
✅ **Delete Items** - Remove specific products by SKU  
✅ **Stock Alerts** - Automatic critical reorder warnings  
✅ **Safety Stock Tracking** - Monitor safety stock levels  
✅ **Reset System** - Complete database reset with confirmation  
✅ **Exit to Menu** - Easy navigation with escape options in all menus

## System Requirements

- **.NET 10** or later
- **Windows/Mac/Linux** (cross-platform support)
- **SQLite** (included via NuGet package)

## Installation

### 1. Clone the Repository
```bash
git clone https://github.com/Dancraver22/Warehouse-System-Testing.git
cd Warehouse-System-Testing
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Project
```bash
dotnet build
```

## Running the Application

### Console Version (Current)
```bash
cd "Warehouse System Testing"
dotnet run
```

### Using the Menu
```
========================================
    WAREHOUSE MANAGEMENT SYSTEM
========================================
1. Process Scans (Remove Stock)
2. Insert/Update Product
3. View Dashboard (Audit)
4. Delete Specific Item (By SKU)
5. CLEAR ALL DATA (Reset System)
6. Exit
```

## Usage Examples

### Adding a Product
1. Select option **2** (Insert/Update Product)
2. Choose **1** to add a product (or **2** to return to main menu)
3. Enter product details:
   - **SKU**: Unique identifier (e.g., PROD001)
   - **Name**: Product name
   - **Stock**: Current stock quantity
   - **Reorder Point**: Minimum stock level
   - **Safety Stock**: Emergency stock buffer
   - **Price**: Product price

### Processing a Scan
1. Select option **1** (Process Scans)
2. Choose **1** to process a scan (or **2** to return to main menu)
3. Enter the SKU to locate the product
4. Enter the amount to remove from stock
5. System will verify sufficient stock before processing

### Viewing Inventory
Select option **3** (View Dashboard) to see:
- Product ID
- Product Name
- Current Stock Level
- Status (Stable or CRITICAL: REORDER NOW)

## Database

The application uses **SQLite** for persistent data storage. The database file `warehouse.db` is automatically created in the application directory.

### Database Schema
```sql
CREATE TABLE Inventory (
    ID TEXT PRIMARY KEY,
    Name TEXT,
    Stock INTEGER,
    ReorderPoint INTEGER,
    SafetyStock INTEGER,
    Price REAL
);
```

## Project Structure

```
Warehouse System Testing/
├── Program.cs                          # Main application code
├── Warehouse System Testing.csproj     # Project configuration
└── warehouse.db                        # SQLite database (auto-generated)
```

## Key Classes

### `Item`
Represents a warehouse item with properties:
- ID (SKU)
- Name
- Stock quantity
- Reorder Point
- Safety Stock
- Current Price
- Status (automatically updated)

### `InventoryManager`
Handles all inventory operations:
- Database setup and management
- Stock processing and transactions
- Product CRUD operations
- Dashboard display

## Stock Status Logic

Items automatically get flagged as **"CRITICAL: REORDER NOW"** when:
```
Stock ≤ (Reorder Point + Safety Stock)
```

This ensures adequate buffer before stockouts occur.

## Transaction Safety

The system includes built-in protections:
- ❌ **Prevents over-removal**: Cannot remove more stock than available
- ❌ **Prevents negative stock**: Transactions denied if they would result in negative stock
- ✅ **Auto-status updates**: Stock status updated in real-time

## Web Hosting (Coming Soon)

An **ASP.NET Core** web version is in development! This will provide:
- 🌐 Web-based UI accessible from any browser
- 📊 Interactive dashboard
- 🔌 REST API endpoints
- ☁️ Cloud deployment support

**Hosting Link**: *(To be added)*

## Error Handling

The application gracefully handles:
- Invalid input formats
- Missing products
- Database errors
- Insufficient stock scenarios

## Future Enhancements

- 🌐 ASP.NET Core Web Interface (MVC/Blazor)
- 📱 Mobile app support
- 📈 Advanced analytics and reporting
- 🔐 User authentication and roles
- 📧 Email notifications for critical stock
- 📦 Multi-warehouse support
- 🔄 Backup and restore functionality

## Troubleshooting

### Issue: Database file not found
**Solution**: Run the application once. The `warehouse.db` file will be auto-created.

### Issue: SQLite error
**Solution**: Ensure the application has write permissions to the directory.

### Issue: Cannot find product
**Solution**: Check the exact SKU. SKUs are case-insensitive but must match exactly.

## Contributing

Contributions are welcome! Feel free to:
- Report bugs
- Suggest features
- Submit pull requests
- Improve documentation

## License

This project is open source. Feel free to use and modify as needed.

## Contact

For questions or feedback, please open an issue on GitHub:
https://github.com/Dancraver22/Warehouse-System-Testing/issues

## Changelog

### Version 1.0.0
- Initial release
- Console-based warehouse management system
- SQLite database integration
- Product CRUD operations
- Stock processing
- Dashboard view
- Exit-to-menu navigation for all submenus

---

**Happy Inventory Management! 📦**
