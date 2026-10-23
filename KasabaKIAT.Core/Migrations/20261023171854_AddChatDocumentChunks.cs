using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KasabaKIAT.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddChatDocumentChunks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "api_refresh_tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_refresh_tokens", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_document_chunks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    source_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    chunk_index = table.Column<int>(type: "int", nullable: false),
                    chunk_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    embedding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    ingested_by = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_document_chunks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "functionality",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_functionality", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "p_provodok",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    score_begin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    score_end = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    order = table.Column<long>(type: "bigint", nullable: true),
                    REGION_ID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_p_provodok", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provodok",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    score_begin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    score_end = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provodok", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_branch",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version = table.Column<int>(type: "int", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_branch", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_contracttype",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_contracttype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_country",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version = table.Column<int>(type: "int", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_country", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_degree",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_degree", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_departmenttype",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_departmenttype", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_employees",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_nation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_nation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_okonx",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_okonx", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_opf",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_opf", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_position",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version = table.Column<int>(type: "int", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_position", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_sanatorium_profil",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_sanatorium_profil", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_sanatorium_type",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version = table.Column<int>(type: "int", nullable: true),
                    s_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_sanatorium_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_section",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_section", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_soato",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_soato", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sp_soogu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version = table.Column<long>(type: "bigint", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    NATIVE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sp_soogu", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_camp",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    camps_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_camp", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_departmentbase",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PARENT_ID = table.Column<long>(type: "bigint", nullable: true),
                    MATCH_DEPARTMENTBASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bkut_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_departmentbase", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_egrpo",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_full = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_short = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    soato_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    okonx_id = table.Column<long>(type: "bigint", nullable: true),
                    opf_id = table.Column<long>(type: "bigint", nullable: true),
                    director_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    director_inn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_egrpo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_normativdoc",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_normativdoc", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "t_sanatorium",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    sanatorium_type_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_sanatorium", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbbu_balance",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUR_YEAR = table.Column<int>(type: "int", nullable: true),
                    CUR_KV = table.Column<int>(type: "int", nullable: true),
                    DEBIT_BAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CREDIT_BAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DEBIT_COUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CREDIT_COUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DEBIT_TRANS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CREDIT_TRANS = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbu_balance", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbbu_bank_payments",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOC_NUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOC_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SUMMA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PAYER_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PAYER_INN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUR_YEAR = table.Column<int>(type: "int", nullable: true),
                    CUR_KV = table.Column<int>(type: "int", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbu_bank_payments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbbu_debitor_creditor",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUR_YEAR = table.Column<int>(type: "int", nullable: true),
                    CUR_KV = table.Column<int>(type: "int", nullable: true),
                    DEBIT_BAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CREDIT_BAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ORG_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbu_debitor_creditor", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbbu_finOtchetSetting",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OTCHET_PART = table.Column<int>(type: "int", nullable: true),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DEBIT_SIDE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREDIT_SIDE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DATA_TYPE = table.Column<int>(type: "int", nullable: true),
                    DATA_DESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USR_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbu_finOtchetSetting", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbbu_platejka",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PLAT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PLAT_NUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLATELNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLATELINN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLATELRASCHET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLATELBANKNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLATELBANKMFO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POLUCHNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POLUCHINN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POLUCHRASCHET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POLUCHBANKNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POLUCHBANKMFO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SUMMA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    COMMENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RUKOVODITEL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GLAVBUX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USR_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbbu_platejka", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbcs_dictionary",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME_UZ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME_LT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS = table.Column<bool>(type: "bit", nullable: true),
                    USER_ID = table.Column<int>(type: "int", nullable: true),
                    DICTIONARY_TYPE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcs_dictionary", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbcs_dictionarytype",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME_RU = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME_UZ = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcs_dictionarytype", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbku_DocumentSchetFaktura",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocNomer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfsoyzKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KolichestvoPutevok = table.Column<int>(type: "int", nullable: true),
                    SummaDoc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DocTypeRealVozv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfsoyzDoverennost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfsoyzDoverennostData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfsoyzDoverennostMOL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfsoyzDogovor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbku_DocumentSchetFaktura", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbku_GrafikZaezdovSanatoriev",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GodZaezdov = table.Column<long>(type: "bigint", nullable: true),
                    SanatoryKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryNaimenovanie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryProfil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryOblast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryKoekVsego = table.Column<long>(type: "bigint", nullable: true),
                    SanatoryKoekLetom = table.Column<long>(type: "bigint", nullable: true),
                    SanatoryKoekZimoy = table.Column<long>(type: "bigint", nullable: true),
                    SanatoryRazmeshenieKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryRazmeshenieNaimenovanie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Putevok1kv = table.Column<long>(type: "bigint", nullable: true),
                    Putevok2kv = table.Column<long>(type: "bigint", nullable: true),
                    Putevok3kv = table.Column<long>(type: "bigint", nullable: true),
                    Putevok4kv = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbku_GrafikZaezdovSanatoriev", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbku_journal",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceID = table.Column<int>(type: "int", nullable: true),
                    CreateDT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: true),
                    ErrorCode = table.Column<int>(type: "int", nullable: true),
                    ErrorDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbku_journal", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbku_SpisokRaspredeleniya",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GodZaezdov = table.Column<long>(type: "bigint", nullable: true),
                    SanatoryKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfsoyzKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryRazmeshenieKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Putevok1kv = table.Column<int>(type: "int", nullable: true),
                    Putevok2kv = table.Column<int>(type: "int", nullable: true),
                    Putevok3kv = table.Column<int>(type: "int", nullable: true),
                    Putevok4kv = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbku_SpisokRaspredeleniya", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbku_StrokaProfsoyza",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfsoyzKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfsoyzNaimenovanie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MATCH_DEPARTMENTBASE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbku_StrokaProfsoyza", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbku_StrokaSchetFakturi",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchetFactura_ID = table.Column<long>(type: "bigint", nullable: true),
                    SanatoryKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryNaimenovanie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Blank1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Blank2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataZaezda = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cena = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbku_StrokaSchetFakturi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbku_ZaezdyClientov",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanatoryKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SanatoryNaimenovanie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomerPutevki = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataZaezdaPlan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataZaezdaFakt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StoimostPutevki = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OplachenaStoimost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClientFIO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientPasport = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbku_ZaezdyClientov", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblBrons",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Sanat = table.Column<int>(type: "int", nullable: true),
                    Num_Put = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BRON_FLG = table.Column<bool>(type: "bit", nullable: true),
                    BRON_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXPIRE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRIM = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBrons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblCloseDays",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PREVDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CURDAY = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NEXTDAY = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCloseDays", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tblCloseHistory",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CLOSED_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REMARK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCloseHistory", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tblCloseKvartal",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PREVKV_YEAR = table.Column<int>(type: "int", nullable: true),
                    PREVKV = table.Column<int>(type: "int", nullable: true),
                    CURKV_YEAR = table.Column<int>(type: "int", nullable: true),
                    CURKV = table.Column<int>(type: "int", nullable: true),
                    NEXTKV_YEAR = table.Column<int>(type: "int", nullable: true),
                    NEXTKV = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCloseKvartal", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tblCloseKvartalHistory",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CLOSED_KVYEAR = table.Column<int>(type: "int", nullable: true),
                    CLOSED_KV = table.Column<int>(type: "int", nullable: true),
                    REMARK = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCloseKvartalHistory", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tblFace_Med",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Face = table.Column<long>(type: "bigint", nullable: true),
                    Med_Doc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Med_Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Med_San = table.Column<int>(type: "int", nullable: true),
                    Opl_Doc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opl_Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Opl_Symma = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tal_Doc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tal_Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentBase = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFace_Med", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblFase",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Put = table.Column<long>(type: "bigint", nullable: true),
                    ID_Sanat = table.Column<int>(type: "int", nullable: true),
                    Num_Put = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rabota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dolj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stav_Vid = table.Column<int>(type: "int", nullable: true),
                    Pas_Ser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pas_Num = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pas_KD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pas_Data = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gost_Pr = table.Column<int>(type: "int", nullable: true),
                    ID_Osnov = table.Column<long>(type: "bigint", nullable: true),
                    Data_Post = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PNFL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFase", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblHolidays",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SET_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DATE_TYPE = table.Column<short>(type: "smallint", nullable: false),
                    STATUS = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHolidays", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tblPos_Put",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_San = table.Column<int>(type: "int", nullable: true),
                    Num_Put = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataNach = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataKon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SMFIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ID_Post = table.Column<long>(type: "bigint", nullable: true),
                    ID_Sanat = table.Column<int>(type: "int", nullable: true),
                    DataSan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataSanEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Symma_Put = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ID_PolVoz = table.Column<bool>(type: "bit", nullable: true),
                    Data_Post = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Departmentbase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIMESTAMP_T = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPos_Put", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblPost",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_KV = table.Column<long>(type: "bigint", nullable: true),
                    Data_Post = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Num_Doc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_Pos = table.Column<long>(type: "bigint", nullable: true),
                    Num_Kol = table.Column<int>(type: "int", nullable: true),
                    Symma = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Name_Dov = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vid = table.Column<bool>(type: "bit", nullable: false),
                    SmFil = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeginNumber = table.Column<int>(type: "int", nullable: true),
                    EndNumber = table.Column<int>(type: "int", nullable: true),
                    PROVODKA_ID = table.Column<long>(type: "bigint", nullable: true),
                    TipVidachi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dogovor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DovNum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DovDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPost", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Face",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Face = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Face", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Kv",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Kv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Kv", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_MFO",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code_MFO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name_MFO = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_MFO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Org",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Ot = table.Column<int>(type: "int", nullable: true),
                    Name_Org = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    INN = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    RS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MFO = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Bank = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    OKONX = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    Region = table.Column<int>(type: "int", nullable: true),
                    DT_OST = table.Column<int>(type: "int", nullable: true),
                    KT_OST = table.Column<int>(type: "int", nullable: true),
                    E_Mail = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentBase = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IS_PRAVSOYUZ = table.Column<bool>(type: "bit", nullable: false),
                    ParentID = table.Column<long>(type: "bigint", nullable: true),
                    PRAVORGLEVEL = table.Column<int>(type: "int", nullable: true),
                    DIRECTOR = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BUXGALTER = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DOVERENNOE_LISO = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ORIG_INN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OWNER_CODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TIMESTAMP_CH = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DB_BAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CT_BAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TO_DEPCODE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FACE_TYPE = table.Column<int>(type: "int", nullable: true),
                    Pas_ser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pas_Num = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pas_KD = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pas_Data = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Org", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Os",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Os = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Os", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Otras",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Ot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentBase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Otras", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Pr",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Pr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Pr", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Reg",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name_ru = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uz = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_uzl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Reg", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_San",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_San = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Num_Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    KurortSanID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_San", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblSprav_Vil",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_Vil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeVil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSprav_Vil", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_avarageprice",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SET_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_avarageprice", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_lager_otchet",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CAMP_ID = table.Column<int>(type: "int", nullable: true),
                    YEAR = table.Column<int>(type: "int", nullable: true),
                    KVARTAL = table.Column<int>(type: "int", nullable: true),
                    CHILDREN_COUNT = table.Column<int>(type: "int", nullable: true),
                    SUMMA = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IS_CLOSED = table.Column<bool>(type: "bit", nullable: false),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_lager_otchet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_putyovka_ostatok_loader",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID_SANAT = table.Column<int>(type: "int", nullable: true),
                    NUM_PUT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CUR_YEAR = table.Column<int>(type: "int", nullable: true),
                    CUR_KV = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_putyovka_ostatok_loader", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_raznoska_putyovki",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FACE_ID = table.Column<long>(type: "bigint", nullable: true),
                    PUT_ID = table.Column<long>(type: "bigint", nullable: true),
                    ID_SANAT = table.Column<int>(type: "int", nullable: true),
                    NUM_PUT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DATE_ZAEZD = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IS_MED = table.Column<bool>(type: "bit", nullable: false),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    TIMESTAMP_T = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_raznoska_putyovki", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_yearlyplanotrasl",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YEAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KVARTAL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SANATORIUM_ID = table.Column<long>(type: "bigint", nullable: true),
                    CAMP_ID = table.Column<long>(type: "bigint", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QUANTITY = table.Column<long>(type: "bigint", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    OTRASL_ID = table.Column<long>(type: "bigint", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DEPARTMANT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_yearlyplanotrasl", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_yearlyplanregion",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YEAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KVARTAL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SANATORIUM_ID = table.Column<long>(type: "bigint", nullable: true),
                    CAMP_ID = table.Column<long>(type: "bigint", nullable: true),
                    TIP_MEST_ID = table.Column<long>(type: "bigint", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QUANTITY = table.Column<long>(type: "bigint", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    REGION_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_yearlyplanregion", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_zaezdi",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZAEZD_BEGIN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ZAEZD_END = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PLACE_TYPE_ID = table.Column<long>(type: "bigint", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    SANATORY_ID = table.Column<int>(type: "int", nullable: true),
                    CAMP_ID = table.Column<long>(type: "bigint", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_zaezdi", x => x.REC_ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_zayavki",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_BASE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ORG_ID = table.Column<long>(type: "bigint", nullable: true),
                    YEAR = table.Column<int>(type: "int", nullable: true),
                    KVARTAL = table.Column<int>(type: "int", nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_zayavki", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbpt_zayavki_detail",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZAYAVKA_ID = table.Column<long>(type: "bigint", nullable: true),
                    ID_SANAT = table.Column<int>(type: "int", nullable: true),
                    QUANTITY = table.Column<int>(type: "int", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_zayavki_detail", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "useractions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    action_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    action_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_useractions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    functionality_id = table.Column<long>(type: "bigint", nullable: true),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_permission_functionality_functionality_id",
                        column: x => x.functionality_id,
                        principalTable: "functionality",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_role_permission_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    version = table.Column<int>(type: "int", nullable: false),
                    account_expired = table.Column<bool>(type: "bit", nullable: false),
                    account_locked = table.Column<bool>(type: "bit", nullable: false),
                    changed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_logged = table.Column<DateTime>(type: "datetime2", nullable: true),
                    last_logged_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_super_admin = table.Column<bool>(type: "bit", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    s_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    s_user_id = table.Column<long>(type: "bigint", nullable: true),
                    usertype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bkut_id = table.Column<long>(type: "bigint", nullable: false),
                    dep_id = table.Column<long>(type: "bigint", nullable: true),
                    role_id = table.Column<long>(type: "bigint", nullable: true),
                    REGION_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BRANCH_ID = table.Column<long>(type: "bigint", nullable: false),
                    NATIVE_ID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tbpt_putyovki",
                columns: table => new
                {
                    REC_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DEPARTMENT_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SANATORIUM_ID = table.Column<int>(type: "int", nullable: true),
                    CAMP_ID = table.Column<long>(type: "bigint", nullable: true),
                    USER_ID = table.Column<long>(type: "bigint", nullable: true),
                    SERIAL_BEGIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SERIAL_END = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REST_BEGIN_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REST_END_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MODIFY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QUANTITY_IN = table.Column<int>(type: "int", nullable: true),
                    REMAIN_BAL = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QUANTITY_OUT = table.Column<int>(type: "int", nullable: true),
                    QUANTITY_REMAIN = table.Column<int>(type: "int", nullable: true),
                    SMFIL = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbpt_putyovki", x => x.REC_ID);
                    table.ForeignKey(
                        name: "FK_tbpt_putyovki_tblSprav_San_SANATORIUM_ID",
                        column: x => x.SANATORIUM_ID,
                        principalTable: "tblSprav_San",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_tbpt_putyovki_users_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_document_chunks_source_name",
                table: "chat_document_chunks",
                column: "source_name");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_functionality_id",
                table: "role_permission",
                column: "functionality_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_role_id",
                table: "role_permission",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbpt_putyovki_SANATORIUM_ID",
                table: "tbpt_putyovki",
                column: "SANATORIUM_ID");

            migrationBuilder.CreateIndex(
                name: "IX_tbpt_putyovki_USER_ID",
                table: "tbpt_putyovki",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_refresh_tokens");

            migrationBuilder.DropTable(
                name: "chat_document_chunks");

            migrationBuilder.DropTable(
                name: "p_provodok");

            migrationBuilder.DropTable(
                name: "provodok");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "sp_branch");

            migrationBuilder.DropTable(
                name: "sp_contracttype");

            migrationBuilder.DropTable(
                name: "sp_country");

            migrationBuilder.DropTable(
                name: "sp_degree");

            migrationBuilder.DropTable(
                name: "sp_departmenttype");

            migrationBuilder.DropTable(
                name: "sp_employees");

            migrationBuilder.DropTable(
                name: "sp_nation");

            migrationBuilder.DropTable(
                name: "sp_okonx");

            migrationBuilder.DropTable(
                name: "sp_opf");

            migrationBuilder.DropTable(
                name: "sp_position");

            migrationBuilder.DropTable(
                name: "sp_sanatorium_profil");

            migrationBuilder.DropTable(
                name: "sp_sanatorium_type");

            migrationBuilder.DropTable(
                name: "sp_section");

            migrationBuilder.DropTable(
                name: "sp_soato");

            migrationBuilder.DropTable(
                name: "sp_soogu");

            migrationBuilder.DropTable(
                name: "t_camp");

            migrationBuilder.DropTable(
                name: "t_departmentbase");

            migrationBuilder.DropTable(
                name: "t_egrpo");

            migrationBuilder.DropTable(
                name: "t_normativdoc");

            migrationBuilder.DropTable(
                name: "t_sanatorium");

            migrationBuilder.DropTable(
                name: "tbbu_balance");

            migrationBuilder.DropTable(
                name: "tbbu_bank_payments");

            migrationBuilder.DropTable(
                name: "tbbu_debitor_creditor");

            migrationBuilder.DropTable(
                name: "tbbu_finOtchetSetting");

            migrationBuilder.DropTable(
                name: "tbbu_platejka");

            migrationBuilder.DropTable(
                name: "tbcs_dictionary");

            migrationBuilder.DropTable(
                name: "tbcs_dictionarytype");

            migrationBuilder.DropTable(
                name: "tbku_DocumentSchetFaktura");

            migrationBuilder.DropTable(
                name: "tbku_GrafikZaezdovSanatoriev");

            migrationBuilder.DropTable(
                name: "tbku_journal");

            migrationBuilder.DropTable(
                name: "tbku_SpisokRaspredeleniya");

            migrationBuilder.DropTable(
                name: "tbku_StrokaProfsoyza");

            migrationBuilder.DropTable(
                name: "tbku_StrokaSchetFakturi");

            migrationBuilder.DropTable(
                name: "tbku_ZaezdyClientov");

            migrationBuilder.DropTable(
                name: "tblBrons");

            migrationBuilder.DropTable(
                name: "tblCloseDays");

            migrationBuilder.DropTable(
                name: "tblCloseHistory");

            migrationBuilder.DropTable(
                name: "tblCloseKvartal");

            migrationBuilder.DropTable(
                name: "tblCloseKvartalHistory");

            migrationBuilder.DropTable(
                name: "tblFace_Med");

            migrationBuilder.DropTable(
                name: "tblFase");

            migrationBuilder.DropTable(
                name: "tblHolidays");

            migrationBuilder.DropTable(
                name: "tblPos_Put");

            migrationBuilder.DropTable(
                name: "tblPost");

            migrationBuilder.DropTable(
                name: "tblSprav_Face");

            migrationBuilder.DropTable(
                name: "tblSprav_Kv");

            migrationBuilder.DropTable(
                name: "tblSprav_MFO");

            migrationBuilder.DropTable(
                name: "tblSprav_Org");

            migrationBuilder.DropTable(
                name: "tblSprav_Os");

            migrationBuilder.DropTable(
                name: "tblSprav_Otras");

            migrationBuilder.DropTable(
                name: "tblSprav_Pr");

            migrationBuilder.DropTable(
                name: "tblSprav_Reg");

            migrationBuilder.DropTable(
                name: "tblSprav_Vil");

            migrationBuilder.DropTable(
                name: "tbpt_avarageprice");

            migrationBuilder.DropTable(
                name: "tbpt_lager_otchet");

            migrationBuilder.DropTable(
                name: "tbpt_putyovka_ostatok_loader");

            migrationBuilder.DropTable(
                name: "tbpt_putyovki");

            migrationBuilder.DropTable(
                name: "tbpt_raznoska_putyovki");

            migrationBuilder.DropTable(
                name: "tbpt_yearlyplanotrasl");

            migrationBuilder.DropTable(
                name: "tbpt_yearlyplanregion");

            migrationBuilder.DropTable(
                name: "tbpt_zaezdi");

            migrationBuilder.DropTable(
                name: "tbpt_zayavki");

            migrationBuilder.DropTable(
                name: "tbpt_zayavki_detail");

            migrationBuilder.DropTable(
                name: "useractions");

            migrationBuilder.DropTable(
                name: "functionality");

            migrationBuilder.DropTable(
                name: "tblSprav_San");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
