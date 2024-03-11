import { click_showPasswordAsync, keyup_enterKeyAsync, saveOrRemoveUsernameFromLocalAsync } from "./miar_module.login.js"
import { updateElementText, updateResultLabel } from "./miar_module.js"


$(function () {
    //#region variables
    const img = {
        "showPassword": $("#img_show"),
        "loading": $("#img_loading")
    };
    const div = {
        signinOrSignupMessage: $("#div_signinOrSignupMessage"),
        login: $("#div_login"),
        register: $("#div_register"),
        validationType: $("#div_validationType"),
        validationCode: $("#validationCode"),
        newPassword: $("#div_newPassword"),
    };
    const inpt = {
        email_login: div.login.find(".inpt_email"),
        email_validationType: div.validationType.find(".inpt_email"),
        password_login: div.login.find(".inpt_password"),
        password_newPassword: div.newPassword.find(".inpt_password:nth-child(1)"),
        password_newPassword_confirm: div.newPassword.find(".inpt_password:nth-child(2)"),
        validationCode = $("#inpt_validationCode")
    };
    const btn = {
        show_login: div.login.find(".btn_show"),
        show_newPassword: div.newPassword.find(".btn_show:nth-child(1)"),
        show_newPassword_confirm: div.newPassword.find(".btn_show:nth-child(2)"),
        submit: $("#btn_submit")
    };
    const p_resultLabel = $("#p_resultLabel");
    const img_loading = $("#img_loading");
    const localKeys_username = "username";
    const chck_rememberMe = $("#chck_rememberMe");
    const signinOrsignupMessage = {
        validationType: {
            spn: "Hesabın var mı?",
            a: "Giriş Yap"
        },
        validationCode: {
            spn: "Kod gelmedi mi?",
            a: "Tekrar Gönder"
        },
        newPassword: {
            spn: "Hesabın var mı?",
            a: "Giriş Yap"
        }
    }
    const spn_signinOrSignupMessage = div.signinOrSignupMessage.children("span");
    const a_signinOrSignupMessage = div.signinOrSignupMessage.children("a");
    let token = null;
    let accountId = 0;
    let currentPage = "login";  // login|register|validationType|validationCode|newPassword
    let forgotPasswordBuffer = {
        email = null,
        verificationCode = null,
        userId = null,
        token = null,
    }
    //#endregion

    //#region events
    $(window).keyup(async (event) => {
        await keyup_enterKeyAsync(event, btn.login);
    })
    btn.submit.click(async () => {
        //#region before start
        p_resultLabel.empty();
        img_loading.removeAttr("hidden");
        //#endregion

        switch (currentPage) {
            case "login":
                if (await loginAsync())
                    await afterLoginAsync();

                break;
            case "validationType":
                //#region show validation code form
                forgotPasswordBuffer.email = inpt.email_validationType.val();

                if (await sendVerificationCodeToMailAsync())
                    await showValidationCodeFormAsync();
                //#endregion

                break;
            case "validationCode":
                //#region show new password form
                forgotPasswordBuffer.verificationCode = inpt.validationCode.val();

                if (await checkVerificationCodeAsync())
                    await showNewPasswordFormAsync();
                //#endregion

                break;
            case "newPassword": break;
        }
    })
    $(".btn_show").click(async () => {
        await click_showPasswordAsync(
            inpt.password,
            img.showPassword,
            "/images/showPassword.png",
            "/images/hidePassword.png");
    })
    a_signinOrSignupMessage.click(() => {
        p_resultLabel.empty();

        switch (currentPage) {
            case "validationType":
                //#region show login form
                // show form
                div.validationType.attr("hidden", "");
                div.login.removeAttr("hidden");
                currentPage = "login";

                // update signin or signup message  
                updateElementText(
                    spn_signinOrSignupMessage,
                    signinOrsignupMessage.login.spn);  // span
                updateElementText(
                    a_signinOrSignupMessage,
                    signinOrsignupMessage.login.a);  // a
                //#endregion

                break;
            case "validationCode":

                break;
            case "newPassword":
                //#region show login form
                // show form
                div.newPassword.attr("hidden", "");
                div.login.removeAttr("hidden");
                currentPage = "login";

                // update signin or signup message  
                updateElementText(
                    spn_signinOrSignupMessage,
                    signinOrsignupMessage.login.spn);  // span
                updateElementText(
                    a_signinOrSignupMessage,
                    signinOrsignupMessage.login.a);  // a
                //#endregion

                break;
        }
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
                url: baseApiUrl + "/adminPanel/login",
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
                    //#region save token and account id to local
                    token = response.token;
                    accountId = response.id;

                    localStorage.setItem("token", token);
                    localStorage.setItem("accountId", accountId);
                    img_loading.attr("hidden", "");
                    //#endregion

                    resolve(true);
                },
                error: (response) => {
                    //#region set error message by status code
                    let errorMessage = "";

                    switch (response.status) {
                        case 400:  // for syntax error
                            errorMessage = `email veya şifre yanlış`;
                            break;
                        case 404:  // for wrong username or password
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

                    resolve(false);
                }
            })
        })
    }
    async function afterLoginAsync() {
        await saveOrRemoveUsernameFromLocalAsync(
            chck_rememberMe,
            inpt.email,
            localKeys_username);  // by "remember me" checkbox

        $.ajax({
            method: "GET",
            url: baseApiUrl + `/adminPanel/userDisplay/id?userId=${accountId}`,
            headers: {
                authorization: "Bearer " + token
            },
            contentType: "application/json",
            dataType: "json",
            success: (accountInfos) => {
                // save account infos to local
                localStorage.setItem(
                    "accountInfos",
                    JSON.stringify(accountInfos));

                // sign in 
                window.location.replace(`/authentication/afterLogin?token=${token}`);
            },
        })
    }
    async function sendVerificationCodeToMailAsync() {
        return await new Promise((resolve) => {
            $.ajax({
                method: "GET",
                url: baseApiUrl + `/adminPanel/forgotPassword/sendCodeToMail?email=${forgotPasswordBuffer.email}`,
                headers: {
                    authorization: jwtToken
                },
                contentType: "application/json",
                dataType: "json",
                success: (response) => {
                    resolve(true);
                },
                error: (response) => {
                    // write error
                    updateResultLabel(
                        p_resultLabel,
                        JSON.parse(response.responseText).errorMessage,
                        resultLabel_errorColor,
                        "30px",
                        img_loading);

                    resolve(false);
                },
            })
        })
    }
    async function checkVerificationCodeAsync() {
        return await new Promise(resolve => {
            $.ajax({
                method: "GET",
                url: (baseApiUrl + "/adminPanel/forgotPassword/verifyCode?" +
                    `email=${forgotPasswordBuffer.email}` +
                    `&verificationCode=${forgotPasswordBuffer.verificationCode}`),
                headers: {
                    authorization: jwtToken
                },
                contentType: "application/json",
                dataType: "json",
                success: (response) => {
                    // update buffer
                    forgotPasswordBuffer.userId = response.userId;
                    forgotPasswordBuffer.token = response.tokenForResetPassword;

                    resolve(true);
                },
                error: (response) => {
                    // write error
                    updateResultLabel(
                        p_resultLabel,
                        JSON.parse(response.responseText).errorMessage,
                        resultLabel_errorColor,
                        "30px",
                        img_loading);

                    resolve(false);
                },
            })
        })
    }
    async function showValidationCodeFormAsync() {
        //#region show validation code form
        div.validationType.attr("hidden", "");
        div.validationCode.removeAttr("hidden");
        currentPage = "validationCode";
        //#endregion

        //#region update signin or signup message
        updateElementText(
            spn_signinOrSignupMessage,
            signinOrsignupMessage.validationCode.spn);  // for <span>
        updateElementText(
            a_signinOrSignupMessage,
            signinOrsignupMessage.validationCode.a);  // for <a>
        //#endregion
    }
    async function showNewPasswordFormAsync() {
        //#region hide validation code form
        div.validationCode.attr("hidden", "");
        inpt.validationCode.val("");  // reset
        //#endregion

        //#region show new password form
        div.newPassword.removeAttr("hidden");
        currentPage = "newPassword";
        //#endregion

        //#region update signin or signup message
        updateElementText(
            spn_signinOrSignupMessage,
            signinOrsignupMessage.newPassword.spn);  // for <span>
        updateElementText(
            a_signinOrSignupMessage,
            signinOrsignupMessage.newPassword.a);  // for <a>
        //#endregion
    }
    //#endregion

    setFormAsync();
})