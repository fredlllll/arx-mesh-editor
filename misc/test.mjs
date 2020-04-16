import fs from 'fs'

const checkCanRead = pathName => {
  return fs.promises.access(pathName, fs.constants.R_OK)
}

;(async () => {
  const fileName = process.argv[2] || ''

  try {
    await checkCanRead(fileName)
    console.log(`${fileName} is readable!`)
  } catch(e) {
    console.error(`can't access ${fileName} for reading`)
  }
})()

// const buffer = await fs.promises.readFile(fileName)
