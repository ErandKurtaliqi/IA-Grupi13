# Social Golfers Problem - Udhëzues i Shpejtë

## 📋 Përmbledhje

Kjo implementim zgjidh **Social Golfers Problem** duke përdorur dy teknika kërkimi të painformuar:
1. **Depth First Search (DFS) me Backtracking**
2. **Depth Limited Search (DLS) me Backtracking**

## 🎯 Problemi

**Deklarata:** 32 lojtarë golfi luajnë golf një herë në javë në grupe me nga 4. Për sa javë mund të luajnë që dy lojtarë të mos luajnë së bashku më shumë se një herë?

**Gjeneralizimi:** n = g × p lojtarë në g grupe me p lojtarë për w javë.

## 🏗️ Modeli i Problemit

### Variablat
- **Week (Java)**: Çdo javë është një variabël
- Vlera: Një ndarje e lojtarëve në grupe

### Domeni
- Të gjitha ndarjet e vlefshme të n lojtarëve në g grupe me nga p lojtarë
- Constraint: Çdo lojtar shfaqet saktësisht një herë për javë

### Constraints (Kufizime)
1. ✅ Çdo lojtar në një grup për javë
2. ✅ Çdo grup ka saktësisht p lojtarë  
3. ✅ Asnjë çift lojtarësh nuk përsëritet në javë të ndryshme

## 📁 Struktura e Kodit

```
Detyra3/
├── Interfaces/
│   └── ISocialGolfersService.cs      # Interface me dokumentacion
├── Services/
│   └── SocialGolfersService.cs       # Implementimi i DFS dhe DLS
├── Controllers/
│   └── SocialGolfersController.cs    # API endpoints
└── Program.cs                         # Regjistrim i servisit
```

## 🔧 API Endpoints

### 1. DFS me Backtracking
```http
GET /api/socialgolfers/dfs?numPlayers=8&groupSize=4&numWeeks=2
```
**Përshkrimi:** Gjen zgjidhje duke përdorur DFS standard me backtracking.

**Parametrat:**
- `numPlayers`: Numri total i lojtarëve (default: 32)
- `groupSize`: Sa lojtarë në çdo grup (default: 4)
- `numWeeks`: Numri i javëve për të planifikuar (default: 10)

**Shembull Response:**
```json
{
  "algorithm": "DFS with Backtracking",
  "success": true,
  "numPlayers": 8,
  "groupSize": 4,
  "numGroups": 2,
  "numWeeks": 2,
  "nodesExplored": 145,
  "timeMs": 23,
  "solution": [
    {
      "weekNumber": 1,
      "groups": [
        { "groupNumber": 1, "players": [0, 1, 2, 3] },
        { "groupNumber": 2, "players": [4, 5, 6, 7] }
      ]
    },
    {
      "weekNumber": 2,
      "groups": [
        { "groupNumber": 1, "players": [0, 4, 5, 6] },
        { "groupNumber": 2, "players": [1, 2, 3, 7] }
      ]
    }
  ]
}
```

### 2. Depth Limited Search
```http
GET /api/socialgolfers/depth-limited?numPlayers=8&groupSize=4&numWeeks=2&depthLimit=10
```
**Përshkrimi:** Gjen zgjidhje duke përdorur Depth Limited Search me backtracking.

**Parametrat:**
- `numPlayers`, `groupSize`, `numWeeks`: Si më sipër
- `depthLimit`: Thellësia maksimale e kërkimit (default: 10)

### 3. Gjej Numrin Maksimal të Javëve
```http
GET /api/socialgolfers/find-max-weeks?numPlayers=16&groupSize=4&maxWeeksToTry=15
```
**Përshkrimi:** Provon të gjejë sa javë mund të luajnë maksimalisht.

**Parametrat:**
- `numPlayers`: Numri i lojtarëve (default: 16)
- `groupSize`: Madhësia e grupit (default: 4)
- `maxWeeksToTry`: Sa javë maksimalisht të provohen (default: 15)

### 4. Shembuj të Gatshëm

#### Shembull i thjeshtë (8 lojtarë, 2 javë)
```http
GET /api/socialgolfers/example-simple
```

#### Problemi origjinal (32 lojtarë, 5 javë)
```http
GET /api/socialgolfers/example-original?numWeeks=5
```

## 🚀 Si të Ekzekutoni

### Metodë 1: Visual Studio / Visual Studio Code
1. Hap `Detyra3.sln` në Visual Studio
2. Vendos `Detyra3` si startup project
3. Shtyp F5 për të filluar

### Metodë 2: Command Line
```bash
cd Detyra3
dotnet run
```

### Metodë 3: Swagger UI
1. Starto aplikacionin
2. Shko te: `https://localhost:7xxx/swagger`
3. Kliko në "SocialGolfers" controller
4. Provo endpoint-et direkt nga UI

## 📊 Shembuj Praktik

### Shembull 1: Rast i vogël (testim i shpejtë)
```
Input: 8 lojtarë, 4 në grup, 2 javë
Pritje: Zgjidhje në < 100ms
```

**Test në Swagger:**
```
GET /api/socialgolfers/dfs?numPlayers=8&groupSize=4&numWeeks=2
```

