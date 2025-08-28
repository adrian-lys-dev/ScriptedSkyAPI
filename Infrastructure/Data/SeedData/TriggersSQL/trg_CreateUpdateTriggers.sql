-- ==============================================
-- Trigger for table: Review
-- Save as file: TR_Review_UpdateUpdatedAt.sql
CREATE TRIGGER TR_Review_UpdateUpdatedAt
ON dbo.[Review]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[Review] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO
 
-- ==============================================
-- Trigger for table: Author
-- Save as file: TR_Author_UpdateUpdatedAt.sql
CREATE TRIGGER TR_Author_UpdateUpdatedAt
ON dbo.[Author]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[Author] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO
 
-- ==============================================
-- Trigger for table: Avatar
-- Save as file: TR_Avatar_UpdateUpdatedAt.sql
CREATE TRIGGER TR_Avatar_UpdateUpdatedAt
ON dbo.[Avatar]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[Avatar] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO
 
-- ==============================================
-- Trigger for table: DeliveryMethod
-- Save as file: TR_DeliveryMethod_UpdateUpdatedAt.sql
CREATE TRIGGER TR_DeliveryMethod_UpdateUpdatedAt
ON dbo.[DeliveryMethod]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[DeliveryMethod] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO
 
-- ==============================================
-- Trigger for table: Genre
-- Save as file: TR_Genre_UpdateUpdatedAt.sql
CREATE TRIGGER TR_Genre_UpdateUpdatedAt
ON dbo.[Genre]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[Genre] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO
 
-- ==============================================
-- Trigger for table: Publisher
-- Save as file: TR_Publisher_UpdateUpdatedAt.sql
CREATE TRIGGER TR_Publisher_UpdateUpdatedAt
ON dbo.[Publisher]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[Publisher] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO
 
-- ==============================================
-- Trigger for table: Book
-- Save as file: TR_Book_UpdateUpdatedAt.sql
CREATE TRIGGER TR_Book_UpdateUpdatedAt
ON dbo.[Book]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[Book] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO
 
-- ==============================================
-- Trigger for table: Order
-- Save as file: TR_Order_UpdateUpdatedAt.sql
CREATE TRIGGER TR_Order_UpdateUpdatedAt
ON dbo.[Order]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
 
    UPDATE t
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.[Order] t
    INNER JOIN Inserted i ON t.Id = i.Id;
END
GO