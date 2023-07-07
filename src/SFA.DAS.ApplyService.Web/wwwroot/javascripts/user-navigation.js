var dasJs = dasJs || {};

dasJs.userNavigation = {
    elems: {
        settingsMenu: $('#das-user-navigation > ul')
    },
    init: function () {
        this.setupMenus(this.elems.settingsMenu);
        this.setupEvents(this.elems.settingsMenu);
    },
    setupMenus: function (menu) {
        menu.find('ul').attr("aria-expanded", "false");
    },
    setupEvents: function (menu) {
        var that = this;
        var subMenuLi = menu.find('li.das-user-navigation__list-item--has-sub-menu');
        subMenuLi.find('> a').on('click', function (e) {
            var $that = $(this);
            that.toggleMenu($that, $that.next('ul'));
            e.stopPropagation();
            e.preventDefault();
        });
    },
    toggleMenu: function (link, subMenu) {
        var $li = link.parent();
        if ($li.hasClass("das-user-navigation__sub-menu--open")) {
            $li.removeClass("das-user-navigation__sub-menu--open");
            subMenu.addClass("js-hidden").attr("aria-expanded", "false");
        } else {
            this.closeAllOpenMenus();
            $li.addClass("das-user-navigation__sub-menu--open");
            subMenu.removeClass("js-hidden").attr("aria-expanded", "true");
        }
    },
    closeAllOpenMenus: function () {
        $('li.das-user-navigation__list-item--has-sub-menu').each(function () {
            var listItem = $(this);
            var subMenu = $(this).children('ul');
            var openClass = 'das-user-navigation__sub-menu--open';
            if (listItem.hasClass(openClass)) {
                listItem.removeClass(openClass);
                subMenu.addClass("js-hidden").attr("aria-expanded", "false");
            }
        });
    }
}

$(document).click(function () {
    dasJs.userNavigation.closeAllOpenMenus();
});

if ($('#das-user-navigation')) {
    dasJs.userNavigation.init();
}