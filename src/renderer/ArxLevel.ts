/* global alert */

import path from 'path'
import { Object3D } from 'three'
import { has } from 'ramda'
import { DLFLoader } from './DLFLoader'
import { FTSLoader } from './FTSLoader'
import { LLFLoader } from './LLFLoader'

interface LevelData {
  resources: Array<string>
}

export const LEVELS: Record<string, LevelData> = {
  'City of Arx': {
    resources: [
      'GRAPH/levels/level11/level11.DLF',
      'GRAPH/levels/level11/level11.LLF',
      'GAME/GRAPH/Levels/Level11/fast.fts'
    ]
  }
}

const isDlf = (filename: string): boolean => filename.toLowerCase().endsWith('.dlf')
const isLlf = (filename: string): boolean => filename.toLowerCase().endsWith('.llf')
const isFts = (filename: string): boolean => filename.toLowerCase().endsWith('.fts')

export class ArxLevel extends Object3D {
  private levelName = ''

  public async load(arxRoot: string, levelName: string): Promise<ArxLevel> {
    // load everything, then set name variable so we dont accidentally think we loaded a level and overwrite it with an empty level

    if (!has(levelName, LEVELS)) {
      alert(`level does not exist: ${levelName}`) // TODO: we need proper error handling, not this shit
      return this
    }

    const loaders = LEVELS[levelName].resources.map(
      (res: string): Promise<any> => {
        let loader = null

        if (isDlf(res)) {
          loader = new DLFLoader()
        }

        if (isLlf(res)) {
          loader = new FTSLoader()
        }

        if (isFts(res)) {
          loader = new LLFLoader()
        }

        if (loader !== null) {
          return loader.load(path.resolve(arxRoot, res))
        } else {
          return Promise.reject(new Error(`unrecognisable file type: ${levelName}`))
        }
      }
    )

    await Promise.all(loaders)

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

export const NEW_LEVEL = 'new'
