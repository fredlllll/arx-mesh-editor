import BinaryIO from '../Binary/BinaryIO'
import { SavedVector3 } from '../SavedVector3'
import { SavedAnglef } from '../SavedAnglef'

export class DanaeLsInter {
  name = ''
  pos: SavedVector3 = new SavedVector3(0, 0, 0)
  angle: SavedAnglef = new SavedAnglef(0, 0, 0)
  ident: number
  flags: number
  pad: number[] = new Array<number>(14)
  fpad: number[] = new Array<number>(16)

  readFrom(binary: BinaryIO): void {
    this.name = binary.readString(512, true)
    this.pos = binary.readVector3(true)
    this.angle = binary.readAnglef(true)
    this.ident = binary.readInt32(true) // could also be a 4 byte string?
    this.flags = binary.readInt32(true)
    this.pad = binary.readInt32Array(14, true)
    this.fpad = binary.readFloat32Array(16, true)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeString(this.name, 512)
    binary.writeVector3(this.pos, true)
    binary.writeAnglef(this.angle, true)
    binary.writeInt32(this.ident)
    binary.writeInt32(this.flags)
    binary.writeInt32Array(this.pad)
    binary.writeFloat32Array(this.fpad)
  }
}
