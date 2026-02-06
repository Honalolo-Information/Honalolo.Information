import query from "../query.js";

export default async function registerRequest(credentials) {
    return await query(`api/users/register`, {
        mode: "cors",
        method: "POST",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(credentials)
    });
}