import BinaryIO, { TRUNCATE_ZERO_BYTES, KEEP_ZERO_BYTES, LITTLE_ENDIAN } from '../Binary/BinaryIO'
import { SavedVector3 } from '../SavedVector3'
import { SavedColor } from '../SavedColor'

export class DanaeLsPath {
  name = ''
  idx = 0
  flags = 0
  initpos = new SavedVector3(0, 0, 0)
  pos = new SavedVector3(0, 0, 0)
  numberOfPathways = 0
  rgb = new SavedColor(0, 0, 0)
  farclip = 0
  reverb = 0
  ambMaxVol = 0
  fpadd = new Array<number>(26)
  height = 0
  lpadd = new Array<number>(31)
  ambiance = '' // 128 chars
  cpadd = '' // 128 bytes

  readFrom(binary: BinaryIO): void {
    this.name = binary.readString(64, TRUNCATE_ZERO_BYTES)
    this.idx = binary.readInt16(LITTLE_ENDIAN)
    this.flags = binary.readInt16(LITTLE_ENDIAN)
    this.initpos = binary.readVector3(LITTLE_ENDIAN)
    this.pos = binary.readVector3(LITTLE_ENDIAN)
    this.numberOfPathways = binary.readInt32(LITTLE_ENDIAN)
    this.rgb = binary.readColor(LITTLE_ENDIAN)
    this.farclip = binary.readFloat32(LITTLE_ENDIAN)
    this.reverb = binary.readFloat32(LITTLE_ENDIAN)
    this.ambMaxVol = binary.readFloat32(LITTLE_ENDIAN)
    this.fpadd = binary.readFloat32Array(26, LITTLE_ENDIAN)
    this.height = binary.readInt32(LITTLE_ENDIAN)
    this.lpadd = binary.readInt32Array(31, LITTLE_ENDIAN)
    this.ambiance = binary.readString(128, TRUNCATE_ZERO_BYTES)
    this.cpadd = binary.readString(128, KEEP_ZERO_BYTES)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeString(this.name, 64)
    binary.writeInt16(this.idx, LITTLE_ENDIAN)
    binary.writeInt16(this.flags, LITTLE_ENDIAN)
    binary.writeVector3(this.initpos, LITTLE_ENDIAN)
    binary.writeVector3(this.pos, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfPathways, LITTLE_ENDIAN)
    binary.writeColor(this.rgb, LITTLE_ENDIAN)
    binary.writeFloat32(this.farclip, LITTLE_ENDIAN)
    binary.writeFloat32(this.reverb, LITTLE_ENDIAN)
    binary.writeFloat32(this.ambMaxVol, LITTLE_ENDIAN)
    binary.writeFloat32Array(this.fpadd, LITTLE_ENDIAN)
    binary.writeInt32(this.height, LITTLE_ENDIAN)
    binary.writeInt32Array(this.lpadd, LITTLE_ENDIAN)
    binary.writeString(this.ambiance, 128)
    binary.writeString(this.cpadd, 128)
  }
}
