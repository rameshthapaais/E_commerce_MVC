var dataTable;

$(document).ready(function(){
    loadDataTable();
});



function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax":
            { url: '/admin/product/getall' },
        "columns": [
            { data: 'title', "width": "15%" },
            { data: 'isbn', "width": "10%" },
            { data: 'listPrice', "width": "5%" },
            { data: 'author', "width": "10%" },
            { data: 'category.categoryName', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                            <div class="w-75 btn-group" role="group">
                              <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                                 </a>
                                 <a onClick=Delete("/admin/product/delete/?id=${data}") class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                                    </a>
                                </div>
                            `;
                },
                "width": "15%"
            },




        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you Sure?",
        text: "you will delete the data",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d333",
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();     
                    toastr.success("Data deleted sucessfully");
                }
            })
        }
    })
}