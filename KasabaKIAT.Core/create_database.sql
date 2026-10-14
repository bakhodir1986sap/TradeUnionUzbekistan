-- ============================================================
-- KasabaKIAT Database Creation Script
-- Generated from EF Core models (KasabaKIAT.Core)
-- Server: W-PF36JE4W\SQLEXPRESS
-- ============================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'KasabaKIAT')
BEGIN
    CREATE DATABASE [KasabaKIAT];
    PRINT 'Database KasabaKIAT created.';
END
ELSE
BEGIN
    PRINT 'Database KasabaKIAT already exists.';
END
GO

USE [KasabaKIAT];
GO

-- ============================================================
-- DROP FK CONSTRAINTS (in reverse dependency order)
-- ============================================================

IF OBJECT_ID('FK_users_roles', 'F') IS NOT NULL
    ALTER TABLE [users] DROP CONSTRAINT FK_users_roles;
IF OBJECT_ID('FK_role_permission_roles', 'F') IS NOT NULL
    ALTER TABLE [role_permission] DROP CONSTRAINT FK_role_permission_roles;
IF OBJECT_ID('FK_role_permission_functionality', 'F') IS NOT NULL
    ALTER TABLE [role_permission] DROP CONSTRAINT FK_role_permission_functionality;
IF OBJECT_ID('FK_api_refresh_tokens_users', 'F') IS NOT NULL
    ALTER TABLE [api_refresh_tokens] DROP CONSTRAINT FK_api_refresh_tokens_users;
IF OBJECT_ID('FK_useractions_users', 'F') IS NOT NULL
    ALTER TABLE [useractions] DROP CONSTRAINT FK_useractions_users;
IF OBJECT_ID('FK_tbpt_putyovki_tblSprav_San', 'F') IS NOT NULL
    ALTER TABLE [tbpt_putyovki] DROP CONSTRAINT FK_tbpt_putyovki_tblSprav_San;
IF OBJECT_ID('FK_tbpt_putyovki_users', 'F') IS NOT NULL
    ALTER TABLE [tbpt_putyovki] DROP CONSTRAINT FK_tbpt_putyovki_users;
IF OBJECT_ID('FK_tbpt_zaezdi_tblSprav_San', 'F') IS NOT NULL
    ALTER TABLE [tbpt_zaezdi] DROP CONSTRAINT FK_tbpt_zaezdi_tblSprav_San;
IF OBJECT_ID('FK_tbpt_yearlyplanregion_users', 'F') IS NOT NULL
    ALTER TABLE [tbpt_yearlyplanregion] DROP CONSTRAINT FK_tbpt_yearlyplanregion_users;
IF OBJECT_ID('FK_tbpt_yearlyplanotrasl_users', 'F') IS NOT NULL
    ALTER TABLE [tbpt_yearlyplanotrasl] DROP CONSTRAINT FK_tbpt_yearlyplanotrasl_users;
IF OBJECT_ID('FK_tbpt_avarageprice_users', 'F') IS NOT NULL
    ALTER TABLE [tbpt_avarageprice] DROP CONSTRAINT FK_tbpt_avarageprice_users;
IF OBJECT_ID('FK_t_egrpo_sp_okonx', 'F') IS NOT NULL
    ALTER TABLE [t_egrpo] DROP CONSTRAINT FK_t_egrpo_sp_okonx;
IF OBJECT_ID('FK_t_egrpo_sp_opf', 'F') IS NOT NULL
    ALTER TABLE [t_egrpo] DROP CONSTRAINT FK_t_egrpo_sp_opf;
IF OBJECT_ID('FK_t_sanatorium_sp_sanatorium_type', 'F') IS NOT NULL
    ALTER TABLE [t_sanatorium] DROP CONSTRAINT FK_t_sanatorium_sp_sanatorium_type;
IF OBJECT_ID('FK_tblBrons_tblSprav_San', 'F') IS NOT NULL
    ALTER TABLE [tblBrons] DROP CONSTRAINT FK_tblBrons_tblSprav_San;
IF OBJECT_ID('FK_tblBrons_users', 'F') IS NOT NULL
    ALTER TABLE [tblBrons] DROP CONSTRAINT FK_tblBrons_users;
IF OBJECT_ID('FK_tblPos_Put_tblSprav_San', 'F') IS NOT NULL
    ALTER TABLE [tblPos_Put] DROP CONSTRAINT FK_tblPos_Put_tblSprav_San;
IF OBJECT_ID('FK_tblPos_Put_tblPost', 'F') IS NOT NULL
    ALTER TABLE [tblPos_Put] DROP CONSTRAINT FK_tblPos_Put_tblPost;
IF OBJECT_ID('FK_tblPos_Put_users', 'F') IS NOT NULL
    ALTER TABLE [tblPos_Put] DROP CONSTRAINT FK_tblPos_Put_users;
IF OBJECT_ID('FK_tblPost_tblSprav_Org', 'F') IS NOT NULL
    ALTER TABLE [tblPost] DROP CONSTRAINT FK_tblPost_tblSprav_Org;
IF OBJECT_ID('FK_tblPost_tblSprav_Kv', 'F') IS NOT NULL
    ALTER TABLE [tblPost] DROP CONSTRAINT FK_tblPost_tblSprav_Kv;
IF OBJECT_ID('FK_tblPost_users', 'F') IS NOT NULL
    ALTER TABLE [tblPost] DROP CONSTRAINT FK_tblPost_users;
IF OBJECT_ID('FK_tblVipl_tblSprav_Org', 'F') IS NOT NULL
    ALTER TABLE [tblVipl] DROP CONSTRAINT FK_tblVipl_tblSprav_Org;
IF OBJECT_ID('FK_tblVipl_tblSprav_Kv', 'F') IS NOT NULL
    ALTER TABLE [tblVipl] DROP CONSTRAINT FK_tblVipl_tblSprav_Kv;
IF OBJECT_ID('FK_tblVipl_users', 'F') IS NOT NULL
    ALTER TABLE [tblVipl] DROP CONSTRAINT FK_tblVipl_users;
IF OBJECT_ID('FK_tblViplF_tblSprav_Org', 'F') IS NOT NULL
    ALTER TABLE [tblViplF] DROP CONSTRAINT FK_tblViplF_tblSprav_Org;
IF OBJECT_ID('FK_tblVipl_Put_tblVipl', 'F') IS NOT NULL
    ALTER TABLE [tblVipl_Put] DROP CONSTRAINT FK_tblVipl_Put_tblVipl;
IF OBJECT_ID('FK_tblVipl_Put_tblSprav_San', 'F') IS NOT NULL
    ALTER TABLE [tblVipl_Put] DROP CONSTRAINT FK_tblVipl_Put_tblSprav_San;
