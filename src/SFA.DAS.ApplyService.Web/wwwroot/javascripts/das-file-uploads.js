var r = new Resumable({
  target: "/Upload/Chunks",
  chunkSize: 100000,
  query: {applicationId: "e19b46d9-fb7b-4294-b86b-cdcca785fc33",sequenceId: 1, sectionId: 3, page: 23, questionId: "FHA-01"}
});

var dropTarget = document.querySelector(".js-drop-target");
r.assignBrowse(dropTarget);
r.assignDrop(dropTarget);

var uploadProgress = document.querySelector(".js-upload-progress");
var uploadControls = document.querySelector(".js-upload-controls");
var uploadedContainer = document.querySelector(".js-uploaded-container");
var uploadedFiles = document.querySelector(".js-uploaded-files");

var progressBar = new ProgressBar(uploadProgress);

r.on("fileAdded", function(file, event) {
  r.upload();
  progressBar.fileAdded();
});

r.on("fileSuccess", function(file, message) {
  // console.debug("fileSuccess", file);

  progressBar.finish();
  // console.log(file.fileName);
  var fileNameListItem = document.createElement("tr");
  fileNameListItem.className = "govuk-table__row";
  fileNameListItem.innerHTML =
    '<th class="govuk-table__header" scope="row">' +
    file.fileName +
    '</th><td class="govuk-table__cell govuk-table__cell--numeric"><a class="js-remove-file" href=#">Remove</a></td>';
  uploadedFiles.appendChild(fileNameListItem);
  uploadedContainer.style.display = "block";

  // Remove file (from ui) when delete clicked
  var deleteButtons = document.querySelectorAll(".js-remove-file");
  deleteButtons.forEach(function(button) {
    button.addEventListener("click", function(event) {
      event.target.parentNode.parentNode.remove();
      r.removeFile(file);
    });
  });
});

r.on("fileProgress", function(file, message) {
  progressBar.uploading(file.progress() * 100);
});

r.on("complete", function() {
  // console.debug("complete");
  uploadProgress.style.display = "none";
  uploadControls.style.display = "block";
});

function ProgressBar(ele) {
  this.fileAdded = function() {
    // console.log("added");
    ele.style.display = "block";
    uploadControls.style.display = "none";
    ele.style.width = "0%";
  };

  this.uploading = function(progress) {
    console.log("uploading: " + Math.round(progress) + "%");
    ele.style.width = progress + "%";
  };

  this.finish = function() {
    // console.log("finished");
    ele.style.width = "100%";
  };
}

//r.on('pause', function(){
//    $('#pause-upload-btn').find('.glyphicon').removeClass('glyphicon-pause').addClass('glyphicon-play');
//});
// r.on('fileProgress', function(file){
//     console.debug('fileProgress', file);
// });
// r.on('fileAdded', function(file, event){
//     r.upload();
//     console.debug('fileAdded', event);
// });
// r.on('filesAdded', function(array){
//     r.upload();
//     console.debug('filesAdded', array);
// });
// r.on('fileRetry', function(file){
//     console.debug('fileRetry', file);
// });
// r.on('fileError', function(file, message){
//     console.debug('fileError', file, message);
// });
// r.on("fileSuccess", function(file) {
//   console.debug("fileSuccess", file);
// });
// r.on('uploadStart', function(){
//     console.debug('uploadStart');
// });
// r.on("complete", function() {
//   console.debug("complete");
// });
// r.on('progress', function(){
//     console.debug('progress');
//     console.log(r.progress())
// });
// r.on("error", function(message, file) {
//   console.debug("error", message, file);
// });
// r.on('pause', function(){
//     console.debug('pause');
// });
// r.on('cancel', function(){
//     console.debug('cancel');
// });
