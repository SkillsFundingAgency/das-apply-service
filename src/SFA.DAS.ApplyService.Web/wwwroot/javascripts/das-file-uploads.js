(function(global) {
    "use strict";

    var GOVUK = global.GOVUK || {};

    GOVUK.fileUpload = {
        init: function(args) {
            var r = new Resumable(args);
            this.handleUpload(r);
        },

        handleUpload: function(r) {
            var dropTarget = document.querySelector(".js-drop-target");
            r.assignDrop(dropTarget);
            r.assignBrowse(dropTarget);

            var manualUploadLink = document.querySelector(".js-browse-link");
            var uploadProgress = document.querySelector(".js-upload-progress");
            var uploadControls = document.querySelector(".js-upload-controls");
            var uploadedContainer = document.querySelector(
                ".js-uploaded-container"
            );
            var uploadedFiles = document.querySelector(".js-uploaded-files");

            var progressBar = new ProgressBar(uploadProgress);

            r.on("fileAdded", function(file, event) {
                // > 20mb
                if (file.file.size >= 2e7) {
                    // this.showError("fileSize", file.file.size);
                    console.error(
                        "Your PDF file(s) must be smaller than 20MB."
                    );
                    return false;
                }

                r.upload();
                progressBar.fileAdded();
            });

            r.on("fileSuccess", function(file, message) {
                progressBar.finish();
                var pageLink =
                    "/Application/" +
                    r.opts.query.applicationId +
                    "/Sequence/" +
                    r.opts.query.sequenceId +
                    "/Section/" +
                    r.opts.query.sectionId +
                    "/Page/" +
                    r.opts.query.page +
                    "/Question/" +
                    r.opts.query.questionId;
                var fileNameListItem = document.createElement("tr");
                fileNameListItem.className = "govuk-table__row";
                fileNameListItem.innerHTML =
                    '<td class="govuk-table__cell govuk-table__cell--break-word" scope="row"><a class="govuk-link" href="' +
                    pageLink +
                    "/" +
                    file.fileName +
                    '/Download" download>' +
                    file.fileName +
                    '</a></td><td class="govuk-table__cell govuk-table__cell--numeric"><a class="govuk-link" href="' +
                    pageLink +
                    '/Section/Delete">Remove</a></td>';
                uploadedFiles.appendChild(fileNameListItem);
                uploadedContainer.style.display = "block";
            });

            r.on("fileProgress", function(file, message) {
                progressBar.uploading(file.progress() * 100);
            });

            r.on("complete", function() {
                uploadProgress.style.display = "none";
                uploadControls.style.display = "block";
            });

            function ProgressBar(ele) {
                this.fileAdded = function() {
                    ele.style.display = "block";
                    uploadControls.style.display = "none";
                    ele.style.width = "0%";
                };

                this.uploading = function(progress) {
                    // console.log("uploading: " + Math.round(progress) + "%");
                    ele.style.width = progress + "%";
                };

                this.finish = function() {
                    ele.style.width = "100%";
                };
            }

            manualUploadLink.addEventListener("click", function(event) {
                event.preventDefault();
            });
        }
    };

    global.GOVUK = GOVUK;
})(window);
