-- ============================================================================
-- Script: EstateNexusDB_Schema.sql
-- Description: Complete T-SQL Schema script to recreate EstateNexusDB
--              strictly conforming to the official EstateNexus ER Diagram.
-- Database Engine: Microsoft SQL Server 2019+
-- Normalized to: Third Normal Form (3NF)
-- ============================================================================

USE master;
GO

-- 1. Recreate Database
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'EstateNexusDB')
BEGIN
    ALTER DATABASE EstateNexusDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EstateNexusDB;
END
GO

CREATE DATABASE EstateNexusDB;
GO

USE EstateNexusDB;
GO

-- ============================================================================
-- 2. CREATE TABLES (in dependency order)
-- ============================================================================

-- Table 1: Roles
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    RoleDescription NVARCHAR(255) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Table 2: Users
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    RoleId INT NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Username NVARCHAR(50) NULL,
    Phone NVARCHAR(20) NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    Address NVARCHAR(255) NULL,
    ProfileImagePath NVARCHAR(500) NULL,
    AccountStatus NVARCHAR(20) NOT NULL DEFAULT 'Active',
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) 
        REFERENCES Roles(RoleId) ON DELETE NO ACTION
);
GO

-- Table 3: PropertyCategories
CREATE TABLE PropertyCategories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(255) NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- Table 4: Properties
CREATE TABLE Properties (
    PropertyId INT IDENTITY(1,1) PRIMARY KEY,
    OwnerId INT NOT NULL,
    CategoryId INT NOT NULL,
    PropertyTitle NVARCHAR(150) NOT NULL,
    ListingType NVARCHAR(20) NOT NULL, -- 'Rent', 'Sale'
    District NVARCHAR(100) NOT NULL,
    AreaLocation NVARCHAR(100) NOT NULL,
    FullAddress NVARCHAR(255) NOT NULL,
    AreaSize DECIMAL(10,2) NOT NULL,
    AreaUnit NVARCHAR(20) NOT NULL DEFAULT 'sqft',
    Bedrooms INT NOT NULL DEFAULT 0,
    Bathrooms INT NOT NULL DEFAULT 0,
    Price DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    PropertyStatus NVARCHAR(20) NOT NULL DEFAULT 'Available', -- 'Available', 'Reserved', 'Sold', 'Rented'
    ApprovalStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending',   -- 'Pending', 'Approved', 'Rejected'
    IsFeatured BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    CONSTRAINT FK_Properties_Users FOREIGN KEY (OwnerId) 
        REFERENCES Users(UserId) ON DELETE NO ACTION,
    CONSTRAINT FK_Properties_Categories FOREIGN KEY (CategoryId) 
        REFERENCES PropertyCategories(CategoryId) ON DELETE NO ACTION
);
GO

-- Table 5: PropertyImages
CREATE TABLE PropertyImages (
    ImageId INT IDENTITY(1,1) PRIMARY KEY,
    PropertyId INT NOT NULL,
    ImagePath NVARCHAR(500) NOT NULL,
    IsPrimary BIT NOT NULL DEFAULT 0,
    UploadedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_PropertyImages_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE CASCADE
);
GO

-- Table 6: Offers
CREATE TABLE Offers (
    OfferId INT IDENTITY(1,1) PRIMARY KEY,
    PropertyId INT NOT NULL,
    DiscountType NVARCHAR(50) NOT NULL, -- 'Percentage', 'Flat'
    DiscountValue DECIMAL(18,2) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Offers_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE CASCADE
);
GO

-- Table 7: PropertyFeatures
CREATE TABLE PropertyFeatures (
    FeatureId INT IDENTITY(1,1) PRIMARY KEY,
    FeatureName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255) NULL
);
GO

-- Table 8: PropertyFeatureMappings (Composite Primary Key)
CREATE TABLE PropertyFeatureMappings (
    PropertyId INT NOT NULL,
    FeatureId INT NOT NULL,
    CONSTRAINT PK_PropertyFeatureMappings PRIMARY KEY (PropertyId, FeatureId),
    CONSTRAINT FK_PropertyFeatureMappings_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE CASCADE,
    CONSTRAINT FK_PropertyFeatureMappings_Features FOREIGN KEY (FeatureId) 
        REFERENCES PropertyFeatures(FeatureId) ON DELETE CASCADE
);
GO

