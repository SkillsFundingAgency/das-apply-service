(function(global) {
    "use strict";

    var GOVUK = global.GOVUK || {};

    GOVUK.fileUpload = {
        uploadedFiles: document.querySelector(".js-uploaded-files"),
        errorSummary: document.querySelector(".js-error-summary"),
        currentErrors: document.querySelector(".js-validation-errors"),
        dropTarget: document.querySelector(".js-drop-target"),
        jsExistingFiles: [],

        init: function(args) {
            
            var that = this;

            var hintText = document.querySelector(".govuk-hint");
            hintText.innerText = hintText.innerText.replace(/5MB/gi, "10MB");

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
            this.clearErrors();
            const errorMessage =
                errorType === "fileType"
                    ? "Upload must be a PDF"
                    : errorType === "maxFiles"
                    ? "You have exceeded the maximum number of files"
                    : errorType === "fileSize"
                    ? "File must be less than 10MB"
                    : errorType === "maxTotalFiles"
                    ? "You have exceeded the maximum number of files"
                    : errorType === "sameFilename"
                    ? "Files must not have the same filename"
                    : null;
            var error = document.createElement("li");
            error.innerHTML = '<a href="#choose-file">' + errorMessage + "</a>";
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
            var dropText = document.querySelector(".js-drop-text");
            var uploadsTable = document.querySelector(".js-file-upload-table");
            var progressBar = new ProgressBar(uploadProgress);

            hideControls(
                that.uploadedFiles.children.length,
                r.opts.maxTotalFiles
            );

            r.on("fileAdded", function(file, event) {

                var existingFileNames = r.opts.existingFiles.map(function(existingFile) {
                    return existingFile.filename
                })

                if (existingFileNames.indexOf(file.fileName) !== -1) {
                    that.handleError("sameFilename");
                    r.cancel();
                    return false;
                }

                that.clearErrors();
                r.upload();
                progressBar.fileAdded();
            });

            r.on("filesAdded", function(_, arraySkipped) {
                var skippedFiles = arraySkipped.map(function(skippedFile) {
                    return skippedFile.name;
                });
                var duplicateFiles = that.jsExistingFiles.some(function(file) {
                    return skippedFiles.indexOf(file) !== -1;
                });

                if (duplicateFiles) {
                    that.handleError("sameFilename");
                    r.cancel();
                    return false;
                }
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
                that.jsExistingFiles.push(file.fileName);
                uploadsTable.classList.remove("govuk-visually-hidden");
            });

            r.on("fileProgress", function(file, message) {
                // submitButton.disabled = true;
                progressBar.uploading(file.progress() * 100);
            });

            r.on("complete", function() {
                hideControls(
                    that.uploadedFiles.children.length,
                    r.opts.maxTotalFiles
                );
                // submitButton.disabled = false;
                uploadProgress.style.display = "none";
                dropText.style.display = "block";
            });

            function ProgressBar(ele) {
                this.fileAdded = function() {
                    ele.style.display = "block";
                    dropText.style.display = "none";
                    ele.style.width = "0%";
                };

                this.uploading = function(progress) {
                    ele.style.width = progress + "%";
                };

                this.finish = function() {
                    ele.style.width = "100%";
                };
            }

            function hideControls(filesUploaded, maxTotal) {
                if (filesUploaded >= maxTotal) {
                    that.dropTarget.style.display = "none";
                    manualUploadLink.parentElement.style.display = "none";
                }
            }
        }
    };

    global.GOVUK = GOVUK;
})(window);
