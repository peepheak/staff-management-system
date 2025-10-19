# 🧑‍💼 Staff Management System

Staff Management system designed for modern organizations with enterprise-grade features and best practices.

**Built with:**
- **Backend:** ASP.NET Core Web API (C#) + Entity Framework Core
- **Database:** PostgreSQL
- **Frontend:** Vue 3 (Vite + TypeScript + Tailwind CSS)
- **Testing:** xUnit (unit + integration)
- **Export:** SheetJS (Excel) & jsPDF (PDF)
- **CI/CD:** GitHub Actions

---

## ✨ Features

- ✅ Complete CRUD operations (Create, Read, Update, Delete) for staff records
- ✅ Advanced search and filtering (Staff ID, Gender, Birthday range)
- ✅ Export functionality to Excel and PDF formats
- ✅ Comprehensive unit and integration tests on backend
- ✅ End-to-end (E2E) testing with Playwright
- ✅ PostgreSQL database with Entity Framework Core migrations
- ✅ Automated CI/CD pipeline with GitHub Actions
- ✅ Type-safe frontend with TypeScript
- ✅ Responsive UI with Tailwind CSS

---

## 🛠️ Tech Stack

| Layer        | Technology            | Version |
| ------------ | --------------------- | ------- |
| **Frontend** | Vue 3 (Vite)          | 3.x     |
| **Language** | TypeScript            | 5.x     |
| **Styling**  | Tailwind CSS          | 3.x     |
| **Backend**  | ASP.NET Core Web API  | .NET 8  |
| **Language** | C#                    | 12.0    |
| **ORM**      | Entity Framework Core | 8.x     |
| **Database** | PostgreSQL            | 15+     |
| **Testing**  | xUnit.                | Latest  |
| **Export**   | SheetJS, jsPDF        | Latest  |
| **CI/CD**    | GitHub Actions        | -       |

---

## 📋 Prerequisites

Ensure you have the following installed on your system:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) or later
- [Node.js 18+](https://nodejs.org) with npm
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- Git for version control
- (Optional) [Docker](https://www.docker.com/) for containerized PostgreSQL setup

---

## 🗄️ Database Setup

### Option 1: Local PostgreSQL Installation

1. **Create the database:**
   ```bash
   createdb STAFF_DB
   ```

2. **Configure connection string:**
   Update `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=STAFF_DB;Username=postgres;Password=your_password"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information"
       }
     }
   }
   ```

3. **Apply migrations:**
   ```bash
   cd Backend/API/
   dotnet ef database update
   ```

### Option 2: Docker (Recommended for Development)

1. **Create and start PostgreSQL container:**
   ```bash
   docker run --name staff-postgres \
     -e POSTGRES_DB=STAFF_DB \
     -e POSTGRES_USER=postgres \
     -e POSTGRES_PASSWORD=root \
     -p 5432:5432 \
     -d postgres:15-alpine
   ```

2. **Verify connection:**
   ```bash
   docker exec -it staff-postgres psql -U postgres -d STAFF_DB -c "SELECT version();"
   ```

3. **Apply migrations:**
   ```bash
   cd Backend/API/
   dotnet ef database update
   ```

### Creating New Migrations

When modifying database models, create and apply new migrations:

```bash
 cd Backend/API/
dotnet ef migrations add <migration-name>
dotnet ef database update
```

---

## 🚀 Getting Started

### Clone the Repository

```bash
git clone https://github.com/peepheak/staff-management-system
cd staff-management-system
```

### Backend Setup

1. **Install dependencies and build:**
   ```bash
    cd Backend/API/
   dotnet restore
   dotnet build
   ```

2. **Run the API server:**
   ```bash
   dotnet run
   ```

   The API will be available at: **http://localhost:5000**

   API Documentation (Swagger): **http://localhost:5000/swagger**

### Frontend Setup

1. **Install dependencies:**
   ```bash
   cd Frontend/
   npm install
   ```

2. **Start development server:**
   ```bash
   npm run dev
   ```

   The application will be available at: **http://localhost:5173**

---

## 🧪 Testing

### Backend Tests

Run all tests (unit, integration, and E2E):

```bash
cd Backend/API/
dotnet test
```

Run specific test project:

```bash
# Unit tests
dotnet test API.Test.Unit/API.Test.Unit.csproj

# Integration tests
dotnet test API.Test.Integration/API.Test.Integration.csproj
```
---

## 📦 Project Structure

```
staff-management-system/
├── Backend/
│   ├── API/                          # Main API project
│   ├── API.Test.Unit/                # Unit tests
│   ├── API.Test.Integration/         # Integration tests
│   └── Backend.sln                   # Solution file
├── Frontend/
│   ├── src/
│   │   ├── components/               # Vue components
│   │   ├── pages/                    # Page views
│   │   ├── services/                 # API services
│   │   ├── stores/                   # Pinia store (state management)
│   │   └── App.vue                   # Main app component
│   ├── package.json
│   └── vite.config.ts                # Vite configuration
├── .github/
│   └── workflows/
│       └── ci.yml                    # CI/CD pipeline
└── README.md
```

---

## ⚡ CI/CD Pipeline

The project includes an automated GitHub Actions workflow for continuous integration and testing.

**Workflow file:** `.github/workflows/ci.yml`

**Pipeline stages:**
1. Checkout code
2. Setup .NET 8
3. Restore dependencies
4. Build solution
5. Run unit tests
6. Run integration tests
7. Build and publish backend
8. Run E2E tests (if applicable)

View workflow runs in GitHub Actions tab of your repository.

---

## 📝 API Endpoints

### Staff Management

| Method | Endpoint            | Description             |
| ------ | ------------------- | ----------------------- |
| GET    | `/api/staffs`       | Get all staff records   |
| GET    | `/api/staff/{id}`   | Get staff by ID         |
| POST   | `/api/staff`        | Create new staff record |
| PUT    | `/api/staff/{id}`   | Update staff record     |
| DELETE | `/api/staff/{id}`   | Delete staff record     |
| GET    | `/api/staffs/pdf`   | Export to PDF           |
| GET    | `/api/staffs/excel` | Export to Excel.        |

---

## 📧 Contact & Author

**Chheun Sopheak**

- 📧 Email: [chheunsopheak8@gmail.com](mailto:chheunsopheak8@gmail.com)
- 🌐 GitHub: [@Sopheak](https://github.com/peepheak)
- 💼 LinkedIn: [Chheun Sopheak](https://www.linkedin.com/in/chheun-sopheak-697128339/)

---

## 📄 License

This project is licensed under @ChheunSopheak.

---