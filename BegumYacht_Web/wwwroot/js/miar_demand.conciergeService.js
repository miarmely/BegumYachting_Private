import { populateInfoMessagesAsync } from "./miar_module.userForm.js"
import { convertDateToStrDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";
import { addCriticalSectionAsync, shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js"

import {
    addImageToArticleAsync, beforePopulateAsync, click_articleAsync, resize_windowAsync,
    click_backButtonAsync, click_InfoDivAsync, getDefaultValueIfValueNullOrEmpty,
    populateArticlesAsync, addInputsToInfoDivsAsync
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
    const formType = "ConciergeServiceDemand";
    const inputInfos = [
        ["input", "text", "nameSurname", "Ad Soyad", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],  // type for switch/case | type for switch/case | type for input | id | label name | info message | hidden/disabled/readonly of input | place to add
        ["input", "text", "phone", "Telefon", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],
        ["input", "text", "email", "Email", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],
        ["input", "text", "newPassportNo", "Yeni Pasaport No", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],
        ["input", "text", "oldPassportNo", "Eski Pasapart No", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],
        ["input", "text", "rank", "Rank", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],
        ["input", "text", "nationality", "Uyruk", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],
        ["input", "text", "gender", "Cinsiyet", false, "readonly", [div_senderInfos_inputs, div_answererInfos_inputs]],
        ["input", "text", "answeredDate", "Cevaplanma Tarihi", false, "readonly", [div_answererInfos_inputs]],
        ["input", "text", "yachtType", "Yat Tipi", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "yachtName", "Yat Adı", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "flag", "Bayrak", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "restaurantName", "Restaurant Adı", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "conciergeDate", "Konsiyerj Hizmeti Tarihi", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "numberOfPeople", "Yolcu Sayısı", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "transportationPreference", "Ulaşım Tercihi", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "transferCarType", "Transfer Aracı Tipi", false, "readonly", [div_demandInfos_inputs]],
        ["input", "text", "createdDate", "Talep Tarihi", false, "readonly", [div_demandInfos_inputs]],
    ];  // for add <input>s and <textarea>s to form
    const inputIds = {
        nameSurname: "inpt_nameSurname",
        phone: "inpt_phone",
        email: "inpt_email",
        newPassportNo: "inpt_newPassportNo",
        oldPassportNo: "inpt_oldPassportNo",
        rank: "inpt_rank",
        nationality: "inpt_nationality",
        gender: "inpt_gender",
        answeredDate: "inpt_answeredDate",
        yachtName: "inpt_yachtName",
        yachtType: "inpt_yachtType",
        flag: "inpt_flag",
        restaurantName: "inpt_restaurantName",
        conciergeDate: "inpt_conciergeDate",
        numberOfPeople: "inpt_numberOfPeople",
        transportationPreference: "inpt_transportationPreference",
        transferCarType: "inpt_transferCarType",
        createdDate: "inpt_createdDate",
    };  // for populate <input>s and <texarea>s
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
            populateDemandArticlesAsync);
    })
    ul_pagination.keyup(async (event) => {
        await keyup_ul_paginationAsync(
            event,
            populateDemandArticlesAsync);
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

        await populateDemandArticlesAsync();
    })  // DISABLED
    spn_eventManager.on("click_article", async (_, event) => {
        await click_articleAsync(
            event,
            inputIds,
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
                //#region set form infos
                let conciergeDateInStr = getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.conciergeDate);
                let createdDateInStr = getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.createdDate);

                let formInfos = {
                    yachtName: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtName),
                    yachtType: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtType),
                    flag: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.flag),
                    restaurantName: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.restaurantName),
                    conciergeDate: (conciergeDateInStr == infosOfLastClickedArticle.conciergeDate ?
                        await convertDateToStrDateAsync(
                            new Date(conciergeDateInStr),
                            { hours: false, minutes: false, seconds: false }) // when date is not null or empty
                        : conciergeDateInStr),  // when date is null or empty,,
                    numberOfPeople: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.numberOfPeople),
                    transportationPreference: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.transportationPreference),
                    transferCarType: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.transferCarType),
                    createdDate: (createdDateInStr == infosOfLastClickedArticle.createdDate ?
                        await convertDateToStrDateAsync(
                            new Date(createdDateInStr),
                            { hours: true, minutes: true, seconds: false }) // when date is not null or empty
                        : createdDateInStr),  // when date is null or empty,
                };
                //#endregion

                //#region populate inputs (DYNAMICALLY)
                for (let elementName in formInfos) 
                    div.demandInfos_inputs
                        .find("#" + inputIds[elementName])
                        .val(formInfos[elementName]);
                //#endregion

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
        await beforePopulateAsync(300, 420, div.articles);
        await populateDemandArticlesAsync();
        await addInputsToInfoDivsAsync(inputInfos);
        await populateInfoMessagesAsync({
            div_senderInfos: ["Şeklin üzerine tıklayarak talebi gönderen personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_answererInfos: ["Şeklin üzerine tıklayarak talebe cevap veren personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_demandInfos: ["Şeklin üzerine tıklayarak talep bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
        });
    }
    async function populateDemandArticlesAsync() {
        await populateArticlesAsync(
            "/adminPanel/conciergeService/filter?" + (
                `pageSize=${pagingBuffer.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}` +
                `&formStatus=${formStatus}`),
            headerKeys.conciergeService,
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
                        60 / 100,
                        70 / 100);

                    //#region set article Infos 
                    let articleInfos = {
                        restaurantName: getDefaultValueIfValueNullOrEmpty(demandInfos.restaurantName),
                        transportationPreference: getDefaultValueIfValueNullOrEmpty(
                            demandInfos.transportationPreference),
                        yachtType: getDefaultValueIfValueNullOrEmpty(demandInfos.yachtType),
                        yachtName: getDefaultValueIfValueNullOrEmpty(demandInfos.yachtName),
                        nameSurname: getDefaultValueIfValueNullOrEmpty(demandInfos.nameSurname),
                        passedTime: await getPassedTimeInStringAsync(null, demandInfos.createdDate)
                    };
                    //#endregion

                    //#region add article infos 
                    let div_article_info = article.children("#" + div_article_info_id);
                    let div_passedTime_id = "div_passedTime";

                    div_article_info.append(`
                        <div>
                            <p class="p_article">${articleInfos.restaurantName}</p>
                            <p class="p_article">${articleInfos.transportationPreference}</p>
                            <p class="p_article">${articleInfos.yachtType}</p>
                            <p class="p_article">${articleInfos.yachtName}</p>
                            <p class="p_article">${articleInfos.nameSurname}</p>
                        </div>
                        <div id="${div_passedTime_id}">${articleInfos.passedTime}</div>
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