-- Table 9: FeaturedListings
CREATE TABLE FeaturedListings (
    FeaturedListingId INT IDENTITY(1,1) PRIMARY KEY,
    PropertyId INT NOT NULL,
    FeaturedFee DECIMAL(18,2) NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Paid'
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active',         -- 'Active', 'Expired', 'Cancelled'
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_FeaturedListings_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE CASCADE
);
GO

-- Table 10: Carts
CREATE TABLE Carts (
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Carts_Users FOREIGN KEY (CustomerId) 
        REFERENCES Users(UserId) ON DELETE CASCADE
);
GO

-- Table 11: CartItems
CREATE TABLE CartItems (
    CartItemId INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT NOT NULL,
    PropertyId INT NOT NULL,
    RentalMonths INT NOT NULL DEFAULT 1,
    OfferedPrice DECIMAL(18,2) NULL,
    AddedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_CartItems_Carts FOREIGN KEY (CartId) 
        REFERENCES Carts(CartId) ON DELETE CASCADE,
    CONSTRAINT FK_CartItems_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE NO ACTION
);
GO

-- Table 12: Orders
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    OrderStatus NVARCHAR(50) NOT NULL DEFAULT 'Completed', -- 'Pending', 'Processing', 'Completed', 'Cancelled'
    TransactionType NVARCHAR(50) NOT NULL DEFAULT 'Sale',  -- 'Sale', 'Rent'
    CONSTRAINT FK_Orders_Users FOREIGN KEY (CustomerId) 
        REFERENCES Users(UserId) ON DELETE NO ACTION
);
GO

-- Table 13: OrderItems
CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    PropertyId INT NOT NULL,
    OwnerId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    RentalMonths INT NOT NULL DEFAULT 0,
    UnitPrice DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    FinalAmount DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) 
        REFERENCES Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE NO ACTION,
    CONSTRAINT FK_OrderItems_Users FOREIGN KEY (OwnerId) 
        REFERENCES Users(UserId) ON DELETE NO ACTION
);
GO

-- Table 14: Payments
CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL, -- 'Card', 'Bkash', 'Nagad', 'Bank Transfer', 'Cash'
    TransactionId NVARCHAR(100) NOT NULL,
    PaymentAmount DECIMAL(18,2) NOT NULL,
    PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Completed', -- 'Pending', 'Completed', 'Failed', 'Refunded'
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderId) 
        REFERENCES Orders(OrderId) ON DELETE CASCADE
);
GO

-- Table 15: Invoices
CREATE TABLE Invoices (
    InvoiceId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    PaymentId INT NOT NULL,
    InvoiceNumber NVARCHAR(50) NOT NULL UNIQUE,
    SubTotal DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    CommissionAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    TotalAmount DECIMAL(18,2) NOT NULL,
    GeneratedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Invoices_Orders FOREIGN KEY (OrderId) 
        REFERENCES Orders(OrderId) ON DELETE NO ACTION,
    CONSTRAINT FK_Invoices_Payments FOREIGN KEY (PaymentId) 
        REFERENCES Payments(PaymentId) ON DELETE NO ACTION
);
GO

-- Table 16: Commissions
CREATE TABLE Commissions (
    CommissionId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    CommissionRate DECIMAL(5,2) NOT NULL, -- Percentage e.g. 5.00
    TransactionAmount DECIMAL(18,2) NOT NULL,
    CommissionAmount DECIMAL(18,2) NOT NULL,
    OwnerAmount DECIMAL(18,2) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Commissions_Orders FOREIGN KEY (OrderId) 
        REFERENCES Orders(OrderId) ON DELETE CASCADE
);
GO

-- Table 17: Complaints
CREATE TABLE Complaints (
    ComplaintId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    PropertyId INT NULL,
    Subject NVARCHAR(200) NOT NULL,
    ComplaintType NVARCHAR(50) NOT NULL, -- 'Property Issue', 'Seller Behavior', 'Payment Issue', 'Other'
    Description NVARCHAR(MAX) NOT NULL,
    Priority NVARCHAR(20) NOT NULL DEFAULT 'Normal', -- 'Low', 'Normal', 'High', 'Urgent'
    ComplaintStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- 'Pending', 'In Review', 'Resolved', 'Dismissed'
    ResolvedBy INT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    ResolvedDate DATETIME NULL,
    CONSTRAINT FK_Complaints_Customer FOREIGN KEY (CustomerId) 
        REFERENCES Users(UserId) ON DELETE NO ACTION,
    CONSTRAINT FK_Complaints_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE NO ACTION,
    CONSTRAINT FK_Complaints_ResolvedBy FOREIGN KEY (ResolvedBy) 
        REFERENCES Users(UserId) ON DELETE NO ACTION
);
GO

