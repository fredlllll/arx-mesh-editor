import BinaryIO from '../Binary/BinaryIO'
import { SavedVector3 } from '../SavedVector3'
import { SavedColor } from '../SavedColor'

export class DanaeLsLight {
  pos: SavedVector3 = new SavedVector3(0, 0, 0)
  rgb: SavedColor = new SavedColor(0, 0, 0)
  fallstart = 0
  fallend = 0
  intensity = 0
  i = 0
  exFlicker: SavedColor = new SavedColor(0, 0, 0)
  exRadius = 0
  exFrequency = 0
  exSize = 0
  exSpeed = 0
  exFlareSize = 0
  fpadd = new Array<number>(24)
  extras = 0
  lpadd = new Array<number>(31)

  readFrom(binary: BinaryIO): void {
    this.pos = binary.readVector3(true)
    this.rgb = binary.readColor(true)
    this.fallstart = binary.readFloat32(true)
    this.fallend = binary.readFloat32(true)
    this.intensity = binary.readFloat32(true)
    this.i = binary.readFloat32(true)
    this.exFlicker = binary.readColor(true)
    this.exRadius = binary.readFloat32(true)
    this.exFrequency = binary.readFloat32(true)
    this.exSize = binary.readFloat32(true)
    this.exSpeed = binary.readFloat32(true)
    this.exFlareSize = binary.readFloat32(true)
    this.fpadd = binary.readFloat32Array(24, true)
    this.extras = binary.readInt32(true)
    this.lpadd = binary.readInt32Array(31, true)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeVector3(this.pos, true)
    binary.writeColor(this.rgb, true)
    binary.writeFloat32(this.fallstart, true)
    binary.writeFloat32(this.fallend, true)
    binary.writeFloat32(this.intensity, true)
    binary.writeFloat32(this.i, true)
    binary.writeColor(this.exFlicker, true)
    binary.writeFloat32(this.exRadius, true)
    binary.writeFloat32(this.exFrequency, true)
    binary.writeFloat32(this.exSize, true)
    binary.writeFloat32(this.exSpeed, true)
    binary.writeFloat32(this.exFlareSize, true)
    binary.writeFloat32Array(this.fpadd, true)
    binary.writeInt32(this.extras)
    binary.writeInt32Array(this.lpadd)
  }

  static SizeOf(): number {
    return 296 // done the math in my head going from this https://github.com/arx/ArxLibertatis/blob/85d293a69d486466e0c51de3ebf92f70941dc4f0/src/scene/LevelFormat.h#L114
  }
}
