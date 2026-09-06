# EstateNexus Database Documentation

This directory contains the reference SQL scripts and schema definitions for the **EstateNexus** platform database.

---

## 1. Files in this Directory

### `schema.sql`
- **Purpose**: Complete T-SQL schema recreation and baseline seeding script.
- **Engine**: Microsoft SQL Server (2019+).
- **Architecture**: Normalized to Third Normal Form (3NF), defining all 19 database tables, primary keys, foreign key constraints, default constraints, and seed data for initial roles, categories, features, sample users, and sample properties.
- **Tables Defined**:
  1. `Roles`
  2. `Users`
  3. `PropertyCategories`
  4. `Properties`
  5. `PropertyImages`
  6. `Offers`
  7. `PropertyFeatures`
  8. `PropertyFeatureMappings`
  9. `FeaturedListings`
  10. `Carts`
  11. `CartItems`
  12. `Orders`
  13. `OrderItems`
  14. `Payments`
  15. `Invoices`
  16. `Commissions`
  17. `Complaints`
  18. `VisitRequests`
  19. `Reviews`

---

## 2. Note on `superadmin Functionality.sql`

- The `superadmin Functionality.sql` script (if maintained in the repository root or historical reference branches) represents an original legacy/reference SQL script designed for manual Super Admin operations, user status verifications, and reporting.
- To preserve repository integrity and historical references, any existing instances of `superadmin Functionality.sql` remain completely untouched in their original locations.

---

## 3. Automatic Initialization

When running the application, manual execution of these SQL scripts is **optional**. The application automatically initializes the database schema, verifies constraints, runs safe migrations, and inserts default seeded records via `EstateNexus.DatabaseSetup.InitializeDatabase()` on first launch.
