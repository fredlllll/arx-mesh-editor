import fs from 'fs'
import { BinaryIO } from '../BinaryIO'
import { checkCanRead } from '../helpers/file'
import { DanaeLsHeader } from './DanaeLsHeader'

export class DLFLoader {
  public async load(fileName: string): Promise<any> {
    await checkCanRead(fileName)
    const buffer = await fs.promises.readFile(fileName)
    const binary = new BinaryIO(buffer.buffer)

    // Step 1: read header information
    // https://github.com/arx/ArxLibertatis/blob/master/plugins/blender/arx_addon/dataDlf.py#L34

    const header = new DanaeLsHeader()
    header.readFrom(binary)

    const data = {
      header: header
    }

    console.log(data)

    return fileName // TODO actually do something
  }
}
