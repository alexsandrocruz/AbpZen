(function ($) {
    $(function () {
        var l = abp.localization.getResource('AbpAuditLogging');

        var $periodGroup = $("#period-group");
        var $inputIsExpiredDeleterEnabled = $('#AuditLogSettings_IsExpiredDeleterEnabled');
        var $inputExpiredDeleterPeriod = $('#AuditLogSettings_ExpiredDeleterPeriod');
        var $spanMinLogsDate = $("#min-logs-date");

        var $deleterEnabledGroup = $("#deleter-enabled-group");
        var $inputGlobalIsPeriodicDeleterEnabled = $("#AuditLogGlobalSettings_IsPeriodicDeleterEnabled");
        var $inputGlobalIsExpiredDeleterEnabled = $("#AuditLogGlobalSettings_IsExpiredDeleterEnabled");
        var $globalExpiredDeleterPeriodGroup = $("#global-expired-deleter-period");
        var $inputGlobalExpiredDeleterPeriod = $('#AuditLogGlobalSettings_ExpiredDeleterPeriod');
        var $spanGlobalMinLogsDate = $("#global-min-logs-date");

        $inputIsExpiredDeleterEnabled.on('change', function () {
            updatePeriodGroupState();
        });

        function updatePeriodGroupState() {
            let isChecked = $inputIsExpiredDeleterEnabled.is(':checked');
            if (isChecked) {
                $periodGroup.show();
                $periodGroup.find('input').prop('disabled', false);
            } else {
                $periodGroup.hide();
                // disable all inputs inside $periodGroup
                $periodGroup.find('input').prop('disabled', true);

                $('#AuditLogSettings_ExpiredDeleterPeriod').val(0);
            }
        }

        updatePeriodGroupState();

        $inputExpiredDeleterPeriod.on('change', function () {
            updateMinLogsDate();
        });

        function updateMinLogsDate() {
            var period = $inputExpiredDeleterPeriod.val();
            if (period) {
                var date = moment().subtract(period, 'days').format(abp.localization.currentCulture.dateTimeFormat.shortDatePattern.replace('d', 'D'));
                $spanMinLogsDate.text(l('AuditLogsBeforeXWillBeDeleted').replace('{0}', date));
            } else {
                $spanMinLogsDate.text('');
            }
        }

        updateMinLogsDate();

        $("#AuditLogSettingsForm").on("submit", function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            abp.ui.block('#Volo-Abp-AuditLogging');
        
            volo.abp.auditLogging.auditLogSettings.update(form)
            .then(function (result) {
                abp.ui.unblock();
                $(document).trigger("AbpSettingSaved");
            }, function (err) {
                abp.ui.unblock();
                abp.notify.error(err.message);
            });
        });

        if ($inputGlobalIsPeriodicDeleterEnabled) {
            $inputGlobalIsPeriodicDeleterEnabled.on('change', function () {
                updateDeleterEnabledGroupState();
            });
        }

        function updateDeleterEnabledGroupState() {
            if ($deleterEnabledGroup) {
                let isChecked = $inputGlobalIsPeriodicDeleterEnabled.is(':checked');
                if (isChecked) {
                    $deleterEnabledGroup.show();
                    $deleterEnabledGroup.find('input').prop('disabled', false);

                } else {
                    $deleterEnabledGroup.hide();
                    $deleterEnabledGroup.find('input').prop('disabled', true);

                    $('#AuditLogGlobalSettings_ExpiredDeleterPeriod').val(0);
                }
            }
        }

        updateDeleterEnabledGroupState();

        if ($inputGlobalIsExpiredDeleterEnabled) {
            $inputGlobalIsExpiredDeleterEnabled.on('change', function () {
                updateGlobalExpiredDeleterPeriodGroupState();
            });
        }

        function updateGlobalExpiredDeleterPeriodGroupState() {
            if ($globalExpiredDeleterPeriodGroup) {
                let isChecked = $inputGlobalIsExpiredDeleterEnabled.is(':checked');
                if (isChecked) {
                    $globalExpiredDeleterPeriodGroup.show();
                    $globalExpiredDeleterPeriodGroup.find('input').prop('disabled', false);
                } else {
                    $globalExpiredDeleterPeriodGroup.hide();
                    $globalExpiredDeleterPeriodGroup.find('input').prop('disabled', true);

                    $('#AuditLogGlobalSettings_ExpiredDeleterPeriod').val(0);
                }
            }
        }

        updateGlobalExpiredDeleterPeriodGroupState();

        $inputGlobalExpiredDeleterPeriod.on('change', function () {
            updateGlobalMinLogsDate();
        });

        function updateGlobalMinLogsDate() {
            var period = $inputGlobalExpiredDeleterPeriod.val();
            if (period) {
                var date = moment().subtract(period, 'days').format(abp.localization.currentCulture.dateTimeFormat.shortDatePattern.replace('d', 'D'));
                $spanGlobalMinLogsDate.text(l('AuditLogsBeforeXWillBeDeleted').replace('{0}', date));
            } else {
                $spanGlobalMinLogsDate.text('');
            }
        }

        updateGlobalMinLogsDate();

        $("#AuditLogGlobalSettingsForm").on("submit", function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            abp.ui.block('#Volo-Abp-AuditLogging');

            volo.abp.auditLogging.auditLogSettings.updateGlobal(form)
            .then(function (result) {
                new abp.WidgetManager('#Volo-Abp-AuditLogging').refresh();
                abp.ui.unblock();
                $(document).trigger("AbpSettingSaved");
            }, function (err) {
                abp.ui.unblock();
                abp.notify.error(err.message);
            });
        });
    });
})(jQuery);