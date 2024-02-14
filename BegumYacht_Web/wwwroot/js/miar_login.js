import { click_showPasswordAsync, loginAsync } from "./miar_module_login.js";


$(function () {
    //#region variables
    const btn = {
        "showPassword": $("#btn_show"),
        "login": $("#btn_login")
    };
    const img = {
        "showPassword": $("#img_show"),
        "loading": $("#img_loading")
    };
    const inpt = {
        "email": $("#inpt_email"),
        "password": $("#inpt_password")
    };
    const p_resultLabel = $("#p_resultLabel");
    const img_loading = $("#img_loading");
    //#endregion

    //#region events
    btn.showPassword.click(async () => {
        await click_showPasswordAsync(
            inpt.password,
            img.showPassword,
            "/images/showPassword.png",
            "/images/hidePassword.png");
    })
    btn.login.click(async () => {
        await loginAsync(
            { "email": inpt.email.val(), "password": inpt.password.val() },
            "email",
            p_resultLabel,
            img_loading);
    })
    //#endregion
})