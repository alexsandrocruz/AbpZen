/**
 * ZenLookup JavaScript Module template
 * Handles modal interactions, search, pagination, and selection
 */
export function getZenLookupJsTemplate(): string {
    return `/**
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
        _lookupModal = new bootstrap.Modal(document.getElementById('zenLookupModal'));
        _quickCreateModal = new bootstrap.Modal(document.getElementById('zenQuickCreateModal'));

        // Bind search button click
        $(document).on('click', '.zen-lookup-btn', function (e) {
            e.preventDefault();
            var targetId = $(this).data('target');
            ZenLookup.open(targetId);
        });

        // Bind clear button click
        $(document).on('click', '.zen-lookup-clear-btn', function (e) {
            e.preventDefault();
            var targetId = $(this).data('target');
            ZenLookup.clear(targetId);
        });

        // Bind create new button in input group
        $(document).on('click', '.zen-lookup-create-btn', function (e) {
            e.preventDefault();
            var targetId = $(this).data('target');
            var entity = $(this).data('entity');
            ZenLookup.openQuickCreate(targetId, entity);
        });

        // Bind search input enter key
        $(document).on('keypress', '#zenLookupSearchInput', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                ZenLookup.search();
            }
        });

        // Bind search button in modal
        $(document).on('click', '#zenLookupSearchBtn', function () {
            ZenLookup.search();
        });

        // Bind row selection
        $(document).on('click', '#zenLookupTableBody tr[data-id]', function () {
            var id = $(this).data('id');
            var displayValue = $(this).data('display');
            ZenLookup.select(id, displayValue);
        });

        // Bind create new button in modal footer
        $(document).on('click', '#zenLookupCreateNewBtn', function () {
            ZenLookup.openQuickCreateFromModal();
        });

        // Bind quick create save
        $(document).on('click', '#zenQuickCreateSaveBtn', function () {
            ZenLookup.saveQuickCreate();
        });

        // Bind pagination
        $(document).on('click', '#zenLookupPagination .page-link', function (e) {
            e.preventDefault();
            var page = $(this).data('page');
            if (page !== undefined) {
                ZenLookup.loadPage(page);
            }
        });
    };

    /**
     * Open the lookup modal for a specific input
     */
    ZenLookup.open = function (targetId) {
        var $input = $('#' + targetId);
        _currentTarget = targetId;

        _currentConfig = {
            entity: $input.data('lookup-entity'),
            displayField: $input.data('display-field'),
            lookupUrl: $input.data('lookup-url'),
            columns: $input.data('lookup-columns') || [{ field: $input.data('display-field'), title: 'Name' }],
            allowCreate: $input.data('allow-create') === true
        };

        // Update modal title
        $('#zenLookupModalTitle').text('Select ' + _currentConfig.entity);

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
        _lookupModal.show();
    };

    /**
     * Load a specific page of data
     */
    ZenLookup.loadPage = function (page) {
        _currentPage = page;
        var searchText = $('#zenLookupSearchInput').val();

        $('#zenLookupTableBody').html('<tr><td colspan="10" class="text-center"><i class="fa fa-spinner fa-spin"></i> Loading...</td></tr>');

        var url = _currentConfig.lookupUrl + '?skipCount=' + (page * _pageSize) + '&maxResultCount=' + _pageSize;
        if (searchText) {
            url += '&filter=' + encodeURIComponent(searchText);
        }

        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                ZenLookup.renderTable(result);
            },
            error: function () {
                $('#zenLookupTableBody').html('<tr><td colspan="10" class="text-center text-danger">Error loading data</td></tr>');
            }
        });
    };

    /**
     * Render the data table
     */
    ZenLookup.renderTable = function (result) {
        var items = result.items || result;
        var totalCount = result.totalCount || items.length;

        if (!items || items.length === 0) {
            $('#zenLookupTableBody').html('<tr><td colspan="10" class="text-center text-muted">No records found</td></tr>');
            $('#zenLookupPagination ul').html('');
            return;
        }

        var bodyHtml = '';
        items.forEach(function (item) {
            var displayValue = item[toCamelCase(_currentConfig.displayField)] || item.displayName || item.name || '';
            bodyHtml += '<tr data-id="' + item.id + '" data-display="' + escapeHtml(displayValue) + '">';
            _currentConfig.columns.forEach(function (col) {
                var value = item[toCamelCase(col.field)] || '';
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
            
            for (var i = 0; i < totalPages && i < 5; i++) {
                var pageNum = i;
                if (totalPages > 5 && _currentPage > 2) {
                    pageNum = _currentPage - 2 + i;
                    if (pageNum >= totalPages) pageNum = totalPages - 5 + i;
                }
                paginationHtml += '<li class="page-item ' + (pageNum === _currentPage ? 'active' : '') + '">';
                paginationHtml += '<a class="page-link" href="#" data-page="' + pageNum + '">' + (pageNum + 1) + '</a></li>';
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
        var $clearBtn = $input.closest('.input-group').find('.zen-lookup-clear-btn');

        $input.val(id);
        $display.val(displayValue);
        $clearBtn.show();

        _lookupModal.hide();
        
        // Trigger change event
        $input.trigger('change');
    };

    /**
     * Clear the selected value
     */
    ZenLookup.clear = function (targetId) {
        var $input = $('#' + targetId);
        var $display = $('#' + targetId + '_Display');
        var $clearBtn = $input.closest('.input-group').find('.zen-lookup-clear-btn');

        $input.val('');
        $display.val('');
        $clearBtn.hide();

        $input.trigger('change');
    };

    /**
     * Open quick create modal from input button
     */
    ZenLookup.openQuickCreate = function (targetId, entity) {
        _currentTarget = targetId;
        var $input = $('#' + targetId);
        
        _currentConfig = {
            entity: entity,
            displayField: $input.data('display-field'),
            createUrl: '/api/app/' + entity.toLowerCase()
        };

        ZenLookup.renderQuickCreateForm();
        _quickCreateModal.show();
    };

    /**
     * Open quick create from modal footer button
     */
    ZenLookup.openQuickCreateFromModal = function () {
        _lookupModal.hide();
        ZenLookup.renderQuickCreateForm();
        _quickCreateModal.show();
    };

    /**
     * Render quick create form dynamically
     */
    ZenLookup.renderQuickCreateForm = function () {
        $('#zenQuickCreateModalTitle').text('New ' + _currentConfig.entity);
        
        // Simple form with display field
        var formHtml = '<div class="mb-3">';
        formHtml += '<label class="form-label">' + _currentConfig.displayField + '</label>';
        formHtml += '<input type="text" class="form-control" id="zenQuickCreate_' + _currentConfig.displayField + '" required>';
        formHtml += '</div>';
        
        $('#zenQuickCreateForm').html(formHtml);
    };

    /**
     * Save quick create and select the new item
     */
    ZenLookup.saveQuickCreate = function () {
        var data = {};
        data[toCamelCase(_currentConfig.displayField)] = $('#zenQuickCreate_' + _currentConfig.displayField).val();

        if (!data[toCamelCase(_currentConfig.displayField)]) {
            abp.notify.warn('Please fill in the required field');
            return;
        }

        $.ajax({
            url: _currentConfig.createUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (result) {
                var displayValue = result[toCamelCase(_currentConfig.displayField)] || result.name || '';
                ZenLookup.select(result.id, displayValue);
                _quickCreateModal.hide();
                abp.notify.success('Created successfully');
            },
            error: function (err) {
                var message = err.responseJSON?.error?.message || 'Error creating record';
                abp.notify.error(message);
            }
        });
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
        if (document.getElementById('zenLookupModal')) {
            ZenLookup.init();
        }
    });

})(jQuery);
`;
}
