import BinaryIO, { LITTLE_ENDIAN, TRUNCATE_ZERO_BYTES, KEEP_ZERO_BYTES } from '../Binary/BinaryIO'
import { SavedVector3 } from '../SavedVector3'
import { SavedAnglef } from '../SavedAnglef'

export class DanaeLsHeader {
  // TODO: should we just ignore the contents of padding fields?

  version = 0
  ident = ''
  lastuser = ''
  time = 0
  posEdit: SavedVector3 = new SavedVector3(0, 0, 0)
  angleEdit: SavedAnglef = new SavedAnglef(0, 0, 0)
  nbScn = 0
  nbInter = 0
  nbNodes = 0
  nbNodeslinks = 0
  nbZones = 0
  lighting = 0
  Bpad: number[] = new Array<number>(256)
  nbLights = 0
  nbFogs = 0
  nbBkgpolys = 0
  nbIgnoredpolys = 0
  nbChildpolys = 0
  nbPaths = 0
  pad: number[] = new Array<number>(250)
  offset: SavedVector3 = new SavedVector3(0, 0, 0)
  fpad: number[] = new Array<number>(253)
  cpad = ''
  bpad: number[] = new Array<number>(256)

  readFrom(binary: BinaryIO): void {
    this.version = binary.readFloat32(LITTLE_ENDIAN)
    this.ident = binary.readString(16, TRUNCATE_ZERO_BYTES)
    this.lastuser = binary.readString(256, TRUNCATE_ZERO_BYTES)
    this.time = binary.readInt32(LITTLE_ENDIAN)
    this.posEdit = binary.readVector3(LITTLE_ENDIAN)
    this.angleEdit = binary.readAnglef(LITTLE_ENDIAN)
    this.nbScn = binary.readInt32(LITTLE_ENDIAN)
    this.nbInter = binary.readInt32(LITTLE_ENDIAN)
    this.nbNodes = binary.readInt32(LITTLE_ENDIAN)
    this.nbNodeslinks = binary.readInt32(LITTLE_ENDIAN)
    this.nbZones = binary.readInt32(LITTLE_ENDIAN)
    this.lighting = binary.readInt32(LITTLE_ENDIAN)
    this.Bpad = binary.readInt32Array(256, LITTLE_ENDIAN)
    this.nbLights = binary.readInt32(LITTLE_ENDIAN)
    this.nbFogs = binary.readInt32(LITTLE_ENDIAN)
    this.nbBkgpolys = binary.readInt32(LITTLE_ENDIAN)
    this.nbIgnoredpolys = binary.readInt32(LITTLE_ENDIAN)
    this.nbChildpolys = binary.readInt32(LITTLE_ENDIAN)
    this.nbPaths = binary.readInt32(LITTLE_ENDIAN)
    this.pad = binary.readInt32Array(250, LITTLE_ENDIAN)
    this.offset = binary.readVector3(LITTLE_ENDIAN)
    this.fpad = binary.readFloat32Array(253, LITTLE_ENDIAN)
    this.cpad = binary.readString(4096, KEEP_ZERO_BYTES)
    this.bpad = binary.readInt32Array(256, LITTLE_ENDIAN)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeFloat32(this.version, LITTLE_ENDIAN)
    binary.writeString(this.ident, 16)
    binary.writeString(this.lastuser, 256)
    binary.writeInt32(this.time, LITTLE_ENDIAN)
    binary.writeVector3(this.posEdit, LITTLE_ENDIAN)
    binary.writeAnglef(this.angleEdit, LITTLE_ENDIAN)
    binary.writeInt32(this.nbScn, LITTLE_ENDIAN)
    binary.writeInt32(this.nbInter, LITTLE_ENDIAN)
    binary.writeInt32(this.nbNodes, LITTLE_ENDIAN)
    binary.writeInt32(this.nbNodeslinks, LITTLE_ENDIAN)
    binary.writeInt32(this.nbZones, LITTLE_ENDIAN)
    binary.writeInt32(this.lighting, LITTLE_ENDIAN)
    binary.writeInt32Array(this.Bpad, LITTLE_ENDIAN)
    binary.writeInt32(this.nbLights, LITTLE_ENDIAN)
    binary.writeInt32(this.nbFogs, LITTLE_ENDIAN)
    binary.writeInt32(this.nbBkgpolys, LITTLE_ENDIAN)
    binary.writeInt32(this.nbIgnoredpolys, LITTLE_ENDIAN)
    binary.writeInt32(this.nbChildpolys, LITTLE_ENDIAN)
    binary.writeInt32(this.nbPaths, LITTLE_ENDIAN)
    binary.writeInt32Array(this.pad, LITTLE_ENDIAN)
    binary.writeVector3(this.offset, LITTLE_ENDIAN)
    binary.writeFloat32Array(this.fpad, LITTLE_ENDIAN)
    binary.writeString(this.cpad, 4096)
    binary.writeInt32Array(this.bpad, LITTLE_ENDIAN)
  }
}
