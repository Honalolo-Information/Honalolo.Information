= Wybór systemu i technologie

== Dlaczego system webowy?
Wybrano architekturę aplikacji internetowej (Web App) ze względu na:
- *Dostępność:* Użytkownicy mogą korzystać z systemu na dowolnym urządzeniu z przeglądarką (komputer, tablet, smartfon) bez konieczności instalacji.
- *Centralizacja danych:* Jedna baza danych aktualizowana w czasie rzeczywistym dla wszystkich użytkowników.
- *Łatwość aktualizacji:* Zmiany w logice biznesowej lub interfejsie są widoczne natychmiast po wdrożeniu na serwer.

== Zastosowane narzędzia i technologie
W projekcie wykorzystano nowoczesny stos technologiczny, podzielony na warstwę frontendową i backendową:

*Frontend (Warstwa Prezentacji):*
- *React 19:* Biblioteka do budowania interfejsów użytkownika.
- *Vite:* Narzędzie do budowania i serwowania aplikacji (HMR).
- *TailwindCSS v4:* Framework CSS do stylizacji (Utility-first).
- *Feather Icons:* Zestaw ikon wektorowych.

*Backend (Warstwa Logiki i Danych):*
- *.NET 8 (C\#):* Platforma wykonawcza po stronie serwera.
- *Entity Framework Core:* System ORM do komunikacji z bazą danych.
- *QuestPDF:* Biblioteka do generowania raportów PDF (zgodnie z wymaganiami).
- *MySQL / MariaDB:* Relacyjna baza danych (zależnie od konfiguracji Docker).
- *Docker:* Konteneryzacja środowiska uruchomieniowego bazy danych.