import { remote } from 'electron'
import React from 'react'
import { render } from 'react-dom'
import App from './components/App'

const container = document.getElementById('app')

if (container !== null) {
  const app = <App electronRemote={remote} />
  render(app, container)
}
