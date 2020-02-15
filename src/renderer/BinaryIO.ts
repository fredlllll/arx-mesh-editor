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
    // TODO: this assumes utf-8 do we need special treatment for the 127-255 characters?
    let retval: string
    if (length !== undefined) {
      // a bit ugly, but better than string concatenations
      retval = String.fromCharCode(...new Uint8Array(this.buffer.slice(this.position, this.position + length)))
      this.position += length
    } else {
      const codes: number[] = []
      let c = 0
      do {
        c = this.readUint8()
        codes.push(c)
      } while (c !== 0)
      retval = String.fromCharCode(...codes)
    }
    return retval
  }

  writeString(str: string, length?: number): void {
    // TODO: this will probably break for utf characters, what encoding to we need to handle? ascii?
    // if length is given we assume a fixed length string
    if (length !== undefined) {
      for (let i = 0; i < length; i++) {
        const c = str.charCodeAt(i) // will be NaN if out of bounds
        if (isNaN(c)) {
          this.writeUint8(0) // pad with 0
        } else {
          this.writeUint8(c)
        }
      }
    } else {
      // otherwise its a 0 terminated c string
      for (let i = 0; i < str.length; i++) {
        const c = str.charCodeAt(i)
        this.writeUint8(c)
      }
      this.writeUint8(0)
    }
  }
}
