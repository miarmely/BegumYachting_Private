export async function openOrCloseTheSubmenuAsync(li_clicked) {
    //#region auto open/close the submenu
    let li_clicked_submenu = $("#" + li_clicked.attr("id") + "_submenu");

    // open
    if (li_clicked_submenu.attr("hidden") != null)
        li_clicked_submenu.removeAttr("hidden");

    // close
    else
        li_clicked_submenu.attr("hidden", "");    
    //#endregion
}