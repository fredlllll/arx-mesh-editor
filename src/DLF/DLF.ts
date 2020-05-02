import fs from 'fs'
import { times } from 'ramda'
import BinaryIO from '../Binary/BinaryIO'
import { checkCanRead } from '../helpers/file'
import { decompress } from '../helpers/compression'
import { DanaeLsHeader } from './DanaeLsHeader'
import { DanaeLsScene } from './DanaeLsScene'
import { DanaeLsInteractiveObject } from './DanaeLsInteactiveObject'
import { DanaeLsLight } from './DanaeLsLight'
import { DanaeLsFog } from './DanaeLsFog'
import { DanaeLsPath } from './DanaeLsPath'

export default class DLF {
  header: DanaeLsHeader
  scene: DanaeLsScene
  interactiveObjects: DanaeLsInteractiveObject[]
  light: DanaeLsLight
  fogs: DanaeLsFog[]
  paths: DanaeLsPath[]

  public async load(fileName: string): Promise<any> {
    await checkCanRead(fileName)
    const buffer = await fs.promises.readFile(fileName)
    const binary = new BinaryIO(buffer.buffer)

    // https://github.com/arx/ArxLibertatis/blob/master/plugins/blender/arx_addon/dataDlf.py#L34
    const header = new DanaeLsHeader()
    header.readFrom(binary)
    this.header = header

    if (header.numberOfScenes !== 0) {
      this.scene = new DanaeLsScene()
      this.scene.readFrom(binary)
    }

    const remainder = await decompress(buffer.slice(DanaeLsHeader.sizeOf()))
    const body = new BinaryIO(remainder.buffer)

    this.interactiveObjects = times(() => {
      const inter = new DanaeLsInteractiveObject()
      inter.readFrom(body)
      return inter
    }, header.numberOfInteractiveObjects)

    if (header.lighting > 0) {
      // TODO: load lighting
    }

    const numberOfLights = header.version < 1.003 ? 0 : header.numberOfLights

    const lightingFile = true // does a lighting file (llf) exist?
    if (!lightingFile) {
      // load lights from dlf
      // loadLights(dat, pos, numberOf_lights);
    } else {
      // skip lights in dlf
      const sizeofDanaeLsLight = DanaeLsLight.sizeOf()
      body.readInt8Array(sizeofDanaeLsLight * numberOfLights)
    }

    this.fogs = times(() => {
      const fog = new DanaeLsFog()
      fog.readFrom(body)
      return fog
    }, header.numberOfFogs)

    // skip nodes for newer versions
    if (header.version >= 1.001) {
      body.readInt8Array(header.numberOfNodes * (204 + header.numberOfNodeLinks * 64))
    }

    this.paths = times(() => {
      const path = new DanaeLsPath()
      path.readFrom(body)
      return path
    }, header.numberOfPaths)
  }

  public async save(): Promise<any> {
    return Promise.resolve()
  }
}
