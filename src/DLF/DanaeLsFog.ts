import BinaryIO from '../Binary/BinaryIO'
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
    this.pos = binary.readVector3(true)
    this.rgb = binary.readColor(true)
    this.size = binary.readFloat32(true)
    this.special = binary.readInt32(true)
    this.scale = binary.readFloat32(true)
    this.move = binary.readVector3(true)
    this.angle = binary.readAnglef(true)
    this.speed = binary.readFloat32(true)
    this.rotatespeed = binary.readFloat32(true)
    this.tolive = binary.readInt32(true)
    this.blend = binary.readInt32(true)
    this.frequency = binary.readFloat32(true)
    this.fpadd = binary.readFloat32Array(32, true)
    this.lpadd = binary.readInt32Array(32, true)
    this.cpadd = binary.readString(256)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeVector3(this.pos, true)
    binary.writeColor(this.rgb, true)
    binary.writeFloat32(this.size, true)
    binary.writeInt32(this.special, true)
    binary.writeFloat32(this.scale, true)
    binary.writeVector3(this.move, true)
    binary.writeAnglef(this.angle, true)
    binary.writeFloat32(this.speed, true)
    binary.writeFloat32(this.rotatespeed, true)
    binary.writeInt32(this.tolive, true)
    binary.writeInt32(this.blend, true)
    binary.writeFloat32(this.frequency, true)
    binary.writeFloat32Array(this.fpadd, true)
    binary.writeInt32Array(this.lpadd)
    binary.writeString(this.cpadd, 256)
  }
}
