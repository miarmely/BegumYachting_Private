import { click_showPasswordAsync } from "./miar_module_login.js"
import { updateResultLabel } from "./miar_tools.js"


$(function () {
    //#region variables
    const settings = {
        "maxUnsuccessfulLoginCount": 3,
        "lockoutTimeInMinute": 3,
    }
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
    let unsuccessfulLoginCount = 0;
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
        //#region sign in
        let accountId = await loginAsync();

        // when login is successful
        if (accountId != null)
            await afterLoginAsync(accountId);
        //#endregion
    })
    //#endregion

    //#region functions
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
                    //#region when unsuccessful login count is finished (error)
                    unsuccessfulLoginCount++;

                    if (unsuccessfulLoginCount == settings.maxUnsuccessfulLoginCount)
                        updateResultLabel(
                            p_resultLabel,
                            "lütfen daha sonra tekrar deneyiniz",
                            resultLabel.errorColor,
                            resultLabel.defaultMarginT,
                            img_loading);
                    //#endregion

                    //#region for other errors (error)
                    else {
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
                    }
                    //#endregion

                    resolve(null);
                }
            })
        })
    }
    async function afterLoginAsync(accountId) {
        $.ajax({
            method: "GET",
            url: baseApiUrl + `/getPersonelInfo?id=${accountId}`,
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
})