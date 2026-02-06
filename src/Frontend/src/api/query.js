import config from "./config.js";

export default async function query(endpoint, options = {}, isBlob = false) {
    const url = `${config.url}/${endpoint}`;

    return fetch(url, options)
        .then(response => {
            if (!response.ok) {
                throw response.text();
            }

            if(isBlob) {
                return response.blob();
            }
            return response.json();
        });
}
