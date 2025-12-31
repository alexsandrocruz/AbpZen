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

                if(leptonx.mobileNavbar){
                    leptonx.mobileNavbar.closeMenu();
                }
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
            replaceStyleWith(
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
        
        replaceStyleWith(
            createStyleUrl('layout-bundle'),
            `lpx-layout-bundle-style`,
            `lpx-layout-bundle-style`
        );
        replaceStyleWith(
            createStyleUrl('abp-bundle'),
            `lpx-abp-bundle-style`,
            `lpx-abp-bundle-style`
        );
        replaceStyleWith(
            createStyleUrl('blazor-bundle'),
            `lpx-blazor-bundle-style`,
            `lpx-blazor-bundle-style`
        );
        replaceStyleWith(
            createStyleUrl('font-bundle'),
            `lpx-font-bundle-style`,
            `lpx-font-bundle-style`
        );
    }

    function createStyleUrl(theme, type) {

        var baseUrl = "/";
        var base = document.getElementsByTagName('base')[0];
        
        if(base){
            baseUrl = base.getAttribute('href');
        }
        
        if (isRtl()) {
            theme = theme + '.rtl';
        }

        if (type) {
            return `${baseUrl}_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme/${window.currentLayout}/css/${type}-${theme}.css`
        }
        return `${baseUrl}_content/Volo.Abp.AspNetCore.Components.Web.LeptonXTheme/${window.currentLayout}/css/${theme}.css`;
    }

    function replaceStyleWith(path, id, previousId) {

        let needToCreateElement = false;
        let link = document.querySelector(`[href*="${path}"]`) ?? document.querySelector(`link[id^="${previousId}"]`);
        if(link == null){
            link = document.createElement('link');
            needToCreateElement = true;
        }

        let href = link.getAttribute('href');
        href = href?.replace(href.substring(href.indexOf('?_v')), '');

        if(href !== path){
            link.href = path;
        }
        link.type = 'text/css';
        link.rel = 'stylesheet';
        link.id = id;

        if(needToCreateElement){
            document.getElementsByTagName('head')[0].appendChild(link);
        }

        return link;
    }

    function isRtl() {
        return document.documentElement.getAttribute('dir') === 'rtl';
    }
})();