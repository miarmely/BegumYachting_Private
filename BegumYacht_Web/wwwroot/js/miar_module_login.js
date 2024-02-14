import { updateResultLabel } from "./miar_tools.js";


//#region variables
const settings = {
    "maxUnsuccessfulLoginCount": 3,
    "lockoutTimeInMinute": 3,
}
let unsuccessfulLoginCount = 0;
//#endregion

//#region events
export async function click_showPasswordAsync(
    inpt_password,
    img_showPassword,
    path_showPassword,
    path_hidePassword
) {
    //#region hide password 
    if (inpt_password.attr("type") == "text") {
        inpt_password.attr("type", "password");

        // add button image as show image
        img_showPassword.attr("src", path_showPassword)
    }
    //#endregion

    //#region show password
    else {
        inpt_password.attr("type", "text");

        // change button iamge as hide image
        img_showPassword.attr("src", path_hidePassword)
    }
    //#endregion
}
//#endregion

//#region functions
export async function loginAsync(
    data,
    usernameType = "email",
    lbl_result,
    img_loading
) {
    $.ajax({
        method: "POST",
        url: baseApiUrl + "/userLogin",
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: "json",
        beforeSend: () => {
            lbl_result.empty();
            img_loading.removeAttr("hidden");  // show
        },
        success: (response) => {
            //#region save account id to local
            localStorage.setItem("accountId", response.userId);
            img_loading.attr("hidden", "");
            //#endregion
        },
        error: (response) => {
            //#region when unsuccessful login count is finished (error)
            unsuccessfulLoginCount++;

            if (unsuccessfulLoginCount == settings.maxUnsuccessfulLoginCount) {
                updateResultLabel(
                    lbl_result,
                    "lütfen daha sonra tekrar deneyiniz",
                    resultLabel.errorColor,
                    resultLabel.defaultMarginT,
                    img_loading);

                return;
            }
            //#endregion

            //#region other errors

            //#region set error message by status code
            let errorMessage = "";

            switch (response.status) {
                case 400:  // for syntax error
                    errorMessage = `${usernameType} veya şifre yanlış`;
                    break;

                case 401:  // for wrong username or password
                    errorMessage = `${usernameType} veya şifre yanlış`;
                    break;

                default:  // for unexpected errors
                    errorMessage = "bir hata oluştu, lütfen daha sonra tekrar deneyiniz";
                    break;
            }
            //#endregion

            updateResultLabel(
                lbl_result,
                errorMessage,
                resultLabel.errorColor,
                "30px",
            // write error
                img_loading);  // write error
            //#endregion
        }
    })
}
export async function isEmailValidAsync(email) {
    //#region when there is no '@'
    let indexOfAt = email.lastIndexOf("@");

    if (indexOfAt == -1)
        return false;
    //#endregion

    //#region when there is no '.' after '@'
    let indexOfDot = email.indexOf(".", indexOfAt);

    if (indexOfDot == -1)
        return false;
    //#endregion

    return true;
}
//#endregion