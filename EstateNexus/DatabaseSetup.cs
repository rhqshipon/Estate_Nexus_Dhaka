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
                string configConn =
                    ConfigurationManager.ConnectionStrings["EstateNexusDB"]?.ConnectionString;

                if (!string.IsNullOrEmpty(configConn))
                {
                    return configConn;
                }

                return @"Data Source=.\MSSQLSERVER01;Initial Catalog=EstateNexusDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            }
        }

        private static string MasterConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder =
                    new SqlConnectionStringBuilder(ConnectionString);

                builder.InitialCatalog = "master";

                return builder.ConnectionString;
            }
        }

        public static void InitializeDatabase()
        {
            try
            {
                CreateDatabaseIfNotExists();

                CreateTables();

                MigrateExistingSchema();

                EnsureSeedData();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Database Initialization Error:\n\n" + ex.Message,
                    "Database Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // CREATE DATABASE
        // =====================================================

        private static void CreateDatabaseIfNotExists()
        {
            using (SqlConnection connection =
                   new SqlConnection(MasterConnectionString))
            {
                connection.Open();

                string checkDatabaseQuery = @"
                    SELECT database_id
                    FROM sys.databases
                    WHERE Name = 'EstateNexusDB'
                ";

                using (SqlCommand command =
                       new SqlCommand(checkDatabaseQuery, connection))
                {
                    object result = command.ExecuteScalar();

                    if (result == null)
                    {
                        string createDatabaseQuery =
                            "CREATE DATABASE EstateNexusDB";

                        using (SqlCommand createCommand =
                               new SqlCommand(createDatabaseQuery, connection))
                        {
                            createCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // =====================================================
        // CREATE TABLES
        // =====================================================

        private static void CreateTables()
        {
            string schema = @"

                ------------------------------------------------
                -- ROLES
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Roles'
                )
                BEGIN
                    CREATE TABLE Roles
                    (
                        RoleId INT IDENTITY(1,1) PRIMARY KEY,
                        RoleName NVARCHAR(50) NOT NULL UNIQUE,
                        RoleDescription NVARCHAR(255) NULL,
                        CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
                    );
                END


                ------------------------------------------------
                -- USERS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Users'
                )
                BEGIN
                    CREATE TABLE Users
                    (
                        UserId INT IDENTITY(1,1) PRIMARY KEY,

                        RoleId INT NOT NULL,

                        FullName NVARCHAR(100) NOT NULL,

                        Email NVARCHAR(100) NOT NULL UNIQUE,

                        Phone NVARCHAR(20) NULL,

                        PasswordHash NVARCHAR(256) NOT NULL,

                        Address NVARCHAR(255) NULL,

                        ProfileImagePath NVARCHAR(500) NULL,

                        AccountStatus NVARCHAR(20)
                        NOT NULL DEFAULT 'Active',

                        IsActive BIT
                        NOT NULL DEFAULT 1,

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_Users_Roles
                        FOREIGN KEY (RoleId)
                        REFERENCES Roles(RoleId),

                        CONSTRAINT CHK_Users_AccountStatus
                        CHECK (AccountStatus IN ('Active', 'Pending', 'Suspended', 'Inactive', 'Approved', 'Rejected'))
                    );
                END


                ------------------------------------------------
                -- PROPERTY CATEGORIES
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'PropertyCategories'
                )
                BEGIN
                    CREATE TABLE PropertyCategories
                    (
                        CategoryId INT IDENTITY(1,1) PRIMARY KEY,

                        CategoryName NVARCHAR(50)
                        NOT NULL UNIQUE,

                        Description NVARCHAR(255) NULL,

                        IsActive BIT
                        NOT NULL DEFAULT 1
                    );
                END


                ------------------------------------------------
                -- PROPERTIES
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Properties'
                )
                BEGIN
                    CREATE TABLE Properties
                    (
                        PropertyId INT IDENTITY(1,1) PRIMARY KEY,

                        OwnerId INT NOT NULL,

                        CategoryId INT NOT NULL,

                        PropertyTitle NVARCHAR(150)
                        NOT NULL,

                        ListingType NVARCHAR(20)
                        NOT NULL,

                        District NVARCHAR(100)
                        NOT NULL,

                        AreaLocation NVARCHAR(100)
                        NOT NULL,

                        FullAddress NVARCHAR(255)
                        NOT NULL,

                        AreaSize DECIMAL(10,2)
                        NOT NULL,

                        AreaUnit NVARCHAR(20)
                        NOT NULL DEFAULT 'sqft',

                        Bedrooms INT
                        NOT NULL DEFAULT 0,

                        Bathrooms INT
                        NOT NULL DEFAULT 0,

                        Price DECIMAL(18,2)
                        NOT NULL,

                        Description NVARCHAR(MAX) NULL,

                        PropertyStatus NVARCHAR(20)
                        NOT NULL DEFAULT 'Available',

                        ApprovalStatus NVARCHAR(20)
                        NOT NULL DEFAULT 'Pending',

                        IsFeatured BIT
                        NOT NULL DEFAULT 0,

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        UpdatedDate DATETIME NULL,

                        CONSTRAINT FK_Properties_Users
                        FOREIGN KEY (OwnerId)
                        REFERENCES Users(UserId),

                        CONSTRAINT FK_Properties_Categories
                        FOREIGN KEY (CategoryId)
                        REFERENCES PropertyCategories(CategoryId)
                    );
                END


                ------------------------------------------------
                -- PROPERTY IMAGES
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'PropertyImages'
                )
                BEGIN
                    CREATE TABLE PropertyImages
                    (
                        ImageId INT IDENTITY(1,1) PRIMARY KEY,

                        PropertyId INT NOT NULL,

                        ImagePath NVARCHAR(500)
                        NOT NULL,

                        IsPrimary BIT
                        NOT NULL DEFAULT 0,

                        UploadedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_PropertyImages_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- OFFERS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Offers'
                )
                BEGIN
                    CREATE TABLE Offers
                    (
                        OfferId INT IDENTITY(1,1) PRIMARY KEY,

                        PropertyId INT NOT NULL,

                        DiscountType NVARCHAR(50)
                        NOT NULL,

                        DiscountValue DECIMAL(18,2)
                        NOT NULL,

                        StartDate DATETIME
                        NOT NULL,

                        EndDate DATETIME
                        NOT NULL,

                        IsActive BIT
                        NOT NULL DEFAULT 1,

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_Offers_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- PROPERTY FEATURES
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'PropertyFeatures'
                )
                BEGIN
                    CREATE TABLE PropertyFeatures
                    (
                        FeatureId INT IDENTITY(1,1) PRIMARY KEY,

                        FeatureName NVARCHAR(100)
                        NOT NULL UNIQUE,

                        Description NVARCHAR(255)
                        NULL
                    );
                END


                ------------------------------------------------
                -- PROPERTY FEATURE MAPPINGS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'PropertyFeatureMappings'
                )
                BEGIN
                    CREATE TABLE PropertyFeatureMappings
                    (
                        PropertyId INT NOT NULL,

                        FeatureId INT NOT NULL,

                        CONSTRAINT PK_PropertyFeatureMappings
                        PRIMARY KEY (PropertyId, FeatureId),

                        CONSTRAINT FK_PropertyFeatureMappings_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId)
                        ON DELETE CASCADE,

                        CONSTRAINT FK_PropertyFeatureMappings_Features
                        FOREIGN KEY (FeatureId)
                        REFERENCES PropertyFeatures(FeatureId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- FEATURED LISTINGS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'FeaturedListings'
                )
                BEGIN
                    CREATE TABLE FeaturedListings
                    (
                        FeaturedListingId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        PropertyId INT NOT NULL,

                        FeaturedFee DECIMAL(18,2)
                        NOT NULL,

                        StartDate DATETIME
                        NOT NULL,

                        EndDate DATETIME
                        NOT NULL,

                        PaymentStatus NVARCHAR(50)
                        NOT NULL DEFAULT 'Pending',

                        Status NVARCHAR(50)
                        NOT NULL DEFAULT 'Active',

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_FeaturedListings_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- CARTS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Carts'
                )
                BEGIN
                    CREATE TABLE Carts
                    (
                        CartId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        CustomerId INT NOT NULL,

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        IsActive BIT
                        NOT NULL DEFAULT 1,

                        CONSTRAINT FK_Carts_Users
                        FOREIGN KEY (CustomerId)
                        REFERENCES Users(UserId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- CART ITEMS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'CartItems'
                )
                BEGIN
                    CREATE TABLE CartItems
                    (
                        CartItemId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        CartId INT NOT NULL,

                        PropertyId INT NOT NULL,

                        RentalMonths INT
                        NOT NULL DEFAULT 1,

                        OfferedPrice DECIMAL(18,2)
                        NULL,

                        AddedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_CartItems_Carts
                        FOREIGN KEY (CartId)
                        REFERENCES Carts(CartId)
                        ON DELETE CASCADE,

                        CONSTRAINT FK_CartItems_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId)
                    );
                END


                ------------------------------------------------
                -- ORDERS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Orders'
                )
                BEGIN
                    CREATE TABLE Orders
                    (
                        OrderId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        CustomerId INT NOT NULL,

                        OrderDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        TotalAmount DECIMAL(18,2)
                        NOT NULL,

                        OrderStatus NVARCHAR(50)
                        NOT NULL DEFAULT 'Completed',

                        TransactionType NVARCHAR(50)
                        NOT NULL DEFAULT 'Sale',

                        CONSTRAINT FK_Orders_Users
                        FOREIGN KEY (CustomerId)
                        REFERENCES Users(UserId)
                    );
                END


                ------------------------------------------------
                -- ORDER ITEMS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'OrderItems'
                )
                BEGIN
                    CREATE TABLE OrderItems
                    (
                        OrderItemId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        OrderId INT NOT NULL,

                        PropertyId INT NOT NULL,

                        OwnerId INT NOT NULL,

                        Quantity INT
                        NOT NULL DEFAULT 1,

                        RentalMonths INT
                        NOT NULL DEFAULT 0,

                        UnitPrice DECIMAL(18,2)
                        NOT NULL,

                        DiscountAmount DECIMAL(18,2)
                        NOT NULL DEFAULT 0,

                        FinalAmount DECIMAL(18,2)
                        NOT NULL,

                        CONSTRAINT FK_OrderItems_Orders
                        FOREIGN KEY (OrderId)
                        REFERENCES Orders(OrderId)
                        ON DELETE CASCADE,

                        CONSTRAINT FK_OrderItems_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId),

                        CONSTRAINT FK_OrderItems_Users
                        FOREIGN KEY (OwnerId)
                        REFERENCES Users(UserId)
                    );
                END


                ------------------------------------------------
                -- PAYMENTS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Payments'
                )
                BEGIN
                    CREATE TABLE Payments
                    (
                        PaymentId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        OrderId INT NOT NULL,

                        PaymentMethod NVARCHAR(50)
                        NOT NULL,

                        TransactionId NVARCHAR(100)
                        NOT NULL,

                        PaymentAmount DECIMAL(18,2)
                        NOT NULL,

                        PaymentStatus NVARCHAR(50)
                        NOT NULL DEFAULT 'Completed',

                        PaymentDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_Payments_Orders
                        FOREIGN KEY (OrderId)
                        REFERENCES Orders(OrderId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- INVOICES
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Invoices'
                )
                BEGIN
                    CREATE TABLE Invoices
                    (
                        InvoiceId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        OrderId INT NOT NULL,

                        PaymentId INT NOT NULL,

                        InvoiceNumber NVARCHAR(50)
                        NOT NULL UNIQUE,

                        SubTotal DECIMAL(18,2)
                        NOT NULL,

                        DiscountAmount DECIMAL(18,2)
                        NOT NULL DEFAULT 0,

                        CommissionAmount DECIMAL(18,2)
                        NOT NULL DEFAULT 0,

                        TotalAmount DECIMAL(18,2)
                        NOT NULL,

                        GeneratedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_Invoices_Orders
                        FOREIGN KEY (OrderId)
                        REFERENCES Orders(OrderId),

                        CONSTRAINT FK_Invoices_Payments
                        FOREIGN KEY (PaymentId)
                        REFERENCES Payments(PaymentId)
                    );
                END


                ------------------------------------------------
                -- COMMISSIONS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Commissions'
                )
                BEGIN
                    CREATE TABLE Commissions
                    (
                        CommissionId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        OrderId INT NOT NULL,

                        CommissionRate DECIMAL(5,2)
                        NOT NULL,

                        TransactionAmount DECIMAL(18,2)
                        NOT NULL,

                        CommissionAmount DECIMAL(18,2)
                        NOT NULL,

                        OwnerAmount DECIMAL(18,2)
                        NOT NULL,

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_Commissions_Orders
                        FOREIGN KEY (OrderId)
                        REFERENCES Orders(OrderId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- COMPLAINTS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Complaints'
                )
                BEGIN
                    CREATE TABLE Complaints
                    (
                        ComplaintId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        CustomerId INT NOT NULL,

                        PropertyId INT NULL,

                        Subject NVARCHAR(200)
                        NOT NULL,

                        ComplaintType NVARCHAR(50)
                        NOT NULL,

                        Description NVARCHAR(MAX)
                        NOT NULL,

                        Priority NVARCHAR(20)
                        NOT NULL DEFAULT 'Normal',

                        ComplaintStatus NVARCHAR(50)
                        NOT NULL DEFAULT 'Pending',

                        ResolvedBy INT NULL,

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        ResolvedDate DATETIME NULL,

                        CONSTRAINT FK_Complaints_Customer
                        FOREIGN KEY (CustomerId)
                        REFERENCES Users(UserId),

                        CONSTRAINT FK_Complaints_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId),

                        CONSTRAINT FK_Complaints_ResolvedBy
                        FOREIGN KEY (ResolvedBy)
                        REFERENCES Users(UserId)
                    );
                END


                ------------------------------------------------
                -- VISIT REQUESTS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'VisitRequests'
                )
                BEGIN
                    CREATE TABLE VisitRequests
                    (
                        VisitRequestId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        CustomerId INT NOT NULL,

                        PropertyId INT NOT NULL,

                        VisitDate DATE
                        NOT NULL,

                        VisitTime NVARCHAR(20)
                        NULL,

                        RequestStatus NVARCHAR(50)
                        NOT NULL DEFAULT 'Pending',

                        CustomerNote NVARCHAR(MAX)
                        NULL,

                        CreatedDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_VisitRequests_Users
                        FOREIGN KEY (CustomerId)
                        REFERENCES Users(UserId),

                        CONSTRAINT FK_VisitRequests_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId)
                        ON DELETE CASCADE
                    );
                END


                ------------------------------------------------
                -- REVIEWS
                ------------------------------------------------

                IF NOT EXISTS
                (
                    SELECT *
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Reviews'
                )
                BEGIN
                    CREATE TABLE Reviews
                    (
                        ReviewId INT IDENTITY(1,1)
                        PRIMARY KEY,

                        CustomerId INT NOT NULL,

                        PropertyId INT NOT NULL,

                        Rating INT
                        NOT NULL CHECK (Rating BETWEEN 1 AND 5),

                        ReviewComment NVARCHAR(MAX)
                        NULL,

                        ReviewStatus NVARCHAR(50)
                        NOT NULL DEFAULT 'Approved',

                        ReviewDate DATETIME
                        NOT NULL DEFAULT GETDATE(),

                        CONSTRAINT FK_Reviews_Users
                        FOREIGN KEY (CustomerId)
                        REFERENCES Users(UserId),

                        CONSTRAINT FK_Reviews_Properties
                        FOREIGN KEY (PropertyId)
                        REFERENCES Properties(PropertyId)
                        ON DELETE CASCADE
                    );
                END
            ";

            using (SqlConnection connection =
                   new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand command =
                       new SqlCommand(schema, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // =====================================================
        // MIGRATE EXISTING DATABASE
        // =====================================================

        private static void MigrateExistingSchema()
        {
            try
            {
                using (SqlConnection connection =
                       new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string migrationScript = @"

                        ------------------------------------------------
                        -- ENSURE USER COLUMNS
                        ------------------------------------------------

                        IF NOT EXISTS
                        (
                            SELECT *
                            FROM sys.columns
                            WHERE object_id = OBJECT_ID('Users')
                            AND name = 'RoleId'
                        )
                        BEGIN
                            ALTER TABLE Users
                            ADD RoleId INT NULL;
                        END


                        IF NOT EXISTS
                        (
                            SELECT *
                            FROM sys.columns
                            WHERE object_id = OBJECT_ID('Users')
                            AND name = 'PasswordHash'
                        )
                        BEGIN
                            ALTER TABLE Users
                            ADD PasswordHash NVARCHAR(256) NULL;
                        END


                        IF NOT EXISTS
                        (
                            SELECT *
                            FROM sys.columns
                            WHERE object_id = OBJECT_ID('Users')
                            AND name = 'IsActive'
                        )
                        BEGIN
                            ALTER TABLE Users
                            ADD IsActive BIT
                            NOT NULL DEFAULT 1;
                        END


                        IF NOT EXISTS
                        (
                            SELECT *
                            FROM sys.columns
                            WHERE object_id = OBJECT_ID('Users')
                            AND name = 'AccountStatus'
                        )
                        BEGIN
                            ALTER TABLE Users
                            ADD AccountStatus NVARCHAR(20)
                            NOT NULL DEFAULT 'Active';
                        END


                        IF NOT EXISTS
                        (
                            SELECT *
                            FROM sys.columns
                            WHERE object_id = OBJECT_ID('Users')
                            AND name = 'ProfileImagePath'
                        )
                        BEGIN
                            ALTER TABLE Users
                            ADD ProfileImagePath NVARCHAR(500)
                            NULL;
                        END


                        IF NOT EXISTS
                        (
                            SELECT *
                            FROM sys.columns
                            WHERE object_id = OBJECT_ID('Users')
                            AND name = 'CreatedDate'
                        )
                        BEGIN
                            ALTER TABLE Users
                            ADD CreatedDate DATETIME
                            NOT NULL DEFAULT GETDATE();
                        END


                        ------------------------------------------------
                        -- ENSURE ACCOUNT STATUS CONSTRAINT
                        ------------------------------------------------

                        IF EXISTS
                        (
                            SELECT 1
                            FROM sys.check_constraints
                            WHERE parent_object_id = OBJECT_ID('Users')
                            AND name = 'CHK_Users_AccountStatus'
                            AND definition NOT LIKE '%Active%'
                        )
                        BEGIN
                            ALTER TABLE Users DROP CONSTRAINT CHK_Users_AccountStatus;
                            ALTER TABLE Users WITH CHECK ADD CONSTRAINT CHK_Users_AccountStatus
                            CHECK (AccountStatus IN ('Active', 'Pending', 'Suspended', 'Inactive', 'Approved', 'Rejected'));
                        END
                        ELSE IF NOT EXISTS
                        (
                            SELECT 1
                            FROM sys.check_constraints
                            WHERE parent_object_id = OBJECT_ID('Users')
                            AND name = 'CHK_Users_AccountStatus'
                        )
                        BEGIN
                            ALTER TABLE Users WITH CHECK ADD CONSTRAINT CHK_Users_AccountStatus
                            CHECK (AccountStatus IN ('Active', 'Pending', 'Suspended', 'Inactive', 'Approved', 'Rejected'));
                        END


                        ------------------------------------------------
                        -- ENSURE EXISTING USERS ARE ACTIVE
                        ------------------------------------------------

                        UPDATE Users
                        SET IsActive = 1
                        WHERE IsActive IS NULL;

                        UPDATE Users
                        SET AccountStatus = 'Active'
                        WHERE AccountStatus IS NULL
                        OR AccountStatus = '';

                    ";

                    using (SqlCommand command =
                           new SqlCommand(migrationScript, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // Existing schema may already be compatible.
            }
        }

        // =====================================================
        // SEED DATA
        // =====================================================

        private static void EnsureSeedData()
        {
            try
            {
                using (SqlConnection connection =
                       new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    EnsureRoles(connection);

                    EnsureDefaultUsers(connection);

                    EnsureCategories(connection);

                    EnsureFeatures(connection);

                    EnsureProperties(connection);
                }
            }
            catch
            {
                // Database will still run if optional seed data fails.
            }
        }

        // =====================================================
        // ENSURE ROLES
        // =====================================================

        private static void EnsureRoles(SqlConnection connection)
        {
            string query = @"

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Roles
                    WHERE RoleName = 'Customer'
                )
                BEGIN
                    INSERT INTO Roles
                    (
                        RoleName,
                        RoleDescription
                    )
                    VALUES
                    (
                        'Customer',
                        'Can browse properties, request visits, write reviews, and submit complaints.'
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Roles
                    WHERE RoleName = 'Admin'
                )
                BEGIN
                    INSERT INTO Roles
                    (
                        RoleName,
                        RoleDescription
                    )
                    VALUES
                    (
                        'Admin',
                        'Property seller or owner who can list and manage properties.'
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Roles
                    WHERE RoleName = 'SuperAdmin'
                )
                BEGIN
                    INSERT INTO Roles
                    (
                        RoleName,
                        RoleDescription
                    )
                    VALUES
                    (
                        'SuperAdmin',
                        'System administrator with full access.'
                    );
                END
            ";

            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        // =====================================================
        // ENSURE DEFAULT USERS
        // =====================================================

        private static void EnsureDefaultUsers(SqlConnection connection)
        {
            string superAdminHash =
                PasswordHelper.HashPassword("admin123");

            string adminHash =
                PasswordHelper.HashPassword("seller123");

            string customerHash =
                PasswordHelper.HashPassword("customer123");


            //--------------------------------------------------
            // SUPER ADMIN
            //--------------------------------------------------

            string superAdminQuery = @"

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Users
                    WHERE Email = 'admin@estatenexus.com'
                )
                BEGIN

                    DECLARE @SuperAdminRoleId INT =
                    (
                        SELECT TOP 1 RoleId
                        FROM Roles
                        WHERE RoleName = 'SuperAdmin'
                    );

                    INSERT INTO Users
                    (
                        RoleId,
                        FullName,
                        Email,
                        Phone,
                        PasswordHash,
                        Address,
                        ProfileImagePath,
                        AccountStatus,
                        IsActive,
                        CreatedDate
                    )
                    VALUES
                    (
                        @SuperAdminRoleId,
                        'Super Admin',
                        'admin@estatenexus.com',
                        '01700000000',
                        @PasswordHash,
                        'EstateNexus HQ, Kuril, Dhaka',
                        NULL,
                        'Active',
                        1,
                        GETDATE()
                    );

                END
            ";

            using (SqlCommand command =
                   new SqlCommand(superAdminQuery, connection))
            {
                command.Parameters.AddWithValue(
                    "@PasswordHash",
                    superAdminHash);

                command.ExecuteNonQuery();
            }


            //--------------------------------------------------
            // DEFAULT ADMIN / SELLER
            //--------------------------------------------------

            string adminQuery = @"

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Users
                    WHERE Email = 'seller@estatenexus.com'
                )
                BEGIN

                    DECLARE @AdminRoleId INT =
                    (
                        SELECT TOP 1 RoleId
                        FROM Roles
                        WHERE RoleName = 'Admin'
                    );

                    INSERT INTO Users
                    (
                        RoleId,
                        FullName,
                        Email,
                        Phone,
                        PasswordHash,
                        Address,
                        ProfileImagePath,
                        AccountStatus,
                        IsActive,
                        CreatedDate
                    )
                    VALUES
                    (
                        @AdminRoleId,
                        'Property Seller',
                        'seller@estatenexus.com',
                        '01711111111',
                        @PasswordHash,
                        'Gulshan-2, Dhaka',
                        NULL,
                        'Active',
                        1,
                        GETDATE()
                    );

                END
            ";

            using (SqlCommand command =
                   new SqlCommand(adminQuery, connection))
            {
                command.Parameters.AddWithValue(
                    "@PasswordHash",
                    adminHash);

                command.ExecuteNonQuery();
            }


            //--------------------------------------------------
            // DEFAULT CUSTOMER
            //--------------------------------------------------

            string customerQuery = @"

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Users
                    WHERE Email = 'customer@estatenexus.com'
                )
                BEGIN

                    DECLARE @CustomerRoleId INT =
                    (
                        SELECT TOP 1 RoleId
                        FROM Roles
                        WHERE RoleName = 'Customer'
                    );

                    INSERT INTO Users
                    (
                        RoleId,
                        FullName,
                        Email,
                        Phone,
                        PasswordHash,
                        Address,
                        ProfileImagePath,
                        AccountStatus,
                        IsActive,
                        CreatedDate
                    )
                    VALUES
                    (
                        @CustomerRoleId,
                        'John Customer',
                        'customer@estatenexus.com',
                        '01722222222',
                        @PasswordHash,
                        'Banani, Dhaka',
                        NULL,
                        'Active',
                        1,
                        GETDATE()
                    );

                END
            ";

            using (SqlCommand command =
                   new SqlCommand(customerQuery, connection))
            {
                command.Parameters.AddWithValue(
                    "@PasswordHash",
                    customerHash);

                command.ExecuteNonQuery();
            }
        }

        // =====================================================
        // ENSURE CATEGORIES
        // =====================================================

        private static void EnsureCategories(SqlConnection connection)
        {
            string query = @"

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyCategories
                    WHERE CategoryName = 'Apartment'
                )
                BEGIN
                    INSERT INTO PropertyCategories
                    (
                        CategoryName,
                        Description,
                        IsActive
                    )
                    VALUES
                    (
                        'Apartment',
                        'Residential flats and luxury condominiums',
                        1
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyCategories
                    WHERE CategoryName = 'House'
                )
                BEGIN
                    INSERT INTO PropertyCategories
                    (
                        CategoryName,
                        Description,
                        IsActive
                    )
                    VALUES
                    (
                        'House',
                        'Independent houses, villas, and duplex homes',
                        1
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyCategories
                    WHERE CategoryName = 'Commercial'
                )
                BEGIN
                    INSERT INTO PropertyCategories
                    (
                        CategoryName,
                        Description,
                        IsActive
                    )
                    VALUES
                    (
                        'Commercial',
                        'Offices, shops, and commercial spaces',
                        1
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyCategories
                    WHERE CategoryName = 'Land'
                )
                BEGIN
                    INSERT INTO PropertyCategories
                    (
                        CategoryName,
                        Description,
                        IsActive
                    )
                    VALUES
                    (
                        'Land',
                        'Residential and commercial land',
                        1
                    );
                END
            ";

            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        // =====================================================
        // ENSURE FEATURES
        // =====================================================

        private static void EnsureFeatures(SqlConnection connection)
        {
            string query = @"

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyFeatures
                    WHERE FeatureName = 'Swimming Pool'
                )
                BEGIN
                    INSERT INTO PropertyFeatures
                    (FeatureName, Description)
                    VALUES
                    (
                        'Swimming Pool',
                        'Private or shared swimming pool facility'
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyFeatures
                    WHERE FeatureName = 'Elevator / Lift'
                )
                BEGIN
                    INSERT INTO PropertyFeatures
                    (FeatureName, Description)
                    VALUES
                    (
                        'Elevator / Lift',
                        'Passenger elevator facility'
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyFeatures
                    WHERE FeatureName = 'Car Parking'
                )
                BEGIN
                    INSERT INTO PropertyFeatures
                    (FeatureName, Description)
                    VALUES
                    (
                        'Car Parking',
                        'Dedicated parking space'
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyFeatures
                    WHERE FeatureName = '24/7 Security & CCTV'
                )
                BEGIN
                    INSERT INTO PropertyFeatures
                    (FeatureName, Description)
                    VALUES
                    (
                        '24/7 Security & CCTV',
                        'Security and CCTV surveillance'
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyFeatures
                    WHERE FeatureName = 'Backup Generator'
                )
                BEGIN
                    INSERT INTO PropertyFeatures
                    (FeatureName, Description)
                    VALUES
                    (
                        'Backup Generator',
                        'Electricity backup facility'
                    );
                END


                IF NOT EXISTS
                (
                    SELECT 1
                    FROM PropertyFeatures
                    WHERE FeatureName = 'Balcony / Terrace'
                )
                BEGIN
                    INSERT INTO PropertyFeatures
                    (FeatureName, Description)
                    VALUES
                    (
                        'Balcony / Terrace',
                        'Open balcony or private terrace'
                    );
                END
            ";

            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        // =====================================================
        // ENSURE SAMPLE PROPERTIES
        // =====================================================

        private static void EnsureProperties(SqlConnection connection)
        {
            string query = @"

                IF NOT EXISTS
                (
                    SELECT 1
                    FROM Properties
                )
                BEGIN

                    DECLARE @SellerId INT =
                    (
                        SELECT TOP 1 UserId
                        FROM Users
                        WHERE RoleId =
                        (
                            SELECT TOP 1 RoleId
                            FROM Roles
                            WHERE RoleName = 'Admin'
                        )
                    );

                    DECLARE @ApartmentId INT =
                    (
                        SELECT TOP 1 CategoryId
                        FROM PropertyCategories
                        WHERE CategoryName = 'Apartment'
                    );

                    DECLARE @HouseId INT =
                    (
                        SELECT TOP 1 CategoryId
                        FROM PropertyCategories
                        WHERE CategoryName = 'House'
                    );

                    DECLARE @CommercialId INT =
                    (
                        SELECT TOP 1 CategoryId
                        FROM PropertyCategories
                        WHERE CategoryName = 'Commercial'
                    );

                    DECLARE @LandId INT =
                    (
                        SELECT TOP 1 CategoryId
                        FROM PropertyCategories
                        WHERE CategoryName = 'Land'
                    );


                    INSERT INTO Properties
                    (
                        OwnerId,
                        CategoryId,
                        PropertyTitle,
                        ListingType,
                        District,
                        AreaLocation,
                        FullAddress,
                        AreaSize,
                        AreaUnit,
                        Bedrooms,
                        Bathrooms,
                        Price,
                        Description,
                        PropertyStatus,
                        ApprovalStatus,
                        IsFeatured,
                        CreatedDate
                    )
                    VALUES

                    (
                        @SellerId,
                        @ApartmentId,
                        'Luxury 3-BHK Apartment in Gulshan',
                        'Sale',
                        'Dhaka',
                        'Gulshan',
                        'Road 11, Block D, Gulshan-2, Dhaka',
                        2200.00,
                        'sqft',
                        3,
                        3,
                        25000000.00,
                        'Luxury apartment with lake view and modern facilities.',
                        'Available',
                        'Approved',
                        1,
                        GETDATE()
                    ),

                    (
                        @SellerId,
                        @HouseId,
                        'Modern Duplex Villa in Banani',
                        'Rent',
                        'Dhaka',
                        'Banani',
                        'Road 7, Block F, Banani, Dhaka',
                        3500.00,
                        'sqft',
                        4,
                        4,
                        120000.00,
                        'Modern duplex villa with private garden.',
                        'Available',
                        'Approved',
                        1,
                        GETDATE()
                    ),

                    (
                        @SellerId,
                        @CommercialId,
                        'Prime Commercial Office Space',
                        'Rent',
                        'Dhaka',
                        'Dhanmondi',
                        'Satmasjid Road, Dhanmondi, Dhaka',
                        1800.00,
                        'sqft',
                        0,
                        2,
                        85000.00,
                        'Commercial office suitable for corporate use.',
                        'Available',
                        'Approved',
                        0,
                        GETDATE()
                    ),

                    (
                        @SellerId,
                        @LandId,
                        'Residential Plot in Purbachal',
                        'Sale',
                        'Dhaka',
                        'Purbachal',
                        'Sector 4, Purbachal New Town',
                        3600.00,
                        'sqft',
                        0,
                        0,
                        9500000.00,
                        'Residential land ready for construction.',
                        'Available',
                        'Approved',
                        0,
                        GETDATE()
                    );

                END
            ";

            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}