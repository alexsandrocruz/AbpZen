(function () {
    var l = abp.localization.getResource("CmsKit");

    var oldFunction = abp.widgets.CmsPageFeedback;
    
    abp.widgets.CmsPageFeedback = function ($widget) {
        
        function init(){
            oldFunction($widget).init();
            
            var modalWidget = $widget.closest('.modal');
            
            if(!modalWidget) {
                return;
            }
            
            var $form = $widget.find('form');
            var $modalHeader = modalWidget.find('.modal-header');
            
            $form.on('page-feedback-sent-successfully', function (e) {
                modalWidget.find('.modal-title').hide();
                var $thankYouTitle = $widget.find('#page-feedback-thank-you h5');
                $thankYouTitle.addClass('modal-title');
                $modalHeader.prepend($thankYouTitle);
            });
            
            
        }
        
        return {
            init: init
        }
    };
})();
