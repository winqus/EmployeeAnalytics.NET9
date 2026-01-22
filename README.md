# EmpAnalytics

Employee analytics application with .NET API backend and React frontend.

## Requirements

- .NET 9 SDK
- Node.js 22 LTS
- Docker **or** SQL Server 2019+

## Configuration

| File | Purpose |
|------|---------|
| `.env` | Used by `docker-compose.yaml` (SQL Server password, API URL for WebClient) |
| `EmpAnalytics.API/appsettings.Development.json` | API config for local development. Adjust if changing DB connection string or CORS origins |
| `EmpAnalytics.API/appsettings.json` | API config base/production |
| `EmpAnalytics.WebClient/.env` | Frontend config (only needed if API URL changes) |

## Development Setup

### 1. Database

Start the database container (recommended way, or use your own SQL Server 2019+ service)

```bash
docker-compose -f docker-compose.development.yaml up -d
```

### 2. Backend (EmpAnalytics.API)

If needed, configured via `appsettings.Development.json`. 
Database should be already running because API on start will attempt to seed the DB.
Seeding might take upto 1 min depending on your hardware.

```bash
cd EmpAnalytics.API
dotnet run
```

API runs on `http://localhost:5112`, docs at `http://localhost:5112/swagger`

### 3. Frontend (EmpAnalytics.WebClient)

```bash
cd EmpAnalytics.WebClient
pnpm install
pnpm run dev
```

Frontend runs on `http://localhost:5173`

## Or Running Full App with Docker

Create `.env` from `.env.example`:

```bash
cp .env.example .env
```

Start all services:

```bash
docker-compose up -d
```

- Frontend: `http://localhost:3000`
- API: `http://localhost:8080`

### Notes

**Windows:** Development was done on macOS. On Windows, there may be issues with:
- Internal Docker network resolution
- `platform: linux/amd64` setting in docker-compose

**JetBrains Rider:** Running docker through Rider may generate docker override files that prevent `docker-compose.yaml` from working correctly. Run docker-compose from terminal instead.

If you encounter problems with docker-compose, try running services individually in development mode instead.
