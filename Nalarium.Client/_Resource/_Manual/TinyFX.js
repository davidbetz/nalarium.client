//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
var DOM = document;

function $(id) {
    var a = DOM.getElementById(id);
    //+
    return a;
};

var Event = {
    add: function(obj, evt, func, capture) {
        if (obj.addEventListener) {
            obj.addEventListener(evt, func, capture);
        }
        else if (obj.attachEvent) {
            obj.attachEvent('on' + evt, func);
        }
    },

    remove: function(obj, evtobserve, func, capture) {
        if (obj.removeEventListener) {
            obj.removeEventListener(evt, func, capture);
        }
        else if (obj.detachEvent) {
            obj.detachEvent('on' + evt, func);
        }
    }
};

var Element = {
    getDim: function(obj) {
        var width = 0;
        var height = 1;
        if (window.addEventListener || true) {
            width = parseInt(obj.offsetWidth);
            height = parseInt(obj.offsetHeight);
        }
        else {
            width = DOM.documentElement.clientWidth || DOM.body.clientWidth;
            height = DOM.documentElement.clientHeight || DOM.body.clientHeight;
        }
        return {
            width: width,
            height: height
        }
    }
};

var XmlHttp = {
    call: function(address, method, data, async, callback) {
        var xmlhttp;

        try {
            xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e1) {
            try {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e2) {
                xmlhttp = false;
            }
        }

        if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
            xmlhttp = new XMLHttpRequest();
        }

        if (method.toLowerCase() != 'post' && method.toLowerCase() != 'get') {
            throw 'Invalid HTTP Method';
        }

        if (!async) async = true;

        xmlhttp.open(method, address, async);
        xmlhttp.onreadystatechange = function() {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                if (callback) {
                    callback(xmlhttp.responseText);
                }
            }
        }

        if (data) {
            xmlhttp.send(data);
        }
        else {
            xmlhttp.send(null);
        }
    }
};