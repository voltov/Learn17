CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX),
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(40) NOT NULL,
    CategoryID INT NOT NULL,
    QuantityPerUnit NVARCHAR(20),
    UnitPrice DECIMAL(18, 2) CHECK (UnitPrice >= 0),
    UnitsInStock SMALLINT CHECK (UnitsInStock >= 0),
    UnitsOnOrder SMALLINT CHECK (UnitsOnOrder >= 0),
    ReorderLevel SMALLINT CHECK (ReorderLevel >= 0),
    Discontinued BIT NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);
