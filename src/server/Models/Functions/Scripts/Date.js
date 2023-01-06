Date = (function (JSDate) {
  var newDate = function () {
    if (!(this instanceof newDate) && arguments.length == 3) {
      return _Date(arguments[0], arguments[1], arguments[2]);
    }

    if (arguments.length == 0) {
      return new JSDate();
    }

    if (arguments.length == 1) {
      return new JSDate(arguments[0]);
    }

    if (arguments.length == 2) {
      return new JSDate(arguments[0], arguments[1]);
    }

    if (arguments.length == 3) {
      return new JSDate(arguments[0], arguments[1], arguments[2]);
    }
    if (arguments.length == 4) {
      return new JSDate(arguments[0], arguments[1], arguments[2], arguments[3]);
    }
    if (arguments.length == 5) {
      return new JSDate(
        arguments[0],
        arguments[1],
        arguments[2],
        arguments[3],
        arguments[4]
      );
    }

    if (arguments.length == 6) {
      return new JSDate(
        arguments[0],
        arguments[1],
        arguments[2],
        arguments[3],
        arguments[4],
        arguments[5]
      );
    }

    if (arguments.length == 7) {
      return new JSDate(
        arguments[0],
        arguments[1],
        arguments[2],
        arguments[3],
        arguments[4],
        arguments[5],
        arguments[6]
      );
    }
  };

  newDate.UTC = JSDate.UTC;
  newDate.now = JSDate.now;
  newDate.parse = JSDate.parse;
  JSDate.prototype.toString = function () {
    return FormatDate(this, arguments.length > 0 ? arguments[0] : "");
  };

  return newDate;
})(Date);