IF OBJECT_ID('FK_tblOplata_tblSprav_Otras', 'F') IS NOT NULL
    ALTER TABLE [tblOplata] DROP CONSTRAINT FK_tblOplata_tblSprav_Otras;
IF OBJECT_ID('FK_tblOplata_tblSprav_Kv', 'F') IS NOT NULL
    ALTER TABLE [tblOplata] DROP CONSTRAINT FK_tblOplata_tblSprav_Kv;
IF OBJECT_ID('FK_tblOplata_users', 'F') IS NOT NULL
    ALTER TABLE [tblOplata] DROP CONSTRAINT FK_tblOplata_users;
IF OBJECT_ID('FK_tblOplata_Rasxod_users', 'F') IS NOT NULL
    ALTER TABLE [tblOplata_Rasxod] DROP CONSTRAINT FK_tblOplata_Rasxod_users;
IF OBJECT_ID('FK_tblVozvrat_tblSprav_San', 'F') IS NOT NULL
    ALTER TABLE [tblVozvrat] DROP CONSTRAINT FK_tblVozvrat_tblSprav_San;
IF OBJECT_ID('FK_tblVozvrat_tblSprav_Org', 'F') IS NOT NULL
    ALTER TABLE [tblVozvrat] DROP CONSTRAINT FK_tblVozvrat_tblSprav_Org;
IF OBJECT_ID('FK_tblVozvrat_tblPost', 'F') IS NOT NULL
    ALTER TABLE [tblVozvrat] DROP CONSTRAINT FK_tblVozvrat_tblPost;
IF OBJECT_ID('FK_tblVozvrat_tblVipl', 'F') IS NOT NULL
    ALTER TABLE [tblVozvrat] DROP CONSTRAINT FK_tblVozvrat_tblVipl;
IF OBJECT_ID('FK_tblZakritiye_users', 'F') IS NOT NULL
    ALTER TABLE [tblZakritiye] DROP CONSTRAINT FK_tblZakritiye_users;
IF OBJECT_ID('FK_tbcs_dictionary_tbcs_dictionarytype', 'F') IS NOT NULL
    ALTER TABLE [tbcs_dictionary] DROP CONSTRAINT FK_tbcs_dictionary_tbcs_dictionarytype;
GO

-- ============================================================
-- DROP TABLES (in reverse dependency order)
-- ============================================================

IF OBJECT_ID('[tblVozvrat]', 'U') IS NOT NULL DROP TABLE [tblVozvrat];
IF OBJECT_ID('[tblVipl_Put]', 'U') IS NOT NULL DROP TABLE [tblVipl_Put];
IF OBJECT_ID('[tblViplF]', 'U') IS NOT NULL DROP TABLE [tblViplF];
IF OBJECT_ID('[tblVipl]', 'U') IS NOT NULL DROP TABLE [tblVipl];
IF OBJECT_ID('[tblOplata_Rasxod]', 'U') IS NOT NULL DROP TABLE [tblOplata_Rasxod];
IF OBJECT_ID('[tblOplata]', 'U') IS NOT NULL DROP TABLE [tblOplata];
IF OBJECT_ID('[tblPos_Put]', 'U') IS NOT NULL DROP TABLE [tblPos_Put];
IF OBJECT_ID('[tblPost]', 'U') IS NOT NULL DROP TABLE [tblPost];
IF OBJECT_ID('[tblZakritiye]', 'U') IS NOT NULL DROP TABLE [tblZakritiye];
IF OBJECT_ID('[tblBrons]', 'U') IS NOT NULL DROP TABLE [tblBrons];
IF OBJECT_ID('[tbpt_avarageprice]', 'U') IS NOT NULL DROP TABLE [tbpt_avarageprice];
IF OBJECT_ID('[tbpt_yearlyplanotrasl]', 'U') IS NOT NULL DROP TABLE [tbpt_yearlyplanotrasl];
IF OBJECT_ID('[tbpt_yearlyplanregion]', 'U') IS NOT NULL DROP TABLE [tbpt_yearlyplanregion];
IF OBJECT_ID('[tbpt_zaezdi]', 'U') IS NOT NULL DROP TABLE [tbpt_zaezdi];
IF OBJECT_ID('[tbpt_putyovki]', 'U') IS NOT NULL DROP TABLE [tbpt_putyovki];
IF OBJECT_ID('[api_refresh_tokens]', 'U') IS NOT NULL DROP TABLE [api_refresh_tokens];
IF OBJECT_ID('[useractions]', 'U') IS NOT NULL DROP TABLE [useractions];
IF OBJECT_ID('[role_permission]', 'U') IS NOT NULL DROP TABLE [role_permission];
IF OBJECT_ID('[users]', 'U') IS NOT NULL DROP TABLE [users];
IF OBJECT_ID('[roles]', 'U') IS NOT NULL DROP TABLE [roles];
IF OBJECT_ID('[functionality]', 'U') IS NOT NULL DROP TABLE [functionality];
IF OBJECT_ID('[t_egrpo]', 'U') IS NOT NULL DROP TABLE [t_egrpo];
IF OBJECT_ID('[t_sanatorium]', 'U') IS NOT NULL DROP TABLE [t_sanatorium];
IF OBJECT_ID('[t_normativdoc]', 'U') IS NOT NULL DROP TABLE [t_normativdoc];
IF OBJECT_ID('[t_departmentbase]', 'U') IS NOT NULL DROP TABLE [t_departmentbase];
IF OBJECT_ID('[t_camp]', 'U') IS NOT NULL DROP TABLE [t_camp];
IF OBJECT_ID('[sp_employees]', 'U') IS NOT NULL DROP TABLE [sp_employees];
IF OBJECT_ID('[sp_soogu]', 'U') IS NOT NULL DROP TABLE [sp_soogu];
IF OBJECT_ID('[sp_okonx]', 'U') IS NOT NULL DROP TABLE [sp_okonx];
IF OBJECT_ID('[sp_opf]', 'U') IS NOT NULL DROP TABLE [sp_opf];
IF OBJECT_ID('[sp_section]', 'U') IS NOT NULL DROP TABLE [sp_section];
IF OBJECT_ID('[sp_departmenttype]', 'U') IS NOT NULL DROP TABLE [sp_departmenttype];
IF OBJECT_ID('[sp_contracttype]', 'U') IS NOT NULL DROP TABLE [sp_contracttype];
IF OBJECT_ID('[sp_degree]', 'U') IS NOT NULL DROP TABLE [sp_degree];
IF OBJECT_ID('[sp_nation]', 'U') IS NOT NULL DROP TABLE [sp_nation];
IF OBJECT_ID('[sp_position]', 'U') IS NOT NULL DROP TABLE [sp_position];
IF OBJECT_ID('[sp_country]', 'U') IS NOT NULL DROP TABLE [sp_country];
IF OBJECT_ID('[sp_branch]', 'U') IS NOT NULL DROP TABLE [sp_branch];
IF OBJECT_ID('[sp_soato]', 'U') IS NOT NULL DROP TABLE [sp_soato];
IF OBJECT_ID('[sp_sanatorium_profil]', 'U') IS NOT NULL DROP TABLE [sp_sanatorium_profil];
IF OBJECT_ID('[sp_sanatorium_type]', 'U') IS NOT NULL DROP TABLE [sp_sanatorium_type];
IF OBJECT_ID('[tbcs_dictionary]', 'U') IS NOT NULL DROP TABLE [tbcs_dictionary];
IF OBJECT_ID('[tbcs_dictionarytype]', 'U') IS NOT NULL DROP TABLE [tbcs_dictionarytype];
IF OBJECT_ID('[tblSprav_Kv]', 'U') IS NOT NULL DROP TABLE [tblSprav_Kv];
IF OBJECT_ID('[tblSprav_San]', 'U') IS NOT NULL DROP TABLE [tblSprav_San];
IF OBJECT_ID('[tblSprav_Org]', 'U') IS NOT NULL DROP TABLE [tblSprav_Org];
IF OBJECT_ID('[tblSprav_Otras]', 'U') IS NOT NULL DROP TABLE [tblSprav_Otras];
IF OBJECT_ID('[tblSprav_Reg]', 'U') IS NOT NULL DROP TABLE [tblSprav_Reg];
IF OBJECT_ID('[tblSprav_Vil]', 'U') IS NOT NULL DROP TABLE [tblSprav_Vil];
IF OBJECT_ID('[tblSprav_MFO]', 'U') IS NOT NULL DROP TABLE [tblSprav_MFO];
IF OBJECT_ID('[tblSprav_Face]', 'U') IS NOT NULL DROP TABLE [tblSprav_Face];
IF OBJECT_ID('[tblSprav_Os]', 'U') IS NOT NULL DROP TABLE [tblSprav_Os];
IF OBJECT_ID('[tblSprav_Pr]', 'U') IS NOT NULL DROP TABLE [tblSprav_Pr];
IF OBJECT_ID('[tbku_SpisokRaspredeleniya]', 'U') IS NOT NULL DROP TABLE [tbku_SpisokRaspredeleniya];
IF OBJECT_ID('[tbku_GrafikZaezdovSanatoriev]', 'U') IS NOT NULL DROP TABLE [tbku_GrafikZaezdovSanatoriev];
IF OBJECT_ID('[tbku_StrokaProfsoyza]', 'U') IS NOT NULL DROP TABLE [tbku_StrokaProfsoyza];
IF OBJECT_ID('[tbbu_finOtchetSetting]', 'U') IS NOT NULL DROP TABLE [tbbu_finOtchetSetting];
IF OBJECT_ID('[tbbu_platejka]', 'U') IS NOT NULL DROP TABLE [tbbu_platejka];
IF OBJECT_ID('[tbbu_provodki]', 'U') IS NOT NULL DROP TABLE [tbbu_provodki];
PRINT 'All existing tables dropped.';
GO

