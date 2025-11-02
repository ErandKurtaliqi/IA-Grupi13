# 🚀 Si të Testoni Social Golfers Problem

## Metoda 1: Command Line (Më e thjeshtë)

### Hapi 1: Navigo në folder-in e projektit
```bash
cd Detyra3
```

### Hapi 2: Ekzekuto aplikacionin
```bash
dotnet run
```

Aplikacioni do të fillojë dhe do të hapet automatikisht Swagger UI në browser në:
- **HTTPS:** https://localhost:7116/swagger
- **HTTP:** http://localhost:5157/swagger

### Hapi 3: Në Swagger UI
1. Gjeni sekcionin **"SocialGolfers"**
2. Klikoni në endpoint që dëshironi të testoni
3. Klikoni "Try it out"
4. Jepni parametrat (ose përdorni default)
5. Klikoni "Execute"

---

## Metoda 2: Visual Studio / VS Code

### Në Visual Studio:
1. Hap `Detyra3.sln`
2. Vendos `Detyra3` si startup project
3. Shtyp **F5** ose kliko "Run"

### Në VS Code:
1. Hap folder-in `Detyra3`
2. Shtyp **F5** ose kliko "Run and Debug"

Swagger do të hapet automatikisht!

---

## 🧪 Testim i Shpejtë - Shembuj

### Test 1: Shembull i Thjeshtë (8 lojtarë, 2 javë)
**Endpoint:**
```
GET /api/socialgolfers/example-simple
```

**Në Swagger:**
1. Gjej `GET /api/socialgolfers/example-simple`
2. Kliko "Try it out" → "Execute"
3. Rezultati duhet të shfaqet menjëherë (< 100ms)

**Në Browser direkt:**
```
https://localhost:7116/api/socialgolfers/example-simple
```

---

### Test 2: DFS me Parametra Custom
**Endpoint:**
```
GET /api/socialgolfers/dfs?numPlayers=8&groupSize=4&numWeeks=2
```

**Në Swagger:**
1. Gjej `GET /api/socialgolfers/dfs`
2. Kliko "Try it out"
3. Jepni parametrat:
   - `numPlayers`: 8
   - `groupSize`: 4
   - `numWeeks`: 2
4. Kliko "Execute"

**Në Browser:**
```
https://localhost:7116/api/socialgolfers/dfs?numPlayers=8&groupSize=4&numWeeks=2
```

