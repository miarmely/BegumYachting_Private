//#region variables
export let paginationInfosInJson;
export let entityCountOnTable;
export let isCriticalSection = {}  // {id: false, id: true, ...}
//#endregion

//#region functions

//#region file processes
export async function displayImageByNormalUrlAsync(
    folderPathAfterWwwroot,
    fileName,
    imgForAddUrl,
    inputForAddFileName,
    fileStatusLabel
) {
    await resetBeforeAddUrlAsync(
        imgForAddUrl,
        "#" + fileStatusLabel.attr("id"),
        inputForAddFileName);

    //#region display image
    // add src to <img>
    imgForAddUrl.attr(
        "src",
        "/" + folderPathAfterWwwroot + "/" + fileName
    );

    // write file name to <input>
    inputForAddFileName.val(fileName);
    //#endregion

    await removeFileStatusLabelAsync(fileStatusLabel);
}
export async function displayFileByDataUrlAsync(
    selectedFileInfos,
    elementForAddDataUrl,
    fileStatusLabel,
    inputForAddFileName = null,
    afterLoad = null,
    beforeLoad = null,
    attributeName = "src") {
    //#region before start
    let fileStatusLabel_oldMessage = fileStatusLabel.text();
    let fileStatusLabel_oldStyle = fileStatusLabel.attr("style");

    await resetBeforeAddUrlAsync(
        elementForAddDataUrl,
        "#" + fileStatusLabel.attr("id"),
        inputForAddFileName);
    //#endregion

    //#region read file as dataUrl
    let fileReader = new FileReader();
    fileReader.readAsDataURL(selectedFileInfos);
    //#endregion

    //#region when reading completed
    fileReader.onloadend = function (event) {
        //#region when successfull
        if (fileReader.readyState == fileReader.DONE) {
            //#region call function before load
            if (beforeLoad != null)
                beforeLoad();
            //#endregion

            //#region add dataUrl to attribute of element
            let dataUrl = event.target.result
            elementForAddDataUrl.attr(attributeName, dataUrl);
            //#endregion

            //#region write file name to <input>
            if (inputForAddFileName != null)
                inputForAddFileName.val(selectedFileInfos.name);
            //#endregion

            //#region call function after load
            if (afterLoad != null)
                afterLoad();
            //#endregion
        }
        //#endregion

        //#region add previous messages and styles to file status label
        removeFileStatusLabelAsync(fileStatusLabel);
        showFileStatusLabelAsync(fileStatusLabel, fileStatusLabel_oldMessage);

        fileStatusLabel.attr("style", fileStatusLabel_oldStyle);
        //#endregion
    }
    //#endregion
}
export async function displayFileByObjectUrlAsync(
    selectedFileInfos,
    elementForAddUrl,
    attributeName,
    fileStatusLabel,
    beforeDisplay = null,
    afterDisplay = null
) {
    //#region call beforeDisplay()
    if (beforeDisplay != null)
        beforeDisplay();
    //#endregion

    await removeObjectUrlFromElementAsync(elementForAddUrl, attributeName);
    await showFileStatusLabelAsync(
        fileStatusLabel,
        partnerInformationMessagesByLanguages[language]["fileLoading"],
        fileStatusLabel_color);

    //#region add object url to element
    let newObjectUrl = URL.createObjectURL(selectedFileInfos);
    elementForAddUrl.attr(attributeName, newObjectUrl);
    //#endregion

    //#region call afterDisplay()
    if (afterDisplay != null)
        afterDisplay();
    //#endregion

    await removeFileStatusLabelAsync(fileStatusLabel);
}
export async function getBase64StrOfFileAsync(selectedFileInfos) {
    //#region read file
    let fileReader = new FileReader();
    fileReader.readAsDataURL(selectedFileInfos);
    //#endregion

    //#region when reading process finished
    return new Promise((resolve) => {
        fileReader.onloadend = () => {
            //#region when reading process is successful
            if (fileReader.readyState == fileReader.DONE) {
                let dataUrl = fileReader.result;
                let base64Str = dataUrl.substring(dataUrl.indexOf(",") + 1);

                resolve(base64Str);
            }
            //#endregion
        }
    });
    //#endregion
}
export async function removeObjectUrlFromElementAsync(
    element,
    attributeName,
    afterRemove = null
) {
    // revoke url
    let oldObjectUrl = element.attr(attributeName);
    URL.revokeObjectURL(oldObjectUrl);

    // remove url from attribute
    element.removeAttr(attributeName);

    // when any process to be do is exists after remove
    if (afterRemove != null)
        afterRemove();
}
export async function resetBeforeAddUrlAsync(
    elementForAddUrl,
    fileStatusLabelId,
    inputForAddFileName = null,
    attributeName = "src"
) {
    // remove old url
    elementForAddUrl.removeAttr(attributeName);

    // reset file name on <input>
    if (inputForAddFileName != null)
        inputForAddFileName.val("");

    await showFileStatusLabelAsync(
        $(fileStatusLabelId),
        partnerInformationMessagesByLanguages[language]["fileLoading"],
        fileStatusLabel_color);
}
export async function isFileTypeValidAsync(
    selectedFileInfos,
    fileType
) {
    //#region when file type invalid
    if (!selectedFileInfos.type.startsWith(fileType))
        return false;
    //#endregion

    return true;
}
export async function isFileSizeValidAsync(fileSizeInByte, limitInMb) {
    //#region when file size is invalid
    let fileSizeInMb = (fileSizeInByte / 1024) / 1024;  // first division for KB; second division for MB

    if (fileSizeInMb > limitInMb)
        return false;
    //#endregion

    return true;
}
export function getFileTypeFromFileName(fileName) {
    return fileName.substring(
        fileName.lastIndexOf(".") + 1);
}
async function showFileStatusLabelAsync(fileStatusLabel, msg, msgColor) {
    // show file status
    fileStatusLabel
        .parent("div")
        .removeAttr("hidden");

    // show message
    updateResultLabel(
        '#' + fileStatusLabel.attr("id"),
        msg,
        msgColor);
}
async function removeFileStatusLabelAsync(fileStatusLabel) {
    // hide
    fileStatusLabel
        .parent("div")
        .attr("hidden", "");

    fileStatusLabel.empty();
}
//#endregion

