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