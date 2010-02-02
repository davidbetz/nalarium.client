//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
Namespace.create('Nalarium');
//+
Nalarium.WCF = {
    //- post -//
    post: function(params) {
        ///+ params: {
        ///+  endpoint,
        ///+  operation
        ///+ }
        Nalarium.WCF._validate(params);
        if (!params.message) { throw 'message is required' }
        //+
        Nalarium.WCF._invoke(params, 'post');
    },

    //- get -//
    'get': function(params) {
        Nalarium.WCF._validate(params);
        //+
        Nalarium.WCF._invoke(params, 'get');
    },

    //- delete -//
    'delete': function(params) {
        Nalarium.WCF._validate(params);
        //+
        Nalarium.WCF._invoke(params, 'delete');
    },

    //- _validate -//
    _validate: function(params) {
        if (params) {
            if (!params.endpoint) { throw 'endpoint is required' }
            if (!params.operation) { throw 'operation is required' }
        }
        else {
            throw 'params is required';
        }
    },

    //- _invoke [abstract] -//
    _invoke: null
};