#let code_block(content) = block(
  fill: luma(240),
  inset: 10pt,
  radius: 4pt,
  width: 100%,
  text(font: "Consolas", size: 0.8em, content)
)

#let section_header(title) = {
  v(1em)
  text(1.3em, weight: "bold", fill: rgb("#003366"))[#title]
  v(0.5em)
}