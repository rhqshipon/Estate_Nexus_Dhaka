-- Database Name: EstateNexusDBB

-- 1. Create the Database
CREATE DATABASE EstateNexusDBB;
GO

USE EstateNexusDBB;
GO

-- 2. Create Tables
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    Password NVARCHAR(100) NOT NULL,
    Address NVARCHAR(255),
    Role NVARCHAR(20) NOT NULL, -- SuperAdmin, Admin, Customer
    AccountStatus NVARCHAR(20) DEFAULT 'Active',
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE PropertyCategories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255)
);

CREATE TABLE Properties (
    PropertyId INT IDENTITY(1,1) PRIMARY KEY,
    OwnerId INT FOREIGN KEY REFERENCES Users(UserId),
    CategoryId INT FOREIGN KEY REFERENCES PropertyCategories(CategoryId),
    PropertyName NVARCHAR(150) NOT NULL,
    ListingType NVARCHAR(20), -- Rent, Sale
    Location NVARCHAR(100),
    Address NVARCHAR(255),
    Area INT,
    Bedrooms INT,
    Bathrooms INT,
    Price DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(MAX),
    Status NVARCHAR(20) DEFAULT 'Available',
    CreatedDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE VisitRequests (
    VisitId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Users(UserId),
    PropertyId INT FOREIGN KEY REFERENCES Properties(PropertyId),
    VisitDate DATE NOT NULL,
    VisitTime NVARCHAR(20),
    Status NVARCHAR(20) DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Users(UserId),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2),
    Status NVARCHAR(20) DEFAULT 'Completed'
);

CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    PropertyId INT FOREIGN KEY REFERENCES Properties(PropertyId),
    UnitPrice DECIMAL(18,2),
    FinalAmount DECIMAL(18,2)
);

CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Users(UserId),
    PropertyId INT FOREIGN KEY REFERENCES Properties(PropertyId),
    Rating INT,
    Comment NVARCHAR(MAX),
    ReviewDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE ReservationCart (
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Users(UserId),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE ReservationCartItems (
    CartItemId INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT FOREIGN KEY REFERENCES ReservationCart(CartId),
    PropertyId INT FOREIGN KEY REFERENCES Properties(PropertyId),
    DurationMonths INT DEFAULT 1,
    AddedAt DATETIME DEFAULT GETDATE()
);

-- 3. Insert Default Data (for Viva demonstration)
INSERT INTO Users (FullName, Email, Password, Role) 
VALUES ('Super Admin', 'admin@estatenexus.com', 'admin123', 'SuperAdmin');

INSERT INTO PropertyCategories (CategoryName) 
VALUES ('Apartment'), ('House'), ('Commercial'), ('Land');

-- 4. Useful Queries for Viva

-- Search available properties by name
-- SELECT PropertyId, PropertyName, ListingType, Location, Address, Price, Status 
-- FROM Properties WHERE Status = 'Available' AND PropertyName LIKE '%searchTerm%';

-- View Visit Requests for an Admin's property
-- SELECT v.VisitId, u.FullName as Customer, p.PropertyName, v.VisitDate, v.VisitTime, v.Status 
-- FROM VisitRequests v
-- JOIN Properties p ON v.PropertyId = p.PropertyId
-- JOIN Users u ON v.CustomerId = u.UserId
-- WHERE p.OwnerId = @OwnerId;

-- View Customer Cart Items
-- SELECT c.CartItemId, p.PropertyName, p.Price 
-- FROM ReservationCartItems c 
-- JOIN Properties p ON c.PropertyId = p.PropertyId 
-- JOIN ReservationCart rc ON c.CartId = rc.CartId 
-- WHERE rc.CustomerId = @CustomerId;