-- ============================================================
-- CREATE TABLES — Group 1: Lookup/Reference (no FKs)
-- ============================================================

CREATE TABLE [sp_soato] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL
);
GO

CREATE TABLE [sp_sanatorium_type] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [version]   INT NULL,
    [s_comment] NVARCHAR(MAX) NULL,
    [s_create]  DATETIME2 NULL,
    [s_status]  NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_user_id] BIGINT NULL,
    [NATIVE_ID] BIGINT NULL
);
GO

CREATE TABLE [sp_sanatorium_profil] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_branch] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [version]   INT NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_status]  NVARCHAR(50) NULL,
    [s_user_id] BIGINT NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_country] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [version]   INT NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_status]  NVARCHAR(50) NULL,
    [s_user_id] BIGINT NULL,
    [s_create]  DATETIME2 NULL,
    [NATIVE_ID] BIGINT NULL
);
GO

CREATE TABLE [sp_position] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [version]   INT NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_status]  NVARCHAR(50) NULL,
    [s_user_id] BIGINT NULL,
    [s_create]  DATETIME2 NULL,
    [NATIVE_ID] BIGINT NULL
);
GO

CREATE TABLE [sp_nation] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_degree] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_contracttype] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_departmenttype] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_section] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_okonx] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_opf] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [sp_soogu] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [version]   BIGINT NULL,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL,
    [s_status]  NVARCHAR(50) NULL,
    [s_user_id] BIGINT NULL,
    [NATIVE_ID] BIGINT NULL
);
GO

CREATE TABLE [sp_employees] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [code]      NVARCHAR(50) NULL,
    [name_ru]   NVARCHAR(500) NULL,
    [name_uz]   NVARCHAR(500) NULL,
    [name_uzl]  NVARCHAR(500) NULL,
    [s_create]  DATETIME2 NULL
);
GO

CREATE TABLE [tbcs_dictionarytype] (
    [REC_ID]    BIGINT NOT NULL PRIMARY KEY,
    [CODE]      NVARCHAR(50) NULL,
    [NAME_RU]   NVARCHAR(500) NULL,
    [NAME_UZ]   NVARCHAR(500) NULL
);
GO

CREATE TABLE [tblSprav_San] (
    [ID]           INT NOT NULL PRIMARY KEY,
    [Name_San]     NVARCHAR(500) NULL,
    [Num_Kod]      NVARCHAR(50) NULL,
    [UserId]       BIGINT NULL,
    [KurortSanID]  INT NULL
);
GO

CREATE TABLE [tblSprav_Kv] (
    [ID]       BIGINT NOT NULL PRIMARY KEY,
    [Name_Kv]  NVARCHAR(500) NULL,
    [UserId]   BIGINT NULL
);
GO

CREATE TABLE [tblSprav_Org] (
    [ID]           BIGINT NOT NULL PRIMARY KEY,
    [Name_Org]     NVARCHAR(500) NULL,
    [INN]          NVARCHAR(20) NULL,
    [OWNER_CODE]   NVARCHAR(50) NULL,
    [ID_Ot]        BIGINT NULL,
    [DepartmentBase] NVARCHAR(255) NULL,
    [UserId]       BIGINT NULL,
    [Address]      NVARCHAR(MAX) NULL,
    [Name_Short]   NVARCHAR(500) NULL,
    [Name_Full]    NVARCHAR(1000) NULL,
    [SOATO_ID]     NVARCHAR(50) NULL
);
GO

