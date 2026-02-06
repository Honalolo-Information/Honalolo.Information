= Ogólne założenia projektu

Celem projektu jest stworzenie systemu informacji turystycznej "Honalolo", umożliwiającego gromadzenie, przeglądanie oraz zarządzanie bazą atrakcji turystycznych. System ma charakter platformy webowej, obsługującej różne typy obiektów, takie jak hotele, restauracje, szlaki czy wydarzenia kulturalne.

Główne funkcjonalności systemu to:
- Przeglądanie i filtrowanie atrakcji przez turystów.
- Rejestracja i logowanie użytkowników.
- Panel administracyjny do zarządzania treścią i generowania raportów PDF.
- Obsługa multimediów (galerie zdjęć atrakcji).

== Dołączone materiały z poprzednich etapów
Zgodnie z wymaganiami, do projektu dołączono (jako załączniki):
- Hierarchię ról (Administrator, Zarejestrowany Użytkownik, Gość).
#figure(
  image("../images/users_roles.png", width: 90%),
  caption: [Hierarchia ról w systemie],
) <rys_erd>

- Model logiczny i fizyczny bazy danych (diagramy ERD).
#figure(
  image("../images/erd_diagram.png", width: 90%),
  caption: [Diagram relacji encji (ERD)],
) <rys_erd>

- Szkice interfejsu użytkownika (widoki React).
#figure(
  image("../images/register.png", width: 90%),
  caption: [Szkic interfejsu rejestracji użytkownika],
) <rys_erd>

#figure(
  image("../images/login.png", width: 90%),
  caption: [Szkic interfejsu logowania użytkownika],
) <rys_erd>

#figure(
  image("../images/main_view.png", width: 90%),
  caption: [Widok główny z listą atrakcji i filtrami],
) <rys_erd>

#figure(
  image("../images/single_attraction.png", width: 90%),
  caption: [Widok szczegółowy pojedynczej atrakcji],
) <rys_erd>