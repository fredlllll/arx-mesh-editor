const isDlf = (filename: string): boolean => filename.toLowerCase().endsWith('.dlf')

const isLlf = (filename: string): boolean => filename.toLowerCase().endsWith('.llf')

const isFts = (filename: string): boolean => filename.toLowerCase().endsWith('.fts')

export { isDlf, isLlf, isFts }
