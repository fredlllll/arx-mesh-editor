import { Camera, Euler, Vector3 } from 'three'
import { ThreeApp } from './ThreeApp'

export class EditorCameraControl {
  public get camera(): Camera {
    return this.camera_
  }

  public get threeApp(): ThreeApp {
    return this.threeApp_
  }

  public rotationSpeed = 0.1
  public movementSpeed = 1

  private isMouseDown = false
  private camRot: Euler

  private controlButton = 2

  private keyForwards = 'KeyW'
  private keyBackwards = 'KeyS'
  private keyLeft = 'KeyA'
  private keyRight = 'KeyD'

  private forwardsDown = false
  private backwardsDown = false
  private leftDown = false
  private rightDown = false

  constructor(private threeApp_: ThreeApp, private camera_: Camera) {
    const elem = threeApp_.renderer.domElement

    elem.addEventListener('mousemove', this.onMouseMove)
    elem.addEventListener('mousedown', this.onMouseDown)
    elem.addEventListener('mouseup', this.onMouseUp)

    elem.addEventListener('keydown', this.onKeyDown)
    elem.addEventListener('keyup', this.onKeyUp)

    window.addEventListener('blur', this.onBlur)

    threeApp_.addEventListener('animate', this.onAnimate)

    this.camRot = camera_.rotation
  }

  private onAnimate = (): void => {
    // update cam position etc
    const forward = this.camera_.localToWorld(new Vector3(0, 0, 1))
    const left = this.camera_.localToWorld(new Vector3(-1, 0, 0))

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
    offset.multiplyScalar(this.movementSpeed)
    this.camera.position.add(offset)
  }

  private onMouseMove = (ev: MouseEvent): void => {
    // camerarotation should actually be updated in animate, but i think its fine in this case?
    if (this.isMouseDown) {
      this.camRot.y -= ev.movementX * this.rotationSpeed
      let x = this.camRot.x + ev.movementY * this.rotationSpeed
      if (x > 89) {
        x = 89
      } else if (x < -89) {
        x = -89
      }
      this.camRot.x = x
    }
  }

  private onMouseDown = (ev: MouseEvent): void => {
    if (ev.button === this.controlButton) {
      this.isMouseDown = true
    }
  }

  private onMouseUp = (ev: MouseEvent): void => {
    if (ev.button === this.controlButton) {
      this.isMouseDown = false
    }
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
    }
  }

  private onKeyDown = (ev: KeyboardEvent): void => {
    this.setKeyDown(ev, true)
  }

  private onKeyUp = (ev: KeyboardEvent): void => {
    this.setKeyDown(ev, false)
  }
}
