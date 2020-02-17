import { map, prop, __, join, has } from 'ramda'
import { CHARS, CODES } from './Latin1CharsetLookup'

const UNKNOWN_CHAR = CODES[' ']

class TextIO {
  charset: string

  constructor(charset: string) {
    this.charset = charset
  }

  public decode(bytes: number[]): string {
    return join('', map(prop(__, CHARS), bytes))
  }

  public encode(str: string): number[] {
    return str.split('').map(char => (has(char, CODES) ? CODES[char] : UNKNOWN_CHAR))
  }
}

export default TextIO
