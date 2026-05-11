# Azure Deployment Guide

This guide will help you deploy the Warehouse Management System web app to Azure App Service.

## Prerequisites

- Azure account (free tier available at https://azure.microsoft.com/en-us/free/)
- GitHub repository access
- Admin access to your Azure subscription

## Step-by-Step Deployment Setup

### Step 1: Create an Azure App Service

1. Go to **Azure Portal**: https://portal.azure.com
2. Click **"Create a resource"**
3. Search for **"App Service"** and click it
4. Click **"Create"**

#### Fill in the Details:

**Basics Tab:**
- **Resource Group**: Create new (e.g., `warehouse-rg`)
- **Name**: `warehouse-system-app` (must be unique - Azure will tell you)
  - This becomes your URL: `warehouse-system-app.azurewebsites.net`
- **Code**: Select this
- **Runtime stack**: .NET 10 (LTS)
- **Operating System**: Linux (cheaper) or Windows
- **Region**: Choose closest to you (e.g., East US, West Europe)
- **App Service Plan**: Click "Create new"
  - **Name**: `warehouse-plan`
  - **Sku and Size**: Click "Change size" → Select **F1 (Free)** tier
    - ⭐ Free tier includes 60 minutes/day
    - Upgrade later if needed

**Deployment Tab:**
- **Source**: GitHub (we'll configure this next)
- **GitHub Account**: Click "Authorize" and sign in
- **Organization**: Select your GitHub username
- **Repository**: `Warehouse-System-Testing`
- **Branch**: `main`

3. Click **"Review + Create"** → **"Create"**
4. Wait for deployment (2-3 minutes)

---

### Step 2: Configure GitHub Deployment

Once Azure App Service is created:

1. In Azure Portal, go to your **App Service** (it should show in notifications)
2. In the left menu, click **"Deployment Center"**
3. You should see GitHub is already connected
4. Under "Build provider", select **"GitHub Actions"**
5. Azure will create a workflow automatically, but we're using our custom one

---

### Step 3: Add GitHub Secrets for Deployment

1. Go to your GitHub repository: https://github.com/Dancraver22/Warehouse-System-Testing
2. Click **Settings** (top right)
3. In left menu, click **"Secrets and variables"** → **"Actions"**
4. Click **"New repository secret"**

**Add Secret 1:**
- **Name**: `AZURE_APP_NAME`
- **Value**: (the name you chose in Step 1, e.g., `warehouse-system-app`)
- Click **"Add secret"**

**Add Secret 2:**
- **Name**: `AZURE_PUBLISH_PROFILE`
- **Value**: (we need to get this from Azure)

#### Getting the Publish Profile:

1. Back in Azure Portal, go to your App Service
2. Click **"Get publish profile"** (button in top bar)
3. This downloads an `.PublishSettings` file
4. Open it with a text editor
5. Copy **ALL** the contents
6. Paste into the GitHub secret value for `AZURE_PUBLISH_PROFILE`
7. Click **"Add secret"**

---

### Step 4: Test the Deployment

1. Go to your GitHub repo
2. Make a small change to `README.md` (or any file)
3. Commit and push to `main` branch
4. Go to **GitHub** → **Actions** tab
5. Watch the workflow run
6. Once it completes (green checkmark), your app is deployed!

---

## Accessing Your Live Application

Once deployed, visit:
```
https://warehouse-system-app.azurewebsites.net
```

Replace `warehouse-system-app` with the actual name you chose.

---

## What This Workflow Does

1. **Checkout code** from your GitHub repo
2. **Setup .NET 10** environment
3. **Restore** NuGet packages
4. **Build** the project in Release mode
5. **Publish** the application
6. **Deploy** to Azure App Service

Every time you push to `main`, this runs automatically! 🚀

---

## Monitoring Your Deployment

In Azure Portal:
- **App Service** → **Overview**: See if app is running
- **App Service** → **Logs**: View error logs if something fails
- **App Service** → **Metrics**: Monitor performance and resource usage

---

## Troubleshooting

### Issue: "Build failed"
- Check GitHub Actions logs for error details
- Ensure `Warehouse.Web.csproj` path is correct

### Issue: "Deployment failed"
- Check Azure App Service logs
- Verify database file path (may need to use Azure Storage or App Service temp directory)

### Issue: "404 Not Found" when accessing app
- Wait 2-3 minutes after deployment completes
- Check app is running in Azure Portal → App Service → Overview

### Issue: Database issues
- The SQLite `warehouse.db` will be created in the app's temporary directory
- On App Service free tier, this resets when app restarts
- **Upgrade to Paid**: For persistent database, use Azure SQL Database or Azure Blob Storage

---

## Next Steps

### Current Setup (Free Tier - Limited)
- Works great for testing
- Database resets on app restart
- 60 minutes of uptime per day

### Upgrade Options (When Ready)

**Option 1: Persistent Database**
- Use Azure SQL Database or PostgreSQL
- Costs ~$5-15/month

**Option 2: Better App Plan**
- Upgrade from F1 (Free) to B1 (Basic)
- Costs ~$10-20/month
- Always running, auto-scaling available

**To upgrade in Azure Portal:**
1. App Service → **Scale up** (App Service plan)
2. Choose desired tier
3. Click **Select**

---

## Deployment Architecture

```
Your Local Computer
         ↓
    GitHub Repo (main branch)
         ↓
  GitHub Actions Workflow
         ↓
  Build & Test (.NET 10)
         ↓
   Azure App Service
         ↓
  Live on Internet! 🌐
  (warehouse-system-app.azurewebsites.net)
```

---

## Support

For Azure-specific help:
- Azure Docs: https://docs.microsoft.com/en-us/azure/app-service/
- GitHub Actions Docs: https://docs.github.com/en/actions

Happy deploying! 🚀
