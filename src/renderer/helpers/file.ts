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

export { isDlf, isLlf, isFts }
