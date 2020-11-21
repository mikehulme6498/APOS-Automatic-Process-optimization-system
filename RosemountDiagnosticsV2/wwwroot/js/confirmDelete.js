function confirmDelete(uniqueId, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (isDeleteClicked) {
        $('#' + deleteSpan).css("visibility", "hidden");
        $('#' + confirmDeleteSpan).css("visibility", "visible");
    }
    else {
        $('#' + deleteSpan).css("visibility", "visible");
        $('#' + confirmDeleteSpan).css("visibility", "hidden");
    }
}