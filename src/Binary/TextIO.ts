import { map, join, propOr, split, nth, defaultTo } from 'ramda'
import { CHARS, CODES } from './Latin1CharsetLookup'

const BYTE_OF_AN_UNKNOWN_CHAR = CODES[' ']
const CHAR_OF_AN_UNKNOWN_BYTE = ' '

class TextIO {
  charset: string

  constructor(charset: string) {
    this.charset = charset
  }

  public decode(bytes: number[]): string {
    return join(
      '',
      map((byte) => defaultTo(CHAR_OF_AN_UNKNOWN_BYTE, nth(byte, CHARS)), bytes)
    )
  }

  public encode(str: string): number[] {
    return map((char) => propOr(BYTE_OF_AN_UNKNOWN_CHAR, char, CODES), split('', str))
  }
}

export default TextIO
