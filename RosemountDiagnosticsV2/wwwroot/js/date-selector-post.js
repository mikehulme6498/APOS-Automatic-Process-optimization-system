function postDateSelector(path, extraParams = null, method = "post") {
    var dateSelectorData = getDateSelectorData();
    const form = document.createElement('form');
    form.method = method;
    form.action = path;


    for (const key in dateSelectorData) {
        if (dateSelectorData.hasOwnProperty(key)) {
            const hiddenField = document.createElement('input');
            hiddenField.type = 'hidden';
            hiddenField.name = dateSelectorData[key].substring(0, dateSelectorData[key].indexOf('='));
            hiddenField.value = dateSelectorData[key].substring(dateSelectorData[key].indexOf('=') + 1);
            form.appendChild(hiddenField);
        }
    }

    for (const key in extraParams) {
        if (extraParams.hasOwnProperty(key)) {
            const hiddenfield = document.createElement('input');
            hiddenfield.type = 'hidden';
            hiddenfield.name = extraParams[key].substring(0, extraParams[key].indexOf('='));
            hiddenfield.value = extraParams[key].substring(extraParams[key].indexOf('=') + 1);
            form.appendChild(hiddenfield);
        }
    }
    document.body.appendChild(form);
    form.submit();
}


function getDateSelectorData() {
    var kvpairs = [];
    var form = document.getElementById('dateSelection');

    var timeFrame = $('input[name=TimeFrame]:checked').val();
    kvpairs.push("dateSelectorModal.TimeFrame=" + timeFrame);


    for (var i = 0; i < form.elements.length; i++) {
        var e = form.elements[i];

        if (form.elements[i].type != 'submit' && form.elements[i].type != 'radio') {
            kvpairs.push("dateSelectorModal." + encodeURIComponent(e.name) + "=" + encodeURIComponent(e.value));
        }
    }
    return kvpairs;
}