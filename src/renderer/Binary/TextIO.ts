class TextIO {
  charset: string

  constructor(charset: string) {
    this.charset = charset
  }

  public decode(bytes: Uint8Array): string {
    return String.fromCharCode(...bytes)
  }

  public encode(str: string): Uint8Array {
    return Uint8Array.from(str.split('').map(char => char.charCodeAt(0)))
  }
}

export default TextIO
