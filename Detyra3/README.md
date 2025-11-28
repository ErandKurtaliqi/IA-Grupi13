# Chess MiniMax Algorithm - Detyra 3

## Project Overview
This project implements the MiniMax algorithm with Alpha-Beta pruning for chess endgame analysis. It consists of an ASP.NET Core backend API and an Angular frontend.

### Assignment Requirements
- **Course**: Artificial Intelligence (IA)
- **Assignment**: Detyra 3 - Game Theory - MiniMax Algorithm
- **Date**: 20/12/2025
- **Group**: Grupi 13

### Features
1. **MiniMax Algorithm with Alpha-Beta Pruning**
   - Efficient tree search for chess positions
   - Alpha-Beta pruning for performance optimization
   - Configurable search depth (2-6 ply)

2. **Endgame Analysis**
   - Default position: `2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1`
   - Support for both "White to move" and "Black to move"
   - Real-time position evaluation

3. **Beautiful UI**
   - Interactive chess board visualization
   - Animated components
   - Real-time analysis results
   - Statistics display (nodes explored, pruning efficiency, etc.)

## Technology Stack

### Backend (ASP.NET Core 8.0)
- C# for algorithm implementation
- RESTful API design
- CORS enabled for frontend communication

### Frontend (Angular 19)
- TypeScript
- Standalone components
- Reactive forms
- Modern CSS with animations

## Project Structure

```
Detyra3/
├── API/
│   └── API/
│       ├── Controllers/
│       │   └── ChessController.cs       # API endpoints
│       ├── Services/
│       │   ├── ChessService.cs          # Chess logic
│       │   └── MiniMaxService.cs        # MiniMax algorithm
│       ├── Models/
│       │   ├── ChessBoard.cs            # Board representation
│       │   ├── Move.cs                  # Move representation
│       │   └── DTOs/                    # Data transfer objects
│       └── Program.cs                   # API configuration
└── client/
    └── src/
        └── app/
            ├── services/
            │   └── chess.service.ts     # API communication
            ├── models/
            │   └── chess.models.ts      # TypeScript models
            ├── app.component.ts         # Main component
            ├── app.component.html       # UI template
            └── app.component.css        # Styling
```

## API Endpoints

### 1. Analyze Position
```http
POST https://localhost:7140/api/chess/analyze
Content-Type: application/json

{
  "fen": "2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1",
  "depth": 4
}
```

**Response:**
```json
{
  "currentPosition": "2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1",
  "isWhiteTurn": true,
  "evaluation": 350,
  "bestMove": "Qf4 → e5",
  "bestMoveNotation": "Qe4 → e5",
  "principalVariation": ["Qe4 → e5", "Qd6 → e7", ...],
  "nodesExplored": 12485,
  "nodesPruned": 8234,
  "computationTimeMs": 145.23,
  "topMoves": [...],
  "resultMessage": "White to move. Best move: Qe4 → e5. Position: White is better"
}
```

### 2. Get Default Position
```http
GET https://localhost:7140/api/chess/default-position
```

### 3. Validate FEN
```http
POST https://localhost:7140/api/chess/validate-fen
Content-Type: application/json

"2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1"
```

## Running the Application

### Backend
1. Navigate to the API directory:
   ```bash
   cd API/API
   ```

2. Build and run:
   ```bash
   dotnet build
   dotnet run
   ```

3. API will be available at: `https://localhost:7140`
4. Swagger documentation: `https://localhost:7140/swagger`

### Frontend
1. Navigate to the client directory:
   ```bash
   cd client
   ```

2. Install dependencies (first time only):
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

4. Open browser at: `http://localhost:4200`

## Algorithm Details

### MiniMax with Alpha-Beta Pruning

The implementation includes:

1. **Board Evaluation**
   - Material counting (piece values)
   - Positional bonuses (center control, king safety)
   - Endgame-specific evaluation

2. **Move Generation**
   - Legal move generation for all pieces
   - King, Queen, Rook, Bishop, Knight, Pawn movements
   - Capture detection

3. **Alpha-Beta Optimization**
   - Prunes branches that cannot influence the final decision
   - Significant performance improvement over pure MiniMax
   - Typically prunes 60-80% of nodes

4. **Search Features**
   - Configurable depth (2-6 ply recommended)
   - Principal variation tracking
   - Move ordering for better pruning

## Usage

1. **Default Analysis**: Load the page to analyze the default endgame position
2. **Switch Turn**: Click "Switch Turn" to analyze from Black's perspective
3. **Adjust Depth**: Change search depth for deeper/faster analysis
4. **Custom Position**: Enter FEN notation for any position

## Performance Metrics

The application displays:
- **Nodes Explored**: Total positions evaluated
- **Nodes Pruned**: Positions skipped due to Alpha-Beta pruning
- **Pruning Efficiency**: Percentage of nodes pruned
- **Computation Time**: Analysis duration in milliseconds

## Chess Piece Values

- Pawn (P/p): 100
- Knight (N/n): 320
- Bishop (B/b): 330
- Rook (R/r): 500
- Queen (Q/q): 900
- King (K/k): 20000

## Default Endgame Position

```
8  . . n . k . . .
7  . . . . . . . .
6  . . . q . . . .
5  . . . . . . . .
4  . . N . . Q . .
3  . . . . . . . .
2  . . . . . . . .
1  . . . . K . . .
   a b c d e f g h
```

FEN: `2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1`

## Authors
- **Group**: Grupi 13
- **Course**: Artificial Intelligence
- **Date**: November 28, 2025

## License
Educational project for IA course.

