var r = new Resumable({
  target: "test.html"
});

var dropTarget = document.querySelector(".js-drop-target");
r.assignBrowse(dropTarget);
r.assignDrop(document.querySelector(".js-drop-target"));
var uploadProgress = document.querySelector(".js-upload-progress");
var uploadedFiles = document.querySelector(".js-uploaded-files");

var progressBar = new ProgressBar(uploadProgress);

r.on("fileAdded", function(file, event) {
  r.upload();
  progressBar.fileAdded();
});

r.on("fileSuccess", function(file, message) {
  console.log(uploadedFiles);

  progressBar.finish();
  console.log(file.fileName);
  var fileNameListItem = document.createElement("li");
  fileNameListItem.innerText = file.fileName;
  uploadedFiles.appendChild(fileNameListItem);
});

r.on("progress", function() {
  progressBar.uploading(r.progress() * 100);
  //$('#pause-upload-btn').find('.glyphicon').removeClass('glyphicon-play').addClass('glyphicon-pause');
});

//r.on('pause', function(){
//    $('#pause-upload-btn').find('.glyphicon').removeClass('glyphicon-pause').addClass('glyphicon-play');
//});

function ProgressBar(ele) {
  (this.fileAdded = function() {
    console.log("added");
    ele.classList.remove("hide");
    ele.style.width = "0%";
  }),
    (this.uploading = function(progress) {
      console.log("uploading: " + Math.round(progress) + "%");
      ele.style.width = progress + "%";
    }),
    (this.finish = function() {
      console.log("finished");
      ele.classList.add("hide");
      ele.style.width = "100%";
    });
}

r.on("fileSuccess", function(file) {
  console.debug("fileSuccess", file);
});
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
