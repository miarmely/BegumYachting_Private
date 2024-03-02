//#region variables
export const a_paginationBack_id = "a_paginationBack";
export const a_paginationCurrent_id = "a_paginationCurrent";
export const a_separator_id = "a_separator";
export const a_paginationLast_id = "a_paginationLast";
export const a_paginationNext_id = "a_paginationNext";
export const inpt_paginationCurrent_id = a_paginationCurrent_id + " input";
export let pagingBuffer = {
    pageNumber: 1,
};
//#endregion

//#region events
export async function click_ul_paginationAsync(
    event,
    paginationInfos,
    func_populateTableAsync
) {
    //#region reset "red" bg-color of "paginationCurrent" input
    event.preventDefault();

    let inpt_paginationCurrent = $("#" + inpt_paginationCurrent_id);
    inpt_paginationCurrent.removeAttr("style");
    //#endregion

    //#region set new page number
    let clickedElementId = event.target.id;

    switch (clickedElementId) {
        case a_paginationBack_id:
            // update input
            pagingBuffer.pageNumber--;
            inpt_paginationCurrent.val(pagingBuffer.pageNumber);

            break;
        case a_separator_id:
            //#region when page number updating is unsuccessful
            if (!await updatePageNumberWhenSeparatorClickedAsync(paginationInfos))
                return;
            //#endregion

            break;
        case a_paginationLast_id:
            // update input
            pagingBuffer.pageNumber = paginationInfos.TotalPage;
            inpt_paginationCurrent.val(pagingBuffer.pageNumber);

            break;
        case a_paginationNext_id:
            // update input
            pagingBuffer.pageNumber++;
            inpt_paginationCurrent.val(pagingBuffer.pageNumber);

            break;
        default:
            //#region when icon of btn_paginationBack is clicked
            let parentId = event.target.parentNode.id;

            if (parentId == a_paginationBack_id) {
                pagingBuffer.pageNumber--;
                inpt_paginationCurrent.val(pagingBuffer.pageNumber);
                break;
            }
            //#endregion

            //#region when icon of btn_paginationNext is clicked
            else if (parentId == a_paginationNext_id) {
                pagingBuffer.pageNumber++;
                inpt_paginationCurrent.val(pagingBuffer.pageNumber);
                break;
            }
            //#endregion

            return;
    }
    //#endregion

    await func_populateTableAsync();
}
export async function keyup_ul_paginationAsync(
    event,
    paginationInfos,
    func_populateArticleAsync) {
    switch (event.key) {
        case "Enter":  // when entered key is "Enter"
            //#region when page number updating is successful
            if(await updatePageNumberWhenSeparatorClickedAsync(paginationInfos))
                await func_populateArticleAsync();
            //#endregion

            break;
    }
}
export async function change_inpt_paginationCurrentAsync() {
    // reset "red" border-color
    $("#" + inpt_paginationCurrent_id).removeAttr("style");
}
//#endregion

//#region functions
export async function addPaginationButtonsAsync(
    paginationInfosInJson,
    paginationButtonQuantity,
    ul_pagination
) {
    //#region set buttonQauntity for pagination
    let buttonQuantity = paginationInfosInJson.TotalPage < paginationButtonQuantity ?
        paginationInfosInJson.TotalPage
        : paginationButtonQuantity
    //#endregion

    //#region add back button as hidden
    ul_pagination.empty()
    ul_pagination.append(
        `<li>
		    <a id="a_paginationBack" href="#" hidden>
			    <i class="fa fa-chevron-left"></i>
		    </a>
	    </li>`);
    //#endregion

    //#region add pagination buttons
    for (let pageNo = 1; pageNo <= buttonQuantity; pageNo += 1)
        ul_pagination.append(
            `<li>
			    <a href="#">${pageNo}</a>
		    </li> `);
    //#endregion

    //#region add next button as hidden
    ul_pagination.append(
        `<li>
		    <a id="a_paginationNext" href="#" hidden>
			    <i class="fa fa-chevron-right"></i>
		    </a>
	    </li>`);
    //#endregion
}
export async function controlPaginationBackAndNextButtonsAsync(paginationInfosInJson) {
    // when total page count more than 1
    if (paginationInfosInJson.TotalPage > 1) {
        //#region for paginationBack button
        // hide
        if (paginationInfosInJson.CurrentPageNo == 1)
            $("#" + a_paginationBack_id).attr("hidden", "");

        // show
        else
            $("#" + a_paginationBack_id).removeAttr("hidden");
        //#endregion

        //#region for paginationNext button
        // hide
        if (paginationInfosInJson.CurrentPageNo == paginationInfosInJson.TotalPage)
            $("#" + a_paginationNext_id).attr("hidden", "");

        // show
        else
            $("#" + a_paginationNext_id).removeAttr("hidden");
        //#endregion
    }
}
export async function addValueToPaginationLastButtonAsync(value) {
    $("#" + a_paginationLast_id).empty();
    $("#" + a_paginationLast_id).append(value);
}
async function updatePageNumberWhenSeparatorClickedAsync(paginationInfos) {
    //#region set page number

    //#region when entered page number bigger than total page count
    let inpt_paginationCurrent = $("#" + inpt_paginationCurrent_id);
    let inpt_paginationCurrent_value = inpt_paginationCurrent.val();

    if (inpt_paginationCurrent_value == ""
        || inpt_paginationCurrent_value < 1
        || inpt_paginationCurrent_value > paginationInfos.TotalPage
    ) {
        inpt_paginationCurrent.css("background-color", "red");
        return false;
    }
    //#endregion

    //#region update page number
    pagingBuffer.pageNumber = inpt_paginationCurrent_value;
    inpt_paginationCurrent.removeAttr("style");  // reset red border-color
    //#endregion

    //#endregion

    return true; // when new page number is valid
}
//#endregion