window.theForm = document.forms['aspnetForm'];
if (!window.theForm) {
    window.theForm = document.aspnetForm;
}
if(!window.__doPostBack) {
    window.__doPostBack = function(eventTarget, eventArgument) {
        if (!window.theForm.onsubmit || (window.theForm.onsubmit() != false)) {
            window.theForm.__EVENTTARGET.value = eventTarget;
            window.theForm.__EVENTARGUMENT.value = eventArgument;
            window.theForm.submit();
        }
    }
}