import { populateInfoMessagesAsync } from "./miar_module.userForm.js"
import { convertDateToStrDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";
import { addCriticalSectionAsync, shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js"

import {
    addImageToArticleAsync, beforePopulateAsync, click_articleAsync, resize_windowAsync,
    click_backButtonAsync, click_InfoDivAsync, getDefaultValueIfValueNullOrEmpty,
    populateArticlesAsync, addInputsToInfoDivsAsync,
} from "./miar_form.js"

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
        senderInfos: $("#div_senderInfos"),
        answererInfos: $("#div_answererInfos"),
        demandInfos: $("#div_demandInfos"),
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
    const inputInfos = [
        ["input", "text", "nameSurname", "Ad Soyad", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],  // type for switch/case | type for switch/case | type for input | id | label name | info message | hidden/disabled/readonly of input | place to add
        ["input", "text", "phone", "Telefon", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],
        ["input", "text", "email", "Email", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],
        ["input", "text", "newPassportNo", "Yeni Pasaport No", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],
        ["input", "text", "oldPassportNo", "Eski Pasapart No", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],
        ["input", "text", "rank", "Rank", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],
        ["input", "text", "nationality", "Uyruk", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],
        ["input", "text", "gender", "Cinsiyet", false, "readonly", [div.senderInfos_inputs, div.answererInfos_inputs]],
        ["input", "text", "answeredDate", "Cevaplanma Tarihi", false, "readonly", [div.answererInfos_inputs]],
        ["input", "text", "yachtType", "Yat Tipi", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "yachtName", "Yat Adı", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "flag", "Bayrak", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "isDutyPaid", "Gümrüklü Mü", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "mgo", "MGO", false, "readonly", [div.demandInfos_inputs]],  // marine gas oil
        ["input", "text", "ago", "AGO", false, "readonly", [div.demandInfos_inputs]],  // automotive gas oil
        ["input", "text", "fuelType", "Yakıt Tipi", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "requestedFuel", "İstenen Yakıt Miktarı (L)", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "fuelSupplyPort", "Yakıt İkmal Yeri", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "fuelSupplyDate", "Yakıt İkmal Tarihi", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "createdDate", "Talep Tarihi", false, "readonly", [div.demandInfos_inputs]],
        ["textarea", "notes", "Notlar", false, "readonly", [div.demandInfos_inputs]]  // type for switch/case | id | label name | info message | hidden/disabled/readonly of input | place to add
    ];
    const inpt_id = {
        nameSurname: "inpt_nameSurname",
        phone: "inpt_phone",
        email: "inpt_email",
        newPassportNo: "inpt_newPassportNo",
        oldPassportNo: "inpt_oldPassportNo",
        rank: "inpt_rank",
        nationality: "inpt_nationality",
        gender: "inpt_gender",
        createdDate: "inpt_createdDate",
        answeredDate: "inpt_answeredDate",
        yachtName: "inpt_yachtName",
        yachtType: "inpt_yachtType",
        flag: "inpt_flag",
        isDutyPaid: "inpt_isDutyPaid",
        mgo: "inpt_mgo",
        ago: "inpt_ago",
        fuelType: "inpt_fuelType",
        requestedFuel: "inpt_requestedFuel",
        fuelSupplyPort: "inpt_fuelSupplyPort",
        fuelSupplyDate: "inpt_fuelSupplyDate",
    };
    const txt_id = {
        notes: "txt_notes"
    }
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
    })  // DISABLED
    spn_eventManager.on("click_article", async (_, event) => {
        await click_articleAsync(
            event,
            inpt_id,
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
                    getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtName));
                div.demandInfos_inputs.find("#" + inpt_id.yachtType).val(
                    getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtType));
                div.demandInfos_inputs.find("#" + inpt_id.flag).val(
                    getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.flag));
                div.demandInfos_inputs.find("#" + inpt_id.isDutyPaid).val(infosOfLastClickedArticle.isDutyPaid);
                div.demandInfos_inputs.find("#" + inpt_id.mgo).val(infosOfLastClickedArticle.mgo);
                div.demandInfos_inputs.find("#" + inpt_id.ago).val(infosOfLastClickedArticle.ago);
                div.demandInfos_inputs.find("#" + inpt_id.fuelType).val(infosOfLastClickedArticle.fuelType);
                div.demandInfos_inputs.find("#" + inpt_id.requestedFuel).val(infosOfLastClickedArticle.requestedFuel);
                div.demandInfos_inputs.find("#" + inpt_id.fuelSupplyPort).val(infosOfLastClickedArticle.fuelSupplyPort);
                div.demandInfos_inputs.find("#" + inpt_id.fuelSupplyDate).val(
                    await convertDateToStrDateAsync(
                        new Date(infosOfLastClickedArticle.fuelSupplyDate),
                        { hours: true, minutes: true, seconds: false }));
                div.demandInfos_inputs.find("#" + inpt_id.createdDate).val(
                    await convertDateToStrDateAsync(
                        new Date(infosOfLastClickedArticle.createdDate),
                        { hours: true, minutes: true, seconds: false }));
                div.demandInfos_inputs.find("#" + txt_id.notes).val(
                    getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.notes));
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
            div.senderInfos,
            div.answererInfos,
            div.demandInfos,
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
        await beforePopulateAsync(300, 550, div.articles);
        await populateFuelPurchaseArticlesAsync();
        await addInputsToInfoDivsAsync(inputInfos);
        await populateInfoMessagesAsync({
            div_senderInfos: ["Şeklin üzerine tıklayarak talebi gönderen personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_answererInfos: ["Şeklin üzerine tıklayarak talebe cevap veren personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_demandInfos: ["Şeklin üzerine tıklayarak talep bilgilerini görüntüleyebilir veya gizleyebilirsin.",]
        });
    }
    async function populateFuelPurchaseArticlesAsync() {
        await populateArticlesAsync(
            "/adminPanel/demand/fuelPurchase/filter?" + (
                `pageSize=${pagingBuffer.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}` +
                `&formStatus=${formStatus}`),
            headerKeys.demand.fuelPurchase,
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

                    await addImageToArticleAsync(
                        articleId,
                        demandInfos.yachtType,
                        65 / 100,
                        65 / 100
                    );

                    //#region add article infos 
                    let div_article_info = article.children("#" + div_article_info_id);

                    div_article_info.append(`
                        <div>
                            <h2>${getDefaultValueIfValueNullOrEmpty(demandInfos.flag)}</h2>
                            <h3 style="margin-top:3px">${getDefaultValueIfValueNullOrEmpty(demandInfos.yachtType)}</h3 >
                            <h4 style="margin-top:2px">${getDefaultValueIfValueNullOrEmpty(demandInfos.yachtName)}</h4>
                            <h4 style="margin-top:20px">${demandInfos.nameSurname}</h4>
                            <h6 style="margin-top:10px">${demandInfos.notes.substring(0, 200)}...</h6>
                        </div>
                        <div id="div_passedTime">${await getPassedTimeInStringAsync(null, demandInfos.createdDate)}</div>
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