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

// export default CookieBanner;
