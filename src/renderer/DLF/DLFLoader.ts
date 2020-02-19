import fs from 'fs'
import { Readable, Writable } from 'stream'
import decodeImplode from 'implode-decoder'
import { BinaryIO } from '../Binary/BinaryIO'
import { checkCanRead } from '../helpers/file'
import { NOP } from '../helpers/function'
import { DanaeLsHeader } from './DanaeLsHeader'

const bufferToStream = (buffer: Buffer): Readable => {
  const readable = new Readable()
  readable.push(buffer)
  readable.push(null)
  return readable
}

/*
const streamToBuffer = (): Writable => {
  return new Promise(resolve => {
    const chunks: Array<Buffer> = []
    stream.on('data', (data: Buffer): void => {
      chunks.push(data)
    })
    stream.on('end', (): void => {
      resolve(Buffer.concat(chunks))
    })
  })
}
*/

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

export class DLFLoader {
  public async load(fileName: string): Promise<any> {
    await checkCanRead(fileName)
    const buffer = await fs.promises.readFile(fileName)
    const binary = new BinaryIO(buffer.buffer)

    // https://github.com/arx/ArxLibertatis/blob/master/plugins/blender/arx_addon/dataDlf.py#L34
    const header = new DanaeLsHeader()
    header.readFrom(binary)

    const headerSize = binary.position
    const rest = buffer.slice(headerSize)

    const readable = bufferToStream(rest)
    readable.pipe(decodeImplode())

    // TODO: test this with a custom transform, instead of decodeImplode
    const bw = new BufferWriter()
    bw.onFinish = (uncompressed: Buffer): void => {
      console.log(rest.byteLength, uncompressed.byteLength)
    }
    readable.pipe(bw)

    const data = {
      header: header
    }

    console.log(data)

    return fileName // TODO actually do something
  }
}
