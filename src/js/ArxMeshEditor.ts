import { Color, PerspectiveCamera, BoxGeometry, MeshPhongMaterial, Mesh, PointLight, AmbientLight } from 'three'
import { ThreeApp } from './ThreeApp'
import { EditorCameraControl } from './EditorCameraControl'

export class ArxMeshEditor {
  threeApp: ThreeApp
  camera: PerspectiveCamera
  cameraControl: EditorCameraControl

  constructor(containerOrId: HTMLElement | string) {
    this.threeApp = new ThreeApp(containerOrId, { antialias: true })
    this.threeApp.scene.background = new Color('#f7f7f7')

    this.threeApp.addEventListener('resize', this.onResize)

    this.camera = new PerspectiveCamera(75, this.threeApp.aspect, 0.1, 1000)
    this.camera.position.x = 5
    this.camera.lookAt(0, 0, 0)
    this.threeApp.cameras.push(this.camera)

    this.threeApp.addEventListener('animate', this.onAnimate)

    this.cameraControl = new EditorCameraControl(this.threeApp, this.camera)

    // test stuff
    const faceColor = new Color(0xffffff)
    const lightColor = new Color(0xffffee)

    const geometry = new BoxGeometry(10, 10, 10)
    const material = new MeshPhongMaterial({ color: faceColor })

    const mesh = new Mesh(geometry, material)
    this.threeApp.scene.add(mesh)

    const light = new PointLight(lightColor, 1.5, 100)
    light.position.set(20, 20, 20)
    this.threeApp.scene.add(light)

    const ambientLight = new AmbientLight(0x404040)
    this.threeApp.scene.add(ambientLight)
  }

  private onResize = (): void => {
    this.camera.aspect = this.threeApp.aspect
    this.camera.updateProjectionMatrix()
  }

  private onAnimate = (): void => {}
}
