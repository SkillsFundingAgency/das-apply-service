(function(global) {
    "use strict";

    var GOVUK = global.GOVUK || {};

    GOVUK.addressLookup = {
        addressInputId: null,
        currentAddressData: null,
        populateResultsFn: null,

        init: function(elementId) {
            this.addressInputId = elementId;
            // Check if we already have address data and show inputs if so.
            if (this.addressExists(elementId)) {
                this.displayAddressLookup("hide");
            } else {
                this.displayAddressLookup("show");
            }
        },

        displayAddressLookup: function(hideOrShow) {
            if (hideOrShow === "hide") {
                document.querySelector(".address-inputs").style.display =
                    "block";
                document.querySelector(
                    ".govuk-label--address-lookup"
                ).style.display = "none";
                document.querySelector(
                    "#postcode-lookup"
                ).parentNode.style.display = "none";
            } else if (hideOrShow === "show") {
                document.querySelector(".address-inputs").style.display =
                    "none";
                document.querySelector(
                    ".govuk-label--address-lookup"
                ).style.display = "block";
                document.querySelector(
                    "#postcode-lookup"
                ).parentNode.style.display = "block";
            } else {
                return false;
            }
        },

        handleFindAndConfirm: function(query, populateResults) {
            if (populateResults) {
                // Data being updated on keypress
                GOVUK.addressLookup.populateResultsFn = populateResults;
                GOVUK.addressLookup.getPcaData(
                    "Find",
                    query,
                    "",
                    populateResults
                );
            } else {
                // Selection confirmed
                const confirmedAddressData = GOVUK.addressLookup.currentAddressData.Items.filter(
                    address => address.Text.indexOf(query.text) !== -1
                )[0];

                GOVUK.addressLookup.getPcaData(
                    confirmedAddressData.Next,
                    confirmedAddressData.Id,
                    confirmedAddressData.Id,
                    GOVUK.addressLookup.populateResultsFn
                );
            }
        },

        getPcaData: function(requestType, searchTerm, lastId, populateResults) {
            var url =
                "//services.postcodeanywhere.co.uk/CapturePlus/Interactive/" +
                requestType +
                "/v2.10/json3.ws";
            var params = "";
            params += "&key=" + encodeURIComponent("JY37-NM56-JA37-WT99");

            if (requestType === "Find") {
                params += "&searchTerm=" + encodeURIComponent(searchTerm);
                params += "&lastId=" + encodeURIComponent(lastId);
                params += "&country=" + encodeURIComponent("GB");
                params += "&language=" + encodeURIComponent("en-gb");
            }

            if (requestType === "Retrieve") {
                params += "&Id=" + encodeURIComponent(lastId);
            }

            var http = new XMLHttpRequest();
            http.open("POST", url, true);
            http.setRequestHeader(
                "Content-type",
                "application/x-www-form-urlencoded"
            );
            http.onreadystatechange = function() {
                if (http.readyState == 4 && http.status == 200) {
                    var response = JSON.parse(http.responseText);
                    // Test for an error
                    if (
                        response.Items.length == 1 &&
                        typeof response.Items[0].Error != "undefined"
                    ) {
                        // Show the error message
                        alert(response.Items[0].Description);
                    } else {
                        // Check if there were any items found
                        if (response.Items.length == 0)
                            alert("Sorry, there were no results");
                        else {
                            if (requestType === "Find") {
                                if (response.Items.length) {
                                    GOVUK.addressLookup.currentAddressData = response;
                                    const results = response.Items.map(
                                        result => ({
                                            text: result.Text,
                                            description: result.Description
                                        })
                                    );

                                    populateResults(results);
                                }
                            }

                            if (requestType === "Retrieve") {
                                // We've found the separate address parts.
                                if (response.Items.length) {
                                    GOVUK.addressLookup.fillAddress(
                                        response.Items[0]
                                    );
                                }
                            }
                        }
                    }
                }
            };
            http.send(params);
        },

        addressExists(elementId) {
            // If first line of address, city and postcode are populated
            if (
                document.querySelector("#" + elementId + "_" + 1).value &&
                document.querySelector("#" + elementId + "_" + 4).value &&
                document.querySelector("#" + elementId + "_" + 5).value
            ) {
                return true;
            } else {
                return false;
            }
        },

        fillAddress: function(address) {
            var addressArr = [
                address.Line1,
                address.Line2,
                address.City,
                address.Province,
                address.PostalCode
            ];
            var panelContent = "";

            // Populate (hidden) inputs and panel content
            addressArr.forEach(function(item, index) {
                document.querySelector(
                    "#" + GOVUK.addressLookup.addressInputId + "_" + (index + 1)
                ).value = item;
                panelContent +=
                    '<p class="govuk-body govuk-!-margin-bottom-0">' +
                    item +
                    "</p>";
            });

            // Clear lookup input
            document.querySelector(
                "#" + GOVUK.addressLookup.addressInputId
            ).value = "";

            // Hide label and empty postcode lookup div
            document.querySelector(".govuk-label--address-lookup").innerText =
                "Address";
            document.querySelector("#postcode-lookup").innerText = "";

            // Create panel and populate with address
            var addressPanel = document.createElement("div");
            addressPanel.className = "govuk-inset-text";
            addressPanel.innerHTML = panelContent;
            document
                .querySelector("#postcode-lookup")
                .appendChild(addressPanel);

            // Create link for editing
            var editLink = document.createElement("a");
            editLink.className = "govuk-link";
            editLink.href = "#edit-address";
            editLink.innerHTML = "Edit address";
            editLink.addEventListener("click", GOVUK.addressLookup.editAddress);
            document.querySelector("#postcode-lookup").appendChild(editLink);
            var editLinkWrapper = document.createElement("p");
            editLinkWrapper.className = "govuk-body";
            editLink.parentNode.insertBefore(editLinkWrapper, editLink);
            editLinkWrapper.appendChild(editLink);
        },

        editAddress: function() {
            GOVUK.addressLookup.displayAddressLookup("hide");
        },

        inputValueTemplate: function(result) {
            if (!result) return;
            return result.text;
        },
        suggestionTemplate: function(result) {
            return result.description
                ? result.text +
                      ' <span class="govuk-!-font-size-14">(' +
                      result.description +
                      ")</span>"
                : result.text;
        }
    };

    global.GOVUK = GOVUK;
})(window);
