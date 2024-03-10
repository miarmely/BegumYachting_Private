import { openOrCloseTheSubmenuAsync } from "./miar_module.sidebarMenu.js";

$(function () {
    //#region events
    $("#ul_profileOptions").click(() => {
        let selectedMenu = $(":focus");

        switch (selectedMenu.attr("id")) {
            //#region when clicked to "logout"
            case "a_logout":
                //#region reset all local
                // reset all local
                let usernameForLogin = localStorage.getItem(localKeys.username);
                localStorage.clear();

                // add "username" to local again
                if (usernameForLogin != null)
                    localStorage.setItem(localKeys.username, usernameForLogin);
                //#endregion

                window.location.replace("/authentication/login");
                break;
            //#endregion
        }
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