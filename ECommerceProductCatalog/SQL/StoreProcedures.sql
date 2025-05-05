//This File have SP for Delete Products and GetFilteredProducts

CREATE PROCEDURE GetFilteredProducts
    @Name NVARCHAR(100) = NULL,
    @Price DECIMAL(18,2) = NULL,
    @CategoryId INT = NULL,
    @PageNo INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StartRow INT = (@PageNo - 1) * @PageSize;

    SELECT 
        p.ProductId,
        p.Name,
        p.Price,
        p.Description,
        p.CategoryId,
        p.SKU,
        c.Name AS CategoryName,

        (
            SELECT TOP 1 pi.ImagePath 
            FROM ProductImages pi 
            WHERE pi.ProductId = p.ProductId 
            ORDER BY pi.ImageId ASC
        ) AS FirstImagePath

    FROM Products p
    LEFT JOIN Categories c ON p.CategoryId = c.CategoryId
    WHERE 
        (@Name IS NULL OR p.Name LIKE '%' + @Name + '%') AND
        (@Price IS NULL OR p.Price = @Price) AND
        (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
    ORDER BY p.Name
    OFFSET @StartRow ROWS FETCH NEXT @PageSize ROWS ONLY;
END


CREATE PROCEDURE DeleteProductWithImages
    @ProductId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM ProductImages WHERE ProductId = @ProductId;
	
    DELETE FROM Products WHERE ProductId = @ProductId;
END
