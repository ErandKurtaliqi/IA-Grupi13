# Test Guide - Verify Everything Works

## 🧪 Pre-Flight Checklist

### 1. Backend API Tests

Start the API first:
```bash
cd Detyra3/Detyra3
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5157
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

#### Test Swagger UI
1. Open browser: http://localhost:5157/swagger
2. You should see three controllers:
   - LatinSquare
   - SocialGolfers  
   - Sudoku

#### Quick API Tests via Swagger

**Social Golfers - Simple Test:**
- Expand `GET /api/socialgolfers/dfs`
- Click "Try it out"
- Use defaults: numPlayers=16, groupSize=4, numWeeks=5
- Click "Execute"
- Should get 200 response with solution in < 5 seconds

**Sudoku - Easy Test:**
- Expand `GET /api/sudoku/solve`
- Click "Try it out"
- Select difficulty: Easy
- Click "Execute"
- Should get 200 response with solved grid

**Latin Square - Quick Test:**
- Expand `GET /api/latinsquare`
- Click "Try it out"
- n=5, algo=iddfs
- Click "Execute"
- Should get 200 response with 5×5 matrix

### 2. Frontend Tests

In a NEW terminal:
```bash
cd Assignment-UI/assignment-UI
npm install
npm start
```

**Expected Output:**
```
** Angular Live Development Server is listening on localhost:4200 **
✔ Compiled successfully.
```

Browser should automatically open at http://localhost:4200

---

## 🎯 Manual UI Tests

### Test 1: Home Page Navigation

**Steps:**
1. Verify home page loads
2. Check hero section with gradient background
3. Verify all 3 problem cards are visible
4. Click each card and verify navigation
5. Use browser back button
6. Click logo in navbar to return home

**Expected:** Smooth navigation, no errors

---

### Test 2: Social Golfers - Quick Solve

**Steps:**
1. Navigate to Social Golfers (click card or navbar link)
2. Keep defaults: 16 players, 4 per group, 5 weeks, DFS algorithm
3. Click "Solve"
4. Wait for results

**Expected:**
- Loading spinner appears
- Results show in < 5 seconds
- Success indicator (✓)
- Time in milliseconds displayed
- Nodes explored count shown
- Solution shows 5 weeks
- Each week shows groups with player numbers

---

### Test 3: Social Golfers - Depth Limited

**Steps:**
1. Change algorithm to "Depth Limited Search"
2. Set: 16 players, 4 per group, 5 weeks, depth limit 6
3. Click "Solve"

**Expected:**
- Solution found
- Shows depth limit used
- Performance metrics displayed

---

### Test 4: Social Golfers - Find Max Weeks

**Steps:**
1. Change algorithm to "Find Maximum Weeks"
2. Set: 8 players, 4 per group, max 10 weeks to try
3. Click "Solve"

**Expected:**
- Shows maximum weeks found (likely 1 for this config)
- Displays attempts grid
- Green checks for successful weeks
- Red X for failed weeks

---

### Test 5: Sudoku - Preset Puzzle

**Steps:**
1. Navigate to Sudoku page
2. Select "Easy" difficulty
3. Click "Solve Puzzle"

**Expected:**
- Original puzzle displayed on left
- Solved puzzle displayed on right
- Original numbers have gray background
- Solved numbers are purple
- Time displayed (should be fast)

---

### Test 6: Sudoku - Medium & Hard

**Steps:**
1. Select "Medium", click "Solve Puzzle"
2. Verify solution appears
3. Select "Hard", click "Solve Puzzle"
4. Verify solution appears

**Expected:**
- All puzzles solve successfully
- Time increases with difficulty
- Visual differences between original and solution clear

---

### Test 7: Sudoku - Custom Input

**Steps:**
1. Click "Custom Puzzle" toggle
2. Enter a simple puzzle:
   - Row 1: 5,3,0,0,7,0,0,0,0
   - Row 2: 6,0,0,1,9,5,0,0,0
   - (Leave rest as 0)
3. Click "Solve Puzzle"

**Expected:**
- Grid accepts input
- Filled cells highlighted in purple
- Solve produces valid solution
- Original vs solution comparison visible

---

### Test 8: Sudoku - Clear Grid

**Steps:**
1. In Custom mode, enter some numbers
2. Click "Clear Grid"

**Expected:**
- All cells return to 0 (empty)
- Can enter new puzzle

---

### Test 9: Latin Square - Small Grid

**Steps:**
1. Navigate to Latin Square
2. Keep n=5, select "IDDFS"
3. Click "Generate Square"

**Expected:**
- 5×5 grid displayed
- Each cell has unique color
- Numbers 1-5 visible
- Validation checks show (green section)
- Example pattern displayed

---

### Test 10: Latin Square - Larger Grid

**Steps:**
1. Change n to 8
2. Select "Backtracking"
3. Click "Generate Square"

**Expected:**
- 8×8 grid displayed
- More colors in palette
- All cells filled correctly
- No duplicate numbers in any row/column

---

### Test 11: Latin Square - Algorithm Comparison

**Steps:**
1. Set n=6
2. Generate with "IDDFS", note the time
3. Generate with "Backtracking", note the time

**Expected:**
- Both produce valid Latin Squares
- Times may vary slightly
- Both solutions are correct (not necessarily identical)

---

## 🔍 Browser Console Tests

Open browser console (F12) and check:

### During Page Load
**Should see NO:**
- Red error messages
- 404 errors
- CORS errors
- TypeScript errors

**Should see (optional):**
- Angular dev mode messages
- Router navigation logs

### During API Calls
**Network tab should show:**
- Status 200 for successful calls
- Correct request URLs
- JSON responses
- Reasonable response times

---

## 🚨 Error Scenarios to Test

### Test: Invalid Input - Social Golfers

**Steps:**
1. Enter 10 players, 4 per group
2. Click "Solve"

**Expected:**
- Error message or validation
- No crash

### Test: API Offline

**Steps:**
1. Stop the API (Ctrl+C in API terminal)
2. Try to solve any problem in UI

**Expected:**
- Loading spinner appears
- Error message displays
- App doesn't crash
- Can try again after restarting API

### Test: Invalid Sudoku

**Steps:**
1. In custom mode, enter impossible puzzle (e.g., 1,1,0,0... in same row)
2. Try to solve

**Expected:**
- Either validation error or no solution message
- No crash

---

## ✅ Success Criteria

### All tests pass if:
- ✅ No console errors during normal operation
- ✅ All pages load within 2 seconds
- ✅ All API calls return successfully
- ✅ Results display correctly
- ✅ Navigation works smoothly
- ✅ UI is responsive (try resizing browser)
- ✅ Loading states appear during computation
- ✅ Error messages display clearly
- ✅ Colors and styling match design
- ✅ All buttons are clickable and functional

---

## 📊 Performance Benchmarks

### Expected Solving Times (approximate)

**Social Golfers:**
- 8 players, 2 weeks: < 100ms
- 16 players, 5 weeks: 1-5 seconds
- 32 players, 5 weeks: 10-60 seconds (may timeout)

**Sudoku:**
- Easy: < 500ms
- Medium: 1-3 seconds
- Hard: 3-10 seconds

**Latin Square:**
- n=5: < 100ms
- n=8: < 500ms
- n=10: 1-3 seconds

---

## 🐛 Common Issues & Solutions

### Issue: CORS Error
**Solution:** 
- Ensure API is running
- Check CORS policy in Program.cs
- Verify apiUrl in api.service.ts

### Issue: Port Already in Use
**Solution:**
```bash
# For API
netstat -ano | findstr :5157
taskkill /PID <PID> /F