CREATE TABLE [tblSprav_Otras] (
    [ID]             BIGINT NOT NULL PRIMARY KEY,
    [Name_Ot]        NVARCHAR(500) NULL,
    [DepartmentBase] NVARCHAR(255) NULL,
    [UserId]         BIGINT NULL
);
GO

CREATE TABLE [tblSprav_Reg] (
    [ID]       BIGINT NOT NULL PRIMARY KEY,
    [name_ru]  NVARCHAR(500) NULL,
    [name_uz]  NVARCHAR(500) NULL,
    [name_uzl] NVARCHAR(500) NULL,
    [code]     NVARCHAR(50) NULL,
    [UserId]   BIGINT NULL,
    [s_create] DATETIME2 NULL
);
GO

CREATE TABLE [tblSprav_Vil] (
    [ID]       BIGINT NOT NULL PRIMARY KEY,
    [Name_Vil] NVARCHAR(500) NULL,
    [CodeVil]  NVARCHAR(50) NULL,
    [RegID]    BIGINT NULL
);
GO

CREATE TABLE [tblSprav_MFO] (
    [ID]       BIGINT NOT NULL PRIMARY KEY,
    [Code_MFO] NVARCHAR(20) NULL,
    [Name_MFO] NVARCHAR(500) NULL
);
GO

CREATE TABLE [tblSprav_Face] (
    [ID]        BIGINT NOT NULL PRIMARY KEY,
    [Name_Face] NVARCHAR(500) NULL,
    [UserId]    BIGINT NULL
);
GO

CREATE TABLE [tblSprav_Os] (
    [ID]      BIGINT NOT NULL PRIMARY KEY,
    [Name_Os] NVARCHAR(500) NULL,
    [UserId]  BIGINT NULL
);
GO

CREATE TABLE [tblSprav_Pr] (
    [ID]      BIGINT NOT NULL PRIMARY KEY,
    [Name_Pr] NVARCHAR(500) NULL,
    [Value]   DECIMAL(18,2) NULL,
    [UserId]  BIGINT NULL
);
GO

-- ============================================================
-- CREATE TABLES — Group 2: Core Entity Tables
-- ============================================================

CREATE TABLE [functionality] (
    [id]          BIGINT NOT NULL PRIMARY KEY,
    [name]        NVARCHAR(500) NULL,
    [description] NVARCHAR(MAX) NULL
);
GO

CREATE TABLE [roles] (
    [id]        BIGINT NOT NULL PRIMARY KEY,
    [version]   INT NOT NULL DEFAULT 0,
    [name]      NVARCHAR(255) NULL,
    [s_comment] NVARCHAR(MAX) NULL,
    [s_create]  DATETIME2 NULL,
    [s_status]  NVARCHAR(50) NULL,
    [s_user_id] BIGINT NULL,
    [NATIVE_ID] BIGINT NULL
);
GO

CREATE TABLE [users] (
    [id]              BIGINT NOT NULL PRIMARY KEY,
    [version]         INT NOT NULL DEFAULT 0,
    [account_expired] BIT NOT NULL DEFAULT 0,
    [account_locked]  BIT NOT NULL DEFAULT 0,
    [changed]         DATETIME2 NULL,
    [created]         DATETIME2 NULL,
    [email]           NVARCHAR(255) NULL,
    [last_logged]     DATETIME2 NULL,
    [last_logged_id]  NVARCHAR(255) NULL,
    [state]           NVARCHAR(50) NULL,
    [password]        NVARCHAR(MAX) NULL,
    [salt]            NVARCHAR(MAX) NULL,
    [language]        NVARCHAR(10) NULL,
    [is_super_admin]  BIT NULL,
    [user_name]       NVARCHAR(255) NULL,
    [s_comment]       NVARCHAR(MAX) NULL,
    [s_create]        DATETIME2 NULL,
    [s_status]        NVARCHAR(50) NULL,
    [s_user_id]       BIGINT NULL,
    [usertype]        NVARCHAR(50) NULL,
    [bkut_id]         BIGINT NOT NULL DEFAULT 0,
    [dep_id]          BIGINT NULL,
    [role_id]         BIGINT NULL,
    [REGION_ID]       NVARCHAR(50) NULL,
    [BRANCH_ID]       BIGINT NOT NULL DEFAULT 0,
    [NATIVE_ID]       BIGINT NULL
);
GO

CREATE TABLE [role_permission] (
    [id]               BIGINT NOT NULL PRIMARY KEY,
    [role_id]          BIGINT NULL,
    [functionality_id] BIGINT NULL,
    [action]           NVARCHAR(255) NULL
);
GO

CREATE TABLE [api_refresh_tokens] (
    [id]         BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [user_id]    BIGINT NOT NULL,
    [token]      NVARCHAR(500) NOT NULL,
    [expires_at] DATETIME2 NOT NULL,
    [created_at] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [revoked_at] DATETIME2 NULL
);
GO
CREATE UNIQUE INDEX UX_api_refresh_tokens_token ON [api_refresh_tokens]([token]);
CREATE INDEX IX_api_refresh_tokens_user_id ON [api_refresh_tokens]([user_id]);
GO

CREATE TABLE [useractions] (
    [id]          BIGINT NOT NULL PRIMARY KEY,
    [user_id]     BIGINT NULL,
    [action_name] NVARCHAR(500) NULL,
    [action_date] DATETIME2 NULL,
    [description] NVARCHAR(MAX) NULL
);
GO

CREATE TABLE [t_departmentbase] (
    [id]                   BIGINT NOT NULL PRIMARY KEY,
    [DEPARTMENT_CODE]      NVARCHAR(255) NULL,
    [name_ru]              NVARCHAR(1000) NULL,
    [name_uz]              NVARCHAR(1000) NULL,
    [name_uzl]             NVARCHAR(1000) NULL,
    [s_status]             NVARCHAR(50) NULL,
    [s_user_id]            BIGINT NULL,
    [s_create]             DATETIME2 NULL,
    [PARENT_ID]            BIGINT NULL,
    [MATCH_DEPARTMENTBASE] NVARCHAR(255) NULL,
    [bkut_id]              BIGINT NULL
);
GO

CREATE TABLE [t_camp] (
    [id]         BIGINT NOT NULL PRIMARY KEY,
    [camps_name] NVARCHAR(500) NULL,
    [s_create]   DATETIME2 NULL,
    [s_status]   NVARCHAR(50) NULL
);
GO

CREATE TABLE [t_sanatorium] (
    [id]                   BIGINT NOT NULL PRIMARY KEY,
    [name_uz]              NVARCHAR(500) NULL,
    [name_ru]              NVARCHAR(500) NULL,
    [code]                 NVARCHAR(50) NULL,
    [s_create]             DATETIME2 NULL,
    [sanatorium_type_id]   BIGINT NULL
);
GO

