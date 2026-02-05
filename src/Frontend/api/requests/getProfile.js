import query from "../query.js";

export default async function getProfile(token) {
    return await query(`api/users/me`, {
        mode: "cors",
        method: "GET",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
    });
}