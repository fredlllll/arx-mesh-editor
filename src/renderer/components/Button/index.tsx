import React, { ReactElement, MouseEventHandler } from 'react'
import cn from 'classnames'
import s from './style.scss'

interface ButtonProps {
  label: string
  onClick: MouseEventHandler
  id?: string
  className?: string
}

const Button = (props: ButtonProps): ReactElement<any> => {
  const { label, onClick, className, ...rest } = props
  return (
    <button type="button" onClick={onClick} className={cn(className, s.Button)} {...rest}>
      {label}
    </button>
  )
}

export default Button
