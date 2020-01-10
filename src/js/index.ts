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

function addScreen(container: HTMLElement | null): [Function, Function, THREE.Camera | null] {
  if (container) {
    const { width, height } = container.getBoundingClientRect()

    const camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000)

    const renderer = new THREE.WebGLRenderer({ antialias: true })
    renderer.setClearColor('#f7f7f7')
    renderer.setSize(width, height)

    container.appendChild(renderer.domElement)

    const render = (): void => {
      renderer.render(scene, camera)
    }

    const resize = (): void => {
      const { width, height } = container.getBoundingClientRect()
      renderer.setSize(width, height)
      camera.aspect = width / height
      camera.updateProjectionMatrix()
    }

    return [render, resize, camera]
  } else {
    return [NOP, NOP, null]
  }
}

const [render3d, resize3d, camera3d] = addScreen(document.getElementById('3d'))

window.addEventListener('resize', (): void => {
  resize3d()
})

let cntrX = 0
let cntrZ = 0
const speed = 0.005
const distance = 50

if (camera3d) {
  camera3d.position.y = 30
}

const animate = (): void => {
  requestAnimationFrame(animate)

  cntrX += speed
  cntrZ += speed

  if (camera3d) {
    camera3d.position.x = Math.sin(Math.PI * cntrX) * distance
    camera3d.position.z = Math.cos(Math.PI * cntrZ) * distance
    camera3d.lookAt(0, 0, 0)
  }

  render3d()
}

// todo: add orthograpic projections via https://threejs.org/docs/#api/en/cameras/OrthographicCamera

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

animate()
