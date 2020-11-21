let dropArea = document.getElementById('drop-area');
let filesDone = 0;
let filesToDo = 0;
let totalFiles = 0;
let progressBar = document.getElementById('progress-bar');
var allData2 = [];
var fileList = document.getElementById('files');
var submitButton = document.getElementById('fileSubmit');

submitButton.style.visibility = "hidden";
progressBar.style.visibility = "hidden";

['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    dropArea.addEventListener(eventName, preventDefaults, false)
})

function preventDefaults(e) {
    e.preventDefault()
    e.stopPropagation()
}

['dragenter', 'dragover'].forEach(eventName => {
    dropArea.addEventListener(eventName, highlight, false)
});

['dragleave', 'drop'].forEach(eventName => {
    dropArea.addEventListener(eventName, unhighlight, false)
});

function highlight(e) {
    dropArea.classList.add('highlight')
}

function unhighlight(e) {
    dropArea.classList.remove('highlight')
}

dropArea.addEventListener('drop', handleDrop, false)

function handleDrop(e) {
    let dt = e.dataTransfer
    let files = dt.files
    progressBar.style.visibility = "visible";
    submitButton.style.visibility = "hidden";
    handleFiles(files)
}

function handleFiles(files) {
files = [...files];
    initializeProgress(files.length);
    files.forEach(uploadFile);
}

function uploadFile(file) {

    var FR = new FileReader();

    FR.addEventListener("load", function (e) {

        var reportText = e.target.result;
        reportText = reportText + "Filename : " + FR.fileName + "\r\nNEWREPORT\r\n";

        var reportTextArray = reportText.split("\n");

        allData2 = allData2.concat(reportTextArray);
        progressDone();
    });

    FR.fileName = file.name;
    FR.readAsText(file, "UTF-8")
}

function initializeProgress(numfiles) {
progressBar.value = 0
    filesDone = 0
    filesToDo = numfiles
}

function progressDone() {
    filesDone++
    totalFiles++
    progressBar.value = filesDone / filesToDo * 100

    if (progressBar.value == 100) {
        allData2[allData2.length] = "";
        document.getElementById("textfromAllfiles").setAttribute('value', allData2);
        fileList.innerText = totalFiles + " files added"
        progressBar.style.visibility = "hidden";
        submitButton.style.visibility = "visible";
    }
}