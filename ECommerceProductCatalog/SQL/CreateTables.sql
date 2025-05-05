CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    SKU NVARCHAR(50) NOT NULL UNIQUE,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

CREATE TABLE ProductImages (
    ImageId INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    ImagePath NVARCHAR(350) NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);