"use strict";
var WorkshopsData = function () {

    var initTable = function () {
        var table = $('#kt_datatable');

         //begin first table
        table.dataTable({
            responsive: false,
            searchDelay: 500,
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            //orderMulti: true, // for disable multiple column at once
            orderable: true,
            "ajax": {
                "url": "/Admin/Workshop/LoadData",
                "type": "POST",
                "datatype": "json",
                data: function (data) {
                     
                    // parameters for custom backend script demo
                    //columnsDef: [
                    //    'id', 'name', 'username',
                    //     'created', 'isActive', 'Actions'],
                },
            },
            "columnDefs": [
                {
                    "data": "Id",
                    name:"Id",
                    "targets": [0],
                    "visible": false,
                    searchable: false
                },
                {
                    data: 'Actions', 
                    name: 'Actions', 
                    targets: [5],
                    title: 'Actions',
                    
                    orderable: false,
                    searchable: false,
                    render: function (data, type, full, meta) {
                        return `
							<a href="#" class="btn btn-sm btn-clean btn-icon btn-edit" title="Edit details" data-id="${full.Id}">
								<i class="la la-edit text-warning"></i>
							</a>
							<a href="#" class="btn btn-sm btn-clean btn-icon btn-toggle" title="Toggle" data-id="${full.Id}">
								<i class="la la-trash text-danger"></i>
							</a>
						`;
                    },
                },
                {
                  'title':"Status",
                    data: "IsActive",
                    name: "IsActive",
                    width: '75px',
                    targets: [4],
                     render: function (data, type, full, meta) {
                        var status = {
                            'true': { 'title': 'Active', 'class': ' label-light-success' },
                            'false': { 'title': 'In-Active', 'class': ' label-light-danger' },
                        };
                        if (typeof status[data] === 'undefined') {
                            return data;
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                    },
                     orderable: true
                },
                {
                    data: "Username",
                    name: "Username",
                    targets: [1],
                    searchable: true,
                    orderable: true
                },
                {
                    data: "Name",
                    name: "Name",
                    targets: [2],
                    searchable: true,
                    orderable: true
                },
                {
                    data: "Created",
                    name: "Created",
                    targets: [3],
                    searchable: true,
                    orderable: true
                },
            ],
            //"columns": [
            //    { data: "id" },
            //    { data: "username" },
            //    { data: "name"},
            //    { data: "created" },
            //    { data: "isActive", responsivePriority: -2 },
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
    WorkshopsData.init();
});