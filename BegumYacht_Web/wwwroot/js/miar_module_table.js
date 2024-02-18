//#region functions
export async function getCellInfosOfClickedRowAsync(event) {
    //#region save td <values> of row
    let cellInfos = []
    let cells = event.currentTarget.cells;

    for (let index = 0; index < cells.length; index++)
        cellInfos.push(
            cells[index].innerText);
    //#endregion

    return cellInfos;
}
//#endregion