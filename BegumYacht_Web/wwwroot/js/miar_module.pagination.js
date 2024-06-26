﻿import { autoObjectMapperAsync, updateElementText } from "./miar_module.js";


//#region variables
export const a_paginationBack_id = "a_paginationBack";
export const a_paginationCurrent_id = "a_paginationCurrent";
export const a_separator_id = "a_separator";
export const a_paginationLast_id = "a_paginationLast";
export const a_paginationNext_id = "a_paginationNext";
export const inpt_paginationCurrent_id = a_paginationCurrent_id + " input";
export let pagingBuffer = {
    pageSize: 0,
    pageNumber: 1,
    infosInHeader: null // object
};
//#endregion

//#region events
export async function click_ul_paginationAsync(event, func_populateArticleAsync) {
    //#region remove "red" bg-color of paginationCurrent
    event.preventDefault();

    let inpt_paginationCurrent = $("#" + inpt_paginationCurrent_id);
    inpt_paginationCurrent.removeAttr("style");
    //#endregion

    //#region when invalid places is clicked (RETURN)
    var ClickedElementLocalName = event.target.localName;
    let clickedElementId = event.target.id;

    if (ClickedElementLocalName == "input"  // info message button and paginationCurrent input
        || (clickedElementId != a_paginationBack_id
            && clickedElementId != a_separator_id
            && clickedElementId != a_paginationLast_id
            && clickedElementId != a_paginationNext_id))
        return;
    //#endregion

    //#region set new page number
    switch (clickedElementId) {
        case a_paginationBack_id:
            pagingBuffer.pageNumber--;

            break;
        case a_separator_id:
            if (!isPaginationCurrentValueValid()) {
                // add "red" bg-color
                inpt_paginationCurrent.css("background-color", "red");
                return;
            }

            pagingBuffer.pageNumber = inpt_paginationCurrent.val();
            break;
        case a_paginationLast_id:
            pagingBuffer.pageNumber = (pagingBuffer.infosInHeader != null ?
                pagingBuffer.infosInHeader.TotalPage
                : 1);

            break;
        case a_paginationNext_id:
            pagingBuffer.pageNumber++;

            break;
        default:
            //#region when icon of btn_paginationBack is clicked
            let parentId = event.target.parentNode.id;

            if (parentId == a_paginationBack_id) {
                pagingBuffer.pageNumber--;
                break;
            }
            //#endregion

            //#region when icon of btn_paginationNext is clicked
            else if (parentId == a_paginationNext_id) {
                pagingBuffer.pageNumber++;
                break;
            }
            //#endregion

            return;
    }
    //#endregion

    await func_populateArticleAsync();
}
export async function keyup_ul_paginationAsync(event, func_populateArticleAsync) {
    switch (event.key) {
        case "Enter":  // when entered key is "Enter"
            //#region remove "red" bg-color of input
            let inpt_paginationCurrent = $("#" + inpt_paginationCurrent_id);
            inpt_paginationCurrent.removeAttr("style");
            //#endregion

            if (!isPaginationCurrentValueValid()) {
                // add "red" bg-color
                inpt_paginationCurrent.css("background-color", "red");
                return;
            }

            //#region populate articles
            pagingBuffer.pageNumber = inpt_paginationCurrent.val();
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
export async function setPagingBufferAsync(newBuffer = {
    pageSize: 0,
    pageNumber: 0,
    infosInHeader: {}
}) {
    await autoObjectMapperAsync(pagingBuffer, newBuffer, false);
}
export async function controlPaginationButtonsAsync() {
    //#region set variables
    let a_paginationBack = $("#" + a_paginationBack_id);
    let inpt_paginationCurrent = $("#" + a_paginationCurrent_id + " input");
    let a_paginationNext = $("#" + a_paginationNext_id);
    let a_paginationLast = $("#" + a_paginationLast_id);
    let pagingInfosInHeader = pagingBuffer.infosInHeader;

    // reset "red" bg color
    inpt_paginationCurrent.removeAttr("style");
    //#endregion

    //#region when any entity is not exists
    if (pagingInfosInHeader == null) {
        // hide pagination back and next buttons
        a_paginationBack.attr("hidden", "");
        a_paginationNext.attr("hidden", "");

        // reset values of pagination current and last
        inpt_paginationCurrent.val("1");
        updateElementText(a_paginationLast, "1");

        return;
    }
    //#endregion

    //#region hide/show paginationBack button
    // hide
    if (pagingInfosInHeader.CurrentPageNo == 1)
        a_paginationBack.attr("hidden", "");

    // show
    else
        a_paginationBack.removeAttr("hidden");
    //#endregion

    //#region hide/show paginationNext button
    // hide
    if (pagingInfosInHeader.CurrentPageNo == pagingInfosInHeader.TotalPage)
        a_paginationNext.attr("hidden", "");

    // show
    else
        a_paginationNext.removeAttr("hidden");
    //#endregion

    //#region add values to pagination current and last
    inpt_paginationCurrent.val(pagingBuffer.infosInHeader.CurrentPageNo);
    updateElementText(
        a_paginationLast,
        pagingInfosInHeader.TotalPage);  // paginationLast button
    //#endregion
}
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
}  // deprecated
function isPaginationCurrentValueValid() {
    //#region set variables
    let inpt_paginationCurrent = $("#" + inpt_paginationCurrent_id);
    let inpt_paginationCurrent_value = inpt_paginationCurrent.val();
    let totalPage = (pagingBuffer.infosInHeader != null ?
        pagingBuffer.infosInHeader.TotalPage
        : 1);
    //#endregion

    //#region when input value is invalid
    if (inpt_paginationCurrent_value == ""
        || inpt_paginationCurrent_value < 1
        || inpt_paginationCurrent_value > totalPage)
        return false;
    //#endregion

    return true;
}
//#endregion