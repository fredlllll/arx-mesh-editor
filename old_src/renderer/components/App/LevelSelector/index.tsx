import React, { ReactElement, useState, useEffect } from 'react'
import { isNil } from 'ramda'
import { remote } from 'electron'
import Button from '../Button'
import { NEW_LEVEL, LEVELS } from '../../../constants/LEVELS'
import s from './style.scss'

interface LevelSelectorProps {
  onArxRootChange: any // TODO: this should be a well defined function
  onSelect: any // TODO: this also should be a well defined function
}

const { dialog } = remote

const LevelSelector = (props: LevelSelectorProps): ReactElement<any> => {
  const { onArxRootChange, onSelect } = props

  const [selectedLevel, setSelectedLevel] = useState()
  const [arxRoot, setArxRoot] = useState('')

  useEffect(() => {
    if (arxRoot !== undefined) {
      onArxRootChange(arxRoot)
    }
  }, [arxRoot])

  return (
    <div id={s.LevelSelector}>
      Arx extracted resources path (click to change):
      <br />
      <input
        readOnly
        type="text"
        value={arxRoot}
        onClick={(): void => {
          dialog
            .showOpenDialog({
              title: 'Select root of Arx extracted resources',
              properties: ['openDirectory']
            })
            .then(result => {
              if (!result.canceled) {
                setArxRoot(result.filePaths[0])
              }
            })
        }}
        onChange={(e): void => {
          setArxRoot(e.target.value)
        }}
      />
      <hr />
      <ul>
        {Object.keys(LEVELS).map(name => (
          <li key={name}>
            <label>
              <input
                type="radio"
                value={name}
                checked={name === selectedLevel}
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
