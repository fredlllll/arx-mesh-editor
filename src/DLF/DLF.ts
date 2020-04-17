import fs from 'fs'
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
  inter: DanaeLsInter
  light: DanaeLsLight
  fog: DanaeLsFog
  path: DanaeLsPath

  public async load(fileName: string): Promise<any> {
    await checkCanRead(fileName)
    const buffer = await fs.promises.readFile(fileName)
    let binary = new BinaryIO(buffer.buffer)

    // https://github.com/arx/ArxLibertatis/blob/master/plugins/blender/arx_addon/dataDlf.py#L34
    const header = new DanaeLsHeader()
    this.header = header
    header.readFrom(binary)
    const headerSize = binary.position

    if (header.nbScn > 0) {
      this.scene = new DanaeLsScene()
      this.scene.readFrom(binary)
      // TODO do something with scene
    }

    const remainder = await decompress(buffer.slice(headerSize))
    binary = new BinaryIO(remainder.buffer)

    for (let i = 0; i < header.nbInter; i++) {
      this.inter = new DanaeLsInter()
      this.inter.readFrom(binary)
      // TODO: do somethign with inter
    }

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
      binary.readInt8Array(sizeofDanaeLsLight * nbLights)
    }

    // fog
    for (let i = 0; i < header.nbFogs; i++) {
      this.fog = new DanaeLsFog()
      this.fog.readFrom(binary)
      // TODO: do something with fog
    }

    // skip nodes for newer versions
    if (header.version >= 1.001) {
      binary.readInt8Array(header.nbNodes * (204 + header.nbNodeslinks * 64))
    }

    // paths
    for (let i = 0; i < header.nbPaths; i++) {
      this.path = new DanaeLsPath()
      this.path.readFrom(binary)
      // TODO: do something with path
    }
  }

  public async save(): Promise<any> {
    return Promise.resolve()
  }
}
