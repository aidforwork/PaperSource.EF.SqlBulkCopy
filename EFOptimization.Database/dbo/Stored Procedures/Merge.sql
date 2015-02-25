
CREATE PROCEDURE [Merge]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE INTO [Order] AS [Target]
    USING [Order_TEMP] AS [Source]
     ON Target.Id = Source.Id
    WHEN MATCHED THEN
     UPDATE SET 
     Target.Date = Source.Date, 
     Target.Number = Source.Number,
     Target.Text = Source.Text
    WHEN NOT MATCHED THEN
    INSERT 
           (Date, Number, Text) 
    VALUES 
           (Source.Date, Source.Number, Source.Text);
END