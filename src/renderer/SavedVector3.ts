export class SavedVector3 {
  x: number
  constructor(x: number, public y: number, public z: number) {
    this.x = x // to shut the linter up
  }
}
