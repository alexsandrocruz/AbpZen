window.afterLeptonXInitialization = function() {
    var isRtl = JSON.parse(localStorage.getItem("Abp.IsRtl"));

    if (isRtl === null || isRtl === undefined) {
        isRtl = document.documentElement.getAttribute('dir') === 'rtl';
    }
    
    var direction = isRtl ? "rtl" : "ltr";

    replaceStyleWith(
        createStyleUrl('layout-bundle', direction),
        `lpx-layout-bundle-style-${direction}`,
        `lpx-layout-bundle-style-${direction === 'rtl' ? 'ltr' : 'rtl'}`
      );
      replaceStyleWith(
        createStyleUrl('abp-bundle', direction),
        `lpx-abp-bundle-style-${direction}`,
        `lpx-abp-bundle-style-${direction === 'rtl' ? 'ltr' : 'rtl'}`
      );
      replaceStyleWith(
        createStyleUrl('blazor-bundle', direction),
        `lpx-blazor-bundle-style-${direction}`,
        `lpx-blazor-bundle-style-${direction === 'rtl' ? 'ltr' : 'rtl'}`
      );
  

        function createStyleUrl(theme, direction='ltr') {
          const styleName = direction === 'rtl' ? `${theme}.rtl` : theme;
            return `_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme/${window.currentLayout}/css/${styleName}.css`
        }
      
        function createId(theme, type) {
          return theme && `lpx-theme-${type}-${theme}`;
        }
      
        function replaceStyleWith(path, id, previousId) {
          const link = document.createElement('link');
          link.href = path;
          link.type = 'text/css';
          link.rel = 'stylesheet';
          link.id = id;
          const prevElem = document.querySelector(`#${previousId}`);
          document.getElementsByTagName('head')[0].appendChild(link);
          if (previousId) {
            prevElem?.remove();
          }
          return link;
        }
      
        function loadThemeCSS(key, theme, themeOld, cssPrefix, direction='ltr') {
          const themeId = createId(theme, key);
          const previousThemeId = createId(themeOld, key);
      
          replaceStyleWith(createStyleUrl(cssPrefix + theme, direction), themeId, previousThemeId);
        }
};

(function () {

  (function(history){
    var pushState = history.pushState;
    history.pushState = function(state) {
        if (typeof history.onpushstate == "function") {
            history.onpushstate({state: state});
        }
        setTimeout(function(){
            var scrollBars = leptonx.init.initializers?.get('initPerfectScrollbar');
            if(scrollBars)
            {
                let scrollBar = scrollBars()[0];
                if (scrollBar) {
                    scrollBar.element.scrollTop = 0;
                } 
            }

            leptonx.mobileNavbar.closeMenu();
        }, 100);
        
        return pushState.apply(history, arguments);
    };
})(window.history);

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
    window.initLeptonX = function (layout = currentLayout, defaultStyle = "dim") {
        window.currentLayout = layout;
        leptonx.globalConfig.defaultSettings =
        {
            appearance: defaultStyle,
            containerWidth: 'full',
        };
        leptonx.CSSLoadEvent.on(event => {
            loadThemeCSS('bootstrap', event, 'bootstrap-');
            loadThemeCSS('color', event, '');
          });
        leptonx.init.run();
    }
    const oldAfterLeptonXInitialization = window.afterLeptonXInitialization;
    
    window.afterLeptonXInitialization = function () {
        if(oldAfterLeptonXInitialization){
            oldAfterLeptonXInitialization();
        }
    }
    function createStyleUrl(theme, type) {
        if (isRtl()) {
            theme = theme + '.rtl';
        }
        if (type) {
            return `_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme/${window.currentLayout}/css/${type}-${theme}.css`
        }
        return `_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme/${window.currentLayout}/css/${theme}.css`;
    }
    function isRtl() {
        return document.documentElement.getAttribute('dir') === 'rtl';
    }
})();