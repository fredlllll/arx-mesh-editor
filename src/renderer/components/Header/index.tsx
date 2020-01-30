import React, { ReactElement } from 'react'
import Button from '../Button'
import s from './style.scss'

interface HeaderProps {
  onMinimizeClick: Function
  onMaximizeClick: Function
  onCloseClick: Function
}

const Header = (props: HeaderProps): ReactElement<any> => {
  const { onMinimizeClick, onMaximizeClick, onCloseClick } = props
  return (
    <header id={s.Header}>
      <h1>Arx Mesh Editor</h1>
      <Button onClick={(): void => onMinimizeClick()} label="_" />
      <Button onClick={(): void => onMaximizeClick()} label="[ ]" />
      <Button onClick={(): void => onCloseClick()} label="x" />
    </header>
  )
}

export default Header
