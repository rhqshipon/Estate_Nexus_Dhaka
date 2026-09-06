# EstateNexus

EstateNexus is a robust, role-based Real Estate Management System developed as a high-performance Windows Forms desktop application. The system provides a unified, secure platform for property buyers, tenants, sellers/agents, and super administrators to manage listings, schedule site visits, conduct commercial transactions, and maintain marketplace integrity.

---

## Table of Contents
1. [Project Overview](#project-overview)
2. [Technology Stack](#technology-stack)
3. [Main Features](#main-features)
   - [Authentication & Registration](#authentication--registration)
   - [Customer Features](#customer-features)
   - [Seller / Admin Features](#seller--admin-features)
   - [Super Admin Features](#super-admin-features)
4. [Application Workflow](#application-workflow)
5. [Platform Revenue Model](#platform-revenue-model)
6. [Installation & Setup](#installation--setup)
7. [Default Seeded Accounts](#default-seeded-accounts)
8. [Project Structure](#project-structure)
9. [Main Forms](#main-forms)
10. [Screenshots](#screenshots)
11. [Known Future Enhancements](#known-future-enhancements)

---

## Project Overview

EstateNexus delivers an end-to-end real estate solution catering to three primary user roles:

1. **Customer**: Individuals looking to explore residential and commercial properties for sale or rent. Customers can perform advanced filtering, book on-site visit appointments, add properties to a digital cart with flexible rental periods, process checkout orders across multiple payment methods, print transaction invoices, and submit property ratings and reviews.
2. **Seller / Admin**: Property owners and agents who manage their real estate catalog. Sellers can publish new property listings with rich image previews, update pricing and details, manage property availability status (Available, Sold, Rented), review and approve/reject scheduled customer visits, and monitor closed sales and revenue records.
3. **Super Admin**: System administrators with platform-wide governance. Super Admins approve or reject pending seller/admin registrations, suspend or reactivate compromised accounts, monitor global marketplace properties, review platform order volume, and analyze the 5% platform commission revenue stream.

---

## Technology Stack

The project uses the following confirmed technologies and versions:

- **Programming Language**: C# 14.0 / C# (.NET 10 compatible)
- **Application Framework**: Windows Forms (`net10.0-windows`)
- **Runtime Target**: `.NET 10.0` (`<TargetFramework>net10.0-windows</TargetFramework>`)
- **Object-Relational Mapping (ORM)**: Entity Framework Core 10.0.11 (`Microsoft.EntityFrameworkCore.SqlServer` 10.0.11, `Microsoft.EntityFrameworkCore.Design` 10.0.11)
- **Database Engine**: Microsoft SQL Server (LocalDB / SQL Server Express / MSSQLSERVER)
- **Data Provider**: Microsoft Data SqlClient 7.0.2 (`Microsoft.Data.SqlClient`)
- **Development Environment**: Microsoft Visual Studio 2022+ / Visual Studio 2026 Preview

---

## Main Features

### Authentication & Registration
- **Flexible Identifier Login**: Login using either registered Email address or Username alias.
- **SHA-256 Cryptographic Hashing**: Secure password hashing with 64-character hexadecimal representation via `PasswordHelper.cs`.
- **Automatic Password Upgrade**: Seamlessly detects legacy plain-text password entries upon successful authentication and automatically upgrades the database record to a SHA-256 hash.
- **Show / Hide Password**: Interactive toggle to switch password input visibility between masked (`*`) and plain-text.
- **Clear Form Action**: One-click form reset clearing all inputs and active validation indicators.
- **Input Validation & ErrorProvider**: Real-time validation for missing credentials with inline error indicators (`ErrorProvider`) and localized warning labels.
- **Confirm Password Verification**: Client-side verification enforcing password confirmation matching prior to submission.
- **Role-Based Registration**: New registrations can choose between **Customer** and **Admin (Seller)** roles.
- **Approval Lifecycle**: Customer accounts are automatically provisioned with `AccountStatus = 'Active'`; Admin/Seller registrations are assigned `AccountStatus = 'Pending'` awaiting Super Admin verification.
- **Access Control & Blocking**: Login verification strictly blocks accounts with `AccountStatus = 'Pending'` or `'Suspended'` / `IsActive = false`.
- **Global Session State**: Thread-safe global session context (`Session.cs`) tracking `UserId`, `FullName`, `Email`, `Role`, and profile metadata throughout the application life-cycle.
- **Role-Based Routing**: Post-login redirection routes users directly to their dedicated dashboard: `CustomerDashboard`, `AdminDashboard`, or `SuperAdminDashboard`.

### Customer Features
- **Property Catalog Browsing**: View all verified, available properties with pricing, location, dimensions, and specifications.
- **Multi-Parameter Search & Filtering**:
  - Keyword search across Title, Location, and Address.
  - Listing Type filtering (`All`, `Sale`, `Rent`).
  - Category filtering (`All`, `Apartment`, `House`, `Commercial`, `Land`).
  - District filtering (dynamically populated from available listings).
  - Price range brackets (under 50k, 50k–100k, 100k–500k, 500k–1M, 1M–5M, 5M–10M, 10M+).
  - Bedroom count filtering (`All`, `1+`, `2+`, `3+`, `4+`, `5+`).
- **Live Search Counter**: Status display indicating the number of properties matched by current filter criteria.
- **Visit Scheduling**: Interactive modal (`ScheduleVisitForm`) allowing customers to select visit dates (tomorrow onwards), preferred time slots, and submit custom notes.
- **Visit Cancellation**: Customers can cancel pending visit requests directly from their "My Visits" tab.
- **Cart Management**: Add sale or rental properties to the shopping cart; specify rental lease duration in months with instant total price re-computation.
- **Checkout & Multi-Payment**: Complete transactional checkout supporting multiple payment methods (`Cash`, `Card`, `Bkash`, `Nagad`, `Bank Transfer`) wrapped in atomic database transactions.
- **Automatic Inventory Status Updates**: Properties purchased or rented are immediately updated to `Sold` or `Rented` and removed from the active marketplace.
- **Order & Invoice History**: View historical purchase records and launch detailed invoices.
- **Printable Invoices**: Dedicated modal (`InvoiceForm`) with built-in `PrintDocument` support for hardcopy printing and PDF generation.
- **Property Reviews & Ratings**: Submit 1-to-5 star ratings and descriptive feedback for completed transactions.
- **Profile Management**: View and update full name, contact phone, mailing address, and profile picture.

### Seller / Admin Features
- **Property Listing Creation**: Add new properties via `AddPropertyForm` specifying category, title, listing type, district, area location, address, square footage, bedrooms, bathrooms, price, and description.
- **Image Upload & Preview**: Upload and preview primary property images with automatic file-system persistence into the `PropertyImages/` directory.
- **Property Editing**: Full modification capabilities for existing listings owned by the logged-in seller.
- **Property Status Toggle**: Quickly mark properties between Available, Reserved, Sold, or Rented.
- **Visit Request Management**: View incoming inspection visit requests from customers; accept or reject requests with real-time grid refresh.
- **Sales & Revenue Tracking**: Track all closed transactions, buyers, payment methods, and net revenue.

### Super Admin Features
- **User Management & Approval**: Review all registered accounts across the platform.
- **One-Click Approval / Rejection**: Approve pending seller accounts (`AccountStatus = 'Active'`) or reject unverified applicants (`AccountStatus = 'Suspended'`).
- **Account Suspension & Activation**: Toggle active/suspended states for existing users.
- **Pending Approvals Badge**: Real-time counter displaying the volume of pending seller applications requiring review.
- **User Grid Filtering**: Filter user list by Role (`All`, `Customer`, `Admin`, `SuperAdmin`) and Account Status (`All`, `Pending`, `Active`, `Suspended`).
- **Self-Protection Logic**: Super Admin accounts cannot be suspended, rejected, or deleted by the Super Admin themselves.
- **Platform Property Removal**: Remove fraudulent or obsolete property listings across the entire marketplace.
- **Marketplace & Revenue Reporting**: Global reporting displaying aggregate marketplace volume and the platform's 5% commission revenue.

---

## Application Workflow

### Customer Journey
```text
Register (Customer) ──> Login ──> Browse / Search / Filter Listings
                                       │
                ┌──────────────────────┴──────────────────────┐
                ▼                                             ▼
        Schedule Site Visit                            Add to Cart
                │                                             │
        Manage Visits (Cancel)                         Specify Rental Months
                                                              │
                                                        Select Payment Method
                                                              │
                                                        Execute Checkout
                                                              │
                                                        Generate & Print Invoice
                                                              │
                                                        Submit Review & Rating
```

### Seller / Admin Journey
```text
Register (Admin) ──> Await Super Admin Approval ──> Login
                                                        │
                      ┌─────────────────────────────────┴─────────────────────────────────┐
                      ▼                                                                   ▼
           Add Property & Upload Images                                        Manage Visit Requests
                      │                                                                   │
           Edit / Update Details                                               Approve or Reject Visits
                      │                                                                   │
           Track Sales & Revenue                                               Monitor Property Inquiries
```

### Super Admin Journey
```text
Login ──> Super Admin Dashboard
             │
             ├──> Review Pending Approvals ──> Approve / Reject Sellers
             ├──> User Management ──> Filter by Role/Status ──> Suspend / Activate
             ├──> Platform Listings ──> Delete Inappropriate Properties
             └──> Financial Oversight ──> Track Volume & 5% Platform Commission
```

---

## Platform Revenue Model

EstateNexus operates on a verified **5% platform commission model** applied automatically to all completed transactions.

### Commission Calculation Formulas
For every order placed by a customer during checkout, a corresponding `Commission` record is created in the database:

$$\text{Order Total} = \sum (\text{Property Price} \times \text{Rental Months or Unit Quantity})$$

$$\text{Platform Commission (5\%)} = \text{Order Total} \times 0.05$$

$$\text{Owner / Seller Payout} = \text{Order Total} - \text{Commission Amount}$$

### Database Representation
- Each `Order` generates exactly one `Commission` record with `CommissionRate = 5.00`.
- The `Invoices` table stores both `SubTotal`, `CommissionAmount`, and `TotalAmount` for transparent accounting.

---

## Installation & Setup

### 1. Prerequisites
- Microsoft Windows 10 or Windows 11 (64-bit)
- [.NET 10.0 SDK](https://dotnet.microsoft.com/)
- [Microsoft SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/) (or SQL Server Express / LocalDB)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with the **.NET Desktop Development** workload installed

### 2. Clone the Repository
```bash
git clone https://github.com/rhqshipon/Estate_Nexus_Dhaka.git
cd Estate_Nexus_Dhaka
```

### 3. Configure Database Connection String
Open `EstateNexus/App.config` and configure your local SQL Server instance name:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <connectionStrings>
        <add name="EstateNexusDB"
             connectionString="Data Source=.\MSSQLSERVER01;Initial Catalog=EstateNexusDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;"
             providerName="Microsoft.Data.SqlClient" />
    </connectionStrings>
</configuration>
```

> **Note**: If your local SQL Server instance is the default instance, set `Data Source=localhost` or `Data Source=.`. If using SQL Server Express, use `Data Source=.\SQLEXPRESS`.

### 4. Automatic Database Initialization & Seeding
You **do not** need to manually execute SQL scripts before running. 

When the application launches, `Program.cs` executes `DatabaseSetup.InitializeDatabase()`, which will:
1. Connect to SQL Server `master` and create `EstateNexusDB` if it does not exist.
2. Create all 19 relational tables matching the official ER schema.
3. Migrate and align column definitions and constraints.
4. Insert baseline seed data for roles, admin/seller/customer accounts, property categories, features, and sample properties.

### 5. Build and Run
#### Via Command Line:
```bash
dotnet restore
dotnet build EstateNexus.sln
dotnet run --project EstateNexus/EstateNexus.csproj
```

#### Via Visual Studio:
1. Double-click `EstateNexus.sln` to open the solution in Visual Studio.
2. Ensure `EstateNexus` is set as the Startup Project.
3. Press `F5` or click **Start Debugging**.

---

## Default Seeded Accounts

The database initialization automatically creates the following default accounts:

| Role | Email / Username | Password | Default Status |
| :--- | :--- | :--- | :--- |
| **Super Admin** | `admin@estatenexus.com` | `admin123` | Active |
| **Admin (Seller)** | `seller@estatenexus.com` | `seller123` | Active |
| **Customer** | `customer@estatenexus.com` | `customer123` | Active |

*(All passwords are automatically hashed with SHA-256 in the database).*

---

## Project Structure

```text
Estate_Nexus_Dhaka/
├── .gitignore                          # Visual Studio & .NET Git ignore rules
├── EstateNexus.sln                     # Visual Studio Solution File
├── EstateNexusDB_Schema.sql            # Master reference SQL schema script
├── database/                           # Database documentation and scripts
│   ├── README.md                       # Database script guide
│   └── schema.sql                      # Complete 3NF T-SQL schema & seed script
├── docs/                               # Project documentation
│   └── screenshots/                    # Application UI screenshot placeholders
│       └── .gitkeep
└── EstateNexus/                        # Main Windows Forms Project
    ├── App.config                      # Database connection string configuration
    ├── EstateNexus.csproj              # Project dependencies and target framework (.NET 10)
    ├── Program.cs                      # Application entry point and database initializer
    ├── DatabaseSetup.cs                # Automated database creation, migration & seed logic
    ├── PasswordHelper.cs               # SHA-256 cryptographic hashing and verification
    ├── Session.cs                      # Global session state manager
    ├── Data/
    │   └── EstateNexusDbContext.cs     # Entity Framework Core DbContext (19 DbSets)
    ├── Models/
    │   └── Entities/                   # Strongly typed EF Core entity models
    │       ├── Role.cs
    │       ├── User.cs
    │       ├── PropertyCategory.cs
    │       ├── Property.cs
    │       ├── PropertyImage.cs
    │       ├── Offer.cs
    │       ├── PropertyFeature.cs
    │       ├── PropertyFeatureMapping.cs
    │       ├── FeaturedListing.cs
    │       ├── Cart.cs
    │       ├── CartItem.cs
    │       ├── Order.cs
    │       ├── OrderItem.cs
    │       ├── Payment.cs
    │       ├── Invoice.cs
    │       ├── Commission.cs
    │       ├── Complaint.cs
    │       ├── VisitRequest.cs
    │       └── Review.cs
    ├── Migrations/                     # Entity Framework Core migration history
    ├── UI/
    │   └── Theme.cs                    # Centralized UI color palette and typography
    ├── LoginForm.cs                    # Authentication form with show/hide password & validation
    ├── RegistrationForm.cs             # User registration form with role selection & validation
    ├── CustomerDashboard.cs            # Customer operations (browse, filter, cart, checkout, visits)
    ├── AdminDashboard.cs               # Seller operations (add/edit listing, visit approvals, sales)
    ├── SuperAdminDashboard.cs          # Super Admin operations (user approvals, suspension, revenue)
    ├── AddPropertyForm.cs              # Property creation and edit modal with image upload
    ├── ScheduleVisitForm.cs            # Customer visit booking dialog
    └── InvoiceForm.cs                  # Printable invoice modal with PrintDocument support
```

---

## Main Forms

| Form Class | Purpose |
| :--- | :--- |
| [`LoginForm.cs`](file:///EstateNexus/LoginForm.cs) | Handles authentication by email/username with SHA-256 verification, plain-text password upgrade, show password toggle, clear button, and role-based routing. |
| [`RegistrationForm.cs`](file:///EstateNexus/RegistrationForm.cs) | Allows new users to register as Customers (auto-activated) or Admins/Sellers (pending approval) with confirmation checks and validation. |
| [`CustomerDashboard.cs`](file:///EstateNexus/CustomerDashboard.cs) | Multi-tab dashboard for property search, filtering, visit scheduling, cart manipulation, checkout, order tracking, reviews, and profile management. |
| [`AdminDashboard.cs`](file:///EstateNexus/AdminDashboard.cs) | Seller workspace for managing properties, editing listing specifications, reviewing visit requests, and inspecting sales records. |
| [`SuperAdminDashboard.cs`](file:///EstateNexus/SuperAdminDashboard.cs) | Administrative control panel for approving/rejecting seller accounts, toggling user statuses, filtering users, removing properties, and auditing revenue. |
| [`AddPropertyForm.cs`](file:///EstateNexus/AddPropertyForm.cs) | Dialog for creating new property listings or editing existing listings, including image file upload and location parsing. |
| [`ScheduleVisitForm.cs`](file:///EstateNexus/ScheduleVisitForm.cs) | Modal dialog for customers to select visit dates, time slots, and submit visit notes for a specific property. |
| [`InvoiceForm.cs`](file:///EstateNexus/InvoiceForm.cs) | Displays formal order invoices with item breakdowns, payment details, commission notes, and native hardcopy printing support. |

---

## Screenshots

Screenshots can be added in the following locations:

- `docs/screenshots/login.png`
- `docs/screenshots/customer-dashboard.png`
- `docs/screenshots/admin-dashboard.png`
- `docs/screenshots/superadmin-dashboard.png`
- `docs/screenshots/invoice.png`

---

## Known Future Enhancements

The following features have database entity models and tables provisioned in the ER schema, but are intentionally designated as future enhancements outside the current core scope:

1. **Complaints Workflow**: Customer complaint submission and dispute resolution panel (`Complaints` table).
2. **Offers / Negotiation Workflow**: Dynamic seller discount creation and direct price counter-offers (`Offers` table).
3. **Featured Listings Promotion**: Paid listing promotions and premium visibility placement (`FeaturedListings` table).
4. **Advanced Property Feature Matrix**: Dynamic multi-feature tagging and filtering (`PropertyFeatures` & `PropertyFeatureMappings` tables).
5. **Automated Password Reset**: Self-service email verification tokens for forgotten passwords.