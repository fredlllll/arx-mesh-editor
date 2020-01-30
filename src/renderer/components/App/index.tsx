import React, { ReactElement } from 'react'
import Screen from '../Screen'
import Header from '../Header'

interface AppProps {
  onClose: Function
  onMinimize: Function
  onMaximize: Function
}

const App = (props: AppProps): ReactElement<any> => {
  const { onClose, onMinimize, onMaximize } = props
  return (
    <>
      <Header onMinimizeClick={onMinimize} onMaximizeClick={onMaximize} onCloseClick={onClose} />
      <Screen />
    </>
  )
}

export default App
