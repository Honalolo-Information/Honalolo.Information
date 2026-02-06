import query from "../query.js";

export default async function deleteAttraction(token, id) {
    return await query(`api/attractions/${id}`, {
        mode: "cors",
        method: "DELETE",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
    });
}