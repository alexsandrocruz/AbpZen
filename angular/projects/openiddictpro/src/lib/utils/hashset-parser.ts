const separatorRegex = /\r?\n/;
const separator = '\n';
export function hashSetParser(stringOrArray: string | Array<string>) {
  if (Array.isArray(stringOrArray)) {
    return stringOrArray.join(separator);
  }
  if (typeof stringOrArray === 'string') {
    if (!stringOrArray) {
      return null;
    }
    return stringOrArray.split(separatorRegex);
  }
  return stringOrArray;
}
