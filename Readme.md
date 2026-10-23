# KasabaKIAT — Trade Union ERP

Trade union management system for Uzbekistan. Manages sanatorium vouchers, yearly plans, accounting, and organizational data. Includes a local AI assistant with RAG powered by Ollama.

- Production URL: https://kiat2.kasaba.uz
- Two main modules: **Vouchers (Путевки)** and **Accounting (Бухгалтерия)**

---

## Prerequisites

### Runtime & Tools

| Tool | Version | Notes |
|------|---------|-------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0+ | Backend runtime |
| [Node.js](https://nodejs.org/) | 24+ | Frontend runtime |
| [npm](https://www.npmjs.com/) | 11+ | Frontend package manager |
| [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | 2019+ | Database (2022 recommended) |
| [Ollama](https://ollama.com/) | Latest | Local LLM runtime for AI assistant |

### Ollama Models

After installing Ollama, pull both required models:

```bash
ollama pull llama3.2
ollama pull nomic-embed-text
```

| Model | Size | Purpose |
|-------|------|---------|
| `llama3.2` | ~2 GB | Chat / answer generation |
| `nomic-embed-text` | ~274 MB | Text embeddings for RAG document search |

Ollama must be running on `http://localhost:11434` before starting the backend.

---

## Backend — KasabaKIAT.Core

ASP.NET Core 10 Web API.

### NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.x | ORM + SQL Server provider |
| `Microsoft.EntityFrameworkCore.Design` | 9.x | EF migrations tooling (dev only) |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.x | JWT auth middleware |
| `Microsoft.IdentityModel.Tokens` | 8.x | JWT token validation |
| `System.IdentityModel.Tokens.Jwt` | 8.x | JWT token generation |
| `Microsoft.SemanticKernel` | 1.x | AI orchestration framework |
| `Microsoft.SemanticKernel.Connectors.Ollama` | 1.x-alpha | Ollama chat + embedding connector |
| `PdfPig` | 0.1.x | PDF text extraction for RAG ingestion |
| `DocumentFormat.OpenXml` | 3.x | DOCX text extraction for RAG ingestion |
| `Swashbuckle.AspNetCore` | 7.x | Swagger / OpenAPI UI |
| `Serilog.AspNetCore` | 8.x | Structured logging |
| `Serilog.Sinks.File` | 5.x | Log to file |
| `BCrypt.Net-Next` | 4.x | Password hashing |
| `ClosedXML` | 0.104.x | Excel report generation |
| `Mapster` | 7.x | Object mapping (DTO ↔ entity) |
| `Mapster.DependencyInjection` | 1.x | DI integration for Mapster |
| `FluentValidation.AspNetCore` | 11.x | Request validation |
| `AspNetCore.HealthChecks.SqlServer` | 8.x | SQL Server health check endpoint |
| `Microsoft.Data.SqlClient` | 6.x | Low-level SQL client |
| `System.ServiceModel.Http` | 10.x | WCF/SOAP service client |

### Configuration

Fill in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<host>\\SQLEXPRESS;Database=KasabaKIAT;User Id=sa;Password=<password>;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "<at-least-32-character-secret>"
  },
  "AllowedOrigins": [ "http://localhost:5173" ],
  "Ollama": {
    "BaseUrl": "http://localhost:11434",
    "ChatModel": "llama3.2",
    "EmbeddingModel": "nomic-embed-text",
    "MaxContextTokens": 4096
  }
}
```

### Database Setup

The database schema pre-exists; EF Core only manages the `chat_document_chunks` table. Apply it once:

```bash
cd KasabaKIAT.Core
dotnet ef database update
```

### Run

```bash
cd KasabaKIAT.Core
dotnet run
```

API: `http://localhost:5166` — Swagger UI: `http://localhost:5166/swagger`

---

## Frontend — KasabaKIAT.Frontend

React 19 + TypeScript + Vite + Ant Design 6.

### npm Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `react` | 19.x | UI framework |
| `react-dom` | 19.x | DOM renderer |
| `react-router-dom` | 7.x | Client-side routing |
| `antd` | 6.x | UI component library |
| `@ant-design/icons` | 6.x | Icon set for Ant Design |
| `axios` | 1.x | HTTP client with auth interceptors |
| `@tanstack/react-query` | 5.x | Server state / data fetching |
| `@tanstack/react-table` | 9.x | Headless table component |
| `dayjs` | 1.x | Date formatting and manipulation |
| `i18next` | 26.x | Internationalization (RU / UZ) |
| `react-i18next` | 17.x | React bindings for i18next |
| `i18next-browser-languagedetector` | 8.x | Auto-detect browser language |
| `react-resizable` | 4.x | Resizable table columns |
| `xlsx` | 0.18.x | Excel file export |

### Dev Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `vite` | 8.x | Build tool and dev server |
| `@vitejs/plugin-react` | 6.x | React Fast Refresh |
| `typescript` | 6.x | Type checker |
| `oxlint` | 1.x | Fast JS/TS linter |
| `@playwright/test` | 1.x | End-to-end test runner |

### Environment

Create `.env.local` in `KasabaKIAT.Frontend/`:

```
VITE_API_URL=
```

Leave empty for local dev — Vite proxies `/api/*` to `http://localhost:5166` automatically.

### Run

```bash
cd KasabaKIAT.Frontend
npm install
npm run dev
```

Frontend: `http://localhost:5173` (falls back to `5174` if port is taken).

---

## AI Assistant

The floating chat button (bottom-right corner on every page) provides a local AI assistant — no external API calls.

**Pipeline per message:**
1. Query is embedded via `nomic-embed-text`
2. Cosine similarity search finds relevant chunks from ingested documents (`chat_document_chunks` table)
3. Live DB data (vouchers, sanatoriums, yearly plan) is fetched and appended as context
4. `llama3.2` generates a streaming response
5. Tokens stream to the browser via Server-Sent Events

**Ingesting documents** (admin / superadmin only — via Swagger or a future UI):

```
POST /api/chat/ingest
Authorization: Bearer <token>
Content-Type: multipart/form-data
file: <PDF | DOCX | TXT, max 50 MB>
```

---

## Project Structure

```
TradeUnionUzbekistan/
├── KasabaKIAT.Core/              # ASP.NET Core backend
│   ├── Controllers/               # API endpoints
│   ├── Data/                      # EF Core DbContext
│   ├── DTOs/                      # Request/response models
│   ├── Migrations/                # EF migrations
│   ├── Models/                    # Entity models
│   ├── Services/                  # Business logic
│   ├── appsettings.json
│   └── Program.cs
├── KasabaKIAT.Frontend/          # React frontend
│   ├── src/
│   │   ├── api/                   # Axios + fetch API clients
│   │   ├── auth/                  # Auth context and route guards
│   │   ├── components/            # Shared UI (incl. AiChat widget)
│   │   └── pages/                 # Route pages
│   ├── vite.config.ts
│   └── package.json
├── KasabaKIAT.Core.IntegrationTests/
├── KasabaKIAT.Core.UnitTests/
└── KasabaKIAT.sln
```