CREATE TABLE [t_normativdoc] (
    [id]          BIGINT NOT NULL PRIMARY KEY,
    [name_uz]     NVARCHAR(1000) NULL,
    [name_ru]     NVARCHAR(1000) NULL,
    [s_create]    DATETIME2 NULL,
    [description] NVARCHAR(MAX) NULL
);
GO

CREATE TABLE [t_egrpo] (
    [id]            BIGINT NOT NULL PRIMARY KEY,
    [inn]           NVARCHAR(20) NULL,
    [name_full]     NVARCHAR(1000) NULL,
    [name_short]    NVARCHAR(500) NULL,
    [address]       NVARCHAR(MAX) NULL,
    [soato_id]      NVARCHAR(50) NULL,
    [okonx_id]      BIGINT NULL,
    [opf_id]        BIGINT NULL,
    [director_name] NVARCHAR(500) NULL,
    [director_inn]  NVARCHAR(20) NULL
);
GO

CREATE TABLE [tbcs_dictionary] (
    [REC_ID]             BIGINT NOT NULL PRIMARY KEY,
    [NAME_RU]            NVARCHAR(500) NULL,
    [NAME_UZ]            NVARCHAR(500) NULL,
    [NAME_LT]            NVARCHAR(500) NULL,
    [CODE]               NVARCHAR(50) NULL,
    [STATUS]             BIT NULL,
    [USER_ID]            INT NULL,
    [DICTIONARY_TYPE_ID] BIGINT NULL
);
GO

-- ============================================================
-- CREATE TABLES — Group 3: Voucher/Planning Tables
-- ============================================================

CREATE TABLE [tbpt_putyovki] (
    [REC_ID]          BIGINT NOT NULL PRIMARY KEY,
    [DEPARTMENT_CODE] NVARCHAR(255) NULL,
    [SANATORIUM_ID]   INT NULL,
    [CAMP_ID]         BIGINT NULL,
    [USER_ID]         BIGINT NULL,
    [SERIAL_BEGIN]    NVARCHAR(50) NULL,
    [SERIAL_END]      NVARCHAR(50) NULL,
    [REST_BEGIN_DATE] DATETIME2 NULL,
    [REST_END_DATE]   DATETIME2 NULL,
    [PRICE]           DECIMAL(18,2) NULL,
    [STATUS]          NVARCHAR(50) NULL,
    [MODIFY_DATE]     DATETIME2 NULL,
    [QUANTITY_IN]     INT NULL,
    [REMAIN_BAL]      DECIMAL(18,2) NULL,
    [QUANTITY_OUT]    INT NULL,
    [QUANTITY_REMAIN] INT NULL,
    [SMFIL]           INT NULL
);
GO

CREATE TABLE [tbpt_zaezdi] (
    [REC_ID]        BIGINT NOT NULL PRIMARY KEY,
    [ZAEZD_BEGIN]   DATETIME2 NULL,
    [ZAEZD_END]     DATETIME2 NULL,
    [PLACE_TYPE_ID] BIGINT NULL,
    [PRICE]         DECIMAL(18,2) NULL,
    [QUANTITY]      INT NULL,
    [SANATORY_ID]   INT NULL,
    [CAMP_ID]       BIGINT NULL,
    [USER_ID]       BIGINT NULL
);
GO

CREATE TABLE [tbpt_yearlyplanregion] (
    [REC_ID]          BIGINT NOT NULL PRIMARY KEY,
    [YEAR]            NVARCHAR(10) NULL,
    [KVARTAL]         NVARCHAR(10) NULL,
    [SANATORIUM_ID]   BIGINT NULL,
    [CAMP_ID]         BIGINT NULL,
    [TIP_MEST_ID]     BIGINT NULL,
    [PRICE]           DECIMAL(18,2) NULL,
    [QUANTITY]        BIGINT NULL,
    [STATUS]          NVARCHAR(50) NULL,
    [USER_ID]         BIGINT NULL,
    [REGION_ID]       NVARCHAR(50) NULL,
    [MODIFY_DATE]     DATETIME2 NULL,
    [DEPARTMENT_BASE] NVARCHAR(255) NULL
);
GO

CREATE TABLE [tbpt_yearlyplanotrasl] (
    [REC_ID]          BIGINT NOT NULL PRIMARY KEY,
    [YEAR]            NVARCHAR(10) NULL,
    [KVARTAL]         NVARCHAR(10) NULL,
    [SANATORIUM_ID]   BIGINT NULL,
    [CAMP_ID]         BIGINT NULL,
    [PRICE]           DECIMAL(18,2) NULL,
    [QUANTITY]        BIGINT NULL,
    [STATUS]          NVARCHAR(50) NULL,
    [USER_ID]         BIGINT NULL,
    [OTRASL_ID]       BIGINT NULL,
    [MODIFY_DATE]     DATETIME2 NULL,
    [DEPARTMANT_BASE] NVARCHAR(255) NULL
);
GO

CREATE TABLE [tbpt_avarageprice] (
    [REC_ID]   BIGINT NOT NULL PRIMARY KEY,
    [SET_DATE] DATETIME2 NULL,
    [AMOUNT]   DECIMAL(18,2) NULL,
    [USER_ID]  BIGINT NULL
);
GO

CREATE TABLE [tblBrons] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [ID_Sanat]        INT NULL,
    [BRON_DATE]       DATETIME2 NULL,
    [EXPIRE_DATE]     DATETIME2 NULL,
    [USER_ID]         BIGINT NULL,
    [DEPARTMENT_BASE] NVARCHAR(255) NULL,
    [PRIM]            NVARCHAR(MAX) NULL
);
GO

-- ============================================================
-- CREATE TABLES — Group 4: KU Distribution Tables
-- ============================================================

CREATE TABLE [tbku_SpisokRaspredeleniya] (
    [ID]                      BIGINT NOT NULL PRIMARY KEY,
    [GodZaezdov]              BIGINT NULL,
    [SanatoryKod]             NVARCHAR(50) NULL,
    [ProfsoyzKod]             NVARCHAR(50) NULL,
    [SanatoryRazmeshenieKod]  NVARCHAR(50) NULL,
    [Putevok1kv]              BIGINT NULL,
    [Putevok2kv]              BIGINT NULL,
    [Putevok3kv]              BIGINT NULL,
    [Putevok4kv]              BIGINT NULL
);
GO

