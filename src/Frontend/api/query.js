import config from "./config.js";

export default async function query(endpoint, options = {}) {
    const url = `${config.url}/${endpoint}`;

    return fetch(url, options)
        .then(response => {
            if (!response.ok) {
                throw response.text();
            }
            return response.json();
        });
}