//#region trends
export async function autoObjectMapperAsync(targetObject, sourceObject, dontAddNullValues = false) {
    //#region update target object values with source object values
    let keysOfTarget = Object.keys(targetObject);

    for (let sourceKey in sourceObject) {
        //#region when source key is exists in target object
        if (keysOfTarget.indexOf(sourceKey) != -1) {
            //#region when source object value is null (check null)
            if (dontAddNullValues && sourceObject[sourceKey] == null)
                continue;
            //#endregion

            targetObject[sourceKey] = sourceObject[sourceKey];
        }
        //#endregion
    }
    //#endregion
}
export function updateResultLabel(
    resultLabel,
    message,
    color,
    marginT = "0px",
    img_loading = null) {
    //#region resets
    // reset result label
    resultLabel.empty();

    // hide loading gif
    if (img_loading != null)
        img_loading.attr("hidden", "");
    //#endregion

    //#region change style
    resultLabel.attr("style",
        `color:	${color}; 
		margin-top: ${marginT};
		text-align: center`);
    //#endregion

    //#region write error to resultLabel
    resultLabel.removeAttr("hidden");  // show resultLabel
    resultLabel.append(message);
    //#endregion
}
//#endregion

export async function getDataByAjaxOrLocalAsync(keyNameInLocal, specialApiUrl) {
    //#region get data from local
    let dataInLocal = JSON.parse(
        localStorage.getItem(keyNameInLocal));
    //#endregion

    return await new Promise(resolve => {
        //#region get data by ajax if not exists in local (ajax)
        if (dataInLocal == null  // data of any language not exists in local
            || dataInLocal[language] == null)  // data belong to language not exists in local
            $.ajax({
                method: "GET",
                url: baseApiUrl + specialApiUrl,
                headers: {
                    authorization: jwtToken
                },
                contentType: "application/json",
                dataType: "json",
                success: (response) => {
                    //#region initialize "dataInLocal"
                    if (dataInLocal == null)  // when any data not exists (sometimes it can be exists on local but associated language of data is not exists on local)
                        dataInLocal = {};

                    dataInLocal[language] = response;
                    //#endregion

                    //#region save data to local
                    localStorage.setItem(
                        keyNameInLocal,
                        JSON.stringify(dataInLocal));
                    //#endregion

                    resolve(response);
                },
                complete: () => {
                    resolve(null);
                }
            });
        //#endregion

        //#region when data already in local
        else
            resolve(dataInLocal[language]);
        //#endregion
    })
}
export async function populateSelectAsync(select, options = [], optionToBeDisplay = null) {
    //#region add <option>'s to <select>
    for (let index in options) {
        let option = options[index];

        select.append(`<option>${option}</option>`);
    }
    //#endregion

    //#region set option to be display on <select>
    if (optionToBeDisplay != null)
        select.val(optionToBeDisplay);
    //#endregion
}
export async function setDisabledOfButtonAsync(doDisabled, button, bgColor) {
    //#region disable the button
    if (doDisabled) {
        button.attr("disabled", "");
        button.css("background-color", bgColor);
    }
    //#endregion

    //#region active the button
    else {
        button.removeAttr("disabled");
        button.css("background-color", bgColor);
    }
    //#endregion
}
export async function setDisabledOfButtonsAsync(doDisabled, buttonIds, bgColor) {
    //#region disable/enable multiple button
    for (let index in buttonIds) {
        //#region disable the button
        let button = $(buttonIds[index]);

        if (doDisabled) {
            button.attr("disabled", "");
            button.css("background-color", bgColor);
        }
        //#endregion

        //#region active the button
        else {
            button.removeAttr("disabled");
            button.css("background-color", bgColor);
        }
        //#endregion
    }
    //#endregion
}
export async function isAllObjectValuesNullAsync(object) {
    //#region compute total null value count
    let nullCounter = 0

    for (let key in object) {
        let value = object[key];

        if (value == null)
            nullCounter += 1;
    }
    //#endregion

    //#region when all object values is null
    if (nullCounter == Object.keys(object).length)
        return true;
    //#endregion

    return false;
}
export async function getKeysOfBlankValuesAsync(data) {
    //#region check whether blank that values of data 
    let keysWithBlankValue = [];

    for (let key in data) {
        let value = data[key];

        if (value == null
            || value == '')
            keysWithBlankValue.push(key);
    }
    //#endregion

    return keysWithBlankValue;
}
export async function isUserRoleThisRoleAsync(userRole, targetRole) {
    //#region check user role whether is desired role
    switch (targetRole) {
        case "user":
            if (userRole == "User" || userRole == "Kullanıcı") return true;
            return false;
        case "editor":
            if (userRole == "Editor" || userRole == "Editör") return true;
            return false;
        case "admin":
            if (userRole == "Admin" || userRole == "Yönetici") return true;
            return false;
    }
    //#endregion
}
export async function showOrHideLoadingImageAsync(mode, img_loading, lbl_result) {
    switch (mode) {
        case "show":
            lbl_result.empty();
            img_loading.removeAttr("hidden");
            break;
        case "hide":
            img_loading.attr("hidden", "");
    }
}
export async function shiftTheChildDivToBottomOfParentDivAsync(div_parent, div_child_Id) {
    //#region set variables
    let div_parent_whiteSpaceQuantity = div_parent.prop("offsetHeight");
    let infosOfChildrenDivs = div_parent.children();
    //#endregion

    //#region compute white space quantity of div_info (DYNAMICALLY)
    for (let index = 0; index < infosOfChildrenDivs.length; index++) {
        let infosOfChildrenDiv = infosOfChildrenDivs[index];
        div_parent_whiteSpaceQuantity -= infosOfChildrenDiv.offsetHeight;
    }
    //#endregion

    //#region shift the bottom desired div
    div_parent
        .children("#" + div_child_Id)
        .css("margin-top", div_parent_whiteSpaceQuantity);
    //#endregion
}
export async function addCriticalSectionAsync(
    criticalSectionId,
    func_toBeRunInCriticalAsync,
    timeout = null) {
    if (!isCriticalSection[criticalSectionId]) {
        isCriticalSection[criticalSectionId] = true;

        //#region when time is not desired
        if (timeout == null) {
            await func_toBeRunInCriticalAsync();
            isCriticalSection[criticalSectionId] = false;  // reset
        }
        //#endregion

        //#region when time is desired
        else
            setTimeout(async () => {
                await func_toBeRunInCriticalAsync();
                isCriticalSection[criticalSectionId] = false;  // reset
            }, timeout);
        //#endregion
    }
}
export function isExistsOnArray(array, value) {
    return array.indexOf(value) != -1;
}
export function getHeaderFromLocalInJson(headerName) {
    return JSON.parse(
        localStorage.getItem(headerName));
}
export function getTokenInSession() {
    return sessionStorage.getItem("token");
}
export function updateElementText(element, text) {
    element.empty();
    element.text(text);
}

//#endregion