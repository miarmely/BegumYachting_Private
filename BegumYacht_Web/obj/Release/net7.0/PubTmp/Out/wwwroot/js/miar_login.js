import { updateElementText, updateResultLabel } from "./miar_module.js"
import {
    checkInputsWhetherBlankAsync, click_inputAsync, keyup_inputAsync, resetFormAsync
} from "./miar_module.userForm.js";

import {
    click_showPasswordAsync, keyup_enterKeyAsync, saveOrRemoveUsernameFromLocalAsync
} from "./miar_module.login.js"


$(function () {
    //#region variables
    const img = {
        loading: $("#img_loading")
    };
    const div = {
        signinOrSignupMessage: $("#div_signinOrSignupMessage"),
        login: $("#div_login"),
        validationType: $("#div_validationType"),
        validationCode: $("#div_validationCode"),
        newPassword: $("#div_newPassword"),
    };
    const inpt = {
        email_login: div.login.find(".inpt_email"),
        email_validationType: div.validationType.find(".inpt_email"),
        password_login: div.login.find(".inpt_password"),
        password_newPassword: (div.newPassword
            .children("div:nth-child(1)")
            .children(".inpt_password")),
        password_newPassword_confirm: (div.newPassword
            .children("div:nth-child(2)")
            .children(".inpt_password")),
        validationCode: $("#inpt_validationCode")
    };
    const btn = {
        show_login: div.login.find(".btn_show"),
        show_newPassword: div.newPassword.find(".btn_show:nth-child(1)"),
        show_newPassword_confirm: div.newPassword.find(".btn_show:nth-child(2)"),
        submit: $("#btn_submit")
    };
    const path = {
        showPasswordImage: "/images/showPassword.png",
        hidePasswordImage: "/images/hidePassword.png"
    };
    const a = {
        forgotPassword: $("#a_forgotPassword"),
        signinOrSignupMessage: div.signinOrSignupMessage.children("a")
    }
    const slct = {
        validationType: $("#slct_validationType")
    };
    const p_resultLabel = $("#p_resultLabel");
    const img_loading = $("#img_loading");
    const spn_signinOrSignupMessage = div.signinOrSignupMessage.children("span");
    const chck_rememberMe = $("#chck_rememberMe");
    const localKeys_username = "username";
    const signinOrsignupMessage = {
        validationType: {
            spn: "Şifreni hatırladın mı?",
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
    const resultLabel_errorColor = "red";
    const resultLabel_successColor = "rgb(16, 155, 16)";  // green
    let token = null;
    let jwtToken = null;  // "Bearer " + token
    let accountId = 0;
    let currentPage = "login";  // login|register|validationType|validationCode|newPassword
    let forgotPasswordBuffer = {
        email: null,
        verificationCode: null,
        userId: null,
        token: null,
    }
    //#endregion

    //#region events
    $(window).keyup(async (event) => {
        await keyup_enterKeyAsync(event, btn.submit);
    })
    btn.submit.click(async () => {
        await resetFormAsync(p_resultLabel, {
            formValues: false,
            helpBlocks: true,
        });
        
        //#region submit actions by form types
        let isAnyInputBlank = false;
        
        switch (currentPage) {
            case "login":
                //#region when any input is blank
                if (await checkInputsWhetherBlankAsync([
                    inpt.email_login,
                    inpt.password_login])) {
                    isAnyInputBlank = true;
                    break;
                }
                //#endregion

                await loginAsync();

                break;
            case "validationType":
                //#region when validation type is not selected (ERROR)
                if (slct.validationType.val() == "info") {
                    updateResultLabel(
                        p_resultLabel,
                        "doğrulama tipini seçmediniz",
                        resultLabel_errorColor,
                        "30px");

                    return;
                }
                //#endregion

                //#region when any input is blank
                if (await checkInputsWhetherBlankAsync([inpt.email_validationType])
                ) {
                    isAnyInputBlank = true;
                    break;
                }
                //#endregion

                //#region show validation code form
                forgotPasswordBuffer.email = inpt.email_validationType.val();

                if (await sendVerificationCodeToMailAsync())
                    await showValidationCodeFormAsync();
                //#endregion

                break;
            case "validationCode":
                //#region when any input is blank
                if (await checkInputsWhetherBlankAsync([inpt.validationCode])) {
                    isAnyInputBlank = true;
                    break;
                }
                //#endregion

                //#region show new password form
                forgotPasswordBuffer.verificationCode = inpt.validationCode.val();

                if (await checkVerificationCodeAsync())
                    await showNewPasswordFormAsync();
                //#endregion

                break;
            case "newPassword":
                //#region when password inputs is blank
                if (await checkInputsWhetherBlankAsync([
                    inpt.password_newPassword,
                    inpt.password_newPassword_confirm
                ])) {
                    isAnyInputBlank = true;
                    break;
                }
                //#endregion

                //#region when passwords is not equal to each other (ERROR)
                var newPassword = inpt.password_newPassword.val()
                var newPasswordConfirm = inpt.password_newPassword_confirm.val();

                if (newPassword != newPasswordConfirm) {
                    // write error
                    updateResultLabel(
                        p_resultLabel,
                        "şifreler eşleşmiyor",
                        resultLabel_errorColor,
                        "30px",
                        img_loading);

                    return;
                }
                //#endregion

                await resetPasswordAsync();
                break;
    }
        //#endregion

        //#region when any input is blank (ERROR)
        if (isAnyInputBlank)
            updateResultLabel(
                p_resultLabel,
                "doldurmadığın alanlar var",
                resultLabel_errorColor,
                "30px");
        //#endregion
    })
    a.forgotPassword.click(async () => {
        currentPage = "validationType";

        await resetFormAsync(p_resultLabel, {
            formValues: false,
            helpBlocks: true
        });
        await showValidationTypeFormAsync();
    })
    a.signinOrSignupMessage.click(async () => {
        await resetFormAsync(p_resultLabel, {
            formValues: false,
            helpBlocks: true
        });

        switch (currentPage) {
            case "validationType":
                //#region show login form
                div.validationType.attr("hidden", "");
                div.login.removeAttr("hidden");

                currentPage = "login";
                //#endregion

                //#region remove signin or signup message
                updateElementText(
                    spn_signinOrSignupMessage,
                    "");
                updateElementText(
                    a.signinOrSignupMessage,
                    "");
                //#endregion

                break;
            case "validationCode":
                await sendVerificationCodeToMailAsync();
                break;
            case "newPassword":
                //#region show login form
                // show form
                div.newPassword.attr("hidden", "");
                div.login.removeAttr("hidden");
                currentPage = "login";

                // reset signin or signup message  
                updateElementText(spn_signinOrSignupMessage, "");  // span
                updateElementText(a.signinOrSignupMessage, "");  // a
                //#endregion

                break;
        }
    })
    $(".btn_show").click(async (event) => {
        let btn_show = $(event.currentTarget);

        await click_showPasswordAsync(
            btn_show.siblings("input"),
            btn_show.children("img"),
            path.showPasswordImage,
            path.hidePasswordImage);
    })
    $("input").click(async (event) => {
        await click_inputAsync(event, p_resultLabel);
    })
    $("input").keyup(async (event) => {
        await keyup_inputAsync(event, p_resultLabel)
    })
    //#endregion

    //#region functions
    async function setFormAsync() {
        //#region populate email input if saved previous
        let username = localStorage.getItem(localKeys_username);

        if (username != null)
            inpt.email_login.val(username);
        //#endregion
    }
    async function loginAsync() {
        $.ajax({
            method: "POST",
            url: baseApiUrl + "/api/login/panel",
            data: JSON.stringify({
                "email": inpt.email_login.val(),
                "password": inpt.password_login.val()
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
                jwtToken = "Bearer " + token;
                accountId = response.id;

                //localStorage.setItem("token", token);
                //localStorage.setItem("accountId", accountId);
                img_loading.attr("hidden", "");
                //#endregion

                //#region sign in
                saveOrRemoveUsernameFromLocalAsync(
                    chck_rememberMe,
                    inpt.email_login,
                    localKeys_username);  // by "remember me" checkbox

                window.location.replace(`/authentication/afterLogin?token=${token}`);
                //#endregion
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
            }
        })
    }
    async function sendVerificationCodeToMailAsync() {
        return await new Promise((resolve) => {
            $.ajax({
                method: "GET",
                url: baseApiUrl + `/api/login/forgotPassword/sendCodeToMail/panel?email=${forgotPasswordBuffer.email}`,
                contentType: "application/json",
                dataType: "json",
                beforeSend: () => {
                    img_loading.removeAttr("hidden");  // show
                },
                success: () => {
                    img_loading.attr("hidden", "");
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
                url: (baseApiUrl + "/api/login/forgotPassword/verifyCode?" +
                    `email=${forgotPasswordBuffer.email}` +
                    `&verificationCode=${forgotPasswordBuffer.verificationCode}`),
                contentType: "application/json",
                dataType: "json",
                beforeSend: () => {
                    img_loading.removeAttr("hidden");
                },
                success: (response) => {
                    //#region update buffer
                    forgotPasswordBuffer.userId = response.userId;
                    forgotPasswordBuffer.token = response.tokenForResetPassword;

                    img_loading.attr("hidden", "");
                    //#endregion

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
    async function resetPasswordAsync() { 
        $.ajax({
            method: "POST",
            url: baseApiUrl + "/api/login/forgotPassword/resetPassword",
            data: JSON.stringify({
                userId: forgotPasswordBuffer.userId.toString(),
                tokenForResetPassword: forgotPasswordBuffer.token,
                newPassword: inpt.password_newPassword.val()
            }),
            contentType: "application/Json",
            dataType: "json",
            beforeSend: () => {
                img_loading.removeAttr("hidden");
            },
            success: () => {
                // hide password inputs
                div.newPassword.attr("hidden", "");

                // write success color
                updateResultLabel(
                    p_resultLabel,
                    "şifreniz başarıyla değiştirildi",
                    resultLabel_successColor,
                    "30px",
                    img.loading);
            },
            error: (response) => {
                // write error
                updateResultLabel(
                    p_resultLabel,
                    JSON.parse(response.responseText).errorMessage,
                    resultLabel_errorColor,
                    "30px",
                    img.loading);
            }
        });
    }
    async function showValidationTypeFormAsync() {
        //#region show validation type form
        div.login.attr("hidden", "");
        div.validationType.removeAttr("hidden");

        currentPage = "validationType";
        //#endregion

        //#region update signin or signup message
        updateElementText(
            spn_signinOrSignupMessage,
            signinOrsignupMessage.validationType.spn);  // for <span>
        updateElementText(
            a.signinOrSignupMessage,
            signinOrsignupMessage.validationType.a);  // for <a>
        //#endregion
    }
    async function showValidationCodeFormAsync() {
        //#region show validation code form
        div.validationType.attr("hidden", "");
        div.validationCode.removeAttr("hidden");
        currentPage = "validationCode";
        //#endregion

        //#region update signin or signup memssage
        updateElementText(
            spn_signinOrSignupMessage,
            signinOrsignupMessage.validationCode.spn);  // for <span>
        updateElementText(
            a.signinOrSignupMessage,
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
            a.signinOrSignupMessage,
            signinOrsignupMessage.newPassword.a);  // for <a>
        //#endregion
    }
    //#endregion

    setFormAsync();
})