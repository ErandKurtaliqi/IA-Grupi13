# Social Golfers Problem - Dokumentacioni

## Përshkrimi i Problemit

32 lojtarë golfi luajnë golf një herë në javë dhe gjithmonë në grupe me nga 4 lojtarë. Për sa javë mund të luajnë në mënyrë që dy lojtarë të mos luajnë së bashku më shumë se një herë në të njëjtin grup?

Në përgjithësi: A është e mundur të planifikohen **n = g × p** lojtarë golfi në **g** grupe me **p** lojtarë për **w** javë në mënyrë që të mos luajnë dy lojtarë golfi në të njëjtin grup më shumë se një herë?

## Modelimi i Problemit

### Variablat (Variables)
- Çdo **javë (week)** konsiderohet si një variabël që duhet të zgjidhet
- Çdo javë përmban një set grupesh
- Çdo grup përmban lojtarë

### Domeni (Domain)
- Domeni për çdo javë është **të gjitha ndarjet e mundshme** të n lojtarëve në g grupe me nga p lojtarë
- Për shembull, për 8 lojtarë në 2 grupe me nga 4:
  - Një ndarje e mundur: [[0,1,2,3], [4,5,6,7]]
  - Një ndarje tjetër: [[0,1,4,5], [2,3,6,7]]
  - etj.

### Constraints (Kufizime)
1. **Constraint 1 - Plotësia**: Çdo lojtar duhet të shfaqet saktësisht një herë për javë
2. **Constraint 2 - Madhësia e grupit**: Çdo grup duhet të ketë saktësisht p lojtarë
3. **Constraint 3 - Unikshmëria e çifteve**: Dy lojtarë nuk mund të jenë në të njëjtin grup më shumë se një herë gjatë të gjitha javëve

## Reprezentimi i State-it

### Struktura e të Dhënave
```
State: List<List<List<int>>>
├── Week 1: List<List<int>>
│   ├── Group 1: List<int> → [0, 1, 2, 3]
│   ├── Group 2: List<int> → [4, 5, 6, 7]
│   └── ...
├── Week 2: List<List<int>>
│   ├── Group 1: List<int> → [0, 4, 8, 12]
│   ├── Group 2: List<int> → [1, 5, 9, 13]
│   └── ...
└── ...
```

### Gjendja Fillestare (Initial State)
- Listë bosh: `[]`
- Asnjë javë nuk është plotësuar ende

### Gjendja e Synuar (Goal State)
- `w` javë të plotësuara
- Të gjitha constraint-et janë të kënaqura

## Algoritmet e Implementuara

### 1. Depth First Search (DFS) me Backtracking

#### Pseudokod
```
function DFS_BACKTRACK(solution, week):
    if week == numWeeks:
        return true  // Zgjidhja u gjet
    
    for each partition in GeneratePartitions():
        if IsValidPartition(partition):
            solution.add(partition)
            AddPairings(partition)
            
            if DFS_BACKTRACK(solution, week + 1):
                return true
            
            // Backtrack
            solution.removeLast()
            RemovePairings(partition)
    
    return false
```

#### Karakteristikat
- **Hapësira**: O(w × g × p) - ku w=weeks, g=groups, p=players per group
- **Kompleksiteti kohor**: O(b^d) ku b është branching factor, d është thellësia (numri i javëve)
- **Komplet**: Po, gjen zgjidhjen nëse ekziston
- **Optimal**: Gjen zgjidhjen e parë të gjetur (jo domosdoshmërisht optimale)

### 2. Depth Limited Search (DLS) me Backtracking

#### Pseudokod
```
function DEPTH_LIMITED_DFS(solution, week, limit):
    if week > limit:
        return false  // Limit i thellësisë u arrit
        
    if week == numWeeks:
        return true  // Zgjidhja u gjet
    
    for each partition in GeneratePartitions():
        if IsValidPartition(partition):
            solution.add(partition)
            AddPairings(partition)
            
            if DEPTH_LIMITED_DFS(solution, week + 1, limit):
                return true
            
            // Backtrack
            solution.removeLast()
            RemovePairings(partition)
    
    return false

// Iterative Deepening
for depth from 1 to maxDepth:
    if DEPTH_LIMITED_DFS([], 0, depth):
        return solution
```

#### Karakteristikat
- **Hapësira**: O(limit × g × p) - më pak se DFS i thjeshtë
- **Kompleksiteti kohor**: O(b^limit)
- **Komplet**: Po (kur kombinohet me iterative deepening)
- **Optimal**: Gjen zgjidhjen më të cekët

## Heuristika dhe Optimizime

### 1. Constraint Propagation
Gjatë gjenerimit të ndarjeve, kontrollojmë menjëherë nëse çiftet përsëriten:
```csharp
if (_pairingsUsed.Contains(pair))
    return false;  // Skip this partition early
```

### 2. Early Pruning
Nuk gjenerojmë të gjitha ndarjet e mundshme - përdorim një approach heuristic për të reduktuar hapësirën e kërkimit.

### 3. HashSet për Çiftet
Përdorim `HashSet<string>` për të kontrolluar shpejt nëse një çift ka luajtur së bashku:
```csharp
string pairKey = $"{min}-{max}";
_pairingsUsed.Contains(pairKey);  // O(1) lookup
```

## Shembuj të Përdorimit

### Shembull 1: Rast i Thjeshtë (8 lojtarë, 4 në grup)