-- Table 18: VisitRequests
CREATE TABLE VisitRequests (
    VisitRequestId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    PropertyId INT NOT NULL,
    VisitDate DATE NOT NULL,
    VisitTime NVARCHAR(20) NULL,
    RequestStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- 'Pending', 'Approved', 'Rejected', 'Completed'
    CustomerNote NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_VisitRequests_Users FOREIGN KEY (CustomerId) 
        REFERENCES Users(UserId) ON DELETE NO ACTION,
    CONSTRAINT FK_VisitRequests_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE CASCADE
);
GO

-- Table 19: Reviews
CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    PropertyId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    ReviewComment NVARCHAR(MAX) NULL,
    ReviewStatus NVARCHAR(50) NOT NULL DEFAULT 'Approved', -- 'Pending', 'Approved', 'Rejected'
    ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Users FOREIGN KEY (CustomerId) 
        REFERENCES Users(UserId) ON DELETE NO ACTION,
    CONSTRAINT FK_Reviews_Properties FOREIGN KEY (PropertyId) 
        REFERENCES Properties(PropertyId) ON DELETE CASCADE
);
GO

-- ============================================================================
-- 3. SEED INITIAL DATA (Guaranteed >= 3 rows per table, seed accounts: 1 Super Admin, 2 Shop Owners, 3 Customers)
-- ============================================================================

-- Table 1: Roles (3 rows)
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (RoleId, RoleName, RoleDescription, CreatedDate) VALUES
(1, 'Customer', 'Can browse properties, make reservations, request visits, write reviews, and submit complaints.', GETDATE()),
(2, 'Admin', 'Property seller/owner who can list properties, manage inventory, handle orders, and manage visits.', GETDATE()),
(3, 'SuperAdmin', 'System administrator with full access to manage users, categories, approvals, and platform revenue.', GETDATE());
SET IDENTITY_INSERT Roles OFF;
GO

-- Table 2: Users (6 rows: 1 Super Admin, 2 Shop Owners, 3 Customers)
-- Passwords (SHA-256):
-- admin123    -> 240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9
-- seller123   -> 2a76110d06bcc4fd437337b984131cfa82db9f792e3e2340acef9f3066b264e0
-- customer123 -> b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (UserId, RoleId, FullName, Email, Username, Phone, PasswordHash, Address, ProfileImagePath, AccountStatus, IsActive, CreatedDate) VALUES
-- 1 Super Admin
(1, 3, 'Super Admin', 'admin@estatenexus.com', 'admin', '01700000000', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'EstateNexus HQ, Kuril, Dhaka', NULL, 'Active', 1, GETDATE()),
-- 2 Shop Owners (Admins / Sellers)
(2, 2, 'Rahim Real Estate', 'seller@estatenexus.com', 'seller', '01711111111', '2a76110d06bcc4fd437337b984131cfa82db9f792e3e2340acef9f3066b264e0', 'Gulshan-2, Dhaka', NULL, 'Active', 1, GETDATE()),
(3, 2, 'Karim Properties Ltd', 'seller2@estatenexus.com', 'seller2', '01712222222', '2a76110d06bcc4fd437337b984131cfa82db9f792e3e2340acef9f3066b264e0', 'Banani, Dhaka', NULL, 'Active', 1, GETDATE()),
-- 3 Customers
(4, 1, 'John Customer', 'customer@estatenexus.com', 'customer', '01722222222', 'b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6', 'Dhanmondi, Dhaka', NULL, 'Active', 1, GETDATE()),
(5, 1, 'Sarah Jenkins', 'customer2@estatenexus.com', 'customer2', '01733333333', 'b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6', 'Uttara, Dhaka', NULL, 'Active', 1, GETDATE()),
(6, 1, 'Tanvir Ahmed', 'customer3@estatenexus.com', 'customer3', '01744444444', 'b041c0aeb35bb0fa4aa668ca5a920b590196fdaf9a00eb852c9b7f4d123cc6d6', 'Bashundhara, Dhaka', NULL, 'Active', 1, GETDATE());
SET IDENTITY_INSERT Users OFF;
GO

