// _cookies.js

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
    if (options.sameSite) {
      cookieString = cookieString + "; SameSite=" + options.sameSite;
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
      ? window.location.hostname.slice(
          window.location.hostname.indexOf(".") + 1
        )
      : window.location.hostname;
  };
})(window);

// _cookieBanner.js

function CookieBanner(module) {
  this.module = module;
  this.settings = {
    seenCookieName: "DASSeenCookieMessage",
    cookiePolicy: {
      AnalyticsConsent: true,
      MarketingConsent: false
    }
  };
  if (!window.GOVUK.cookie(this.settings.seenCookieName)) {
    this.start();
  }
}

CookieBanner.prototype.start = function() {
  this.module.cookieBanner = this.module.querySelector(".das-cookie-banner");
  this.module.cookieBannerInnerWrap = this.module.querySelector(
    ".das-cookie-banner__wrapper"
  );
  this.module.cookieBannerConfirmationMessage = this.module.querySelector(
    ".das-cookie-banner__confirmation"
  );
  this.setupCookieMessage();
};

CookieBanner.prototype.setupCookieMessage = function() {
  this.module.hideLink = this.module.querySelector(
    "button[data-hide-cookie-banner]"
  );

  this.module.acceptCookiesButton = this.module.querySelector(
    "button[data-accept-cookies]"
  );

  if (this.module.hideLink) {
    this.module.hideLink.addEventListener(
      "click",
      this.hideCookieBanner.bind(this)
    );
  }

  if (this.module.acceptCookiesButton) {
    this.module.acceptCookiesButton.addEventListener(
      "click",
      this.acceptAllCookies.bind(this)
    );
  }
  this.showCookieBanner();
};

CookieBanner.prototype.showCookieBanner = function() {
  var cookiePolicy = this.settings.cookiePolicy;
  this.module.cookieBanner.style.display = "block";
  Object.keys(cookiePolicy).forEach(function(cookieName) {
    window.GOVUK.cookie(cookieName, cookiePolicy[cookieName].toString(), {
      days: 365,
      sameSite: "None"
    });
  });
};

CookieBanner.prototype.hideCookieBanner = function() {
  this.module.cookieBanner.style.display = "none";
  window.GOVUK.cookie(this.settings.seenCookieName, true, {
    days: 365,
    sameSite: "None"
  });
};

CookieBanner.prototype.acceptAllCookies = function() {
  this.module.cookieBannerInnerWrap.style.display = "none";
  this.module.cookieBannerConfirmationMessage.style.display = "block";

  window.GOVUK.cookie(this.settings.seenCookieName, true, {
    days: 365,
    sameSite: "None"
  });

  Object.keys(this.settings.cookiePolicy).forEach(function(cookieName) {
    window.GOVUK.cookie(cookieName, true, { days: 365, sameSite: "None" });
  });
};

// _cookieSettings.js

function CookieSettings(module, options) {
  this.module = module;
  this.settings = {
    seenCookieName: "DASSeenCookieMessage",
    cookiePolicy: {
      AnalyticsConsent: true,
      MarketingConsent: false
    },
    isModal: options === "modal"
  };

  if (!this.settings.isModal) {
    // Hide cookie banner on settings page
    var cookieBanner = document.querySelector(".das-cookie-banner");
    cookieBanner.style.display = "none";
  }

  if (this.settings.isModal) {
    // Hide cookie settings if modal option is set
    this.hideCookieSettings();
    this.modalControls();
  }

  this.start();
}

CookieSettings.prototype.start = function() {
  this.setRadioValues();
  this.module.addEventListener("submit", this.formSubmitted.bind(this));
};

CookieSettings.prototype.setRadioValues = function() {
  var cookiePolicy = this.settings.cookiePolicy;

  Object.keys(cookiePolicy).forEach(function(cookieName) {
    var existingCookie = window.GOVUK.cookie(cookieName),
      radioButtonValue =
        existingCookie !== null ? existingCookie : cookiePolicy[cookieName],
      radioButton = document.querySelector(
        "input[name=cookies-" +
          cookieName +
          "][value=" +
          (radioButtonValue === "true" ? "on" : "off") +
          "]"
      );

    radioButton.checked = true;
  });
};

