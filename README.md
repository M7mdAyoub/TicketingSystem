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

## 🌟 Features In Detail

### 1. Multi-Language Engine (English & Arabic)
*   **What does it do?**: The system provides a fully localized experience in both English and Arabic. It doesn't just translate text; it flips the entire layout (RTL support) using a custom layout engine. It remembers user preferences via cookies and adjusts typography (using Google Fonts like Noto Sans Arabic) to ensure readability in both languages.
*   **What did I use?**: Custom `Lang` helper class, Cookie-based localization, and dynamic CSS `dir="rtl"` injection in the main layout.

### 2. Interactive Performance Dashboard
*   **What does it do?**: Provides a bird's-eye view of the system's health. It visualizes ticket distributions (Open vs. In Progress vs. Resolved) through a custom SVG donut chart that animates based on real-time data. It also features a "Live Activity Feed" showing the 5 most recent actions taken across the system.
*   **What did I use?**: C# ViewModels for pre-calculating SVG math (circumference/offsets), and ADO.NET for high-performance data aggregation.

### 3. Comprehensive Ticket Lifecycle Management
*   **What does it do?**: This is the core engine of the app. It allows users to submit inquiries, categorize them, and track their progress. It includes advanced server-side search and filtering (by status or category) and handle pagination for large datasets. Tickets support a "Soft Delete" mechanism, ensuring data isn't accidentally lost.
*   **What did I use?**: SQL Server Indexing, Parameterized SQL queries (t-SQL), and server-side pagination (OFFSET/FETCH).

### 4. Collaborative Comment System
*   **What does it do?**: Facilitates communication between team members. Every ticket has a dedicated discussion thread. Comments are styled as "bubbles" (differently for the current user vs others) and include automated initials generation for user avatars. The system automatically locks the thread once a ticket is marked as "Closed".
*   **What did I use?**: Clean HTML structure (no inline scripts), SQL joins for author metadata, and CSS Flexbox for "reverse" orientation in conversations.

### 5. Smart User Administration
*   **What does it do?**: Allows admins to manage the team through a modern, modal-driven interface. You can create users, edit their profile details, and toggle their active status instantly without leave the page. It features real-time status indicators (online/active dots) on user cards.
*   **What did I use?**: Vanilla JavaScript AJAX (Fetch API), CSS micro-animations for status toggles, and DataAnnotations for robust server-side validation.

### 6. Automated Category Organization
*   **What does it do?**: Enables ticket sorting by business function (e.g., Software, Hardware, Network). Similar to user management, it uses AJAX-powered modals for creation, ensuring a smooth administrative experience.
*   **What did I use?**: JavaScript event delegation for handling dynamic table rows and custom CSS utility classes.

### 7. Advanced Security & Authentication
*   **What does it do?**: Protects sensitive helpdesk data using secure cookie authentication. It enforces strong password policies (Uppercase, Special Chars, Numbers) and includes built-in anti-forgery protection (CSRF) for every form submission and AJAX request.
*   **What did I use?**: ASP.NET Core Cookie Auth, `[ValidateAntiForgeryToken]`, and BCrypt-compatible password hashing (simulated with parameterized queries).

### 8. Premium Nature-Inspired UI/UX
*   **What does it do?**: The "NatureDesk" aesthetic provides a calm, productive environment. It features a fully responsive design that works on mobile, tablet, and desktop. The UI is completely free of inline scripts and styles, following a strict "Separation of Concerns" architecture for maximum loading speed and security.
*   **What did I use?**: Modern CSS Custom Properties (Variables), Google Fonts API, and a custom JavaScript page-loading convention.

