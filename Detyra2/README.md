# AI Search Algorithms - Detyra 2

## Përshkrimi i Projektit

Ky projekt implementon tre probleme fundamentale të AI-së:
1. **Blocked N-Queens Problem** - Duke përdorur A* algoritmin me tre heuristika të ndryshme
2. **SAT Problem** - Problem i vendosjes së mysafirëve në tavolina me constraint-e
3. **Killer Sudoku** - Duke përdorur Constraint Satisfaction Problem (CSP)

## Teknologjitë e Përdorura

### Backend
- ASP.NET Core 8.0 Web API
- C# 12
- Service-oriented architecture me Dependency Injection

### Frontend
- Angular 18
- TypeScript
- Reactive programming me RxJS
- Modern UI/UX design

## Struktura e Projektit

```
Detyra2/
├── API/
│   └── API/
│       ├── Controllers/          # API controllers
│       ├── Services/            # Business logic implementations
│       ├── Models/              # Data models
│       └── Program.cs           # Application configuration
└── client/
    └── src/
        ├── app/
        │   ├── components/      # Angular components
        │   ├── services/        # HTTP services
        │   └── models/          # TypeScript models
        └── styles.css           # Global styles
```

## Instalimi dhe Ekzekutimi

### Prerequisites
- .NET 8.0 SDK
- Node.js (v18 ose më të ri)
- Angular CLI

### Backend (API)

1. Navigo në dosjen e API:
```bash
cd API/API
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Run the API:
```bash
dotnet run
```

API do të jetë i disponueshëm në: `http://localhost:5220`
Swagger UI: `http://localhost:5220/swagger`

### Frontend (Angular)

1. Navigo në dosjen e klientit:
```bash
cd client
```

2. Instalo dependencies:
```bash
npm install
```

3. Run the application:
```bash
ng serve
```

Aplikacioni do të jetë i disponueshëm në: `http://localhost:4200`

## Karakteristikat

### 1. Blocked N-Queens Problem
- **Algoritmi**: A* (A-Star)
- **Heuristikat**:
  1. **Conflicts Heuristic**: Numri i mbetejeve pa mbretëresha
  2. **Attacking Pairs Heuristic**: Numri i çifteve që sulmojnë njëra-tjetrën
  3. **Combined Heuristic**: Kombinim i heuristikave (admissible)
- **Features**:
  - Zgjedhja e madhësisë së tabelës (4-20)
  - Shtimi i pozicioneve të bllokuara
  - Vizualizim në kohë reale
  - Statistika të performancës

### 2. SAT Problem - Wedding Seating Arrangement
- **Algoritmi**: SAT Solving me Backtracking
- **Constraints**:
  - Çdo mysafir ulët në një tavolinë
  - Respektimi i limitit të vendeve për tavolinë
  - Mysafirë që nuk mund të ulen së bashku
  - Mysafirë që duhet të ulen së bashku
  - Balanci i gjinisë (±3 diferenca maksimale)
- **Features**:
  - Gjenerimi i SAT formula
  - Konfigurimi i constraint-eve
  - Vizualizimi i tavolinave
  - Ngjyrosje sipas gjinisë

### 3. Killer Sudoku
- **Algoritmi**: CSP me Backtracking dhe Forward Checking
- **Constraints**:
  - Rregullat e Sudoku-s klasike
  - Kafaze me shuma specifike
  - Nuk lejohen përsëritje brenda kafazës
- **Features**:
  - Puzzle i paracaktuar
  - Mundësia e shtimit të kafazave të reja
  - Vizualizim me ngjyra
  - Statistika të backtracking-ut

## API Endpoints

### N-Queens
- `POST /api/NQueens/solve` - Zgjidh problemin e N-Queens
- `GET /api/NQueens/heuristics` - Merr listën e heuristikave

### SAT
- `POST /api/SAT/solve` - Zgjidh problemin e SAT
- `POST /api/SAT/generate-formula` - Gjeneron SAT formulën

### Killer Sudoku
- `POST /api/KillerSudoku/solve` - Zgjidh Killer Sudoku
- `GET /api/KillerSudoku/sample` - Merr një shembull puzzle

## Algoritmet

### A* Algorithm (N-Queens)
```csharp
f(n) = g(n) + h(n)
```
- `g(n)`: Kostoja aktuale (thellësia e pemës së kërkimit)
- `h(n)`: Heuristika (vlerësimi i kostos së mbetur)
- `f(n)`: Kostoja totale e vlerësuar

### SAT Solving
Përdor backtracking me constraint propagation për të gjetur një zgjidhje që plotëson të gjitha constraint-et boolean.

### CSP (Constraint Satisfaction Problem)
Përdor:
- **MRV (Minimum Remaining Values)**: Zgjedh variablën me më pak vlera të mundshme
- **Forward Checking**: Përhap constraint-et pas çdo assignment-i
- **Backtracking**: Kthehet pas kur hasim në konflikt

## Screenshots

### Home Page
Faqja kryesore me tre problemet.

### N-Queens Solver
Vizualizim i tabelës së shahut me mbretëresha dhe pozicione të bllokuara.

### SAT Solver
Tavolinat me mysafirë të ndarë sipas gjinisë.

### Killer Sudoku
Grid 9x9 me kafaze të ngjyrosura.

## Performanca

- **N-Queens (N=8)**: ~100-500 nodes explored, <50ms
- **SAT (20 guests, 4 tables)**: ~1000-5000 assignments tested, <200ms
- **Killer Sudoku**: ~100-1000 backtracks, <500ms

## Autori

**Kursi**: Artificial Intelligence - Master  
**Detyra**: Detyra 2 - AI (20/12/2025)  
**Grupi**: IA-Grupi13  
**Tema**: Uninformed Search Techniques, CSP Problems, SAT Problems

## License

Ky projekt është krijuar për qëllime akademike.

