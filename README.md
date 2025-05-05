# Product Management System

This project is a basic Product Management System that allows you to view, filter, edit, and delete products. Product images are managed and stored in the `wwwroot` folder.

---

## Database Setup

1.  **Navigate to the project directory** and locate the SQL script file containing the database setup scripts.

2.  **Run the script** to create the necessary tables:
   As there are currently no user interface screens for adding new categories or products. Please use the provided SQL `INSERT` queries to manage the initial data.

    ```sql
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
    ```

4.  **Insert sample data** into the tables using the following SQL:

    ```sql
    INSERT INTO Categories (Name)
    VALUES
        ('Mobiles'),
        ('Laptops'),
        ('Smart LED'),
        ('Keyboards'),
        ('Hard disks');

    INSERT INTO Products (Name, SKU, Description, Price, CategoryId)
    VALUES
        ('IPhone 13', '100', 'Latest model of IPhone', 150000.00, 1),
        ('IPhone 11', '101', 'Previous model of IPhone', 120000.00, 1),
        ('ThinkPad', '103', 'Latest model of Laptop', 250000.00, 2),
        ('Eco Star', '102', 'Previous model of LCD', 100000.00, 3);
    ```

---

## Configuration

Set your database connection string within the `appsettings.json` file. Ensure that the `wwwroot` folder exists in your project's root directory, as this is where product images will be stored.

---

## Functionality

* **Edit Product**: Modify product details and attach an image. After saving, the uploaded image will be displayed on the main product listing page.
* **Delete Product**: Remove products from the product list view.
* **View Products**: Display all products on the main page, including their Name, SKU, Description, Price, and Category.
* **Filter Products**: Filter the displayed products based on:
    * Name
    * Price Range
    * Category

---

## Image Handling

*  Uploaded product images are saved in the `wwwroot` directory. If images are not immediately visible for existing products, you may need to edit the product and manually upload an image.

* Important Note: Since this project runs locally, image URLs are constructed using your local host address.

For example:
*  If you're running the project at https://localhost:7274, image URLs will be prefixed with that base address.

⚠️ Make sure to replace https://localhost:7274 with your own Base URL when deploying or testing on a different environment.

---

## Notes

* There are currently no user interface screens for adding new categories or products. Please use the provided SQL `INSERT` queries to manage the initial data.
* Product images can only be added or updated through the "Edit Product" functionality.
