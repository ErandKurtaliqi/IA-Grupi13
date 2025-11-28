# 🚀 Quick Start Guide

## Prerequisites
- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js v18+** - [Download](https://nodejs.org/)
- **Angular CLI** - Install with `npm install -g @angular/cli`

## 🎯 Running the Application

### Step 1: Start the Backend API

Open a terminal and navigate to the API directory:

```bash
cd API/API
dotnet restore
dotnet run
```

✅ The API will be running at: **http://localhost:5220**
✅ Swagger documentation: **http://localhost:5220/swagger**

### Step 2: Start the Angular Frontend

Open a **NEW** terminal and navigate to the client directory:

```bash
cd client
npm install
ng serve
```

✅ The application will be running at: **http://localhost:4200**

## 🎨 Features

### 🏠 Home Dashboard
- Beautiful animated background with gradient orbs
- Three problem cards with interactive hover effects
- Course information and project goals
- Technology stack showcase

### ♛ N-Queens Problem (A* Algorithm)
- **Default Parameters**: Automatically loads with N=8 and 2 blocked positions
- **3 Heuristics**: 
  - Conflicts (basic)
  - Attacking Pairs
  - Combined (admissible)
- **Interactive Board**: Click-friendly interface with animations
- **Real-time Stats**: Nodes explored, execution time

### 🎭 SAT Problem (Wedding Seating)
- **Default Parameters**: 20 guests, 4 tables with sample constraints
- **Constraint Management**: Cannot sit together / Must sit together
- **Gender Balance**: Automatic gender distribution (±3 difference)
- **Visual Tables**: Color-coded by gender
- **SAT Formula**: View the generated boolean formula

### 🔪 Killer Sudoku (CSP)
- **Default Parameters**: Pre-loaded sample puzzle that auto-solves
- **Cage System**: Sum constraints with colored cages
- **Smart Solving**: Backtracking with forward checking
- **MRV Heuristic**: Minimum Remaining Values strategy
- **Visual Feedback**: Animated cell filling

## 📊 What Makes This Special

### Backend (ASP.NET Core)
- **Clean Architecture**: Service-oriented with dependency injection
- **3 Algorithms Implemented**:
  - A* Search with PriorityQueue
  - SAT Solving with Backtracking
  - CSP with Constraint Propagation
- **RESTful API**: Swagger documentation included
- **CORS Enabled**: Configured for Angular frontend

### Frontend (Angular 18)
- **Standalone Components**: Modern Angular architecture
- **Reactive Programming**: RxJS for API calls
- **Beautiful UI**: Dark theme with glassmorphism effects
- **Animations**: Smooth transitions and hover effects
- **Responsive Design**: Works on all screen sizes
- **Navigation**: Clean navbar with active route highlighting

## 🎓 Educational Value

This project demonstrates:

1. **A* Search Algorithm**
   - Different heuristic functions
   - Admissible heuristics
   - Priority queue implementation
   - State space search

2. **SAT Problem Solving**
   - Boolean constraint formulation
   - Backtracking with pruning
   - Constraint propagation
   - Solution verification

3. **Constraint Satisfaction**
   - Forward checking
   - Domain reduction
   - MRV (Minimum Remaining Values)
   - Arc consistency

## 🔍 API Endpoints

### N-Queens
```http
POST /api/NQueens/solve
GET  /api/NQueens/heuristics
```

### SAT
```http
POST /api/SAT/solve
POST /api/SAT/generate-formula
```

### Killer Sudoku
```http
POST /api/KillerSudoku/solve
GET  /api/KillerSudoku/sample
```

## 💡 Tips

1. **First Time Setup**: The first run might take longer as dependencies are restored
2. **Hot Reload**: Both frontend and backend support hot reload - changes apply automatically
3. **Swagger UI**: Test APIs directly at http://localhost:5220/swagger
4. **Performance**: All algorithms execute in < 1 second for default inputs
5. **Explore**: Try different board sizes, constraints, and puzzles!

## 🐛 Troubleshooting

### API won't start
```bash
# Make sure .NET 8 is installed
dotnet --version

# Clean and rebuild
cd API/API
dotnet clean
dotnet build
```

### Angular won't compile
```bash
# Clear node modules and reinstall
cd client
rm -rf node_modules package-lock.json
npm install
```

### Port already in use
- API uses port **5220** - change in `launchSettings.json`
- Angular uses port **4200** - change with `ng serve --port 4300`

## 📚 Project Structure

```
Detyra2/
├── API/
│   └── API/
│       ├── Controllers/       # API endpoints
│       ├── Services/          # Business logic
│       ├── Models/            # Data models
│       └── Program.cs         # Configuration
└── client/
    └── src/app/
        ├── components/        # UI components
        ├── services/          # HTTP services
        └── models/            # TypeScript models
```

## 🎯 Assignment Requirements ✅

✅ A* algorithm with multiple heuristics  
✅ Admissible heuristic implemented  
✅ SAT problem with boolean formula  
✅ CSP with constraint programming  
✅ Beautiful interactive UI  
✅ Real-time visualizations  
✅ Complete documentation  

## 🏆 Credits

**Course**: Artificial Intelligence - Master's Program  
**Assignment**: Detyra 2 - 20/12/2025  
**Group**: IA-Grupi13  
**Topics**: Uninformed Search, CSP, SAT Problems

---

**Enjoy exploring AI algorithms! 🎉**

