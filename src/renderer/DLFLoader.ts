/* global alert */

import fs from 'fs'

const tryAccessing = (fileName: string): Promise<void> => {
  // .then() is available, when file exists, else .catch()
  return fs.promises.access(fileName, fs.constants.R_OK)
}

export class DLFLoader {
  public async load(fileName: string): Promise<any> {
    await tryAccessing(fileName)
    const buffer = await fs.promises.readFile(fileName)
    const bufferView = new DataView(buffer)

    // Step 1: read header information
    // https://github.com/arx/ArxLibertatis/blob/master/plugins/blender/arx_addon/dataDlf.py#L34

    const data = {
      header: {
        version: bufferView.getFloat32(0)
      }
    }

    alert(JSON.stringify(data)) // TODO: really need to make the console accessible asap!

    return fileName // TODO actually do something
  }
}
