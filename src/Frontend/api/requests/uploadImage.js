import query from "../query.js";

export default async function uploadImage(token, id, body) {

    return await query(`api/attractions/${id}/photos`, {
        mode: "cors",
        method: "POST",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Authorization": `Bearer ${token}`
        },
        body: body
    });
}