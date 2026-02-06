= Specyfikacja wewnętrzna

== Architektura systemu
System zrealizowano w architekturze warstwowej (Clean Architecture/Onion Architecture):
1. *Domain:* Zawiera encje (np. `Attraction`, `User`, `Report`) i interfejsy repozytoriów. Jest niezależna od innych warstw.
2. *Application:* Zawiera logikę biznesową, serwisy (`AttractionService`, `PdfService`) oraz obiekty DTO.
3. *Infrastructure:* Implementacja dostępu do danych (EF Core, `TouristInfoDbContext`) oraz serwisów zewnętrznych.
4. *WebAPI:* Kontrolery REST API (`AttractionsController`) udostępniające punkty końcowe dla frontendu.

== Analiza implementacji wybranych funkcjonalności

=== Funkcjonalność 1: Dodawanie nowej atrakcji (Polimorfizm)
Proces tworzenia atrakcji jest obsługiwany przez `AttractionService.CreateAsync`. Ze względu na różne typy atrakcji (Hotel, Szlak, Wydarzenie), system stosuje logikę warunkową do budowania odpowiednich obiektów.

*Działanie od wewnątrz:*
1. Kontroler odbiera obiekt `CreateAttractionDto`.
2. Serwis najpierw rozwiązuje powiązania geograficzne (Kraj -> Region -> Miasto) metodą `GetOrCreateLocationChainAsync`, aby uniknąć duplikacji w bazie.
3. Następnie, na podstawie pola `TypeName`, tworzony jest obiekt bazowy `Attraction`.
4. System dołącza szczegółowe dane w zależności od typu (Pattern Factory/Builder wewnątrz switcha):
   - Dla *Event*: dodawane są daty rozpoczęcia/zakończenia.
   - Dla *Trail*: dodawany jest dystans i poziom trudności.
   - Dla *Hotel*: dodawana jest lista udogodnień (zapisywana jako JSON).
5. Całość jest zapisywana w jednej transakcji do bazy danych.

Fragment kodu odpowiedzialny za logikę typów (`AttractionService.cs`):
```csharp
switch (attractionType.TypeName)
{
    case "Event":
        attraction.EventDetails = new Event { ... };
        break;
    case "Trail":
        attraction.TrailDetails = new Trail { ... };
        break;
    // ...
}
```