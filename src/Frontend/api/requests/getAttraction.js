import query from "../query.js";

export default async function getAttraction(token, id) {
    return await query(`api/attractions/${id}`, {
        mode: "cors",
        method: "GET",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
    });
}