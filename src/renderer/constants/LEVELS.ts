interface LevelData {
  resources: Array<string>
}

const LEVELS: Record<string, LevelData> = {
  'City of Arx': {
    resources: [
      'GRAPH/levels/level11/level11.DLF',
      'GRAPH/levels/level11/level11.LLF',
      'GAME/GRAPH/Levels/Level11/fast.fts'
    ]
  }
}

const NEW_LEVEL = 'new'

export { LEVELS, NEW_LEVEL }
