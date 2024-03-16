import { addCriticalSectionAsync, updateElementText } from "./miar_module.js";
import { resetFormAsync, showOrHideBackButtonAsync } from "./miar_module.userForm.js";

import {
    addMsgWithImgToDivArticlesAsync, alignArticlesToCenterAsync, alignImageToVerticalCenterAsync,
    controlArticleWidthAsync, div_article_image_id, getArticleCountOnOneRowAsync,
    setHeightOfArticlesDivAsync, articleBuffer, setArticleBufferAsync, getValidArticleWidthAsync, addArticlesAsync
} from "./miar_module.article.js";

import { addValueToPaginationLastButtonAsync, controlPaginationBackAndNextButtonsAsync, pagingBuffer, setPagingBufferAsync } from "./miar_module.pagination.js";
import { convertStrUtcDateToStrLocalDateAsync } from "./miar_module.date.js";


//#region variables
export let infosOfLastClickedArticle = {};
const path = {
    loadingImage: "./images/loading.gif",
    questionImage: "./images/question.png"
}
//#endregion

//#region events
export async function resize_windowAsync(
    div_article_display,
    criticalSectionId
) {
    //#region when display page is open
    if (div_article_display.attr("hidden") == null)
        await addCriticalSectionAsync(
            criticalSectionId,
            async () => {
                await controlArticleWidthAsync();
                await alignArticlesToCenterAsync("px");
                await setHeightOfArticlesDivAsync();
            },
            500);
    //#endregion
}
export async function click_InfoDivAsync(event) {
    //#region set variables
    let div_infos_id = event.currentTarget.parentNode.id;
    let div_infos = $("#" + div_infos_id);
    let div_infos_button = div_infos.children(".div_infos_button");
    let div_infos_inputs = $("#" + div_infos_id + "_inputs");
    let inputTypesForBorderColor = ["input", "textarea"];
    //#endregion

    //#region when inputs of info is showing (HIDE)
    if (div_infos_inputs.attr("hidden") == null) {
        div_infos_inputs.attr("hidden", "");
        updateElementText(
            div_infos_button.children("h4").children("span"),
            "Görüntüle");
    }
    //#endregion

    //#region when inputs of info is not showing (SHOW)
    else {
        // add border color of div_infos to inputs
        for (let index in inputTypesForBorderColor)
            div_infos_inputs
                .find(inputTypesForBorderColor[index])
                .css("border-color", div_infos_button.css("border-color"));

        // show inputs
        div_infos_inputs.removeAttr("hidden");
        updateElementText(
            div_infos_button.children("h4").children("span"),
            "Gizle");
    }
    //#endregion
}
export async function click_backButtonAsync(
    lbl_result,
    div_backButton,
    div_panelTitle,
    div_article_update,
    div_article_display,
    div_senderInfos,
    div_answererInfos,
    div_demandInfos,
    div_senderInfos_inputs,
    div_answererInfos_inputs,
    formInfos_inputs,
    btn_back
) {
    await resetFormAsync(lbl_result);
    await showOrHideBackButtonAsync(
        div_backButton,
        div_panelTitle,
        btn_back);

    //#region reset info <div>s
    // hide <div>s
    div_senderInfos_inputs.attr("hidden", "");
    div_answererInfos_inputs.attr("hidden", "");
    formInfos_inputs.attr("hidden", "");

    // change "Gizle" text to "Görüntüle" text of <div>s
    updateElementText(
        div_senderInfos.find("h4 span"),
        "Görüntüle");
    updateElementText(
        div_answererInfos.find("h4 span"),
        "Görüntüle");
    updateElementText(
        div_demandInfos.find("h4 span"),
        "Görüntüle");
    //#endregion

    //#region show user display page
    div_article_update.attr("hidden", "");
    div_article_display.removeAttr("hidden");
    //#endregion
}
export async function click_articleAsync(
    event,
    inputIds,
    articleIdsAndInfos,
    div_article_display,
    div_article_update,
    div_backButton,
    div_panelTitle,
    div_senderInfos_inputs,
    div_answererInfos_inputs,
    btn_back,
    formStatus = "Unanswered|Accepted|Rejected",
    func_populateDemandInfosAsync = (infosOfLastClickedArticle) => { }
) {
    //#region save clicked article infos
    window.scrollTo(0, 0);  // take scroll to start of page

    let articleId = event.currentTarget.id;
    infosOfLastClickedArticle = articleIdsAndInfos[articleId];
    //#endregion

    await populateSenderInfosAsync(inputIds, div_senderInfos_inputs);

    //#region populate answerer infos
    if (formStatus == "Accepted"
        || formStatus == "Rejected")
        await populateAnswererInfosAsync(inputIds, div_answererInfos_inputs);
    //#endregion

    await func_populateDemandInfosAsync(infosOfLastClickedArticle);

    //#region show article update page
    div_article_display.attr("hidden", "");
    div_article_update.removeAttr("hidden");
    //#endregion

    await showOrHideBackButtonAsync(div_backButton, div_panelTitle, btn_back);
}
//#endregion

