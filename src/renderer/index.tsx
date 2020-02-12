/* global location */

import { remote } from 'electron'
import React from 'react'
import { render } from 'react-dom'
import App from './components/App'

const container = document.getElementById('app')

if (container !== null) {
  const app = <App electronRemote={remote} />
  render(app, container)
}

document.addEventListener('keydown', (e): void => {
  if (e.code === 'F12') {
    remote.getCurrentWindow().webContents.toggleDevTools()
  }
  if (e.code === 'F5') {
    location.reload()
  }
})
