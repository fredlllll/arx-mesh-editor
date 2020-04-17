import DLFLoader from './DLF/DLFLoader'
import { checkCanRead } from './helpers/file'
;(async (): Promise<any> => {
  const fileName = process.argv[2] || ''

  try {
    await checkCanRead(fileName)
  } catch (e) {
    console.error(`can't access ${fileName} for reading`)
    return
  }

  const dlfLoader = new DLFLoader()
  const dlfData = await dlfLoader.load(fileName)
  console.log(dlfData)
})()
