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

import * as THREE from 'three'
import { NOP } from './helpers/function'

const scene = new THREE.Scene()
const container = document.getElementById('screen')

const distance = 50
const rotationUnitPerPixel = 0.003

let isWindowActive = true
let isMouseDown = false
let prevMouseX = 0
// let prevMouseY = 0
let render = NOP
let resize = NOP
let repositionCamera = NOP
let camera: THREE.PerspectiveCamera | null = null

let cameraX = 0.3
// let cameraY = 0.3

if (container) {
  const { width, height } = container.getBoundingClientRect()
  camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000)

  const renderer = new THREE.WebGLRenderer({ antialias: true })
  renderer.setClearColor('#f7f7f7')
  renderer.setSize(width, height)

  container.appendChild(renderer.domElement)

  render = (): void => {
    if (camera) {
      renderer.render(scene, camera)
    }
  }

  resize = (): void => {
    const { width, height } = container.getBoundingClientRect()
    renderer.setSize(width, height)
    if (camera) {
      camera.aspect = width / height
      camera.updateProjectionMatrix()
    }
    if (isWindowActive) {
      render()
    }
  }

  repositionCamera = (): void => {
    if (camera) {
      camera.position.y = 30
      camera.position.x = Math.sin(Math.PI * cameraX) * distance
      camera.position.z = Math.cos(Math.PI * cameraX) * distance
      camera.lookAt(0, 0, 0)
    }
    if (isWindowActive) {
      render()
    }
  }
}

window.addEventListener('resize', (): void => {
  resize()
})
window.addEventListener('focus', (): void => {
  isWindowActive = true
  resize()
})
window.addEventListener('blur', (): void => {
  isWindowActive = false
  isMouseDown = false
})

// -----------------

window.addEventListener('mousedown', (e: MouseEvent): void => {
  isMouseDown = true
  if (container) {
    const { x } = container.getBoundingClientRect()
    prevMouseX = e.clientX - x
    // prevMouseY = e.clientY - y
  }
})

window.addEventListener('mousemove', (e: MouseEvent): void => {
  if (isMouseDown && container) {
    const { x } = container.getBoundingClientRect()
    const currentMouseX = e.clientX - x
    // const currentMouseY = e.clientY - y

    const diffX = currentMouseX - prevMouseX
    // const diffY = currentMouseY - prevMouseY

    if (camera) {
      if (diffX !== 0) {
        cameraX -= diffX * rotationUnitPerPixel
        camera.position.x = Math.sin(cameraX) * distance
        camera.position.z = Math.cos(cameraX) * distance
        camera.lookAt(0, 0, 0)
        render()
      }
    }

    prevMouseX = currentMouseX
    // prevMouseY = currentMouseY
  }
})

window.addEventListener('mouseup', (): void => {
  isMouseDown = false
})

// -----------------

repositionCamera()

const faceColor = new THREE.Color(0xffffff)
const lightColor = new THREE.Color(0xffffee)

const geometry = new THREE.BoxGeometry(10, 10, 10)
const material = new THREE.MeshPhongMaterial({ color: faceColor })

const mesh = new THREE.Mesh(geometry, material)
scene.add(mesh)

const light = new THREE.PointLight(lightColor, 1.5, 100)
light.position.set(20, 20, 20)
scene.add(light)

const ambientLight = new THREE.AmbientLight(0x404040)
scene.add(ambientLight)

render()
