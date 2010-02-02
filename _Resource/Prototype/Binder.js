//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
Namespace.create('Nalarium.Prototype');
//+
//- Binder -//
Nalarium.Prototype.Binder = (function() {
    //- ctor -//
    function ctor(init) {
    }
    ctor.prototype = {
        //- doCallback -//
        apply: function() {
            //+ wcf
            Nalarium.WCF._invoke = function(params, method) {
                var _onSuccess = params.onSuccess;
                new Ajax.Request(params.endpoint + '/' + params.operation, Object.extend(params, {
                    parseJSON: params.parseJSON || true,
                    method: method,
                    postBody: Object.toJSON(params.message),
                    contentType: 'application/json',
                    onSuccess: function(r) {
                        if (typeof _onSuccess != undefined) {
                            if (!!params.parseJSON == true) {
                                if (r.responseJSON.d) {
                                    _onSuccess(r.responseJSON.d);
                                }
                                else {
                                    _onSuccess(r.responseJSON);
                                }
                            }
                            else {
                                _onSuccess(r);
                            }
                        }
                    },
                    onException: params.onException || Nalarium.WCF.globalFaultHandler || Prototype.emptyFunction,
                    onFailure: params.onFailure || Nalarium.WCF.globalFaultHandler || Prototype.emptyFunction
                }));
            };
            //+ loader
            Nalarium.AjaxLoader._invoke = function(params, method) {
                var _onSuccess = params.onSuccess;
                new Ajax.Request(params.path, Object.extend(params, {
                    parseJSON: params.parseJSON || false,
                    method: method,
                    postBody: Object.toJSON(params.message),
                    contentType: 'application/json',
                    onSuccess: function(r) {
                        if (typeof _onSuccess != undefined) {
                            if (!!params.parseJSON == true) {
                                _onSuccess(r.responseJSON.d);
                            }
                            else {
                                _onSuccess(r);
                            }
                        }
                    },
                    onException: params.onException || Nalarium.WCF.globalFaultHandler || Prototype.emptyFunction,
                    onFailure: params.onFailure || Nalarium.WCF.globalFaultHandler || Prototype.emptyFunction
                }));
            };
        }
    };
    //+
    return ctor;
})();
//+
new Nalarium.Prototype.Binder().apply();