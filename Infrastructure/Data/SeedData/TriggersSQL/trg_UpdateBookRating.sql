CREATE TRIGGER trg_UpdateBookRating
ON Review
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE b
    SET b.Rating = r.AvgRating
    FROM Book b
    INNER JOIN (
        SELECT i.BookId, AVG(CAST(i.Rating AS float)) AS AvgRating
        FROM Review i
        GROUP BY i.BookId
    ) r ON b.Id = r.BookId
    WHERE b.Id IN (SELECT DISTINCT BookId FROM inserted);
END
