export async function getDateInfosInJsonAsync(dateTime) {
    //#region set variables
    let month = dateTime.getMonth() + 1;
    let day = dateTime.getDate();
    let hours = dateTime.getHours();
    let minutes = dateTime.getMinutes();
    let seconds = dateTime.getSeconds();
    //#endregion

    return {
        year: dateTime.getFullYear(),
        month: month < 10 ? '0' + month : month,
        day: day < 10 ? '0' + day : day,
        hours: hours < 10 ? '0' + hours : hours,
        minutes: minutes < 10 ? '0' + minutes : minutes,
        seconds: seconds < 10 ? '0' + seconds : seconds,
    };
}
export async function convertStrUtcDateToStrLocalDateAsync(
    utcDateTimeInStr,
    add = {hours: true, minutes: true, seconds: true}) {
    //#region when datetime is invalid
    if (utcDateTimeInStr == null
        || utcDateTimeInStr == "")
        return;
    //#endregion

    //#region convert utc date to local date
    let utcDateTime = new Date(utcDateTimeInStr);
    let localDateTime = new Date(Date.UTC(
        utcDateTime.getFullYear(),
        utcDateTime.getMonth(),
        utcDateTime.getDate(),
        utcDateTime.getHours(),
        utcDateTime.getMinutes(),
        utcDateTime.getSeconds()
    ));
    //#endregion

    //#region set pattern of local date

    //#region set date format options
    let formatOptions = {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
    };
    if (add.hours) {
        formatOptions["hour"] = "2-digit";
        formatOptions["hour12"] = false;
    }
    if (add.minutes) formatOptions["minute"] = "2-digit";
    if (add.seconds) formatOptions["second"] = "2-digit";
    //#endregion

    //#region set pattern
    let formatter = new Intl.DateTimeFormat(
        window.navigator.language,
        formatOptions);

    let formattedDate = formatter
        .format(localDateTime)
        .replace(", ", "__");
    //#endregion

    //#endregion

    return formattedDate;
}
export async function convertStrDateToDateAsync(dateInStr) {
    dateInStr = dateInStr.replace("__", ",");  // "01.03.2002__13.00" ~~> "01.03.2002, 13.00"

    return new Date(dateInStr);
}
export async function convertLocalDateToUtcDateAsync(localDate) {
    let utcDateInStr = localDate.toLocaleString(window.navigator.language, {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        hour12: false,
        timeZone: "UTC",
    });
    
    return new Date(utcDateInStr);
}
export async function convertUtcDateToLocalDateAsync(utcDate) {
    let dateInfos = await getDateInfosInJsonAsync(utcDate);
    let localDateInNumber = Date.UTC(
        dateInfos.year,
        dateInfos.month,
        dateInfos.day,
        dateInfos.hours,
        dateInfos.minutes,
        dateInfos.seconds);

    return new Date(localDateInNumber);
}
export async function convertDateToStrDateAsync(
    dateTime,
    add = { hours: true, minutes: true, seconds: true }) {
    //#region set date format options
    let formatOptions = {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
    };
    if (add.hours) {
        formatOptions["hour"] = "2-digit";
        formatOptions["hour12"] = false;
    }
    if (add.minutes) formatOptions["minute"] = "2-digit";
    if (add.seconds) formatOptions["second"] = "2-digit";
    //#endregion

    //#region convert date to str date
    let dateInStr = dateTime.toLocaleString(
        window.navigator.language,
        formatOptions);

    dateInStr = dateInStr.replace(", ", "__");  // "12.03.2002, 13:00:00" ~~> "12.03.2002__13:00:00" 
    //#endregion

    return dateInStr;
}
export async function addValueToDateInputAsync(input, inputType, dateTime = null, dateTimeInStr = null) {
    // inputType? "datetime" | "date"

    //#region when datetime in param is str (convert)
    if (dateTimeInStr != null)
        dateTime = await convertStrDateToDateAsync(dateTimeInStr);
    //#endregion

    //#region add value to date input
    let dateInfos = await getDateInfosInJsonAsync(dateTime);

    switch (inputType) {
        case "datetime":
            input.val(
                dateInfos.year + "-" +
                dateInfos.month + "-" +
                dateInfos.day + "T" +
                dateInfos.hours + ":" +
                dateInfos.minutes);
            break;

        case "date":
            input.val(
                dateInfos.year + "-" +
                dateInfos.month + "-" +
                dateInfos.day);
            break;
    }
    //#endregion
}
export async function isDatesEqualAsync(date1, date2, check = {
    year: true,
    month: true,
    day: true,
    hours: true,
    minutes: true,
    seconds: true
}) {
    //#region compare dates
    if (check.year && date1.getFullYear() != date2.getFullYear()) return false;
    if (check.month && date1.getMonth() != date2.getMonth()) return false;
    if (check.day && date1.getDate() != date2.getDate()) return false;
    if (check.hours && date1.getHours() != date2.getHours()) return false;
    if (check.minutes && date1.getMinutes() != date2.getMinutes()) return false;
    if (check.seconds && date1.getSeconds() != date2.getSeconds()) return false;
    //#endregion

    return true;
}