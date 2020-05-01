import BinaryIO, { TRUNCATE_ZERO_BYTES, LITTLE_ENDIAN, BIG_ENDIAN } from '../Binary/BinaryIO'
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
    this.name = binary.readString(512, TRUNCATE_ZERO_BYTES)
    this.pos = binary.readVector3(LITTLE_ENDIAN)
    this.angle = binary.readAnglef(LITTLE_ENDIAN)
    this.ident = binary.readInt32(LITTLE_ENDIAN) // could also be a 4 byte string?
    this.flags = binary.readInt32(LITTLE_ENDIAN)
    this.pad = binary.readInt32Array(14, LITTLE_ENDIAN)
    this.fpad = binary.readFloat32Array(16, LITTLE_ENDIAN)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeString(this.name, 512)
    binary.writeVector3(this.pos, LITTLE_ENDIAN)
    binary.writeAnglef(this.angle, LITTLE_ENDIAN)
    binary.writeInt32(this.ident, BIG_ENDIAN)
    binary.writeInt32(this.flags, BIG_ENDIAN)
    binary.writeInt32Array(this.pad, BIG_ENDIAN)
    binary.writeFloat32Array(this.fpad, BIG_ENDIAN)
  }
}
