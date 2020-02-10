import React, { ReactElement, useState } from 'react'
import { isNil } from 'ramda'
import { remote } from 'electron'
import Button from '../Button'
import { NEW_LEVEL } from '../../../ArxLevel'
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

const { dialog } = remote

const LevelSelector = (props: LevelSelectorProps): ReactElement<any> => {
  const { onSelect } = props

  const [selectedLevel, setSelectedLevel] = useState()
  const [arxPath, setArxPath] = useState()

  return (
    <div id={s.LevelSelector}>
      Arx extracted resources path (click to change):
      <br />
      <input
        readOnly
        type="text"
        value={arxPath}
        onClick={(): void => {
          dialog
            .showOpenDialog({
              title: 'Select root of Arx extracted resources',
              properties: ['openDirectory']
            })
            .then(result => {
              if (!result.canceled) {
                setArxPath(result.filePaths[0])
              }
            })
        }}
        onChange={(e): void => {
          setArxPath(e.target.value)
        }}
      />
      <hr />
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
      <div>----- or -----</div>
      <Button
        label="Create a new level"
        onClick={(): void => {
          onSelect(NEW_LEVEL)
        }}
      />
    </div>
  )
}

export default LevelSelector
