import DLF from './DLF/DLF'
import { checkCanRead } from './helpers/file'
;(async (): Promise<any> => {
  const fileName = process.argv[2] || ''

  try {
    await checkCanRead(fileName)
  } catch (e) {
    console.error(`can't access ${fileName} for reading`)
    return
  }

  const dlf = new DLF()
  await dlf.load(fileName)

  console.log(dlf)
})()
