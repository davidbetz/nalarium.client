//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
Namespace.create('Nalarium');
Nalarium.AjaxLoader = {
    //- get -//
    'get': function(params) {
        Nalarium.AjaxLoader._validate(params);
        //+
        Nalarium.AjaxLoader._invoke(params, 'get');
    },

    //- _validate -//
    _validate: function(params) {
        if (params) {
            if (!params.path) { throw 'path is required' }
            if (!params.onSuccess) { throw 'onSuccess is required' }
        }
        else {
            throw 'params is required';
        }
    },

    //- _invoke [abstract] -//
    _invoke: null
};