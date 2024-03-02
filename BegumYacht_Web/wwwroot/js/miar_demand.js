import {
    alignImageToVerticalCenterAsync, div_article_image_id,
    getArticleCountOnOneRowAsync
} from "./miar_module.article.js";

//#region variables
const divIdsOfSenderInfos = [
    "div_nameSurname",
    "div_phone",
    "div_email",
    "div_newPassportNo",
    "div_oldPassportNo",
    "div_rank",
    "div_nationality",
    "div_gender"
];
let isSenderInfosDisplaying = false;
let isSenderInfosLoadedToInputs = false;
//#endregion

//#region events
export async function click_senderInfosDivAsync(userId) {
    ////////////////////////// BUILDING.... //////////////////////////

    $.ajax({

    });

    //////////////////////////////////////////////////////////////////

    //#region show/hide inputs belong to senderer infos

    //#region when sender infos is not displaying
    if (!isSenderInfosDisplaying) {
        // show inputs
        for (let index in divIdsOfSenderInfos)
            $("#" + divIdsOfSenderInfos[index]).removeAttr("hidden");

        isSenderInfosDisplaying = true;
        isSenderInfosLoadedToInputs = true;
    }
    //#endregion

    //#region when sender infos is displaying
    else {
        // hide inputs
        for (let index in divIdsOfSenderInfos)
            $("#" + divIdsOfSenderInfos[index]).attr("hidden", "");

        isSenderInfosDisplaying = false;
    }
    //#endregion

    //#endregion
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
//#endregion