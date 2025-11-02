# Quick Start Guide

## 🚀 Running the Application

### Step 1: Start the Backend API

Open a terminal and navigate to the API directory:

```bash
cd Detyra3/Detyra3
dotnet run
```

The API will start at: **http://localhost:5157**

You should see:
```
Now listening on: http://localhost:5157
Application started. Press Ctrl+C to shut down.
```

### Step 2: Start the Frontend

Open a **new terminal** and navigate to the UI directory:

```bash
cd Assignment-UI/assignment-UI
npm install
npm start
```

The app will open automatically at: **http://localhost:4200**

## 📱 Using the Application

### Home Page
- Click on any problem card to navigate to that solver
- Read about the algorithms and features

### Social Golfers Problem
1. Choose an algorithm (DFS, Depth Limited, or Find Max Weeks)
2. Set the number of players (must be divisible by group size)
3. Set the group size (e.g., 4)
4. Set the number of weeks
5. Click "Solve"

**Recommended Starting Values:**
- 16 players, 4 per group, 5 weeks (fast)
- 8 players, 4 per group, 2 weeks (very fast)

### Sudoku Solver
1. Choose preset difficulty (Easy, Medium, Hard) OR
2. Switch to Custom mode and enter your own puzzle
3. Click "Solve Puzzle"
4. View original and solved grids side-by-side

### Latin Square Generator
1. Choose algorithm (IDDFS or Backtracking)
2. Set grid size (n) - try 5 to 10 for best visualization
3. Click "Generate Square"
4. View the color-coded matrix

## ⚠️ Troubleshooting

### API Not Connecting
- Make sure the API is running (check terminal for errors)
- Visit http://localhost:5157/swagger to test API directly
- Check that CORS is enabled in Program.cs

### Port Already in Use
If port 5157 or 4200 is in use:

**For API:**
```bash
# Edit launchSettings.json and change ports
# Then update apiUrl in src/app/services/api.service.ts
```

**For Angular:**
```bash
ng serve --port 4201
```

### Slow Performance
For large problems (e.g., 32 golfers):
- Start with smaller values
- Consider using "Find Max Weeks" to see limits
- The algorithm may take several seconds

## 🎯 Testing Recommendations

### Quick Tests (< 1 second)
- Social Golfers: 8 players, 4 per group, 2 weeks
- Sudoku: Easy difficulty
- Latin Square: n = 5

### Medium Tests (1-5 seconds)
- Social Golfers: 16 players, 4 per group, 5 weeks
- Sudoku: Medium difficulty
- Latin Square: n = 8

### Challenging Tests (5+ seconds)
- Social Golfers: 32 players, 4 per group, 5 weeks
- Sudoku: Hard difficulty
- Latin Square: n = 12

## 📊 API Endpoints

Test directly in Swagger: http://localhost:5157/swagger

### Social Golfers
```
GET /api/socialgolfers/dfs?numPlayers=16&groupSize=4&numWeeks=5
GET /api/socialgolfers/depth-limited?numPlayers=16&groupSize=4&numWeeks=5&depthLimit=6
GET /api/socialgolfers/find-max-weeks?numPlayers=16&groupSize=4&maxWeeksToTry=15
```

### Sudoku
```
GET /api/sudoku/solve?difficulty=Easy
POST /api/sudoku/solve-custom
```

### Latin Square
```
GET /api/latinsquare?n=5&algo=iddfs
GET /api/latinsquare?n=5&algo=backtracking
```

## 💡 Tips

1. **Start Small**: Begin with small problem sizes to understand the algorithm behavior
2. **Compare Algorithms**: Try different algorithms on the same problem to see performance differences
3. **Watch Metrics**: Pay attention to "Nodes Explored" and "Time" to understand complexity
4. **Use Find Max Weeks**: For Social Golfers, use this to discover the maximum possible weeks
5. **Custom Sudoku**: Input puzzles from newspapers or online to test the solver

## 🆘 Need Help?

1. Check the console in your browser (F12) for errors
2. Check the terminal where the API is running for backend errors
3. Verify both servers are running
4. Make sure you're using the correct ports

---

**Ready to explore AI search algorithms! 🚀**

