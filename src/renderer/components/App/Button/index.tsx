import React, { ReactElement, MouseEventHandler } from 'react'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { IconProp } from '@fortawesome/fontawesome-svg-core'
import cn from 'classnames'
import s from './style.scss'

interface ButtonProps {
  label?: string
  onClick: MouseEventHandler
  id?: string
  className?: string
  disabled?: boolean
  icon?: IconProp
}

const Button = (props: ButtonProps): ReactElement<any> => {
  const { label, icon, onClick, className, ...rest } = props
  return (
    <button type="button" onClick={onClick} className={cn(className, s.Button)} {...rest}>
      {label && label}
      {icon && <FontAwesomeIcon icon={icon} />}
    </button>
  )
}

export default Button
