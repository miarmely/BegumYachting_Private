$(function () {
    //#region variables
    const tbl_user = $("#tbl_user");
    //#endregion

    //#region events
    //#endregion

    //#region
    async function populateTableAsync() {
        $.ajax({
            method: "GET",
            url: baseApiUrl + "/getAllUsers",
            dataType: "json",
            success: (users) => {
                // populate datatable
                tbl_user.DataTable({
                    data: users,
                    columns: [
                        { data: "nameSurname" },
                        { data: "phoneNumber" },
                        { data: "email" },
                        { data: "flag" },
                        { data: "newPassportNo" },
                        { data: "oldPassportNo" },
                        { data: "rank" },
                        { data: "dateOfIssue" },
                        { data: "passPortExpiry" },
                        { data: "nationality" },
                        { data: "dateOfBirth" },
                        { data: "placeOfBirth" },
                        { data: "gender" },
                        { data: "isPersonel" },
                        { data: "yacthType" },
                        { data: "yacthName" }
                    ],
                    ordering: true,
                    paging: true,
                    info: true,
                    language: {
                        lengthMenu: "_MENU_ kullanıcı görüntüle",
                        search: "Ara",
                        info: "Sayfa: _PAGE_ / _PAGES_ ~ Toplam: _MAX_",
                        infoEmpty: "kullanıcı bulunamadı",
                        infoFiltered: "",
                        paginate: {
                            previous: "Önceki",
                            next: "Sonraki",
                            first: "İlk",
                            last: "Son"
                        },
                        zeroRecords: "eşleşen kişi bulunamadı",
                        emptyTable: "kullanıcı bulunamadı",
                    },
                    select: true
                })
                .on("select", () => alert(2))
            }
        })

    }
    //#region functions

    populateTableAsync();
})