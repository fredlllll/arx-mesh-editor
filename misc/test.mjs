import fs from 'fs'
import { Transform } from 'stream'

const checkCanRead = pathName => {
  return fs.promises.access(pathName, fs.constants.R_OK)
}

class TestTransform extends Transform {
  _transform(chunk, encoding, next) {
    this.push(Buffer.from([65]))
    next()
  }
}

;(async () => {
  const fileName = process.argv[2] || ''

  try {
    await checkCanRead(fileName)
  } catch(e) {
    console.error(`can't access ${fileName} for reading`)
    return;
  }

  const reader = fs.createReadStream(fileName)
  const writer = fs.createWriteStream('e:/dummy')
  const transformer = new TestTransform()
  reader.pipe(transformer).pipe(writer)
})()
