CREATE TRIGGER trg_UpdateBookRating
ON Review
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Collect all affected BookIds from INSERTED and DELETED pseudo-tables
    DECLARE @AffectedBookIds TABLE (BookId INT);

    INSERT INTO @AffectedBookIds (BookId)
    SELECT BookId FROM inserted
    UNION
    SELECT BookId FROM deleted;

    -- Recalculate average rating for books with existing reviews
    UPDATE b
    SET b.Rating = r.AvgRating
    FROM Book b
    INNER JOIN (
        SELECT i.BookId, AVG(CAST(i.Rating AS float)) AS AvgRating
        FROM Review i
        WHERE i.BookId IN (SELECT BookId FROM @AffectedBookIds)
        GROUP BY i.BookId
    ) r ON b.Id = r.BookId;

    -- Set rating to 0 for books that now have no reviews
    UPDATE b
    SET b.Rating = 0
    FROM Book b
    WHERE b.Id IN (SELECT BookId FROM @AffectedBookIds)
      AND NOT EXISTS (SELECT 1 FROM Review r WHERE r.BookId = b.Id);
END;
