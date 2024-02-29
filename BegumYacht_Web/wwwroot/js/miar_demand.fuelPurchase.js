import { base_addArticlesAsync } from "./miar_demand.js"
import { addArticlesAsync, setArticleBufferAsync } from "./miar_module.article.js";

$(function () {
    //#region variables
    const div = {
        articles: $("#div_articles"),
    }
    const articleSettings = {
        "div_articles": div.articles,  // should be set
        "totalArticleCount": 30,  // should be set
        "articleCountOnOneRow": 0,
        "articleType": "imageAndText",  // should be set 
        "articleStyle": {  // should be set
            "width": 300,
            "height": 450,
            "marginT": 10,
            "marginB": 10,
            "marginR": 20,
            "marginL": 20,
            "paddingT": 5,
            "paddingB": 5,
            "paddingR": 5,
            "paddingL": 5,
            "border": 1,
            "borderColor": "blue",
            "boxShadow": "5px 5px 10px rgba(0, 0, 0, 0.3)",
            "bgColorForDelete": "red"
        }  // firstly articleType should be set
    }
    //#endregion

    //#region functions
    async function setupPageAsync() {
        await setArticleBufferAsync(articleSettings);
        await addArticlesAsync(true);
    }
    //#endregion

    setupPageAsync();
})