import query from "../query.js";

export default async function loginRequest(credentials) {

    return await query(`api/users/login`, {
        mode: "cors",
        method: "POST",
        headers: {
            "Allow-Control-Allow-Origin": "*",
            "Content-Type": "application/json"
        },
        body: JSON.stringify(credentials)
    });
}