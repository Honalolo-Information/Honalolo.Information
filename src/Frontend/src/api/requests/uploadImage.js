import query from "../query.js";

export default async function uploadImage(token, id, body, method = "POST") {

    return await query(`api/attractions/${id}/photos`, {
        mode: "cors",
        method: method,
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Authorization": `Bearer ${token}`
        },
        body: body
    });
}