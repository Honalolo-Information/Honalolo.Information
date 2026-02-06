import query from "../query.js";

export default async function downloadReport(token, id) {

    return await query(`api/reports/${id}/download`, {
        mode: "cors",
        method: "GET",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
    }, true);
}