//#region functions
export async function beforePopulateAsync(articleWidth, articleHeight, div_articles) {
    await setArticleBufferAsync({
        div_articles: div_articles,
        articleType: "imageAndText",
        articleStyle: {
            "width": await getValidArticleWidthAsync({
                width: articleWidth,
                marginL: 20,
                marginR: 20
            }, div_articles),
            "height": articleHeight,
            "marginT": 10,
            "marginB": 10,
            "marginR": 20,
            "marginL": 20,
            "paddingT": 10,
            "paddingB": 10,
            "paddingR": 10,
            "paddingL": 10,
            "border": 1,
            "borderColor": "blue",
            "boxShadow": "5px 5px 10px rgba(0, 0, 0, 0.3)",
            "bgColorForDelete": "red"
        },
        heightOfPageMenubar: 80
    });  // i have to define article buffer before setting the page size.
    await setPageSizeAsync();
}
export async function populateArticlesAsync(
    specialUrl,
    headerKey,
    lbl_entityQuantity,
    func_populateInsideOfArticleAsync = (demands) => { },
    func_declareEventsAsync = () => { }
) {
    // populate article
    await new Promise((resolve) => {
        $.ajax({
            method: "GET",
            url: baseApiUrl + specialUrl,
            headers: {
                authorization: jwtToken
            },
            dataType: "json",
            beforeSend: () => {
                addMsgWithImgToDivArticlesAsync(
                    path.loadingImage,
                    "Yükleniyor",
                    "Yükleniyor...");
            },
            success: (demands, status, xhr) => {
                new Promise(async () => {
                    await resetDivArticlesAsync(); // remove loading img
                    await setArticleBufferAsync({
                        "totalArticleCount": demands.length
                    });
                    await addArticlesAsync(true);
                    await func_populateInsideOfArticleAsync(demands);
                    await setPagingBufferAsync({
                        infosInHeader: JSON.parse(
                            xhr.getResponseHeader(headerKey))
                    });  // save pagination infos
                    await updateEntityQuantityAsync(
                        lbl_entityQuantity,
                        pagingBuffer.infosInHeader.CurrentPageCount + "/" + pagingBuffer.pageSize
                    );
                    await addValueToPaginationLastButtonAsync(
                        pagingBuffer.infosInHeader.TotalPage);
                    await controlPaginationBackAndNextButtonsAsync(pagingBuffer.infosInHeader);
                    await func_declareEventsAsync();
                })
            },
            error: () => {
                addMsgWithImgToDivArticlesAsync(
                    path.questionImage,
                    "Talep Bulunamadı",
                    "Talep Bulunamadı"
                );
            },
            complete: () => {
                resolve();
            }
        })
    });  
}
export async function populateSenderInfosAsync(inputIds, div_senderInfos_inputs) {
    // get senderer infos and add to inputs
    $.ajax({
        method: "GET",
        url: (baseApiUrl + "/adminPanel/userDisplay/id?" +
            `userId=${infosOfLastClickedArticle.userId}` +
            `&checkIsDeleted=false`),
        headers: {
            authorization: jwtToken
        },
        contentType: "application/json",
        dataType: "json",
        success: (senderInfos) => {
            //#region populate inputs
            div_senderInfos_inputs.find("#" + inputIds.nameSurname).val(senderInfos.nameSurname);
            div_senderInfos_inputs.find("#" + inputIds.phone).val(senderInfos.phoneNumber);
            div_senderInfos_inputs.find("#" + inputIds.email).val(senderInfos.email);
            div_senderInfos_inputs.find("#" + inputIds.newPassportNo).val(
                getDefaultValueIfValueNullOrEmpty(senderInfos.newPassportNo));
            div_senderInfos_inputs.find("#" + inputIds.oldPassportNo).val(
                getDefaultValueIfValueNullOrEmpty(senderInfos.oldPassportNo));
            div_senderInfos_inputs.find("#" + inputIds.rank).val(
                getDefaultValueIfValueNullOrEmpty(senderInfos.rank));
            div_senderInfos_inputs.find("#" + inputIds.nationality).val(
                getDefaultValueIfValueNullOrEmpty(senderInfos.nationality));
            div_senderInfos_inputs.find("#" + inputIds.gender).val(
            getDefaultValueIfValueNullOrEmpty(senderInfos.gender));
            //#endregion
        },
    })
}
export async function populateAnswererInfosAsync(inputIds, div_answererInfos_inputs) {
    // get answerer infos and add to inputs
    $.ajax({
        method: "GET",
        url: (baseApiUrl + "/adminPanel/userDisplay/id?" +
            `userId=${infosOfLastClickedArticle.answererId}` +
            `&checkIsDeleted=false`),
        contentType: "application/json",
        dataType: "json",
        success: (answererInfos) => {
            //#region populate inputs
            new Promise(async resolve => {
                div_answererInfos_inputs.find("#" + inputIds.nameSurname).val(answererInfos.nameSurname);
                div_answererInfos_inputs.find("#" + inputIds.phone).val(answererInfos.phoneNumber);
                div_answererInfos_inputs.find("#" + inputIds.email).val(answererInfos.email);
                div_answererInfos_inputs.find("#" + inputIds.newPassportNo).val(
                    getDefaultValueIfValueNullOrEmpty(answererInfos.newPassportNo));
                div_answererInfos_inputs.find("#" + inputIds.oldPassportNo).val(
                    getDefaultValueIfValueNullOrEmpty(answererInfos.oldPassportNo));
                div_answererInfos_inputs.find("#" + inputIds.rank).val(
                    getDefaultValueIfValueNullOrEmpty(answererInfos.rank));
                div_answererInfos_inputs.find("#" + inputIds.nationality).val(
                    getDefaultValueIfValueNullOrEmpty(answererInfos.nationality));
                div_answererInfos_inputs.find("#" + inputIds.gender).val(
                    getDefaultValueIfValueNullOrEmpty(answererInfos.gender));
                div_answererInfos_inputs.find("#" + inputIds.answeredDate).val(
                    await convertStrUtcDateToStrLocalDateAsync(
                        infosOfLastClickedArticle.answeredDate,
                        { hours: true, minutes: true, seconds: false }));

                resolve();
            });
            //#endregion
        },
    })
}
export async function addInputsToInfoDivsAsync(inputInfos) {
    //#region add inputs to form
    for (let index in inputInfos) {
        let inputInfo = inputInfos[index];
        let div_formGroup_id;
        let inputId;

        switch (inputInfo[0]) {
            case "input":
                //#region add inputs to answerer, sender or demand <div>s
                div_formGroup_id = "div_" + inputInfo[2];
                inputId = "inpt_" + inputInfo[2];

                for (let indexOfDiv in inputInfo[6]) {
                    //#region add label and <input>
                    let divInfos = inputInfo[6][indexOfDiv];
                    let div = $("#" + divInfos.attr("id"));

                    div.append(
                        `<div id="${div_formGroup_id}" class="form-group">
                            <label class="col-sm-3 control-label">${inputInfo[3]}</label>
                            <div class="col-sm-6">
                                <input id="${inputId}" type="${inputInfo[1]}" class="form-control" ${inputInfo[5]} />
                                <span id="spn_help_${inputId}" class="help-block"></span>
                            </div>
                        </div>`);
                    //#endregion

                    //#region add info message if desired
                    if (inputInfo[4])
                        $("#" + div_formGroup_id).append(`
                            <div class="col-sm-3 div_infoMessage">
                                <button type="button" tabindex="-1" data-toggle="dropdown" class="dropdown-toggle"></button>
                                <ul role="menu" class="dropdown-menu"></ul>
                            </div>
                        `);
                    //#endregion
                }
                //#endregion

                break;
            case "textarea":
                //#region add inputs to answerer, sender or demand divs
                div_formGroup_id = "div_" + inputInfo[1];
                inputId = "txt_" + inputInfo[1];

                for (let indexOfDiv in inputInfo[5]) {
                    //#region add label and <textarea>
                    let divInfos = inputInfo[5][indexOfDiv];
                    let div = $("#" + divInfos.attr("id"));

                    div.append(`
                        <div id="${div_formGroup_id}" class="form-group">
                            <label class="col-sm-3 control-label">${inputInfo[2]}</label>
                            <div class="col-sm-6">
                                <textarea id="${inputId}" class="form-control" ${inputInfo[4]}></textarea>
                                <span id="spn_help_${inputId}" class="help-block"></span>
                            </div>
                        </div>
                    `);
                    //#endregion

                    //#region add info message if desired
                    if (inputInfo[3])
                        $("#" + div_formGroup_id).append(`
                            <div class="col-sm-3 div_infoMessage">
                                <button type="button" tabindex="-1" data-toggle="dropdown" class="dropdown-toggle"></button>
                                <ul role="menu" class="dropdown-menu"></ul>
                            </div>
                        `);
                    //#endregion
                }
                //#endregion

                break;
        }
    }
    //#endregion
}
export async function updateEntityQuantityAsync(lbl_entityQuantity, newQuantity) {
    let b_entityQuantity = lbl_entityQuantity.children("b");

    b_entityQuantity.empty();
    b_entityQuantity.append(newQuantity);
}
export async function setPageSizeAsync() {
    // firstly you have to define "articleBuffer" if is not initialized

    //#region set page size
    let articleCountOnOneRow = await getArticleCountOnOneRowAsync("px");
    let pageSize = (articleCountOnOneRow == 1 ?
        10
        : articleCountOnOneRow == 2 ?
            6 * 2  // column | row
            : articleCountOnOneRow == 3 ?
                5 * 3
                : 4 * articleCountOnOneRow);  // 5 -> 15;  6 -> 18;  7 -> 21...
    //#endregion

    await setPagingBufferAsync({
        pageSize: pageSize
    });
}
export async function addImageToArticleAsync(
    articleId,
    yachtType,
    imageWidthRate,  // ex: 65 / 100
    imageHeightRate  
) {
    //#region set image path by yacht type
    let path_image = "";

    switch (yachtType) {
        case "Catamaran":
            path_image = "/images/catamaranYacht.png";
            break;
        case "SailingYacht":
            path_image = "/images/sailingYacht.png"
            break;
        case "MotorYacht":
            path_image = "/images/motorYacht.png";
            break;
        default:  // when yacht type is null or invalid
            path_image = "/images/noImage.png";
            break;
    }
    //#endregion

    //#region add image to article
    let div_article_image = $("#" + articleId + " #" + div_article_image_id);
    let img_width = div_article_image.prop("offsetWidth") * imageWidthRate;  // %n of width
    let img_height = div_article_image.prop("offsetHeight") * imageHeightRate;  // %n of height

    div_article_image.append(
        `<img src="${path_image}" style="width:${img_width}px; height:${img_height}px"></img>`
    );
    //#endregion

    await alignImageToVerticalCenterAsync(articleId);
}
export async function resetDivArticlesAsync() {
    articleBuffer.div_articles.empty();
    articleBuffer.div_articles.removeAttr("style");
}
export function getDefaultValueIfValueNullOrEmpty(value) {
    return value == null || value == "" ?
        "Girilmedi"
        : value;
}
//#endregion