//#region variables
export let div_form;
const div_form_id = "div_form";
const div_form_css = {
    "padding-top": 20
}
const div_row_class = "form-group";
const langPack = {
    "errorMessages": {
        "blankInput": "bu alanı doldurmalısın",
    },
    "infoMessages": {
        "div_firstName": ["bu alan doldurulacak"],
        "div_lastName": ["bu alan doldurulacak"],
        "div_phone": ["bu alan doldurulacak"],
        "div_email": ["bu alan doldurulacak"],
        "div_flag": ["bu alan doldurulacak"],
        "div_newPassportNo": ["bu alan doldurulacak"],
        "div_oldPassportNo": ["bu alan doldurulacak"],
        "div_rank": ["bu alan doldurulacak"],
        "div_issueDate": ["bu alan doldurulacak"],
        "div_passportExpiration": ["bu alan doldurulacak"],
        "div_nationality": ["bu alan doldurulacak"],
        "div_birthDate": ["bu alan doldurulacak"],
        "div_birthPlace": ["bu alan doldurulacak"],
        "div_gender": ["bu alan doldurulacak"],
        "div_yachtType": ["bu alan doldurulacak"],
        "div_yachtName": ["bu alan doldurulacak"],
        "div_isPersonal": ["bu alan doldurulacak"],
        "div_password": ["bu alan doldurulacak"]
    }
}
//#endregion

//#region events
export async function click_showPasswordButtonAsync(inpt_password, btn_showPassword) {
    //#region show password
    if (inpt_password.attr("type") == "password") {
        inpt_password.attr("type", "text");
        btn_showPassword.css("background-image", "url(/images/hide.png)");
    }
    //#endregion

    //#region hide password
    else {
        inpt_password.attr("type", "password");
        btn_showPassword.css("background-image", "url(/images/show.png)");
    }
    //#endregion
}
export async function click_inputAsync(event, lbl_result) {
    //#region remove "red" color from border of input
    let input = $("#" + event.target.id);
    input.removeAttr("style");
    //#endregion

    //#region reset "spn_help" of clicked input
    let spn_help = input.siblings("span[class= help-block]");
    spn_help.attr("hidden", "");
    spn_help.empty();
    //#endregion

    //#region reset result label
    lbl_result.empty();
    lbl_result.removeAttr("style");
    //#endregion
}
export async function click_radioButtonTextAsync(event) {
    //#region click to radio button belong to clicked text
    let lbl_radioButton = $("#" + event.target.id);

    lbl_radioButton
        .siblings("input")
        .trigger("click");
    //#endregion
}
export async function keyup_inputAsync(event, spn_resultLabel) {
    //#region when clicked key is "TAB"
    let clickedKeyNo = event.which;

    // reset error messages belong to input
    if (clickedKeyNo == 9)
        await click_inputAsync(event, spn_resultLabel);
    //#endregion
}
//#endregion

//#region functions
export async function createInputFormAsync(form, rowCount) {
    //#region add form <div>
    form.append(`
        <div id="${div_form_id}" class="form-horizontal bucket-form">
        </div>`);

    // add css
    div_form = form.find("#" + div_form_id);
    div_form.css(div_form_css);
    //#endregion

    //#region add rows to form <div>
    for (let repeat = 0; repeat < rowCount; repeat++)
        div_form.append(`
            <div class="${div_row_class}">
                <label class="col-sm-3 control-label"></label>
                <div class="col-sm-6"></div>
            </div>`);
    //#endregion
}
export async function populateInputFormAsync(rowNo, html_label, html_input) {
    //#region add label
    let div_row = div_form.children("." + div_row_class + `:nth-child(${rowNo})`);

    div_row
        .children("label")
        .append(html_label);
    //#endregion

    //#region add input
    div_row
        .children("div")
        .append(html_input);
    //#endregion
}
export async function populateInfoMessagesAsync() {
    //#region fill in info messages
    let infoMessages = langPack.infoMessages;

    for (let div_id in infoMessages)
        for (let msgIndex in infoMessages[div_id]) {
            let message = infoMessages[div_id][msgIndex];

            $("#" + div_id + " .div_infoMessage" + " ul")
                .append(`<li>* ${message}</li>`);
        }
    //#endregion
}
export async function addValueToInputAsync(rowNo, inputType, value) {
    //#region add value to by input type
    let div_row = div_form.children("." + div_row_class + `:nth-child(${rowNo})`);

    switch (inputType) {
        case "input":
            div_row
                .children("div>input")
                .value(value);
            break;
        case "select":
            div_row
                .children("div>select")
                .value(value);
            break;
    }
    //#endregion
}
export async function checkValueOfNumberInputAsync(inpt, minValue, maxValue) {
    //#region when current value smaller than min value
    let inpt_val = inpt.val();

    if (inpt_val < minValue)
        inpt.val(minValue);
    //#endregion

    //#region when current value bigger than max value
    else if (inpt_val > maxValue)
        inpt.val(maxValue);
    //#endregion
}
export async function checkInputsWhetherBlankAsync(inputList) {
    //#region check inputs whether is blank 
    let isAnyInputBlank = false;
   
    for (let index in inputList) {
        //#region when input is blank
        let input = inputList[index];
        
        if (input.val() == ''
            || input.val() == null) {
            await writeErrorToBelowOfInputAsync(
                input,
                langPack.errorMessages.blankInput);

            isAnyInputBlank = true;
        }
        //#endregion
    }
    //#endregion

    return isAnyInputBlank;
}
export async function writeErrorToBelowOfInputAsync(input, error) {
    // add "red" border to input
    input.css({
        "border-color": "red",
        "border-width": "1.4px"
    });

    // write error to "span_help"
    let spn_help = input.siblings("span[class= help-block]");
    spn_help.empty();
    spn_help.append(error);
}
export async function showOrHideBackButtonAsync(
    mode,
    div_backButton,
    div_panelTitle,
    btn_back
) {
    switch (mode) {
        case "show":
            // show back button
            div_backButton.removeAttr("hidden");

            // shift the panel title to right
            div_panelTitle.css(
                "padding-left",
                btn_back.css("width"));
            break;
        case "hide":
            // hide back button
            div_backButton.attr("hidden", "");

            // shift the panel title to left
            div_panelTitle.css("padding-left", "");
            break;
    }
}
export async function resetFormAsync(lbl_result) {
    // reset inputs and result label
    $("form")[0].reset();
    lbl_result.empty();

    // remove error message
    $("form .help-block").empty();

    // reset "red" border color of input or select
    $("form input").css("border-color", "");
    $("form select").css("border-color", "");
}
//#endregion