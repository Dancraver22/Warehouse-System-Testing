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

### Console Version
```bash
cd "Warehouse System Testing"
dotnet run
```

### Web Version (ASP.NET Core MVC)
```bash
cd Warehouse.Web
dotnet run
```

The web application will start and be accessible at:
- **Local**: https://localhost:5000 (or https://localhost:5001)
- **Production**: *(Coming soon - Follow the [Azure Deployment Guide](AZURE_DEPLOYMENT_GUIDE.md))*

#### Web Features
- 🌐 Modern web interface with responsive design
- 📊 Interactive dashboard for inventory management
- ➕ Easy product add/update forms
- 🔍 Quick scan processing interface
- 🗑️ Safe product deletion with confirmation
- 🔄 Database reset functionality
- 📱 Mobile-friendly Bootstrap UI

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
├── Warehouse System Testing/           # Console Application
│   ├── Program.cs                      # Main console app
│   ├── Warehouse System Testing.csproj # Project config
│   └── warehouse.db                    # SQLite database
│
├── Warehouse.Web/                      # ASP.NET Core MVC Web App
│   ├── Controllers/
│   │   ├── HomeController.cs           # Home page
│   │   └── InventoryController.cs      # Inventory operations
│   ├── Models/
│   │   └── Item.cs                     # Item model
│   ├── Services/
│   │   └── InventoryManager.cs         # Core business logic
│   ├── Views/
│   │   ├── Inventory/
│   │   │   ├── Dashboard.cshtml        # Dashboard view
│   │   │   ├── AddProduct.cshtml       # Add product form
│   │   │   ├── ProcessScan.cshtml      # Scan processing form
│   │   │   ├── DeleteProduct.cshtml    # Delete confirmation
│   │   │   └── Reset.cshtml            # Reset confirmation
│   │   └── Shared/                     # Layout & shared views
│   └── Program.cs                      # Web app startup config
│
├── README.md                            # This file
└── .gitignore                          # Git ignore rules
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

## Web Hosting & Deployment

An **ASP.NET Core MVC** web version is now available with automated Azure deployment!

### Quick Start - Local Development
```bash
cd Warehouse.Web
dotnet run
```
Visit: **https://localhost:5000**

### Production Deployment - Azure App Service

**The recommended and easiest way to deploy:**

1. **[Follow the Azure Deployment Guide](AZURE_DEPLOYMENT_GUIDE.md)** - Step-by-step instructions
2. **GitHub Actions** - Automatic deployment on every push to `main`
3. **Free Tier Available** - Start with Azure's free tier, upgrade as you grow

**Benefits:**
- ✅ Automatic deployment on every GitHub push
- ✅ .NET 10 native support (no compatibility issues)
- ✅ Free tier available ($0 startup cost)
- ✅ Professional hosting infrastructure
- ✅ Built-in monitoring and scaling

**Deployment Workflow:**
```
Push to GitHub → GitHub Actions → Build & Test → Deploy to Azure → Live! 🚀
```

### Alternative Hosting Options

Other platforms that support .NET:
- **Railway.app** - Similar to Vercel, supports .NET
- **Render.com** - Free tier available
- **DigitalOcean App Platform** - $5-12/month
- **Heroku** - With buildpack support

### Web Version Features
- ✅ Modern, responsive web UI (Bootstrap)
- ✅ Dashboard for inventory overview
- ✅ Add/Update products via forms
- ✅ Process stock scans
- ✅ Delete products with confirmation
- ✅ Reset database with safety confirmation
- ✅ Real-time stock status indicators
- ✅ Mobile-friendly interface
- ✅ Automated CI/CD with GitHub Actions

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

### Version 2.0.0 (Current)
- **NEW**: ASP.NET Core MVC web application
- **NEW**: Modern web UI with Bootstrap
- **NEW**: Interactive dashboard with real-time status
- **IMPROVED**: Shared InventoryManager service between console and web
- **IMPROVED**: Better user experience with confirmations
- Console and web versions now share core logic

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
