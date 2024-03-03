import { addCriticalSectionAsync } from "./miar_module.js";
import { resetFormAsync, showOrHideBackButtonAsync } from "./miar_module.userForm.js";

import {
    alignArticlesToCenterAsync, alignImageToVerticalCenterAsync, controlArticleWidthAsync,
    div_article_image_id, getArticleCountOnOneRowAsync, setHeightOfArticlesDivAsync
} from "./miar_module.article.js";


//#region variables
export let infosOfLastClickedArticle = {};
const elementNamesAndPropertyNames = {
    nameSurname: "nameSurname",
    phone: "phoneNumber",
    email: "email",
    newPassportNo: "newPassportNo",
    oldPassportNo: "oldPassportNo",
    rank: "rank",
    nationality: "nationality",
    gender: "gender"
};
const css_inputsOfSenderInfos = {
    "border-color": "#4136f1",
    "border-width": "1.5px"
}
let isSenderInfosDisplaying = false;
let isSenderInfosLoadedBefore = false;
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
export async function click_senderInfosDivAsync() {
    //#region when sender infos is not loaded to inputs before (ajax)
    if (!isSenderInfosLoadedBefore)
        await new Promise(resolve => {
            $.ajax({
                method: "GET",
                url: baseApiUrl + `/adminPanel/userInfos?userId=${infosOfLastClickedArticle.userId}`,
                contentType: "application/json",
                dataType: "json",
                success: (userInfos) => {
                    //#region populate inputs belong to sender infos
                    for (let elementName in elementNamesAndPropertyNames) {
                        let propertyName = elementNamesAndPropertyNames[elementName];
                        let inpt = $("#inpt_" + elementName)

                        inpt.val(userInfos[propertyName]);
                        inpt.css(css_inputsOfSenderInfos);
                    }

                    isSenderInfosLoadedBefore = true;
                    //#endregion
                },
                complete: () => {
                    resolve();
                }
            });
        });
    //#endregion

    await showOrHideInputsOfSenderInfosAsync();
}
export async function click_backButtonAsync(
    lbl_result,
    div_backButton,
    div_panelTitle,
    div_article_update,
    div_article_display,
    btn_back
) {
    await resetFormAsync(lbl_result);
    await hideInputsOfSenderInfosAsync();
    await showOrHideBackButtonAsync(
        div_backButton,
        div_panelTitle,
        btn_back);

    //#region show user display page
    isSenderInfosLoadedBefore = false;

    div_article_update.attr("hidden", "");
    div_article_display.removeAttr("hidden");
    //#endregion
}
export async function click_articleAsync(
    event,
    articleIdsAndInfos,
    div_article_display,
    div_article_update,
    div_backButton,
    div_panelTitle,
    btn_back,
    func_populateDemandInputsAsync
) {
    //#region save clicked article infos
    window.scrollTo(0, 0);  // take scroll to start of page

    let articleId = event.currentTarget.id;
    infosOfLastClickedArticle = articleIdsAndInfos[articleId];
    //#endregion

    await func_populateDemandInputsAsync(infosOfLastClickedArticle);

    //#region show article update page
    div_article_display.attr("hidden", "");
    div_article_update.removeAttr("hidden");
    //#endregion

    await showOrHideBackButtonAsync(div_backButton, div_panelTitle, btn_back);
}
//#endregion

//#region functions
export async function updateEntityQuantityAsync(lbl_entityQuantity, newQuantity) {
    let b_entityQuantity = lbl_entityQuantity.children("b");

    b_entityQuantity.empty();
    b_entityQuantity.append(newQuantity);
}
export async function getPageSizeAsync() {
    let articleCountOnOneRow = await getArticleCountOnOneRowAsync("px");

    let pageSize = (articleCountOnOneRow == 1 ?
        10
        : articleCountOnOneRow == 2 ?
            6 * 2  // column | row
            : articleCountOnOneRow == 3 ?
                5 * 3
                : 4 * articleCountOnOneRow);  // 5 -> 15;  6 -> 18;  7 -> 21...

    return pageSize;
}
export async function addImageToArticleAsync(articleId, yachtType) {
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
    }
    //#endregion

    //#region add image to article
    let div_article_image = $("#" + articleId + " #" + div_article_image_id);
    let img_width = div_article_image.prop("offsetWidth") * (65 / 100);  // %n of width
    let img_height = div_article_image.prop("offsetHeight") * (65 / 100);  // %n of height

    div_article_image.append(
        `<img src="${path_image}" style="width:${img_width}px; height:${img_height}px"></img>`
    );
    //#endregion

    await alignImageToVerticalCenterAsync(articleId);
}
export async function resetDivArticlesAsync(div_articles) {
    div_articles.empty();
    div_articles.removeAttr("style");
}
async function showInputsOfSenderInfosAsync() {
    // show inputs
    for (let elementName in elementNamesAndPropertyNames)
        $("#div_" + elementName).removeAttr("hidden");

    isSenderInfosDisplaying = true;
}
async function hideInputsOfSenderInfosAsync() {
    // hide inputs
    for (let elementName in elementNamesAndPropertyNames)
        $("#div_" + elementName).attr("hidden", "");

    isSenderInfosDisplaying = false;
}
async function showOrHideInputsOfSenderInfosAsync() {
    if (isSenderInfosDisplaying)
        await hideInputsOfSenderInfosAsync();

    else
        await showInputsOfSenderInfosAsync();
}
//#endregion