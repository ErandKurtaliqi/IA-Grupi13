# 📂 Complete Project Structure

```
Detyra1/
│
├── 📘 README.md                    # Main project documentation
├── 📗 QUICKSTART.md               # Quick setup guide
├── 📙 PROJECT_SUMMARY.md          # What we built
├── 📕 UI_SCREENS.md               # UI design documentation
├── 📔 TESTING_GUIDE.md            # How to test everything
│
├── 📁 Detyra3/                    # Backend (ASP.NET Core)
│   ├── Detyra3.sln                # Solution file
│   └── 📁 Detyra3/
│       ├── Detyra3.csproj         # Project file
│       ├── Program.cs             # 🔧 Entry point (CORS configured here)
│       ├── appsettings.json       # Configuration
│       ├── Detyra3.http           # HTTP test file
│       │
│       ├── 📁 Controllers/        # API Endpoints
│       │   ├── SocialGolfersController.cs    # ⛳ Social Golfers API
│       │   ├── SudokuController.cs           # 🔢 Sudoku API
│       │   └── LatinSquareController.cs      # 📐 Latin Square API
│       │
│       ├── 📁 Services/           # Algorithm Implementations
│       │   ├── SocialGolfersService.cs       # DFS, DLS logic
│       │   ├── SudokuService.cs              # BFS logic
│       │   └── LatinSquareSolverService.cs   # IDDFS, Backtracking
│       │
│       ├── 📁 Interfaces/         # Service Contracts
│       │   ├── ISocialGolfersService.cs
│       │   ├── ISudokuService.cs
│       │   └── ILatinSquareSolverService.cs
│       │
│       └── 📁 Properties/
│           └── launchSettings.json # Port configuration (5157)
│
└── 📁 Assignment-UI/              # Frontend (Angular)
    └── 📁 assignment-UI/
        ├── package.json           # Dependencies
        ├── angular.json           # Angular config
        ├── tsconfig.json          # TypeScript config
        │
        ├── 📁 src/
        │   ├── index.html         # HTML entry point
        │   ├── main.ts            # TypeScript entry point
        │   ├── styles.css         # 🎨 Global styles
        │   │
        │   └── 📁 app/
        │       ├── app.ts         # 🏠 Root component
        │       ├── app.html       # Layout & navbar
        │       ├── app.css        # App-wide styles
        │       ├── app.routes.ts  # 🗺️ Route configuration
        │       ├── app.config.ts  # Angular configuration
        │       │
        │       ├── 📁 services/
        │       │   └── api.service.ts  # 🔌 HTTP API client
        │       │
        │       └── 📁 components/
        │           │
        │           ├── 📁 home/                    # 🏠 Landing page
        │           │   ├── home.component.ts
        │           │   ├── home.component.html
        │           │   └── home.component.css
        │           │
        │           ├── 📁 social-golfers/         # ⛳ Social Golfers UI
        │           │   ├── social-golfers.component.ts
        │           │   ├── social-golfers.component.html
        │           │   └── social-golfers.component.css
        │           │
        │           ├── 📁 sudoku/                 # 🔢 Sudoku UI
        │           │   ├── sudoku.component.ts
        │           │   ├── sudoku.component.html
        │           │   └── sudoku.component.css
        │           │
        │           └── 📁 latin-square/           # 📐 Latin Square UI
        │               ├── latin-square.component.ts
        │               ├── latin-square.component.html
        │               └── latin-square.component.css
        │
        └── 📁 public/
            └── favicon.ico
```

---

## 🔗 Data Flow

```
User Browser (localhost:4200)
         ↓
    Angular App
         ↓
   API Service (api.service.ts)
         ↓
    HTTP Request
         ↓
ASP.NET Core API (localhost:5157)
         ↓
    Controllers (routing)
         ↓
    Services (algorithms)
         ↓
    Computation
         ↓
   JSON Response
         ↓
    Angular Component
         ↓
    HTML Template
         ↓
    User sees result
```

---

## 🎯 Component Mapping

| Problem | Backend Controller | Backend Service | Frontend Component | Route |
|---------|-------------------|-----------------|-------------------|--------|
| Social Golfers | `SocialGolfersController.cs` | `SocialGolfersService.cs` | `social-golfers.component.*` | `/social-golfers` |
| Sudoku | `SudokuController.cs` | `SudokuService.cs` | `sudoku.component.*` | `/sudoku` |
| Latin Square | `LatinSquareController.cs` | `LatinSquareSolverService.cs` | `latin-square.component.*` | `/latin-square` |
| Home | - | - | `home.component.*` | `/` |

---

## 📡 API Endpoints → UI Mapping

