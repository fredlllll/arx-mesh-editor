import fs from 'fs'
import { times } from 'ramda'
import BinaryIO from '../Binary/BinaryIO'
import { checkCanRead } from '../helpers/file'
import { decompress } from '../helpers/compression'
import { DanaeLsHeader } from './DanaeLsHeader'
import { DanaeLsScene } from './DanaeLsScene'
import { DanaeLsInter } from './DanaeLsInter'
import { DanaeLsLight } from './DanaeLsLight'
import { DanaeLsFog } from './DanaeLsFog'
import { DanaeLsPath } from './DanaeLsPath'

export default class DLF {
  header: DanaeLsHeader
  scene: DanaeLsScene
  inters: DanaeLsInter[]
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
    const headerSize = binary.position

    if (header.nbScn > 0) {
      this.scene = new DanaeLsScene()
      this.scene.readFrom(binary)
    }

    const remainder = await decompress(buffer.slice(headerSize))
    const body = new BinaryIO(remainder.buffer)

    this.inters = times(() => {
      const inter = new DanaeLsInter()
      inter.readFrom(body)
      return inter
    }, header.nbInter)

    if (header.lighting > 0) {
      // TODO: load lighting
    }

    const nbLights = header.version < 1.003 ? 0 : header.nbLights

    const lightingFile = true // does a lighting file (llf) exist?
    if (!lightingFile) {
      // load lights from dlf
      // loadLights(dat, pos, nb_lights);
    } else {
      // skip lights in dlf
      const sizeofDanaeLsLight = DanaeLsLight.SizeOf()
      body.readInt8Array(sizeofDanaeLsLight * nbLights)
    }

    this.fogs = times(() => {
      const fog = new DanaeLsFog()
      fog.readFrom(body)
      return fog
    }, header.nbFogs)

    // skip nodes for newer versions
    if (header.version >= 1.001) {
      body.readInt8Array(header.nbNodes * (204 + header.nbNodeslinks * 64))
    }

    this.paths = times(() => {
      const path = new DanaeLsPath()
      path.readFrom(body)
      return path
    }, header.nbPaths)
  }

  public async save(): Promise<any> {
    return Promise.resolve()
  }
}
