import query from "../query.js";

export default async function editAttraction(token, id, data) {

    return await query(`api/attractions/${id}`, {
        mode: "cors",
        method: "PUT",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
}