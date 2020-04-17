import { SavedVector3 } from '../SavedVector3'
import { SavedAnglef } from '../SavedAnglef'
import { SavedColor } from '../SavedColor'
import TextIO from './TextIO'

class BinaryIO extends DataView {
  position: number
  textIO: TextIO

  constructor(buffer: ArrayBufferLike, byteOffset?: number, byteLength?: number) {
    super(buffer, byteOffset, byteLength)
    this.position = 0
    this.textIO = new TextIO('iso-8859-1')
  }

  readFloat32(littleEndian?: boolean): number {
    const val = this.getFloat32(this.position, littleEndian)
    this.position += 4
    return val
  }

  readFloat32Array(length: number, littleEndian?: boolean): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readFloat32(littleEndian)
    }
    return arr
  }

  readFloat64(littleEndian?: boolean): number {
    const val = this.getFloat64(this.position, littleEndian)
    this.position += 8
    return val
  }

  readFloat64Array(length: number, littleEndian?: boolean): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readFloat64(littleEndian)
    }
    return arr
  }

  readInt8(): number {
    const val = this.getInt8(this.position)
    this.position += 1
    return val
  }

  readInt8Array(length: number): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readInt8()
    }
    return arr
  }

  readInt16(littleEndian?: boolean): number {
    const val = this.getInt16(this.position, littleEndian)
    this.position += 2
    return val
  }

  readInt16Array(length: number, littleEndian?: boolean): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readInt16(littleEndian)
    }
    return arr
  }

  readInt32(littleEndian?: boolean): number {
    const val = this.getInt32(this.position, littleEndian)
    this.position += 4
    return val
  }

  readInt32Array(length: number, littleEndian?: boolean): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readInt32(littleEndian)
    }
    return arr
  }

  readUint8(): number {
    const val = this.getUint8(this.position)
    this.position += 1
    return val
  }

  readUint8Array(length: number): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readUint8()
    }
    return arr
  }

  readUint16(littleEndian?: boolean): number {
    const val = this.getUint16(this.position, littleEndian)
    this.position += 2
    return val
  }

  readUint16Array(length: number, littleEndian?: boolean): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readUint16(littleEndian)
    }
    return arr
  }

  readUint32(littleEndian?: boolean): number {
    const val = this.getUint32(this.position, littleEndian)
    this.position += 4
    return val
  }

  readUint32Array(length: number, littleEndian?: boolean): number[] {
    const arr = new Array<number>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readUint32(littleEndian)
    }
    return arr
  }

  writeFloat32(value: number, littleEndian?: boolean): void {
    this.setFloat32(this.position, value, littleEndian)
    this.position += 4
  }

  writeFloat32Array(values: number[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeFloat32(values[i], littleEndian)
    }
  }

  writeFloat64(value: number, littleEndian?: boolean): void {
    this.setFloat32(this.position, value, littleEndian)
    this.position += 4
  }

  writeFloat64Array(values: number[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeFloat64(values[i], littleEndian)
    }
  }

  writeInt8(value: number): void {
    this.setInt8(this.position, value)
    this.position += 1
  }

  writeInt8Array(values: number[]): void {
    for (let i = 0; i < values.length; i++) {
      this.writeInt8(values[i])
    }
  }

  writeInt16(value: number, littleEndian?: boolean): void {
    this.setInt16(this.position, value, littleEndian)
    this.position += 2
  }

  writeInt16Array(values: number[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeInt16(values[i], littleEndian)
    }
  }

  writeInt32(value: number, littleEndian?: boolean): void {
    this.setInt32(this.position, value, littleEndian)
    this.position += 4
  }

  writeInt32Array(values: number[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeInt32(values[i], littleEndian)
    }
  }

  writeUint8(value: number): void {
    this.setUint8(this.position, value)
    this.position += 1
  }

  writeUint8Array(values: number[]): void {
    for (let i = 0; i < values.length; i++) {
      this.writeUint8(values[i])
    }
  }

  writeUint16(value: number, littleEndian?: boolean): void {
    this.setUint16(this.position, value, littleEndian)
    this.position += 2
  }

  writeUint16Array(values: number[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeUint16(values[i], littleEndian)
    }
  }

  writeUint32(value: number, littleEndian?: boolean): void {
    this.setUint32(this.position, value, littleEndian)
    this.position += 4
  }

  writeUint32Array(values: number[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeUint32(values[i], littleEndian)
    }
  }

  readBigInt64(littleEndian?: boolean): bigint {
    const val = this.getBigInt64(this.position, littleEndian)
    this.position += 8
    return val
  }

  readBigInt64Array(length: number, littleEndian?: boolean): bigint[] {
    const arr = new Array<bigint>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readBigInt64(littleEndian)
    }
    return arr
  }

  readBigUint64(littleEndian?: boolean): bigint {
    const val = this.getBigUint64(this.position, littleEndian)
    this.position += 8
    return val
  }

  readBigUint64Array(length: number, littleEndian?: boolean): bigint[] {
    const arr = new Array<bigint>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readBigUint64(littleEndian)
    }
    return arr
  }

  writeBigInt64(value: bigint, littleEndian?: boolean): void {
    this.setBigInt64(this.position, value, littleEndian)
    this.position += 8
  }

  writeBigInt64Array(values: bigint[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeBigInt64(values[i], littleEndian)
    }
  }

  writeBigUint64(value: bigint, littleEndian?: boolean): void {
    this.setBigUint64(this.position, value, littleEndian)
    this.position += 8
  }

  writeBigUint64Array(values: bigint[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeBigUint64(values[i], littleEndian)
    }
  }

  readString(length?: number, truncateZeroBytes?: boolean): string {
    const codes: number[] = []
    let c = 0
    if (length !== undefined) {
      for (let i = 0; i < length; i++) {
        c = this.readUint8()
        if (!(c === 0 && truncateZeroBytes)) {
          codes.push(c)
        }
      }
    } else {
      while (true) {
        c = this.readUint8()
        if (c === 0) {
          break
        }
        codes.push(c)
      }
    }

    return this.textIO.decode(codes)
  }

  writeString(str: string, length?: number): void {
    // if length is given we assume a fixed length string
    if (length !== undefined) {
      const charCodes = new Array<number>(length)
      charCodes.fill(0)

      // replacing 0s in charCodes one by one from left to right
      this.textIO.encode(str).forEach((charCode, index) => {
        charCodes[index] = charCode
      })

      charCodes.forEach(this.writeUint8)
    } else {
      // otherwise its a 0 terminated c string
      this.textIO.encode(str).forEach(this.writeUint8)
      this.writeUint8(0)
    }
  }

  // vector3
  readVector3(littleEndian?: boolean): SavedVector3 {
    const x = this.readFloat32(littleEndian)
    const y = this.readFloat32(littleEndian)
    const z = this.readFloat32(littleEndian)
    return new SavedVector3(x, y, z)
  }

  readVector3Array(length: number, littleEndian?: boolean): SavedVector3[] {
    const arr = new Array<SavedVector3>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readVector3(littleEndian)
    }
    return arr
  }

  writeVector3(vec: SavedVector3, littleEndian?: boolean): void {
    this.writeFloat32(vec.x, littleEndian)
    this.writeFloat32(vec.y, littleEndian)
    this.writeFloat32(vec.z, littleEndian)
  }

  writeVector3Array(values: SavedVector3[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeVector3(values[i], littleEndian)
    }
  }

  // anglef
  readAnglef(littleEndian?: boolean): SavedAnglef {
    const a = this.readFloat32(littleEndian)
    const b = this.readFloat32(littleEndian)
    const g = this.readFloat32(littleEndian)
    return new SavedAnglef(a, b, g)
  }

  readAnglefArray(length: number, littleEndian?: boolean): SavedAnglef[] {
    const arr = new Array<SavedAnglef>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readAnglef(littleEndian)
    }
    return arr
  }

  writeAnglef(ang: SavedAnglef, littleEndian?: boolean): void {
    this.writeFloat32(ang.a, littleEndian)
    this.writeFloat32(ang.b, littleEndian)
    this.writeFloat32(ang.g, littleEndian)
  }

  writeAnglefArray(values: SavedAnglef[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeAnglef(values[i], littleEndian)
    }
  }

  // savedcolor
  readColor(littleEndian?: boolean): SavedColor {
    const r = this.readFloat32(littleEndian)
    const g = this.readFloat32(littleEndian)
    const b = this.readFloat32(littleEndian)
    return new SavedColor(r, g, b)
  }

  readColorArray(length: number, littleEndian?: boolean): SavedColor[] {
    const arr = new Array<SavedColor>(length)
    for (let i = 0; i < length; i++) {
      arr[i] = this.readColor(littleEndian)
    }
    return arr
  }

  writeColor(col: SavedColor, littleEndian?: boolean): void {
    this.writeFloat32(col.r, littleEndian)
    this.writeFloat32(col.g, littleEndian)
    this.writeFloat32(col.b, littleEndian)
  }

  writeColorArray(values: SavedColor[], littleEndian?: boolean): void {
    for (let i = 0; i < values.length; i++) {
      this.writeColor(values[i], littleEndian)
    }
  }
}

export default BinaryIO