CREATE TABLE [tbku_GrafikZaezdovSanatoriev] (
    [ID]                           BIGINT NOT NULL PRIMARY KEY,
    [GodZaezdov]                   BIGINT NULL,
    [SanatoryKod]                  NVARCHAR(50) NULL,
    [SanatoryNaimenovanie]         NVARCHAR(500) NULL,
    [SanatoryProfil]               NVARCHAR(255) NULL,
    [SanatoryOblast]               NVARCHAR(255) NULL,
    [SanatoryKoekVsego]            BIGINT NULL,
    [SanatoryKoekLetom]            BIGINT NULL,
    [SanatoryKoekZimoy]            BIGINT NULL,
    [SanatoryRazmeshenieKod]       NVARCHAR(50) NULL,
    [SanatoryRazmeshenieNaimenovanie] NVARCHAR(500) NULL,
    [Putevok1kv]                   BIGINT NULL,
    [Putevok2kv]                   BIGINT NULL,
    [Putevok3kv]                   BIGINT NULL,
    [Putevok4kv]                   BIGINT NULL
);
GO

CREATE TABLE [tbku_StrokaProfsoyza] (
    [ID]                   BIGINT NOT NULL PRIMARY KEY,
    [ProfsoyzKod]          NVARCHAR(50) NULL,
    [ProfsoyzNaimenovanie] NVARCHAR(500) NULL,
    [MATCH_DEPARTMENTBASE] NVARCHAR(255) NULL
);
GO

-- ============================================================
-- CREATE TABLES — Group 5: Accounting / Finance Tables
-- ============================================================

CREATE TABLE [tbbu_provodki] (
    [REC_ID]          BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [DEPARTMENT_BASE] NVARCHAR(255) NULL,
    [USER_ID]         BIGINT NULL,
    [INOUTFLG]        BIT NULL,
    [TRDT]            DATETIME2 NULL,
    [NUMBER]          NVARCHAR(50) NULL,
    [TIPSCHETA]       BIT NULL,
    [TIPOBOROTA]      BIT NULL,
    [PLATELNAME]      NVARCHAR(500) NULL,
    [PLATELINN]       NVARCHAR(20) NULL,
    [PLATELMFO]       NVARCHAR(10) NULL,
    [POLUCHNAME]      NVARCHAR(500) NULL,
    [POLUCHINN]       NVARCHAR(20) NULL,
    [POLUCHMFO]       NVARCHAR(10) NULL,
    [PLATELRSCH]      NVARCHAR(50) NULL,
    [POLUCHRSCH]      NVARCHAR(50) NULL,
    [SUMMA]           DECIMAL(18,2) NULL,
    [COMMENT]         NVARCHAR(1000) NULL,
    [TRREF]           NVARCHAR(50) NULL,
    [TRREFNO]         NVARCHAR(50) NULL
);
GO

CREATE TABLE [tbbu_finOtchetSetting] (
    [REC_ID]          BIGINT NOT NULL PRIMARY KEY,
    [OTCHET_PART]     INT NULL,
    [CODE]            NVARCHAR(50) NULL,
    [DEBIT_SIDE]      NVARCHAR(50) NULL,
    [CREDIT_SIDE]     NVARCHAR(50) NULL,
    [ACCOUNT_CODE]    NVARCHAR(50) NULL,
    [DATA_TYPE]       INT NULL,
    [DATA_DESC]       NVARCHAR(500) NULL,
    [DEPARTMENT_BASE] NVARCHAR(255) NULL,
    [USR_ID]          BIGINT NULL
);
GO

CREATE TABLE [tbbu_platejka] (
    [REC_ID]          BIGINT NOT NULL PRIMARY KEY,
    [PLAT_DATE]       DATETIME2 NULL,
    [PLAT_NUM]        NVARCHAR(20) NULL,
    [PLATELNAME]      NVARCHAR(500) NULL,
    [PLATELINN]       NVARCHAR(20) NULL,
    [PLATELRASCHET]   NVARCHAR(50) NULL,
    [PLATELBANKNAME]  NVARCHAR(500) NULL,
    [PLATELBANKMFO]   NVARCHAR(10) NULL,
    [POLUCHNAME]      NVARCHAR(500) NULL,
    [POLUCHINN]       NVARCHAR(20) NULL,
    [POLUCHRASCHET]   NVARCHAR(50) NULL,
    [POLUCHBANKNAME]  NVARCHAR(500) NULL,
    [POLUCHBANKMFO]   NVARCHAR(10) NULL,
    [SUMMA]           DECIMAL(18,2) NULL,
    [COMMENT]         NVARCHAR(500) NULL,
    [RUKOVODITEL]     NVARCHAR(200) NULL,
    [GLAVBUX]         NVARCHAR(200) NULL,
    [DEPARTMENT_CODE] NVARCHAR(255) NULL,
    [USR_ID]          BIGINT NULL,
    [MODIFY_DATE]     DATETIME2 NULL
);
GO

-- ============================================================
-- CREATE TABLES — Group 6: Transaction Tables (no EF context)
-- ============================================================

CREATE TABLE [tblPost] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [ID_KV]           BIGINT NULL,
    [Data_Post]       DATETIME2 NULL,
    [Num_Doc]         NVARCHAR(50) NULL,
    [ID_Pos]          BIGINT NULL,
    [Num_Kol]         INT NULL,
    [Symma]           DECIMAL(18,2) NULL,
    [Name_Dov]        NVARCHAR(500) NULL,
    [Vid]             BIT NULL,
    [SmFil]           INT NULL,
    [UserId]          BIGINT NULL,
    [DepartmentBase]  NVARCHAR(255) NULL,
    [BeginNumber]     INT NULL,
    [EndNumber]       INT NULL,
    [PROVODKA_ID]     BIGINT NULL
);
GO

CREATE TABLE [tblVipl] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [ID_Org]          BIGINT NULL,
    [Data_Post]       DATETIME2 NULL,
    [Num_Doc]         NVARCHAR(50) NULL,
    [Num_Kol]         INT NULL,
    [Symma]           DECIMAL(18,2) NULL,
    [Name_Dov]        NVARCHAR(500) NULL,
    [ID_Kv]           BIGINT NULL,
    [Vid]             BIT NULL,
    [SmFil]           INT NULL,
    [UserId]          BIGINT NULL,
    [DepartmentBase]  NVARCHAR(255) NULL,
    [PROVODKA_ID]     BIGINT NULL
);
GO

CREATE TABLE [tblViplF] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [ID_Org]          BIGINT NULL,
    [Data_Post]       DATETIME2 NULL,
    [Num_Doc]         NVARCHAR(50) NULL,
    [Num_Kol]         INT NULL,
    [Symma]           DECIMAL(18,2) NULL,
    [Name_Dov]        NVARCHAR(500) NULL,
    [ID_Kv]           BIGINT NULL,
    [Vid]             BIT NULL,
    [SmFil]           INT NULL,
    [UserId]          BIGINT NULL,
    [DepartmentBase]  NVARCHAR(255) NULL,
    [PROVODKA_ID]     BIGINT NULL
);
GO

