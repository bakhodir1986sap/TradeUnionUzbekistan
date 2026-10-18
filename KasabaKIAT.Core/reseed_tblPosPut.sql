-- Reseed tblPos_Put with proper ID_Post links to tblPost headers
-- Generates one row per voucher number in each header's BeginNumber..EndNumber range

SET IDENTITY_INSERT [tblPos_Put] ON;
DELETE FROM [tblPos_Put];

DECLARE @PostID   bigint
DECLARE @BeginNum int
DECLARE @EndNum   int
DECLARE @DataPost datetime
DECLARE @ID_Pos   bigint
DECLARE @RowID    int = 11  -- start from 11 to match existing sequence

DECLARE cur CURSOR FOR
  SELECT ID, BeginNumber, EndNumber, Data_Post, ID_Pos
  FROM tblPost
  ORDER BY ID;

OPEN cur;
FETCH NEXT FROM cur INTO @PostID, @BeginNum, @EndNum, @DataPost, @ID_Pos;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @n int = @BeginNum;
    WHILE @n <= @EndNum
    BEGIN
        DECLARE @NumPut nvarchar(50) = N'ПУТ' + RIGHT('000' + CAST(@ID_Pos AS varchar), 3)
                                             + RIGHT('000' + CAST(@n AS varchar), 4);
        DECLARE @DataNach datetime = DATEADD(day, (@n % 15), @DataPost);
        DECLARE @DataKon  datetime = DATEADD(day, 20, @DataNach);
        DECLARE @Price    decimal(18,2) = 1000000.00 + ((@n % 5) * 200000.00);

        INSERT INTO [tblPos_Put]
          (ID, ID_San, ID_Sanat, Num_Put, DataNach, DataKon, Price,
           Mes, Tip, Status, UserId, DEPARTMENT_BASE, SMFIL, MODIFY_DATE,
           ID_Post, Symma_Put, DataSan, DataSanEnd, Data_Post, Departmentbase)
        VALUES
          (@RowID, @ID_Pos, @ID_Pos, @NumPut, @DataNach, @DataKon, @Price,
           N'Квартал', N'Взрослая', N'1', 1, N'01-001', N'1', @DataPost,
           @PostID, @Price, @DataNach, @DataKon, @DataPost, N'01-001');

        SET @RowID += 1;
        SET @n     += 1;
    END;

    FETCH NEXT FROM cur INTO @PostID, @BeginNum, @EndNum, @DataPost, @ID_Pos;
END;

CLOSE cur;
DEALLOCATE cur;

SET IDENTITY_INSERT [tblPos_Put] OFF;
GO
