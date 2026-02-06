import query from "../query.js";

export default async function getReport(token, data) {

    return await query(`api/reports/generate`, {
        mode: "cors",
        method: "POST",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
}