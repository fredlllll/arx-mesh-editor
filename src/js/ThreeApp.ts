import { WebGLRenderer, Scene, EventDispatcher, Camera, WebGLRendererParameters } from 'three'
import { isString } from './TypeGuards'

export class ThreeApp extends EventDispatcher {
  private container_: HTMLElement
  public get container() {
    return this.container_
  }

  private renderer_: WebGLRenderer
  public get renderer() {
    return this.renderer_
  }

  private scene_: Scene
  public get scene() {
    return this.scene_
  }

  private cameras_: Camera[] = []
  public get cameras() {
    return this.cameras_
  }

  private aspect_ = 0
  public get aspect() {
    return this.aspect_
  }

  private screenWidth_ = 0
  public get screenWidth() {
    return this.screenWidth_
  }

  private screenHeight_ = 0
  public get screenHeight() {
    return this.screenHeight_
  }

  private isWindowActive_ = true
  public get isWindowActive() {
    return this.isWindowActive_
  }

  constructor(containerOrId: HTMLElement | string, webGLParameters: WebGLRendererParameters) {
    super()
    if (isString(containerOrId)) {
      this.container_ = document.getElementById(containerOrId) as HTMLElement
    } else {
      this.container_ = containerOrId
    }

    this.updateScreenDimensions()
    this.renderer_ = this.createRenderer(webGLParameters)
    this.container_.appendChild(this.renderer_.domElement)
    this.scene_ = new Scene()
    this.initEvents()

    this.animate()
  }

  private animate = () => {
    window.requestAnimationFrame(this.animate)

    this.dispatchEvent({ type: 'animate' })

    this.renderer_.clear()
    if (this.isWindowActive_) {
      for (const cam of this.cameras_) {
        this.renderer_.render(this.scene_, cam)
      }
    }
  }

  private updateScreenDimensions() {
    this.screenWidth_ = this.container_.clientWidth
    this.screenHeight_ = this.container_.clientHeight
    this.aspect_ = this.screenWidth_ / this.screenHeight_
  }

  private createRenderer(webGLParameters: WebGLRendererParameters) {
    const renderer = new WebGLRenderer(webGLParameters)
    renderer.setPixelRatio(window.devicePixelRatio)
    renderer.setSize(this.screenWidth_, this.screenHeight_)
    renderer.setViewport(0, 0, this.screenWidth_, this.screenHeight_)
    renderer.autoClear = false
    return renderer
  }

  private initEvents() {
    window.addEventListener('resize', this.onWindowResize, false)
    window.addEventListener('focus', this.onWindowFocus)
    window.addEventListener('blur', this.onWindowBlur)
  }

  private onWindowFocus = () => {
    this.isWindowActive_ = true
    this.onWindowResize()
  }

  private onWindowBlur = () => {
    this.isWindowActive_ = false
  }

  private onWindowResize = () => {
    this.updateScreenDimensions()
    this.renderer_.setSize(this.screenWidth_, this.screenHeight_)
    this.renderer_.setViewport(0, 0, this.screenWidth_, this.screenHeight_)

    this.dispatchEvent({ type: 'resize', sender: this }) // cameras have to be updated externally
  }
}
