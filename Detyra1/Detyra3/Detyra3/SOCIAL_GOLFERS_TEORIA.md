# Social Golfers Problem - Analizë Teorike dhe Implementim

## Hyrje

Social Golfers Problem është një problem klasik i optimizimit kombinatorial dhe një shembull i mirë i një Constraint Satisfaction Problem (CSP). Ky dokument shpjegon në detaje teorinë, modelimin, dhe implementimin e zgjidhjes.

---

## 1. FORMULIMI I PROBLEMIT

### 1.1 Përshkrimi Formal

**Dhënë:**
- `n` = numri total i lojtarëve golfi
- `g` = numri i grupeve
- `p` = numri i lojtarëve për grup
- `w` = numri i javëve

**Kushtet:**
- n = g × p (numri i lojtarëve është i plotpjesëtueshëm me madhësinë e grupit)

**Objektivi:**
Të planifikohen `w` javë lojërash në mënyrë që:
1. Çdo lojtar luan saktësisht një herë për javë
2. Çdo grup ka saktësisht `p` lojtarë
3. Asnjë dy lojtarë nuk luajnë së bashku në të njëjtin grup më shumë se një herë

**Pyetja:**
Cili është numri maksimal i javëve `w_max` që mund të arrihet për një konfiguracion të dhënë `(n, g, p)`?

### 1.2 Rasti i Dhënë në Detyrë

**Parametrat:**
- n = 32 lojtarë
- g = 8 grupe
- p = 4 lojtarë për grup
- w = ? (për të përcaktuar)

**Pyetja:** Për sa javë mund të luajnë 32 lojtarë?

---

## 2. MODELIMI SI CSP

### 2.1 Çfarë është një CSP?

Një **Constraint Satisfaction Problem (CSP)** përkufizohet nga:
- Një set variablash: X = {X₁, X₂, ..., Xₙ}
- Një domain për çdo variabël: D = {D₁, D₂, ..., Dₙ}
- Një set kufizimesh (constraints): C = {C₁, C₂, ..., Cₖ}

**Objektivi:** Të gjesh një vlerësim të variablave që kënaq të gjitha kufizime.

### 2.2 Modelimi i Social Golfers si CSP

#### Variables (Variablat)
```
X = {Week₁, Week₂, ..., Weekw}
```
Çdo variabël `Weekᵢ` përfaqëson konfigurimin e grupeve për javën i.

#### Domain (Domeni)
Për çdo `Weekᵢ`, domeni `Dᵢ` përbëhet nga të gjitha ndarjet e vlefshme të `n` lojtarëve në `g` grupe me nga `p` lojtarë.

**Shembull për n=8, g=2, p=4:**
```
D = {
    [[0,1,2,3], [4,5,6,7]],
    [[0,1,2,4], [3,5,6,7]],
    [[0,1,2,5], [3,4,6,7]],
    ...
}
```

**Madhësia e domenit:**
Numri i ndarjeve të ndryshme të n lojtarëve në g grupe është:

```
|D| = (n!)/(p!^g × g!)
```

Për n=32, g=8, p=4:
```
|D| = 32!/(4!^8 × 8!) ≈ 2.08 × 10²¹
```
Kjo është një hapësirë shumë e madhe!

#### Constraints (Kufizime)

**Constraint 1: Plotësia (Completeness)**
```
∀ week ∈ Weeks: 
    ∀ player ∈ [0, n-1]:
        player shfaqet saktësisht në një grup
```

**Constraint 2: Madhësia e Grupit**
```
∀ week ∈ Weeks:
    ∀ group ∈ week:
        |group| = p
```

**Constraint 3: Unikshmëria e Çifteve**
```
∀ week₁, week₂ ∈ Weeks (week₁ ≠ week₂):
    ∀ group₁ ∈ week₁, group₂ ∈ week₂:
        |group₁ ∩ group₂| ≤ 1
```
(Çdo dy grupe në javë të ndryshme kanë maksimumi 1 lojtar të përbashkët)

**Formulim alternativ i Constraint 3:**
```
∀ player₁, player₂ ∈ Players (player₁ ≠ player₂):
    ∃! week ∈ Weeks: player₁ dhe player₂ janë në të njëjtin grup
```
(Çdo çift lojtarësh mund të jenë së bashku në maksimumi një grup)

---

## 3. REPREZENTIMI I STATE-IT

### 3.1 Struktura e të Dhënave

