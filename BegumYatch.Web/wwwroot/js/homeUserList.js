$(document).ready(function () {
    $('#usersTable').DataTable({
        dom:
            "<'row'<'col-sm-6'l><'col-sm-3 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: "btnAdd"
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {

                }
            },
            
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json',
        },
        responsive: true,
    });

    $(function () {
        const url = '/Admin/Home/AddUser/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });

        /* Ajax GET / Getting the _UserAddPartial as Modal Form ends here. */

        placeHolderDiv.on('click',
            '#btnSave',
            function (event) {
                event.preventDefault();
                const form = $('#form-user-add');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        console.log(data);
                        const userAddAjaxModel = jQuery.parseJSON(data);
                        console.log(userAddAjaxModel);
                        const newFormBody = $('.modal-body', userAddAjaxModel.UserAddPartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            placeHolderDiv.find('.modal').modal('hide');
                            const newTableRow = `
                                                                                        <tr>
                                                                    <td>${userAddAjaxModel.UserDto.User.NameSurname}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.UserName}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.Email}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.PhoneNumber}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.Flag}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.Nationality}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.YacthName}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.YacthType}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.DateOfBirth}</td>
                                                                    <td>${userAddAjaxModel.UserDto.User.IsPersonel}</td>
                                                                     <td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-minus-circle"></span></button>
                                                                      </td>
                                                                   </tr>`;

                            const jqueryTableRow = $(newTableRow);
                            jqueryTableRow.hide();
                            $('#usersTable').append(jqueryTableRow);
                            jqueryTableRow.fadeIn(3500);
                            toastr.success(`${userAddAjaxModel.UserDto.Message}`, 'Başarılı İşlem!');                           
                        }
                    },
                    error: function (err) {
                        console.log(err);
                    }
                });
            });



        $(document).on('click',
            '.btn-delete',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                const userName = tableRow.find('td:eq(1)').text();
                Swal.fire({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: `${userName} adlı kullanıcı silinicektir!`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, silmek istiyorum.',
                    cancelButtonText: 'Hayır, silmek istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            data: { userId: id },
                            url: '/Admin/Home/Delete/',
                            success: function (data) {
                                const userDto = jQuery.parseJSON(data);
                                if (userDto.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `${userDto.User.UserName} adlı kullanıcı başarıyla silinmiştir.`,
                                        'success'
                                    ).then(function () {
                                        window.location.reload();
                                        //$('#usersTable').DataTable.ajax.reload();
                                    });

                                    //dataTable.row(tableRow).remove().draw();
                                   
                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Başarısız İşlem!',
                                        text: `${userDto.Message}`,
                                    });
                                }
                            },
                            error: function (err) {
                                console.log(err);
                                toastr.error(`${err.responseText}`, "Hata!")
                            }
                        });
                    }
                });
            });
    });


    $(function () {
        const url = '/Admin/Home/UpdateUser/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function () {
                    toastr.error("Bir hata oluştu.");
                });
            });     


        placeHolderDiv.on('click',
            '#btnUpdate',
            function (event) {
                event.preventDefault();

                const form = $('#form-user-update');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        const userUpdateAjaxModel = jQuery.parseJSON(data);
                        console.log(userUpdateAjaxModel);
                        const id = userUpdateAjaxModel.UserDto.User.Id;
                        const tableRow = $(`[name="${id}"]`);
                        const newFormBody = $('.modal-body', userUpdateAjaxModel.UserUpdatePartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            placeHolderDiv.find('.modal').modal('hide');
                            const newTableRow = `
                                                               <tr name="${userUpdateAjaxModel.UserDto.User.Id}">
                                                                    <td>${userUpdateAjaxModel.UserDto.User.NameSurname}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.UserName}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.Email}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.PhoneNumber}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.Flag}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.Nationality}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.YacthName}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.YacthType}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.DateOfBirth}</td>
                                                                    <td>${userUpdateAjaxModel.UserDto.User.IsPersonel}</td>
                                                                     <td>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-minus-circle"></span></button>
                                                                      </td>
                                                                  </tr>`;
                            const newTableRowObject = $(newTableRow);
                            const userTableRow = $(`[name="${userUpdateAjaxModel.UserDto.User.Id}"]`);                            
                            newTableRowObject.hide();                            
                            userTableRow.replaceWith(newTableRowObject);                           
                            newTableRowObject.fadeIn(3500);
                            toastr.success(`${userUpdateAjaxModel.UserDto.Message}`, "Başarılı İşlem!");
                            //location.reload();

                        } else {
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText = `*${text}\n`;
                            });
                            toastr.warning(summaryText);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            });

    });
});