CookieSettings.prototype.formSubmitted = function(event) {
  event.preventDefault();

  var formInputs = event.target.getElementsByTagName("input"),
    button = event.target.getElementsByTagName("button");

  for (var i = 0; i < formInputs.length; i++) {
    var input = formInputs[i];
    if (input.checked) {
      var name = input.name.replace("cookies-", "");
      var value = input.value === "on";
      window.GOVUK.setCookie(name, value, { days: 365, sameSite: "None" });
    }
  }

  window.GOVUK.setCookie(this.settings.seenCookieName, true, {
    days: 365,
    sameSite: "None"
  });

  if (button.length > 0) {
    button[0].removeAttribute("disabled");
  }

  if (this.settings.isModal) {
    document.location.href = document.location.pathname;
  }

  if (!this.settings.isModal) {
    this.showConfirmationMessage();
  }
};

CookieSettings.prototype.showConfirmationMessage = function() {
  var confirmationMessage = document.querySelector(
    "div[data-cookie-confirmation]"
  );
  var previousPageLink = document.querySelector(
    ".das-cookie-settings__prev-page"
  );
  var referrer = CookieSettings.prototype.getReferrerLink();

  document.body.scrollTop = document.documentElement.scrollTop = 0;

  if (referrer && referrer !== document.location.pathname) {
    previousPageLink.href = referrer;
    previousPageLink.style.display = "inline-block";
  } else {
    previousPageLink.style.display = "none";
  }

  confirmationMessage.style.display = "block";
};

CookieSettings.prototype.getReferrerLink = function() {
  return document.referrer ? new URL(document.referrer).pathname : false;
};

CookieSettings.prototype.hideCookieSettings = function() {
  document.getElementById("cookie-settings").style.display = "none";
};

CookieSettings.prototype.modalControls = function() {
  var closeLink = document.createElement("a");
  //var closeLinkText = document.createTextNode("Close cookie settings");
  var closeIcon = htmlToElement(
    '<svg height="14px" version="1.1" viewBox="0 0 14 14" width="14px" xmlns="http://www.w3.org/2000/svg" xmlns:sketch="http://www.bohemiancoding.com/sketch/ns" xmlns:xlink="http://www.w3.org/1999/xlink"><title/><desc/><defs/><g fill="none" fill-rule="evenodd" id="Page-1" stroke="none" stroke-width="1"><g fill="#000000" id="Core" transform="translate(-341.000000, -89.000000)"><g id="close" transform="translate(341.000000, 89.000000)"><path d="M14,1.4 L12.6,0 L7,5.6 L1.4,0 L0,1.4 L5.6,7 L0,12.6 L1.4,14 L7,8.4 L12.6,14 L14,12.6 L8.4,7 L14,1.4 Z" id="Shape"/></g></g></g></svg>'
  );
  closeLink.append(closeIcon);
  closeLink.href = document.location.pathname;
  closeLink.classList.add("das-cookie-settings__close-modal");
  this.module.prepend(closeLink);
};

function htmlToElement(html) {
  var template = document.createElement("template");
  template.innerHTML = html;
  return template.content.firstChild;
}

// Cookie banner initialisation

(function(global) {
  "use strict";

  var DASFrontend = global.DASFrontend || {};

  DASFrontend.cookies = {
    $cookieBanner: document.querySelector("[data-module='cookie-banner']"),
    $cookieSettings: document.querySelector("[data-module='cookie-settings']"),

    init: function() {
      if (this.$cookieBanner != null) {
        new CookieBanner(this.$cookieBanner);
      }

      if (this.$cookieSettings != null) {
        var $cookieSettingsOptions = this.$cookieSettings.dataset.options;
        new CookieSettings(this.$cookieSettings, $cookieSettingsOptions);
      }
    }
  };

  global.DASFrontend = DASFrontend;
})(window);
