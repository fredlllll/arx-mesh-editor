import { remote } from 'electron'
import React from 'react'
import { render } from 'react-dom'
import { ArxMeshEditor } from './ArxMeshEditor'
import App from './components/App'
import { ScreenID } from './components/Screen'
import './reset.scss'

const container = document.getElementById('app')

if (container !== null) {
  const app = (
    <App
      onClose={(): void => {
        const window = remote.getCurrentWindow()
        window.close()
      }}
      onMinimize={(): void => {
        const window = remote.getCurrentWindow()
        window.minimize()
      }}
      onMaximize={(): void => {
        const window = remote.getCurrentWindow()
        if (window.isMaximized()) {
          window.unmaximize()
        } else {
          window.maximize()
        }
      }}
    />
  )
  render(app, container)
}

const arxMeshEditor = new ArxMeshEditor(ScreenID)

// test to load level11
arxMeshEditor.loadLevel('level11')