-- Table 3: PropertyCategories (4 rows)
SET IDENTITY_INSERT PropertyCategories ON;
INSERT INTO PropertyCategories (CategoryId, CategoryName, Description, IsActive) VALUES
(1, 'Apartment', 'Residential flats, luxury condominiums, and multi-family units', 1),
(2, 'House', 'Independent residential houses, villas, and duplex homes', 1),
(3, 'Commercial', 'Offices, commercial spaces, retail shops, and warehouses', 1),
(4, 'Land', 'Residential plots, commercial lands, and open agricultural plots', 1);
SET IDENTITY_INSERT PropertyCategories OFF;
GO

-- Table 4: Properties (17 rows)
SET IDENTITY_INSERT Properties ON;
INSERT INTO Properties (PropertyId, OwnerId, CategoryId, PropertyTitle, ListingType, District, AreaLocation, FullAddress, AreaSize, AreaUnit, Bedrooms, Bathrooms, Price, Description, PropertyStatus, ApprovalStatus, IsFeatured, CreatedDate) VALUES
(1, 2, 1, 'Luxury 3-BHK Apartment in Gulshan', 'Sale', 'Dhaka', 'Gulshan', 'Road 11, Block D, Gulshan-2, Dhaka', 2200.00, 'sqft', 3, 3, 25000000.00, 'Stunning luxury apartment with serene lake view, imported fittings, and round-the-clock security.', 'Available', 'Approved', 1, GETDATE()),
(2, 2, 2, 'Modern Duplex Villa in Banani', 'Rent', 'Dhaka', 'Banani', 'Road 7, Block F, Banani, Dhaka', 3500.00, 'sqft', 4, 4, 120000.00, 'Spacious modern duplex villa with private landscaped garden, rooftop terrace, and separate servant room.', 'Available', 'Approved', 1, GETDATE()),
(3, 3, 3, 'Prime Commercial Office Space', 'Rent', 'Dhaka', 'Dhanmondi', 'Satmasjid Road, Dhanmondi, Dhaka', 1800.00, 'sqft', 0, 2, 85000.00, 'Ready commercial space optimal for software firms, corporate banks, or regional head offices.', 'Available', 'Approved', 0, GETDATE()),
(4, 3, 4, 'Residential Plot in Purbachal', 'Sale', 'Dhaka', 'Purbachal', 'Sector 4, Road 202, Purbachal New Town', 3600.00, 'sqft', 0, 0, 9500000.00, 'Prime south-facing 5 katha residential plot ready for immediate architectural construction.', 'Available', 'Approved', 0, GETDATE()),
(5, 2, 1, 'Cozy 2-BHK Flat in Uttara', 'Rent', 'Dhaka', 'Uttara', 'Sector 3, Road 14, Uttara, Dhaka', 1250.00, 'sqft', 2, 2, 35000.00, 'Bright and airy flat located within walking distance of airport and metro station.', 'Available', 'Approved', 0, GETDATE()),
(6, 2, 1, 'Executive 3-BHK Furnished Apartment in Bashundhara', 'Rent', 'Dhaka', 'Bashundhara R/A', 'Block C, Road 5, Bashundhara R/A, Dhaka', 1850.00, 'sqft', 3, 3, 55000.00, 'Fully furnished 3-bedroom apartment with modern interior, imported fixtures, lake view, and 24/7 security.', 'Available', 'Approved', 1, GETDATE()),
(7, 2, 1, 'Brand New 4-BHK Luxury Flat in Mirpur DOHS', 'Sale', 'Dhaka', 'Mirpur DOHS', 'Avenue 3, Road 8, Mirpur DOHS, Dhaka', 2400.00, 'sqft', 4, 4, 18500000.00, 'Spacious south-facing family apartment in serene and secure cantonment environment with double car parking.', 'Available', 'Approved', 1, GETDATE()),
(8, 3, 2, 'Exclusive Triplex Luxury Villa in Baridhara', 'Sale', 'Dhaka', 'Baridhara', 'Road 2, Park Way, Baridhara Diplomatic Zone, Dhaka', 5200.00, 'sqft', 5, 6, 85000000.00, 'Architectural masterpiece triplex villa featuring private indoor heated swimming pool, landscaped lawn, high-tech security, and premium Italian marble finishing.', 'Available', 'Approved', 1, GETDATE()),
(9, 3, 2, 'Spacious Colonial Style Duplex House in Dhanmondi', 'Rent', 'Dhaka', 'Dhanmondi', 'Road 9/A, Dhanmondi Residential Area, Dhaka', 4000.00, 'sqft', 4, 5, 175000.00, 'Quiet and elegant duplex home with private driveway, lush green front garden, and rooftop BBQ zone.', 'Available', 'Approved', 0, GETDATE()),
(10, 3, 3, 'Corporate Floor Space in Motijheel Financial Hub', 'Sale', 'Dhaka', 'Motijheel', 'Dilkusha Commercial Area, Motijheel, Dhaka', 3200.00, 'sqft', 0, 4, 42000000.00, 'Prime full corporate commercial floor suitable for multinational bank, financial institution, or corporate headquarters.', 'Available', 'Approved', 0, GETDATE()),
(11, 2, 3, 'Premium Retail Showroom on Gulshan Avenue', 'Rent', 'Dhaka', 'Gulshan', 'Gulshan Avenue, Gulshan-1, Dhaka', 2200.00, 'sqft', 0, 2, 210000.00, 'High-footfall ground floor commercial glass showroom facing main Gulshan Avenue with grand frontage and customer parking.', 'Available', 'Approved', 1, GETDATE()),
(12, 3, 4, 'South-Facing 5 Katha Residential Plot in Bashundhara', 'Sale', 'Dhaka', 'Bashundhara R/A', 'Plot 340, Road 12, Block I, Bashundhara R/A, Dhaka', 3600.00, 'sqft', 0, 0, 14500000.00, 'Demarcated ready plot with Rajuk approved layout, wide 40ft road frontage, and immediate registration.', 'Available', 'Approved', 0, GETDATE()),
(13, 2, 4, 'Prime 10 Katha Corner Commercial Plot in Uttara', 'Sale', 'Dhaka', 'Uttara', 'Sector 18, Road 204, Uttara 3rd Phase, Dhaka', 7200.00, 'sqft', 0, 0, 38000000.00, 'Corner commercial plot beside metro rail depot, ideal for commercial complex, private hospital, or international school.', 'Available', 'Approved', 1, GETDATE()),
(14, 2, 1, 'Modern 3-BHK Flat in Peaceful Mohakhali DOHS', 'Rent', 'Dhaka', 'Mohakhali DOHS', 'Road 14, Mohakhali DOHS, Dhaka', 1650.00, 'sqft', 3, 3, 48000.00, 'Bright and cross-ventilated 3-bedroom flat on middle floor, strictly family residential community.', 'Available', 'Approved', 0, GETDATE()),
(15, 3, 1, 'Sea-Breeze Luxury 3-BHK Apartment in Nasirabad', 'Sale', 'Chittagong', 'Nasirabad', 'Nasirabad Housing Society, Road 3, Chittagong', 2050.00, 'sqft', 3, 3, 16000000.00, 'Panoramic hill and city view luxury apartment in premier residential neighborhood of Chittagong.', 'Available', 'Approved', 1, GETDATE()),
(16, 3, 2, 'Charming 4-BHK Independent House in Sylhet', 'Rent', 'Sylhet', 'Shahjalal Uposhohor', 'Block D, Main Road, Shahjalal Uposhohor, Sylhet', 2800.00, 'sqft', 4, 4, 40000.00, 'Independent double-storey home with fruit garden, front courtyard, and peaceful environment.', 'Available', 'Approved', 0, GETDATE()),
(17, 2, 1, 'Affordable 2-BHK Family Flat in Khilgaon', 'Rent', 'Dhaka', 'Khilgaon', 'Taltola City Corporation Road, Khilgaon, Dhaka', 1050.00, 'sqft', 2, 2, 22000.00, 'Budget-friendly family flat close to market, schools, and transit facilities with continuous utility supply.', 'Available', 'Approved', 0, GETDATE());
SET IDENTITY_INSERT Properties OFF;
GO

