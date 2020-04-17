import { Euler, Vector3, Object3D } from 'three'
import { ThreeApp } from './ThreeApp'

export class EditorCameraControl {
  public rotationSpeed = 0.3
  public movementSpeed = 1

  private threeApp_: ThreeApp
  private target_: Object3D

  private isMouseDown = false
  private targetEuler = new Euler(0, 0, 0, 'YXZ')

  private controlButton = 2

  private keyForwards = 'KeyW'
  private keyBackwards = 'KeyS'
  private keyLeft = 'KeyA'
  private keyRight = 'KeyD'
  private keyUp = 'Space'
  private keyDown = 'ControlLeft'

  private forwardsDown = false
  private backwardsDown = false
  private leftDown = false
  private rightDown = false
  private upDown = false
  private downDown = false

  public get target(): Object3D {
    return this.target_
  }

  public get threeApp(): ThreeApp {
    return this.threeApp_
  }

  constructor(threeApp: ThreeApp, target: Object3D) {
    this.initEvents(threeApp)

    this.targetEuler.y = target.rotation.y
    this.targetEuler.x = target.rotation.x

    this.threeApp_ = threeApp
    this.target_ = target
  }

  private initEvents(threeApp: ThreeApp): void {
    const elem = threeApp.renderer.domElement
    elem.addEventListener('mousemove', this.onMouseMove)
    elem.addEventListener('mousedown', this.onMouseDown)
    elem.addEventListener('mouseup', this.onMouseUp)
    elem.addEventListener('contextmenu', this.onContextMenu)
    elem.addEventListener('mouseout', this.onMouseOut)

    document.addEventListener('keydown', this.onKeyDown)
    document.addEventListener('keyup', this.onKeyUp)

    window.addEventListener('blur', this.onBlur)

    threeApp.addEventListener('animate', this.onAnimate)
  }

  private onAnimate = (): void => {
    // update target position etc
    const forward = new Vector3(0, 0, -1).applyQuaternion(this.target.quaternion)
    const left = new Vector3(-1, 0, 0).applyQuaternion(this.target.quaternion)
    const up = new Vector3(0, 1, 0) // up down motion is not depending on target orientation

    const offset = new Vector3()
    if (this.forwardsDown) {
      offset.add(forward)
    }
    if (this.backwardsDown) {
      offset.sub(forward)
    }
    if (this.leftDown) {
      offset.add(left)
    }
    if (this.rightDown) {
      offset.sub(left)
    }
    if (this.upDown) {
      offset.add(up)
    }
    if (this.downDown) {
      offset.sub(up)
    }

    offset.multiplyScalar(this.movementSpeed)
    this.target.position.add(offset)
  }

  // prevent context menu from showing up
  private onContextMenu = (ev: MouseEvent): void => {
    ev.preventDefault()
  }

  private onMouseMove = (ev: MouseEvent): void => {
    // target rotation should actually be updated in animate, but i think its fine in this case?
    if (this.isMouseDown) {
      this.targetEuler.y -= ev.movementX * this.rotationSpeed * 0.01
      this.targetEuler.x -= ev.movementY * this.rotationSpeed * 0.01
      this.targetEuler.x = Math.max(-Math.PI / 2, Math.min(Math.PI / 2, this.targetEuler.x))

      this.target_.quaternion.setFromEuler(this.targetEuler)
    }
  }

  private onMouseDown = (ev: MouseEvent): void => {
    if (ev.button === this.controlButton) {
      this.isMouseDown = true
    }
  }

  private onMouseUp = (ev: MouseEvent): void => {
    if (this.isMouseDown && ev.button === this.controlButton) {
      this.isMouseDown = false
    }
  }

  private onMouseOut = (): void => {
    this.isMouseDown = false
  }

  private onBlur = (): void => {
    this.isMouseDown = false

    this.forwardsDown = false
    this.backwardsDown = false
    this.leftDown = false
    this.rightDown = false
  }

  private setKeyDown(ev: KeyboardEvent, down: boolean): void {
    switch (ev.code) {
      case this.keyForwards:
        this.forwardsDown = down
        break
      case this.keyBackwards:
        this.backwardsDown = down
        break
      case this.keyLeft:
        this.leftDown = down
        break
      case this.keyRight:
        this.rightDown = down
        break
      case this.keyUp:
        this.upDown = down
        break
      case this.keyDown:
        this.downDown = down
        break
    }
  }

  private onKeyDown = (ev: KeyboardEvent): void => {
    this.setKeyDown(ev, true)
  }

  private onKeyUp = (ev: KeyboardEvent): void => {
    this.setKeyDown(ev, false)
  }
}
