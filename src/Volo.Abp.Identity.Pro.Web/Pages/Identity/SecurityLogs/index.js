var _dataTable = null;

var l = abp.localization.getResource("AbpIdentity");

var _identitySecurityLog = volo.abp.identity.identitySecurityLog;

abp.ui.extensions.tableColumns.get("identity.securityLogs").addContributor(
  function (columnList) {
    columnList.addManyTail([
      {
        title: l("SecurityLogs:Date"),
        data: "creationTime",
        dataFormat: "datetime",
      },
      {
        title: l("SecurityLogs:Action"),
        data: "action",
        autoWidth: true,
        render: function (data) {
          data = $.fn.dataTable.render.text().display(data || "");
          return (
            '<span class="datatableCell" data-filter-field="Action">' +
            data +
            "</span>"
          );
        },
      },
      {
        title: l("SecurityLogs:IpAddress"),
        data: "clientIpAddress",
        autoWidth: true,
        orderable: false,
      },
      {
        title: l("SecurityLogs:Browser"),
        data: "browserInfo",
        autoWidth: true,
        orderable: false,
        render: function (data) {
          data = $.fn.dataTable.render.text().display(data || "");

          var maxChars = 20;

          if (data.length > maxChars) {
            return (
              '<span data-toggle="tooltip" title="' +
              data +
              '">' +
              data.substring(0, maxChars) +
              "..." +
              "</span>"
            );
          } else {
            return data;
          }
        },
      },
      {
        title: l("SecurityLogs:Application"),
        data: "applicationName",
        autoWidth: true,
        render: function (data) {
          data = $.fn.dataTable.render.text().display(data || "");
          return (
            '<span class="datatableCell" data-filter-field="ApplicationName">' +
            data +
            "</span>"
          );
        },
      },
      {
        title: l("SecurityLogs:Identity"),
        data: "identity",
        autoWidth: true,
        render: function (data) {
          data = $.fn.dataTable.render.text().display(data || "");
          return (
            '<span class="datatableCell" data-filter-field="Identity">' +
            data +
            "</span>"
          );
        },
      },
      {
        title: l("SecurityLogs:UserName"),
        data: "userName",
        autoWidth: true,
        render: function (data) {
          data = $.fn.dataTable.render.text().display(data || "");
          return (
            '<span class="datatableCell" data-filter-field="UserName">' +
            data +
            "</span>"
          );
        },
      },
      {
        title: l("SecurityLogs:Client"),
        data: "clientId",
        autoWidth: true,
        render: function (data) {
          data = $.fn.dataTable.render.text().display(data || "");
          return (
            '<span class="datatableCell" data-filter-field="ClientId">' +
            data +
            "</span>"
          );
        },
      },
      {
        title: l("SecurityLogs:CorrelationId"),
        data: "correlationId",
        autoWidth: true,
        orderable: false,
        render: function (data) {
          data = $.fn.dataTable.render.text().display(data || "");
          return (
            '<span class="datatableCell" data-filter-field="CorrelationId">' +
            data +
            "</span>"
          );
        },
      },
    ]);
  },
  0 //adds as the first contributor
);

$(function () {
    
  _dataTable = $("#IdentitySecurityLogsTable").DataTable(
    abp.libs.datatables.normalizeConfiguration({
      processing: true,
      serverSide: true,
      paging: true,
      scrollX: true,
      searching: false, 
      scrollCollapse: true,
      order: [[0, "desc"]],
      ajax: abp.libs.datatables.createAjax(
        _identitySecurityLog.getList,
        function () {
          return $("#FilterFormId").serializeFormToObject();
        }
      ),
      columnDefs: abp.ui.extensions.tableColumns
        .get("identity.securityLogs")
        .columns.toArray(),
    })
  );

  $("#FilterFormId").submit(function (e) {
    e.preventDefault();
    _dataTable.ajax.reloadEx();
  });
});

$("#ClearFilterButton").click(function (e) {
  e.preventDefault();

  $("#StartTimePicker").find("input").val("").trigger("change");
  $("#EndTimePicker").find("input").val("").trigger("change");
  $("#ApplicationName").val("");
  $("#Identity").val("");
  $("#UserName").val("");
  $("#Action").val("");
  $("#ClientId").val("");
  $("#CorrelationId").val("");
  $("#ClientIpAddress").val("");

  _dataTable.ajax.reloadEx();
});
