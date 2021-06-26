"use strict";
var TempData = function () {

    var initTable = function () {
        var table = $('#kt_datatable');

        // begin first table
        table.dataTable({
            //dom: 'Blfrtip',
            //buttons: ['copy', 'csv', 'excel', 'pdf', 'print'],
            responsive: false,
            searchDelay: 500,
            processing: true, // for show progress bar
            serverSide: true, // for process server side
            filter: true, // this is for disable filter (search box)
            //orderMulti: true, // for disable multiple column at once
            orderable: true,
            destroy: true,
            "ajax": {
                "url": "/Admin/Order/LoadData",
                "type": "POST",
                "datatype": "json",
                data: {
                    // parameters for custom backend script demo
                    //columnsDef: [
                    //    'id', 'service', 'workshop', 'creationDate',
                    //    'expectedCompletionDate', 'completionDate', 'orderNumber', 'estimatedPrice',
                    //    'orderStatusId', 'orderProgress', 'Actions'],
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
                    data: "Service", name: "Service", 
                    "targets": [1],
                    searchable: true,
                    orderable: true

                },
                {
                    data: "Workshop", name: "Workshop",
                    "targets": [2],
                    searchable: true,
                    orderable: true

                },
                {
                    data: "CreationDate", name: "CreationDate",
                    "targets": [3],
                    searchable: true,
                    orderable: true

                },
                {
                    data: "ExpectedCompletionDate", name: "ExpectedCompletionDate",
                    "targets": [4],
                    searchable: true,
                    orderable: true

                },
                {
                    data: "CompletionDate", name: "CompletionDate" ,
                    "targets": [5],
                    searchable: true,
                    orderable: true

                },

                {
                    data: "OrderNumber", name: "OrderNumber",
                    "targets": [6],
                    searchable: true,
                    orderable: true

                },
                {
                    data: "EstimatedPrice", name: "EstimatedPrice",
                    "targets": [7],
                    searchable: true,
                    orderable: true

                },
                {
                    data: "OrderProgress", name: "OrderProgress",
                    "targets": [8],
                    searchable: true,
                    orderable: true

                },
               
                {
                    data: "OrderStatusId", name: "OrderStatusId",
                    title: 'Status',
                    width: '75px',
                    targets: [9],
                    searchable: false,
                    orderable: false,
                    render: function (data, type, full, meta) {
                        console.log(data);
                        var status = {
                            '1': { 'title': 'Offer', 'class': ' label-light-info' },
                            '2': { 'title': 'Accepted', 'class': ' label-light-primary' },
                            '3': { 'title': 'In-Progress', 'class': ' label-light-danger' },
                            '4': { 'title': 'Declined', 'class': ' label-light-secondary' },
                            '5': { 'title': 'Cancelled', 'class': ' label-light-warning' },
                            '6': { 'title': 'Completed', 'class': ' label-light-success' },
                        };
                        if (typeof status[data] === 'undefined') {
                            return data;
                        }
                        return '<span class="label label-lg font-weight-bold' + status[data].class + ' label-inline">' + status[data].title + '</span>';
                    },
                },
                {
                    data: "Actions", name: "Actions",
                    width: '120px',
                    targets: [10],
                    title: 'Actions',
                    orderable: false,
                    searchable: false,
                    render: function (data, type, full, meta) {

                        var isHidden = '';
                        var isSteps = '';
                        var isEditable = '';
                        var isDeletable = '';
                        switch (full.orderStatusId) {
                            case 1:
                                isHidden = '';
                                isSteps = 'hidden';
                                isEditable = 'hidden';
                                isDeletable = '';
                                break;
                            case 2:
                                isHidden = 'hidden';
                                isSteps = '';
                                isEditable = '';
                                isDeletable = 'hidden';
                                break;
                            case 3:
                                isHidden = 'hidden';
                                isSteps = '';
                                isEditable = '';
                                isDeletable = 'hidden';
                                break;
                            case 4:
                                isHidden = 'hidden';
                                isSteps = '';
                                isEditable = '';
                                isDeletable = 'hidden';
                                break;
                            case 5:
                                isHidden = 'hidden';
                                isSteps = 'hidden';
                                isEditable = 'hidden';
                                isDeletable = 'hidden';
                                break;
                            case 6:
                                isHidden = 'hidden';
                                isSteps = '';
                                isEditable = 'hidden';
                                isDeletable = 'hidden';
                                break;
                        }

                        // console.log(isHidden, full.orderStatusId);

                        return `
							<a href="#" class="btn btn-sm btn-clean btn-icon btn-edit" title="Edit offer details"
                                data-id="${full.Id}" ${isHidden}>
								<i class="la la-bell text-info"></i>
							</a>
                            
                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-progress" title="Edit order details"
                                data-id="${full.Id}" ${isEditable}>
								<i class="la la-pen text-primary"></i>
							</a>
                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-accept" title="Accept offer"
                                data-id="${full.Id}" ${isHidden}>
								<i class="la la-anchor text-warning"></i>
							</a>
							
                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-steps" title="Steps"
                                data-id="${full.Id}" data-service="${full.Service}" data-workshop="${full.Workshop}"
                                 ${isSteps}>
								<i class="la la-th-list text-success"></i>
							</a>
                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-delete" title="Delete"
                                data-id="${full.Id}" ${isDeletable}>
								<i class="la la-trash text-danger"></i>
							</a>
						`;
                    },
                },
            ],
            //"columns": [
            //    { data: "Id",name:"Id" },
            //    { data: "Service", name: "Service" },
            //    { data: "Workshop", name: "Workshop"  },
            //    { data: "CreationDate", name: "CreationDate"  },
            //    { data: "ExpectedCompletionDate", name: "ExpectedCompletionDate"  },
            //    { data: "CompletionDate", name: "CompletionDate"  },
            //    { data: "OrderNumber", name: "OrderNumber"  },
            //    { data: "EstimatedPrice", name: "EstimatedPrice"  },
            //    { data: "OrderProgress", name: "OrderProgress" },
            //    { data: "OrderStatusId", responsivePriority: -2  },
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