**Input:**
- numPlayers = 8
- groupSize = 4
- numWeeks = 2

**Output i mundshëm:**
```
Week 1:
  Group 1: [0, 1, 2, 3]
  Group 2: [4, 5, 6, 7]

Week 2:
  Group 1: [0, 4, 1, 5]
  Group 2: [2, 6, 3, 7]
```

**Verifikimi:**
- Week 1: Çiftet: (0,1), (0,2), (0,3), (1,2), (1,3), (2,3), (4,5), (4,6), (4,7), (5,6), (5,7), (6,7)
- Week 2: Çiftet: (0,4), (0,1), (0,5), (4,1), (4,5), (1,5), (2,6), (2,3), (2,7), (6,3), (6,7), (3,7)
- Problem: Disa çifte përsëriten! Kjo nuk është zgjidhje e vlefshme.

**Zgjidhje e saktë:**
```
Week 1:
  Group 1: [0, 1, 2, 3]
  Group 2: [4, 5, 6, 7]

Week 2:
  Group 1: [0, 4, 5, 6]
  Group 2: [1, 2, 7, 3]  // Të gjitha çiftet janë të reja!
```

### Shembull 2: Problemi Origjinal (32 lojtarë, 4 në grup)

**Input:**
- numPlayers = 32
- groupSize = 4
- numWeeks = ?

**Pyetja:** Sa javë mund të luajnë?

**Llogaritja Teorike:**
- Çdo lojtar mund të luajë me 3 lojtarë të tjerë për javë
- Ka 31 lojtarë të tjerë
- Maksimumi teorik: 31/3 ≈ 10 javë (por ky është maksimum teorik, në praktikë mund të jetë më pak)

## API Endpoints

### 1. DFS me Backtracking
```
GET /api/socialgolfers/dfs?numPlayers=8&groupSize=4&numWeeks=2
```

### 2. Depth Limited Search
```
GET /api/socialgolfers/depth-limited?numPlayers=8&groupSize=4&numWeeks=2&depthLimit=5
```

### 3. Gjej Numrin Maksimal të Javëve
```
GET /api/socialgolfers/find-max-weeks?numPlayers=16&groupSize=4&maxWeeksToTry=10
```

### 4. Shembuj të Gatshëm
```
GET /api/socialgolfers/example-simple
GET /api/socialgolfers/example-original?numWeeks=5
```

## Rezultatet e Pritshme

### Performanca
- **8 lojtarë, 4 në grup**: Shpejtë (milliseconds)
- **16 lojtarë, 4 në grup**: Mesatar (seconds)
- **32 lojtarë, 4 në grup**: I ngadaltë (minutes ose më shumë)

### Limitimet
- Kemi vendosur një limit sigurie të `MAX_NODES = 100,000` për të parandaluar hang-et
- Për probleme të mëdha (32+ lojtarë), mund të mos gjenden zgjidhje brenda limitit kohor

## Referencat dhe Material Shtesë

1. **CSP (Constraint Satisfaction Problem)**: Social Golfers është një CSP klasik
2. **Symmetry Breaking**: Mund të përdoren teknika për të reduktuar hapësirën e kërkimit duke eliminuar zgjidhje simetrike
3. **Research Papers**:
   - "The Social Golfer Problem" - Harvey (2002)
   - "Breaking symmetries in constraint satisfaction problems" - Puget (2004)

## Si të Testoni

1. **Starto aplikacionin:**
   ```bash
   cd Detyra3
   dotnet run
   ```

2. **Hap Swagger UI:**
   ```
   https://localhost:7xxx/swagger
   ```

3. **Provo endpoint-et:**
   - Fillo me `example-simple` për të parë një shembull të shpejtë
   - Përdor `find-max-weeks` për të gjetur limitin maksimal për një konfiguracion
   - Eksperimento me parametra të ndryshëm

## Pyetje të Shpeshta (FAQ)

**P: Pse është aq i vështirë ky problem?**
A: Hapësira e kërkimit rritet eksponencialisht me numrin e lojtarëve. Për 32 lojtarë, ka miliarda kombinime të mundshme.

**P: A ka zgjidhje për 32 lojtarë për 10 javë?**
A: Po, ekziston, por gjenerimi i saj kërkon algoritme të avancuara dhe optimizime. Implementimi ynë mund të mos jetë mjaft i shpejtë për ta gjetur.

**P: Çfarë dallimi ka midis DFS dhe Depth Limited Search?**
A: DFS eksploron në thellësi pa limit, ndërsa Depth Limited Search ka një thellësi maksimale. DLS është më i sigurt për probleme të mëdha sepse nuk bie në loop të pafund.

**P: Si mund ta përmirësoj performancën?**
A: 
1. Përdor heuristika më të mira për gjenerimin e ndarjeve
2. Implemento symmetry breaking
3. Përdor forward checking gjatë backtracking-ut
4. Konsidero algoritme më të avancuara si Arc Consistency

## Konkluzioni

Social Golfers Problem është një problem i vështirë NP-complete që demonstron fuqinë dhe limitimet e teknikave të kërkimit të painformuar. Implementimi ynë ofron dy qasje:

1. **DFS me Backtracking**: E thjeshtë dhe e plotë, por mund të jetë e ngadaltë
2. **Depth Limited Search**: Më e kontrolluar, mirë për testing

Për probleme praktike të mëdha, algoritme të specializuara (si CSP solvers) janë më efektive.

