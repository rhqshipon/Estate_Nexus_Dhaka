-- ============================================================================
-- Script: schema.sql (database/schema.sql)
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
-- 3. SEED INITIAL DATA
-- ============================================================================

-- Roles
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (RoleId, RoleName, RoleDescription, CreatedDate) VALUES
(1, 'Customer', 'Can browse properties, make reservations, request visits, write reviews, and submit complaints.', GETDATE()),
(2, 'Admin', 'Property seller/owner who can list properties, manage inventory, handle orders, and manage visits.', GETDATE()),
(3, 'SuperAdmin', 'System administrator with full access to manage users, categories, approvals, and platform revenue.', GETDATE());
SET IDENTITY_INSERT Roles OFF;
GO

-- Default Users (Password is SHA-256 for: admin123, seller123, customer123)
-- SHA-256:
-- admin123    -> 240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9
-- seller123   -> ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f
-- customer123 -> 5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8
SET IDENTITY_INSERT Users ON;
INSERT INTO Users (UserId, RoleId, FullName, Email, Phone, PasswordHash, Address, ProfileImagePath, AccountStatus, IsActive, CreatedDate) VALUES
(1, 3, 'Super Admin', 'admin@estatenexus.com', '01700000000', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'EstateNexus HQ, Kuril, Dhaka', NULL, 'Active', 1, GETDATE()),
(2, 2, 'Property Seller', 'seller@estatenexus.com', '01711111111', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Gulshan-2, Dhaka', NULL, 'Active', 1, GETDATE()),
(3, 1, 'John Customer', 'customer@estatenexus.com', '01722222222', '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8', 'Banani, Dhaka', NULL, 'Active', 1, GETDATE());
SET IDENTITY_INSERT Users OFF;
GO

-- Categories
SET IDENTITY_INSERT PropertyCategories ON;
INSERT INTO PropertyCategories (CategoryId, CategoryName, Description, IsActive) VALUES
(1, 'Apartment', 'Residential flats, luxury condominiums, and multi-family units', 1),
(2, 'House', 'Independent residential houses, villas, and duplex homes', 1),
(3, 'Commercial', 'Offices, commercial spaces, retail shops, and warehouses', 1),
(4, 'Land', 'Residential plots, commercial lands, and open agricultural plots', 1);
SET IDENTITY_INSERT PropertyCategories OFF;
GO

-- Features
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

-- Properties
SET IDENTITY_INSERT Properties ON;
INSERT INTO Properties (PropertyId, OwnerId, CategoryId, PropertyTitle, ListingType, District, AreaLocation, FullAddress, AreaSize, AreaUnit, Bedrooms, Bathrooms, Price, Description, PropertyStatus, ApprovalStatus, IsFeatured, CreatedDate) VALUES
(1, 2, 1, 'Luxury 3-BHK Apartment in Gulshan', 'Sale', 'Dhaka', 'Gulshan', 'Road 11, Block D, Gulshan-2, Dhaka', 2200.00, 'sqft', 3, 3, 25000000.00, 'Stunning luxury apartment with serene lake view, imported fittings, and round-the-clock security.', 'Available', 'Approved', 1, GETDATE()),
(2, 2, 2, 'Modern Duplex Villa in Banani', 'Rent', 'Dhaka', 'Banani', 'Road 7, Block F, Banani, Dhaka', 3500.00, 'sqft', 4, 4, 120000.00, 'Spacious modern duplex villa with private landscaped garden, rooftop terrace, and separate servant room.', 'Available', 'Approved', 1, GETDATE()),
(3, 2, 3, 'Prime Commercial Office Space', 'Rent', 'Dhaka', 'Dhanmondi', 'Satmasjid Road, Dhanmondi, Dhaka', 1800.00, 'sqft', 0, 2, 85000.00, 'Ready commercial space optimal for software firms, corporate banks, or regional head offices.', 'Available', 'Approved', 0, GETDATE()),
(4, 2, 4, 'Residential Plot in Purbachal', 'Sale', 'Dhaka', 'Purbachal', 'Sector 4, Road 202, Purbachal New Town', 3600.00, 'sqft', 0, 0, 9500000.00, 'Prime south-facing 5 katha residential plot ready for immediate architectural construction.', 'Available', 'Approved', 0, GETDATE()),
(5, 2, 1, 'Cozy 2-BHK Flat in Uttara', 'Rent', 'Dhaka', 'Uttara', 'Sector 3, Road 14, Uttara, Dhaka', 1250.00, 'sqft', 2, 2, 35000.00, 'Bright and airy flat located within walking distance of airport and metro station.', 'Available', 'Approved', 0, GETDATE());
SET IDENTITY_INSERT Properties OFF;
GO

-- PropertyFeatureMappings
INSERT INTO PropertyFeatureMappings (PropertyId, FeatureId) VALUES
(1, 2), (1, 3), (1, 4), (1, 5), (1, 6),
(2, 1), (2, 3), (2, 4), (2, 6),
(3, 2), (3, 3), (3, 4), (3, 5),
(5, 2), (5, 4), (5, 6);
GO

-- Offers
INSERT INTO Offers (PropertyId, DiscountType, DiscountValue, StartDate, EndDate, IsActive, CreatedDate) VALUES
(1, 'Percentage', 5.00, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, 25, GETDATE()), 1, GETDATE()),
(2, 'FixedAmount', 5000.00, DATEADD(DAY, -2, GETDATE()), DATEADD(DAY, 15, GETDATE()), 1, GETDATE());
GO

-- Carts & CartItems
SET IDENTITY_INSERT Carts ON;
INSERT INTO Carts (CartId, CustomerId, CreatedDate, IsActive) VALUES
(1, 3, GETDATE(), 1);
SET IDENTITY_INSERT Carts OFF;
GO

INSERT INTO CartItems (CartId, PropertyId, RentalMonths, OfferedPrice, AddedDate) VALUES
(1, 2, 6, 115000.00, GETDATE());
GO

-- Reviews
INSERT INTO Reviews (CustomerId, PropertyId, Rating, ReviewComment, ReviewStatus, ReviewDate) VALUES
(3, 1, 5, 'Outstanding property with excellent build quality and serene surroundings.', 'Approved', GETDATE());
GO

-- ============================================================================
-- 4. VERIFICATION / AUDIT QUERIES
-- ============================================================================
-- Confirm all 19 tables created:
SELECT TABLE_NAME, TABLE_TYPE 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
GO
