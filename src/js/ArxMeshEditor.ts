import { Color, PerspectiveCamera, BoxGeometry, MeshPhongMaterial, Mesh, PointLight, AmbientLight } from 'three'
import { ThreeApp } from './ThreeApp'
import { EditorCameraControl } from './EditorCameraControl'
import { ArxLevel } from './ArxLevel'

export class ArxMeshEditor {
  private threeApp: ThreeApp
  private camera: PerspectiveCamera

  private currentLevel_?: ArxLevel

  constructor(containerOrId: HTMLElement | string) {
    const threeApp = this.createThreeApp(containerOrId)

    const camera = this.createCamera(threeApp.aspect)
    threeApp.addCamera(camera)
    this.createCameraControl(threeApp, camera)

    this.initEvents(threeApp)
    this.initTestScene(threeApp)

    this.threeApp = threeApp
    this.camera = camera
  }

  public newLevel(): void {
    // create a new empty level
    const level = new ArxLevel()
    this.SetCurrentLevel(level)
  }

  public loadLevel(name: string): Promise<ArxLevel> {
    const level = new ArxLevel()
    this.SetCurrentLevel(level)
    return level.load(name)
  }

  public saveLevel(name?: string): Promise<ArxLevel> | undefined {
    if (this.currentLevel_) {
      return this.currentLevel_.save(name)
    }
    return undefined // cant save level if its not set
  }

  private SetCurrentLevel(level: ArxLevel | undefined): void {
    if (this.currentLevel_) {
      this.threeApp.scene.remove(this.currentLevel_)
    }
    if (level) {
      this.threeApp.scene.add(level)
    }
    this.currentLevel_ = level
  }

  private createThreeApp(containerOrId: HTMLElement | string): ThreeApp {
    const threeApp = new ThreeApp(containerOrId, { antialias: true })
    threeApp.scene.background = new Color('#f7f7f7')
    return threeApp
  }

  private createCamera(aspect: number): PerspectiveCamera {
    const camera = new PerspectiveCamera(75, aspect, 0.1, 1000)
    camera.position.x = 50
    camera.lookAt(0, 0, 0)
    return camera
  }

  private createCameraControl(threeApp: ThreeApp, camera: PerspectiveCamera): EditorCameraControl {
    const cameraControl = new EditorCameraControl(threeApp, camera)
    return cameraControl
  }

  private initEvents(threeApp: ThreeApp): void {
    threeApp.addEventListener('resize', this.onResize)
    threeApp.addEventListener('animate', this.onAnimate)
  }

  private initTestScene(threeApp: ThreeApp): void {
    const faceColor = new Color(0xffffff)
    const lightColor = new Color(0xffffee)

    const geometry = new BoxGeometry(10, 10, 10)
    const material = new MeshPhongMaterial({ color: faceColor })

    const mesh = new Mesh(geometry, material)
    threeApp.scene.add(mesh)

    const light = new PointLight(lightColor, 1.5, 100)
    light.position.set(20, 20, 20)
    threeApp.scene.add(light)

    const ambientLight = new AmbientLight(0x404040)
    threeApp.scene.add(ambientLight)
  }

  private onResize = (): void => {
    this.camera.aspect = this.threeApp.aspect
    this.camera.updateProjectionMatrix()
  }

  private onAnimate = (): void => {}
}
