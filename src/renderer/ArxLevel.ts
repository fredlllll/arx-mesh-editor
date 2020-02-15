import path from 'path'
import { Object3D } from 'three'
import { has } from 'ramda'
import { DLFLoader } from './DLF/DLFLoader'
import { FTSLoader } from './FTSLoader'
import { LLFLoader } from './LLFLoader'
import { LEVELS } from './constants/LEVELS'

export class ArxLevel extends Object3D {
  private levelName = ''

  public async load(arxRoot: string, levelName: string): Promise<ArxLevel> {
    // load everything, then set name variable so we dont accidentally think we loaded a level and overwrite it with an empty level

    if (!has(levelName, LEVELS)) {
      console.error(new Error(`level does not exist: ${levelName}`)) // TODO: we need proper error handling, not this shit
      return this
    }

    const levelData = LEVELS[levelName]

    const loaderPromises = [
      new DLFLoader().load(path.resolve(arxRoot, levelData.dlf)),
      new FTSLoader().load(path.resolve(arxRoot, levelData.fts)),
      new LLFLoader().load(path.resolve(arxRoot, levelData.llf))
    ]

    await Promise.all(loaderPromises)

    // TODO: work with results

    this.levelName = levelName

    return this
  }

  public async save(name?: string): Promise<ArxLevel> {
    if (name === undefined) {
      name = this.levelName
    }

    // TODO: save stuff

    return this
  }
}
