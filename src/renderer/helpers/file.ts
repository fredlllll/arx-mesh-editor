import fs from 'fs'

const isExtension: (f: string, e: string) => boolean = (filename: string, extensionWithDot: string) => {
  return filename.toLowerCase().endsWith(extensionWithDot)
}

const isExtensionClosure: (e: string) => (f: string) => boolean = (extensionWithDot: string) => {
  return (filename: string) => {
    return isExtension(filename, extensionWithDot)
  }
}

const isDlf = isExtensionClosure('.dlf')

const isLlf = isExtensionClosure('.llf')

const isFts = isExtensionClosure('.fts')

/**
 * will fail if cant read
 * @param path
 */
const checkCanRead: (p: string) => Promise<void> = (path: string) => {
  return fs.promises.access(path, fs.constants.R_OK)
}

/**
 * will fail if cant write
 * @param path
 */
const checkCanWrite: (p: string) => Promise<void> = (path: string) => {
  return fs.promises.access(path, fs.constants.W_OK)
}

export { isDlf, isLlf, isFts, checkCanRead, checkCanWrite }
