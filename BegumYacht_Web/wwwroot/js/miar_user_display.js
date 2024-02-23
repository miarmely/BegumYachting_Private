import { isAllObjectValuesNullAsync, updateResultLabel } from "./miar_module.js"

import {
    changeCellInfosOfRowAsync, getCellInfosOfClickedRowAsync
} from "./miar_module.table.js";

import {
    addValueToDateInputAsync, convertDateToStrDateAsync, convertStrDateToDateAsync,
    convertStrUtcDateToStrLocalDateAsync, isDatesEqualAsync,
} from "./miar_module.date.js"

import {
    checkInputsWhetherBlankAsync, click_inputAsync, click_showPasswordButtonAsync,
    keyup_inputAsync, resetFormAsync, showOrHideBackButtonAsync
} from "./miar_module.userForm.js";


$(function () {
    //#region variables
    const tbl_user = $("#tbl_user");
    const p_resultLabel = $("#p_resultLabel");
    const slct = {
        yachtType: $("#div_yachtType select")
    }
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
        yachtName: $("#inpt_yachtName"),
        password: $("#inpt_password"),
        isPersonal: $("#inpt_isPersonal")
    }
    const div = {
        userDisplay: $("#div_userDisplay"),
        userUpdate: $("#div_userUpdate"),
        backButton: $("#div_backButton"),
        panelTitle: $("#div_panelTitle")
    }
    const img_loading = $("#img_loading");
    let tr_lastClicked;
    let userInfosOfLastClickedRow = [];
    //#endregion

    //#region events

    //#region update page
    $("form").submit(async (event) => {
        //#region when any input is blank (return)
        event.preventDefault();

        if (await checkInputsWhetherBlankAsync([
            inpt.firstnameLastname,
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
            slct.yachtType,
            inpt.yachtName
        ]))
            return;
        //#endregion

        await updateUserAsync();
    })
    $("input").click(async (event) => {
        await click_inputAsync(event, p_resultLabel);
    })
    $("input").on("keyup", async (event) => {
        await keyup_inputAsync(event, p_resultLabel);
    })
    $("select").click(async (event) => {
        await click_inputAsync(event, p_resultLabel);
    })
    $("select").on("keyup", async (event) => {
        await keyup_inputAsync(event, p_resultLabel);
    })
    btn.showPassword.click(async () => {
        await click_showPasswordButtonAsync(inpt.password, btn.showPassword);
    })
    btn.back.click(async () => {
        await resetFormAsync(p_resultLabel);
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
        //#region set variables
        tr_lastClicked = $(`tbody tr:nth-child(${event.currentTarget.rowIndex})`);
        userInfosOfLastClickedRow = await getCellInfosOfClickedRowAsync(event);
        //#endregion

        await openUpdatePageAsync();
        await addDefaultValuesToFormAsync();
    })
    //#endregion

    //#endregion

    //#region functions
    async function populateTableAsync() {
        $.ajax({
            method: "GET",
            url: baseApiUrl + "/getAllUsers",
            dataType: "json",
            success: (users) => {
                new Promise(async resolve => {
                    //#region set data for add to table
                    let modifiedData = []

                    for (let index in users) {
                        let userInfo = users[index];

                        modifiedData.push({
                            nameSurname: userInfo.nameSurname,
                            phoneNumber: userInfo.phoneNumber,
                            email: userInfo.email,
                            flag: userInfo.flag,
                            email: userInfo.email,
                            newPassportNo: userInfo.newPassportNo,
                            oldPassportNo: userInfo.oldPassportNo,
                            rank: userInfo.rank,
                            dateOfIssue: await convertStrUtcDateToStrLocalDateAsync(userInfo.dateOfIssue),
                            passPortExpiry: await convertStrUtcDateToStrLocalDateAsync(userInfo.passPortExpiry),
                            nationality: userInfo.nationality,
                            dateOfBirth: await convertStrUtcDateToStrLocalDateAsync(userInfo.dateOfBirth),
                            placeOfBirth: userInfo.placeOfBirth,
                            gender: userInfo.gender,
                            yacthType: userInfo.yacthType,
                            yacthName: userInfo.yacthName,
                            isPersonel: userInfo.isPersonel == null? null : userInfo.isPersonel.toString(),
                        })
                    }
                    //#endregion

                    tbl_user.DataTable({
                        data: modifiedData,
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
                        }
                    })
                    resolve();
                });
            }
        })
    }
    async function openUpdatePageAsync() {
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
        // normal inputs
        inpt.firstnameLastname.val(userInfosOfLastClickedRow[0]);
        inpt.phone.val(userInfosOfLastClickedRow[1]);
        inpt.email.val(userInfosOfLastClickedRow[2]);
        inpt.flag.val(userInfosOfLastClickedRow[3]);
        inpt.newPassportNo.val(userInfosOfLastClickedRow[4]);
        inpt.oldPassportNo.val(userInfosOfLastClickedRow[5]);
        inpt.rank.val(userInfosOfLastClickedRow[6]);
        await addValueToDateInputAsync(
            inpt.issueDate,
            "datetime",
            null,
            userInfosOfLastClickedRow[7]);  // issue date
        await addValueToDateInputAsync(
            inpt.passportExpiration,
            "datetime",
            null,
            userInfosOfLastClickedRow[8]);  // passport expiration
        inpt.nationality.val(userInfosOfLastClickedRow[9]);
        await addValueToDateInputAsync(
            inpt.birthDate,
            "date",
            null,
            userInfosOfLastClickedRow[10]);  // birth date
        inpt.birthPlace.val(userInfosOfLastClickedRow[11]);
        inpt.gender.val(userInfosOfLastClickedRow[12]);
        slct.yachtType.val(userInfosOfLastClickedRow[13]);
        inpt.yachtName.val(userInfosOfLastClickedRow[14]);
        //#region set "isPersonal"
        if (userInfosOfLastClickedRow[15] == "true")
            $("#rad_yes").prop("checked", true);

        else
            $("#rad_no").prop("checked", true);
        //#endregion
    }
    async function updateUserAsync() {
        //#region set data
        let inputValues = {
            nameSurname: inpt.firstnameLastname.val(),
            phoneNumber: inpt.phone.val(),
            email: inpt.email.val(),
            flag: inpt.flag.val(),
            newPassportNo: inpt.newPassportNo.val(),
            oldPassportNo: inpt.oldPassportNo.val(),
            rank: inpt.rank.val(),
            dateOfIssue: new Date(inpt.issueDate.val()),
            passPortExpiry: new Date(inpt.passportExpiration.val()),
            nationality: inpt.nationality.val(),
            birthDate: new Date(inpt.birthDate.val()),
            birthPlace: inpt.birthPlace.val(),
            gender: inpt.gender.val(),
            yachtType: slct.yachtType.val(),
            yachtName: inpt.yachtName.val(),
            isPersonel: $("input[type= radio][name= isPersonal]:checked").attr("id") == "rad_yes" ? "true" : "false",
            password: inpt.password.val()
        };
        let data = {
            nameSurname: inputValues.nameSurname == userInfosOfLastClickedRow[0] ? null : inputValues.nameSurname,
            phoneNumber: inputValues.phoneNumber == userInfosOfLastClickedRow[1] ? null : inputValues.phoneNumber,
            email: inputValues.email == userInfosOfLastClickedRow[2] ? null : inputValues.email,
            flag: inputValues.flag == userInfosOfLastClickedRow[3] ? null : inputValues.flag,
            newPassportNo: inputValues.newPassportNo == userInfosOfLastClickedRow[4] ? null : inputValues.newPassportNo,
            oldPassportNo: inputValues.oldPassportNo == userInfosOfLastClickedRow[5] ? null : inputValues.oldPassportNo,
            rank: inputValues.rank == userInfosOfLastClickedRow[6] ? null : inputValues.rank,
            dateOfIssue: (await isDatesEqualAsync(
                inputValues.dateOfIssue,
                await convertStrDateToDateAsync(userInfosOfLastClickedRow[7]),
                { year: true, month: true, day: true, hours: true, minutes: true, second: false }) ?
                null
                : inputValues.dateOfIssue),
            passPortExpiry: (await isDatesEqualAsync(
                inputValues.passPortExpiry,
                await convertStrDateToDateAsync(userInfosOfLastClickedRow[8]),
                { year: true, month: true, day: true, hours: true, minutes: true, second: false }) ?
                null
                : inputValues.passPortExpiry),
            nationality: inputValues.nationality == userInfosOfLastClickedRow[9] ? null : inputValues.nationality,
            dateOfBirth: (await isDatesEqualAsync(
                inputValues.birthDate,
                await convertStrDateToDateAsync(userInfosOfLastClickedRow[10]),
                { year: true, month: true, day: true, hours: false, minutes: false, second: false }) ?
                null
                : inputValues.birthDate),
            placeOfBirth: inputValues.birthPlace == userInfosOfLastClickedRow[11] ? null : inputValues.birthPlace,
            gender: inputValues.gender == userInfosOfLastClickedRow[12] ? null : inputValues.gender,
            yacthType: inputValues.yachtType == userInfosOfLastClickedRow[13] ? null : inputValues.yachtType,
            yacthName: inputValues.yachtName == userInfosOfLastClickedRow[14] ? null : inputValues.yachtName,
            isPersonel: inputValues.isPersonel == userInfosOfLastClickedRow[15] ? null : inputValues.isPersonel == "true" ? true : false,
            password: inputValues.password == "" ? null : inputValues.password
        }
        //#endregion

        //#region when no changes were made (error)
        if (await isAllObjectValuesNullAsync(data)) {
            updateResultLabel(
                p_resultLabel,
                "herhangi bir değişiklik yapılmadı",
                resultLabel_errorColor,
                "30px",
                img_loading);

            return;
        }
        //#endregion

        $.ajax({
            method: "POST",
            url: baseApiUrl + `/adminPanel/update?email=${userInfosOfLastClickedRow[2]}`,
            data: JSON.stringify(data),
            contentType: "application/json",
            dataType: "json",
            beforeSend: () => {
                p_resultLabel.empty();
                img_loading.removeAttr("hidden"); // show
            },
            success: () => {
                new Promise(async resolve => {
                    //#region change cell infos
                    let newCellInfos = [
                        inputValues.nameSurname,
                        inputValues.phoneNumber,
                        inputValues.email,
                        inputValues.flag,
                        inputValues.newPassportNo,
                        inputValues.oldPassportNo,
                        inputValues.rank,
                        await convertDateToStrDateAsync(inputValues.dateOfIssue),
                        await convertDateToStrDateAsync(inputValues.passPortExpiry),
                        inputValues.nationality,
                        await convertDateToStrDateAsync(inputValues.birthDate),
                        inputValues.birthPlace,
                        inputValues.gender,
                        inputValues.yachtType,
                        inputValues.yachtName,
                        inputValues.isPersonel
                    ];
                    userInfosOfLastClickedRow = newCellInfos;

                    await changeCellInfosOfRowAsync(tr_lastClicked, newCellInfos);
                    //#endregion

                    //#region write success messsage
                    updateResultLabel(
                        p_resultLabel,
                        "başarıyla güncellendi",
                        resultLabel_successColor,
                        "30px",
                        img_loading);

                    resolve();
                    //#endregion
                })
            },
            error: (response) => {
                // write error message
                updateResultLabel(
                    p_resultLabel,
                    JSON.parse(response.responseText).errorMessage,
                    resultLabel_errorColor,
                    "30px",
                    img_loading);
            },
        })
    }
    //#endregion

    populateTableAsync();
})