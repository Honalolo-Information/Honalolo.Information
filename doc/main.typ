#import "utils.typ": *

// --- KONFIGURACJA DOKUMENTU ---
#let project_title = "System Informacji Turystycznej - Honalolo"
#let authors = ("Marcel Lis", "Piotr Juszkiewicz", "Karol Rozmus", "Jan Lelko", "Adam Haber")
#let supervisor = "dr hab. inż. prof. PŚ Bożena Małysiak-Mrozek"
#let group_id = "[Nr Sekcji]"

#set document(title: project_title, author: authors)
#set text(font: "Linux Libertine", lang: "pl")
#set page(
  paper: "a4",
  margin: (x: 2.5cm, y: 2.5cm),
  numbering: "1",
)

// --- STRONA TYTUŁOWA ---
#align(center + horizon)[
  #text(2em, weight: "bold")[Raport Końcowy Projektu z przedmiotu Bazy Danych]
  
  #v(2cm)
  #text(1.5em)[#project_title]
  
  
  #v(4cm)
  *Autorzy:* \
  #for author in authors [
    #author \
  ]
  
  #v(2cm)
  *Prowadzący:* \
  #supervisor
  
  #v(1fr)
  #datetime.today().display("[day]-[month]-[year]")
]

#pagebreak()
#outline(indent: auto)
#pagebreak()

// --- ZAWARTOŚĆ ---
#include "chapters/01_wstep.typ"
#include "chapters/02_technologie.typ"
#include "chapters/03_spec_zewnetrzna.typ"
#include "chapters/04_spec_wewnetrzna.typ"
#include "chapters/05_podsumowanie.typ"