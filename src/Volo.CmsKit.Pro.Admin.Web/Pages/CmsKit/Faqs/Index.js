var abp = abp || {};
(function () {
    var l = abp.localization.getResource('CmsKit');
    var createSectionModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/Faqs/CreateSectionModal", modalClass: 'createSection'
    });
    var editSectionModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/Faqs/EditSectionModal",
        modalClass: 'editSection'
    });
    var createQuestionModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/Faqs/CreateQuestionModal",
        modalClass: 'createQuestion'
    });
    var editQuestionModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/Faqs/EditQuestionModal",
        modalClass: 'editQuestion'
    });

    var faqSectionService = volo.cmsKit.admin.faqs.faqSectionAdmin;
    var faqQuestionService = volo.cmsKit.admin.faqs.faqQuestionAdmin;

    var dataTable = $('#FaqsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(faqSectionService.getList),
            columnDefs: [
                {
                    title: '',
                    data: null,
                    defaultContent: '<div class="details-control"><i class="fa-solid fa-chevron-down"></i></div>',
                    orderable: false
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items: [
                            {
                                text: l('Edit'),
                                visible: abp.auth.isGranted('CmsKit.Faq.Update'),
                                action: function (data) {
                                    editSectionModal.open({ id: data.record.id });
                                }
                            },
                            {
                                text: l('CreateNewQuestion'),
                                action: function (data) {
                                    createQuestionModal.open({ sectionId: data.record.id });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('CmsKit.Faq.Delete'),
                                confirmMessage: function (data) {
                                    return l('FaqDeletionConfirmationMessage', data.record.name);
                                },
                                action: function (data) {
                                    faqSectionService.delete(data.record.id).then(function () {
                                        abp.notify.info(l('SuccessfullyDeleted'));
                                        dataTable.ajax.reload();
                                    });
                                }
                            }
                        ]
                    }
                },
                {
                    title: l('Order'),
                    data: "order"
                },
                {
                    title: l('Name'),
                    data: "name",
                    render: function (data, type, row) {
                        if (data.length > 50) {
                            return data.substring(0, 47) + '...';
                        }
                        return data;
                    }
                },
                {
                    title: l('QuestionCount'),
                    data: "questionCount"
                },
                {
                    title: l('CreationTime'),
                    data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString();
                    }
                },

            ]
        })
    );

    createSectionModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editSectionModal.onResult(function () {
        dataTable.ajax.reload();
    });
    createQuestionModal.onResult(function () {
        dataTable.ajax.reload();
    });
    editQuestionModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewFaqButton').click(function (e) {
        e.preventDefault();
        createSectionModal.open();
    });
    
    function initAddNewQuestionButton() {
        $('button[name=NewQuestionButton]').click(function(){
            var sectionId = $(this).data('id');
            createQuestionModal.open({ sectionId: sectionId });
        });
    }
    
    
    $('#FaqsTable tbody').on('click', '.details-control', function () {
        var tr = $(this).closest('tr');
        var row = dataTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            $(this).html('<i class="fa-solid fa-chevron-down"></i>');
        } else {
            loadChildTable(row, $(this));
        }
    });

    function loadChildTable(row, element) {
        var sectionId = row.data().id;
        faqQuestionService.getList({
            sectionId: sectionId,
            maxResultCount: 1000
        })
            .then(function (questions) {
                var newQuestionHtml = ' <div class="row lpx-content-toolbar"> <div class="col"> <h1 class="lpx-main-title"></h1> </div> <div class="col-auto mb-2"> <button name="NewQuestionButton" class="mx-1 btn btn-primary btn-sm" type="button" data-id="' + sectionId + '" data-busy-text="Processing..."><i class="fa fa-plus"></i> <span>' + l('NewQuestion') + '</span></button></div></div>';
                var childTableHtml = newQuestionHtml + '<div class="card mb-3"><div class="card-body"><table class="table table-striped table-bordered"><thead><tr><th>' + l('Actions') + '</th><th>' + l('Order') + '</th><th>' + l('Title') + '</th><th>' + l('CreationTime') + '</th></tr></thead><tbody>';
                questions.items.forEach(function (question) {
                    var creationTimeFormatted = luxon
                        .DateTime
                        .fromISO(question.creationTime, {
                            locale: abp.localization.currentCulture.name
                        }).toLocaleString();

                    var actionButtonHtml = '<div class="dropdown">' +
                        '<button class="btn btn-sm  btn-primary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                        '<i class="fa-solid fa-gear"></i> ' + l('Actions') +
                        '</button>' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<a class="dropdown-item question-edit-button"  data-id="' + question.id + '">' + l('Edit') + '</a>' +
                        '<a class="dropdown-item question-delete-button"  data-id="' + question.id + '">' + l('Delete') + '</a>' +
                        '</div>' +
                        '</div>';
                    var truncatedTitle = question.title.length > 80 ? question.title.substring(0, 77) + '...' : question.title;
                    childTableHtml += '<tr><td>' + actionButtonHtml + '</td><td>' + question.order + '</td><td>' + truncatedTitle + '</td><td>' + creationTimeFormatted + '</td></tr>';
                });
                childTableHtml += '</tbody></table></div></div>';

                row.child(childTableHtml).show();
                element.html('<i class="fa-solid fa-chevron-up"></i>');

                initAddNewQuestionButton();
            });
    }

    $(document).on('click', '.dropdown-toggle', function () {
        var $dropdownMenu = $(this).next('.dropdown-menu');
        if ($dropdownMenu.is(':visible')) {
            $dropdownMenu.hide();
        } else {
            $('.dropdown-menu').hide();
            $dropdownMenu.show();
        }
    });

    $(document).on('click', '.question-edit-button', function () {
        var questionId = $(this).data('id');
        editQuestionModal.open({id: questionId});
    });

    $(document).on('click', '.question-delete-button', function () {
        var questionId = $(this).data('id');
        faqQuestionService.delete(questionId).then(function () {
            abp.notify.info(l('SuccessfullyDeleted'));
            dataTable.ajax.reload();
        });
    });

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.dropdown').length) {
            $('.dropdown-menu').hide();
        }
    });
})();