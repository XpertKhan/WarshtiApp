"use strict";
var UsersData = function () {

    var initUserTable = function () {
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
                "url": "/Admin/User/LoadData",
                "type": "POST",
                "datatype": "json",
                data: {
                    // parameters for custom backend script demo
                    //columnsDef: [
                    //    'id', 'name', 'username',
                    //     'created', 'isActive', 'Roles', 'Actions'],
                },
            },
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    searchable: false,
                    "name": "Id",
                    "data":"Id"
                },
                {
                    "name": "Actions",
                    "data": "Actions",
                    targets: [6],
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
                    "data": "IsActive",
                    "name": "IsActive",
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
                },
                {
                    "data": "Roles",
                    "name": "Roles",
                    orderable: false,
                    searchable: false,
                    targets: [5], 
                    render: function (data, type, full, meta) {

                        var response = "";
                        for (var i = 0; i < data.length; i++) {

                            console.log(data[i]);

                            var role = data[i];
                            var color = "light";
                            switch (role) {
                                case "Admin":
                                    color = "danger";
                                    break;
                                case "Operator":
                                    color = "success";
                                    break;
                                case "Client":
                                    color = "warning";
                                    break;
                                case "Workshop":
                                    color = "info";
                                    break;
                            }

                            var row = `<div class="label label-lg font-weight-bold label-light-${color} label-inline mr-2 mt-1">${role}</div>`;
                            response += row + `<br/>`;
                        }

                        return `${response}`;
                    }
                },
                //{
                //    targets: -4, //2019-12-12T00:00:00.000
                //    //render: $.fn.dataTable.render.moment("YYYY-MM-DD[T]HH:mm:ss", 'Do MMM YYYY')
                //},
                {
                    data: "Username",
                    name :"Username",
                    targets: 1,
                    searchable: true
                },
                {
                    data: "Name",
                    name: "Name",
                    targets: 2,
                    searchable: true
                },
                {
                    data: "Created",
                    name: "Created",
                    targets: 3,
                    searchable: true
                },
            ],
            //"columns": [
            //    { data: "id" },
            //    { data: "username" },
            //    { data: "name"},
            //    { data: "created", responsivePriority: -4 },
            //    { data: "isActive", responsivePriority: -3 },
            //    { data: "roles", responsivePriority: -2 },
            //    { data: 'Actions', responsivePriority: -1 },
            //]
        });
    };

    return {

        //main function to initiate the module
        init: function () {
            initUserTable();
        },

    };

}();

jQuery(document).ready(function () {
    UsersData.init();
});