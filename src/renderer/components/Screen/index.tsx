import React, { ReactElement } from 'react'
import s from './style.scss'

const ScreenID = s.Screen

const Screen = (): ReactElement<any> => {
  return <div id={s.Screen} />
}

export default Screen

export { ScreenID }
