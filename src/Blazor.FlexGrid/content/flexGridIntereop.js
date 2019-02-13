window.flexGrid = {
    showModal: function (modalName) {
        var modal = document.getElementById(modalName);
        modal.style.display = "block";

        this.flexGrid.showedModal.name = modalName;

        return true;
    },

    hideModal: function (modalName) {
        var modal = document.getElementById(modalName);
        modal.style.display = "none";

        return true;
    },

    showedModal: {
        name: "None"
    }
}

window.addEventListener("click", function (event) {
    var modal = document.getElementById(window.flexGrid.showedModal.name);
    if (event.target == modal) {
        modal.style.display = "none";
    }
});

    