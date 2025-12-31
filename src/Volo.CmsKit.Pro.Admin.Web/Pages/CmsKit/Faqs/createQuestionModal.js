var abp = abp || {};
(function ($) {
    abp.modals.createQuestion = function () {
        function initModal() {
            var $editorContainer = $("#QuestionInputContainer");
            var $editorInput = $("#ViewModel_Text");
            var initialValue = $editorInput.val();

            $.validator.setDefaults({
                ignore: ".content-editor *"
            });

            var editor = new toastui.Editor({
                el: $editorContainer[0],
                usageStatistics: false,
                useCommandShortcut: true,
                initialValue: initialValue,
                previewStyle: 'tab',
                hideModeSwitch: true,
                events: {
                    change: function () {
                        $editorInput.val(editor.getMarkdown());
                        $editorInput.trigger("change");
                    }
                },
                plugins: [toastui.Editor.plugin.codeSyntaxHighlight],
                height: "300px",
                minHeight: "25em",
                initialEditType: 'markdown',
                language: $editorContainer.data("language"),
                toolbarItems: [
                    ['heading', 'bold', 'italic'],
                    ['ul', 'ol', 'indent', 'outdent'],
                ]
            });
        }

        return {
            initModal: initModal
        };
    }
})(jQuery);
