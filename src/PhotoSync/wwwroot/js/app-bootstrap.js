let appBootstrap = {
    modal: {
        open: function (elementId) {
            let element = document.getElementById(elementId);
            let modal = bootstrap.Modal.getOrCreateInstance(element, { keyboard: false, backdrop: 'static'});
            modal.show();
        },
        close: function (elementId) {
            let element = document.getElementById(elementId);
            let modal = bootstrap.Modal.getOrCreateInstance(element);
            modal.hide();
        },
        init: function (elementId, backdrop, focus, keyboard) {
            let element = document.getElementById(elementId);
            let modal = bootstrap.Modal.getOrCreateInstance(element);
            modal.dispose();
            modal = new bootstrap.Modal(element, { backdrop: backdrop, focus: focus, keyboard: keyboard });
        },
        getOrCreate: function (elementId) {
            let element = document.getElementById(elementId);
            return bootstrap.Modal.getOrCreateInstance(element);
        }
    }
};

export { appBootstrap as default };