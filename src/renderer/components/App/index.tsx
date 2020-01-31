import React, { ReactElement, useState, useRef } from 'react'
import { isNil } from 'ramda'
import Screen from '../Screen'
import Header from '../Header'
import { ArxMeshEditor } from '../../ArxMeshEditor'
import './reset.scss'
import LevelSelector from './LevelSelector'

interface AppProps {
  electronRemote: any
}

let arxMeshEditor: ArxMeshEditor

const App = (props: AppProps): ReactElement<any> => {
  const { electronRemote: remote } = props

  const [isLevelLoaded, setIsLevelLoaded] = useState(false)

  const screenRef = useRef<HTMLDivElement>(null)

  return (
    <>
      <Header
        onMinimizeClick={(): void => {
          const window = remote.getCurrentWindow()
          window.minimize()
        }}
        onMaximizeClick={(): void => {
          const window = remote.getCurrentWindow()
          if (window.isMaximized()) {
            window.unmaximize()
          } else {
            window.maximize()
          }
        }}
        onCloseClick={(): void => {
          const window = remote.getCurrentWindow()
          window.close()
        }}
      />
      <Screen ref={screenRef} />
      {!isLevelLoaded && (
        <LevelSelector
          onSelect={(level: string): void => {
            setIsLevelLoaded(true)
            if (!isNil(screenRef.current)) {
              arxMeshEditor = new ArxMeshEditor(screenRef.current)
              arxMeshEditor.loadLevel(level)
            }
          }}
        />
      )}
    </>
  )
}

export default App
