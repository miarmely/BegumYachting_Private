import {
    checkInputsWhetherBlankAsync, click_inputAsync, click_showPasswordButtonAsync,
    keyup_inputAsync
} from "./miar_module.userForm.js";

//#region variables
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

//#endregion