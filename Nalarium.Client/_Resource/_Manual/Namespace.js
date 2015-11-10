//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
Namespace = window.Namespace || { };
Namespace.create = function(ns, separator) {
    var parts = ns.split(separator || '.');
    var nsObj = window;
    var count = parts.length;
    for(var i = 0; i <count; i++) {
        nsObj[parts[i]] = nsObj[parts[i]] || {};
        nsObj = nsObj[parts[i]];
    }
};
//+
Enum = Namespace;