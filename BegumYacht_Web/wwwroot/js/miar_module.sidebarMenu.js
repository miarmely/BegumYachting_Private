export async function openOrCloseTheSubmenuAsync(clickedElement) {
    //#region find submenu of clicked <li>
    let li_clicked_id = clickedElement
        .closest("li")
        .attr("id");
    //#endregion

    //#region auto open/close the submenu
    let li_clicked_submenu = $("#" + li_clicked_id + "_submenu");

    // open
    if (li_clicked_submenu.attr("hidden") != null)
        li_clicked_submenu.removeAttr("hidden");

    // close
    else
        li_clicked_submenu.attr("hidden", "");    
    //#endregion
}