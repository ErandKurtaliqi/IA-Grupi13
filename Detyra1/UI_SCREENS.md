# UI Screens Overview

## 🏠 Home Page

**Route**: `/` (http://localhost:4200)

### Layout:
1. **Hero Section**
   - Gradient background (Purple → Pink → Red)
   - Large title: "AI Search Algorithms"
   - Subtitle: "Uninformed Search Techniques"
   - Descriptive text about the project

2. **Problems Section**
   - Three cards in a responsive grid:
   
   **Card 1: Social Golfers Problem**
   - Icon: ⛳ (purple background)
   - Description: Schedule golfers into groups
   - Algorithms: DFS with Backtracking, Depth Limited Search
   - Link to /social-golfers

   **Card 2: Sudoku Solver**
   - Icon: 🔢 (pink background)
   - Description: Solve 9×9 Sudoku puzzles
   - Algorithm: BFS with Backtracking
   - Link to /sudoku

   **Card 3: Latin Square Generator**
   - Icon: 📐 (red background)
   - Description: Generate n×n grids
   - Algorithms: IDDFS, Backtracking
   - Link to /latin-square

3. **Features Section**
   - 4 feature cards in a grid:
     - ⚡ Real-time Solving
     - 📊 Performance Metrics
     - 🎨 Visual Results
     - ⚙️ Configurable

4. **Algorithms Section**
   - 5 algorithm badges with full names:
     - DFS - Depth-First Search
     - BFS - Breadth-First Search
     - DLS - Depth Limited Search
     - IDDFS - Iterative Deepening DFS
     - Backtracking - Systematic Exploration

---

## ⛳ Social Golfers Page

**Route**: `/social-golfers`

### Layout:
**Left Panel - Configuration Card**
- Algorithm dropdown (DFS / Depth Limited / Find Max Weeks)
- Number of Players input (must be divisible by group size)
- Group Size input
- Number of Weeks input (hidden for Find Max Weeks)
- Depth Limit input (only for Depth Limited)
- Max Weeks to Try input (only for Find Max Weeks)
- "Solve" button (gradient purple/pink)

**Right Panel - Results Card**
- Algorithm used
- Success/Failure indicator (✓/✗)
- Time in milliseconds
- Nodes explored
- Solution display:
  - Week by Week breakdown
  - Groups with player numbers
  - Colored boxes for each week

**Find Max Weeks Mode**
- Shows maximum weeks found
- Grid of attempts (Week 1: ✓, Week 2: ✓, Week 3: ✗, etc.)
- Color coded: green for success, red for failure

---

## 🔢 Sudoku Solver Page

**Route**: `/sudoku`

### Layout:
**Mode Toggle** (above main content)
- "Preset Puzzles" button
- "Custom Puzzle" button
- One highlighted at a time

**Left Panel - Configuration Card**

**Preset Mode:**
- Three difficulty buttons (Easy / Medium / Hard)
- "Solve Puzzle" button

**Custom Mode:**
- 9×9 grid input
- Each cell is an input field (accepts 0-9)
- Filled cells have purple highlight
- "Clear Grid" button
- "Solve Puzzle" button

**Right Panel - Results Card**
- Difficulty level
- Solving time
- Two grids side by side:
  
  **Original Puzzle Grid:**
  - 9×9 grid display
  - Filled cells in normal style
  - Empty cells blank
  
  **Solution Grid:**
  - 9×9 grid display
  - Original numbers in gray background
  - Solved numbers in purple

---

## 📐 Latin Square Page

**Route**: `/latin-square`

### Layout:
**Left Panel - Configuration Card**
- Algorithm buttons (Backtracking / IDDFS)
- Grid Size (n) input slider/number (1-12)
- Info box explaining Latin Squares (light blue background)
- "Generate Square" button

**Right Panel - Results Card**
- Grid size (n×n)
- Algorithm used
- Latin Square visualization:
  - n×n grid of colored cells
  - Each number has a unique color
  - Numbers displayed in cells
  - Hover effect on cells (slight zoom)

**Validation Section** (green background)
- ✓ Each number appears once per row
- ✓ Each number appears once per column
- ✓ All numbers from 1 to n are used

**Example Section** (gray background)
- Shows text representation of a 5×5 Latin Square
- Monospace font with green text
- Dark background for code style

---

## 🎨 Navigation Bar (All Pages)

**Top of every page:**
- Gradient background (purple to pink)
- Left: Logo (sparkle icon) + "AI Search Algorithms" text
  - Clickable to go home
- Right: Three navigation links
  - Social Golfers
  - Sudoku Solver
  - Latin Square
  - Active page highlighted with light overlay

---

## 📱 Footer (All Pages)

**Bottom of every page:**
- White background
- Centered text
- "AI Search Algorithms - Uninformed Search Techniques"
- Algorithm badges: "DFS • BFS • IDDFS • Depth Limited Search • Backtracking"

---

## 🎨 Color Palette

### Gradients
- **Primary Gradient**: #7702FF → #CC26D5
- **Hero Gradient**: #7702FF → #CC26D5 → #F0060B

### Functional Colors
- **Success**: #22c55e (green)
- **Error**: #ef4444 (red)
- **Info**: #3b82f6 (blue)

### Neutrals
- **Background**: #f9fafb (light gray)
- **Cards**: #ffffff (white)
- **Text Dark**: #1a1a1a
- **Text Medium**: #666666
- **Borders**: #e0e0e0

### Semantic
- **Primary Button**: Gradient purple/pink
- **Hover**: Darker shade with transform
- **Disabled**: 60% opacity
- **Input Focus**: Purple border (#7702FF)

---

## 🔄 Interactive States

### Buttons
- **Normal**: Gradient background, white text
- **Hover**: Moves up 2px, larger shadow
- **Active/Pressed**: Slightly darker
- **Disabled**: Lower opacity, no pointer

### Cards
- **Normal**: White with subtle shadow
- **Hover**: Lifts up (translateY), larger shadow, top border appears

### Inputs
- **Normal**: Light gray border
- **Focus**: Purple border, no outline
- **Error**: Red border
- **Filled**: Slightly darker background

### Loading
- **Spinner**: Rotating purple circle
- **Large Spinner**: For main content areas
- **Button Spinner**: Small inline spinner

---

## 📐 Responsive Breakpoints

### Desktop (1400px+)
- Two-column layout (form left, results right)
- Full navigation bar
- Large fonts and spacing

### Tablet (768px-1400px)
- Two-column layout (adjusted widths)
- Full navigation bar
- Medium fonts and spacing

### Mobile (<768px)
- Single column layout (stacked)
- Navigation bar stacks vertically
- Smaller fonts and compact spacing
- Touch-friendly button sizes (min 44px)

---

## 🎯 User Flow Examples

### Quick Test Flow:
1. Land on Home page
2. Click "Social Golfers" card
3. Keep default values (16 players, 4 per group, 5 weeks)
4. Click "Solve"
5. See solution in ~1 second

### Custom Sudoku Flow:
1. Navigate to Sudoku page
2. Click "Custom Puzzle"
3. Enter your puzzle (0 for empty)
4. Click "Solve Puzzle"
5. Compare original and solution

### Algorithm Comparison Flow:
1. Navigate to Latin Square
2. Set n=5, select "Backtracking"
3. Click "Generate", note the result
4. Select "IDDFS"
5. Click "Generate", compare performance

---

## 🎬 Animation & Transitions

- **Page transitions**: Smooth (router animations could be added)
- **Button hover**: 0.2s ease
- **Card hover**: 0.3s ease
- **Spinner rotation**: 0.8s linear infinite
- **Border slide**: 0.3s scale transform
- **Cell hover**: 0.2s scale

---

## ♿ Accessibility Features

- Semantic HTML elements
- High contrast text colors
- Large click targets
- Keyboard navigation support (via Angular Router)
- Clear focus indicators
- Error messages are text-based
- Loading states announced

---

**All screens are fully functional and ready to use! 🎉**

