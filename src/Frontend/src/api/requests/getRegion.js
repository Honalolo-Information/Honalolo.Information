import query from "../query";

export default async function getRegion() {
    return await query(`api/dictionaries`, {
        mode: "cors",
        method: "GET",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json"
        },
    });
}