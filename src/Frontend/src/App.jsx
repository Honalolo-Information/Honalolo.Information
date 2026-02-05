import { useEffect, useState } from "react";
import AuthContext from "./contexts/AuthContext"

function App(props) {
    const [userId, setUserId] = useState("");
    const [username, setUsername] = useState("");
    const [auth, setAuth] = useState(false);

    const authValue = {
        userId, setUserId,
        value: auth,
        setValue: setAuth,
        username: username,
        setUsername: setUsername
    };

    useEffect(() => {
        const token = localStorage.getItem("authToken");
        const storedUsername = localStorage.getItem("username");
        const storedUserId = localStorage.getItem("userId");

        if (token) {
            setAuth(token);
        }
        if (storedUsername) {
            setUsername(storedUsername);
        }
        if (storedUserId) {
            setUserId(storedUserId);
        }
    }, []);

    return (
        <AuthContext.Provider value={authValue} >
            {props.children}
        </AuthContext.Provider>
    )
}

export default App
