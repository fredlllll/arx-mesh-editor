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
    const file = new BinaryIO(buffer.buffer)

    this.readHeader(file)
    this.readScene(file)

    const remainder = await decompress(buffer.slice(DanaeLsHeader.sizeOf()))
    const body = new BinaryIO(remainder.buffer)

    this.interactiveObjects = times(() => {
      const inter = new DanaeLsInteractiveObject()
      inter.readFrom(body)
      return inter
    }, this.header.numberOfInteractiveObjects)

    if (this.header.lighting > 0) {
      // TODO: load lighting
    }

    const numberOfLights = this.header.version < 1.003 ? 0 : this.header.numberOfLights

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
    }, this.header.numberOfFogs)

    // skip nodes for newer versions
    if (this.header.version >= 1.001) {
      body.readInt8Array(this.header.numberOfNodes * (204 + this.header.numberOfNodeLinks * 64))
    }

    this.paths = times(() => {
      const path = new DanaeLsPath()
      path.readFrom(body)
      return path
    }, this.header.numberOfPaths)
  }

  public async save(): Promise<any> {
    return Promise.resolve()
  }

  private readHeader(file: BinaryIO): void {
    const header = new DanaeLsHeader()
    header.readFrom(file)
    this.header = header
  }

  private readScene(file: BinaryIO): void {
    if (this.header.numberOfScenes !== 0) {
      this.scene = new DanaeLsScene()
      this.scene.readFrom(file)
    }
  }
}
