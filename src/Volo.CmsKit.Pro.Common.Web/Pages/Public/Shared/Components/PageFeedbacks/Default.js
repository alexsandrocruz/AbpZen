(function () {
    var l = abp.localization.getResource("CmsKit");
    
    abp.widgets.CmsPageFeedback = function ($widget) {
        var $form = $widget.find("#page-feedback-form");
        var $yesBtn = $widget.find("#page-feedback-yes-btn");
        var $noBtn = $widget.find("#page-feedback-no-btn");
        var $isUseful = $widget.find("#is-useful");
        var entityId = $form.data('entity-id');
        var entityType = $form.data('entity-type');
        var validationError = $widget.find('#page-feedback-validation-error');
        var $userNoteValidation = $widget.find('#page-feedback-user-note-validation');
        var $userNoteRequiredValidation = $widget.find('#page-feedback-user-note-required-validation');
        var $submitBtn = $widget.find('#page-feedback-submit-btn');
        var $userNote = $widget.find('#user-feedback');

        var feedbackUserId = GetOrCreateFeedbackUserId();
        
        var requireCommentsForNegativeFeedback = $form.data('require-comments-for-negative-feedback') === 'True';
        
        function init() {
            $form.on('submit', '', function (e) {
                e.preventDefault();
                
                var isUseful = $isUseful.val();
                
                if(isUseful === '') {
                    validationError.removeClass('d-none');
                    $yesBtn.addClass('btn-outline-danger').removeClass('btn-outline-dark');
                    $noBtn.addClass('btn-outline-danger').removeClass('btn-outline-dark');
                    return;
                }
                
                var userNote = $userNote.val().trim();
                
                if(isUseful === "false" && requireCommentsForNegativeFeedback && !userNote) {
                    $userNote.focus();
                    $userNoteValidation.removeClass('d-none');
                    return;
                }
                
                $userNoteValidation.addClass('d-none');

                var payload = {
                    entityId: entityId,
                    entityType: entityType,
                    url: window.location.href,
                    isUseful: isUseful,
                    userNote: userNote,
                    feedbackUserId: feedbackUserId
                };
                
                var method = volo.cmsKit.public.pageFeedbacks.pageFeedbackPublic.create;
                var hasPageFeedbackId = $form.attr('data-page-feedback-id');
                if (hasPageFeedbackId) {
                    payload.id = hasPageFeedbackId;
                    method = volo.cmsKit.public.pageFeedbacks.pageFeedbackPublic.initializeUserNote;
                }

                method(payload).done(function () {
                    $form.trigger('page-feedback-sent-successfully');
                    $form.addClass('d-none');
                    $widget.find('#page-feedback-thank-you').removeClass('d-none');
                });
            });
            
            $yesBtn.on('click', function () {
                $yesBtn.removeClass('btn-outline-dark').removeClass('btn-outline-danger').addClass('btn-success');
                $noBtn.removeClass('btn-danger').removeClass('btn-outline-danger').addClass('btn-outline-dark');
                $isUseful.val(true);
                validationError.addClass('d-none');
                $userNoteValidation.addClass('d-none');
                $userNoteRequiredValidation.addClass('d-none');  

                createOrChangeFeedback(true);
            });
            
            $noBtn.on('click', function () {
                $noBtn.removeClass('btn-outline-dark').removeClass('btn-outline-danger').addClass('btn-danger');
                $yesBtn.removeClass('btn-success').removeClass('btn-outline-danger').addClass('btn-outline-dark');
                $isUseful.val(false);
                validationError.addClass('d-none'); 

                if(requireCommentsForNegativeFeedback) {
                    $userNote.focus();
                    $userNoteRequiredValidation.removeClass('d-none');
                    return;
                } 
                
                createOrChangeFeedback(false);
            });
        }
        
        function createOrChangeFeedback(isUseful) {            
            var pageFeedbackId = $form.attr('data-page-feedback-id');
            if(pageFeedbackId) {
                changeIsUseful(isUseful, pageFeedbackId);
            } else {
                createFeedback(isUseful);
            }
        }
        
        function createFeedback(isUseful){            
            var createPayload = {
                entityId: entityId,
                entityType: entityType,
                url: window.location.href,
                isUseful: isUseful,
                feedbackUserId: feedbackUserId
            };

            $submitBtn.buttonBusy(true);
            volo.cmsKit.public.pageFeedbacks.pageFeedbackPublic.create(createPayload).done(function (pageFeedback) {
                $form.attr('data-page-feedback-id', pageFeedback.id);
            }).always(function () {
                $submitBtn.buttonBusy(false);
            });
        }
        
        function changeIsUseful(isUseful, pageFeedbackId) {            
            $submitBtn.buttonBusy(true);
            volo.cmsKit.public.pageFeedbacks.pageFeedbackPublic.changeIsUseful({
                id: pageFeedbackId,
                isUseful: isUseful
            }).always(function () {
                $submitBtn.buttonBusy(false);
            });
        }

        function GetOrCreateFeedbackUserId() {
            const feedbackUserIdKey = 'CmsKit.FeedbackUserId';
            var feedbackUserId = localStorage.getItem(feedbackUserIdKey);
            if (!feedbackUserId) {
                feedbackUserId = generateGuid();
                localStorage.setItem(feedbackUserIdKey, feedbackUserId);
            }
            
            return feedbackUserId;
        }
        
        function generateGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0,
                    v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }

        return {
            init: init
        }
    };

})();
