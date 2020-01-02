const loadFile = filename => {
  return fetch(filename)
    .then(res => res.arrayBuffer())
}

const parseDLF = raw => {
  return raw
}
const parseFTS = raw => {
  return raw
}
const parseLLF = raw => {
  return raw
}

document.addEventListener("DOMContentLoaded", () => {
  Promise.all([
    loadFile('data/level11.DLF').then(parseDLF),
    loadFile('data/level11.FTS').then(parseFTS),
    loadFile('data/level11.LLF').then(parseLLF)
  ]).then(([dlf, fts, llf]) => {
    console.log(dlf, fts, llf)
  })
})
