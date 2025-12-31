abp.modals.CreateUrlShortingModal = function (options) {
    var l = abp.localization.getResource("CmsKit");
    
    function initModal(modalManager, args) {
        var $form = modalManager.getForm();

        var $isRegex = $form.find("#ViewModel_IsRegex");
        var $source = $form.find("#ViewModel_Source");
        var $target = $form.find("#ViewModel_Target");
        
        var $testArea = $form.find("#create-modal-test-area");
        var ignoreCase = $testArea.data("ignore-case") === "True";
        var $outputDiv = $form.find("#output-div");
        var $testInput = $testArea.find("#test-input");
        var $testOutput = $outputDiv.find("#output-input");
        $testInput.keyup(keyUpHandler);
        $source.keyup(keyUpHandler);
        $target.keyup(keyUpHandler);
        function keyUpHandler(){
            var testInput = $testInput.val();
            if(!testInput){
                $testOutput.val("");
                $outputDiv.addClass("d-none");
                return;
            }
            var source = $source.val();
            var regex = new RegExp(source, ignoreCase ? "i" : "");
            if(!regex.test(testInput)){
                $testOutput.val(l("NoMatch"));
                $testOutput.addClass("text-danger");
                $outputDiv.removeClass("d-none");
                return;
            }

            var result = testInput.replace(regex, $target.val());
            if(result){
                $testOutput.val(result);
                $testOutput.removeClass("text-danger");
                $outputDiv.removeClass("d-none");
            }else{
                $testOutput.val("");
                $outputDiv.addClass("d-none");
            }
        }
        $isRegex.change(function () {
            var isRegex = $isRegex.prop("checked");
            
            if(isRegex){
               $testArea.removeClass("d-none");
            }else{
                $testArea.addClass("d-none");
            }
        });
    }

    return {
        initModal: initModal
    };
}