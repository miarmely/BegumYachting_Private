import { click_showPasswordAsync, keyup_enterKeyAsync, saveOrRemoveUsernameFromLocalAsync } from "./miar_module.login.js"
import { updateResultLabel } from "./miar_module.js"


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
    const localKeys_username = "username";
    const chck_rememberMe = $("#chck_rememberMe");
    //#endregion

    //#region events
    $(window).keyup(async (event) => {
        await keyup_enterKeyAsync(event, btn.login);
    })
    btn.login.click(async () => {
        //#region sign in
        let accountId = await loginAsync();

        // when login is successful
        if (accountId != null)
            await afterLoginAsync(accountId);
        //#endregion
    })
    btn.showPassword.click(async () => {
        await click_showPasswordAsync(
            inpt.password,
            img.showPassword,
            "/images/showPassword.png",
            "/images/hidePassword.png");
    })
    //#endregion

    //#region functions
    async function setFormAsync() {
        //#region populate email input if saved previous
        let username = localStorage.getItem(localKeys_username);

        if (username != null)
            inpt.email.val(username);
        //#endregion
    }
    async function loginAsync() {
        return await new Promise(resolve => {
            $.ajax({
                method: "POST",
                url: baseApiUrl + "/userLogin",
                data: JSON.stringify({
                    "email": inpt.email.val(),
                    "password": inpt.password.val()
                }),
                contentType: "application/json",
                dataType: "json",
                beforeSend: () => {
                    p_resultLabel.empty();
                    img_loading.removeAttr("hidden");  // show
                },
                success: (response) => {
                    //#region save account id to local
                    localStorage.setItem("accountId", response.userId);
                    img_loading.attr("hidden", "");
                    //#endregion

                    resolve(response.userId);
                },
                error: (response) => {
                    //#region set error message by status code
                    let errorMessage = "";

                    switch (response.status) {
                        case 400:  // for syntax error
                            errorMessage = `email veya şifre yanlış`;
                            break;
                        case 401:  // for wrong username or password
                            errorMessage = `email veya şifre yanlış`;
                            break;
                        default:  // for unexpected errors
                            errorMessage = "bir hata oluştu, lütfen daha sonra tekrar deneyiniz";
                            break;
                    }
                    //#endregion

                    updateResultLabel(
                        p_resultLabel,
                        errorMessage,
                        resultLabel.errorColor,
                        "30px",
                        img_loading);  // write error

                    resolve(null);
                }
            })
        })
    }
    async function afterLoginAsync(accountId) {
        await saveOrRemoveUsernameFromLocalAsync(
            chck_rememberMe,
            inpt.email,
            localKeys_username);  // by "remember me" checkbox

        $.ajax({
            method: "GET",
            url: baseApiUrl + `/adminPanel/userDisplay/id?userId=${accountId}`,
            contentType: "application/json",
            dataType: "json",
            success: (accountInfos) => {
                // save account infos to local
                localStorage.setItem(
                    "accountInfos",
                    JSON.stringify(accountInfos));

                // sign in 
                window.location.replace(`/homepage/index`);
                //window.location.replace(`/authentication/afterLogin?accountId=${accountInfos.id}`);
            },
        })
    }
    //#endregion

    setFormAsync();
})