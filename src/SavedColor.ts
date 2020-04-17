export class SavedColor {
  r: number
  constructor(r: number, public g: number, public b: number) {
    this.r = r // to shut the linter up
  }
}
