using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

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
                            string createDbQuery = "CREATE DATABASE EstateNexusDBB";
                            using (SqlCommand createDbCommand = new SqlCommand(createDbQuery, connection))
                            {
                                createDbCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                // Create Tables and Seed Data
                CreateTables();
                MigrateExistingSchema();
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
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Roles')
                BEGIN
                    CREATE TABLE Roles (
                        RoleId INT IDENTITY(1,1) PRIMARY KEY,
                        RoleName NVARCHAR(50) NOT NULL UNIQUE,
                        RoleDescription NVARCHAR(255) NULL,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
                BEGIN
                    CREATE TABLE Users (
                        UserId INT IDENTITY(1,1) PRIMARY KEY,
                        RoleId INT NOT NULL CONSTRAINT FK_Users_Roles REFERENCES Roles(RoleId),
                        FullName NVARCHAR(100) NOT NULL,
                        Email NVARCHAR(100) NOT NULL UNIQUE,
                        Username NVARCHAR(50) NULL,
                        Phone NVARCHAR(20) NULL,
                        PasswordHash NVARCHAR(256) NOT NULL,
                        Address NVARCHAR(255) NULL,
                        ProfileImagePath NVARCHAR(500) NULL,
                        AccountStatus NVARCHAR(20) NOT NULL DEFAULT 'Active',
                        IsActive BIT NOT NULL DEFAULT 1,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PropertyCategories')
                BEGIN
                    CREATE TABLE PropertyCategories (
                        CategoryId INT IDENTITY(1,1) PRIMARY KEY,
                        CategoryName NVARCHAR(50) NOT NULL UNIQUE,
                        Description NVARCHAR(255) NULL,
                        IsActive BIT NOT NULL DEFAULT 1
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Properties')
                BEGIN
                    CREATE TABLE Properties (
                        PropertyId INT IDENTITY(1,1) PRIMARY KEY,
                        OwnerId INT NOT NULL CONSTRAINT FK_Properties_Users REFERENCES Users(UserId),
                        CategoryId INT NOT NULL CONSTRAINT FK_Properties_Categories REFERENCES PropertyCategories(CategoryId),
                        PropertyTitle NVARCHAR(150) NOT NULL,
                        ListingType NVARCHAR(20) NOT NULL,
                        District NVARCHAR(100) NOT NULL,
                        AreaLocation NVARCHAR(100) NOT NULL,
                        FullAddress NVARCHAR(255) NOT NULL,
                        AreaSize DECIMAL(10,2) NOT NULL,
                        AreaUnit NVARCHAR(20) NOT NULL DEFAULT 'sqft',
                        Bedrooms INT NOT NULL DEFAULT 0,
                        Bathrooms INT NOT NULL DEFAULT 0,
                        Price DECIMAL(18,2) NOT NULL,
                        Description NVARCHAR(MAX) NULL,
                        PropertyStatus NVARCHAR(20) NOT NULL DEFAULT 'Available',
                        ApprovalStatus NVARCHAR(20) NOT NULL DEFAULT 'Pending',
                        IsFeatured BIT NOT NULL DEFAULT 0,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                        UpdatedDate DATETIME NULL
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PropertyImages')
                BEGIN
                    CREATE TABLE PropertyImages (
                        ImageId INT IDENTITY(1,1) PRIMARY KEY,
                        PropertyId INT NOT NULL CONSTRAINT FK_PropertyImages_Properties REFERENCES Properties(PropertyId) ON DELETE CASCADE,
                        ImagePath NVARCHAR(500) NOT NULL,
                        IsPrimary BIT NOT NULL DEFAULT 0,
                        UploadedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Offers')
                BEGIN
                    CREATE TABLE Offers (
                        OfferId INT IDENTITY(1,1) PRIMARY KEY,
                        PropertyId INT NOT NULL CONSTRAINT FK_Offers_Properties REFERENCES Properties(PropertyId) ON DELETE CASCADE,
                        DiscountType NVARCHAR(50) NOT NULL,
                        DiscountValue DECIMAL(18,2) NOT NULL,
                        StartDate DATETIME NOT NULL,
                        EndDate DATETIME NOT NULL,
                        IsActive BIT NOT NULL DEFAULT 1,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PropertyFeatures')
                BEGIN
                    CREATE TABLE PropertyFeatures (
                        FeatureId INT IDENTITY(1,1) PRIMARY KEY,
                        FeatureName NVARCHAR(100) NOT NULL UNIQUE,
                        Description NVARCHAR(255) NULL
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PropertyFeatureMappings')
                BEGIN
                    CREATE TABLE PropertyFeatureMappings (
                        PropertyId INT NOT NULL CONSTRAINT FK_PropertyFeatureMappings_Properties REFERENCES Properties(PropertyId) ON DELETE CASCADE,
                        FeatureId INT NOT NULL CONSTRAINT FK_PropertyFeatureMappings_Features REFERENCES PropertyFeatures(FeatureId) ON DELETE CASCADE,
                        CONSTRAINT PK_PropertyFeatureMappings PRIMARY KEY (PropertyId, FeatureId)
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FeaturedListings')
                BEGIN
                    CREATE TABLE FeaturedListings (
                        FeaturedListingId INT IDENTITY(1,1) PRIMARY KEY,
                        PropertyId INT NOT NULL CONSTRAINT FK_FeaturedListings_Properties REFERENCES Properties(PropertyId) ON DELETE CASCADE,
                        FeaturedFee DECIMAL(18,2) NOT NULL,
                        StartDate DATETIME NOT NULL,
                        EndDate DATETIME NOT NULL,
                        PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
                        Status NVARCHAR(50) NOT NULL DEFAULT 'Active',
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Carts')
                BEGIN
                    CREATE TABLE Carts (
                        CartId INT IDENTITY(1,1) PRIMARY KEY,
                        CustomerId INT NOT NULL CONSTRAINT FK_Carts_Users REFERENCES Users(UserId) ON DELETE CASCADE,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                        IsActive BIT NOT NULL DEFAULT 1
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CartItems')
                BEGIN
                    CREATE TABLE CartItems (
                        CartItemId INT IDENTITY(1,1) PRIMARY KEY,
                        CartId INT NOT NULL CONSTRAINT FK_CartItems_Carts REFERENCES Carts(CartId) ON DELETE CASCADE,
                        PropertyId INT NOT NULL CONSTRAINT FK_CartItems_Properties REFERENCES Properties(PropertyId),
                        RentalMonths INT NOT NULL DEFAULT 1,
                        OfferedPrice DECIMAL(18,2) NULL,
                        AddedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders')
                BEGIN
                    CREATE TABLE Orders (
                        OrderId INT IDENTITY(1,1) PRIMARY KEY,
                        CustomerId INT NOT NULL CONSTRAINT FK_Orders_Users REFERENCES Users(UserId),
                        OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
                        TotalAmount DECIMAL(18,2) NOT NULL,
                        OrderStatus NVARCHAR(50) NOT NULL DEFAULT 'Completed',
                        TransactionType NVARCHAR(50) NOT NULL DEFAULT 'Sale'
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderItems')
                BEGIN
                    CREATE TABLE OrderItems (
                        OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
                        OrderId INT NOT NULL CONSTRAINT FK_OrderItems_Orders REFERENCES Orders(OrderId) ON DELETE CASCADE,
                        PropertyId INT NOT NULL CONSTRAINT FK_OrderItems_Properties REFERENCES Properties(PropertyId),
                        OwnerId INT NOT NULL CONSTRAINT FK_OrderItems_Users REFERENCES Users(UserId),
                        Quantity INT NOT NULL DEFAULT 1,
                        RentalMonths INT NOT NULL DEFAULT 0,
                        UnitPrice DECIMAL(18,2) NOT NULL,
                        DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                        FinalAmount DECIMAL(18,2) NOT NULL
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Payments')
                BEGIN
                    CREATE TABLE Payments (
                        PaymentId INT IDENTITY(1,1) PRIMARY KEY,
                        OrderId INT NOT NULL CONSTRAINT FK_Payments_Orders REFERENCES Orders(OrderId) ON DELETE CASCADE,
                        PaymentMethod NVARCHAR(50) NOT NULL,
                        TransactionId NVARCHAR(100) NOT NULL,
                        PaymentAmount DECIMAL(18,2) NOT NULL,
                        PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Completed',
                        PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Invoices')
                BEGIN
                    CREATE TABLE Invoices (
                        InvoiceId INT IDENTITY(1,1) PRIMARY KEY,
                        OrderId INT NOT NULL CONSTRAINT FK_Invoices_Orders REFERENCES Orders(OrderId),
                        PaymentId INT NOT NULL CONSTRAINT FK_Invoices_Payments REFERENCES Payments(PaymentId),
                        InvoiceNumber NVARCHAR(50) NOT NULL UNIQUE,
                        SubTotal DECIMAL(18,2) NOT NULL,
                        DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                        CommissionAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                        TotalAmount DECIMAL(18,2) NOT NULL,
                        GeneratedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Commissions')
                BEGIN
                    CREATE TABLE Commissions (
                        CommissionId INT IDENTITY(1,1) PRIMARY KEY,
                        OrderId INT NOT NULL CONSTRAINT FK_Commissions_Orders REFERENCES Orders(OrderId) ON DELETE CASCADE,
                        CommissionRate DECIMAL(5,2) NOT NULL,
                        TransactionAmount DECIMAL(18,2) NOT NULL,
                        CommissionAmount DECIMAL(18,2) NOT NULL,
                        OwnerAmount DECIMAL(18,2) NOT NULL,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Complaints')
                BEGIN
                    CREATE TABLE Complaints (
                        ComplaintId INT IDENTITY(1,1) PRIMARY KEY,
                        CustomerId INT NOT NULL CONSTRAINT FK_Complaints_Customer REFERENCES Users(UserId),
                        PropertyId INT NULL CONSTRAINT FK_Complaints_Properties REFERENCES Properties(PropertyId),
                        Subject NVARCHAR(200) NOT NULL,
                        ComplaintType NVARCHAR(50) NOT NULL,
                        Description NVARCHAR(MAX) NOT NULL,
                        Priority NVARCHAR(20) NOT NULL DEFAULT 'Normal',
                        ComplaintStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
                        ResolvedBy INT NULL CONSTRAINT FK_Complaints_ResolvedBy REFERENCES Users(UserId),
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                        ResolvedDate DATETIME NULL
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VisitRequests')
                BEGIN
                    CREATE TABLE VisitRequests (
                        VisitRequestId INT IDENTITY(1,1) PRIMARY KEY,
                        CustomerId INT NOT NULL CONSTRAINT FK_VisitRequests_Users REFERENCES Users(UserId),
                        PropertyId INT NOT NULL CONSTRAINT FK_VisitRequests_Properties REFERENCES Properties(PropertyId) ON DELETE CASCADE,
                        VisitDate DATE NOT NULL,
                        VisitTime NVARCHAR(20) NULL,
                        RequestStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
                        CustomerNote NVARCHAR(MAX) NULL,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END

                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Reviews')
                BEGIN
                    CREATE TABLE Reviews (
                        ReviewId INT IDENTITY(1,1) PRIMARY KEY,
                        CustomerId INT NOT NULL CONSTRAINT FK_Reviews_Users REFERENCES Users(UserId),
                        PropertyId INT NOT NULL CONSTRAINT FK_Reviews_Properties REFERENCES Properties(PropertyId) ON DELETE CASCADE,
                        Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
                        ReviewComment NVARCHAR(MAX) NULL,
                        ReviewStatus NVARCHAR(50) NOT NULL DEFAULT 'Approved',
                        ReviewDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END
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

        private static void MigrateExistingSchema()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string migrationScript = @"
                        -- 1. Ensure standard Roles exist
                        IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'Customer')
                            INSERT INTO Roles (RoleName, RoleDescription) VALUES ('Customer', 'Can browse properties, make reservations, request visits, write reviews, and submit complaints.');
                        IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'Admin')
                            INSERT INTO Roles (RoleName, RoleDescription) VALUES ('Admin', 'Property seller/owner who can list properties, manage inventory, handle orders, and manage visits.');
                        IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = 'SuperAdmin')
                            INSERT INTO Roles (RoleName, RoleDescription) VALUES ('SuperAdmin', 'System administrator with full access to manage users, categories, approvals, and platform revenue.');

                        -- 2. Migrate Users table
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'RoleId')
                                ALTER TABLE Users ADD RoleId INT NULL;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'PasswordHash')
                                ALTER TABLE Users ADD PasswordHash NVARCHAR(256) NULL;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'IsActive')
                                ALTER TABLE Users ADD IsActive BIT NOT NULL DEFAULT 1;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'AccountStatus')
                                ALTER TABLE Users ADD AccountStatus NVARCHAR(20) NOT NULL DEFAULT 'Active';

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'ProfileImagePath')
                                ALTER TABLE Users ADD ProfileImagePath NVARCHAR(500) NULL;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'CreatedDate')
                                ALTER TABLE Users ADD CreatedDate DATETIME NOT NULL DEFAULT GETDATE();

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Username')
                                ALTER TABLE Users ADD Username NVARCHAR(50) NULL;

                            -- Map Role string to RoleId if present
                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Role')
                            BEGIN
                                UPDATE Users SET RoleId = (SELECT TOP 1 RoleId FROM Roles WHERE RoleName = 'SuperAdmin') WHERE (Role = 'SuperAdmin' OR (Role = 'Admin' AND Email LIKE '%admin%')) AND (RoleId IS NULL OR RoleId = 0);
                                UPDATE Users SET RoleId = (SELECT TOP 1 RoleId FROM Roles WHERE RoleName = 'Admin') WHERE Role = 'Admin' AND (RoleId IS NULL OR RoleId = 0);
                                UPDATE Users SET RoleId = (SELECT TOP 1 RoleId FROM Roles WHERE RoleName = 'Customer') WHERE (Role = 'Customer' OR Role IS NULL OR RoleId IS NULL OR RoleId = 0);
                            END
                            ELSE
                            BEGIN
                                UPDATE Users SET RoleId = (SELECT TOP 1 RoleId FROM Roles WHERE RoleName = 'Customer') WHERE RoleId IS NULL OR RoleId = 0;
                            END

                            -- Copy Password to PasswordHash if not set
                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Password')
                            BEGIN
                                UPDATE Users SET PasswordHash = Password WHERE PasswordHash IS NULL OR PasswordHash = '';
                            END

                            UPDATE Users SET IsActive = 1 WHERE IsActive IS NULL;
                            UPDATE Users SET AccountStatus = 'Active' WHERE AccountStatus IS NULL OR AccountStatus = '';

                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Password')
                                ALTER TABLE Users ALTER COLUMN [Password] NVARCHAR(256) NULL;
                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Role')
                                ALTER TABLE Users ALTER COLUMN [Role] NVARCHAR(50) NULL;

                            IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Users_Roles')
                            BEGIN
                                ALTER TABLE Users ADD CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId);
                            END
                        END

                        -- 3. Migrate PropertyCategories table
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PropertyCategories')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('PropertyCategories') AND name = 'IsActive')
                                ALTER TABLE PropertyCategories ADD IsActive BIT NOT NULL DEFAULT 1;
                            EXEC('UPDATE PropertyCategories SET IsActive = 1 WHERE IsActive IS NULL;');
                        END

                        -- 4. Migrate Properties table
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Properties')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'PropertyTitle')
                                ALTER TABLE Properties ADD PropertyTitle NVARCHAR(150) NULL;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'District')
                                ALTER TABLE Properties ADD District NVARCHAR(100) NOT NULL DEFAULT 'Dhaka';

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'AreaLocation')
                                ALTER TABLE Properties ADD AreaLocation NVARCHAR(100) NOT NULL DEFAULT 'General';

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'FullAddress')
                                ALTER TABLE Properties ADD FullAddress NVARCHAR(255) NOT NULL DEFAULT 'Dhaka';

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'AreaSize')
                                ALTER TABLE Properties ADD AreaSize DECIMAL(10,2) NOT NULL DEFAULT 1000.00;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'AreaUnit')
                                ALTER TABLE Properties ADD AreaUnit NVARCHAR(20) NOT NULL DEFAULT 'sqft';

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'PropertyStatus')
                                ALTER TABLE Properties ADD PropertyStatus NVARCHAR(20) NOT NULL DEFAULT 'Available';

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'ApprovalStatus')
                                ALTER TABLE Properties ADD ApprovalStatus NVARCHAR(20) NOT NULL DEFAULT 'Approved';

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'IsFeatured')
                                ALTER TABLE Properties ADD IsFeatured BIT NOT NULL DEFAULT 0;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'UpdatedDate')
                                ALTER TABLE Properties ADD UpdatedDate DATETIME NULL;

                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'PropertyName')
                            BEGIN
                                ALTER TABLE Properties ALTER COLUMN PropertyName NVARCHAR(150) NULL;
                                EXEC('UPDATE Properties SET PropertyTitle = PropertyName WHERE PropertyTitle IS NULL OR PropertyTitle = ''''');
                            END

                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'Location')
                            BEGIN
                                EXEC('UPDATE Properties SET District = ISNULL(Location, ''Dhaka'') WHERE District = ''Dhaka''');
                                EXEC('UPDATE Properties SET AreaLocation = ISNULL(Location, ''General'') WHERE AreaLocation = ''General''');
                            END

                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'Address')
                                EXEC('UPDATE Properties SET FullAddress = ISNULL(Address, ''Dhaka'') WHERE FullAddress = ''Dhaka''');

                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'Area')
                                EXEC('UPDATE Properties SET AreaSize = CAST(ISNULL(Area, 1000) AS DECIMAL(10,2)) WHERE AreaSize = 1000.00');

                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Properties') AND name = 'Status')
                                EXEC('UPDATE Properties SET PropertyStatus = ISNULL(Status, ''Available'') WHERE PropertyStatus = ''Available''');

                            DECLARE @DefaultSeller INT = (SELECT TOP 1 UserId FROM Users WHERE RoleId = (SELECT TOP 1 RoleId FROM Roles WHERE RoleName = 'Admin'));
                            IF @DefaultSeller IS NOT NULL
                                UPDATE Properties SET OwnerId = @DefaultSeller WHERE OwnerId IS NULL;
                        END

                        -- 5. Migrate VisitRequests
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'VisitRequests')
                        BEGIN
                            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'VisitId')
                               AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'VisitRequestId')
                            BEGIN
                                EXEC sp_rename 'VisitRequests.VisitId', 'VisitRequestId', 'COLUMN';
                            END

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'RequestStatus')
                            BEGIN
                                ALTER TABLE VisitRequests ADD RequestStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending';
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'Status')
                                    EXEC('UPDATE VisitRequests SET RequestStatus = ISNULL(Status, ''Pending'')');
                            END

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'CustomerNote')
                            BEGIN
                                ALTER TABLE VisitRequests ADD CustomerNote NVARCHAR(MAX) NULL;
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'Notes')
                                    EXEC('UPDATE VisitRequests SET CustomerNote = Notes WHERE CustomerNote IS NULL');
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'Note')
                                    EXEC('UPDATE VisitRequests SET CustomerNote = Note WHERE CustomerNote IS NULL');
                            END

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'CreatedDate')
                            BEGIN
                                ALTER TABLE VisitRequests ADD CreatedDate DATETIME NOT NULL DEFAULT GETDATE();
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('VisitRequests') AND name = 'CreatedAt')
                                    EXEC('UPDATE VisitRequests SET CreatedDate = ISNULL(CreatedAt, GETDATE())');
                            END
                        END

                        -- 6. Migrate Reviews
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Reviews')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Reviews') AND name = 'ReviewComment')
                            BEGIN
                                ALTER TABLE Reviews ADD ReviewComment NVARCHAR(MAX) NULL;
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Reviews') AND name = 'Comment')
                                    EXEC('UPDATE Reviews SET ReviewComment = Comment WHERE ReviewComment IS NULL');
                            END

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Reviews') AND name = 'ReviewStatus')
                            BEGIN
                                ALTER TABLE Reviews ADD ReviewStatus NVARCHAR(50) NOT NULL DEFAULT 'Approved';
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Reviews') AND name = 'Status')
                                    EXEC('UPDATE Reviews SET ReviewStatus = ISNULL(Status, ''Approved'')');
                            END

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Reviews') AND name = 'ReviewDate')
                            BEGIN
                                ALTER TABLE Reviews ADD ReviewDate DATETIME NOT NULL DEFAULT GETDATE();
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Reviews') AND name = 'CreatedDate')
                                    EXEC('UPDATE Reviews SET ReviewDate = ISNULL(CreatedDate, GETDATE())');
                            END
                        END

                        -- 6. Migrate Orders
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Orders')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Orders') AND name = 'OrderStatus')
                            BEGIN
                                ALTER TABLE Orders ADD OrderStatus NVARCHAR(50) NOT NULL DEFAULT 'Completed';
                                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Orders') AND name = 'Status')
                                    EXEC('UPDATE Orders SET OrderStatus = ISNULL(Status, ''Completed'')');
                            END

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Orders') AND name = 'TransactionType')
                                ALTER TABLE Orders ADD TransactionType NVARCHAR(50) NOT NULL DEFAULT 'Sale';
                        END

                        -- 7. Migrate OrderItems
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderItems')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('OrderItems') AND name = 'OwnerId')
                            BEGIN
                                ALTER TABLE OrderItems ADD OwnerId INT NULL;
                                EXEC('UPDATE oi SET oi.OwnerId = ISNULL(p.OwnerId, (SELECT TOP 1 UserId FROM Users WHERE RoleId = 2)) FROM OrderItems oi LEFT JOIN Properties p ON oi.PropertyId = p.PropertyId');
                            END

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('OrderItems') AND name = 'Quantity')
                                ALTER TABLE OrderItems ADD Quantity INT NOT NULL DEFAULT 1;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('OrderItems') AND name = 'RentalMonths')
                                ALTER TABLE OrderItems ADD RentalMonths INT NOT NULL DEFAULT 0;

                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('OrderItems') AND name = 'DiscountAmount')
                                ALTER TABLE OrderItems ADD DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0;
                        END

                        -- 8. Migrate Invoices
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Invoices')
                        BEGIN
                            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Invoices') AND name = 'CommissionAmount')
                                ALTER TABLE Invoices ADD CommissionAmount DECIMAL(18,2) NOT NULL DEFAULT 0;
                        END
                    ";

                    using (SqlCommand cmd = new SqlCommand(migrationScript, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Fallback silently if tables are already in expected shape
            }
        }

        private static void EnsureSeedData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // 1. Seed Roles
                    string roleCountQuery = "SELECT COUNT(*) FROM Roles";
                    using (SqlCommand cmd = new SqlCommand(roleCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertRoles = @"
                                SET IDENTITY_INSERT Roles ON;
                                INSERT INTO Roles (RoleId, RoleName, RoleDescription, CreatedDate) VALUES
                                (1, 'Customer', 'Can browse properties, make reservations, request visits, write reviews, and submit complaints.', GETDATE()),
                                (2, 'Admin', 'Property seller/owner who can list properties, manage inventory, handle orders, and manage visits.', GETDATE()),
                                (3, 'SuperAdmin', 'System administrator with full access to manage users, categories, approvals, and platform revenue.', GETDATE());
                                SET IDENTITY_INSERT Roles OFF;
                            ";
                            using (SqlCommand insertCmd = new SqlCommand(insertRoles, connection))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // 2. Seed Users
                    string userCountQuery = "SELECT COUNT(*) FROM Users";
                    using (SqlCommand cmd = new SqlCommand(userCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string adminHash = PasswordHelper.HashPassword("admin123");
                            string sellerHash = PasswordHelper.HashPassword("seller123");
                            string customerHash = PasswordHelper.HashPassword("customer123");

                            string insertUsers = @"
                                SET IDENTITY_INSERT Users ON;
                                INSERT INTO Users (UserId, RoleId, FullName, Email, Phone, PasswordHash, Address, ProfileImagePath, AccountStatus, IsActive, CreatedDate) VALUES 
                                (1, 3, 'Super Admin', 'admin@estatenexus.com', '01700000000', @AdminPassword, 'EstateNexus HQ, Kuril, Dhaka', NULL, 'Active', 1, GETDATE()),
                                (2, 2, 'Property Seller', 'seller@estatenexus.com', '01711111111', @SellerPassword, 'Gulshan-2, Dhaka', NULL, 'Active', 1, GETDATE()),
                                (3, 1, 'John Customer', 'customer@estatenexus.com', '01722222222', @CustomerPassword, 'Banani, Dhaka', NULL, 'Active', 1, GETDATE());
                                SET IDENTITY_INSERT Users OFF;
                            ";
                            using (SqlCommand insertCmd = new SqlCommand(insertUsers, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@AdminPassword", adminHash);
                                insertCmd.Parameters.AddWithValue("@SellerPassword", sellerHash);
                                insertCmd.Parameters.AddWithValue("@CustomerPassword", customerHash);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // 3. Seed Categories
                    string catCountQuery = "SELECT COUNT(*) FROM PropertyCategories";
                    using (SqlCommand cmd = new SqlCommand(catCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertCats = @"
                                SET IDENTITY_INSERT PropertyCategories ON;
                                INSERT INTO PropertyCategories (CategoryId, CategoryName, Description, IsActive) VALUES 
                                (1, 'Apartment', 'Residential flats, luxury condominiums, and multi-family units', 1),
                                (2, 'House', 'Independent residential houses, villas, and duplex homes', 1),
                                (3, 'Commercial', 'Offices, commercial spaces, retail shops, and warehouses', 1),
                                (4, 'Land', 'Residential plots, commercial lands, and open agricultural plots', 1);
                                SET IDENTITY_INSERT PropertyCategories OFF;
                            ";
                            using (SqlCommand insertCmd = new SqlCommand(insertCats, connection))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // 4. Seed Features
                    string featCountQuery = "SELECT COUNT(*) FROM PropertyFeatures";
                    using (SqlCommand cmd = new SqlCommand(featCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertFeats = @"
                                SET IDENTITY_INSERT PropertyFeatures ON;
                                INSERT INTO PropertyFeatures (FeatureId, FeatureName, Description) VALUES
                                (1, 'Swimming Pool', 'Private or shared luxury swimming pool facility'),
                                (2, 'Elevator / Lift', 'High-speed passenger elevator in the building'),
                                (3, 'Car Parking', 'Dedicated covered parking space'),
                                (4, '24/7 Security & CCTV', 'Gated security guard surveillance 24 hours'),
                                (5, 'Backup Generator', 'Full electricity backup for apartments and common areas'),
                                (6, 'Balcony / Terrace', 'Spacious open view balcony or private terrace');
                                SET IDENTITY_INSERT PropertyFeatures OFF;
                            ";
                            using (SqlCommand insertCmd = new SqlCommand(insertFeats, connection))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // 5. Seed Properties
                    string propCountQuery = "SELECT COUNT(*) FROM Properties";
                    using (SqlCommand cmd = new SqlCommand(propCountQuery, connection))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertProps = @"
                                DECLARE @SellerId INT = (SELECT TOP 1 UserId FROM Users WHERE RoleId = 2);
                                DECLARE @AptId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'Apartment');
                                DECLARE @HouseId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'House');
                                DECLARE @CommId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'Commercial');
                                DECLARE @LandId INT = (SELECT TOP 1 CategoryId FROM PropertyCategories WHERE CategoryName = 'Land');

                                SET IDENTITY_INSERT Properties ON;
                                INSERT INTO Properties (PropertyId, OwnerId, CategoryId, PropertyTitle, ListingType, District, AreaLocation, FullAddress, AreaSize, AreaUnit, Bedrooms, Bathrooms, Price, Description, PropertyStatus, ApprovalStatus, IsFeatured, CreatedDate) VALUES 
                                (1, @SellerId, @AptId, 'Luxury 3-BHK Apartment in Gulshan', 'Sale', 'Dhaka', 'Gulshan', 'Road 11, Block D, Gulshan-2, Dhaka', 2200.00, 'sqft', 3, 3, 25000000.00, 'Stunning luxury apartment with lake view, imported fittings, and 24/7 security.', 'Available', 'Approved', 1, GETDATE()),
                                (2, @SellerId, @HouseId, 'Modern Duplex Villa in Banani', 'Rent', 'Dhaka', 'Banani', 'Road 7, Block F, Banani, Dhaka', 3500.00, 'sqft', 4, 4, 120000.00, 'Spacious modern duplex villa with private garden, rooftop terrace, and servant room.', 'Available', 'Approved', 1, GETDATE()),
                                (3, @SellerId, @CommId, 'Prime Commercial Office Space', 'Rent', 'Dhaka', 'Dhanmondi', 'Satmasjid Road, Dhanmondi, Dhaka', 1800.00, 'sqft', 0, 2, 85000.00, 'Ready commercial space ideal for IT company, bank, or corporate headquarters.', 'Available', 'Approved', 0, GETDATE()),
                                (4, @SellerId, @LandId, 'Residential Plot in Purbachal', 'Sale', 'Dhaka', 'Purbachal', 'Sector 4, Road 202, Purbachal New Town', 3600.00, 'sqft', 0, 0, 9500000.00, 'South-facing 5 katha residential plot ready for immediate construction.', 'Available', 'Approved', 0, GETDATE()),
                                (5, @SellerId, @AptId, 'Cozy 2-BHK Flat in Uttara', 'Rent', 'Dhaka', 'Uttara', 'Sector 3, Road 14, Uttara, Dhaka', 1250.00, 'sqft', 2, 2, 35000.00, 'Well-ventilated flat close to airport and metro rail station.', 'Available', 'Approved', 0, GETDATE());
                                SET IDENTITY_INSERT Properties OFF;

                                INSERT INTO PropertyFeatureMappings (PropertyId, FeatureId) VALUES
                                (1, 2), (1, 3), (1, 4), (1, 5), (1, 6),
                                (2, 1), (2, 3), (2, 4), (2, 6),
                                (3, 2), (3, 3), (3, 4), (3, 5),
                                (5, 2), (5, 4), (5, 6);
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
                // Fallback silently if database not reachable at startup
            }
        }
    }
}
