import { getPassedTimeInStringAsync } from "./miar_module.date.js";
import { addCriticalSectionAsync, addPaginationButtonsAsync, shiftTheChildDivToBottomOfParentDivAsync } from "./miar_module.js";

import {
    addImageToArticleAsync, getPageSizeAsync, updateEntityQuantityAsync
} from "./miar_demand.js";

import {
    addArticlesAsync, alignArticlesAsAutoAsync, alignArticlesToCenterAsync, art_baseId, controlArticleWidthAsync, div_article_info_id, getValidArticleWidthAsync, isSidebarOpenAsync, setArticleBufferAsync, setHeightOfArticlesDivAsync
} from "./miar_module.article.js";




$(function () {
    //#region variables
    const ul_pagination = $("#ul_pagination");
    const div = {
        article_update: $("#div_article_update"),
        article_display: $("#div_article_display"),
        articles: $("#div_articles"),
        sidebarMenuButton: $("#div_sidebarMenuButton")
    }
    const pagination = {
        pageSize: 0,  // it will be initialize
        pageNumber: 1,
        buttonQuantity: 5,
        infosInHeader: {}  // it will be initialize
    };
    const headerKeys = {
        fuelPurchase: "Demand-FuelPurchase"
    };
    const lbl = {
        entityQuantity: $("#small_entityQuantity")
    }
    const criticalSectionIds = {
        sidebarMenuButton: "sidebarMenuButton",
        window: "window"
    }
    //#endregion

    //#region events
    $(window).resize(async () => {
        await addCriticalSectionAsync(
            criticalSectionIds.window,
            async () => {
                await controlArticleWidthAsync();
                await alignArticlesToCenterAsync("px");
                await setHeightOfArticlesDivAsync();
            },
            500);
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
    //#endregion

    //#region functions
    async function populateArticlesAsync() {
        //#region set page size
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
        });
        pagination.pageSize = await getPageSizeAsync();
        //#endregion

        $.ajax({
            method: "GET",
            url: (baseApiUrl + "/adminPanel/fuelPurchaseDemand/all?" +
                `pageSize=${pagination.pageSize}` +
                `&pageNumber=${pagination.pageNumber}`),
            dataType: "json",
            success: (demands, status, xhr) => {
                new Promise(async () => {
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
                        //#endregion

                        await addImageToArticleAsync(articleId, demandInfos.yachtType);

                        //#region //////////////// TEMPORARY ////////////////
                        let div_article_info = article.children("#" + div_article_info_id);
                        let notes = "work is underway to remove it from this browser. Usage of this event listener will cause performance issues today, and represents a risk of future incompatibility. Consider using MutationObserver instead. and work is underway to remove it from this browser. Usage of this event listener will cause performance issues today, and represents a risk of future incompatibility. Consider using MutationObserver instead. and work is underway to remove it from this browser. Usage of this event listener will cause performance issues today, and represents a risk of future incompatibility. Consider using MutationObserver instead.";

                        div_article_info
                            //.append(`
                            //    <h3>${demandInfos.flag}</h3>
                            //    <h4 style="padding-top:2px">${demandInfos.nameSurname}</h3>
                            //    <h5 style="padding-top:3px">${demandInfos.yachtType}</h5>
                            //    <h5 style="padding-top:2px">${demandInfos.yachtName}</h5>
                            //    <h5 style="padding-top:3px">${demandInfos.notes.substring(0, 50)}...</h5>
                            //`);
                            .append(`
                                <div>
                                    <h2>Türkiye</h2>
                                    <h3 style="margin-top:3px">MotorYacht</h5>
                                    <h4 style="margin-top:2px">Barbaros</h5>
                                    <h4 style="margin-top:20px; font-size:17px">Mert Akdemir</h3>
                                    <h6 style="margin-top:10px">${notes.substring(0, 200)}...</h5>
                                </div>
                                <div id="div_passedTime">${await getPassedTimeInStringAsync(demandInfos.createdDate)}</div>
                            `);
                        //#endregion

                        await shiftTheChildDivToBottomOfParentDivAsync(
                            div_article_info,
                            "div_passedTime");  // shift the passed time to bottom
                    }
                    //#endregion

                    //#region get pagination infos
                    pagination.infosInHeader = JSON.parse(xhr
                        .getResponseHeader(headerKeys.fuelPurchase));
                    //#endregion

                    await updateEntityQuantityAsync(
                        lbl.entityQuantity,
                        pagination.infosInHeader.CurrentPageCount + "/" + pagination.pageSize
                    );
                    await addPaginationButtonsAsync(
                        pagination.infosInHeader,
                        pagination.buttonQuantity,
                        ul_pagination);
                })
            }
        })
    }
    //#endregion

    populateArticlesAsync();
})