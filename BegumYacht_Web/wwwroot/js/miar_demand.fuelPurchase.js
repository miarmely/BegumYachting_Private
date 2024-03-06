import { populateInfoMessagesAsync } from "./miar_module.userForm.js"
import { convertStrUtcDateToStrLocalDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";
import { addCriticalSectionAsync, shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js"

import {
    addImageToArticleAsync, beforePopulateAsync, click_articleAsync, click_backButtonAsync,
    click_InfoDivAsync, getDefaultValueIfValueNull, inpt_id, populateArticlesAsync,
    addInputsToInfoDivsAsync, resize_windowAsync, txt_id,
} from "./miar_demand.js"

import {
    alignArticlesToCenterAsync, art_baseId, controlArticleWidthAsync, div_article_info_id,
    setHeightOfArticlesDivAsync
} from "./miar_module.article.js"

import {
    change_inpt_paginationCurrentAsync, click_ul_paginationAsync, keyup_ul_paginationAsync,
    inpt_paginationCurrent_id, pagingBuffer
} from "./miar_module.pagination.js"


$(function () {
    //#region variables
    const ul_pagination = $("#ul_pagination");
    const p_resultLabel = $("#p_resultLabel");
    const headerKeys = {
        fuelPurchase: "Demand-FuelPurchase"
    };
    const criticalSectionIds = {
        sidebarMenuButton: "sidebarMenuButton",
        window: "window",
        backButton: "backButton"
    }
    const div = {
        article_update: $("#div_article_update"),
        article_display: $("#div_article_display"),
        articles: $("#div_articles"),
        sidebarMenuButton: $("#div_sidebarMenuButton"),
        answererInfos: $("#div_answererInfos"),
        backButton: $("#div_backButton"),
        panelTitle: $("#div_panelTitle"),
        senderInfos_inputs: $("#div_senderInfos_inputs"),
        answererInfos_inputs: $("#div_answererInfos_inputs"),
        demandInfos_inputs: $("#div_demandInfos_inputs"),
    };
    const btn = {
        back: $("#btn_back")
    };
    const lbl = {
        entityQuantity: $("#small_entityQuantity")
    };
    const slct = {
        article_submenu_display: $("#slct_article_submenu_display")
    };
    const formType = "FuelPurchaseDemand";
    let articleIdsAndInfos = {};
    let formStatus = "Unanswered";
    //#endregion

    //#region events

    //#region partner
    $(window).resize(async () => {
        await resize_windowAsync(
            div.article_display,
            criticalSectionIds.window);
    })
    div.sidebarMenuButton.click(async () => {
        //#region when display page is opened
        if (div.article_display.attr("hidden") == null)
            await addCriticalSectionAsync(
                criticalSectionIds.sidebarMenuButton,
                async () => {
                    await controlArticleWidthAsync();
                    await alignArticlesToCenterAsync("px");
                    await setHeightOfArticlesDivAsync()
                },
                500);
        //#endregion
    })
    $("#" + inpt_paginationCurrent_id).on("input", async () => {
        await change_inpt_paginationCurrentAsync();
    })
    //#endregion

    //#region display page
    ul_pagination.click(async (event) => {
        await click_ul_paginationAsync(
            event,
            populateFuelPurchaseArticlesAsync);
    })
    ul_pagination.keyup(async (event) => {
        await keyup_ul_paginationAsync(
            event,
            populateFuelPurchaseArticlesAsync);
    })
    slct.article_submenu_display.change(async () => {
        //#region show/hide anserer infos <div>
        formStatus = slct.article_submenu_display.val();

        // show
        if (formStatus == "Accepted"
            || formStatus == "Rejected")
            div.answererInfos.removeAttr("hidden");

        // hide
        else
            div.answererInfos.attr("hidden", "");
        //#endregion

        await populateFuelPurchaseArticlesAsync();
    })
    spn_eventManager.on("click_article", async (_, event) => {
        await click_articleAsync(
            event,
            articleIdsAndInfos,
            div.article_display,
            div.article_update,
            div.backButton,
            div.panelTitle,
            div.senderInfos_inputs,
            div.answererInfos_inputs,
            btn.back,
            formStatus,
            async (infosOfLastClickedArticle) => {
                div.demandInfos_inputs.find("#" + inpt_id.yachtName).val(
                    getDefaultValueIfValueNull(infosOfLastClickedArticle.yachtName));
                div.demandInfos_inputs.find("#" + inpt_id.yachtType).val(
                    getDefaultValueIfValueNull(infosOfLastClickedArticle.yachtType));
                div.demandInfos_inputs.find("#" + inpt_id.flag).val(
                    getDefaultValueIfValueNull(infosOfLastClickedArticle.flag));
                div.demandInfos_inputs.find("#" + inpt_id.isDutyPaid).val(infosOfLastClickedArticle.isDutyPaid);
                div.demandInfos_inputs.find("#" + inpt_id.mgo).val(infosOfLastClickedArticle.mgo);
                div.demandInfos_inputs.find("#" + inpt_id.ago).val(infosOfLastClickedArticle.ago);
                div.demandInfos_inputs.find("#" + inpt_id.fuelType).val(infosOfLastClickedArticle.fuelType);
                div.demandInfos_inputs.find("#" + inpt_id.requestedFuel).val(infosOfLastClickedArticle.requestedFuel);
                div.demandInfos_inputs.find("#" + inpt_id.fuelSupplyPort).val(infosOfLastClickedArticle.fuelSupplyPort);
                div.demandInfos_inputs.find("#" + inpt_id.fuelSupplyDate).val(
                    await convertStrUtcDateToStrLocalDateAsync(
                        infosOfLastClickedArticle.fuelSupplyDate,
                        { hours: true, minutes: true, seconds: false }));
                div.demandInfos_inputs.find("#" + inpt_id.createdDate).val(
                    await convertStrUtcDateToStrLocalDateAsync(
                        infosOfLastClickedArticle.createdDate,
                        { hours: true, minutes: true, seconds: false }));
                div.demandInfos_inputs.find("#" + txt_id.notes).val(
                    getDefaultValueIfValueNull(infosOfLastClickedArticle.notes));
            }  // populate demand inputs
        );
    })
    //#endregion

    //#region update page
    $(".div_infos_button").click(async (event) => {
        await click_InfoDivAsync(event);
    })
    btn.back.click(async () => {
        await click_backButtonAsync(
            p_resultLabel,
            div.backButton,
            div.panelTitle,
            div.article_update,
            div.article_display,
            div.senderInfos_inputs,
            div.answererInfos_inputs,
            div.demandInfos_inputs,
            btn.back);
        await alignArticlesToCenterAsync();
    })
    //#endregion

    //#endregion

    //#region functions
    async function setupPageAsync() {
        await beforePopulateAsync(div.articles);
        await populateFuelPurchaseArticlesAsync();
        await addInputsToInfoDivsAsync(
            div.senderInfos_inputs,
            div.answererInfos_inputs,
            div.demandInfos_inputs,
            div.answererInfos,
            formStatus);
        await populateInfoMessagesAsync({
            div_senderInfos: ["Şeklin üzerine tıklayarak talebi gönderen personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_answererInfos: ["Şeklin üzerine tıklayarak talebe cevap veren personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_demandInfos: ["Şeklin üzerine tıklayarak talep bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_notes: ["şekli, sağ alt ucundan aşağı doğru çekerek uzatabilirsin.",]
        });
    }
    async function populateFuelPurchaseArticlesAsync() {
        await populateArticlesAsync(
            "/adminPanel/fuelPurchaseDemand/filter?" + (
                `pageSize=${pagingBuffer.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}` +
                `&formType=${formType}` +
                `&formStatus=${formStatus}`),
            headerKeys.fuelPurchase,
            lbl.entityQuantity,
            async (demands) => {
                //#region populate inside of articles
                for (let index in demands) {
                    //#region set variables
                    let articleId = art_baseId + index;
                    let article = $("#" + articleId);
                    let demandInfos = demands[index];

                    // save demand infos
                    articleIdsAndInfos[articleId] = demandInfos;
                    //#endregion

                    await addImageToArticleAsync(articleId, demandInfos.yachtType);

                    //#region add article infos 
                    let div_article_info = article.children("#" + div_article_info_id);

                    div_article_info.append(`
                        <div>
                            <h2>${getDefaultValueIfValueNull(demandInfos.flag)}</h2>
                            <h3 style="margin-top:3px">${getDefaultValueIfValueNull(demandInfos.yachtType)}</h3 >
                            <h4 style="margin-top:2px">${getDefaultValueIfValueNull(demandInfos.yachtName)}</h4>
                            <h4 style="margin-top:20px">${demandInfos.nameSurname}</h4>
                            <h6 style="margin-top:10px">${demandInfos.notes.substring(0, 200)}...</h6>
                        </div>
                        <div id="div_passedTime">${await getPassedTimeInStringAsync(demandInfos.createdDate)}</div>

                    `);
                    //#endregion

                    await shiftTheChildDivToBottomOfParentDivAsync(
                        div_article_info,
                        "div_passedTime");  // shift the passed time to bottom
                }
                //#endregion
            },
            async () => {
                //#region declare events
                $(".article").click(event => {
                    spn_eventManager.trigger("click_article", [event]);
                })
                //#endregion
            }
        );
    }
    //#endregion

    setupPageAsync();
})