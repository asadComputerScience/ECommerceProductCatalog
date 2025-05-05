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