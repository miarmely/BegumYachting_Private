import { openOrCloseTheSubmenuAsync } from "./miar_module.sidebarMenu.js";

$(function () {
    //#region events
    $("#a_logout").click(() => {
        // reset all local
        let usernameForLogin = localStorage.getItem(localKeys.username);
        localStorage.clear();
       
        // add "username" to local again
        if (usernameForLogin != null)
            localStorage.setItem(localKeys.username, usernameForLogin);
    })
    $(".li_dropdown").click(async () => {
        await openOrCloseTheSubmenuAsync($(":focus"));
    }) // submenus of sidebar
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