import {
    addArticlesAsync, setArticleBufferAsync
} from "./miar_module.article.js"



//#region functions
export async function base_addArticlesAsync(articleBuffer) {
    await setArticleBufferAsync(articleBuffer);
    await addArticlesAsync(true);
}
//#endregion