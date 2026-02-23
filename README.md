# 🎫 Helpdesk Ticketing System

A modern helpdesk ticketing system built with **ASP.NET Core MVC**, **SQL Server**, and **ADO.NET** (no Entity Framework).

## ⚡ Tech Stack

- ASP.NET Core MVC (.NET 9)
- SQL Server + ADO.NET
- Bootstrap 5 + Bootstrap Icons
- Cookie Authentication

## 🚀 Setup

### 1. Database

- Open **SQL Server Management Studio (SSMS)**
- Create a new database called `HelpdeskDB`
- Run the script: `SQL/HelpdeskDB_Setup.sql`
- Then run: `SQL/SeedData_Additional.sql` (for sample data)

### 2. Connection String

Update `appsettings.json` with your SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=HelpdeskDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3. Run

```bash
dotnet run
```

## 🔑 Default Login

| Email | Password |
|---|---|
| `mohammadayoub@helpdesk.com` | `Test@1234` |
| `alimahmoud@helpdesk.com` | `Test@1234` |

> **All users** share the same password: `Test@1234`

## 📁 Project Structure

```
Controllers/     → AccountController, TicketsController, UsersController, CategoriesController
Models/          → User, Ticket, TicketComment, Category, ViewModels
Data/            → DbHelper (ADO.NET connection helper)
Views/           → Razor views for all pages
SQL/             → Database setup and seed data scripts
```

## ✅ Features

- Login / Logout (Cookie Authentication)
- Ticket Management (Create, List, Details, Soft Delete)
- Search, Filter by Category/Status, Pagination
- Comments on Tickets (blocked for Closed tickets)
- User Management (Create, List, Activate/Deactivate)
- Category Management (Create, List, Activate/Deactivate)
- Landing Page
- Parameterized SQL queries throughout
- DataAnnotations validation
- Strong password enforcement
