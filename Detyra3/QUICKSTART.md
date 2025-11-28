# Quick Start Guide

## 🚀 Running the Application

### Prerequisites
- .NET 8.0 SDK installed
- Node.js and npm installed
- Both servers will run on:
  - Backend API: `https://localhost:7140`
  - Frontend: `http://localhost:4200`

### Step 1: Start the Backend API

Open a terminal and run:

```powershell
cd "C:\Users\lenovo\Desktop\IA Ushtrime\IA-Grupi13\Detyra3\API\API"
dotnet run
```

The API should start and display:
```
Now listening on: https://localhost:7140
```

You can test it by opening: `https://localhost:7140/swagger`

### Step 2: Start the Angular Frontend

Open another terminal and run:

```powershell
cd "C:\Users\lenovo\Desktop\IA Ushtrime\IA-Grupi13\Detyra3\client"
npm start
```

The Angular dev server will start on: `http://localhost:4200`

### Step 3: Open the Application

Open your browser and navigate to: `http://localhost:4200`

## 🎮 Using the Application

### Default Analysis
When you load the page, it automatically analyzes the default endgame position:
- **FEN**: `2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1`
- **White to move**
- **Depth**: 4

### Testing Both Scenarios

#### 1. White to Move
- Default state when page loads
- Click **"Analyze Position"** to rerun analysis
- Shows best move for White

#### 2. Black to Move
- Click **"Switch Turn"** button
- FEN automatically changes from `w` to `b`
- Shows best move for Black

### Features to Test

1. **Change Search Depth**
   - Select depth from dropdown (2-6)
   - Higher depth = deeper analysis = longer computation time
   - Recommended: 4 for balance

2. **View Analysis Results**
   - **Best Move**: Highlighted move notation
   - **Evaluation**: Position score (positive = White better, negative = Black better)
   - **Principal Variation**: Sequence of best moves
   - **Top Moves**: Alternative moves with evaluations
   - **Statistics**: Nodes explored, pruning efficiency, computation time

3. **Custom Positions**
   - Enter any valid FEN in the input field
   - Click "Analyze Position" to evaluate

## 📊 Expected Results for Default Position

### White to Move
- **Best move**: Should find the strongest move with the Queen or Knight
- **Evaluation**: Likely positive (White has material advantage)
- **Nodes explored**: ~10,000-20,000 at depth 4
- **Pruning efficiency**: ~60-80%

### Black to Move
- **Best move**: Should find defensive moves with Queen or Knight
- **Evaluation**: Likely negative (Black defending)
- **Nodes explored**: Similar to White

## 🔧 Troubleshooting

### Backend Issues
- Ensure port 7140 is not in use
- Check HTTPS certificate trust
- View logs in the terminal

### Frontend Issues
- Clear browser cache
- Check browser console for errors
- Ensure backend is running first

### CORS Errors
- Backend has CORS enabled for all origins
- Should work without issues

## 📝 Testing Checklist

- [ ] Backend starts successfully on port 7140
- [ ] Frontend starts successfully on port 4200
- [ ] Page loads with default position
- [ ] "Analyze Position" button works
- [ ] "Switch Turn" changes from White to Black
- [ ] Results display correctly:
  - [ ] Best move shown
  - [ ] Evaluation bar updates
  - [ ] Principal variation displayed
  - [ ] Top moves listed
  - [ ] Statistics shown
- [ ] Different depth levels work
- [ ] Custom FEN positions work

## 🎯 Demonstration Points

For your assignment presentation, highlight:

1. **Algorithm Implementation**
   - MiniMax with Alpha-Beta pruning
   - Efficient pruning (show statistics)
   - Configurable depth

2. **Both Scenarios**
   - White to move analysis
   - Black to move analysis
   - Different best moves shown

3. **Performance**
   - Nodes explored vs nodes pruned
   - Pruning efficiency percentage
   - Fast computation times

4. **User Interface**
   - Beautiful, modern design
   - Interactive chess board
   - Real-time analysis
   - Clear result presentation

## 📚 API Testing (Optional)

You can also test the API directly using curl or Postman:

```powershell
# Test default position
curl -X POST "https://localhost:7140/api/chess/analyze" `
  -H "Content-Type: application/json" `
  -d '{"fen":"2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1","depth":4}'
```

## ✅ Success Indicators

The application is working correctly when you see:
- ✅ Chess board displays with pieces
- ✅ Turn indicator shows correct player
- ✅ Analysis completes in < 2 seconds
- ✅ Best move is displayed
- ✅ Statistics show pruning efficiency
- ✅ Switching turns works instantly
- ✅ No errors in browser console

## 🎓 Assignment Requirements Met

- ✅ MiniMax algorithm implemented
- ✅ Alpha-Beta pruning applied
- ✅ Default endgame position used
- ✅ "White to move" scenario works
- ✅ "Black to move" scenario works
- ✅ Results displayed clearly
- ✅ Performance metrics shown

Enjoy your chess AI! 🎉

