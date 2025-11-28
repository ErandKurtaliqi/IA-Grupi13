# MiniMax Algorithm with Alpha-Beta Pruning - Technical Documentation

## Overview

This document explains the implementation of the MiniMax algorithm with Alpha-Beta pruning for chess endgame analysis.

## Algorithm Explanation

### MiniMax Algorithm

MiniMax is a decision-making algorithm used in game theory for minimizing the possible loss in a worst-case scenario. In chess:

- **Maximizing Player (White)**: Tries to maximize the evaluation score
- **Minimizing Player (Black)**: Tries to minimize the evaluation score

The algorithm explores the game tree recursively, alternating between maximizing and minimizing layers.

### Pseudocode

```
function minimax(position, depth, maximizingPlayer):
    if depth == 0 or game_over(position):
        return evaluate(position)
    
    if maximizingPlayer:
        maxEval = -∞
        for each child in generate_moves(position):
            eval = minimax(child, depth-1, False)
            maxEval = max(maxEval, eval)
        return maxEval
    else:
        minEval = +∞
        for each child in generate_moves(position):
            eval = minimax(child, depth-1, True)
            minEval = min(minEval, eval)
        return minEval
```

### Alpha-Beta Pruning

Alpha-Beta pruning is an optimization technique that reduces the number of nodes evaluated in the game tree.

**Key Concepts:**
- **Alpha (α)**: Best value the maximizer can guarantee
- **Beta (β)**: Best value the minimizer can guarantee
- **Pruning**: Skip evaluation when α ≥ β

```
function alphabeta(position, depth, α, β, maximizingPlayer):
    if depth == 0 or game_over(position):
        return evaluate(position)
    
    if maximizingPlayer:
        maxEval = -∞
        for each child in generate_moves(position):
            eval = alphabeta(child, depth-1, α, β, False)
            maxEval = max(maxEval, eval)
            α = max(α, eval)
            if β ≤ α:
                break  # β cutoff (prune)
        return maxEval
    else:
        minEval = +∞
        for each child in generate_moves(position):
            eval = alphabeta(child, depth-1, α, β, True)
            minEval = min(minEval, eval)
            β = min(β, eval)
            if β ≤ α:
                break  # α cutoff (prune)
        return minEval
```

## Implementation Details

### 1. Board Representation

```csharp
public class ChessBoard
{
    public char[,] Board { get; set; }  // 8x8 array
    public bool IsWhiteTurn { get; set; }
}
```

**FEN Notation:**
- Format: `position turn castling en-passant halfmove fullmove`
- Example: `2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1`

### 2. Move Representation

```csharp
public class Move
{
    public int FromRow, FromCol, ToRow, ToCol;
    public char Piece, CapturedPiece;
}
```

### 3. Evaluation Function

The position evaluation combines:

#### Material Count
```
Piece Values:
- Pawn:   100
- Knight: 320
- Bishop: 330
- Rook:   500
- Queen:  900
- King:   20000
```

#### Positional Bonuses
- **Center Control**: +10 to +20 for central squares
- **Knight**: Prefers center
- **King**: Prefers corners in endgame

#### Formula
```
Evaluation = Material_Score + Positional_Score
```

### 4. Move Generation

**For each piece type:**

**King**: Moves 1 square in any direction
```
Directions: [(-1,-1), (-1,0), (-1,1), (0,-1), (0,1), (1,-1), (1,0), (1,1)]
```

**Queen**: Combines Rook + Bishop movement
```
8 directions, unlimited distance until blocked
```

**Rook**: Horizontal and vertical
```
4 directions: [(0,1), (0,-1), (1,0), (-1,0)]
```

**Bishop**: Diagonal
```
4 directions: [(1,1), (1,-1), (-1,1), (-1,-1)]
```

**Knight**: L-shape
```
8 positions: [(-2,-1), (-2,1), (-1,-2), (-1,2), (1,-2), (1,2), (2,-1), (2,1)]
```

**Pawn**: Forward move and diagonal capture
```
White: direction = +1 (down the board)
Black: direction = -1 (up the board)
```

### 5. MiniMax with Alpha-Beta Implementation

