# [Project Name]

[Ek line mein: ye app kis problem ko hal karti hai aur kiske liye.]

## Features
- Dataset (CSV) load karke screen par preview dikhati hai
- User sawal likhta hai aur Claude dataset ke data se jawab deta hai
- [Event wale din ka core feature yahan likho]

## Tech Stack
- ASP.NET Core Web API (C#)
- Claude API (model: Claude Haiku 4.5)
- CsvHelper (CSV padhne ke liye)
- DotNetEnv (API key `.env` se load karne ke liye)
- HTML/JavaScript UI (`wwwroot/index.html`)

## Setup
1. Repo clone karo aur Visual Studio mein kholo.
2. Project ke root folder mein `.env` file banao aur usmein ye ek line likho:
```
   ANTHROPIC_API_KEY=your-api-key-here
```
   Key apni Anthropic Console se banao: https://console.anthropic.com
3. Dataset `data/dataset.csv` mein rakho.
4. F5 dabao, phir browser mein `http://localhost:5249/` kholo.
   (Port alag ho sakta hai, terminal mein "Now listening on" dekho.)

## Architecture
```
Browser (index.html)
   |  GET /api/dataset, POST /api/analyze
   v
Program.cs (API endpoints)
   |-- DatasetService: data/dataset.csv padhta hai
   |-- ClaudeService: Claude API ko prompt bhejta hai
   v
Claude API (api.anthropic.com)
```
- **Program.cs**: endpoints aur dependency injection
- **Services/DatasetService.cs**: CSV load karke prompt ke liye text banata hai
- **Services/ClaudeService.cs**: Claude Messages API ko HTTP call
- **wwwroot/index.html**: single-page UI

## How AI is used
- **Inside the app:** Claude ko dataset ke rows aur user ka sawal prompt mein bheja jata hai, aur usse kaha jata hai ke sirf dataset se jawab de. [Event ke prompt ke hisaab se detail likho.]
- **During development:** Claude (chat) se code likhwane aur debug karne mein madad li. [Kya kya banwaya, wo likho.]

## Limitations and Next Steps
- Bade datasets ke liye sirf pehle 200 rows bheji jati hain
- [Jo features waqt ki kami se reh gaye]