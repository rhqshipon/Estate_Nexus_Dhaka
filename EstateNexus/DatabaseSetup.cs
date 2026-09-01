using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.IO;

namespace EstateNexus
{
    public static class DatabaseSetup
    {
        public static string ConnectionString
        {
            get
            {
                string configConn = ConfigurationManager.ConnectionStrings["EstateNexusDB"]?.ConnectionString;
                if (!string.IsNullOrEmpty(configConn))
                {
                    return configConn;
                }
                return @"Data Source=localhost;Initial Catalog=EstateNexusDBB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            }
        }

        private static string _masterConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                builder.InitialCatalog = "master";
                return builder.ConnectionString;
            }
        }

        public static void InitializeDatabase()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_masterConnectionString))
                {
                    connection.Open();
                    string checkDbQuery = "SELECT database_id FROM sys.databases WHERE Name = 'EstateNexusDBB'";
                    using (SqlCommand command = new SqlCommand(checkDbQuery, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result == null)
                        {
                            // Create Database
                            string createDbQuery = "CREATE DATABASE EstateNexusDBB";
                            using (SqlCommand createDbCommand = new SqlCommand(createDbQuery, connection))
                            {
                                createDbCommand.ExecuteNonQuery();
                            }

                            // Create Tables
                            CreateTables();
                        }
                    }
                }

                // Ensure initial seed data exists
                EnsureSeedData();
            }
            catch (Exception ex)
            {
                // Simple error handling for academic project
                System.Windows.Forms.MessageBox.Show("Database Initialization Error: " + ex.Message);
            }
        }

        private static void CreateTables()
        {
            string schema = @"
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
                    PaymentMethod NVARCHAR(50) DEFAULT 'Card',
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
            ";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(schema, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void EnsureSeedData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Ensure PaymentMethod column exists on existing databases
                    string ensureColsQuery = @"
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Orders') AND name = 'PaymentMethod')
                            BEGIN
                                ALTER TABLE Orders ADD PaymentMethod NVARCHAR(50) DEFAULT 'Online/Card';
                            END
                        END";
                    using (SqlCommand altCmd = new SqlCommand(ensureColsQuery, connection))
                    {
                        altCmd.ExecuteNonQuery();
                    }

                    // Seed Users
                    string userCountQuery = "SELECT COUNT(*) FROM Users";
                    using (SqlCommand cmd = new SqlCommand(userCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertUsers = @"
                                INSERT INTO Users (FullName, Email, Password, Role, Phone, Address) VALUES 
                                ('Super Admin', 'admin@estatenexus.com', 'admin123', 'SuperAdmin', '01700000000', 'EstateNexus HQ'),
                                ('Property Seller', 'seller@estatenexus.com', 'seller123', 'Admin', '01711111111', 'Gulshan, Dhaka'),
                                ('John Customer', 'customer@estatenexus.com', 'customer123', 'Customer', '01722222222', 'Banani, Dhaka');
                            ";
                            using (SqlCommand insertCmd = new SqlCommand(insertUsers, connection))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Seed Categories
                    string catCountQuery = "SELECT COUNT(*) FROM PropertyCategories";
                    using (SqlCommand cmd = new SqlCommand(catCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertCats = @"
                                INSERT INTO PropertyCategories (CategoryName, Description) VALUES 
                                ('Apartment', 'Residential apartments and flats'),
                                ('House', 'Independent houses and villas'),
                                ('Commercial', 'Commercial office spaces and shops'),
                                ('Land', 'Plots and open land');
                            ";
                            using (SqlCommand insertCmd = new SqlCommand(insertCats, connection))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Seed Properties
                    string propCountQuery = "SELECT COUNT(*) FROM Properties";
                    using (SqlCommand cmd = new SqlCommand(propCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertProps = @"
                                -- Get Seller ID
                                DECLARE @SellerId INT = (SELECT TOP 1 UserId FROM Users WHERE Role = 'Admin');
                                DECLARE @AptId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'Apartment');
                                DECLARE @HouseId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'House');
                                DECLARE @CommId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'Commercial');
                                DECLARE @LandId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'Land');

                                INSERT INTO Properties (OwnerId, CategoryId, PropertyName, ListingType, Location, Address, Area, Bedrooms, Bathrooms, Price, Description, Status)
                                VALUES 
                                (@SellerId, @AptId, 'Luxury 3-BHK Apartment in Gulshan', 'Sale', 'Gulshan', 'Road 11, Block D, Gulshan-2, Dhaka', 2200, 3, 3, 25000000.00, 'Stunning luxury apartment with lake view, imported fittings, and 24/7 security.', 'Available'),
                                (@SellerId, @HouseId, 'Modern Duplex Villa in Banani', 'Rent', 'Banani', 'Road 7, Block F, Banani, Dhaka', 3500, 4, 4, 120000.00, 'Spacious modern duplex villa with private garden, rooftop terrace, and servant room.', 'Available'),
                                (@SellerId, @CommId, 'Prime Commercial Office Space', 'Rent', 'Dhanmondi', 'Satmasjid Road, Dhanmondi, Dhaka', 1800, 0, 2, 85000.00, 'Ready commercial space ideal for IT company, bank, or corporate headquarters.', 'Available'),
                                (@SellerId, @LandId, 'Residential Plot in Purbachal', 'Sale', 'Purbachal', 'Sector 4, Road 202, Purbachal New Town', 3600, 0, 0, 9500000.00, 'South-facing 5 katha residential plot ready for immediate construction.', 'Available'),
                                (@SellerId, @AptId, 'Cozy 2-BHK Flat in Uttara', 'Rent', 'Uttara', 'Sector 3, Road 14, Uttara, Dhaka', 1250, 2, 2, 35000.00, 'Well-ventilated flat close to airport and metro rail station.', 'Available');
                            ";
                            using (SqlCommand insertCmd = new SqlCommand(insertProps, connection))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch
            {
                // Fallback silently if tables not ready yet
            }
        }
    }
}
