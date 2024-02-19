import { openOrCloseTheSubmenuAsync } from "./miar_module.sidebarMenu.js";

$(function () {
    //#region events
    $(".li_dropdown").click(async () => {
        await openOrCloseTheSubmenuAsync($(":focus"));
    })
    //#endregion

    //#region functions
    async function setPageAsync() {
        // add username to menubar
        $("#spn_username").append(accountInfos
            .nameSurname
            .toUpperCase());
    }
    //#endregion

    setPageAsync();
})