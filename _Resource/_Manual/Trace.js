//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
window.Nalarium = window.Nalarium || {};
//+
//- Nalarium.Trace -//
Nalarium.Trace = {
    _enabled: false,
    //+ by default each browser uses its own console
    alwaysUseFirebug: false,
    forceFirebugLite: false,
    _counter: 0,
    _pureFirebugLogger: !!(typeof (console) != 'undefined' && typeof (console.log) != 'undefined'),
    _liteFirebugLogger: !!(typeof (firebug) != 'undefined' && typeof (firebug.d) != 'undefined' && typeof (firebug.d.console) != 'undefined'),
    _isIE: !!(window.attachEvent && !window.opera),
    _isMozilla: navigator.userAgent.indexOf('Gecko') > -1 && navigator.userAgent.indexOf('KHTML') == -1,
    _isWebKit: navigator.userAgent.indexOf('KHTML') > -1,
    _isOpera: !!window.opera
};

//- isSupported -//
Nalarium.Trace.isSupported = function() {
    return typeof (Nalarium.Trace._isMozilla || Nalarium.Trace._isWebKit || Nalarium.Trace._isOpera || window.debugService || window.console) != 'undefined'
};

//- enable -//
Nalarium.Trace.enable = function() {
    Nalarium.Trace._enabled = true;
    //+
    return Nalarium.Trace;
};

//- _firebugLogger -//
Nalarium.Trace._firebugLogger = function(text) {
    if (Nalarium.Trace._pureFirebugLogger == true && Nalarium.Trace.forceFirebugLite == false) {
        console.log(text);
    }
    else if (Nalarium.Trace._liteFirebugLogger == true) {
        firebug.d.console.log(text);
    }
};

//- addNewLine -//
Nalarium.Trace.addNewLine = function(count) {
    if (Nalarium.Trace._enabled == true && Nalarium.Trace.isSupported() == true) {
        var countNumber = isNaN(count) == true ? 1 : parseInt(count);
        for (var i = 0; i < countNumber; i++) {
            Nalarium.Trace.write('\n');
        }
    }
    //+
    return Nalarium.Trace;
};

//- writeLine -//
Nalarium.Trace.writeLine = function(text) {
    text = text || '';
    if (Nalarium.Trace._enabled == true && Nalarium.Trace.isSupported() == true) {
        Nalarium.Trace._counter++;
        if (!!window.debugService == false) {
            text = text + '\n';
        }
        Nalarium.Trace.write(Nalarium.Trace._counter + ':' + text);
    }
    //+
    return Nalarium.Trace;
};

//- writeLabeledLine -//
Nalarium.Trace.writeLabeledLine = function(label, text) {
    label = label || '';
    if (Nalarium.Trace._enabled == true && Nalarium.Trace.isSupported() == true) {
        Nalarium.Trace._counter++;
        Nalarium.Trace.write(Nalarium.Trace._counter + ':' + label + ' (' + text + ')\n');
    }
    //+
    return Nalarium.Trace;
};

//- write -//
Nalarium.Trace.write = function(text) {
    if (Nalarium.Trace._enabled == true && Nalarium.Trace.isSupported() == true) {
        if (Nalarium.Trace.alwaysUseFirebug == true) {
            if (text != '\n') {
                Nalarium.Trace._firebugLogger(text);
            }
        }
        else if (window.debugService) {
            window.debugService.trace(text);
        }
        else if (Nalarium.Trace._isMozilla == true) {
            dump(text);
        }
        else if (Nalarium.Trace._isWebKit == true) {
            if (text != '\n') {
                window.console.log(text);
            }
        }
        else if (Nalarium.Trace._isOpera == true) {
            opera.postError(text);
        }
    }
    //+
    return Nalarium.Trace;
};

//+
//- Nalarium.Trace.Buffer -//
Nalarium.Trace.Buffer = function() {
    this._stringBuilder = [];
    this._depth = 0;
    var that = this;

    //- $getIndent -//
    this.getIndent = function() {
        var indent = '';
        for (var i = 0; i < this._depth; i++) {
            indent += '    ';
        }
        //+
        return indent;
    }

    return {
        //- @beginSegment -//
        beginSegment: function(title) {
            title = title || '';
            that._stringBuilder.push(that.getIndent() + '++' + title);
            that._depth++;
            //+
            return this;
        },

        //- @endSegment -//
        endSegment: function(title) {
            title = title || '';
            that._depth--;
            that._stringBuilder.push(that.getIndent() + '--' + title);
            //+
            return this;
        },

        //- @write -//
        write: function(text) {
            text = text || '';
            if (typeof text == 'number' || typeof text == 'string') {
                that._stringBuilder.push(that.getIndent() + text);
            }
            else if (typeof m == 'object' && 'join' in m) {
                this._writeArray(text);
            }
            else {
                this._writeObject(text);
            }
            //+
            return this;
        },

        writeLabeledLine: function(label, text) {
            this.write(label + ':' + text);
            //+
            return this;
        },

        //- @writeNewLine -//
        writeNewLine: function() {
            this.write('\n');
            //+
            return this;
        },

        //- flush -//
        flush: function(useAlert) {
            useAlert = !!useAlert;
            var data = that._stringBuilder.join('\n');
            if (useAlert == true) {
                alert(data);
            }
            else {
                Nalarium.Trace.write(data + '\n');
            }
            that._stringBuilder = [];
            //+
            return this;
        },

        //+ internal
        _writeArray: function(array) {
            if (array && array.length > 0) {
                var first = true;
                var c = array.length;
                for (var n = 0; n < c; n++) {
                    if (array[n]) {
                        this.write('[');
                        that._depth++;
                        this.write(array[n]);
                        that._depth--;
                        first = false;
                        if (n + 1 < c) {
                            this.write('],');
                        }
                        else {
                            this.write(']');
                        }
                    }
                }
            }
            //+
            return this;
        },

        _writeObject: function(obj) {
            for (var o in obj) {
                if (obj[o] && typeof obj[o] != 'function') {
                    var m = obj[o];
                    if (typeof m == 'object' && 'join' in m) {
                        this.beginSegment(o + '[]');
                        this._writeArray(m);
                        this.endSegment(o + '[]');
                    }
                    else if (typeof obj[o] == 'number' || typeof obj[o] == 'string') {
                        this.write(o + ':' + m);
                    }
                    else {
                        this.beginSegment(o);
                        this._writeObject(m);
                        this.endSegment(o);
                    }
                }
            }
            //+
            return this;
        }
    }
};
