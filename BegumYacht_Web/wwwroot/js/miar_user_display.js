import { getCellInfosOfClickedRowAsync } from "./miar_module.table.js";
import { getDateTimeInString } from "./miar_module.js"

import {
    checkInputsWhetherBlankAsync, click_inputAsync, click_showPasswordButtonAsync,
    keyup_inputAsync, populateInfoMessagesAsync, showOrHideBackButtonAsync
} from "./miar_module.userForm.js";


$(function () {
    //#region variables
    const tbl_user = $("#tbl_user");
    const p_resultLabel = $("#p_resultLabel");
    const btn = {
        showPassword: $("#btn_showPassword"),
        save: $("#btn_save"),
        back: $("#btn_back")
    }
    const inpt = {
        firstnameLastname: $("#inpt_firstnameLastname"),
        phone: $("#inpt_phone"),
        email: $("#inpt_email"),
        flag: $("#inpt_flag"),
        newPassportNo: $("#inpt_newPassportNo"),
        oldPassportNo: $("#inpt_oldPassportNo"),
        rank: $("#inpt_rank"),
        issueDate: $("#inpt_issueDate"),
        passportExpiration: $("#inpt_passportExpiration"),
        nationality: $("#inpt_nationality"),
        birthDate: $("#inpt_birthDate"),
        birthPlace: $("#inpt_birthPlace"),
        gender: $("#inpt_gender"),
        yachtType: $("#inpt_yachtType"),
        yachtName: $("#inpt_yachtName"),
        password: $("#inpt_password")
    }
    const div = {
        userDisplay: $("#div_userDisplay"),
        userUpdate: $("#div_userUpdate"),
        backButton: $("#div_backButton"),
        panelTitle: $("#div_panelTitle")
    }
    let cellInfosOfLastClickedRow = [];
    let user_emailsAndInfos = {}
    //#endregion

    //#region events

    //#region update page
    $("form").submit(async (event) => {
        event.preventDefault();
        await checkInputsWhetherBlankAsync([
            inpt.firstName,
            inpt.lastName,
            inpt.phone,
            inpt.email,
            inpt.flag,
            inpt.newPassportNo,
            inpt.oldPassportNo,
            inpt.rank,
            inpt.issueDate,
            inpt.passportExpiration,
            inpt.nationality,
            inpt.birthDate,
            inpt.birthPlace,
            inpt.gender,
            inpt.yachtType,
            inpt.yachtName,
            inpt.isPersonal
        ])
    })
    $("input").click(async (event) => {
        await click_inputAsync(event, p_resultLabel);
    })
    $("input").on("keyup", async (event) => {
        await keyup_inputAsync(event, p_resultLabel);
    })
    btn.showPassword.click(async () => {
        await click_showPasswordButtonAsync(inpt.password, btn.showPassword);
    })
    btn.back.click(async () => {
        //#region reset form
        $("form")[0].reset();
        p_resultLabel.empty();
        //#endregion

        await showOrHideBackButtonAsync(
            "hide",
            div.backButton,
            div.panelTitle,
            btn.back);

        //#region show user display page
        div.userUpdate.attr("hidden", "");
        div.userDisplay.removeAttr("hidden");
        //#endregion
    })
    //#endregion

    //#region display page
    tbl_user.children("tbody").on("click", "tr", async (event) => {
        cellInfosOfLastClickedRow = await getCellInfosOfClickedRowAsync(event);
        await openUpdatePageAsync();
        await addDefaultValuesToFormAsync();
    })
    //#endregion

    //#endregion

    //#region
    async function populateTableAsync() {
        $.ajax({
            method: "GET",
            url: baseApiUrl + "/getAllUsers",
            dataType: "json",
            success: (users) => {
                // populate datatable
                let dataTable = tbl_user.DataTable({
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
                        { data: "yacthType" },
                        { data: "yacthName" },
                        { data: "isPersonel" }
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

                for(let user in users)
                    dataTable.row.add([
                        user.nameSurname,
                        user.phoneNumber,
                        user.email,
                        user.flag,
                        user.newPassportNo,
                        user.oldPassportNo,
                        user.rank,
                        user.dateOfIssue,
                        user.passPortExpiry,
                        user.nationality,
                        user.dateOfBirth,
                        user.placeOfBirth,
                        user.gender,
                        user.yacthType,
                        user.yacthName,
                        user.isPersonel
                    ])
                    
            }
        })
    }
    async function openUpdatePageAsync(event) {
        //#region show user update page
        div.userDisplay.attr("hidden", "");
        div.userUpdate.removeAttr("hidden");
        //#endregion

        await showOrHideBackButtonAsync(
            "show",
            div.backButton,
            div.panelTitle,
            btn.back);
    }
    async function addDefaultValuesToFormAsync() {
        inpt.firstnameLastname.val(cellInfosOfLastClickedRow[0]);
        inpt.phone.val(cellInfosOfLastClickedRow[1]);
        inpt.email.val(cellInfosOfLastClickedRow[2]);
        inpt.flag.val(cellInfosOfLastClickedRow[3]);
        inpt.newPassportNo.val(cellInfosOfLastClickedRow[4]);
        inpt.oldPassportNo.val(cellInfosOfLastClickedRow[5]);
        inpt.rank.val(cellInfosOfLastClickedRow[6]);
        inpt.issueDate.val(cellInfosOfLastClickedRow[7]);
        inpt.passportExpiration.val(cellInfosOfLastClickedRow[8]);
        inpt.nationality.val(cellInfosOfLastClickedRow[9]);
        inpt.birthDate.val(cellInfosOfLastClickedRow[10]);
        inpt.birthPlace.val(cellInfosOfLastClickedRow[11]);
        inpt.gender.val(cellInfosOfLastClickedRow[12]);
        inpt.yachtType.val(cellInfosOfLastClickedRow[13]);
        inpt.yachtName.val(cellInfosOfLastClickedRow[14]);
        //#region set "isPersonal"
        if (cellInfosOfLastClickedRow[15] == true)
            $("#rad_yes").prop("checked", true);

        else
            $("#rad_no").prop("checked", true);
        //#endregion
    }
    //#region functions

    populateTableAsync();
})