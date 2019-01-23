// Application javascript
window.GOVUKFrontend.initAll();

(function(global) {
  "use strict";

  var GOVUK = global.GOVUK || {};

  GOVUK.checkAll = {
    checkAllContainer: document.querySelector(".js-check-all-container"),
    checkAllCheckboxContainer: document.querySelector(
      ".js-check-all-checkbox-container"
    ),
    checkAllCheckbox: document.querySelectorAll(".js-check-all-checkbox"),

    addEvent: function(node, type, callback) {
      if (node.addEventListener) {
        node.addEventListener(
          type,
          function(e) {
            callback(e, e.target);
          },
          false
        );
      } else if (node.attachEvent) {
        node.attachEvent("on" + type, function(e) {
          callback(e, e.srcElement);
        });
      }
    },

    handleCheckAll: function(event) {
      if (event.target.id !== "allAreas") return false;
      for (
        var i = 0, length = GOVUK.checkAll.checkAllCheckbox.length;
        i < length;
        i++
      ) {
        GOVUK.checkAll.checkAllCheckbox[i].checked = event.target.checked;
      }
    },

    init: function() {
      GOVUK.checkAll.checkAllCheckboxContainer.classList.add(
        "govuk-!-margin-left-8"
      );
      GOVUK.checkAll.addEvent(
        document,
        "change",
        GOVUK.checkAll.handleCheckAll
      );
    }
  };

  global.GOVUK = GOVUK;
})(window);