### Social Golfers Component
```
┌─────────────────────────────┐
│ social-golfers.component.ts │
└──────────────┬──────────────┘
               │
               ├─ solveSocialGolfersDFS()
               │  → GET /api/socialgolfers/dfs
               │
               ├─ solveSocialGolfersDepthLimited()
               │  → GET /api/socialgolfers/depth-limited
               │
               └─ findMaxWeeks()
                  → GET /api/socialgolfers/find-max-weeks
```

### Sudoku Component
```
┌─────────────────────────┐
│ sudoku.component.ts     │
└──────────┬──────────────┘
           │
           ├─ solveSudoku(difficulty)
           │  → GET /api/sudoku/solve
           │
           └─ solveCustomSudoku(grid)
              → POST /api/sudoku/solve-custom
```

### Latin Square Component
```
┌──────────────────────────────┐
│ latin-square.component.ts    │
└──────────┬───────────────────┘
           │
           └─ solveLatinSquare(n, algo)
              → GET /api/latinsquare
```

---

## 🎨 Style Architecture

```
Global Styles (styles.css)
    ↓
App Styles (app.css)
    ↓
Component Styles
    ├── home.component.css
    ├── social-golfers.component.css
    ├── sudoku.component.css
    └── latin-square.component.css
```

**Shared Design Tokens:**
- Colors (purple, pink, red gradients)
- Fonts (Inter family)
- Spacing (consistent padding/margins)
- Shadows (consistent box-shadow)
- Border radius (8px, 12px, 16px)
- Transitions (0.2s, 0.3s ease)

---

## 🔄 State Management

### Using Angular Signals

```typescript
// Example from social-golfers.component.ts
numPlayers = signal(16);      // Reactive primitive
groupSize = signal(4);         // Auto-updates UI
loading = signal(false);       // Loading state
result = signal<any>(null);   // API response
```

**Benefits:**
- Automatic change detection
- No manual subscriptions needed
- Type-safe
- Modern Angular approach

---

## 🎯 Key Files to Understand

### Backend
1. **Program.cs** - Entry point, CORS, DI setup
2. **Controllers** - Define API endpoints
3. **Services** - Implement algorithms

### Frontend
1. **api.service.ts** - All API calls
2. **app.routes.ts** - Define navigation
3. **Components** - UI logic and display

---

## 🚀 Startup Sequence

### Backend Startup
```
1. dotnet run
2. Read Program.cs
3. Configure services (DI)
4. Setup CORS
5. Start Kestrel server
6. Listen on localhost:5157
7. Swagger UI available at /swagger
```

### Frontend Startup
```
1. npm start (runs ng serve)
2. Read angular.json
3. Load main.ts
4. Bootstrap App component
5. Initialize router
6. Render home page
7. Listen on localhost:4200
```

---

## 📦 Dependencies

### Backend (.NET)
- ASP.NET Core 8.0
- Swashbuckle (Swagger)
- No external algorithm libraries (pure C#)

### Frontend (Angular)
```json
"dependencies": {
  "@angular/common": "^20.3.0",
  "@angular/core": "^20.3.0",
  "@angular/forms": "^20.3.0",
  "@angular/router": "^20.3.0",
  "rxjs": "~7.8.0"
}
```

---

## 🔧 Configuration Files

### Backend
- `appsettings.json` - App settings
- `launchSettings.json` - Development ports
- `Detyra3.csproj` - Project dependencies

### Frontend
- `package.json` - npm dependencies
- `angular.json` - Build configuration
- `tsconfig.json` - TypeScript settings

---

## 📈 Complexity Overview

### Lines of Code (approximate)

**Backend:**
- Controllers: ~400 lines
- Services: ~800 lines
- Interfaces: ~50 lines
- **Total: ~1,250 lines**

**Frontend:**
- Components (TS): ~350 lines
- Templates (HTML): ~600 lines
- Styles (CSS): ~1,200 lines
- Services: ~60 lines
- **Total: ~2,210 lines**

**Grand Total: ~3,500 lines of code**

---

## 🎓 Learning Path

**To understand the project:**

1. **Start Here:**
   - README.md (overview)
   - QUICKSTART.md (get it running)

2. **Backend:**
   - Program.cs (setup)
   - One controller (e.g., SudokuController)
   - Corresponding service

3. **Frontend:**
   - app.routes.ts (navigation)
   - api.service.ts (HTTP calls)
   - One component (e.g., sudoku)

4. **Advanced:**
   - Algorithm implementations
   - State management patterns
   - Styling architecture

---

## 🎯 File Naming Conventions

### Backend (C#)
- PascalCase for everything
- Files end in `.cs`
- Controllers: `*Controller.cs`
- Services: `*Service.cs`
- Interfaces: `I*Service.cs`

### Frontend (TypeScript/Angular)
- kebab-case for file names
- component files: `*.component.ts/html/css`
- service files: `*.service.ts`
- Class names: PascalCase
- Properties/methods: camelCase

---

**Use this as your reference guide! 📚**

