export const isString = (str: any): str is string => {
  return Object.prototype.toString.call(str) === '[object String]'
}
