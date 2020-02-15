import fs from 'fs'
import { BinaryIO } from './BinaryIO'
import { checkCanRead } from './helpers/file'

export class DLFLoader {
  public async load(fileName: string): Promise<any> {
    await checkCanRead(fileName)
    const buffer = await fs.promises.readFile(fileName)
    const binary = new BinaryIO(buffer.buffer)

    // Step 1: read header information
    // https://github.com/arx/ArxLibertatis/blob/master/plugins/blender/arx_addon/dataDlf.py#L34

    const data = {
      header: {
        version: binary.readFloat32(true),
        ident: binary.readString(16),
        lastuser: binary.readString(256),
        time: binary.readInt32(true)
      }
    }

    /*
      ("pos_edit",        SavedVec3),
      ("angle_edit",      SavedAnglef),
      ("nb_scn",          c_int32),
      ("nb_inter",        c_int32),
      ("nb_nodes",        c_int32),
      ("nb_nodeslinks",   c_int32),
      ("nb_zones",        c_int32),
      ("lighting",        c_int32),
      ("Bpad",            c_int32 * 256),
      ("nb_lights",       c_int32),
      ("nb_fogs",         c_int32),
      ("nb_bkgpolys",     c_int32),
      ("nb_ignoredpolys", c_int32),
      ("nb_childpolys",   c_int32),
      ("nb_paths",        c_int32),
      ("pad",             c_int32 * 250),
      ("offset",          SavedVec3),
      ("fpad",            c_float * 253),
      ("cpad",            c_char * 4096),
      ("bpad",            c_int32 * 256),
    */

    console.log(data)

    return fileName // TODO actually do something
  }
}
