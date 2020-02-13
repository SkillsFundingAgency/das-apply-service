(function(global) {
  "use strict";

  var DASFrontend = global.DASFrontend || {};

  /*
    Cookie methods (using alphagov cookie code)
    ===========================================

    Usage:
      Setting a cookie:
      DASFrontend.cookie('hobnob', 'tasty', { days: 30 });

      Reading a cookie:
      DASFrontend.cookie('hobnob');

      Deleting a cookie:
      DASFrontend.cookie('hobnob', null);
  */

  DASFrontend.cookie = function(name, value, options) {
    if (typeof value !== "undefined") {
      if (value === false || value === null) {
        return DASFrontend.setCookie(name, "", { days: -1 });
      } else {
        return DASFrontend.setCookie(name, value, options);
      }
    } else {
      return DASFrontend.getCookie(name);
    }
  };

  DASFrontend.setCookie = function(name, value, options) {
    if (typeof options === "undefined") {
      options = {};
    }
    var cookieString = name + "=" + value + "; path=/";
    if (options.days) {
      var date = new Date();
      date.setTime(date.getTime() + options.days * 24 * 60 * 60 * 1000);
      cookieString = cookieString + "; expires=" + date.toGMTString();
    }
    if (document.location.protocol == "https:") {
      cookieString = cookieString + "; Secure";
    }
    document.cookie = cookieString;
  };

  DASFrontend.getCookie = function(name) {
    var nameEQ = name + "=";
    var cookies = document.cookie.split(";");
    for (var i = 0, len = cookies.length; i < len; i++) {
      var cookie = cookies[i];
      while (cookie.charAt(0) == " ") {
        cookie = cookie.substring(1, cookie.length);
      }
      if (cookie.indexOf(nameEQ) === 0) {
        return decodeURIComponent(cookie.substring(nameEQ.length));
      }
    }
    return null;
  };

  DASFrontend.addCookieMessage = function() {
    var message = document.querySelector(".js-cookie-banner"),
      hasCookieMessage =
        message && DASFrontend.cookie("seen_cookie_message") === null;

    if (hasCookieMessage) {
      message.style.display = "block";
      DASFrontend.cookie("seen_cookie_message", "yes", { days: 28 });
    }
  };

  global.DASFrontend = DASFrontend;
})(window);
