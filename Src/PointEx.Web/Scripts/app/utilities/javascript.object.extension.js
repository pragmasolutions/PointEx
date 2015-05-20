//--------------------------------------------------------------------------------
//String Extensions
//--------------------------------------------------------------------------------
String.isNullOrEmpty = function (value) {
    return !(typeof value === "string" && value.length > 0);
};


String.format = function (text) {
    
    if (arguments.length <= 1) {
        return text;
    }
    
    var argCount = arguments.length - 2;
    for (var arg = 0; arg <= argCount; arg++) {
        text = text.replace(new RegExp("\\{" + arg + "\\}", "gi"), arguments[arg + 1]);
    }
    
    return text;
};


//--------------------------------------------------------------------------------
//Array Extensions
//--------------------------------------------------------------------------------
Array.prototype.removeIf = function (callback) {
    var i = this.length;
    while (i--) {
        if (callback(this[i], i)) {
            this.splice(i, 1);
        }
    }
};

/**
* Generates a GUID string, according to RFC4122 standards.
* @returns {String} The generated GUID.
* @example af8a8416-6e18-a307-bd9c-f2c947bbb3aa
* @author Slavik Meltser (slavik@meltser.info).
* @link http://slavik.meltser.info/?p=142
*/
function guid() {
    function _p8(s) {
        var p = (Math.random().toString(16) + "000000000").substr(2, 8);
        return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
    }
    return _p8() + _p8(true) + _p8(true) + _p8();
}