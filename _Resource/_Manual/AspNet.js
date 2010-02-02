//+ Nalarium Pro 3.0 - Client Module
//+ Copyright © Jampad Technology, Inc. 2008-2010
//+
//+ This file is a part of the Nalarium Suite.
//+ The use and distribution terms for this software are covered by the
//+ Microsoft Permissive License (Ms-PL) which can be found at
//+ http://www.microsoft.com/opensource/licenses.mspx.
Namespace.create('Nalarium');
//+
Nalarium.AspNet = {
    _objects: new Object( ), 

    registerObject: function(clientId, aspNetId, encapsulated) {
        if(!aspNetId) {
            aspNetId = clientId;
        }
        
        if((!!encapsulated) == true) {
            eval('Nalarium.AspNet._objects.' + clientId + ' = $(aspNetId)');
        }
        else {
            eval('window.' + clientId + ' = $(aspNetId)');
        }
    }
};