CREATE TABLE [tblVipl_Put] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [ID_Vipl]         BIGINT NULL,
    [ID_Sanat]        INT NULL,
    [Num_Put]         NVARCHAR(50) NULL,
    [Num_Proc]        NVARCHAR(50) NULL,
    [Symma]           DECIMAL(18,2) NULL,
    [ID_PolVoz]       BIT NULL,
    [Data_Post]       DATETIME2 NULL,
    [UserId]          BIGINT NULL,
    [ZayavkaID]       BIGINT NULL,
    [DepartmentBase]  NVARCHAR(255) NULL
);
GO

CREATE TABLE [tblOplata] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [Data_Post]       DATETIME2 NULL,
    [Num_Doc]         NVARCHAR(50) NULL,
    [ID_Otras]        BIGINT NULL,
    [Symma_DT]        DECIMAL(18,2) NULL,
    [Symma_KT]        DECIMAL(18,2) NULL,
    [Osnov]           NVARCHAR(500) NULL,
    [ID_KV]           BIGINT NULL,
    [SmFil]           INT NULL,
    [Name_org]        NVARCHAR(500) NULL,
    [Tip_Org]         INT NULL,
    [PrizOpl]         INT NULL,
    [UserId]          BIGINT NULL,
    [Departmentbase]  NVARCHAR(255) NULL,
    [PROVODKA_ID]     BIGINT NULL
);
GO

CREATE TABLE [tblOplata_Rasxod] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [Data_Post]       DATETIME2 NULL,
    [Num_Doc]         NVARCHAR(50) NULL,
    [ID_Otras]        BIGINT NULL,
    [Symma_DT]        DECIMAL(18,2) NULL,
    [Symma_KT]        DECIMAL(18,2) NULL,
    [Osnov]           NVARCHAR(500) NULL,
    [ID_KV]           BIGINT NULL,
    [SmFil]           INT NULL,
    [Name_org]        NVARCHAR(500) NULL,
    [Tip_Org]         INT NULL,
    [PrizOpl]         INT NULL,
    [UserId]          BIGINT NULL,
    [Departmentbase]  NVARCHAR(255) NULL,
    [PROVODKA_ID]     BIGINT NULL
);
GO

CREATE TABLE [tblVozvrat] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [Data_Post]       DATETIME2 NULL,
    [Num_Doc]         NVARCHAR(50) NULL,
    [ID_Sanat]        INT NULL,
    [Num_Put]         NVARCHAR(50) NULL,
    [ID_Ot]           BIGINT NULL,
    [Dover]           NVARCHAR(500) NULL,
    [ID_Pol]          BIGINT NULL,
    [SymmaP]          DECIMAL(18,2) NULL,
    [ID_KV]           BIGINT NULL,
    [SmFil]           INT NULL,
    [Tip_Voz]         BIT NULL,
    [UserId]          BIGINT NULL,
    [DepartmentBase]  NVARCHAR(255) NULL,
    [PRIXOD_ID]       BIGINT NULL,
    [RASXOD_ID]       BIGINT NULL,
    [PRIXOD_PROVODKA_ID]  BIGINT NULL,
    [RASXOD_PROVODKA_ID]  BIGINT NULL
);
GO

CREATE TABLE [tblZakritiye] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [DepartmentBase]  NVARCHAR(255) NULL,
    [KvartalNum]      INT NULL,
    [Year]            INT NULL,
    [UserId]          BIGINT NULL,
    [Data_Post]       DATETIME2 NULL
);
GO

CREATE TABLE [tblPos_Put] (
    [ID]              BIGINT NOT NULL PRIMARY KEY,
    [ID_San]          INT NULL,
    [Num_Put]         NVARCHAR(50) NULL,
    [DataNach]        DATETIME2 NULL,
    [DataKon]         DATETIME2 NULL,
    [Price]           DECIMAL(18,2) NULL,
    [Mes]             NVARCHAR(50) NULL,
    [Tip]             NVARCHAR(50) NULL,
    [Status]          NVARCHAR(50) NULL,
    [UserId]          BIGINT NULL,
    [DEPARTMENT_BASE] NVARCHAR(255) NULL,
    [SMFIL]           NVARCHAR(50) NULL,
    [MODIFY_DATE]     DATETIME2 NULL
);
GO

PRINT 'All tables created successfully.';
GO

-- ============================================================
-- ADD FK CONSTRAINTS
-- ============================================================

ALTER TABLE [users]
    ADD CONSTRAINT FK_users_roles
    FOREIGN KEY ([role_id]) REFERENCES [roles]([id]);
GO

ALTER TABLE [role_permission]
    ADD CONSTRAINT FK_role_permission_roles
    FOREIGN KEY ([role_id]) REFERENCES [roles]([id]);
GO

ALTER TABLE [role_permission]
    ADD CONSTRAINT FK_role_permission_functionality
    FOREIGN KEY ([functionality_id]) REFERENCES [functionality]([id]);
GO

ALTER TABLE [api_refresh_tokens]
    ADD CONSTRAINT FK_api_refresh_tokens_users
    FOREIGN KEY ([user_id]) REFERENCES [users]([id]);
GO

ALTER TABLE [useractions]
    ADD CONSTRAINT FK_useractions_users
    FOREIGN KEY ([user_id]) REFERENCES [users]([id]);
GO

ALTER TABLE [tbpt_putyovki]
    ADD CONSTRAINT FK_tbpt_putyovki_tblSprav_San
    FOREIGN KEY ([SANATORIUM_ID]) REFERENCES [tblSprav_San]([ID]);
GO

ALTER TABLE [tbpt_putyovki]
    ADD CONSTRAINT FK_tbpt_putyovki_users
    FOREIGN KEY ([USER_ID]) REFERENCES [users]([id]);
GO

ALTER TABLE [tbpt_zaezdi]
    ADD CONSTRAINT FK_tbpt_zaezdi_tblSprav_San
    FOREIGN KEY ([SANATORY_ID]) REFERENCES [tblSprav_San]([ID]);
GO

ALTER TABLE [tbcs_dictionary]
    ADD CONSTRAINT FK_tbcs_dictionary_tbcs_dictionarytype
    FOREIGN KEY ([DICTIONARY_TYPE_ID]) REFERENCES [tbcs_dictionarytype]([REC_ID]);
GO

ALTER TABLE [tbpt_yearlyplanregion]
    ADD CONSTRAINT FK_tbpt_yearlyplanregion_users
    FOREIGN KEY ([USER_ID]) REFERENCES [users]([id]);
