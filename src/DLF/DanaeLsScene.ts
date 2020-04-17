import BinaryIO from '../Binary/BinaryIO'

export class DanaeLsScene {
  name = ''
  pad: number[] = new Array<number>(16)
  fpad: number[] = new Array<number>(16)

  readFrom(binary: BinaryIO): void {
    this.name = binary.readString(512, true)
    this.pad = binary.readInt32Array(16, true)
    this.fpad = binary.readFloat32Array(16, true)
  }

  writeTo(binary: BinaryIO): void {
    binary.writeString(this.name, 512)
    binary.writeInt32Array(this.pad)
    binary.writeFloat32Array(this.fpad)
  }
}
