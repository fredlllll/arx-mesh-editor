import React, { ReactElement, useState } from 'react'
import { isNil } from 'ramda'
import Button from '../../Button'
import s from './style.scss'

const levels = [
  {
    name: 'City of Arx',
    filename: 'level11'
  }
]

interface LevelSelectorProps {
  onSelect: any
}

const LevelSelector = (props: LevelSelectorProps): ReactElement<any> => {
  const { onSelect } = props

  const [selectedLevel, setSelectedLevel] = useState()
  return (
    <div id={s.LevelSelector}>
      <ul>
        {levels.map(({ name, filename }) => (
          <li key={filename}>
            <label>
              <input
                type="radio"
                value={filename}
                checked={filename === selectedLevel}
                onChange={(e): void => {
                  setSelectedLevel(e.target.value)
                }}
              />{' '}
              {name}
            </label>
          </li>
        ))}
      </ul>
      <Button
        disabled={isNil(selectedLevel)}
        label="Open level"
        onClick={(): void => {
          onSelect(selectedLevel)
        }}
      />
    </div>
  )
}

export default LevelSelector
