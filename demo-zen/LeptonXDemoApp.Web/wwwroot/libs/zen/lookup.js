/**
 * ZenLookup - Generic Lookup Component
 * Provides modal-based entity selection with search and optional quick-create
 */
(function ($) {
    'use strict';

    // Namespace
    window.ZenLookup = window.ZenLookup || {};

    var _currentTarget = null;
    var _currentConfig = null;
    var _lookupModal = null;
    var _quickCreateModal = null;
    var _currentPage = 0;
    var _pageSize = 10;

    /**
     * Initialize the lookup component
     */
    ZenLookup.init = function () {
        // Bind search button click (Use event delegation for dynamic content)
        $(document).off('click', '.zen-lookup-btn').on('click', '.zen-lookup-btn', function (e) {
            e.preventDefault();
            var targetId = $(this).data('target');
            ZenLookup.open(targetId);
        });

        // Bind clear button click
        $(document).off('click', '.zen-lookup-clear-btn').on('click', '.zen-lookup-clear-btn', function (e) {
            e.preventDefault();
            var targetId = $(this).data('target');
            ZenLookup.clear(targetId);
        });

        // Bind create new button in input group
        $(document).off('click', '.zen-lookup-create-btn').on('click', '.zen-lookup-create-btn', function (e) {
            e.preventDefault();
            var targetId = $(this).data('target');
            var entity = $(this).data('entity');
            ZenLookup.openQuickCreate(targetId, entity);
        });

        // Bind search input enter key
        $(document).off('keypress', '#zenLookupSearchInput').on('keypress', '#zenLookupSearchInput', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                ZenLookup.search();
            }
        });

        // Bind search button in modal
        $(document).off('click', '#zenLookupSearchBtn').on('click', '#zenLookupSearchBtn', function () {
            ZenLookup.search();
        });

        // Bind row selection
        $(document).off('click', '#zenLookupTableBody tr[data-id]').on('click', '#zenLookupTableBody tr[data-id]', function () {
            var id = $(this).data('id');
            var displayValue = $(this).data('display');
            ZenLookup.select(id, displayValue);
        });

        // Bind create new button in modal footer
        $(document).off('click', '#zenLookupCreateNewBtn').on('click', '#zenLookupCreateNewBtn', function () {
            ZenLookup.openQuickCreateFromModal();
        });

        // Bind quick create save
        $(document).off('click', '#zenQuickCreateSaveBtn').on('click', '#zenQuickCreateSaveBtn', function () {
            ZenLookup.saveQuickCreate();
        });

        // Bind pagination
        $(document).off('click', '#zenLookupPagination .page-link').on('click', '#zenLookupPagination .page-link', function (e) {
            e.preventDefault();
            var page = $(this).data('page');
            if (page !== undefined) {
                ZenLookup.loadPage(page);
            }
        });

        console.log('ZenLookup initialized');
    };

    /**
     * Open the lookup modal for a specific input
     */
    ZenLookup.open = function (targetId) {
        ZenLookup.ensureModalExists();

        var $input = $('#' + targetId);
        _currentTarget = targetId;

        if ($input.length === 0) {
            console.error('ZenLookup: Target input not found: ' + targetId);
            return;
        }

        _currentConfig = {
            entity: $input.data('lookup-entity'),
            displayField: $input.data('display-field'),
            lookupUrl: $input.data('lookup-url'),
            columns: [{ field: $input.data('display-field') || 'name', title: 'Name' }],
            allowCreate: ($input.data('allow-create') === true || $input.data('allow-create') === 'true'),
            modalTitle: $input.data('modal-title') || ('Select ' + $input.data('lookup-entity'))
        };

        // Update modal title
        $('#zenLookupModalTitle').text(_currentConfig.modalTitle);

        // Show/hide create button
        if (_currentConfig.allowCreate) {
            $('#zenLookupCreateNewBtn').show();
        } else {
            $('#zenLookupCreateNewBtn').hide();
        }

        // Clear search
        $('#zenLookupSearchInput').val('');

        // Build table headers
        var headHtml = '';
        _currentConfig.columns.forEach(function (col) {
            headHtml += '<th>' + (col.title || col.field) + '</th>';
        });
        headHtml += '<th style="width:100px;">Action</th>';
        $('#zenLookupTableHead').html(headHtml);

        // Load data
        _currentPage = 0;
        ZenLookup.loadPage(0);

        // Show modal
        if (_lookupModal) {
            _lookupModal.show();
        }
    };

    /**
     * Load a specific page of data
     */
    ZenLookup.loadPage = function (page) {
        _currentPage = page;
        var searchText = $('#zenLookupSearchInput').val();

        $('#zenLookupTableBody').html('<tr><td colspan="10" class="text-center"><i class="fa fa-spinner fa-spin"></i> Loading...</td></tr>');

        var url = _currentConfig.lookupUrl;

        // Handle URL parameters (check if URL already has query string)
        var separator = url.indexOf('?') !== -1 ? '&' : '?';
        url += separator + 'skipCount=' + (page * _pageSize) + '&maxResultCount=' + _pageSize;

        // Re-calculate separator because we just added params
        separator = '&';

        if (searchText) {
            url += separator + 'filter=' + encodeURIComponent(searchText);
        }

        console.log('ZenLookup: Requesting ' + url);

        if (window.abp && abp.ajax) {
            abp.ajax({
                url: url,
                type: 'GET'
            }).done(function (result) {
                ZenLookup.renderTable(result);
            }).fail(function (err) {
                console.error('ZenLookup search error:', err);
                $('#zenLookupTableBody').html('<tr><td colspan="10" class="text-center text-danger py-4">Error loading data. Status: ' + (err.status || 'Error') + '</td></tr>');
            });
        } else {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (result) {
                    ZenLookup.renderTable(result);
                },
                error: function (err) {
                    console.error('ZenLookup search error:', err);
                    $('#zenLookupTableBody').html('<tr><td colspan="10" class="text-center text-danger py-4">Error loading data. Status: ' + err.status + '</td></tr>');
                }
            });
        }
    };

    /**
     * Render the data table
     */
    ZenLookup.renderTable = function (result) {
        var items = result.items || result;
        var totalCount = result.totalCount || (Array.isArray(items) ? items.length : 0);

        if (!Array.isArray(items) || items.length === 0) {
            $('#zenLookupTableBody').html('<tr><td colspan="10" class="text-center text-muted">No records found</td></tr>');
            $('#zenLookupPagination ul').html('');
            return;
        }

        var bodyHtml = '';
        items.forEach(function (item) {
            console.log('ZenLookup Item:', item);

            // Robust property lookup function
            var getValue = function (obj, prop) {
                if (!prop) return undefined;
                if (obj[prop] !== undefined) return obj[prop];
                if (obj[toCamelCase(prop)] !== undefined) return obj[toCamelCase(prop)];
                var key = Object.keys(obj).find(function (k) { return k.toLowerCase() === prop.toLowerCase(); });
                return key ? obj[key] : undefined;
            };

            var displayField = _currentConfig.displayField;
            var displayValue = getValue(item, displayField) || item.displayName || item.name || '';

            bodyHtml += '<tr data-id="' + item.id + '" data-display="' + escapeHtml(displayValue) + '">';
            _currentConfig.columns.forEach(function (col) {
                var value;
                // If column field matches the main display field, reuse the robust display value we already calculated
                if (col.field === displayField || col.field === 'name') { // 'name' check is for robustness if field defaults to 'name'
                    value = displayValue;
                } else {
                    value = getValue(item, col.field) || '';
                }
                bodyHtml += '<td>' + escapeHtml(value) + '</td>';
            });
            bodyHtml += '<td><button class="btn btn-sm btn-primary">Select</button></td>';
            bodyHtml += '</tr>';
        });
        $('#zenLookupTableBody').html(bodyHtml);

        // Render pagination
        var totalPages = Math.ceil(totalCount / _pageSize);
        var paginationHtml = '';

        if (totalPages > 1) {
            paginationHtml += '<li class="page-item ' + (_currentPage === 0 ? 'disabled' : '') + '">';
            paginationHtml += '<a class="page-link" href="#" data-page="' + (_currentPage - 1) + '">&laquo;</a></li>';

            var startPage = Math.max(0, _currentPage - 2);
            var endPage = Math.min(totalPages, startPage + 5);
            if (endPage - startPage < 5) startPage = Math.max(0, endPage - 5);

            for (var i = startPage; i < endPage; i++) {
                paginationHtml += '<li class="page-item ' + (i === _currentPage ? 'active' : '') + '">';
                paginationHtml += '<a class="page-link" href="#" data-page="' + i + '">' + (i + 1) + '</a></li>';
            }

            paginationHtml += '<li class="page-item ' + (_currentPage >= totalPages - 1 ? 'disabled' : '') + '">';
            paginationHtml += '<a class="page-link" href="#" data-page="' + (_currentPage + 1) + '">&raquo;</a></li>';
        }

        $('#zenLookupPagination ul').html(paginationHtml);
    };

    /**
     * Search with current filter value
     */
    ZenLookup.search = function () {
        _currentPage = 0;
        ZenLookup.loadPage(0);
    };

    /**
     * Select an item and close the modal
     */
    ZenLookup.select = function (id, displayValue) {
        var $input = $('#' + _currentTarget);
        var $display = $('#' + _currentTarget + '_Display');
        var group = $input.closest('.input-group');
        var $clearBtn = group.find('.zen-lookup-clear-btn');

        $input.val(id);
        $display.val(displayValue);
        $clearBtn.show();

        if (_lookupModal) {
            _lookupModal.hide();
        }

        // Trigger change event
        $input.trigger('change');
    };

    /**
     * Clear the selected value
     */
    ZenLookup.clear = function (targetId) {
        var $input = $('#' + targetId);
        var $display = $('#' + targetId + '_Display');
        var group = $input.closest('.input-group');
        var $clearBtn = group.find('.zen-lookup-clear-btn');

        $input.val('');
        $display.val('');
        $clearBtn.hide();

        $input.trigger('change');
    };

    /**
     * Open quick create modal from input button
     */
    ZenLookup.openQuickCreate = function (targetId, entity) {
        ZenLookup.ensureModalExists();

        _currentTarget = targetId;
        var $input = $('#' + targetId);

        _currentConfig = {
            entity: entity,
            displayField: $input.data('display-field') || 'Name',
            createUrl: '/ZenLookup?handler=' + entity
        };

        ZenLookup.renderQuickCreateForm();
        if (_quickCreateModal) {
            _quickCreateModal.show();
        }
    };

    /**
     * Open quick create from modal footer button
     */
    ZenLookup.openQuickCreateFromModal = function () {
        if (_lookupModal) {
            _lookupModal.hide();
        }
        ZenLookup.renderQuickCreateForm();
        if (_quickCreateModal) {
            _quickCreateModal.show();
        }
    };

    /**
     * Render quick create form dynamically
     */
    ZenLookup.renderQuickCreateForm = function () {
        $('#zenQuickCreateModalTitle').text('New ' + _currentConfig.entity);

        // Simple form with display field
        var formHtml = '<div class="mb-3">';
        formHtml += '<label class="form-label">' + _currentConfig.displayField + ' <span class="text-danger">*</span></label>';
        formHtml += '<input type="text" class="form-control" id="zenQuickCreate_' + _currentConfig.displayField + '" required>';
        formHtml += '</div>';

        $('#zenQuickCreateForm').html(formHtml);
    };

    /**
     * Save quick create and select the new item
     */
    ZenLookup.saveQuickCreate = function () {
        var fieldId = 'zenQuickCreate_' + _currentConfig.displayField;
        var val = $('#' + fieldId).val();

        var data = {};
        data[toCamelCase(_currentConfig.displayField)] = val;

        if (!val) {
            if (window.abp && abp.notify) abp.notify.warn('Please fill in the required field');
            return;
        }

        if (window.abp && abp.ajax) {
            abp.ajax({
                url: _currentConfig.createUrl,
                type: 'POST',
                data: JSON.stringify(data)
            }).done(function (result) {
                var displayValue = result[toCamelCase(_currentConfig.displayField)] || result.displayName || result.name || '';
                ZenLookup.select(result.id, displayValue);
                if (_quickCreateModal) {
                    _quickCreateModal.hide();
                }
                if (abp.notify) abp.notify.success('Created successfully');
            }).fail(function (err) {
                var message = err.responseJSON?.error?.message || 'Error creating record';
                if (abp.notify) abp.notify.error(message);
            });
        } else {
            $.ajax({
                url: _currentConfig.createUrl,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (result) {
                    var displayValue = result[toCamelCase(_currentConfig.displayField)] || result.name || '';
                    ZenLookup.select(result.id, displayValue);
                    if (_quickCreateModal) {
                        _quickCreateModal.hide();
                    }
                    if (window.abp && abp.notify) abp.notify.success('Created successfully');
                },
                error: function (err) {
                    var message = err.responseJSON?.error?.message || 'Error creating record';
                    if (window.abp && abp.notify) abp.notify.error(message);
                }
            });
        }
    };

    /**
     * Ensure the lookup modal exists in DOM (create dynamically if needed)
     */
    ZenLookup.ensureModalExists = function () {
        if (document.getElementById('zenLookupModal')) {
            // Re-initialize bootstrap modal instances if needed
            if (!_lookupModal) {
                var el = document.getElementById('zenLookupModal');
                if (el) _lookupModal = new bootstrap.Modal(el);
            }
            if (!_quickCreateModal) {
                var el = document.getElementById('zenQuickCreateModal');
                if (el) _quickCreateModal = new bootstrap.Modal(el);
            }
            return;
        }

        var modalHtml = `
            <div class="modal fade" id="zenLookupModal" tabindex="-1" aria-hidden="true" style="z-index: 1060;">
                <div class="modal-dialog modal-lg modal-dialog-centered">
                    <div class="modal-content shadow-lg border-0">
                        <div class="modal-header bg-primary text-white py-3">
                            <h5 class="modal-title fw-bold" id="zenLookupModalTitle" style="color: white !important;">Select</h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body p-4">
                            <div class="row pt-2 mb-4">
                                <div class="col">
                                    <div class="input-group">
                                        <input type="text" class="form-control" id="zenLookupSearchInput" placeholder="Search...">
                                        <button class="btn btn-primary px-4" type="button" id="zenLookupSearchBtn">
                                            <i class="fa fa-search"></i> Search
                                        </button>
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive" style="min-height: 200px;">
                                <table class="table table-hover align-middle" id="zenLookupTable">
                                    <thead class="table-light">
                                        <tr id="zenLookupTableHead"></tr>
                                    </thead>
                                    <tbody id="zenLookupTableBody">
                                        <tr>
                                            <td colspan="10" class="text-center text-muted py-5">
                                                <i class="fa fa-spinner fa-spin fa-2x mb-2"></i><br>Loading...
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <nav id="zenLookupPagination" class="d-flex justify-content-center mt-3">
                                <ul class="pagination pagination-sm mb-0"></ul>
                            </nav>
                        </div>
                        <div class="modal-footer bg-light border-0 p-3">
                            <button type="button" class="btn btn-outline-secondary px-4" data-bs-dismiss="modal">Cancel</button>
                            <button type="button" class="btn btn-success px-4" id="zenLookupCreateNewBtn" style="display:none;">
                                <i class="fa fa-plus"></i> Create New
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal fade" id="zenQuickCreateModal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" style="z-index: 1070;">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content shadow-lg border-0">
                        <div class="modal-header bg-success text-white py-3">
                            <h5 class="modal-title fw-bold" id="zenQuickCreateModalTitle" style="color: white !important;">Create New</h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body p-4" id="zenQuickCreateModalBody">
                            <form id="zenQuickCreateForm"></form>
                        </div>
                        <div class="modal-footer bg-light border-0 p-3">
                            <button type="button" class="btn btn-outline-secondary px-4" data-bs-dismiss="modal">Cancel</button>
                            <button type="button" class="btn btn-primary px-4" id="zenQuickCreateSaveBtn">
                                <i class="fa fa-check"></i> Save & Select
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        `;

        $('body').append(modalHtml);

        // Initialize bootstrap modal instances
        _lookupModal = new bootstrap.Modal(document.getElementById('zenLookupModal'));
        _quickCreateModal = new bootstrap.Modal(document.getElementById('zenQuickCreateModal'));
    };

    // Utility functions
    function toCamelCase(str) {
        if (!str) return str;
        return str.charAt(0).toLowerCase() + str.slice(1);
    }

    function escapeHtml(text) {
        if (!text) return '';
        return String(text)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#039;');
    }

    // Initialize on document ready
    $(function () {
        ZenLookup.init();

        // Re-init on ABP modal events to ensure delegation works for new elements
        if (window.abp && abp.event) {
            abp.event.on('abp.modal.opened', function () {
                ZenLookup.init();
            });
        }
    });

})(jQuery);
