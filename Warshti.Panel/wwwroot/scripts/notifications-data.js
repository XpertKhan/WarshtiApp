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
                "url": "/Admin/Notification/LoadData",
                "type": "POST",
                "datatype": "json",
                data: {
                    // parameters for custom backend script demo
                    //columnsDef: [
                    //    'id', 'message', 'time','user', 'Actions'],
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
                    data: "Message", name: "Message",
                    "targets": [1],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "Time", name: "NotificationTime",
                    "targets": [2],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "User", name: "User",
                    "targets": [3],
                    orderable: true,
                    searchable: true
                },
                {
                    data: "Actions", name: "Actions",
                    targets: [4],
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
                }
            ],
            //"columns": [
            //    { data: "id" },
            //    { data: "message" },
            //    { data: "time" },
            //    { data: "user" },
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