var abp = abp || {};
$(function () {
    abp.modals.claimTypeEdit = function () {
        var initModal = function (publicApi, args) {
            var l = abp.localization.getResource("AbpIdentity");
            var $form = publicApi.getForm();
            
			moment.localeData().preparse = (s)=>s;
			moment.localeData().postformat = (s)=>s;
			
            function setsingledatepicker() {
                $('.singledatepicker').daterangepicker({
                    "singleDatePicker": true,
                    "showDropdowns": true,
                    "autoUpdateInput": false,
                    "autoApply": true,
                    "opens": "center",
                    "drops": "auto",
                    "minYear": 1901,
                    "maxYear": 2099,
                }).on('apply.daterangepicker', function (ev, picker) {
                    $(this).val(picker.startDate.locale('en').format('l'));
                }).on('hide.daterangepicker', function (e) { e.stopPropagation(); });

                $('.singledatepicker').on('show.daterangepicker', function (e) {
                    e.stopPropagation();
                    var $this = $(this);
                    var globalFormat = moment().locale('en').localeData().longDateFormat('l');
                    var localDate = moment($this.val(), globalFormat);
                    if (localDate.isValid()) {
                        $this.data('daterangepicker').setStartDate(localDate);
                        $this.data('daterangepicker').setEndDate(localDate);
                        $this.data('daterangepicker').updateView();
                    }
                });

                $('.singledatepicker').attr('autocomplete', 'off');
            }
            
            $(document).on('keydown', 'input[type=number]', function (e) {
                if (e.keyCode === 69) {
                    e.preventDefault();
                    return false;
                }
            });
            
            setsingledatepicker();

            $('#NewClaimInputs input').keydown(function (e) {
                if (e.keyCode == 13) {
                    $('#CreateNewClaimButton').click();
                    e.preventDefault();
                    return false;
                }
            });

            var getStringInput = function (type, index, regex, claimIndex) {
                
                var dataName= 'need-valid="true" data-name="' + type + '"';

                return '<input type="text" class="form-control" id="' +
                    type +
                    'InputId_' +
                    index +
                    '" ' + dataName +
                    'name = "Claims[' + claimIndex + '].Value[' +
                    index +
                    ']" aria-describedby="DeleteClaim" />';
            };

            var getIntInput = function (type, index, claimIndex) {

                return '<input type="number" class="form-control" id="' +
                    type +
                    'InputId_' +
                    index +
                    '" name="Claims[' + claimIndex + '].Value[' +
                    index +
                    ']" aria-describedby="DeleteClaim" />';
            };

            var getDateTimeInput = function (type, index, claimIndex) {

                return '<input type="text" class="form-control singledatepicker" id="' +
                    type +
                    'InputId_' +
                    index +
                    '" name="Claims['
                    + claimIndex +
                    '].Value[' +
                    index +
                    ']" aria-describedby="DeleteClaim" />';
            };

            var getBooleanInput = function (type, index, claimIndex) {

                return '<select id="' +
                    type +
                    'InputId_' +
                    index +
                    '" name="Claims['
                    + claimIndex +
                    '].Value[' +
                    index +
                    ']" class="form-control" aria-describedby="DeleteClaim">' +
                    '<option value="true" selected>true</option> <option value = "false" > false</option>' +
                    '</select>';
            };

            var getNewInputGroup = function (type, index, regex) {
                var claimIndex = $('#Claims' + type + 'Index').val();
                var valueType = $('#Claims' + claimIndex + 'ValueType').val();

                var input = '';

                if (valueType === 'String') {
                    input = getStringInput(type, index, regex, claimIndex);
                }
                else if (valueType === 'Int') {
                    input = getIntInput(type, index, claimIndex);
                }
                else if (valueType === 'DateTime') {
                    input = getDateTimeInput(type, index, claimIndex);
                }
                else if (valueType === 'Boolean') {
                    input = getBooleanInput(type, index, claimIndex);
                }

                return  '<div id="' + type + 'GroupId_' + index + '" class="willBeHidden">\r\n' +
                        '   <div class="input-group  mb-3">' +
                        '       <label class="input-group-text mw-100 fs-9" for="' + type + 'InputId_' + index + '">' + type + '</label>\r\n' +
                        '       ' + input + '\r\n' +
                        '        <button class="btn btn-danger deleteClaim" type="button" data="' + type + '" index="' + index + '">\r\n' +
                        '            <i class="fa fa-trash"></i>\r\n' +
                        '        </button>\r\n' +
                        '   </div>\r\n' +
                        '</div>';
            };

            var changeCreateInput = function () {
                $('div .newClaimValueInput').hide();
                var type = $('#NewClaimTypeSelect').val();
                $('#New' + type + 'ClaimValueInput').parent().show();
            };

            $('#NewClaimTypeSelect').on('change', function () {
                changeCreateInput();
            });

            var setNewClaim = function (type, value) {
                var baseGroupId = type + 'GroupId_';
                var baseInputId = type + 'InputId_';
                var index = '0';

                while (index < 5000) {
                    if ($('#' + baseGroupId + index).length < 1) {
                        $('#' + baseGroupId + (index - 1)).after(getNewInputGroup(type, index, $('#Claims' + type + 'Regex').val()));
                        $('#' + baseInputId + index).val(value);
                        return;
                    }

                    var oldValue = $('#' + baseInputId + index).val();

                    if (oldValue === undefined || oldValue == '') {
                        $('#' + baseGroupId + index).show();
                        $('#' + baseInputId + index).val(value);
                        return;
                    }

                    index++;
                }
            };

            $('#CreateNewClaimButton').click(function () {
                var type = $('#NewClaimTypeSelect').val();
                
                var value = $('#New' + type + 'ClaimValueInput').val();

                if (value === undefined || value == '') {
                    abp.message.info(l("ClaimValueCanNotBeBlank"));
                    return;
                }
                
                var index = $(`#Claims${type}Index`).val();
                var valueType = $(`#Claims${index}ValueType`).val();
                
                if(valueType == "String"){
                    var regex = $(`#Claims${type}Regex`).val();
                    
                    if(regex && !new RegExp(regex).test(value)){
                        abp.message.info(l("ClaimValueIsInvalid", type));
                        return;
                    }
                }
                

                $('#New' + type + 'ClaimValueInput').val('');

                setNewClaim(type, value);
                setsingledatepicker();
            });

            $(document).on('click', '.deleteClaim', function () {
                var type = $(this).attr('data');
                var index = $(this).attr('index');
                $('#' + type + 'GroupId_' + index).hide();
                $('#' + type + 'InputId_' + index).val('');
            });

            $('div .willBeHidden').hide();

            changeCreateInput();

            $form.abpAjaxForm({
                beforeSubmit : function (){
                    var inputs = $("input[need-valid='true']");
                    for(var i = 0; i< inputs.length; i++)
                    {
                        var input = inputs[i];
                        var type = $(input).data("name");
                        var value = $(input).val();

                        var index = $(`#Claims${type}Index`).val();
                        var valueType = $(`#Claims${index}ValueType`).val();

                        if(valueType == "String"){
                            var regex = $(`#Claims${type}Regex`).val();

                            if(regex && !new RegExp(regex).test(value)){
                                abp.message.info(l("ClaimValueIsInvalid", type));
                                return false;
                            }
                        }
                    }
                }
            })
        };
        return {
            initModal: initModal
        };
    };
});