﻿import { getDataByAjaxOrLocalAsync, populateSelectAsync, showOrHideLoadingImageAsync, updateElementText, updateResultLabel } from "./miar_module.js";
import {
    checkInputsWhetherBlankAsync, click_inputAsync, click_showPasswordButtonAsync,
    keyup_inputAsync, populateInfoMessagesAsync, resetFormAsync
} from "./miar_module.userForm.js";


//#region variables
const img_loading = $("#img_loading");
const btn = {
    showPassword: $("#btn_showPassword"),
    save: $("#btn_save")
};
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
};
const slct = {
    yachtType: $("#div_yachtType select"),
    roles: $("#slct_roles")
};
const p_resultLabel = $("#p_resultLabel");
//#endregion

//#region events
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
        inpt.yachtName,
        inpt.password
    ]))
        return;
    //#endregion

    $.ajax({
        method: "POST",
        url: baseApiUrl + `/adminPanel/userCreateForPanel?roleName=${slct.roles.val()}`,
        headers: {
            authorization: jwtToken
        },
        data: JSON.stringify({
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
            dateOfBirth: new Date(inpt.birthDate.val()),
            placeOfBirth: inpt.birthPlace.val(),
            gender: inpt.gender.val(),
            yacthType: slct.yachtType.val(),
            yacthName: inpt.yachtName.val(),
            isPersonel: $("input[type= radio][name= isPersonal]:checked").attr("id") == "rad_yes" ? true : false,
            password: inpt.password.val()
        }),
        contentType: "application/json",
        dataType: "json",
        beforeSend: () => {
            showOrHideLoadingImageAsync("show", img_loading, p_resultLabel);
        },
        success: () => {
            resetFormAsync(p_resultLabel);

            // write success message
            updateResultLabel(
                p_resultLabel,
                "Başarıyla Eklendi",
                resultLabel_successColor,
                "30px",
                img_loading);
        },
        error: (response) => {
            // write error message
            updateResultLabel(
                p_resultLabel,
                JSON.parse(response.responseText).errorMessage,
                resultLabel_errorColor,
                "30px",
                img_loading);
        }
    })
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
//#endregion

//#region functions
async function populateFormAsync() {
    //#region populate role <select>
    var roleNames = await getDataByAjaxOrLocalAsync(
        localKeys.roleNames,
        "/adminPanel/roleDisplay");
    await populateSelectAsync(
        slct.roles,
        roleNames);
    //#endregion
}
//#endregion

populateFormAsync();