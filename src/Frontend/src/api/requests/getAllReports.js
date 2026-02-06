import query from "../query.js";

export default async function getReport(token) {

    return await query(`api/reports`, {
        mode: "cors",
        method: "GET",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
    });
}