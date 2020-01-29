import path from 'path'
import { format as formatUrl } from 'url'
import { app, BrowserWindow } from 'electron'

const isDevelopment = process.env.NODE_ENV !== 'production'

// TODO: add custom icon - https://stackoverflow.com/a/31538436/1806628
app.on('ready', () => {
  const window = new BrowserWindow({
    frame: false,
    title: 'Arx Mesh Editor',
    webPreferences: {
      nodeIntegration: true
    }
  })

  if (isDevelopment) {
    window.loadURL(`http://localhost:${process.env.ELECTRON_WEBPACK_WDS_PORT}`)
  } else {
    window.loadURL(
      formatUrl({
        pathname: path.join(__dirname, 'index.html'),
        protocol: 'file',
        slashes: true
      })
    )
  }
})
