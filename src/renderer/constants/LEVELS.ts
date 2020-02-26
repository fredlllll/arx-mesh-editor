interface LevelData {
  dlf: string
  llf: string
  fts: string
}

const LEVELS: Record<string, LevelData> = {
  'City of Arx': {
    dlf: 'GRAPH/levels/level11/level11.DLF',
    llf: 'GRAPH/levels/level11/level11.LLF',
    fts: 'GAME/GRAPH/Levels/Level11/fast.fts'
  },
  'local dev': {
    dlf: 'level11.DLF',
    llf: 'level11.LLF',
    fts: 'level11.FTS'
  }
}

const NEW_LEVEL = 'new'

export { LEVELS, NEW_LEVEL }
