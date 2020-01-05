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

const width = 640
const height = 480

var scene = new THREE.Scene();
var camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);

var renderer = new THREE.WebGLRenderer();
renderer.setSize(width, height);
document.body.appendChild(renderer.domElement);

var geometry = new THREE.BoxGeometry(1, 1, 1);
var material = new THREE.MeshBasicMaterial({ color: 0x00ff00 });
var cube = new THREE.Mesh(geometry, material);
scene.add(cube);

camera.position.z = 5;

function animate() {
  requestAnimationFrame(animate);
  renderer.render(scene, camera);
  cube.rotation.x += 0.01;
  cube.rotation.y += 0.01;
}

animate();