-- Table 5: PropertyImages (3 rows)
SET IDENTITY_INSERT PropertyImages ON;
INSERT INTO PropertyImages (ImageId, PropertyId, ImagePath, IsPrimary, UploadedDate) VALUES
(1, 1, 'images/properties/prop1_main.jpg', 1, GETDATE()),
(2, 2, 'images/properties/prop2_main.jpg', 1, GETDATE()),
(3, 3, 'images/properties/prop3_main.jpg', 1, GETDATE());
SET IDENTITY_INSERT PropertyImages OFF;
GO

-- Table 6: Offers (5 rows)
INSERT INTO Offers (PropertyId, DiscountType, DiscountValue, StartDate, EndDate, IsActive, CreatedDate) VALUES
(1, 'Percentage', 5.00, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, 25, GETDATE()), 1, GETDATE()),
(2, 'FixedAmount', 5000.00, DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, 15, GETDATE()), 1, GETDATE()),
(6, 'FixedAmount', 3000.00, DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, 20, GETDATE()), 1, GETDATE()),
(7, 'Percentage', 5.00, DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, 30, GETDATE()), 1, GETDATE()),
(11, 'Percentage', 8.00, DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, 15, GETDATE()), 1, GETDATE());
GO

-- Table 7: PropertyFeatures (6 rows)
SET IDENTITY_INSERT PropertyFeatures ON;
INSERT INTO PropertyFeatures (FeatureId, FeatureName, Description) VALUES
(1, 'Swimming Pool', 'Private or shared luxury swimming pool facility'),
(2, 'Elevator / Lift', 'High-speed passenger elevator in the building'),
(3, 'Car Parking', 'Dedicated covered parking space'),
(4, '24/7 Security & CCTV', 'Gated security guard surveillance 24 hours'),
(5, 'Backup Generator', 'Full electricity backup for apartments and common areas'),
(6, 'Balcony / Terrace', 'Spacious open view balcony or private terrace');
SET IDENTITY_INSERT PropertyFeatures OFF;
GO

