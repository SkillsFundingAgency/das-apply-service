(function(global) {
    "use strict";

    var GOVUK = global.GOVUK || {};

    GOVUK.fileUpload = {
        uploadedFiles: document.querySelector(".js-uploaded-files"),
        errorSummary: document.querySelector(".js-error-summary"),
        currentErrors: document.querySelector(".js-validation-errors"),
        dropTarget: document.querySelector(".js-drop-target"),

        init: function(args) {
            var that = this;

            args["maxFilesErrorCallback"] = function(files, errorCount) {
                that.handleError("maxFiles", files, errorCount);
            };
            args["maxFileSizeErrorCallback"] = function(file, errorCount) {
                that.handleError("fileSize", file, errorCount);
            };
            args["fileTypeErrorCallback"] = function(file, errorCount) {
                that.handleError("fileType", file, errorCount);
            };

            var r = new Resumable(args);
            this.handleUpload(r, that);
        },

        clearErrors: function() {
            this.dropTarget.style.borderColor = "#dee0e2";
            this.currentErrors.innerText = "";
            this.errorSummary.style.display = "none";
        },

        handleError: function(errorType, files, errorCount) {
            // console.log(errorType);
            this.clearErrors();
            const errorMessage =
                errorType === "fileType"
                    ? "Upload must be a PDF"
                    : errorType === "maxFiles"
                    ? "You may only upload 1 file at once"
                    : errorType === "fileSize"
                    ? "File must be less than 10mb"
                    : errorType === "maxTotalFiles"
                    ? "You may only upload 1 file"
                    : null;
            var error = document.createElement("li");
            error.innerHTML = '<a href="#go-to-error">' + errorMessage + "</a>";
            this.dropTarget.style.borderColor = "#b10e1e";
            this.currentErrors.appendChild(error);
            this.errorSummary.style.display = "block";
            this.errorSummary.focus();
        },

        handleUpload: function(r, that) {
            r.assignDrop(that.dropTarget);

            var manualUploadLink = document.querySelector(".js-browse-link");
            r.assignBrowse(manualUploadLink);

            var uploadProgress = document.querySelector(".js-upload-progress");
            var uploadControls = document.querySelector(".js-upload-controls");
            var uploadedContainer = document.querySelector(
                ".js-uploaded-container"
            );

            var progressBar = new ProgressBar(uploadProgress);
            var totalFilesUploaded = that.uploadedFiles.children.length;

            r.on("fileAdded", function(file, event) {
                that.clearErrors();
                // console.log("maxFiles: ", r.opts.maxFiles);
                // console.log("maxTotalFiles: ", r.opts.query.maxTotalFiles);
                // console.log("totalFilesUploaded: ", totalFilesUploaded);

                // checkNumbeOfExistingFilesUploaded is not more than r.opts.query.maxTotalFiles
                totalFilesUploaded = that.uploadedFiles.children.length;
                if (totalFilesUploaded >= r.opts.query.maxTotalFiles) {
                    that.handleError("maxTotalFiles", file);
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
                    "/Filename/" +
                    file.fileName +
                    '/Section/Delete">Remove <span class="govuk-visually-hidden"> file</span></a></td>';
                that.uploadedFiles.appendChild(fileNameListItem);
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
        }
    };

    global.GOVUK = GOVUK;
})(window);
