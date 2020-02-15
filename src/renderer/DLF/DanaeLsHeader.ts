import { BinaryIO } from '../BinaryIO'
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
    this.version = binary.readFloat32(true)
    this.ident = binary.readString(16, true)
    this.lastuser = binary.readString(256, true)
    this.time = binary.readInt32(true)
    this.posEdit = binary.readVector3(true)
    this.angleEdit = binary.readAnglef(true)
    this.nbScn = binary.readInt32(true)
    this.nbInter = binary.readInt32(true)
    this.nbNodes = binary.readInt32(true)
    this.nbNodeslinks = binary.readInt32(true)
    this.nbZones = binary.readInt32(true)
    this.lighting = binary.readInt32(true)
    this.Bpad = binary.readInt32Array(256, true)
    this.nbLights = binary.readInt32(true)
    this.nbFogs = binary.readInt32(true)
    this.nbBkgpolys = binary.readInt32(true)
    this.nbIgnoredpolys = binary.readInt32(true)
    this.nbChildpolys = binary.readInt32(true)
    this.nbPaths = binary.readInt32(true)
    this.pad = binary.readInt32Array(250, true)
    this.offset = binary.readVector3(true)
    this.fpad = binary.readFloat32Array(253, true)
    this.cpad = binary.readString(4096)
    this.bpad = binary.readInt32Array(256, true)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeFloat32(this.version, true)
    binary.writeString(this.ident, 16)
    binary.writeString(this.lastuser, 256)
    binary.writeInt32(this.time, true)
    binary.writeVector3(this.posEdit, true)
    binary.writeAnglef(this.angleEdit, true)
    binary.writeInt32(this.nbScn, true)
    binary.writeInt32(this.nbInter, true)
    binary.writeInt32(this.nbNodes, true)
    binary.writeInt32(this.nbNodeslinks, true)
    binary.writeInt32(this.nbZones, true)
    binary.writeInt32(this.lighting, true)
    binary.writeInt32Array(this.Bpad, true)
    binary.writeInt32(this.nbLights, true)
    binary.writeInt32(this.nbFogs, true)
    binary.writeInt32(this.nbBkgpolys, true)
    binary.writeInt32(this.nbIgnoredpolys, true)
    binary.writeInt32(this.nbChildpolys, true)
    binary.writeInt32(this.nbPaths, true)
    binary.writeInt32Array(this.pad, true)
    binary.writeVector3(this.offset, true)
    binary.writeFloat32Array(this.fpad, true)
    binary.writeString(this.cpad, 4096)
    binary.writeInt32Array(this.bpad, true)
  }
}
