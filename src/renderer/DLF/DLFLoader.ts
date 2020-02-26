import fs from 'fs'
import { Readable, Writable, Transform } from 'stream'
// import decodeImplode from 'implode-decoder'
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

class TestTransform extends Transform {
  _transform(chunk: any, _encoding: string, next: Function): void {
    const bytes = []

    for (let i = 0; i < chunk.length; i++) {
      bytes.push(chunk[i], 0)
    }

    const chunk1 = Buffer.from(bytes.slice(0, bytes.length / 2))
    const chunk2 = Buffer.from(bytes.slice(bytes.length / 2 + 1, bytes.length + 1))

    console.log(chunk1)
    console.log(chunk2)

    this.push(chunk1)
    this.push(chunk2)

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
    // readable.pipe(decodeImplode())

    const testTransform = new TestTransform()
    readable.pipe(testTransform)

    // TODO: test this with a custom transform, instead of decodeImplode
    const bw = new BufferWriter()
    bw.onFinish = (uncompressed: Buffer): void => {
      console.log(uncompressed)
    }
    readable.pipe(bw)

    readable.on('end', () => {
      console.log('--- readable ended')
    })

    readable.on('finish', () => {
      console.log('--- writable finished')
    })

    const data = {
      header: header
    }

    console.log(Object.keys(data))

    return fileName // TODO actually do something
  }
}
