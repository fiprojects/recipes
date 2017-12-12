## Struktura projektu
* Crawler - stahování dat z Allrecipes
* Graphs/Graphs_Pictures - výstupní vizualizace dat pro prezentaci
* Recipes - jádro systému
* RecipesConsole - konsolová aplikace pro ovládání off-line komponent (Crawler, předvýpočet TD-IDF)
* RecipesGraphs - generování dat pro grafy
* RecipesTest - jednotkové testy pro některé komponenty
* RecipesWeb - webové rozhraní

## Implementace algoritmů
* /RecipesWeb/Controllers/RecipesController.cs - MVC controller pro výpis detailu receptu, zde se aplikují algoritmy

### Random
* /RecipesWeb/Controllers/RecipesController.cs - přímá implementace

### Doporučení na základě ingrediencí
* /Recipes/Services/RecipesService.cs - především metoda GetRecommendedByIngredience

### TF-IDF
* /Recipes/Processors/StopWords.cs - filtr stop words
* /Recipes/Processors/TfIdfComputer.cs - vlastní algoritmus výpočtu TF-IDF
* /Recipes/Services/TfIdfService.cs - výpočet doporučení a podobnosti na základě TF-IDF

### Critiquing
* /Recipes/Critiquing - definice otázek, odpovědí a evaluátorů odpovědí
* /Recipes/Services/CritiquingService.cs - výpočet doporučení na základě vah

## Rozdělení rolí
Veronika - analýza a vizualizace dat, ruční korekce ingrediencí, evaluace
Patrik - náhodný výběr a doporučení na základě ingrediencí, kategorizace ingrediencí
Jozef - TF-IDF, filtrace slov
Michael - critiquing, crawling