-- Table 8: PropertyFeatureMappings (16 rows)
INSERT INTO PropertyFeatureMappings (PropertyId, FeatureId) VALUES
(1, 2), (1, 3), (1, 4), (1, 5), (1, 6),
(2, 1), (2, 3), (2, 4), (2, 6),
(3, 2), (3, 3), (3, 4), (3, 5),
(5, 2), (5, 4), (5, 6),
(6, 2), (6, 3), (6, 4), (6, 5), (6, 6),
(7, 2), (7, 3), (7, 4), (7, 5), (7, 6),
(8, 1), (8, 2), (8, 3), (8, 4), (8, 5), (8, 6),
(9, 3), (9, 4), (9, 5), (9, 6),
(10, 2), (10, 3), (10, 4), (10, 5),
(11, 2), (11, 3), (11, 4), (11, 5),
(12, 4),
(13, 4),
(14, 2), (14, 3), (14, 4), (14, 5), (14, 6),
(15, 1), (15, 2), (15, 3), (15, 4), (15, 5), (15, 6),
(16, 3), (16, 4), (16, 6),
(17, 2), (17, 5), (17, 6);
GO

-- Table 9: FeaturedListings (3 rows)
SET IDENTITY_INSERT FeaturedListings ON;
INSERT INTO FeaturedListings (FeaturedListingId, PropertyId, FeaturedFee, StartDate, EndDate, PaymentStatus, Status, CreatedDate) VALUES
(1, 1, 5000.00, DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, 20, GETDATE()), 'Paid', 'Active', GETDATE()),
(2, 6, 3500.00, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, 25, GETDATE()), 'Paid', 'Active', GETDATE()),
(3, 8, 8000.00, DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, 28, GETDATE()), 'Paid', 'Active', GETDATE());
SET IDENTITY_INSERT FeaturedListings OFF;
GO

-- Table 10: Carts (3 rows: for Customers 4, 5, 6)
SET IDENTITY_INSERT Carts ON;
INSERT INTO Carts (CartId, CustomerId, CreatedDate, IsActive) VALUES
(1, 4, GETDATE(), 1),
(2, 5, GETDATE(), 1),
(3, 6, GETDATE(), 1);
SET IDENTITY_INSERT Carts OFF;
GO

-- Table 11: CartItems (3 rows)
SET IDENTITY_INSERT CartItems ON;
INSERT INTO CartItems (CartItemId, CartId, PropertyId, RentalMonths, OfferedPrice, AddedDate) VALUES
(1, 1, 2, 6, 115000.00, GETDATE()),
(2, 2, 5, 12, 35000.00, GETDATE()),
(3, 3, 9, 3, 170000.00, GETDATE());
SET IDENTITY_INSERT CartItems OFF;
GO

-- Table 12: Orders (3 rows)
SET IDENTITY_INSERT Orders ON;
INSERT INTO Orders (OrderId, CustomerId, OrderDate, TotalAmount, OrderStatus, TransactionType) VALUES
(1, 4, DATEADD(DAY, -7, GETDATE()), 720000.00, 'Completed', 'Rent'),
(2, 5, DATEADD(DAY, -4, GETDATE()), 18500000.00, 'Completed', 'Sale'),
(3, 6, DATEADD(DAY, -1, GETDATE()), 255000.00, 'Completed', 'Rent');
SET IDENTITY_INSERT Orders OFF;
GO

