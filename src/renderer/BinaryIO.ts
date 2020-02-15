export class BinaryIO extends DataView {
  position: number

  constructor(buffer: ArrayBufferLike, byteOffset?: number, byteLength?: number) {
    super(buffer, byteOffset, byteLength)
    this.position = 0
  }

  readFloat32(littleEndian?: boolean): number {
    const val = this.getFloat32(this.position, littleEndian)
    this.position += 4
    return val
  }

  readFloat64(littleEndian?: boolean): number {
    const val = this.getFloat64(this.position, littleEndian)
    this.position += 8
    return val
  }

  readInt8(): number {
    const val = this.getInt8(this.position)
    this.position += 1
    return val
  }

  readInt16(littleEndian?: boolean): number {
    const val = this.getInt16(this.position, littleEndian)
    this.position += 2
    return val
  }

  readInt32(littleEndian?: boolean): number {
    const val = this.getInt32(this.position, littleEndian)
    this.position += 4
    return val
  }

  readUint8(): number {
    const val = this.getUint8(this.position)
    this.position += 1
    return val
  }

  readUint16(littleEndian?: boolean): number {
    const val = this.getUint16(this.position, littleEndian)
    this.position += 2
    return val
  }

  readUint32(littleEndian?: boolean): number {
    const val = this.getUint32(this.position, littleEndian)
    this.position += 4
    return val
  }

  writeFloat32(value: number, littleEndian?: boolean): void {
    this.setFloat32(this.position, value, littleEndian)
    this.position += 4
  }

  writeFloat64(value: number, littleEndian?: boolean): void {
    this.setFloat32(this.position, value, littleEndian)
    this.position += 4
  }

  writeInt8(value: number): void {
    this.setInt8(this.position, value)
    this.position += 1
  }

  writeInt16(value: number, littleEndian?: boolean): void {
    this.setInt16(this.position, value, littleEndian)
    this.position += 2
  }

  writeInt32(value: number, littleEndian?: boolean): void {
    this.setInt32(this.position, value, littleEndian)
    this.position += 4
  }

  writeUint8(value: number): void {
    this.setUint8(this.position, value)
    this.position += 1
  }

  writeUint16(value: number, littleEndian?: boolean): void {
    this.setUint16(this.position, value, littleEndian)
    this.position += 2
  }

  writeUint32(value: number, littleEndian?: boolean): void {
    this.setUint32(this.position, value, littleEndian)
    this.position += 4
  }

  readBigInt64(littleEndian?: boolean): bigint {
    const val = this.getBigInt64(this.position, littleEndian)
    this.position += 8
    return val
  }

  readBigUint64(littleEndian?: boolean): bigint {
    const val = this.getBigUint64(this.position, littleEndian)
    this.position += 8
    return val
  }

  writeBigInt64(value: bigint, littleEndian?: boolean): void {
    this.setBigInt64(this.position, value, littleEndian)
    this.position += 8
  }

  writeBigUint64(value: bigint, littleEndian?: boolean): void {
    this.setBigUint64(this.position, value, littleEndian)
    this.position += 8
  }

  readString(length?: number): string {
    // TODO: implement reading either fixed length or variable length strings (c strings or with length byte??)
    return `${length}`
  }

  writeString(str: string): void {
    // TODO: write string to view, either fixed length or variable length?
    this.writeUint32(str.length) // TODO: just so str is used somehow
  }
}
