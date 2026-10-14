-- Run this script once on the KasabaKIAT database before starting the API
-- This is the only schema change required to the existing database

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'api_refresh_tokens')
BEGIN
    CREATE TABLE api_refresh_tokens (
        id          BIGINT IDENTITY(1,1) PRIMARY KEY,
        user_id     BIGINT NOT NULL,
        token       NVARCHAR(256) NOT NULL UNIQUE,
        expires_at  DATETIME2 NOT NULL,
        created_at  DATETIME2 NOT NULL DEFAULT GETDATE(),
        revoked_at  DATETIME2 NULL
    );

    CREATE INDEX IX_api_refresh_tokens_token ON api_refresh_tokens(token);
    CREATE INDEX IX_api_refresh_tokens_user_id ON api_refresh_tokens(user_id);

    PRINT 'api_refresh_tokens table created successfully';
END
ELSE
BEGIN
    PRINT 'api_refresh_tokens table already exists';
END
