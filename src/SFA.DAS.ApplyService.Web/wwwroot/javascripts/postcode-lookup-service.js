(function(global) {
    "use strict";

    var GOVUK = global.GOVUK || {};

    GOVUK.addressLookup = {
        currentAddressData: null,
        populateResultsFn: null,

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

        fillAddress: function(address) {
            document.querySelector("#address-line-1").value = address.Line1;
            document.querySelector("#address-line-2").value = address.Line2;
            document.querySelector("#address-city").value = address.City;
            document.querySelector("#address-county").value = address.Province;
            document.querySelector("#address-postcode").value =
                address.PostalCode;
        },

        inputValueTemplate: function(result) {
            return result ? result.text : null;
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

accessibleAutocomplete({
    element: document.querySelector("#postcode-lookup"),
    id: "CD-03_address-lookup-QID",
    source: GOVUK.addressLookup.handleFindAndConfirm,
    onConfirm: GOVUK.addressLookup.handleFindAndConfirm,
    confirmOnBlur: false,
    autoselect: false,
    templates: {
        inputValue: GOVUK.addressLookup.inputValueTemplate,
        suggestion: GOVUK.addressLookup.suggestionTemplate
    }
});
