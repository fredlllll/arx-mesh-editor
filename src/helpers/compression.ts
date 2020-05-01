import decodeImplode from 'implode-decoder'
import { BufferWriter } from '../Binary/BufferWriter'
import { bufferToStream } from './streams'

export async function decompress(buffer: Buffer): Promise<Buffer> {
  return new Promise<Buffer>((resolve) => {
    const bufferWriter = new BufferWriter()
    bufferWriter.onFinish = (data: Buffer): void => {
      resolve(data)
    }

    const readable = bufferToStream(buffer)
    readable.pipe(decodeImplode()).pipe(bufferWriter)
  })
}
