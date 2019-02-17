window.flexGrid = {
    showModal: function (modalName) {
        $('#' + modalName).modal('show');
        this.flexGrid.showedModal.name = modalName;

        return true;
    },

    hideModal: function (modalName) {
        $('#' + modalName).modal('hide');

        return true;
    },

    showedModal: {
        name: "None"
    }
}

document.addEventListener('keydown', function (event) {
    const key = event.key;
    if (key === "Escape") {
        window.flexGrid.hideModal(window.flexGrid.showedModal.name);
    }    
});

    