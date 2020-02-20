// used by the cookie banner component

(function(root) {
  "use strict";

  window.GOVUK = window.GOVUK || {};

  window.GOVUK.cookie = function(name, value, options) {
    if (typeof value !== "undefined") {
      if (value === false || value === null) {
        return window.GOVUK.setCookie(name, "", { days: -1 });
      } else {
        // Default expiry date of 30 days
        if (typeof options === "undefined") {
          options = { days: 30 };
        }
        return window.GOVUK.setCookie(name, value, options);
      }
    } else {
      return window.GOVUK.getCookie(name);
    }
  };

  window.GOVUK.setCookie = function(name, value, options) {
    if (typeof options === "undefined") {
      options = {};
    }
    var cookieString = name + "=" + value + "; path=/";
    if (options.days) {
      var date = new Date();
      date.setTime(date.getTime() + options.days * 24 * 60 * 60 * 1000);
      cookieString = cookieString + "; expires=" + date.toGMTString();
    }
    if (document.location.protocol === "https:") {
      cookieString = cookieString + "; Secure";
    }
    document.cookie = cookieString + ";domain=" + window.GOVUK.getDomain();
  };

  window.GOVUK.getCookie = function(name) {
    var nameEQ = name + "=";
    var cookies = document.cookie.split(";");
    for (var i = 0, len = cookies.length; i < len; i++) {
      var cookie = cookies[i];
      while (cookie.charAt(0) === " ") {
        cookie = cookie.substring(1, cookie.length);
      }
      if (cookie.indexOf(nameEQ) === 0) {
        return decodeURIComponent(cookie.substring(nameEQ.length));
      }
    }
    return null;
  };

  window.GOVUK.getDomain = function() {
    return window.location.hostname !== "localhost"
      ? "." +
          window.location.hostname.slice(
            window.location.hostname.indexOf(".") + 1
          )
      : window.location.hostname;
  };
})(window);
