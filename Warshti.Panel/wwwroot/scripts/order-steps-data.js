"use strict";
var StepsData = function () {

    var initTable = function (id) {
        console.log(id, new Date());

        var table = $('#kt_datatable_steps');

        // begin first table
        var oldTable = table.dataTable({
            responsive: false,
            searchDelay: 500,
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            //orderMulti: true, // for disable multiple column at once
            orderable: true,
            destroy: true,
            "ajax": {
                "url": "/Admin/OrderStep/LoadData",
                "type": "POST",
                "datatype": "json",
                "data": {
                    //"id": id
                }
            },
            "columnDefs": [
                {
                    data: "Id", name: "Id",
                    "targets": [0],
                    "visible": false,
                    searchable: false
                },
                {
                    data: "Title", name: "Title",
                    "targets": [1],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "ActionDate", name: "ActionDate",
                    "targets": [2],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "Actions", name: "Actions",
                    width: '75px',
                    targets: -1,
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
                    data: "OrderStepStatus", name: "OrderStepStatus",
                    width: '75px',
                    title: 'Status',
                    targets: -2,
                    render: function (data, type, full, meta) {
                        var status = {
                            '1': { 'title': 'In-Progress', 'class': ' label-light-warning' },
                            '2': { 'title': 'Completed', 'class': ' label-light-success' },
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
            //    { data: "title" },
            //    { data: "actionDate" },
            //    { data: "orderStepStatus", responsivePriority: -2 },
            //    { data: 'Actions', responsivePriority: -1 },
            //]
        });


    };

    return {

        //main function to initiate the module
        init: function (id) {
            initTable(id);
        },

    };

}();