-- Table 13: OrderItems (3 rows: Junction table between Orders and Properties)
SET IDENTITY_INSERT OrderItems ON;
INSERT INTO OrderItems (OrderItemId, OrderId, PropertyId, OwnerId, Quantity, RentalMonths, UnitPrice, DiscountAmount, FinalAmount) VALUES
(1, 1, 2, 2, 1, 6, 120000.00, 0.00, 720000.00),
(2, 2, 7, 2, 1, 0, 18500000.00, 0.00, 18500000.00),
(3, 3, 3, 3, 1, 3, 85000.00, 0.00, 255000.00);
SET IDENTITY_INSERT OrderItems OFF;
GO

-- Table 14: Payments (3 rows)
SET IDENTITY_INSERT Payments ON;
INSERT INTO Payments (PaymentId, OrderId, PaymentMethod, TransactionId, PaymentAmount, PaymentStatus, PaymentDate, CreatedDate) VALUES
(1, 1, 'Bkash', 'TRX-BK-9823101', 720000.00, 'Completed', DATEADD(DAY, -7, GETDATE()), GETDATE()),
(2, 2, 'Bank Transfer', 'TRX-BT-5542199', 18500000.00, 'Completed', DATEADD(DAY, -4, GETDATE()), GETDATE()),
(3, 3, 'Card', 'TRX-CC-7712034', 255000.00, 'Completed', DATEADD(DAY, -1, GETDATE()), GETDATE());
SET IDENTITY_INSERT Payments OFF;
GO

-- Table 15: Invoices (3 rows)
SET IDENTITY_INSERT Invoices ON;
INSERT INTO Invoices (InvoiceId, OrderId, PaymentId, InvoiceNumber, SubTotal, DiscountAmount, CommissionAmount, TotalAmount, GeneratedDate) VALUES
(1, 1, 1, 'INV-2026-0001', 720000.00, 0.00, 36000.00, 720000.00, DATEADD(DAY, -7, GETDATE())),
(2, 2, 2, 'INV-2026-0002', 18500000.00, 0.00, 925000.00, 18500000.00, DATEADD(DAY, -4, GETDATE())),
(3, 3, 3, 'INV-2026-0003', 255000.00, 0.00, 12750.00, 255000.00, DATEADD(DAY, -1, GETDATE()));
SET IDENTITY_INSERT Invoices OFF;
GO

-- Table 16: Commissions (3 rows)
SET IDENTITY_INSERT Commissions ON;
INSERT INTO Commissions (CommissionId, OrderId, CommissionRate, TransactionAmount, CommissionAmount, OwnerAmount, CreatedDate) VALUES
(1, 1, 5.00, 720000.00, 36000.00, 684000.00, DATEADD(DAY, -7, GETDATE())),
(2, 2, 5.00, 18500000.00, 925000.00, 17575000.00, DATEADD(DAY, -4, GETDATE())),
(3, 3, 5.00, 255000.00, 12750.00, 242250.00, DATEADD(DAY, -1, GETDATE()));
SET IDENTITY_INSERT Commissions OFF;
GO

-- Table 17: Complaints (3 rows)
SET IDENTITY_INSERT Complaints ON;
INSERT INTO Complaints (ComplaintId, CustomerId, PropertyId, Subject, ComplaintType, Description, Priority, ComplaintStatus, ResolvedBy, CreatedDate, ResolvedDate) VALUES
(1, 4, 2, 'Water pump noise issue', 'Property Issue', 'The water pump makes loud noise late at night.', 'Normal', 'Resolved', 1, DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, -8, GETDATE())),
(2, 5, 7, 'Clarification on elevator maintenance', 'Other', 'Inquiring about monthly service charges for lift.', 'Low', 'Pending', NULL, DATEADD(DAY, -3, GETDATE()), NULL),
(3, 6, 3, 'Delay in receiving physical invoice copy', 'Payment Issue', 'Needed stamped paper receipt for commercial tax filing.', 'High', 'Resolved', 1, DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, -1, GETDATE()));
SET IDENTITY_INSERT Complaints OFF;
GO

