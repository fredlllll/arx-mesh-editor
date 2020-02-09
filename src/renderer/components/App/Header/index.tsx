import React, { ReactElement } from 'react'
import { faWindowMinimize, faWindowMaximize, faWindowRestore } from '@fortawesome/free-regular-svg-icons'
import { faTimes } from '@fortawesome/free-solid-svg-icons'
import Button from '../Button'
import s from './style.scss'

interface HeaderProps {
  isWindowMaximized: boolean
  onMinimizeClick: Function
  onMaximizeClick: Function
  onCloseClick: Function
}

const Header = (props: HeaderProps): ReactElement<any> => {
  const { isWindowMaximized, onMinimizeClick, onMaximizeClick, onCloseClick } = props
  return (
    <header id={s.Header}>
      <div className={s.right}>
        <Button onClick={(): void => onMinimizeClick()} icon={faWindowMinimize} />
        <Button onClick={(): void => onMaximizeClick()} icon={isWindowMaximized ? faWindowRestore : faWindowMaximize} />
        <Button onClick={(): void => onCloseClick()} icon={faTimes} />
      </div>
      <h1>Arx Mesh Editor</h1>
    </header>
  )
}

export default Header
