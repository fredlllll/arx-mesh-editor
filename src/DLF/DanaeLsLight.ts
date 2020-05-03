import BinaryIO, { LITTLE_ENDIAN } from '../Binary/BinaryIO'
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
    this.pos = binary.readVector3(LITTLE_ENDIAN)
    this.rgb = binary.readColor(LITTLE_ENDIAN)
    this.fallstart = binary.readFloat32(LITTLE_ENDIAN)
    this.fallend = binary.readFloat32(LITTLE_ENDIAN)
    this.intensity = binary.readFloat32(LITTLE_ENDIAN)
    this.i = binary.readFloat32(LITTLE_ENDIAN)
    this.exFlicker = binary.readColor(LITTLE_ENDIAN)
    this.exRadius = binary.readFloat32(LITTLE_ENDIAN)
    this.exFrequency = binary.readFloat32(LITTLE_ENDIAN)
    this.exSize = binary.readFloat32(LITTLE_ENDIAN)
    this.exSpeed = binary.readFloat32(LITTLE_ENDIAN)
    this.exFlareSize = binary.readFloat32(LITTLE_ENDIAN)
    this.fpadd = binary.readFloat32Array(24, LITTLE_ENDIAN)
    this.extras = binary.readInt32(LITTLE_ENDIAN)
    this.lpadd = binary.readInt32Array(31, LITTLE_ENDIAN)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeVector3(this.pos, LITTLE_ENDIAN)
    binary.writeColor(this.rgb, LITTLE_ENDIAN)
    binary.writeFloat32(this.fallstart, LITTLE_ENDIAN)
    binary.writeFloat32(this.fallend, LITTLE_ENDIAN)
    binary.writeFloat32(this.intensity, LITTLE_ENDIAN)
    binary.writeFloat32(this.i, LITTLE_ENDIAN)
    binary.writeColor(this.exFlicker, LITTLE_ENDIAN)
    binary.writeFloat32(this.exRadius, LITTLE_ENDIAN)
    binary.writeFloat32(this.exFrequency, LITTLE_ENDIAN)
    binary.writeFloat32(this.exSize, LITTLE_ENDIAN)
    binary.writeFloat32(this.exSpeed, LITTLE_ENDIAN)
    binary.writeFloat32(this.exFlareSize, LITTLE_ENDIAN)
    binary.writeFloat32Array(this.fpadd, LITTLE_ENDIAN)
    binary.writeInt32(this.extras, LITTLE_ENDIAN)
    binary.writeInt32Array(this.lpadd, LITTLE_ENDIAN)
  }

  static sizeOf(): number {
    return 296 // done the math in my head going from this https://github.com/arx/ArxLibertatis/blob/85d293a69d486466e0c51de3ebf92f70941dc4f0/src/scene/LevelFormat.h#L114
  }
}
