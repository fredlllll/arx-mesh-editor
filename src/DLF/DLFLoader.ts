import fs from 'fs'
import { Readable, Writable } from 'stream'
import decodeImplode from 'implode-decoder'
import BinaryIO from '../Binary/BinaryIO'
import { checkCanRead } from '../helpers/file'
import { NOP } from '../helpers/function'
import { DanaeLsHeader } from './DanaeLsHeader'

const bufferToStream = (buffer: Buffer): Readable => {
  const readable = new Readable()
  readable.push(buffer)
  readable.push(null)
  return readable
}

class BufferWriter extends Writable {
  chunks: Buffer[] = []
  onFinish: Function = NOP
  _write(chunk: any, _encoding: string, next: Function): void {
    this.chunks.push(chunk)
    next()
  }

  _final(next: Function): void {
    this.onFinish(Buffer.concat(this.chunks))
    next()
  }
}

export default class DLFLoader {
  public async load(fileName: string): Promise<any> {
    await checkCanRead(fileName)
    const buffer = await fs.promises.readFile(fileName)
    const binary = new BinaryIO(buffer.buffer)

    // https://github.com/arx/ArxLibertatis/blob/master/plugins/blender/arx_addon/dataDlf.py#L34
    const header = new DanaeLsHeader()
    header.readFrom(binary)

    const headerSize = binary.position
    const rest = buffer.slice(headerSize)

    return new Promise((resolve: Function) => {
      const bufferWriter = new BufferWriter()
      bufferWriter.onFinish = (data: Buffer): void => {
        resolve({
          header: header,
          body: data
        })
      }

      const readable = bufferToStream(rest)
      readable.pipe(decodeImplode()).pipe(bufferWriter)
    })
  }
}
