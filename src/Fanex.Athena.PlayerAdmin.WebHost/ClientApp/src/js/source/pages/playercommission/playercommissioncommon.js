'use strict';
define([], function () {
    function setTopWindowValue(key, value) {
        try {
            top.window.postMessage({ isSetData: true, key, value }, "*");
        }
        catch (err) {
        }
    }

    function setParentWindowValue(key, value) {
        try {
            parent.window[key] = value;
        }
        catch (err) {
        }
    }

    function bindTopClickEvent() {
        $(top.window).bind("click", () => {
            closeDropdown();
        });

        top.window.$("#mainFrame > iframe").contents().click(() => {
            closeDropdown();
        });
    }

    function closeDropdown() {
        $("#ddl-customers").multiSelect("hideMe");
    }

    return {
        setTopWindowValue: setTopWindowValue,
        setParentWindowValue: setParentWindowValue,
        bindTopClickEvent: bindTopClickEvent
    };
});