**Në kod (C#):**
```csharp
List<List<List<int>>> solution
    ├── Week 0: List<List<int>>
    │   ├── Group 0: List<int> → [0, 1, 2, 3]
    │   ├── Group 1: List<int> → [4, 5, 6, 7]
    │   └── ...
    ├── Week 1: List<List<int>>
    │   ├── Group 0: List<int> → [0, 4, 8, 12]
    │   └── ...
    └── ...
```

**Struktura abstrakte:**
```
State = (W₁, W₂, ..., Wₖ)
  ku Wᵢ = (G₁, G₂, ..., Gₘ)
  ku Gⱼ = {p₁, p₂, ..., pₙ}
```

### 3.2 State Space (Hapësira e Gjendjes)

**Initial State (Gjendja fillestare):**
```
S₀ = () 
```
Asnjë javë nuk është planifikuar.

**Goal State (Gjendja e synuar):**
```
S_goal = (W₁, W₂, ..., Ww)
```
Të gjitha `w` javët janë planifikuar dhe të gjitha constraints janë të kënaqura.

**Transition Function (Funksioni i tranzicionit):**
```
T(S, week_partition) = S'
```
ku `S'` është `S` me një javë të re të shtuar.

**Valid Transition:**
Një tranzicion është i vlefshëm nëse dhe vetëm nëse `week_partition` nuk shkel constraint-in e unikshmërisë së çifteve.

### 3.3 Gjurmimi i Çifteve

Për të kontrolluar shpejt constraint-in e unikshmërisë, mbajmë një strukture shtesë:

```csharp
HashSet<string> _pairingsUsed
```

**Përmbajtja:**
```
_pairingsUsed = {"0-1", "0-2", "0-3", "1-2", "1-3", "2-3", ...}
```

**Avantazhet:**
- Lookup në O(1)
- Insert në O(1)
- Delete në O(1) (për backtracking)

---

## 4. ALGORITMET

### 4.1 Depth First Search (DFS) me Backtracking

#### 4.1.1 Ideja Kryesore

DFS eksploron hapësirën e kërkimit në **thellësi**, duke tentuar të zgjidhë një javë në të cilën herë.

**Hapat:**
1. Zgjidh javën aktuale duke përzgjedhur një ndarje të vlefshme
2. Shko në thellësi (zgjidh javën tjetër)
3. Nëse arrin në një rrugë pa zgjidhje, **backtrack** (kthehu prapa) dhe provo një zgjidhje tjetër

#### 4.1.2 Pseudokod i Detajuar

```
ALGORITHM DFS-BACKTRACK(solution, currentWeek, targetWeeks)
INPUT:
    solution: zgjidhja parciale (lista e javëve të zgjidhura)
    currentWeek: numri i javës aktuale (0-indexed)
    targetWeeks: numri total i javëve për të zgjidhur

OUTPUT:
    true nëse gjendet zgjidhje, false ndryshe

BEGIN
    // Base case: Kemi zgjidhur të gjitha javët
    IF currentWeek == targetWeeks THEN
        RETURN true
    END IF
    
    // Gjenero të gjitha ndarjet e mundshme për këtë javë
    partitions ← GeneratePartitions(numPlayers, groupSize, numGroups)
    
    // Provo çdo ndarje
    FOR EACH partition IN partitions DO
        // Kontrollo nëse kjo ndarje është e vlefshme
        IF IsValidPartition(partition) THEN
            // Shto këtë ndarje në zgjidhje
            solution.Add(partition)
            AddPairings(partition)
            
            // Rekurro për javën tjetër
            IF DFS-BACKTRACK(solution, currentWeek + 1, targetWeeks) THEN
                RETURN true  // Zgjidhje e gjetur!
            END IF
            
            // Backtrack: Hiq këtë ndarje
            solution.RemoveLast()
            RemovePairings(partition)
        END IF
    END FOR
    
    // Asnjë ndarje nuk funksionoi
    RETURN false
END
```

#### 4.1.3 Helper Functions

**IsValidPartition:**
```
FUNCTION IsValidPartition(partition)
    FOR EACH group IN partition DO
        FOR EACH pair (player1, player2) IN group DO
            pairKey ← CreatePairKey(player1, player2)
            IF pairKey ∈ _pairingsUsed THEN
                RETURN false  // Ky çift ka luajtur së bashku më parë
            END IF
        END FOR
    END FOR
    RETURN true
END FUNCTION
```

**AddPairings:**
```
FUNCTION AddPairings(partition)
    FOR EACH group IN partition DO
        FOR EACH pair (player1, player2) IN group DO
            pairKey ← CreatePairKey(player1, player2)
            _pairingsUsed.Add(pairKey)
        END FOR
    END FOR
END FUNCTION
```

**RemovePairings (për backtracking):**
```
FUNCTION RemovePairings(partition)
    FOR EACH group IN partition DO
        FOR EACH pair (player1, player2) IN group DO
            pairKey ← CreatePairKey(player1, player2)
            _pairingsUsed.Remove(pairKey)
        END FOR
    END FOR
END FUNCTION
```

#### 4.1.4 Analiza e Kompleksitetit

**Time Complexity:**
- **Worst case:** O(b^d)
  - b = branching factor ≈ |D| (madhësia e domenit)
  - d = thellësia = w (numri i javëve)
- **Best case:** O(w) nëse zgjidhja gjendet menjëherë

**Space Complexity:**
- **Stack depth:** O(w) për rekursionin
- **State storage:** O(w × g × p) për zgjidhjen
- **Pairings:** O(w × g × C(p, 2)) për _pairingsUsed
  - C(p, 2) = p × (p-1) / 2 është numri i çifteve në një grup

**Total:** O(w × g × p²)

#### 4.1.5 Karakteristikat

| Karakteristikë | Vlera |
|----------------|-------|
| **Kompletësi** | Po - gjen zgjidhje nëse ekziston |
| **Optimaliteti** | Jo - gjen zgjidhjen e parë, jo domosdoshmërisht më të mirën |
| **Kompleksiteti kohor** | O(b^d) - eksponencial |
| **Kompleksiteti hapësinor** | O(d) - linear në thellësi |

### 4.2 Depth Limited Search (DLS) me Backtracking

#### 4.2.1 Ideja Kryesore

DLS është një **variant i DFS** që kufizon thellësinë e kërkimit në një vlerë maksimale `depthLimit`.

**Arsyet për përdorimin:**
1. **Siguria:** Parandalon loop-et e pafundme
2. **Kontrolli:** Lejon testimin sistematik me thellësi të vogla
3. **Iterative Deepening:** Mund të kombinohet me rritje graduale të limitit

#### 4.2.2 Pseudokod

```
ALGORITHM DEPTH-LIMITED-SEARCH(solution, currentWeek, targetWeeks, depthLimit)
INPUT:
    solution: zgjidhja parciale
    currentWeek: java aktuale
    targetWeeks: numri i javëve për të zgjidhur
    depthLimit: thellësia maksimale e lejuar

OUTPUT:
    true nëse gjendet zgjidhje brenda limitit, false ndryshe

BEGIN
    // Kontrollo limitin e thellësisë
    IF currentWeek > depthLimit THEN
        RETURN false  // Kemi shkuar shumë thellë
    END IF
    
    // Base case: Kemi zgjidhur të gjitha javët
    IF currentWeek == targetWeeks THEN
        RETURN true
    END IF
    
    // Pjesa tjetër është identike me DFS
    partitions ← GeneratePartitions(numPlayers, groupSize, numGroups)
    
    FOR EACH partition IN partitions DO
        IF IsValidPartition(partition) THEN
            solution.Add(partition)
            AddPairings(partition)
            
            IF DEPTH-LIMITED-SEARCH(solution, currentWeek + 1, targetWeeks, depthLimit) THEN
                RETURN true
            END IF
            
            solution.RemoveLast()
            RemovePairings(partition)
        END IF
    END FOR
    
    RETURN false
END
```

#### 4.2.3 Iterative Deepening Depth-First Search (IDDFS)

DLS shpesh kombinohet me **Iterative Deepening**:

```
ALGORITHM ITERATIVE-DEEPENING-DLS(numPlayers, groupSize, targetWeeks, maxDepth)
    FOR depth FROM 1 TO maxDepth DO
        solution ← EmptyList()
        _pairingsUsed ← EmptySet()
        
        IF DEPTH-LIMITED-SEARCH(solution, 0, targetWeeks, depth) THEN
            RETURN (true, solution)
        END IF
    END FOR
    
    RETURN (false, null)
END
```

#### 4.2.4 Analiza e Kompleksitetit

**Time Complexity:**
- Për një depth limit l: O(b^l)
- Me iterative deepening: O(b^d) (i njëjtë si DFS, por me konstantë më të madhe)

**Space Complexity:**
- O(l) ku l është depth limit (më mirë se DFS për probleme të mëdha)

#### 4.2.5 Avantazhet vs Disavantazhet

**Avantazhet:**
- ✅ Më i sigurt se DFS (nuk bie në thellësi të pafundme)
- ✅ Space complexity më i mirë
- ✅ I mirë për testing dhe debug

**Disavantazhet:**
- ❌ Mund të mos gjejë zgjidhje nëse limit është shumë i vogël
- ❌ Me iterative deepening, ri-eksploron node-t (më ngadalë)

---

## 5. OPTIMIZIME DHE HEURISTIKA

### 5.1 Constraint Propagation

Para se të shtohet një ndarje, kontrollojmë nëse shkakton konflikt:

```csharp
if (_pairingsUsed.Contains(pair))
    return false;  // Mos e eksploro këtë degë
```

Kjo është një formë e **forward checking** që redukton hapësirën e kërkimit.

### 5.2 Symmetry Breaking

Mund të reduktohet hapësira e kërkimit duke eleminuar zgjidhje simetrike:

**Shembull:**
```
Week 1: [[0,1,2,3], [4,5,6,7]]
Week 1: [[4,5,6,7], [0,1,2,3]]  // Simetrike! E njëjta zgjidhje
```

**Teknika:**
- Fikso grupin e parë të javës së parë: `[0, 1, 2, 3]`
- Fikso lojtarin me ID më të vogël në çdo grup

### 5.3 Heuristic Ordering

Zgjedhja e rendit të provimit të ndarjeve mund të përmirësojë performancën:

**Most Constrained First:**
Provo fillimisht ndarjet që krijojnë më pak çifte të reja (lënë më shumë opsione për të ardhmen).

### 5.4 Caching

Mund të cache-ojmë ndarjet e gjeneruara:

```csharp
private Dictionary<int, List<List<List<int>>>> _partitionCache;
```

---

## 6. VLERËSIMI TEORIK

### 6.1 Numri Maksimal i Javëve

**Limiti teorik sipër:**

Për çdo lojtar, numri maksimal i çifteve unike që mund të formojë është:
```
max_pairs_per_player = n - 1
```

Në çdo javë, çdo lojtar luan me `p - 1` lojtarë të tjerë. Pra, numri maksimal i javëve është:
```
w_max ≤ (n - 1) / (p - 1)
```

**Për rastin tonë (n=32, p=4):**
```
w_max ≤ 31 / 3 ≈ 10.33
```

Pra, maksimumi teorik është **10 javë**.

### 6.2 Kompleksiteti i Problemit

Social Golfers Problem është **NP-complete**. Kjo do të thotë:
1. Nuk ka algoritëm polinomial i njohur
2. Zgjidhja e rasteve të mëdha kërkon kohë eksponenciale
3. Heuristikat dhe optimizimi janë thelbësore

---

## 7. KRAHASIMI I ALGORITMEVE

| Aspekt | DFS me Backtracking | Depth Limited Search |
|--------|---------------------|----------------------|
| **Thellësia** | E pakufizuar | E kufizuar |
| **Siguria** | Mund të bie në loop | Më i sigurt |
| **Space** | O(d) | O(l) ku l < d |
| **Time** | O(b^d) | O(b^l) |
| **Kompletësi** | Po (në finitohet) | Vetëm brenda limitit |
| **Zgjidhje** | E para e gjetur | E para brenda limitit |

---

## 8. KONKLUZIONI

Social Golfers Problem është një CSP sfidues që demonstron:
1. **Fuqinë e modelimit CSP** për probleme të botës reale
2. **Efektivitetin e backtracking** për eksplorim sistematik
3. **Nevojën për optimizime** në probleme eksponenciale

Implementimi ynë ofron dy qasje:
- **DFS**: E plotë dhe e thjeshtë
- **DLS**: Më e sigurt dhe e kontrollueshme

Për raste praktike të mëdha, algoritme më të avancuara (Arc Consistency, Constraint Programming Solvers) janë të nevojshme.

---

## 9. REFERENCAT

1. **Warwick Harvey (2002)** - "Symmetry Breaking and the Social Golfer Problem"
2. **Barbara M. Smith et al. (2003)** - "The Social Golfer Problem: A Challenging CSP"
3. **Russell & Norvig (2020)** - "Artificial Intelligence: A Modern Approach" (Chapter 6: Constraint Satisfaction Problems)
4. **Wikipedia** - "Social Golfer Problem"
   - URL: https://en.wikipedia.org/wiki/Social_golfer_problem

---

**Përgatiti:** [Emri]  
**Data:** [Data]  
**Kursi:** Inteligjenca Artificiale  
**Universiteti:** [Universiteti]

