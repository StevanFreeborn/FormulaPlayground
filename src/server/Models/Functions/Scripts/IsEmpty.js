function IsEmpty(value) {
  if (value == null || value == undefined) {
    return true;
  }

  if (typeof value === 'string') {
    return value.length === 0 || /[^\s]/.test(value) === false;
  }

  if (Array.isArray(value)) {
    return value.length === 0 || value.every(element => IsEmpty(element));
  }

  return false;
}