GO

ALTER TABLE [tbpt_yearlyplanotrasl]
    ADD CONSTRAINT FK_tbpt_yearlyplanotrasl_users
    FOREIGN KEY ([USER_ID]) REFERENCES [users]([id]);
GO

ALTER TABLE [tbpt_avarageprice]
    ADD CONSTRAINT FK_tbpt_avarageprice_users
    FOREIGN KEY ([USER_ID]) REFERENCES [users]([id]);
GO

ALTER TABLE [t_sanatorium]
    ADD CONSTRAINT FK_t_sanatorium_sp_sanatorium_type
    FOREIGN KEY ([sanatorium_type_id]) REFERENCES [sp_sanatorium_type]([id]);
GO

ALTER TABLE [t_egrpo]
    ADD CONSTRAINT FK_t_egrpo_sp_okonx
    FOREIGN KEY ([okonx_id]) REFERENCES [sp_okonx]([id]);
GO

ALTER TABLE [t_egrpo]
    ADD CONSTRAINT FK_t_egrpo_sp_opf
    FOREIGN KEY ([opf_id]) REFERENCES [sp_opf]([id]);
GO

ALTER TABLE [tblBrons]
    ADD CONSTRAINT FK_tblBrons_tblSprav_San
    FOREIGN KEY ([ID_Sanat]) REFERENCES [tblSprav_San]([ID]);
GO

ALTER TABLE [tblBrons]
    ADD CONSTRAINT FK_tblBrons_users
    FOREIGN KEY ([USER_ID]) REFERENCES [users]([id]);
GO

ALTER TABLE [tblPos_Put]
    ADD CONSTRAINT FK_tblPos_Put_tblSprav_San
    FOREIGN KEY ([ID_San]) REFERENCES [tblSprav_San]([ID]);
GO

ALTER TABLE [tblPos_Put]
    ADD CONSTRAINT FK_tblPos_Put_tblPost
    FOREIGN KEY ([ID]) REFERENCES [tblPost]([ID]);
GO

ALTER TABLE [tblPost]
    ADD CONSTRAINT FK_tblPost_tblSprav_Org
    FOREIGN KEY ([ID_Pos]) REFERENCES [tblSprav_Org]([ID]);
GO

ALTER TABLE [tblPost]
    ADD CONSTRAINT FK_tblPost_tblSprav_Kv
    FOREIGN KEY ([ID_KV]) REFERENCES [tblSprav_Kv]([ID]);
GO

ALTER TABLE [tblPost]
    ADD CONSTRAINT FK_tblPost_users
    FOREIGN KEY ([UserId]) REFERENCES [users]([id]);
GO

ALTER TABLE [tblVipl]
    ADD CONSTRAINT FK_tblVipl_tblSprav_Org
    FOREIGN KEY ([ID_Org]) REFERENCES [tblSprav_Org]([ID]);
GO

ALTER TABLE [tblVipl]
    ADD CONSTRAINT FK_tblVipl_tblSprav_Kv
    FOREIGN KEY ([ID_Kv]) REFERENCES [tblSprav_Kv]([ID]);
GO

ALTER TABLE [tblVipl]
    ADD CONSTRAINT FK_tblVipl_users
    FOREIGN KEY ([UserId]) REFERENCES [users]([id]);
GO

ALTER TABLE [tblViplF]
    ADD CONSTRAINT FK_tblViplF_tblSprav_Org
    FOREIGN KEY ([ID_Org]) REFERENCES [tblSprav_Org]([ID]);
GO

ALTER TABLE [tblVipl_Put]
    ADD CONSTRAINT FK_tblVipl_Put_tblVipl
    FOREIGN KEY ([ID_Vipl]) REFERENCES [tblVipl]([ID]);
GO

ALTER TABLE [tblVipl_Put]
    ADD CONSTRAINT FK_tblVipl_Put_tblSprav_San
    FOREIGN KEY ([ID_Sanat]) REFERENCES [tblSprav_San]([ID]);
GO

ALTER TABLE [tblOplata]
    ADD CONSTRAINT FK_tblOplata_tblSprav_Otras
    FOREIGN KEY ([ID_Otras]) REFERENCES [tblSprav_Otras]([ID]);
GO

ALTER TABLE [tblOplata]
    ADD CONSTRAINT FK_tblOplata_tblSprav_Kv
    FOREIGN KEY ([ID_KV]) REFERENCES [tblSprav_Kv]([ID]);
GO

ALTER TABLE [tblOplata]
    ADD CONSTRAINT FK_tblOplata_users
    FOREIGN KEY ([UserId]) REFERENCES [users]([id]);
GO

ALTER TABLE [tblOplata_Rasxod]
    ADD CONSTRAINT FK_tblOplata_Rasxod_users
    FOREIGN KEY ([UserId]) REFERENCES [users]([id]);
GO

ALTER TABLE [tblVozvrat]
    ADD CONSTRAINT FK_tblVozvrat_tblSprav_San
    FOREIGN KEY ([ID_Sanat]) REFERENCES [tblSprav_San]([ID]);
GO

ALTER TABLE [tblVozvrat]
    ADD CONSTRAINT FK_tblVozvrat_tblSprav_Org
    FOREIGN KEY ([ID_Ot]) REFERENCES [tblSprav_Org]([ID]);
GO

ALTER TABLE [tblVozvrat]
    ADD CONSTRAINT FK_tblVozvrat_tblPost
    FOREIGN KEY ([PRIXOD_ID]) REFERENCES [tblPost]([ID]);
GO

ALTER TABLE [tblVozvrat]
    ADD CONSTRAINT FK_tblVozvrat_tblVipl
    FOREIGN KEY ([RASXOD_ID]) REFERENCES [tblVipl]([ID]);
GO

ALTER TABLE [tblZakritiye]
    ADD CONSTRAINT FK_tblZakritiye_users
    FOREIGN KEY ([UserId]) REFERENCES [users]([id]);
GO

PRINT 'All FK constraints added successfully.';
GO

-- ============================================================
-- CREATE uspLogin stored procedure
-- ============================================================

IF OBJECT_ID('[dbo].[uspLogin]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[uspLogin];
GO

CREATE PROCEDURE [dbo].[uspLogin]
    @login  NVARCHAR(255),
    @parol  NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT id
    FROM [users]
    WHERE user_name = @login
      AND [password] = @parol
      AND account_locked = 0
      AND account_expired = 0;
END
GO

PRINT 'uspLogin stored procedure created.';
GO

PRINT '================================================';
PRINT 'KasabaKIAT database schema created successfully.';
SELECT COUNT(*) AS TableCount FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
GO
