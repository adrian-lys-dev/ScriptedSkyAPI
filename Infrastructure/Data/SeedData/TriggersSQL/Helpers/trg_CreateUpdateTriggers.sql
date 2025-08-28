SET NOCOUNT ON;

DECLARE @TableName NVARCHAR(255);
DECLARE @TriggerName NVARCHAR(255);

DECLARE cur CURSOR FOR
SELECT t.name
FROM sys.tables t
JOIN sys.columns c ON t.object_id = c.object_id
WHERE c.name = 'UpdatedAt'
  AND EXISTS (
      SELECT 1 
      FROM sys.columns c2
      WHERE c2.object_id = t.object_id
        AND c2.name = 'Id'
  );

OPEN cur;
FETCH NEXT FROM cur INTO @TableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @TriggerName = 'TR_' + @TableName + '_UpdateUpdatedAt';

    PRINT '-- ==============================================';
    PRINT '-- Trigger for table: ' + @TableName;
    PRINT '-- Save as file: ' + @TriggerName + '.sql';
    PRINT 'CREATE TRIGGER ' + @TriggerName;
    PRINT 'ON dbo.[' + @TableName + ']';
    PRINT 'AFTER UPDATE';
    PRINT 'AS';
    PRINT 'BEGIN';
    PRINT '    SET NOCOUNT ON;';
    PRINT '';
    PRINT '    UPDATE t';
    PRINT '    SET UpdatedAt = GETUTCDATE()';
    PRINT '    FROM dbo.[' + @TableName + '] t';
    PRINT '    INNER JOIN Inserted i ON t.Id = i.Id;';
    PRINT 'END';
    PRINT 'GO';
    PRINT '';
    
    FETCH NEXT FROM cur INTO @TableName;
END

CLOSE cur;
DEALLOCATE cur;
