import { populateInfoMessagesAsync } from "./miar_module.userForm.js"
import { convertDateToStrDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";
import { shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js"

import {
    addImageToArticleAsync, beforePopulateAsync, click_articleAsync, resize_windowAsync,
    click_backButtonAsync, click_InfoDivAsync, getDefaultValueIfValueNullOrEmpty,
    populateArticlesAsync, addInputsToInfoDivsAsync, click_sidebarMenuAsync, formStatus, showOrHideAnswererInfosMenuByFormStatusAsync
} from "./miar_form.js"

import {
    alignArticlesToCenterAsync, art_baseId, div_article_info_id,
} from "./miar_module.article.js"

import {
    change_inpt_paginationCurrentAsync, click_ul_paginationAsync, keyup_ul_paginationAsync,
    inpt_paginationCurrent_id, pagingBuffer
} from "./miar_module.pagination.js"


$(function () {
    //#region variables
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
        ["input", "text", "yachtType", "Yat Tipi", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "yachtName", "Yat Adı", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "flag", "Bayrak", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "requestedService", "İstenen Servis", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "createdDate", "Talep Tarihi", false, "readonly", [div.formInfos_inputs]],
        ["textarea", "notes", "Notlar", false, "readonly", [div.formInfos_inputs]],
    ];  // for add <input>s and <textarea>s
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
        requestedService: "inpt_requestedService",
        createdDate: "inpt_createdDate",
        notes: "txt_notes"
    };  // for populate <input>s and <textarea>s
    let articleIdsAndInfos = {};
    //#endregion

    //#region events

    //#region partner
    $(window).resize(async () => {
        await resize_windowAsync(
            div.article_display,
            criticalSectionIds.window);
    })
    div.sidebarMenuButton.click(async () => {
        await click_sidebarMenuAsync(div.article_display);
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
        await showOrHideAnswererInfosMenuByFormStatusAsync(
            slct.article_submenu_display,
            div.answererInfos);
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
            div.answererInfos,
            div.buttons,
            btn.back,
            async (infosOfLastClickedArticle) => {
                //#region set form infos
                let createdDateInStr = getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.createdDate);

                let formInfos = {
                    yachtName: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtName),
                    yachtType: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.yachtType),
                    flag: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.flag),
                    requestedService: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.requestedService),
                    createdDate: (createdDateInStr == infosOfLastClickedArticle.createdDate ?
                        await convertDateToStrDateAsync(
                            new Date(createdDateInStr),
                            { hours: true, minutes: true, seconds: false }) // when date is not null or empty
                        : createdDateInStr),  // when date is null or empty,
                    notes: getDefaultValueIfValueNullOrEmpty(infosOfLastClickedArticle.notes)
                };
                //#endregion

                //#region populate inputs (DYNAMICALLY)
                for (let elementName in formInfos)
                    div.formInfos_inputs
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
            div.formInfos,
            div.senderInfos_inputs,
            div.answererInfos_inputs,
            div.formInfos_inputs,
            btn.back);
        await alignArticlesToCenterAsync();
    })
    //#endregion

    //#endregion

    //#region functions
    async function setupPageAsync() {
        div.panelTitle.append("GÜVENLİK VE KORUMA HİZMETİ TALEBİ");
        spn.formInfos_formType.append("Talep");

        await beforePopulateAsync(300, 580, div.articles);
        await populateDemandArticlesAsync();
        await addInputsToInfoDivsAsync(inputInfos);
    }
    async function populateDemandArticlesAsync() {
        await populateArticlesAsync(
            "/adminPanel/demand/securityAndProtectionService/filter?" + (
                `pageSize=${pagingBuffer.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}` +
                `&formStatus=${formStatus}`),
            headerKeys.demand.securityAndProtectionService,
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
                        60 / 100);

                    //#region set article Infos 
                    let notes = getDefaultValueIfValueNullOrEmpty(demandInfos.notes);

                    let articleInfos = {
                        requestedService: getDefaultValueIfValueNullOrEmpty(demandInfos.requestedService),
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
                            <p class="p_article">${articleInfos.requestedService}</p>
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