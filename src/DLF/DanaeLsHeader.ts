import BinaryIO, { LITTLE_ENDIAN, TRUNCATE_ZERO_BYTES, KEEP_ZERO_BYTES } from '../Binary/BinaryIO'
import { SavedVector3 } from '../SavedVector3'
import { SavedAnglef } from '../SavedAnglef'

export class DanaeLsHeader {
  // TODO: should we just ignore the contents of padding fields?

  version = 0
  ident = ''
  lastUser = ''
  time = 0
  posEdit: SavedVector3 = new SavedVector3(0, 0, 0)
  angleEdit: SavedAnglef = new SavedAnglef(0, 0, 0)
  numberOfScenes = 0
  numberOfInteractiveObjects = 0
  numberOfNodes = 0
  numberOfNodeLinks = 0
  numberOfZones = 0
  lighting = 0
  Bpad: number[] = new Array<number>(256)
  numberOfLights = 0
  numberOfFogs = 0
  numberOfBackgroundPolygons = 0
  numberOfIgnoredPolygons = 0
  numberOfChildPolygons = 0
  numberOfPaths = 0
  pad: number[] = new Array<number>(250)
  offset: SavedVector3 = new SavedVector3(0, 0, 0)
  fpad: number[] = new Array<number>(253)
  cpad = ''
  bpad: number[] = new Array<number>(256)

  readFrom(binary: BinaryIO): void {
    this.version = binary.readFloat32(LITTLE_ENDIAN)
    this.ident = binary.readString(16, TRUNCATE_ZERO_BYTES)
    this.lastUser = binary.readString(256, TRUNCATE_ZERO_BYTES)
    this.time = binary.readInt32(LITTLE_ENDIAN)
    this.posEdit = binary.readVector3(LITTLE_ENDIAN)
    this.angleEdit = binary.readAnglef(LITTLE_ENDIAN)
    this.numberOfScenes = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfInteractiveObjects = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfNodes = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfNodeLinks = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfZones = binary.readInt32(LITTLE_ENDIAN)
    this.lighting = binary.readInt32(LITTLE_ENDIAN)
    this.Bpad = binary.readInt32Array(256, LITTLE_ENDIAN)
    this.numberOfLights = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfFogs = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfBackgroundPolygons = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfIgnoredPolygons = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfChildPolygons = binary.readInt32(LITTLE_ENDIAN)
    this.numberOfPaths = binary.readInt32(LITTLE_ENDIAN)
    this.pad = binary.readInt32Array(250, LITTLE_ENDIAN)
    this.offset = binary.readVector3(LITTLE_ENDIAN)
    this.fpad = binary.readFloat32Array(253, LITTLE_ENDIAN)
    this.cpad = binary.readString(4096, KEEP_ZERO_BYTES)
    this.bpad = binary.readInt32Array(256, LITTLE_ENDIAN)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeFloat32(this.version, LITTLE_ENDIAN)
    binary.writeString(this.ident, 16)
    binary.writeString(this.lastUser, 256)
    binary.writeInt32(this.time, LITTLE_ENDIAN)
    binary.writeVector3(this.posEdit, LITTLE_ENDIAN)
    binary.writeAnglef(this.angleEdit, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfScenes, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfInteractiveObjects, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfNodes, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfNodeLinks, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfZones, LITTLE_ENDIAN)
    binary.writeInt32(this.lighting, LITTLE_ENDIAN)
    binary.writeInt32Array(this.Bpad, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfLights, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfFogs, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfBackgroundPolygons, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfIgnoredPolygons, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfChildPolygons, LITTLE_ENDIAN)
    binary.writeInt32(this.numberOfPaths, LITTLE_ENDIAN)
    binary.writeInt32Array(this.pad, LITTLE_ENDIAN)
    binary.writeVector3(this.offset, LITTLE_ENDIAN)
    binary.writeFloat32Array(this.fpad, LITTLE_ENDIAN)
    binary.writeString(this.cpad, 4096)
    binary.writeInt32Array(this.bpad, LITTLE_ENDIAN)
  }

  static sizeOf(): number {
    return 8520 // calculated manually
  }
}
