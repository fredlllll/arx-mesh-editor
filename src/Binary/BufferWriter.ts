import { Writable } from 'stream'
import { NOP } from '../helpers/function'

export class BufferWriter extends Writable {
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
