= Specyfikacja zewnętrzna

== Wymagania sprzętowe
Do poprawnego działania systemu wymagane są:
- Serwer aplikacji: CPU 2-rdzeniowy, 4GB RAM, obsługa konteneru dla bazy danych w Docker oraz środowiska .NET 8.
- Klient: Dowolne urządzenie z przeglądarką internetową (Chrome, Firefox, Edge) obsługującą JavaScript i HTML5.

== Scenariusz 1: Generowanie raportu PDF (Funkcja dla Administratora)
Jest to kluczowa funkcjonalność wymagana w projekcie.
1. Użytkownik loguje się na konto z uprawnieniami Administratora.
2. Przechodzi do zakładki "Raporty".
3. Wybiera parametry raportu (np. zakres dat, miasto, zakres cenowy).
4. System przetwarza dane i generuje dokument PDF zawierający statystyki oraz listę atrakcji.
5. Następuje automatyczne pobranie pliku PDF na urządzenie użytkownika.

== Scenariusz 2: Wyszukiwanie i filtrowanie atrakcji
1. Użytkownik wchodzi na stronę główną (HomePage).
2. W pasku wyszukiwania wprowadza kryteria:
   - Nazwa miasta (np. "Kraków").
   - Typ atrakcji (np. "Event").
3. System dynamicznie odpytuje API.
4. Użytkownik otrzymuje listę kafelków ze zdjęciami i cenami spełniającymi kryteria.