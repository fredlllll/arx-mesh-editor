import React, { ReactElement, useState, useRef, useLayoutEffect } from 'react'
import { isNil } from 'ramda'
import { ArxMeshEditor } from '../../ArxMeshEditor'
import './reset.scss'
import { NEW_LEVEL } from '../../ArxLevel'
import Screen from './Screen'
import Header from './Header'
import LevelSelector from './LevelSelector'

interface AppProps {
  electronRemote: any
}

let arxMeshEditor: ArxMeshEditor

const App = (props: AppProps): ReactElement<any> => {
  const { electronRemote: remote } = props

  const [isLevelLoaded, setIsLevelLoaded] = useState(false)
  const [isWindowMaximized, setIsWindowMaximized] = useState(false)

  const screenRef = useRef<HTMLDivElement>(null)

  useLayoutEffect(() => {
    if (!isNil(screenRef.current)) {
      arxMeshEditor = new ArxMeshEditor(screenRef.current)
    }
  }, [])

  const window = remote.getCurrentWindow()

  return (
    <>
      <Header
        isWindowMaximized={isWindowMaximized}
        onMinimizeClick={(): void => {
          window.minimize()
        }}
        onMaximizeClick={(): void => {
          if (window.isMaximized()) {
            window.unmaximize()
          } else {
            window.maximize()
          }
          setIsWindowMaximized(window.isMaximized())
        }}
        onCloseClick={(): void => {
          window.close()
        }}
      />
      <Screen ref={screenRef} />
      {!isLevelLoaded && (
        <LevelSelector
          onSelect={(level: string): void => {
            setIsLevelLoaded(true)
            if (level === NEW_LEVEL) {
              arxMeshEditor.newLevel()
            } else {
              arxMeshEditor.loadLevel(level)
            }
          }}
        />
      )}
    </>
  )
}

export default App
