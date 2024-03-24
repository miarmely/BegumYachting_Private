import { convertDateToStrDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";
import { shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js"

import {
    addImageToArticleAsync, click_articleAsync, resize_windowAsync,
    click_backButtonAsync, click_InfoDivAsync, getDefaultValueIfValueNullOrEmpty,
    populateArticlesAsync, addInputsToInfoDivsAsync, click_sidebarMenuAsync,
    formStatus, acceptTheFormAsync, rejectTheFormAsync, infosOfLastClickedArticle, setPageSizeAsync,
    change_submenuOfDisplayOptionAsync
} from "./miar_form.js"

import {
    art_baseId, div_article_info_id, getValidArticleWidthAsync, setArticleBufferAsync,
} from "./miar_module.article.js"

import {
    change_inpt_paginationCurrentAsync, click_ul_paginationAsync,
    keyup_ul_paginationAsync, inpt_paginationCurrent_id, pagingBuffer
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
        ["input", "text", "restaurantName", "Restaurant Adı", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "conciergeDate", "Konsiyerj Hizmeti Tarihi", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "numberOfPeople", "Yolcu Sayısı", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "transportationPreference", "Ulaşım Tercihi", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "transferCarType", "Transfer Aracı Tipi", false, "readonly", [div.formInfos_inputs]],
        ["input", "text", "createdDate", "Talep Tarihi", false, "readonly", [div.formInfos_inputs]],
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
        await change_submenuOfDisplayOptionAsync(
            slct.article_submenu_display,
            div.answererInfos,
            div.buttons,
            populateDemandArticlesAsync);
    })
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
            div.buttons,
            btn.back,
            populateDemandArticlesAsync);
    })
    btn.accept.click(async () => {
        await acceptTheFormAsync(
            "/adminPanel/demand/conciergeService/answer",
            infosOfLastClickedArticle.formId,
            inputIds,
            p_resultLabel,
            img_loading,
            div.answererInfos,
            div.answererInfos_inputs,
            div.buttons);
    })
    btn.reject.click(async () => {
        await rejectTheFormAsync(
            "/adminPanel/demand/conciergeService/answer",
            infosOfLastClickedArticle.formId,
            inputIds,
            p_resultLabel,
            img_loading,
            div.answererInfos,
            div.answererInfos_inputs,
            div.buttons);
    })
    //#endregion

    //#endregion

    //#region functions
    async function setupPageAsync() {
        div.panelTitle.append("KONSİYERJ HİZMETİ TALEBİ");
        spn.formInfos_formType.append("Talep");

        await populateDemandArticlesAsync();
        await addInputsToInfoDivsAsync(inputInfos);
    }
    async function populateDemandArticlesAsync() {
        await setArticleBufferAsync({
            div_articles: div.articles,
            articleType: "imageAndText",
            articleStyle: {
                "width": await getValidArticleWidthAsync({
                    width: 300,
                    marginL: 20,
                    marginR: 20
                }, div.articles),
                "height": 420,
                "marginT": 10,
                "marginB": 10,
                "marginR": 20,
                "marginL": 20,
                "paddingT": 10,
                "paddingB": 10,
                "paddingR": 10,
                "paddingL": 10,
                "border": 1,
                "borderColor": "#0095ff",
                "boxShadow": "5px 5px 10px rgba(0, 0, 0, 0.3)",
                "bgColorForDelete": "red"
            },
            heightOfPageMenubar: 80
        });  // i have to define article buffer before setting the page size.
        await setPageSizeAsync();
        await populateArticlesAsync(
            "/adminPanel/demand/conciergeService/filter?" + (
                `pageSize=${pagingBuffer.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}` +
                `&formStatus=${formStatus}`),
            headerKeys.demand.conciergeService,
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