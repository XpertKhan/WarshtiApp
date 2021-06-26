"use strict";
var TempData = function () {

    var initTable = function () {
        var table = $('#kt_datatable');

        // begin first table
        table.dataTable({
            responsive: false,
            searchDelay: 500,
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            //orderMulti: true, // for disable multiple column at once
            orderable: true,
            "ajax": {
                "url": "/Admin/Service/LoadData",
                "type": "POST",
                "datatype": "json",
                data: {
                    // parameters for custom backend script demo
                    //columnsDef: [
                    //    'id', 'description', 'company', 'model',
                    //    'color', 'transmission', 'paymentMethod', 'serviceStatus',
                    //    'department', 'Actions'],
                },
            },
            "columnDefs": [
                {
                    data: "Id", name: "Id",
                    "targets": [0],
                    "visible": false,
                    searchable: false
                },
                {
                    data: "Description", name: "Description",
                    "targets": [1],
                    orderable: true,
                    searchable: true
                },
                 {
                    data: "Company", name: "Company",
                    "targets": [2],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "Model", name: "Model",
                    "targets": [3],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "Color", name: "Color",
                    "targets": [4],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "Transmission", name: "Transmission",
                    "targets": [5],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "PaymentMethod", name: "PaymentMethod",
                    "targets": [6],
                    orderable: true,
                    searchable: true
                },
                 
                {
                    data: "Department", name: "Department",
                    "targets": [7],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "Actions", name: "Actions",
                    "targets": [9],
                    title: 'Actions',
                    orderable: false,
                    searchable: false,
                    render: function (data, type, full, meta) {
                        return `
							<a href="#" class="btn btn-sm btn-clean btn-icon btn-edit" title="Edit details" data-id="${full.Id}">
								<i class="la la-edit text-warning"></i>
							</a>
							<a href="#" class="btn btn-sm btn-clean btn-icon btn-delete" title="Delete" data-id="${full.Id}">
								<i class="la la-trash text-danger"></i>
							</a>
						`;
                    },
                },
                {
                    data: "ServiceStatus", name: "ServiceStatus",
                    width: '75px',
                    "targets": [8],
                    render: function (data, type, full, meta) {
                        var status = {
                            '1': { 'title': 'Created', 'class': ' label-light-success' },
                            '2': { 'title': 'Ordered', 'class': ' label-light-info' },
                            '3': { 'title': 'Cancelled', 'class': ' label-light-danger' },
                        };
                        if (typeof status[data] === 'undefined') {
                            return data;
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                    },
                }
            ],
            //"columns": [
            //    { data: "id" },
            //    { data: "description" },
            //    { data: "company" },
            //    { data: "model" },
            //    { data: "color" },
            //    { data: "transmission" },
            //    { data: "paymentMethod" },
            //    { data: "department" },
            //    { data: "serviceStatus", responsivePriority: -2  },
            //    { data: 'Actions', responsivePriority: -1 },
            //]
        });
    };

    return {

        //main function to initiate the module
        init: function () {
            initTable();
        },

    };

}();

jQuery(document).ready(function () {
    TempData.init();
});