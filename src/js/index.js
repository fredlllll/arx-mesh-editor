/*
const loadFile = filename => {
  return fetch(filename)
    .then(res => res.arrayBuffer())
}

const parseDLF = raw => {
  return raw
}
const parseFTS = raw => {
  return raw
}
const parseLLF = raw => {
  return raw
}

Promise.all([
  loadFile('data/level11.DLF').then(parseDLF),
  loadFile('data/level11.FTS').then(parseFTS),
  loadFile('data/level11.LLF').then(parseLLF)
]).then(([dlf, fts, llf]) => {
  console.log(dlf, fts, llf)
})
*/

/* global requestAnimationFrame */

import {
  Scene,
  PerspectiveCamera,
  WebGLRenderer,
  Color,
  Mesh,
  BoxGeometry,
  MeshPhongMaterial,
  PointLight,
  AmbientLight
} from 'three'

const width = 640
const height = 480

const scene = new Scene()
const camera = new PerspectiveCamera(75, width / height, 0.1, 1000)

const renderer = new WebGLRenderer()
renderer.setSize(width, height)
document.body.appendChild(renderer.domElement)

// const edgeColor = new Color(0x00ff00)
const faceColor = new Color(0xffffff)
const lightColor = new Color(0xffffee)

const geometry = new BoxGeometry(10, 10, 10)
const material = new MeshPhongMaterial({ color: faceColor })

const mesh = new Mesh(geometry, material)
scene.add(mesh)

const light = new PointLight(lightColor, 1.5, 100)
light.position.set(20, 20, 20)
scene.add(light)

const ambientLight = new AmbientLight(0x404040)
scene.add(ambientLight)

camera.position.set(0, 30, 0)
camera.lookAt(0, 0, 0)

let cntrX = 0
let cntrZ = 0
const speed = 0.005

function animate() {
  requestAnimationFrame(animate)

  cntrX += speed
  cntrZ += speed

  camera.position.x = Math.sin(Math.PI * cntrX) * 100
  camera.position.z = Math.cos(Math.PI * cntrZ) * 100
  camera.lookAt(0, 0, 0)

  renderer.render(scene, camera)
}

animate()