-- Table 18: VisitRequests (3 rows)
SET IDENTITY_INSERT VisitRequests ON;
INSERT INTO VisitRequests (VisitRequestId, CustomerId, PropertyId, VisitDate, VisitTime, RequestStatus, CustomerNote, CreatedDate) VALUES
(1, 4, 1, DATEADD(DAY, 3, GETDATE()), '11:00 AM', 'Approved', 'Will visit with my family to inspect master bedroom.', GETDATE()),
(2, 5, 6, DATEADD(DAY, 5, GETDATE()), '03:30 PM', 'Pending', 'Interested in the furnished apartment interior.', GETDATE()),
(3, 6, 8, DATEADD(DAY, 7, GETDATE()), '05:00 PM', 'Approved', 'Client VIP visit for triplex villa.', GETDATE());
SET IDENTITY_INSERT VisitRequests OFF;
GO

-- Table 19: Reviews (3 rows)
SET IDENTITY_INSERT Reviews ON;
INSERT INTO Reviews (ReviewId, CustomerId, PropertyId, Rating, ReviewComment, ReviewStatus, ReviewDate) VALUES
(1, 4, 1, 5, 'Outstanding property with excellent build quality and serene surroundings.', 'Approved', GETDATE()),
(2, 5, 2, 4, 'Spacious modern duplex villa, friendly owner and clean surroundings.', 'Approved', GETDATE()),
(3, 6, 6, 5, 'Exceptional apartment with high-end interior and great security.', 'Approved', GETDATE());
SET IDENTITY_INSERT Reviews OFF;
GO

-- ============================================================================
-- 4. ANALYTICAL BUSINESS QUERIES (With Features, JOIN, GROUP BY, HAVING, AGGREGATES)
-- ============================================================================

-- Feature: Super Admin - Platform Sales and Commission Report (JOIN, GROUP BY, HAVING, SUM, COUNT, AVG)
SELECT 
    u.FullName AS ShopOwnerName,
    COUNT(DISTINCT o.OrderId) AS TotalOrdersCompleted,
    SUM(oi.FinalAmount) AS GrossSalesVolume,
    SUM(c.CommissionAmount) AS TotalPlatformCommission,
    AVG(oi.FinalAmount) AS AverageOrderValue
FROM Orders o
INNER JOIN OrderItems oi ON o.OrderId = oi.OrderId
INNER JOIN Users u ON oi.OwnerId = u.UserId
LEFT JOIN Commissions c ON o.OrderId = c.OrderId
WHERE o.OrderStatus = 'Completed'
GROUP BY u.FullName
HAVING SUM(oi.FinalAmount) > 50000;
GO

-- Feature: Super Admin - Review Moderation and Top-Rated Property Categories (JOIN, GROUP BY, HAVING, AVG, COUNT)
SELECT 
    pc.CategoryName,
    COUNT(r.ReviewId) AS TotalReviewsCount,
    AVG(CAST(r.Rating AS DECIMAL(3,2))) AS AverageRatingScore
FROM PropertyCategories pc
INNER JOIN Properties p ON pc.CategoryId = p.CategoryId
INNER JOIN Reviews r ON p.PropertyId = r.PropertyId
WHERE r.ReviewStatus = 'Approved'
GROUP BY pc.CategoryName
HAVING AVG(CAST(r.Rating AS DECIMAL(3,2))) >= 4.0;
GO

-- Feature: Admin (Shop Owner) - Earnings and Sales Report (JOIN, GROUP BY, SUM, COUNT)
SELECT 
    p.ListingType,
    COUNT(DISTINCT p.PropertyId) AS TotalPropertiesListed,
    COUNT(oi.OrderItemId) AS TotalUnitsSold,
    ISNULL(SUM(oi.FinalAmount), 0) AS TotalRevenueGenerated
FROM Properties p
LEFT JOIN OrderItems oi ON p.PropertyId = oi.PropertyId
GROUP BY p.ListingType;
GO

-- Feature: Customer - Order History and Printable Invoice Summary (JOIN, GROUP BY, COUNT, SUM, MAX)
SELECT 
    u.FullName AS CustomerName,
    COUNT(o.OrderId) AS OrdersPlaced,
    SUM(o.TotalAmount) AS TotalSpent,
    MAX(p.PaymentMethod) AS LastPaymentMethodUsed
FROM Users u
INNER JOIN Orders o ON u.UserId = o.CustomerId
INNER JOIN Payments p ON o.OrderId = p.OrderId
GROUP BY u.FullName;
GO

-- Feature: System Audit - Verification of All 19 Base Tables
SELECT TABLE_NAME, TABLE_TYPE 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
GO
