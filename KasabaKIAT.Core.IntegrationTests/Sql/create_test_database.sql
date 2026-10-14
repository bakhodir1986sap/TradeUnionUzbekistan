-- Reference SQL for manual setup if needed
-- DatabaseFixture.cs creates the schema via EF Core EnsureCreated()
-- and creates uspLogin via ExecuteSqlRawAsync

-- To manually run:
-- 1. CREATE DATABASE KasabaKIAT_Test
-- 2. Run the main project's create_database.sql against KasabaKIAT_Test
-- 3. Run create_refresh_tokens_table.sql against KasabaKIAT_Test
-- 4. Run the uspLogin snippet below

USE [KasabaKIAT_Test];
GO

CREATE OR ALTER PROCEDURE [dbo].[uspLogin]
    @login  NVARCHAR(256),
    @parol  NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    -- Test-only implementation: plain-text password match
    -- Production uses salted hashes; this proc is only for the isolated test DB
    SELECT TOP 1 id
    FROM   users
    WHERE  user_name      = @login
      AND  [password]     = @parol
      AND  [state]        = '1'
      AND  account_locked  = 0
      AND  account_expired = 0;
END
GO
