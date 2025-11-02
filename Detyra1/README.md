# AI Search Algorithms - Uninformed Search Techniques

A comprehensive implementation of uninformed search algorithms solving three classic AI problems:

1. **Social Golfers Problem** - DFS with Backtracking & Depth Limited Search
2. **Sudoku Solver** - BFS with Backtracking
3. **Latin Square Generator** - IDDFS (Iterative Deepening Depth-First Search)

## 📁 Project Structure

```
.
├── Detyra3/              # ASP.NET Core Web API Backend
│   └── Detyra3/
│       ├── Controllers/  # API Controllers
│       ├── Services/     # Algorithm implementations
│       └── Interfaces/   # Service interfaces
│
└── Assignment-UI/        # Angular Frontend
    └── assignment-UI/
        └── src/
            ├── app/
            │   ├── components/  # UI Components
            │   └── services/    # API Service
            └── styles.css
```

## 🚀 Getting Started

### Prerequisites

- **.NET 8.0 SDK** or later
- **Node.js** (v18+) and **npm**
- **Angular CLI** (`npm install -g @angular/cli`)

### Running the Backend (ASP.NET Core API)

1. Navigate to the API directory:
   ```bash
   cd Detyra3/Detyra3
   ```

2. Restore dependencies and run:
   ```bash
   dotnet restore
   dotnet run
   ```

3. The API will start at: `http://localhost:5157`
4. Swagger documentation available at: `http://localhost:5157/swagger`

### Running the Frontend (Angular)

1. Navigate to the UI directory:
   ```bash
   cd Assignment-UI/assignment-UI
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

4. Open your browser at: `http://localhost:4200`

## 🧩 Problems & Algorithms

### 1. Social Golfers Problem

**Problem**: Schedule n golfers into groups of size g for w weeks such that no two golfers play together more than once.

**Algorithms**:
- **DFS with Backtracking**: Explores the search space depth-first, backtracking when constraints are violated
- **Depth Limited Search**: Limits the depth of exploration to prevent infinite recursion
- **Find Maximum Weeks**: Iteratively finds the maximum number of weeks possible for a given configuration

**Example**: 32 golfers, 4 per group → How many weeks can they play?

### 2. Sudoku Solver

**Problem**: Fill a 9×9 grid so that each row, column, and 3×3 box contains digits 1-9 exactly once.

**Algorithm**:
- **BFS with Backtracking**: Explores all possible states level by level, finding the solution systematically

**Features**:
- Preset puzzles (Easy, Medium, Hard)
- Custom puzzle input
- Visual grid display showing original and solved states

### 3. Latin Square Generator

**Problem**: Create an n×n grid filled with n different numbers, each appearing exactly once in every row and column.

**Algorithm**:
- **IDDFS (Iterative Deepening DFS)**: Combines benefits of BFS (optimal solution) with space efficiency of DFS
- **Backtracking**: Alternative implementation for comparison

**Features**:
- Configurable grid size (1 to 12)
- Color-coded visualization
- Validation checks display

## 🎨 UI Features

- **Modern, Responsive Design**: Works on desktop, tablet, and mobile
- **Real-time Algorithm Execution**: See results instantly
- **Performance Metrics**: View execution time and nodes explored
- **Interactive Controls**: Easy-to-use forms for all configurations
- **Visual Feedback**: Loading states, error handling, and success indicators

## 📊 API Endpoints

### Social Golfers
- `GET /api/socialgolfers/dfs?numPlayers=16&groupSize=4&numWeeks=5`
- `GET /api/socialgolfers/depth-limited?numPlayers=16&groupSize=4&numWeeks=5&depthLimit=6`
- `GET /api/socialgolfers/find-max-weeks?numPlayers=16&groupSize=4&maxWeeksToTry=15`

### Sudoku
- `GET /api/sudoku/solve?difficulty=Easy`
- `POST /api/sudoku/solve-custom` (Body: 9×9 grid array)

### Latin Square
- `GET /api/latinsquare?n=5&algo=iddfs`
- `GET /api/latinsquare?n=5&algo=backtracking`

## 🔧 Technical Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12
- **API Documentation**: Swagger/OpenAPI

### Frontend
- **Framework**: Angular 20
- **Language**: TypeScript 5.9
- **Styling**: Modern CSS with gradients and animations
- **HTTP Client**: Angular HttpClient with RxJS

## 📝 Assignment Details

**Course**: Artificial Intelligence  
**Topic**: Uninformed Search Techniques  
**Submission Date**: November 7-8, 2025  
**Group**: Grupi 13

### Problem Models

Each problem includes:
- **Variable Representation**: What we're solving for
- **Domain Definition**: Possible values/states
- **Constraint Definition**: Rules that must be satisfied
- **Goal State**: What constitutes a solution

### Search Strategies Implemented

1. **DFS (Depth-First Search)**: Goes deep into the search tree before backtracking
2. **BFS (Breadth-First Search)**: Explores level by level
3. **DLS (Depth Limited Search)**: DFS with a depth constraint
4. **IDDFS**: Repeatedly applies DLS with increasing depth limits
5. **Backtracking**: Systematic way to iterate through all possible configurations

## 🎯 Usage Examples

### Social Golfers
Try finding the maximum weeks for different configurations:
- 8 players, 4 per group
- 16 players, 4 per group  
- 32 players, 4 per group (challenging!)

### Sudoku
- Start with Easy puzzles to see instant solutions
- Try Medium and Hard for more complex solving
- Input your own puzzle to solve

### Latin Square
- Generate small squares (3×3, 4×4) for quick results
- Try larger squares (8×8, 10×10) to see performance
- Compare IDDFS vs Backtracking execution times

## 🐛 Troubleshooting

### CORS Issues
If you get CORS errors, ensure:
- The API is running on `http://localhost:5157`
- The Angular app is running on `http://localhost:4200`
- CORS policy is configured in `Program.cs`

### API Connection
If the UI can't connect to the API:
1. Check the API is running (visit `http://localhost:5157/swagger`)
2. Verify the `apiUrl` in `api.service.ts` matches your API port
3. Check browser console for error messages

### Performance
For large problems (e.g., 32 golfers), expect:
- Longer computation times
- Higher memory usage
- Consider using smaller test cases first

## 📚 Learning Outcomes

This project demonstrates:
- Implementation of classic uninformed search algorithms
- RESTful API design and development
- Modern frontend development with Angular
- Integration of backend and frontend systems
- Problem modeling and constraint satisfaction
- Algorithm performance analysis

## 🤝 Contributing

This is an academic project. For improvements or bug fixes, please maintain the core algorithm implementations as per the assignment requirements.

## 📄 License

Academic project for University coursework.

---

**Made with ❤️ for AI Course - Grupi 13**

