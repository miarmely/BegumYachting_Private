import { addCriticalSectionAsync } from "./miar_module.js";
import { resetFormAsync, showOrHideBackButtonAsync } from "./miar_module.userForm.js";

import {
    addMsgWithImgToDivArticlesAsync, alignArticlesToCenterAsync, alignImageToVerticalCenterAsync,
    controlArticleWidthAsync, div_article_image_id, getArticleCountOnOneRowAsync,
    setHeightOfArticlesDivAsync, articleBuffer, setArticleBufferAsync, getValidArticleWidthAsync, addArticlesAsync
} from "./miar_module.article.js";

import { addValueToPaginationLastButtonAsync, controlPaginationBackAndNextButtonsAsync, pagingBuffer, setPagingBufferAsync } from "./miar_module.pagination.js";


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
const path = {
    loadingImage: "./images/loading.gif",
    questionImage: "./images/question.png"
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
export async function change_articleMenuSelectAsync() {}
export async function change_articleSubMenuSelectAsync(slct_article_submenu) {

    //$.ajax({
    //    method: "GET",
    //    url: (baseApiUrl + "/adminPanel/fuelPurchaseDemand/filter` +
    //        `?demandStatus=${slct_article_submenu.val()}`
    //        `&pageSize=${pagingBuffer.pageSize}` +
    //        `&pageNumber=${pagingBuffer.pageNumber}`),
    //    contentType = "application/json",
    //    dataType = "json",
    //    beforeSend: () => {
    //        addMsgWithImgToDivArticlesAsync(
    //            path.loadingImage,
    //            "Yükleniyor",
    //            "Yükleniyor...");
    //    },
    //    success: (demands) => {
    //        new Promise(async () => {
    //            await resetDivArticlesAsync();
    //            alert("successful");
    //        })
    //    },
    //    error: (response) => {
    //        addMsgWithImgToDivArticlesAsync(
    //            path.questionImage,
    //            "Form Bulunamadı",
    //            "Form Bulunamadı...");
    //    }
    //})
}
//#endregion

//#region functions
export async function beforePopulateAsync(div_articles) {
    await setArticleBufferAsync({
        div_articles: div_articles,
        articleType: "imageAndText",
        articleStyle: {
            "width": await getValidArticleWidthAsync({
                width: 300,
                marginL: 20,
                marginR: 20
            }, div_articles),
            "height": 550,
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
    await new Promise((resolve) => {
        $.ajax({
            method: "GET",
            url: baseApiUrl + specialUrl,
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
    });  // populate article
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
export async function resetDivArticlesAsync() {
    articleBuffer.div_articles.empty();
    articleBuffer.div_articles.removeAttr("style");
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