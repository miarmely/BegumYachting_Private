import { openOrCloseTheSubmenuAsync } from "./miar_module_sidebarMenu.js";

$(function () {
    //#region variables
    //#endregion

    //#region events
    $(".li_dropdown").click(async (event) => {
        await openOrCloseTheSubmenuAsync($(":focus"));
    })
    //#endregion
})