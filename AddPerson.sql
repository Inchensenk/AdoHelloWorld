USE AdoExplanationDb
GO
CREATE PROCEDURE AddPerson
@name nvarchar(50),
@age int
AS
    INSERT INTO Users (Name, Age)
    VALUES (@name, @age)

    SELECT SCOPE_IDENTITY()
GO;

Select * FROM Users