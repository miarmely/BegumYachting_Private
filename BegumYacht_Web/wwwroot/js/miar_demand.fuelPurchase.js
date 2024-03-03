import { convertStrUtcDateToStrLocalDateAsync, getPassedTimeInStringAsync } from "./miar_module.date.js";

import {
    addCriticalSectionAsync, shiftTheChildDivToBottomOfParentDivAsync
} from "./miar_module.js"

import {
    addImageToArticleAsync, click_articleAsync, click_backButtonAsync, click_senderInfosDivAsync, getPageSizeAsync, resetDivArticlesAsync, resize_windowAsync, updateEntityQuantityAsync
} from "./miar_demand.js"

import {
    addArticlesAsync, addMsgWithImgToDivArticlesAsync, alignArticlesToCenterAsync, art_baseId, controlArticleWidthAsync,
    div_article_info_id, getValidArticleWidthAsync, setArticleBufferAsync,
    setHeightOfArticlesDivAsync
} from "./miar_module.article.js"

import {
    addValueToPaginationLastButtonAsync, change_inpt_paginationCurrentAsync,
    click_ul_paginationAsync, controlPaginationBackAndNextButtonsAsync,
    inpt_paginationCurrent_id, keyup_ul_paginationAsync, pagingBuffer
} from "./miar_module.pagination.js"

import { populateInfoMessagesAsync, showOrHideBackButtonAsync } from "./miar_module.userForm.js"


