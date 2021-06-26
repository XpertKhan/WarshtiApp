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
                "url": "/Application/Faq/LoadData",
                "type": "POST",
                "datatype": "json",
                data: function (data) {
                    debugger
                    // parameters for custom backend script demo
                    //columnsDef: [
                    //    'id', 'question', 'answer', 'Actions'],
                },
            },
            "columnDefs": [
                {
                    "name": "Id",
                    "data":"Id",
                    "targets": [0],
                    "visible": false,
                    searchable: false
                },
                {
                    "name": "Question",
                    "data": "Question",
                    "targets": [1],
                    orderable: true,
                    searchable: true
                },
                {
                    "name": "Answer",
                    "data": "Answer",
                    "targets": [2],
                    orderable: true,
                    searchable: true
                },
                {
                    "name": "Actions",
                    "data": "Actions",
                    targets: [3],
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
            //    { data: "question" },
            //    { data: "answer" },
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