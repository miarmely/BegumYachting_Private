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
export async function changeCellInfosOfRowAsync(row, newCellInfos) {
    let cellCount = row.prop("cells").length;

    for (let index = 0; index < cellCount; index++) {
        let td = row.children(`td:nth-child(${index + 1})`);

        td.empty();
        td.append(newCellInfos[index]);
    }
}
//#endregion