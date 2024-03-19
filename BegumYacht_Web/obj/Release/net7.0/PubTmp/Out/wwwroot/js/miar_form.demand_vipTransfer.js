import { populateInfoMessagesAsync } from "./miar_module.userForm.js"
import { convertDateToStrDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";
import { addCriticalSectionAsync, shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js"

import {
    addImageToArticleAsync, beforePopulateAsync, click_articleAsync, resize_windowAsync,
    click_backButtonAsync, click_InfoDivAsync, getDefaultValueIfValueNullOrEmpty,
    populateArticlesAsync, addInputsToInfoDivsAsync
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
    const formType = "VipTransferDemand";
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
        ["input", "text", "vehicleType", "Araç Tipi", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "transferDate", "Transfer Tarihi", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "from", "Nereden", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "to", "Nereye", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "numberOfPeople", "Yolcu Sayısı", false, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "luggage", "Ek Bagaj Arabası", true, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "accountOps", "Ücretin Ödeneceği Hesap", true, "readonly", [div.demandInfos_inputs]],
        ["input", "text", "createdDate", "Talep Tarihi", false, "readonly", [div.demandInfos_inputs]],
        ["textarea", "notes", "Notlar", false, "readonly", [div.demandInfos_inputs]],
    ];
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
        vehicleType: "inpt_vehicleType",
        transferDate: "inpt_transferDate",
        from: "inpt_from",
        to: "inpt_to",
        numberOfPeople: "inpt_numberOfPeople",
        luggage: "inpt_luggage",
        accountOps: "inpt_accountOps",
        createdDate: "inpt_createdDate",
        notes: "txt_notes"
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
                let transferDateInStr = getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.transferDate);
                let createdDateInStr = getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.createdDate);

                let formInfos = {
                    yachtName: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtName),
                    yachtType: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtType),
                    flag: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.flag),
                    vehicleType: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.vehicleType),
                    transferDate: (transferDateInStr == infosOfLastClickedArticle.transferDate ?
                        await convertDateToStrDateAsync(
                            new Date(transferDateInStr),
                            { hours: false, minutes: false, seconds: false }) // when date is not null or empty
                        : transferDateInStr),  // when date is null or empty
                    from: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.from),
                    to: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.to),
                    numberOfPeople: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.numberOfPeople),
                    luggage: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.luggage),
                    accountOps: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.accountOps),
                    createdDate: (createdDateInStr == infosOfLastClickedArticle.createdDate ?
                        await convertDateToStrDateAsync(
                            new Date(createdDateInStr),
                            { hours: true, minutes: true, seconds: false }) // when date is not null or empty
                        : createdDateInStr),  // when date is null or empty,
                    notes: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.notes)
                };
                //#endregion

                //#region populate inputs
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
        await beforePopulateAsync(300, 650, div.articles);
        await populateDemandArticlesAsync();
        await addInputsToInfoDivsAsync(inputInfos);
        await populateInfoMessagesAsync({
            div_senderInfos: ["Şeklin üzerine tıklayarak talebi gönderen personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_answererInfos: ["Şeklin üzerine tıklayarak talebe cevap veren personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_demandInfos: ["Şeklin üzerine tıklayarak talep bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_luggage: ["Ek bagaj arabasının istenip istenmediğidir."],
            div_accountOps: ["Marina ücretinin yatın hesabına mı yoksa müşterinin hesabına mı ekleneceğidir.",]
        });
    }
    async function populateDemandArticlesAsync() {
        await populateArticlesAsync(
            "/adminPanel/demand/vipTransfer/filter?" + (
                `pageSize=${pagingBuffer.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}` +
                `&formStatus=${formStatus}`),
            headerKeys.demand.vipTransfer,
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
                        65 / 100,
                        55 / 100);

                    //#region set article Infos 
                    let notes = getDefaultValueIfValueNullOrEmpty(demandInfos.notes);

                    let articleInfos = {
                        vehicleType: getDefaultValueIfValueNullOrEmpty(demandInfos.vehicleType),
                        from: getDefaultValueIfValueNullOrEmpty(demandInfos.from),
                        to: getDefaultValueIfValueNullOrEmpty(demandInfos.to),
                        yachtType: getDefaultValueIfValueNullOrEmpty(demandInfos.yachtType),
                        yachtName: getDefaultValueIfValueNullOrEmpty(demandInfos.yachtName),
                        nameSurname: getDefaultValueIfValueNullOrEmpty(demandInfos.nameSurname),
                        notes: (notes == demandInfos.notes ?
                            notes.substring(0, 200) // when notes is not null or empty
                            : notes),  // when notes is null or empty                            
                        passedTime: await getPassedTimeInStringAsync(null, demandInfos.createdDate)
                    };
                    //#endregion

                    //#region add article infos 
                    let div_article_info = article.children("#" + div_article_info_id);
                    let div_passedTime_id = "div_passedTime";

                    div_article_info.append(`
                        <div>
                            <p class="p_article">${articleInfos.vehicleType}</p>
                            <p class="p_article">${articleInfos.from} -> ${articleInfos.to}</p>
                            <p class="p_article">${articleInfos.yachtType}</p>
                            <p class="p_article">${articleInfos.yachtName}</p>
                            <p class="p_article">${articleInfos.nameSurname}</p>
                            <p class="p_article">${articleInfos.notes}</p>
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