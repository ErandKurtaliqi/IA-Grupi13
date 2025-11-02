# ⚡ START HERE - Get Running in 2 Minutes!

## 🎯 Goal
Get both backend and frontend running and see your beautiful AI application!

---

## 📝 Prerequisites Check

Before starting, make sure you have:
- [ ] .NET 8.0 SDK installed → `dotnet --version`
- [ ] Node.js installed → `node --version`
- [ ] npm installed → `npm --version`

If any are missing, install them first!

---

## 🚀 Step-by-Step Launch

### Step 1: Open TWO Terminal Windows

You'll need 2 terminals running simultaneously.

**Windows:**
- Open PowerShell or CMD
- Press `Ctrl + Shift + 2` to split (in VS Code)
- Or just open 2 separate windows

**Mac/Linux:**
- Open Terminal
- Press `Cmd + T` for new tab
- Or use split panes

---

### Step 2: Start Backend API

**In Terminal 1:**

```bash
# Navigate to API folder
cd Detyra3/Detyra3

# Run the API
dotnet run
```

**Wait for this message:**
```
Now listening on: http://localhost:5157
Application started. Press Ctrl+C to shut down.
```

✅ **Backend is ready!**

**Keep this terminal open!** Don't close it.

---

### Step 3: Start Frontend UI

**In Terminal 2 (NEW terminal):**

```bash
# Navigate to UI folder
cd Assignment-UI/assignment-UI

# Install dependencies (first time only)
npm install

# Start the dev server
npm start
```

**Wait for this message:**
```
** Angular Live Development Server is listening on localhost:4200 **
✔ Compiled successfully.
```

✅ **Frontend is ready!**

**Your browser should automatically open to http://localhost:4200**

If it doesn't, manually open: http://localhost:4200

---

## 🎉 You're Done! Now Test It

### Quick Test Sequence (2 minutes)

1. **You should see the Home Page**
   - Beautiful gradient hero section
   - Three problem cards
   - ✅ If you see this, everything is working!

2. **Click "Social Golfers" Card**
   - Form appears on the left
   - Keep default values (16, 4, 5)
   - Click "Solve"
   - ✅ See schedule appear in 1-5 seconds

3. **Click "Sudoku Solver" in Navbar**
   - Select "Easy"
   - Click "Solve Puzzle"
   - ✅ See original and solved grids

4. **Click "Latin Square" in Navbar**
   - Keep n=5
   - Click "Generate Square"
   - ✅ See colorful 5×5 grid

---

## ✅ Success Checklist

If you can do all of the above:
- ✅ Backend is working
- ✅ Frontend is working
- ✅ API connection is working
- ✅ All algorithms are working
- ✅ UI is rendering correctly

**🎉 YOUR PROJECT IS FULLY FUNCTIONAL! 🎉**

---

## 🐛 Troubleshooting

### Problem: Port 5157 already in use

**Solution:**
```bash
# Find what's using the port
netstat -ano | findstr :5157

# Kill the process (replace PID with actual number)
taskkill /PID <PID> /F

# Or just change the port in launchSettings.json
```

### Problem: Port 4200 already in use

**Solution:**
```bash
# Use a different port
ng serve --port 4201

# Then update api.service.ts if needed
```

### Problem: npm install fails

**Solution:**
```bash
# Clear npm cache
npm cache clean --force

# Delete node_modules
rm -rf node_modules package-lock.json

# Try again
npm install
```

### Problem: dotnet run fails

**Solution:**
```bash
# Clean and restore
dotnet clean
dotnet restore
dotnet build
dotnet run
```

### Problem: CORS error in browser

**Solution:**
- Make sure API is running first
- Check that apiUrl in api.service.ts is `http://localhost:5157/api`
- Verify CORS is configured in Program.cs (it should be!)

### Problem: Browser doesn't auto-open

**Solution:**
- Manually open http://localhost:4200
- Should work fine!

---

## 🎬 What You Should See

