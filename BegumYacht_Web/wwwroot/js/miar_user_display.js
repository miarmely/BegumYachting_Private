import { getCellInfosOfClickedRowAsync } from "./miar_module_table.js";
import { getDateTimeInString } from "./miar_tools.js"


$(function () {
    //#region variables
    const tbl_user = $("#tbl_user");
    let cellInfosOfLastClickedRow = [];
    //#endregion

    //#region events
    tbl_user.children("tbody").on("click", "tr", async (event) => {
        cellInfosOfLastClickedRow = await getCellInfosOfClickedRowAsync(event);

    })
    //#endregion

    //#region
    async function populateTableAsync() {
        let data = [];

        for (let repeat = 0; repeat < 20; repeat++) {
            data.push({
                "nameSurname": "Mert Akdemir",
                "phoneNumber": "5528093408",
                "email": "mert@gmail.com",
                "flag": "Türkiye",
                "newPassportNo": "1234",
                "oldPassportNo": "1235",
                "rank": "Rank1",
                "dateOfIssue": getDateTimeInString(new Date().toLocaleString()),
                "passPortExpiry": getDateTimeInString(new Date()),
                "nationality": "Turkey",
                "dateOfBirth": "01.03.2002",
                "placeOfBirth": "Gaziosmanpaşa",
                "gender": "Erkek",
                "isPersonel": "Yes",
                "yacthType": "type1",
                "yacthName": "barbaros"
            });
            data.push({
                "nameSurname": "Gülay Akdemir",
                "phoneNumber": "5528093409",
                "email": "gülay@gmail.com",
                "flag": "Türkiye",
                "newPassportNo": "1234",
                "oldPassportNo": "1235",
                "rank": "Rank1",
                "dateOfIssue": getDateTimeInString(new Date().toLocaleString()),
                "passPortExpiry": getDateTimeInString(new Date()),
                "nationality": "Turkey",
                "dateOfBirth": "23.10.1967",
                "placeOfBirth": "Avcılar",
                "gender": "Kadın",
                "isPersonel": "Yes",
                "yacthType": "type1",
                "yacthName": "barbaros"
            });
            data.push({
                "nameSurname": "Göksel Akdemir",
                "phoneNumber": "5528093407",
                "email": "göksel@gmail.com",
                "flag": "Türkiye",
                "newPassportNo": "1234",
                "oldPassportNo": "1235",
                "rank": "Rank1",
                "dateOfIssue": getDateTimeInString(new Date().toLocaleString()),
                "passPortExpiry": getDateTimeInString(new Date()),
                "nationality": "Turkey",
                "dateOfBirth": "12.04.1963",
                "placeOfBirth": "Beşiktaş",
                "gender": "Erkek",
                "isPersonel": "Yes",
                "yacthType": "type1",
                "yacthName": "barbaros"
            });
        }

        tbl_user.DataTable({
            data: data,
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
        })

    
        
       

        //$.ajax({
        //    method: "GET",
        //    url: baseApiUrl + "/getAllUsers",
        //    dataType: "json",
        //    success: (users) => {
        //        // populate datatable
        //        tbl_user.DataTable({
        //            data: users,
        //            columns: [
        //                { data: "nameSurname" },
        //                { data: "phoneNumber" },
        //                { data: "email" },
        //                { data: "flag" },
        //                { data: "newPassportNo" },
        //                { data: "oldPassportNo" },
        //                { data: "rank" },
        //                { data: "dateOfIssue" },
        //                { data: "passPortExpiry" },
        //                { data: "nationality" },
        //                { data: "dateOfBirth" },
        //                { data: "placeOfBirth" },
        //                { data: "gender" },
        //                { data: "isPersonel" },
        //                { data: "yacthType" },
        //                { data: "yacthName" }
        //            ],
        //            ordering: true,
        //            paging: true,
        //            info: true,
        //            language: {
        //                lengthMenu: "_MENU_ kullanıcı görüntüle",
        //                search: "Ara",
        //                info: "Sayfa: _PAGE_ / _PAGES_ ~ Toplam: _MAX_",
        //                infoEmpty: "kullanıcı bulunamadı",
        //                infoFiltered: "",
        //                paginate: {
        //                    previous: "Önceki",
        //                    next: "Sonraki",
        //                    first: "İlk",
        //                    last: "Son"
        //                },
        //                zeroRecords: "eşleşen kişi bulunamadı",
        //                emptyTable: "kullanıcı bulunamadı",
        //            },
        //            select: true
        //        })
        //            .on("select", () => alert(2))
        //    }
        //})
    }
    //#region functions

    populateTableAsync();
})