-- ==============================================================
-- Script tạo cơ sở dữ liệu BookStore cho bài học Entity Framework Core
-- Môn: Lập Trình Web 1 - K65 CNT - Giảng viên: Thầy Trịnh Văn Chung
-- ==============================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'BookStore')
BEGIN
    CREATE DATABASE BookStore;
END
GO

USE BookStore;
GO

-- Xóa bảng nếu đã tồn tại để tránh xung đột khóa ngoại
IF OBJECT_ID('dbo.OrderDetails', 'U') IS NOT NULL DROP TABLE dbo.OrderDetails;
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL DROP TABLE dbo.Books;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
IF OBJECT_ID('dbo.Publishers', 'U') IS NOT NULL DROP TABLE dbo.Publishers;
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL DROP TABLE dbo.Categories;
GO

-- 1. Bảng Loại sách (Categories)
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

-- 2. Bảng Nhà xuất bản (Publishers)
CREATE TABLE Publishers (
    PublisherId INT IDENTITY(1,1) PRIMARY KEY,
    PublisherName NVARCHAR(150) NOT NULL,
    Address NVARCHAR(255) NULL,
    Phone NVARCHAR(20) NULL
);
GO

-- 3. Bảng Sách (Books)
CREATE TABLE Books (
    BookId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NULL,
    Price DECIMAL(18,2) NOT NULL DEFAULT 0,
    Quantity INT NOT NULL DEFAULT 0,
    CategoryId INT NOT NULL,
    PublisherId INT NOT NULL,
    ImageUrl NVARCHAR(255) NULL,
    Description NVARCHAR(MAX) NULL,
    CONSTRAINT FK_Books_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId) ON DELETE CASCADE,
    CONSTRAINT FK_Books_Publishers FOREIGN KEY (PublisherId) REFERENCES Publishers(PublisherId) ON DELETE CASCADE
);
GO

-- 4. Bảng Khách hàng (Customers)
CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    Address NVARCHAR(255) NULL
);
GO

-- 5. Bảng Hóa đơn bán / Đơn hàng (Orders)
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    CustomerId INT NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    Status NVARCHAR(50) NOT NULL DEFAULT N'Đang xử lý',
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId) ON DELETE CASCADE
);
GO

-- 6. Bảng Chi tiết hóa đơn bán (OrderDetails)
CREATE TABLE OrderDetails (
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderDetails_Books FOREIGN KEY (BookId) REFERENCES Books(BookId)
);
GO

-- ==============================================================
-- Dữ liệu mẫu (Seed Data)
-- ==============================================================

SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (CategoryId, CategoryName, Description) VALUES
(1, N'Công nghệ thông tin', N'Sách về lập trình, trí tuệ nhân tạo, an toàn thông tin'),
(2, N'Kinh tế & Quản trị', N'Sách kinh doanh, quản trị doanh nghiệp, tài chính'),
(3, N'Văn học & Nghệ thuật', N'Tiểu thuyết, truyện ngắn, tác phẩm kinh điển'),
(4, N'Kỹ năng sống', N'Phát triển bản thân, tư duy lãnh đạo, quản lý thời gian');
SET IDENTITY_INSERT Categories OFF;
GO

SET IDENTITY_INSERT Publishers ON;
INSERT INTO Publishers (PublisherId, PublisherName, Address, Phone) VALUES
(1, N'NXB Giáo Dục', N'81 Trần Hưng Đạo, Hà Nội', N'02438220801'),
(2, N'NXB Trẻ', N'161B Lý Chính Thắng, TP.HCM', N'02839316289'),
(3, N'NXB Kim Đồng', N'55 Quang Trung, Hà Nội', N'02439434730'),
(4, N'NXB Thông Tin và Truyền Thông', N'115 Trần Duy Hưng, Hà Nội', N'02435772138');
SET IDENTITY_INSERT Publishers OFF;
GO

SET IDENTITY_INSERT Books ON;
INSERT INTO Books (BookId, Title, Author, Price, Quantity, CategoryId, PublisherId, ImageUrl, Description) VALUES
(1, N'Lập trình Web với ASP.NET Core & EF Core', N'Trịnh Văn Chung', 125000, 50, 1, 1, N'/images/aspnetcore.jpg', N'Giáo trình bài bản hướng dẫn xây dựng ứng dụng web hiện đại với ASP.NET Core và EF Core.'),
(2, N'Cấu trúc dữ liệu và giải thuật nâng cao', N'Nguyễn Đức Nghĩa', 95000, 30, 1, 1, N'/images/dsa.jpg', N'Sách kinh điển về thuật toán và cấu trúc dữ liệu cho sinh viên ngành CNTT.'),
(3, N'Đắc Nhân Tâm', N'Dale Carnegie', 88000, 100, 4, 2, N'/images/dacnhantam.jpg', N'Nghệ thuật thu phục lòng người và dẫn lối thành công.'),
(4, N'Nhà Giả Kim', N'Paulo Coelho', 79000, 65, 3, 2, N'/images/nhagiakim.jpg', N'Cuốn sách truyền cảm hứng cho hàng triệu bạn trẻ theo đuổi ước mơ.'),
(5, N'Thiết kế cơ sở dữ liệu quan hệ và NoSQL', N'Lê Minh Hoàng', 110000, 40, 1, 4, N'/images/database.jpg', N'Hướng dẫn phân tích thiết kế CSDL thực tế trong doanh nghiệp.');
SET IDENTITY_INSERT Books OFF;
GO

SET IDENTITY_INSERT Customers ON;
INSERT INTO Customers (CustomerId, FullName, Email, Phone, Address) VALUES
(1, N'Nguyễn Đình Sơn', N'dinhson@gmail.com', N'0912345678', N'Cầu Giấy, Hà Nội'),
(2, N'Trần Huy', N'tranhuy@gmail.com', N'0987654321', N'Đống Đa, Hà Nội'),
(3, N'Đào Ngọc Diệp', N'ngocdiep@gmail.com', N'0905123456', N'Hai Bà Trưng, Hà Nội');
SET IDENTITY_INSERT Customers OFF;
GO

SET IDENTITY_INSERT Orders ON;
INSERT INTO Orders (OrderId, OrderDate, CustomerId, TotalAmount, Status) VALUES
(1, '2026-10-01 10:30:00', 1, 220000, N'Đã hoàn thành'),
(2, '2026-10-04 15:45:00', 2, 88000, N'Đang giao hàng');
SET IDENTITY_INSERT Orders OFF;
GO

SET IDENTITY_INSERT OrderDetails ON;
INSERT INTO OrderDetails (OrderDetailId, OrderId, BookId, Quantity, UnitPrice) VALUES
(1, 1, 1, 1, 125000),
(2, 1, 2, 1, 95000),
(3, 2, 3, 1, 88000);
SET IDENTITY_INSERT OrderDetails OFF;
GO
