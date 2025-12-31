(function () {

    function isAlreadyLoaded(id) {
        return document.querySelector(`link[id^="lpx-theme-${id}-"]`)?.id;
    }
  
    function loadThemeCSS(key, event, cssPrefix) {
        const newThemeId = createId(event.detail.theme, key);
        const previousThemeId = createId(event.detail.previousTheme, key);
        const loadedCSS = isAlreadyLoaded(key);
  
        if (newThemeId !== loadedCSS) {
            leptonx.replaceStyleWith(
                createStyleUrl(cssPrefix + event.detail.theme),
                newThemeId,
                previousThemeId || loadedCSS
            );
        }
      }

    function createId(theme, type) {
        return theme && `lpx-theme-${type}-${theme}`;
    }

    leptonx.CSSLoadEvent.on(event => {
        loadThemeCSS('bootstrap', event, 'bootstrap-');
        loadThemeCSS('color', event, '');
    });

    window.initLeptonX = function (layout = currentLayout) {
        currentLayout = layout;
        leptonx.CSSLoadEvent.on(event => {
            loadThemeCSS('bootstrap', event, 'bootstrap-');
            loadThemeCSS('color', event, '');
        });

        leptonx.init.run();
    }

    function createStyleUrl(theme, type) {

        if (isRtl()) {
            theme = theme + '.rtl';
        }
        
        if (type) {
            return `/Themes/LeptonX/Global/${currentLayout}/css/${type}-${theme}.css`;
        }
        return `/Themes/LeptonX/Global/${currentLayout}/css/${theme}.css`;
    }

    function isRtl() {
        return document.documentElement.getAttribute('dir') === 'rtl';
    }
})();