import { remote } from 'electron'
import { ArxMeshEditor } from './ArxMeshEditor'
import './style.scss'

const container = document.getElementById('app')

if (container !== null) {
  // TODO: probably use a library to create the additional UI elements outside of the canvas
  // *cough* react *cough*
  container.innerHTML = `
  <header>
    <h1>Arx Mesh Editor</h1>
    <button type="button" id="minimize-button">_</button>
    <button type="button" id="maximize-button">[ ]</button>
    <button type="button" id="close-button">X</button>
  </header>
  <div id="screen"></div>
  `
}

const arxMeshEditor = new ArxMeshEditor('screen')

// test to load level11
arxMeshEditor.loadLevel('level11')

const closeButton = document.getElementById('close-button')
if (closeButton) {
  closeButton.addEventListener('click', () => {
    const window = remote.getCurrentWindow()
    window.close()
  })
}

const minimizeButton = document.getElementById('minimize-button')
if (minimizeButton) {
  minimizeButton.addEventListener('click', () => {
    const window = remote.getCurrentWindow()
    window.minimize()
  })
}

const maximizeButton = document.getElementById('maximize-button')
if (maximizeButton) {
  maximizeButton.addEventListener('click', () => {
    const window = remote.getCurrentWindow()
    if (!window.isMaximized()) {
      window.maximize()
    } else {
      window.unmaximize()
    }
  })
}
