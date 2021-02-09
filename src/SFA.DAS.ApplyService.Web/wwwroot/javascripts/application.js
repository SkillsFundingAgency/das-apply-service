// Application javascript
window.GOVUKFrontend.initAll();
window.DASFrontend.cookies.init();

(function (global) {
  "use strict";

  var DASFrontend = global.DASFrontend || {};

  DASFrontend.checkAll = {
    checkAllContainer: document.querySelector(".js-check-all-container"),
    checkboxContainer: document.querySelector(".js-checkbox-container"),
    checkAllControl: document.querySelector(".js-check-all-control"),
    checkAllCheckbox: document.querySelectorAll(".js-check-all-checkbox"),

    numberChecked: function () {
      var trueCount = 0;
      for (
        var i = 0, length = DASFrontend.checkAll.checkAllCheckbox.length;
        i < length;
        i++
      ) {
        if (DASFrontend.checkAll.checkAllCheckbox[i].checked === true) {
          trueCount++;
        }
      }

      return trueCount;
    },

    setControl: function (indeterminate, checked) {
      DASFrontend.checkAll.checkAllControl.indeterminate = indeterminate;
      if (checked !== undefined)
        DASFrontend.checkAll.checkAllControl.checked = checked;
      if (indeterminate) {
        DASFrontend.checkAll.checkAllControl.classList.add(
          "govuk-checkboxes__input--indeterminate"
        );
      } else {
        DASFrontend.checkAll.checkAllControl.classList.remove(
          "govuk-checkboxes__input--indeterminate"
        );
      }
    },

    determineControlState: function () {
      if (
        DASFrontend.checkAll.numberChecked() ===
        DASFrontend.checkAll.checkAllCheckbox.length
      ) {
        DASFrontend.checkAll.setControl(false, true);
      } else if (DASFrontend.checkAll.numberChecked() === 0) {
        DASFrontend.checkAll.setControl(false, false);
      } else if (!DASFrontend.checkAll.checkAllControl.indeterminate) {
        DASFrontend.checkAll.setControl(true);
      }
    },

    handleClick: function (event) {
      if (event.target.classList.contains("js-check-all-control")) {
        if (
          DASFrontend.checkAll.checkAllControl.classList.contains(
            "govuk-checkboxes__input--indeterminate"
          )
        ) {
          DASFrontend.checkAll.setControl(false);
        }

        for (
          var i = 0, length = DASFrontend.checkAll.checkAllCheckbox.length;
          i < length;
          i++
        ) {
          DASFrontend.checkAll.checkAllCheckbox[i].checked =
            event.target.checked;
        }
      } else if (event.target.classList.contains("js-check-all-checkbox")) {
        DASFrontend.checkAll.determineControlState();
      } else {
        return false;
      }
    },

    init: function () {
      DASFrontend.checkAll.checkboxContainer.classList.add(
        "govuk-!-margin-left-8"
      );
      DASFrontend.checkAll.determineControlState();
      document.addEventListener("change", DASFrontend.checkAll.handleClick);
    },
  };

  DASFrontend.disableOnSubmit = {
    buttonsToDisable: document.querySelectorAll(
      "[data-disable-on-submit='true']"
    ),

    init: function () {
      if (!this.buttonsToDisable.length) return;
      document.addEventListener("click", this.disableButton);
    },

    disableButton: function (event) {
      if (event.target.type !== "submit") return;
      if (event.target.dataset.disableOnSubmit !== "true") return;

      var button = DASFrontend.disableOnSubmit.buttonsToDisable;

      setTimeout(function () {
        for (var i = 0; i < button.length; i++) {
          button[i].disabled = true;
        }
      }, 0);
    },
  };

  global.DASFrontend = DASFrontend;
})(window);

window.DASFrontend.disableOnSubmit.init();