$(function () {
    //#region variables
    const ul_pagination = $("#ul_pagination");
    const p_resultLabel = $("#p_resultLabel");
    const pagination = {
        pageSize: 0,  // it will be initialize
        buttonQuantity: 5,
        infosInHeader: {}  // it will be initialize
    };
    const headerKeys = {
        fuelPurchase: "Demand-FuelPurchase"
    };
    const criticalSectionIds = {
        sidebarMenuButton: "sidebarMenuButton",
        window: "window",
        backButton: "backButton"
    }
    const path = {
        "laodingImage": "images/loading.gif"
    };
    const div = {
        article_update: $("#div_article_update"),
        article_display: $("#div_article_display"),
        articles: $("#div_articles"),
        sidebarMenuButton: $("#div_sidebarMenuButton"),
        senderInfos: $("#div_senderInfos"),
        backButton: $("#div_backButton"),
        panelTitle: $("#div_panelTitle")
    };
    const btn = {
        back: $("#btn_back")
    };
    const lbl = {
        entityQuantity: $("#small_entityQuantity")
    };
    const inpt = {
        "yachtName": $("#inpt_yachtName"),
        "yachtType": $("#inpt_yachtType"),
        "flag": $("#inpt_flag"),
        "isDutyPaid": $("#inpt_isDutyPaid"),
        "mgo": $("#inpt_mgo"),
        "ago": $("#inpt_ago"),
        "fuelType": $("#inpt_fuelType"),
        "requestedFuel": $("#inpt_requestedFuel"),
        "fuelSupplyPort": $("#inpt_fuelSupplyPort"),
        "fuelSupplyDate": $("#inpt_fuelSupplyDate"),
        "creationDate": $("#inpt_creationDate"),
    };
    const txt = {
        "notes": $("#txt_notes"),
    }
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
            pagination.infosInHeader,
            populateArticlesAsync);
    })
    ul_pagination.keyup(async (event) => {
        await keyup_ul_paginationAsync(
            event,
            pagination.infosInHeader,
            populateArticlesAsync);
    })
    spn_eventManager.on("click_article", async (_, event) => {
        await click_articleAsync(
            event,
            articleIdsAndInfos,
            div.article_display,
            div.article_update,
            div.backButton,
            div.panelTitle,
            btn.back,
            async (infosOfLastClickedArticle) => {
                //#region populate demand inputs
                inpt.yachtName.val(infosOfLastClickedArticle.yachtName);
                inpt.yachtType.val(infosOfLastClickedArticle.yachtType);
                inpt.flag.val(infosOfLastClickedArticle.flag);
                inpt.isDutyPaid.val(infosOfLastClickedArticle.isDutyPaid);
                inpt.mgo.val(infosOfLastClickedArticle.mgo);
                inpt.ago.val(infosOfLastClickedArticle.ago);
                inpt.fuelType.val(infosOfLastClickedArticle.fuelType);
                inpt.requestedFuel.val(infosOfLastClickedArticle.requestedFuel);
                inpt.fuelSupplyPort.val(infosOfLastClickedArticle.fuelSupplyPort);
                inpt.fuelSupplyDate.val(
                    await convertStrUtcDateToStrLocalDateAsync(
                        infosOfLastClickedArticle.fuelSupplyDate));
                inpt.creationDate.val(
                    await convertStrUtcDateToStrLocalDateAsync(
                        infosOfLastClickedArticle.createdDate));
                txt.notes.val(infosOfLastClickedArticle.notes);
                //#endregion
            }
        );
    })
    //#endregion

    //#region update page
    div.senderInfos.click(async () => {
        await click_senderInfosDivAsync();
    })
    btn.back.click(async () => {
        await click_backButtonAsync(
            p_resultLabel,
            div.backButton,
            div.panelTitle,
            div.article_update,
            div.article_display,
            btn.back);
        await alignArticlesToCenterAsync();
    })
    //#endregion

    //#endregion

    //#region functions
    async function setupPageAsync() {
        await populateArticlesAsync();
        await populateInfoMessagesAsync({
            div_senderInfos_base: ["Şeklin üzerine tıklayarak talebi gönderen personelin bilgilerini görüntüleyebilir veya gizleyebilirsin.",],
            div_notes: ["şekli, sağ alt ucundan aşağı doğru çekerek uzatabilirsin.",]
        });
    }
    async function populateArticlesAsync() {
        //#region set articleBuffer and page size
        await setArticleBufferAsync({
            div_articles: div.articles,
            articleType: "imageAndText",
            articleStyle: {
                "width": await getValidArticleWidthAsync({
                    width: 300,
                    marginL: 20,
                    marginR: 20
                }, div.articles),
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

        pagination.pageSize = await getPageSizeAsync();
        //#endregion

        $.ajax({
            method: "GET",
            url: (baseApiUrl + "/adminPanel/fuelPurchaseDemand/all?" +
                `pageSize=${pagination.pageSize}` +
                `&pageNumber=${pagingBuffer.pageNumber}`),
            dataType: "json",
            beforeSend: () => {
                addMsgWithImgToDivArticlesAsync(
                    path.laodingImage,
                    "Yükleniyor",
                    "Yükleniyor...");
            },
            success: (demands, status, xhr) => {
                new Promise(async () => {
                    await resetDivArticlesAsync(div.articles); // remove loading img
                    await setArticleBufferAsync({
                        "totalArticleCount": demands.length
                    });
                    await addArticlesAsync(true);

                    //#region populate articles
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
                                <h2>${demandInfos.flag}</h2>
                                <h3 style="margin-top:3px">${demandInfos.yachtType}</h3>
                                <h4 style="margin-top:2px">${demandInfos.yachtName}</h4>
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

                    //#region declare events
                    $(".article").click(event => {
                        spn_eventManager.trigger("click_article", [event]);
                    })
                    //#endregion

                    //#region get pagination infos
                    pagination.infosInHeader = JSON.parse(xhr
                        .getResponseHeader(headerKeys.fuelPurchase));
                    //#endregion

                    await updateEntityQuantityAsync(
                        lbl.entityQuantity,
                        pagination.infosInHeader.CurrentPageCount + "/" + pagination.pageSize
                    );
                    await addValueToPaginationLastButtonAsync(
                        pagination.infosInHeader.TotalPage);
                    await controlPaginationBackAndNextButtonsAsync(pagination.infosInHeader);
                })
            },
            error: () => {
                addMsgWithImgToDivArticlesAsync(
                    "images/question.png",
                    "Talep Bulunamadı",
                    "Herhangi Bir Talep Bulunamadı"
                );
            }
        })
    }
    //#endregion

    setupPageAsync();
})