import { populateInfoMessagesAsync } from "./miar_module.userForm.js"
import { convertDateToStrDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";
import { addCriticalSectionAsync, shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js"

import {
    addImageToArticleAsync, beforePopulateAsync, click_articleAsync,
    click_backButtonAsync, click_InfoDivAsync, getDefaultValueIfValueNullOrEmpty,
    populateArticlesAsync, addInputsToInfoDivsAsync, resize_windowAsync, click_sidebarMenuAsync
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
    const formType = "CheckinAndCheckoutDemand";
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
        ["input", "text", "checkinDate", "Giriş Tarihi", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "arrivalPort", "Giriş Yeri", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "checkoutDate", "Çıkış Tarihi", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "departurePort", "Varış Yeri", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "createdDate", "Talep Tarihi", false, "readonly", [div.demandInfos_inputs]],
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
        checkinDate: "inpt_checkinDate",
        arrivalPort: "inpt_arrivalPort",
        checkoutDate: "inpt_checkoutDate",
        departurePort: "inpt_departurePort"
    };
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
        await click_sidebarMenuAsync(
            div.article_display,
            criticalSectionIds.sidebarMenuButton);
    })
    $("#" + inpt_paginationCurrent_id).on("input", async () => {
        await change_inpt_paginationCurrentAsync();
    })
    //#endregion

    //#region display page
    ul_pagination.click(async (event) => {
        await click_ul_paginationAsync(
            event,
            populateCheckinAndCheckoutArticlesAsync);
    })
    ul_pagination.keyup(async (event) => {
        await keyup_ul_paginationAsync(
            event,
            populateCheckinAndCheckoutArticlesAsync);
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

        await populateCheckinAndCheckoutArticlesAsync();
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
                div.demandInfos_inputs.find("#" + inpt_id.checkinDate).val(
                    await convertDateToStrDateAsync(
                        new Date(infosOfLastClickedArticle.checkInDate),
                        { hours: true, minutes: true, seconds: false }));
                div.demandInfos_inputs.find("#" + inpt_id.arrivalPort).val(
                    infosOfLastClickedArticle.arrivalPort);
                div.demandInfos_inputs.find("#" + inpt_id.checkoutDate).val(
                    await convertDateToStrDateAsync(
                        new Date(infosOfLastClickedArticle.checkOutDate),
                        { hours: true, minutes: true, seconds: false }));
                div.demandInfos_inputs.find("#" + inpt_id.departurePort).val(
                    infosOfLastClickedArticle.departurePort);
                div.demandInfos_inputs.find("#" + inpt_id.createdDate).val(
                    await convertDateToStrDateAsync(
                        new Date(infosOfLastClickedArticle.createdDate),
                        { hours: true, minutes: true, seconds: false }));
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
        await beforePopulateAsync(300, 400, div.articles);
        await populateCheckinAndCheckoutArticlesAsync();
        await addInputsToInfoDivsAsync(inputInfos);
        await populateInfoMessagesAsync({
            div_senderInfos: ["Şeklin üzerine tıklayarak talebi gönderen personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_answererInfos: ["Şeklin üzerine tıklayarak talebe cevap veren personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_demandInfos: ["Şeklin üzerine tıklayarak talep bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
        });
    }
    async function populateCheckinAndCheckoutArticlesAsync() {
        await populateArticlesAsync(
            "/adminPanel/demand/checkinAndCheckout/filter?" + (
                `pageSize=${pagingBuffer.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}` +
                `&formStatus=${formStatus}`),
            headerKeys.demand.checkinAndCheckout,
            lbl.entityQuantity,
            async (demands) => {
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
                        55 / 100,
                        70 / 100);

                    //#region add article infos 
                    let div_article_info = article.children("#" + div_article_info_id);
                    let div_passedTime_id = "div_passedTime";

                    div_article_info.append(`
                        <div>
                            <h3>Giriş: ${demandInfos.arrivalPort}</h3>
                            <h3>Çıkış: ${demandInfos.departurePort}</h3>
                            <h4 style="margin-top:15px">${getDefaultValueIfValueNullOrEmpty(demandInfos.yachtType)}</h3 >
                            <h4 style="margin-top:2px">${getDefaultValueIfValueNullOrEmpty(demandInfos.yachtName)}</h5>
                            <h5 style="margin-top:10px; font-size: 14.5px">${demandInfos.nameSurname}</h5>
                        </div>
                        <div id="${div_passedTime_id}">${await getPassedTimeInStringAsync(null, demandInfos.createdDate)}</div>
                    `);
                    //#endregion

                    await shiftTheChildDivToBottomOfParentDivAsync(
                        div_article_info,
                        div_passedTime_id);  // shift the passed time to bottom
                }
            },  // populate inside of articles
            async () => {
                $(".article").click(event => {
                    spn_eventManager.trigger("click_article", [event]);
                })
            }  // declare events
        );
    }
    //#endregion

    setupPageAsync();
})