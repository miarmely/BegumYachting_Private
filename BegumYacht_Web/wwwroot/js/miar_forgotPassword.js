import { keyup_enterKeyAsync } from "./miar_module.login.js";

$(function () {
    //#region variables
    const slct_chooseValidationType = $("#slct_chooseValidationType");
    const inpt_validation = $("#inpt_validation");
    const p_resultLabel = $("p_resultLabel");
    const btn_send = $("#btn_send");
    //#endregion

    //#region events
    $(window).keyup(async (event) => {
        //#region control whether "enter" key is clicked
        if (btn_send.attr("disabled") == null)  // when send button is active 
            await keyup_enterKeyAsync(event, btn_send);
        //#endregion
    })
    btn_send.click(() => {


    })
    slct_chooseValidationType.change(() => {
        //#region resets
        p_resultLabel.empty();
        inpt_validation.val("");
        //#endregion

        switch (slct_chooseValidationType.val()) {
            case "info":
                btn_send.attr("disabled", "");
                break;
            case "email":
                btn_send.removeAttr("disabled");
                inpt_validation.attr("placeholder", "Email");
                break;
            case "phone":  // DISABLED
                btn_send.removeAttr("disabled");
                inpt_validation.attr("placeholder", "Telefon Numarası");
                break;
        }
    })
    //#endregion
})