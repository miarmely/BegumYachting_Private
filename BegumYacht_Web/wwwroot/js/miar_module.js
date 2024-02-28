//#region variables
export let paginationInfosInJson;
export let entityCountOnTable;
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

//#region pagination
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
            $("#a_paginationBack").attr("hidden", "");

        // show
        else
            $("#a_paginationBack").removeAttr("hidden");
        //#endregion

        //#region for paginationNext button
        // hide
        if (paginationInfosInJson.CurrentPageNo == paginationInfosInJson.TotalPage)
            $("#a_paginationNext").attr("hidden", "");

        // show
        else
            $("#a_paginationNext").removeAttr("hidden");
        //#endregion
    }
}
//#endregion

export async function populateElementByAjaxOrLocalAsync(
    keyNameInLocal,
    specialApiUrl,
    func_populate,
    func_afterPopulated = null) {
    //#region get data from local
    let dataInLocal = JSON.parse(
        localStorage.getItem(keyNameInLocal));
    //#endregion

    //#region get data by ajax if not exists in local (ajax)
    if (dataInLocal == null  // data of any language not exists in local
        || dataInLocal[language] == null)  // data belong to language not exists in local
        $.ajax({
            method: "GET",
            url: baseApiUrl + specialApiUrl,
            headers: {
                "Authorization": jwtToken
            },
            contentType: "application/json",
            dataType: "json",
            success: (response) => {
                //#region save data to local

                //#region initialize "dataInLocal"
                if (dataInLocal == null)  // when any data not exists
                    dataInLocal = {};

                dataInLocal[language] = response;
                //#endregion

                //#region add to local
                localStorage.setItem(
                    keyNameInLocal,
                    JSON.stringify(dataInLocal));
                //#endregion

                //#endregion

                func_populate(response);

                //#region call function after populate process
                if (func_afterPopulated != null)
                    func_afterPopulated();
                //#endregion
            }
        });
    //#endregion

    //#region when data already in local
    else {
        func_populate(dataInLocal[language]);

        //#region call function after populate process
        if (func_afterPopulated != null)
            func_afterPopulated();
        //#endregion
    }
    //#endregion
}
export async function populateSelectAsync(select, options, optionToBeDisplay = null) {
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
export async function getPassedTimeInStringAsync(utcDateTimeInStr) {
    //#region convert old date in utc to local date
    let oldDateInUtc = new Date(utcDateTimeInStr);
    let oldYear = oldDateInUtc.getFullYear();
    let oldMonth = oldDateInUtc.getMonth();
    let oldDay = oldDateInUtc.getDate();
    let oldHours = oldDateInUtc.getHours();
    let oldMinutes = oldDateInUtc.getMinutes();
    let oldSeconds = oldDateInUtc.getSeconds();

    let oldDateInLocal = new Date(
        oldYear,
        oldMonth,
        oldDay,
        oldHours + 3,
        oldMinutes,
        oldSeconds);
    //#endregion

    //#region get dates in unix
    let nowDate = new Date();
    let nowDateInMs = nowDate.getTime();
    let oldDateInMs = oldDateInLocal.getTime();
    //#endregion

    //#region return passed time  

    //#region set variables
    let totalSecondAtOneHour = 3600;
    let totalSecondAtOneDay = totalSecondAtOneHour * 24;
    let totalSecondAtOneMonth = totalSecondAtOneDay * 30;
    let totalSecondAtOneYear = (totalSecondAtOneDay * 365) + (totalSecondAtOneHour * 6)  // one year == 365 day 6 hours
    let dateDifferenceInSn = (nowDateInMs - oldDateInMs) / 10 ** 3;
    //#endregion

    //#region write passed time as year
    var languagePackage_message = {
        "TR": {
            "year": " yıl önce",
            "month": " ay önce",
            "day": " gün önce",
            "hours": " saat önce",
            "minutes": " dakika önce",
            "seconds": " saniye önce"
        },
        "EN": {
            "year": " year ago",
            "month": " month ago",
            "day": " day ago",
            "hours": " hours ago",
            "minutes": " minutes ago",
            "seconds": " seconds ago"
        }
    };
    let yearDifference = Math.floor(dateDifferenceInSn / totalSecondAtOneYear);

    if (yearDifference > 0)
        return yearDifference + languagePackage_message[language]["year"];
    //#endregion

    //#region write passed time as month
    let monthDifference = Math.floor(dateDifferenceInSn / totalSecondAtOneMonth);

    if (monthDifference > 0)
        return monthDifference + languagePackage_message[language]["month"];
    //#endregion

    //#region write passed time as day
    let dayDifference = Math.floor(dateDifferenceInSn / totalSecondAtOneDay);

    if (dayDifference > 0)
        return dayDifference + languagePackage_message[language]["day"];
    //#endregion

    //#region write passed time as hours
    let hoursDifference = Math.floor(dateDifferenceInSn / totalSecondAtOneHour);

    if (hoursDifference > 0)
        return hoursDifference + languagePackage_message[language]["hours"];
    //#endregion

    //#region write passed time as minutes
    let minutesDifference = Math.floor(dateDifferenceInSn / 60);

    if (minutesDifference > 0)
        return minutesDifference + languagePackage_message[language]["minutes"];
    //#endregion

    //#region write passed time as second
    let secondDifference = Math.floor(dateDifferenceInSn);

    return secondDifference + languagePackage_message[language]["seconds"];
    //#endregion

    //#endregion
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
export function updateElementText(element, text) {
    element.empty();
    element.text(text);
}
//#endregion