**Rezultati i Pritur:**
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
  "solution": [...]
}
```

---

### Test 3: Depth Limited Search
**Endpoint:**
```
GET /api/socialgolfers/depth-limited?numPlayers=12&groupSize=4&numWeeks=3&depthLimit=5
```

**Në Swagger:**
1. Gjej `GET /api/socialgolfers/depth-limited`
2. Jepni:
   - `numPlayers`: 12
   - `groupSize`: 4
   - `numWeeks`: 3
   - `depthLimit`: 5
3. Execute

---

### Test 4: Gjej Numrin Maksimal të Javëve
**Endpoint:**
```
GET /api/socialgolfers/find-max-weeks?numPlayers=16&groupSize=4&maxWeeksToTry=10
```

Ky endpoint provon të gjejë sa javë maksimalisht mund të luajnë 16 lojtarë.

---

## 📋 Të Gjitha Endpoint-et

| Endpoint | Metoda | Parametrat | Përshkrimi |
|----------|--------|------------|------------|
| `/api/socialgolfers/dfs` | GET | `numPlayers`, `groupSize`, `numWeeks` | DFS me Backtracking |
| `/api/socialgolfers/depth-limited` | GET | `numPlayers`, `groupSize`, `numWeeks`, `depthLimit` | Depth Limited Search |
| `/api/socialgolfers/find-max-weeks` | GET | `numPlayers`, `groupSize`, `maxWeeksToTry` | Gjen maksimumin |
| `/api/socialgolfers/example-simple` | GET | - | Shembull i thjeshtë (8 lojtarë) |
| `/api/socialgolfers/example-original` | GET | `numWeeks` | Problemi origjinal (32 lojtarë) |

---

## 🔧 Nëse Ka Probleme

### Problem: "Port already in use"
**Zgjidhje:**
```bash
# Në Windows PowerShell
netstat -ano | findstr :7116
taskkill /PID <PID> /F
```

### Problem: "Cannot find project"
**Zgjidhje:**
```bash
cd "c:\Users\lenovo\Desktop\IA Ushtrime\IA-Grupi13\Detyra1\Detyra3\Detyra3"
dotnet run
```

### Problem: "Build failed"
**Zgjidhje:**
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

---

## 🌐 Testim me Browser Direkt (pa Swagger)

Pasi aplikacioni është duke u ekzekutuar, mund të hapësh direkt në browser:

### 1. Shembull i Thjeshtë
```
http://localhost:5157/api/socialgolfers/example-simple
```
ose
```
https://localhost:7116/api/socialgolfers/example-simple
```

### 2. DFS Custom
```
http://localhost:5157/api/socialgolfers/dfs?numPlayers=8&groupSize=4&numWeeks=2
```

### 3. Problemi Origjinal (32 lojtarë)
```
http://localhost:5157/api/socialgolfers/example-original?numWeeks=5
```
⚠️ **Vërejtje:** Kjo mund të jetë shumë e ngadaltë! Filloni me `numWeeks=2` ose `3`.

---

## 📊 Çfarë të Shikoni në Rezultat

### Nëse SUCCESS:
```json
{
  "success": true,
  "nodesExplored": 145,      // Sa node janë eksploruar
  "timeMs": 23,              // Koha në millisekonda
  "solution": [
    {
      "weekNumber": 1,
      "groups": [
        {
          "groupNumber": 1,
          "players": [0, 1, 2, 3]
        },
        {
          "groupNumber": 2,
          "players": [4, 5, 6, 7]
        }
      ]
    },
    ...
  ]
}
```

### Nëse FAIL:
```json
{
  "success": false,
  "message": "Nuk u gjet zgjidhje...",
  "nodesExplored": 50000
}
```

---

## 💡 Rekomandime për Testing

1. **Fillo me raste të vogla:**
   - 8 lojtarë, 2 javë → Shpejtë (< 100ms)
   - 12 lojtarë, 3 javë → Mesatar (< 1s)

2. **Rrit gradualisht:**
   - 16 lojtarë, 4 javë → Më e ngadaltë (< 10s)
   - 32 lojtarë → Shumë e ngadaltë (minutes+)

3. **Krahaso algoritmet:**
   - Provo të njëjtin rast me DFS dhe DLS
   - Shiko diferencën në `nodesExplored` dhe `timeMs`

4. **Përdor `find-max-weeks`:**
   - Për të gjetur limitin maksimal për një konfiguracion
   - Kjo është shumë e dobishme për të kuptuar problemin

---

## 🎯 Sekuenca e Rekomanduar e Testimit

1. ✅ **Test 1:** `example-simple` - Verifiko që gjithçka funksionon
2. ✅ **Test 2:** `dfs` me 8 lojtarë, 2 javë - Shoh rezultatin
3. ✅ **Test 3:** `dfs` me 12 lojtarë, 3 javë - Provo rast më të madh
4. ✅ **Test 4:** `depth-limited` me të njëjtat parametra - Krahasoni
5. ✅ **Test 5:** `find-max-weeks` me 16 lojtarë - Gjej limitin

---

## 📸 Screenshot-i i Pritur

Kur hapësh Swagger UI, duhet të shohësh:

```
📁 SocialGolfers
  ├─ GET /api/socialgolfers/dfs
  ├─ GET /api/socialgolfers/depth-limited
  ├─ GET /api/socialgolfers/find-max-weeks
  ├─ GET /api/socialgolfers/example-simple
  └─ GET /api/socialgolfers/example-original
```

Kliko në çdo endpoint për ta testuar!

---

**Gati për të testuar! 🚀**

Nëse ke pyetje, shiko dokumentacionin në `README_SOCIAL_GOLFERS.md` ose `SOCIAL_GOLFERS_DOKUMENTACIONI.md`.

