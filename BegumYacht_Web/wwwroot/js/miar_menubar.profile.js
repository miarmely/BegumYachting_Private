import {
    checkInputsWhetherBlankAsync, click_inputAsync, click_showPasswordButtonAsync,
    keyup_inputAsync, populateInfoMessagesAsync
} from "./miar_module.userForm.js";

import {
    addValueToDateInputAsync, isDatesEqualAsync, convertStrDateToDateAsync
} from "./miar_module.date.js";

import {
    autoObjectMapperAsync, updateResultLabel, isAllObjectValuesNullAsync, getDataByAjaxOrLocalAsync, populateSelectAsync
} from "./miar_module.js";


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
//#endregion

//#region functions
async function setupPageAsync() {
    await addDefaultValuesToFormAsync();

    //#region populate role <select>
    var roleNames = await getDataByAjaxOrLocalAsync(
        localKeys.roleNames,
        "/adminPanel/roleDisplay");
    await populateSelectAsync(slct.roles, roleNames, accountInfos.roleName);
    //#endregion
}
async function addDefaultValuesToFormAsync() {
    inpt.firstnameLastname.val(accountInfos.nameSurname);
    inpt.phone.val(accountInfos.phoneNumber);
    inpt.email.val(accountInfos.email);
    inpt.flag.val(accountInfos.flag);
    inpt.newPassportNo.val(accountInfos.newPassportNo);
    inpt.oldPassportNo.val(accountInfos.oldPassportNo);
    inpt.rank.val(accountInfos.rank);
    await addValueToDateInputAsync(
        inpt.issueDate,
        "datetime",
        null,
        accountInfos.dateOfIssue);  // issue date
    await addValueToDateInputAsync(
        inpt.passportExpiration,
        "datetime",
        null,
        accountInfos.passPortExpiry);  // passport expiration
    inpt.nationality.val(accountInfos.nationality);
    await addValueToDateInputAsync(
        inpt.birthDate,
        "date",
        null,
        accountInfos.dateOfBirth);  // birth date
    inpt.birthPlace.val(accountInfos.placeOfBirth);
    inpt.gender.val(accountInfos.gender);
    slct.yachtType.val(accountInfos.yacthType);
    inpt.yachtName.val(accountInfos.yacthName);
    slct.roles.val(accountInfos.roleName);
    //#region set "isPersonal"
    if (accountInfos.isPersonel == true)
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
        nameSurname: inputValues.nameSurname == accountInfos.nameSurname ? null : inputValues.nameSurname,
        phoneNumber: inputValues.phoneNumber == accountInfos.phoneNumber ? null : inputValues.phoneNumber,
        email: inputValues.email == accountInfos.email ? null : inputValues.email,
        flag: inputValues.flag == accountInfos.flag ? null : inputValues.flag,
        newPassportNo: inputValues.newPassportNo == accountInfos.newPassportNo ? null : inputValues.newPassportNo,
        oldPassportNo: inputValues.oldPassportNo == accountInfos.oldPassportNo ? null : inputValues.oldPassportNo,
        rank: inputValues.rank == accountInfos.rank ? null : inputValues.rank,
        dateOfIssue: (await isDatesEqualAsync(
            inputValues.dateOfIssue,
            await convertStrDateToDateAsync(accountInfos.dateOfIssue),
            { year: true, month: true, day: true, hours: true, minutes: true, second: false }) ?
            null
            : inputValues.dateOfIssue),
        passPortExpiry: (await isDatesEqualAsync(
            inputValues.passPortExpiry,
            await convertStrDateToDateAsync(accountInfos.passPortExpiry),
            { year: true, month: true, day: true, hours: true, minutes: true, second: false }) ?
            null
            : inputValues.passPortExpiry),
        nationality: inputValues.nationality == accountInfos.nationality ? null : inputValues.nationality,
        dateOfBirth: (await isDatesEqualAsync(
            inputValues.birthDate,
            await convertStrDateToDateAsync(accountInfos.dateOfBirth),
            { year: true, month: true, day: true, hours: false, minutes: false, second: false }) ?
            null
            : inputValues.birthDate),
        placeOfBirth: inputValues.birthPlace == accountInfos.placeOfBirth ? null : inputValues.birthPlace,
        gender: inputValues.gender == accountInfos.gender ? null : inputValues.gender,
        yacthType: inputValues.yachtType == accountInfos.yacthType ? null : inputValues.yachtType,
        yacthName: inputValues.yachtName == accountInfos.yacthName ? null : inputValues.yachtName,
        isPersonel: inputValues.isPersonel == accountInfos.isPersonel ? null : inputValues.isPersonel == "true" ? true : false,
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
        url: baseApiUrl + `/adminPanel/userUpdate?email=${accountInfos.email}`,
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: "json",
        beforeSend: () => {
            p_resultLabel.empty();
            img_loading.removeAttr("hidden"); // show
        },
        success: () => {
            new Promise(async resolve => {
                //#region update account infos in local
                await autoObjectMapperAsync(accountInfos, data, true);

                localStorage.setItem(
                    localKeys.accountInfos,
                    JSON.stringify(accountInfos));
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

setupPageAsync();