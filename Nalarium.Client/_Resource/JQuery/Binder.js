//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
Namespace.create('Nalarium.JQuery');
//+
//- Binder -//
Nalarium.JQuery.Binder = (function( ) {
    //- ctor -//
    function ctor(init) {
    }
    ctor.prototype = {
        //- doCallback -//
        apply: function( ) {
            //+ wcf
            Nalarium.WCF._invoke = function(params, verb) {
                var _onSuccess = params.onSuccess;
                $.ajax({
                    type: verb.toUpperCase(),
                    dataType: 'json',
                    url: params.endpoint + '/' + params.operation,
                    data: JSON.stringify(params.message) || '{}',
                    contentType: 'application/json',
                    success: function(r) {
                        if(typeof _onSuccess != 'undefined') {
                            _onSuccess(r);
                        }
                    },
                    error: params.onFailure || Nalarium.WCF.globalFaultHandler || function( ) { }
                });
            };
            //+ loader
            Nalarium.AjaxLoader._invoke = function(params, verb) {
                var _onSuccess = params.onSuccess;
                $.ajax({
                    type: verb.toUpperCase(),
                    dataType: 'json',
                    url: params.path,
                    data: $.toJSON(params.message) || '{}',
                    contentType: 'application/json',
                    success: function(r) {
                        if(typeof _onSuccess != 'undefined') {
                            _onSuccess(r);
                        }
                    },
                    error: params.onFailure || Nalarium.WCF.globalFaultHandler || function( ) { }
                });
            };
        }
    };
    //+
    return ctor;
})( );
//+
new Nalarium.JQuery.Binder().apply();