# For Angular
netstat -ano | findstr :4200
taskkill /PID <PID> /F
```

### Issue: npm install fails
**Solution:**
```bash
# Clear cache
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

### Issue: dotnet run fails
**Solution:**
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

---

## 🎓 Demo Sequence

If presenting to someone:

1. **Start with Home Page** (30 seconds)
   - Show hero section
   - Explain the three problems
   - Point out features

2. **Demo Latin Square** (1 minute)
   - Quickest to show results
   - Visual and easy to understand
   - Try n=5 and n=8

3. **Demo Sudoku** (1 minute)
   - Show Easy preset
   - Switch to custom and enter a few numbers
   - Show solve

4. **Demo Social Golfers** (2 minutes)
   - Start with 8 players, 2 weeks (fast)
   - Show the schedule output
   - Try "Find Max Weeks" with 8 players
   - Show it finds 1 week maximum

5. **Show Performance** (30 seconds)
   - Point out time and nodes explored
   - Try 16 players to show longer computation

**Total: ~5 minutes for complete demo**

---

## ✨ Advanced Testing (Optional)

### Browser Compatibility
- Test in Chrome
- Test in Firefox
- Test in Edge
- Test in Safari (if on Mac)

### Responsive Design
- Desktop view (1920×1080)
- Laptop view (1366×768)
- Tablet view (768×1024)
- Mobile view (375×667)

### Performance Testing
- Open DevTools → Performance tab
- Record while solving a problem
- Check for performance bottlenecks

---

**Happy Testing! 🚀**

If all tests pass, your application is ready for submission! 🎉

