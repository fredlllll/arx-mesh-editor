import React, { ReactElement } from 'react'
import cn from 'classnames'
import s from './style.scss'

interface ButtonProps {
  label: string
  onClick: any // TODO: I don't know what the hell I supposed to write here, MouseEvent is not enough
  id?: string
  className?: string
  // style?: ??? - string is not okay
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
