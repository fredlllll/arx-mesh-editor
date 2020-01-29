import { ArxMeshEditor } from './ArxMeshEditor'
import './style.scss'

const container = document.getElementById('app')

if (container !== null) {
  // TODO: probably use a library to create the additional UI elements outside of the canvas
  // *cough* react *cough*
  container.innerHTML = `
  <header>
    <h1>Arx Mesh Editor</h1>
  </header>
  <div id="screen"></div>
  `
}

const arxMeshEditor = new ArxMeshEditor('screen')

// test to load level11
arxMeshEditor.loadLevel('level11')

console.log('console?')
