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
  Vector3,
  Triangle,
  Geometry,
  Face3,
  Mesh,
  Line,
  MeshBasicMaterial,
  LineBasicMaterial
} from 'three'

const width = 640
const height = 480

const scene = new Scene()
const camera = new PerspectiveCamera(75, width / height, 0.1, 1000)

const renderer = new WebGLRenderer()
renderer.setSize(width, height)
document.body.appendChild(renderer.domElement)

const edgeColor = new Color(0x00ff00)
const faceColor = new Color(0xffffff)

const geom = new Geometry()
const v1 = new Vector3(0, 0, 0)
const v2 = new Vector3(30, 0, 0)
const v3 = new Vector3(30, 30, 0)
const triangle = new Triangle(v1, v2, v3)
const normal = triangle.normal()
geom.vertices.push(triangle.a)
geom.vertices.push(triangle.b)
geom.vertices.push(triangle.c)
geom.faces.push(new Face3(0, 1, 2, normal))

const mesh = new Mesh(geom, new MeshBasicMaterial({ color: faceColor }))
scene.add(mesh)
const line = new Line(geom, new LineBasicMaterial({ color: edgeColor }))
scene.add(line)

camera.position.set(0, 0, 100)
camera.lookAt(0, 0, 0)

function animate() {
  requestAnimationFrame(animate)

  mesh.rotation.x += 0.01
  mesh.rotation.y += 0.01
  line.rotation.x += 0.01
  line.rotation.y += 0.01

  renderer.render(scene, camera)
}

animate()