### Shembull 2: Rast mesatar
```
Input: 12 lojtarë, 4 në grup, 3 javë
Pritje: Zgjidhje në < 1 sekondë
```

**Test në Swagger:**
```
GET /api/socialgolfers/dfs?numPlayers=12&groupSize=4&numWeeks=3
```

### Shembull 3: Gjej maksimumin për 16 lojtarë
```
Input: 16 lojtarë, 4 në grup
Pyetje: Sa javë maksimalisht?
```

**Test në Swagger:**
```
GET /api/socialgolfers/find-max-weeks?numPlayers=16&groupSize=4&maxWeeksToTry=10
```

## 🧠 Algoritmet - Shpjegim i Shkurtër

### DFS me Backtracking

**Hapat:**
1. Fillo me një zgjidhje boshe
2. Për çdo javë, gjenero të gjitha ndarjet e mundshme
3. Kontrollo nëse ndarjа është e vlefshme (nuk përsërit çifte)
4. Nëse po, shto ndarjen dhe vazhdo me javën tjetër
5. Nëse arrin në fund → SUCCESS!
6. Nëse jo → Backtrack (hiq javën e fundit) dhe provo tjetrën

**Pseudokod:**
```
function DFS(solution, week):
    if week == target:
        return SUCCESS
    
    for partition in all_partitions:
        if valid(partition):
            add(partition)
            if DFS(solution, week+1) == SUCCESS:
                return SUCCESS
            remove(partition)  // Backtrack
    
    return FAIL
```

### Depth Limited Search

**Dallimi nga DFS:**
- Ka një limit maksimal të thellësisë
- Nuk shkon më thellë se `depthLimit`
- Më i sigurt për probleme të mëdha

**Pseudokod:**
```
function DLS(solution, week, limit):
    if week > limit:
        return FAIL  // Tepër thellë
    
    if week == target:
        return SUCCESS
    
    for partition in all_partitions:
        if valid(partition):
            add(partition)
            if DLS(solution, week+1, limit) == SUCCESS:
                return SUCCESS
            remove(partition)  // Backtrack
    
    return FAIL
```

## 📈 Kompleksiteti

### Hapësira (Space Complexity)
- **DFS:** O(w × g × p)
  - w = numri i javëve
  - g = numri i grupeve
  - p = numri i lojtarëve për grup

### Koha (Time Complexity)
- **Worst Case:** O(b^d)
  - b = branching factor (numri mesatar i ndarjeve të mundshme)
  - d = thellësia (numri i javëve)

### Performanca Praktike
| Lojtarë | Grupe | Javë | Kohë e Pritur |
|---------|-------|------|---------------|
| 8       | 2     | 2    | < 100ms       |
| 12      | 3     | 3    | < 1s          |
| 16      | 4     | 4    | < 10s         |
| 32      | 8     | 5    | minutes+      |

## ⚠️ Limitime dhe Vërejtje

1. **Safety Limit:** Kemi vendosur `MAX_NODES = 100,000` për të parandaluar hang-et
2. **Probleme të Mëdha:** Për 32+ lojtarë, algoritmi mund të jetë shumë i ngadaltë
3. **Optimaliteti:** DFS gjen zgjidhjen e parë, jo domosdoshmërisht optimalen
4. **Memoria:** Për probleme të mëdha, mund të konsumohet shumë memorie

## 🎓 Koncepte Kyçe për Prezantim

### 1. Modeli CSP (Constraint Satisfaction Problem)
- **Variables:** Javët
- **Domain:** Ndarjet e lojtarëve
- **Constraints:** Çiftet unike

### 2. Backtracking
- Provo një zgjidhje
- Nëse nuk funksionon, kthehu mbrapa dhe provo tjetrën
- Elegante dhe e plotë

### 3. Depth Limited Search
- DFS me mbrojtje
- Parandalon "humbjën" në thellësi të pafundme
- I mirë për testing

## 📚 Material Shtesë

Shiko `SOCIAL_GOLFERS_DOKUMENTACIONI.md` për:
- Shpjegim të detajuar të algoritmeve
- Analiza teorike
- Shembuj më kompleks
- Referencat dhe literatura

## ✅ Checklist për Prezantim

- [ ] Shpjego problemin me fjalë të thjeshta
- [ ] Trego modelimin CSP (Variables, Domain, Constraints)
- [ ] Demonstro një ekzekutim në Swagger UI
- [ ] Shpjego DFS me Backtracking (trego pseudokodin)
- [ ] Shpjego Depth Limited Search (dallimi nga DFS)
- [ ] Trego rezultatet për raste të ndryshme
- [ ] Diskuto kompleksitetin dhe limitimet

## 🤝 Kontributi

**Detyra 1:** Social Golfers Problem ✅
- Modeli i problemit ✅
- Reprezentimi i state-it ✅
- DFS me Backtracking ✅
- Depth Limited Search me Backtracking ✅
- API Endpoints ✅
- Dokumentacion i plotë ✅

**Krijuar:** [Data]
**Anëtarët e Grupit:** [Emrat]
**Kursi:** Inteligjenca Artificiale - IA

---

*Për çdo pyetje ose problem, konsultoni dokumentacionin e plotë në `SOCIAL_GOLFERS_DOKUMENTACIONI.md`*

