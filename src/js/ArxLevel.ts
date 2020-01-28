import { Object3D } from 'three'
import { DLFLoader } from './DLFLoader'
import { FTSLoader } from './FTSLoader'
import { LLFLoader } from './LLFLoader'

export class ArxLevel extends Object3D {
  private levelName = ''

  public async load(levelName: string): Promise<ArxLevel> {
    // load everything, then set name variable so we dont accidentally think we loaded a level and overwrite it with an empty level

    const dlfLoader = new DLFLoader()
    const ftsLoader = new FTSLoader()
    const llfLoader = new LLFLoader()

    await Promise.all([
      dlfLoader.load(`data/${levelName}.DLF`),
      ftsLoader.load(`data/${levelName}.FTS`),
      llfLoader.load(`data/${levelName}.LLF`)
    ])

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
