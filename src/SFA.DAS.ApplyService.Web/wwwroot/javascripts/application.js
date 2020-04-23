// Application javascript
window.GOVUKFrontend.initAll();
window.DASFrontend.cookies.init();

(function(global) {
  "use strict";

  var GOVUK = global.GOVUK || {};

  GOVUK.checkAll = {
    checkAllContainer: document.querySelector(".js-check-all-container"),
    checkboxContainer: document.querySelector(".js-checkbox-container"),
    checkAllControl: document.querySelector(".js-check-all-control"),
    checkAllCheckbox: document.querySelectorAll(".js-check-all-checkbox"),

    numberChecked: function() {
      var trueCount = 0;
      for (
        var i = 0, length = GOVUK.checkAll.checkAllCheckbox.length;
        i < length;
        i++
      ) {
        if (GOVUK.checkAll.checkAllCheckbox[i].checked === true) {
          trueCount++;
        }
      }

      return trueCount;
    },

    setControl: function(indeterminate, checked) {
      GOVUK.checkAll.checkAllControl.indeterminate = indeterminate;
      if (checked !== undefined)
        GOVUK.checkAll.checkAllControl.checked = checked;
      if (indeterminate) {
        GOVUK.checkAll.checkAllControl.classList.add(
          "govuk-checkboxes__input--indeterminate"
        );
      } else {
        GOVUK.checkAll.checkAllControl.classList.remove(
          "govuk-checkboxes__input--indeterminate"
        );
      }
    },

    determineControlState: function() {
      if (
        GOVUK.checkAll.numberChecked() ===
        GOVUK.checkAll.checkAllCheckbox.length
      ) {
        GOVUK.checkAll.setControl(false, true);
      } else if (GOVUK.checkAll.numberChecked() === 0) {
        GOVUK.checkAll.setControl(false, false);
      } else if (!GOVUK.checkAll.checkAllControl.indeterminate) {
        GOVUK.checkAll.setControl(true);
      }
    },

    handleClick: function(event) {
      if (event.target.classList.contains("js-check-all-control")) {
        if (
          GOVUK.checkAll.checkAllControl.classList.contains(
            "govuk-checkboxes__input--indeterminate"
          )
        ) {
          GOVUK.checkAll.setControl(false);
        }

        for (
          var i = 0, length = GOVUK.checkAll.checkAllCheckbox.length;
          i < length;
          i++
        ) {
          GOVUK.checkAll.checkAllCheckbox[i].checked = event.target.checked;
        }
      } else if (event.target.classList.contains("js-check-all-checkbox")) {
        GOVUK.checkAll.determineControlState();
      } else {
        return false;
      }
    },

    init: function() {
      GOVUK.checkAll.checkboxContainer.classList.add("govuk-!-margin-left-8");
      GOVUK.checkAll.determineControlState();
      document.addEventListener("change", GOVUK.checkAll.handleClick);
    }
  };

  global.GOVUK = GOVUK;
})(window);
