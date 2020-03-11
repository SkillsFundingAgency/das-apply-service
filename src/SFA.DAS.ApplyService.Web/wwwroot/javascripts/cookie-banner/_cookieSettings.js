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

// export default CookieSettings;
