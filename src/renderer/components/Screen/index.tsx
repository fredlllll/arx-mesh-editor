import React, { forwardRef } from 'react'
import s from './style.scss'

const Screen = forwardRef<HTMLDivElement, {}>((props, ref) => {
  return <div id={s.Screen} ref={ref} {...props} />
})

export default Screen