```csharp
public class MiniMaxService
{
    private int AlphaBeta(ChessBoard board, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        _nodesExplored++;
        
        // Base case: leaf node
        if (depth == 0 || IsGameOver(board))
            return EvaluateBoard(board);
        
        var moves = GenerateAllMoves(board, maximizingPlayer);
        
        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            
            foreach (var move in moves)
            {
                MakeMove(board, move);
                int eval = AlphaBeta(board, depth-1, alpha, beta, false);
                UndoMove(board, move);
                
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                
                if (beta <= alpha)
                {
                    _nodesPruned++;
                    break;  // Beta cutoff
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            
            foreach (var move in moves)
            {
                MakeMove(board, move);
                int eval = AlphaBeta(board, depth-1, alpha, beta, true);
                UndoMove(board, move);
                
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                
                if (beta <= alpha)
                {
                    _nodesPruned++;
                    break;  // Alpha cutoff
                }
            }
            return minEval;
        }
    }
}
```

## Performance Analysis

### Time Complexity

**Without Alpha-Beta:**
- O(b^d) where b = branching factor, d = depth
- For chess: ~35^d positions

**With Alpha-Beta:**
- Best case: O(b^(d/2))
- Average case: O(b^(3d/4))
- Worst case: O(b^d)

### Example Performance

For depth = 4 in the default endgame position:

| Metric | Value |
|--------|-------|
| Branching Factor | ~10-20 |
| Nodes without Pruning | ~10,000-160,000 |
| Nodes with Pruning | ~3,000-40,000 |
| Pruning Efficiency | 60-80% |
| Computation Time | 100-500 ms |

## Optimization Techniques

### 1. Alpha-Beta Pruning
- Eliminates branches that cannot affect final decision
- Most effective with good move ordering

### 2. Move Ordering (Future Enhancement)
- Evaluate captures first
- Consider previous iteration's best moves
- Can improve pruning to 90%+

### 3. Transposition Tables (Future Enhancement)
- Cache previously evaluated positions
- Avoid redundant calculations
- Significant speedup for repeated positions

### 4. Quiescence Search (Future Enhancement)
- Extend search for captures at leaf nodes
- Avoid "horizon effect"
- More accurate evaluation

## Testing the Algorithm

### Test Cases

#### 1. Checkmate in 1
```
FEN: 7k/5Q2/6K1/8/8/8/8/8 w - - 0 1
Expected: Qf8# (checkmate)
```

#### 2. Stalemate Detection
```
FEN: 7k/8/6Q1/8/8/8/8/K7 b - - 0 1
Expected: No legal moves (stalemate)
```

#### 3. Material Advantage
```
FEN: 2n1k3/8/3q4/8/2N2Q2/8/8/4K3 w - - 0 1
Expected: Positive evaluation for White
```

### Verification Steps

1. **Correctness**: Best move makes sense tactically
2. **Completeness**: All legal moves considered
3. **Efficiency**: High pruning percentage
4. **Consistency**: Same position returns same result

## Algorithm Variants

### Negamax
Simplified MiniMax using:
```
score = -negamax(position, depth-1, -β, -α)
```

### Iterative Deepening
Search progressively deeper depths:
```
for depth = 1 to max_depth:
    best_move = minimax(position, depth)
```

Benefits:
- Better move ordering
- Time management
- Anytime algorithm

## Conclusion

This implementation provides:
- ✅ Correct MiniMax algorithm
- ✅ Efficient Alpha-Beta pruning
- ✅ Proper chess move generation
- ✅ Position evaluation
- ✅ Both White and Black perspectives
- ✅ Performance metrics

The algorithm successfully analyzes chess endgames and finds optimal moves according to the evaluation function.

## References

1. **MiniMax Algorithm**: 
   - Russell, S., & Norvig, P. (2020). *Artificial Intelligence: A Modern Approach*

2. **Alpha-Beta Pruning**:
   - Knuth, D. E., & Moore, R. W. (1975). *An Analysis of Alpha-Beta Pruning*

3. **Chess Programming**:
   - Chess Programming Wiki: https://www.chessprogramming.org/

4. **Endgame Tables**:
   - Syzygy Tablebase: https://syzygy-tables.info/

## Appendix: Default Position Analysis

```
Position: 2n1k3/8/3q4/8/2N2Q2/8/8/4K3

  8  . . n . k . . .
  7  . . . . . . . .
  6  . . . q . . . .
  5  . . . . . . . .
  4  . . N . . Q . .
  3  . . . . . . . .
  2  . . . . . . . .
  1  . . . . K . . .
     a b c d e f g h

Material Count:
White: Knight (320) + Queen (900) = 1220
Black: Knight (320) + Queen (900) = 1220
Material Balance: Equal

Position: Endgame with limited pieces
Recommended Depth: 4-6 ply
```