### Terminal 1 (Backend)
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5157
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### Terminal 2 (Frontend)
```
✔ Browser application bundle generation complete.

Initial Chunk Files   | Names         |  Raw Size
main.js              | main          | 250.42 kB |
styles.css           | styles        |  15.23 kB |

✔ Compiled successfully.
```

### Browser
```
🎨 Beautiful gradient navbar
📊 "AI Search Algorithms" title
🃏 Three colorful problem cards
✨ Smooth animations
```

---

## 📱 Verify Everything Works

Open these URLs to double-check:

1. **Frontend**: http://localhost:4200
   - ✅ Should show home page

2. **Backend API**: http://localhost:5157/swagger
   - ✅ Should show Swagger UI with 3 controllers

3. **Test API Directly**: http://localhost:5157/api/latinsquare?n=3&algo=iddfs
   - ✅ Should return JSON with 3×3 matrix

---

## 🎯 Quick Demo Sequence

Once everything is running:

1. **Home Page** (you're here)
   - Look at the design
   - See the three problems

2. **Latin Square** (fastest to demo)
   - Click Latin Square card
   - Keep n=5
   - Click Generate
   - See colorful grid appear

3. **Sudoku** (most recognizable)
   - Click Sudoku in navbar
   - Keep Easy selected
   - Click Solve
   - See puzzle and solution

4. **Social Golfers** (most complex)
   - Click Social Golfers in navbar
   - Keep defaults
   - Click Solve
   - See schedule with weeks and groups

**Total time: 2 minutes**

---

## 🎓 For Presentation

When showing to your instructor:

1. **Start with explanation** (while things load)
   - "This is a full-stack application"
   - "ASP.NET Core backend, Angular frontend"
   - "Three AI problems with uninformed search"

2. **Show the home page**
   - "Clean, modern interface"
   - "Three problems to solve"

3. **Demo one problem fully**
   - Show input parameters
   - Click solve
   - Explain the output
   - Point out performance metrics

4. **Quick show the others**
   - Just click and show they work

5. **Mention the code**
   - "All algorithms implemented from scratch"
   - "Well-documented and tested"

---

## 💾 Stop the Servers

When you're done:

**In Terminal 1 (Backend):**
```
Press Ctrl + C
```

**In Terminal 2 (Frontend):**
```
Press Ctrl + C
```

Both will shut down cleanly.

---

## 🔄 Restart Anytime

Just run the same commands again:

```bash
# Terminal 1
cd Detyra3/Detyra3 && dotnet run

# Terminal 2  
cd Assignment-UI/assignment-UI && npm start
```

---

## 📚 Next Steps

Once running:

1. ✅ **Read**: TESTING_GUIDE.md - Test all features
2. ✅ **Read**: UI_SCREENS.md - Understand each page
3. ✅ **Read**: README.md - Complete documentation
4. ✅ **Play**: Try different values and parameters
5. ✅ **Present**: Show off your amazing work!

---

## 🎊 Final Checklist

Before your presentation:

- [ ] Backend starts successfully
- [ ] Frontend starts successfully
- [ ] Home page loads and looks good
- [ ] Can navigate between pages
- [ ] Social Golfers solves (try 8 players for speed)
- [ ] Sudoku solves (Easy is fastest)
- [ ] Latin Square generates (n=5 is fastest)
- [ ] You understand what each algorithm does
- [ ] You can explain the problem modeling

---

## 🌟 You're All Set!

Everything is working. Your project is:

✅ **Functional** - All algorithms work  
✅ **Beautiful** - Modern, professional UI  
✅ **Complete** - Fully documented  
✅ **Tested** - No errors  
✅ **Ready** - For submission/presentation  

**GO MAKE THOSE TERMINALS RUN! 🚀**

---

**Need help? Check:**
- QUICKSTART.md - Detailed setup
- TESTING_GUIDE.md - Full test procedures  
- README.md - Complete documentation

**Just want to see it work?**

```bash
# One terminal
cd Detyra3/Detyra3 && dotnet run

# Another terminal
cd Assignment-UI/assignment-UI && npm install && npm start
```

**That's it! Open http://localhost:4200 and enjoy! 🎉**

