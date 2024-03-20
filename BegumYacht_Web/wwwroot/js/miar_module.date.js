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

    //#region if time is "24:00", change it to "00:00"
    let dateInStr = dateTime.toLocaleString(
        window.navigator.language,
        formatOptions);
    let hourInStr = dateInStr.substring(
        dateInStr.indexOf(",") + 2);

    if (hourInStr == "24:00") {
        dateInStr = dateInStr.replace("24:00", "00:00");
    }
    else if (hourInStr == "24:00:00") {
        dateInStr = dateInStr.replace("24:00:00", "00:00:00");
    }
    //#endregion

    return dateInStr.replace(", ", "__");  // "12.03.2002, 13:00:00" ~~> "12.03.2002__13:00:00" ;
}
export async function convertStrUtcDateToStrLocalDateAsync(
    utcDateTimeInStr,
    add = { hours: true, minutes: true, seconds: true }) {
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
        .format(localDateTime);
    //#endregion

    //#endregion

    return formattedDate;
}
export async function convertStrUtcDateToIsoLocalDateAsync(utcDateTimeInStr, addTime = true) {
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

    //#region convert local date to iso date
    var isoDate = localDateTime.toISOString()

    if (!addTime) {
        isoDate = await removeTimeFromIsoDateAsync(isoDate);
    }
    //#endregion

    return isoDate;
}
export async function convertStrLocalDateToLocalDateAsync(localDateInStr) {
    localDateInStr = localDateInStr.replace("__", " ");  // "2040/02/20__11:12" ~~> "2040/02/20 11:12"

    return new Date(localDateInStr);
}
export async function convertStrLocalDateToUtcDateAsync(localDateInStr) {
    localDateInStr = localDateInStr.replace("__", ",");  // "01.03.2002__13.00" ~~> "01.03.2002, 13.00
    let localDate = new Date(localDateInStr);

    return await convertLocalDateToUtcDateAsync(localDate);
}
export async function convertStrUtcDateToLocalDateAsync(utcDateInStr) {
    utcDateInStr = utcDateInStr.replace("__", ",");  // "01.03.2002__13.00" ~~> "01.03.2002, 13.00"
    let utcDate = new Date(utcDateInStr);

    return await convertUtcDateToLocalDateAsync(utcDate);
}
export async function convertStrUtcDateToUtcDateAsync(utcDateInStr) {
    utcDateInStr = utcDateInStr.replace("__", ",");  // "01.03.2002__13.00" ~~> "01.03.2002, 13.00"

    return new Date(utcDateInStr);
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
        dateInfos.month - 1,
        dateInfos.day,
        dateInfos.hours,
        dateInfos.minutes,
        dateInfos.seconds);

    return new Date(localDateInNumber);
}
export async function addLocalDateToDateInputAsync(input, inputType, localDate = null, localDateInStr = null) {
    // inputType? "datetime" | "date"

    //#region when datetime in param is str (convert)
    if (localDateInStr != null)
        localDate = await convertStrLocalDateToLocalDateAsync(localDateInStr);
    //#endregion

    //#region add value to date input
    let dateInfos = await getDateInfosInJsonAsync(localDate);

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
export async function addUtcDateToDateInputAsync(input, inputType, utcDate = null, utcDateInStr = null) {
    // inputType? "datetime" | "date"

    //#region get date infos
    let localDate = utcDateInStr == null ?
        await convertUtcDateToLocalDateAsync(utcDate)
        : await convertStrUtcDateToLocalDateAsync(utcDateInStr);

    let dateInfos = await getDateInfosInJsonAsync(localDate);
    //#endregion

    //#region add value to date input
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
export async function getPassedTimeInStringAsync(
    utcDateInStr = null,
    localDateInStr = null
) {
    //#region get dates in unix
    let nowDate = new Date();
    let nowDateInMs = nowDate.getTime();

    let oldDateInLocal = utcDateInStr != null ?
        await convertUtcDateToLocalDateAsync(new Date(utcDateTimeInStr))
        : new Date(localDateInStr);
    let oldDateInMs = oldDateInLocal.getTime();
    //#endregion

    //#region return passed time  

    //#region set variables
    let dateDifferenceInSn = (nowDateInMs - oldDateInMs) / 10 ** 3;
    let totalSecondAtOneHour = 3600;
    let totalSecondAtOneDay = totalSecondAtOneHour * 24;
    let totalSecondAtOneMonth = totalSecondAtOneDay * 30;
    let totalSecondAtOneYear = (totalSecondAtOneDay * 365) + (totalSecondAtOneHour * 6)  // one year == 365 day 6 hours
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
        return yearDifference + languagePackage_message.TR.year;
    //#endregion

    //#region write passed time as month
    let monthDifference = Math.floor(dateDifferenceInSn / totalSecondAtOneMonth);

    if (monthDifference > 0)
        return monthDifference + languagePackage_message.TR.month;
    //#endregion

    //#region write passed time as day
    let dayDifference = Math.floor(dateDifferenceInSn / totalSecondAtOneDay);

    if (dayDifference > 0)
        return dayDifference + languagePackage_message.TR.day;
    //#endregion

    //#region write passed time as hours
    let hoursDifference = Math.floor(dateDifferenceInSn / totalSecondAtOneHour);

    if (hoursDifference > 0)
        return hoursDifference + languagePackage_message.TR.hours;
    //#endregion

    //#region write passed time as minutes
    let minutesDifference = Math.floor(dateDifferenceInSn / 60);

    if (minutesDifference > 0)
        return minutesDifference + languagePackage_message.TR.minutes;
    //#endregion

    //#region write passed time as second
    let secondDifference = Math.floor(dateDifferenceInSn);

    return secondDifference + languagePackage_message.TR.seconds;
    //#endregion

    //#endregion
}
export async function removeTimeFromIsoDateAsync(isoDate) {
    let indexOfT = isoDate.indexOf('T');
    isoDate = isoDate.substring(0, indexOfT);
    
    return isoDate;
}