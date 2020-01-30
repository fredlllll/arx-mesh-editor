import React, { ReactElement, MutableRefObject } from 'react'
import s from './style.scss'

interface ScreenProps {
  ref: MutableRefObject<any>
}

const Screen = (props: ScreenProps): ReactElement<any> => {
  const { ref } = props
  return <div id={s.Screen} ref={ref} />
}

export default Screen
