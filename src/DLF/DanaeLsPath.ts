import BinaryIO from '../Binary/BinaryIO'
import { SavedVector3 } from '../SavedVector3'
import { SavedColor } from '../SavedColor'

export class DanaeLsPath {
  name = ''
  idx = 0
  flags = 0
  initpos = new SavedVector3(0, 0, 0)
  pos = new SavedVector3(0, 0, 0)
  nbPathways = 0
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
    this.name = binary.readString(64, true)
    this.idx = binary.readInt16(true)
    this.flags = binary.readInt16(true)
    this.initpos = binary.readVector3(true)
    this.pos = binary.readVector3(true)
    this.nbPathways = binary.readInt32(true)
    this.rgb = binary.readColor(true)
    this.farclip = binary.readFloat32(true)
    this.reverb = binary.readFloat32(true)
    this.ambMaxVol = binary.readFloat32(true)
    this.fpadd = binary.readFloat32Array(26, true)
    this.height = binary.readInt32(true)
    this.lpadd = binary.readInt32Array(31, true)
    this.ambiance = binary.readString(128, true)
    this.cpadd = binary.readString(128, false)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeString(this.name, 64)
    binary.writeInt16(this.idx, true)
    binary.writeInt16(this.flags, true)
    binary.writeVector3(this.initpos, true)
    binary.writeVector3(this.pos, true)
    binary.writeInt32(this.nbPathways, true)
    binary.writeColor(this.rgb, true)
    binary.writeFloat32(this.farclip, true)
    binary.writeFloat32(this.reverb, true)
    binary.writeFloat32(this.ambMaxVol, true)
    binary.writeFloat32Array(this.fpadd)
    binary.writeInt32(this.height, true)
    binary.writeInt32Array(this.lpadd, true)
    binary.writeString(this.ambiance, 128)
    binary.writeString(this.cpadd, 128)
  }
}
