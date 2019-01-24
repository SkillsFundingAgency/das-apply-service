// Application javascript
window.GOVUKFrontend.initAll();

(function(global) {
  "use strict";

  var GOVUK = global.GOVUK || {};

  GOVUK.checkAll = {
    checkAllContainer: document.querySelector(".js-check-all-container"),
    checkboxContainer: document.querySelector(".js-checkbox-container"),
    checkAllControl: document.querySelector(".js-check-all-control"),
    checkAllCheckbox: document.querySelectorAll(".js-check-all-checkbox"),

    handleClick: function(event) {
      if (event.target.classList.contains("js-check-all-control")) {
        if (
          GOVUK.checkAll.checkAllControl.classList.contains(
            "govuk-checkboxes__input--indeterminate"
          )
        ) {
          GOVUK.checkAll.checkAllControl.indeterminate = false;
          GOVUK.checkAll.checkAllControl.classList.remove(
            "govuk-checkboxes__input--indeterminate"
          );
        }

        for (
          var i = 0, length = GOVUK.checkAll.checkAllCheckbox.length;
          i < length;
          i++
        ) {
          GOVUK.checkAll.checkAllCheckbox[i].checked = event.target.checked;
        }
      } else if (event.target.classList.contains("js-check-all-checkbox")) {
        if (!GOVUK.checkAll.checkAllControl.indeterminate) {
          GOVUK.checkAll.checkAllControl.indeterminate = true;
          GOVUK.checkAll.checkAllControl.classList.add(
            "govuk-checkboxes__input--indeterminate"
          );
        }
      } else {
        return false;
      }
    },

    init: function() {
      GOVUK.checkAll.checkboxContainer.classList.add("govuk-!-margin-left-8");
      document.addEventListener("change", GOVUK.checkAll.handleClick);
    }
  };

  global.GOVUK = GOVUK;
})(window);
