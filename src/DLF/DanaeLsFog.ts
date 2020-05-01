import BinaryIO, { LITTLE_ENDIAN, BIG_ENDIAN, KEEP_ZERO_BYTES } from '../Binary/BinaryIO'
import { SavedVector3 } from '../SavedVector3'
import { SavedAnglef } from '../SavedAnglef'
import { SavedColor } from '../SavedColor'

export class DanaeLsFog {
  pos: SavedVector3 = new SavedVector3(0, 0, 0)
  rgb: SavedColor = new SavedColor(0, 0, 0)
  size = 0
  special = 0
  scale = 0
  move = new SavedVector3(0, 0, 0)
  angle = new SavedAnglef(0, 0, 0)
  speed = 0
  rotatespeed = 0
  tolive = 0
  blend = 0
  frequency = 0
  fpadd = new Array<number>(32)
  lpadd = new Array<number>(32)
  cpadd = '' // 256 bytes

  readFrom(binary: BinaryIO): void {
    this.pos = binary.readVector3(LITTLE_ENDIAN)
    this.rgb = binary.readColor(LITTLE_ENDIAN)
    this.size = binary.readFloat32(LITTLE_ENDIAN)
    this.special = binary.readInt32(LITTLE_ENDIAN)
    this.scale = binary.readFloat32(LITTLE_ENDIAN)
    this.move = binary.readVector3(LITTLE_ENDIAN)
    this.angle = binary.readAnglef(LITTLE_ENDIAN)
    this.speed = binary.readFloat32(LITTLE_ENDIAN)
    this.rotatespeed = binary.readFloat32(LITTLE_ENDIAN)
    this.tolive = binary.readInt32(LITTLE_ENDIAN)
    this.blend = binary.readInt32(LITTLE_ENDIAN)
    this.frequency = binary.readFloat32(LITTLE_ENDIAN)
    this.fpadd = binary.readFloat32Array(32, LITTLE_ENDIAN)
    this.lpadd = binary.readInt32Array(32, LITTLE_ENDIAN)
    this.cpadd = binary.readString(256, KEEP_ZERO_BYTES)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeVector3(this.pos, LITTLE_ENDIAN)
    binary.writeColor(this.rgb, LITTLE_ENDIAN)
    binary.writeFloat32(this.size, LITTLE_ENDIAN)
    binary.writeInt32(this.special, LITTLE_ENDIAN)
    binary.writeFloat32(this.scale, LITTLE_ENDIAN)
    binary.writeVector3(this.move, LITTLE_ENDIAN)
    binary.writeAnglef(this.angle, LITTLE_ENDIAN)
    binary.writeFloat32(this.speed, LITTLE_ENDIAN)
    binary.writeFloat32(this.rotatespeed, LITTLE_ENDIAN)
    binary.writeInt32(this.tolive, LITTLE_ENDIAN)
    binary.writeInt32(this.blend, LITTLE_ENDIAN)
    binary.writeFloat32(this.frequency, LITTLE_ENDIAN)
    binary.writeFloat32Array(this.fpadd, LITTLE_ENDIAN)
    binary.writeInt32Array(this.lpadd, BIG_ENDIAN)
    binary.writeString(this.cpadd, 256)
  }
}
