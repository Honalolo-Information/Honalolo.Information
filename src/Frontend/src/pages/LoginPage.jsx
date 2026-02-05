import { useContext, useState } from "react";
import image from "../assets/login.png";
import Button from "../components/Button";
import Input from "../components/Input";
import AuthContext from "../contexts/AuthContext";
import loginRequest from "../../api/requests/loginRequest";
import { Link, useNavigate } from "react-router";

export default function LoginPage() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const auth = useContext(AuthContext);

    async function handleLogin(e) {
        e.preventDefault();
        try {
            const r = await loginRequest({
                Email: email,
                Password: password
            });

            auth.setValue(r.token);
            auth.setUsername(r.userName);
            auth.setUserId(r.userId);

            localStorage.setItem("authToken", r.token);
            localStorage.setItem("userId", r.userId);
            localStorage.setItem("username", r.userName);

            navigate("/search");
        } catch (error) {
            alert("Błędny email lub hasło")
        }
    }

    return <div className="min-h-[calc(100vh-59px)] grid grid-cols-[2fr_1fr]">
        <img src={image} className="hidden sm:flex w-full h-full object-cover" />

        <form onSubmit={handleLogin} className="w-full flex items-center justify-center">
            <div className="p-8 w-[100vw] sm:w-[400px] flex flex-col gap-4 ">
                <h1>Logowanie</h1>
                <Input value={email} onChange={setEmail} label="Email" type="email" required />
                <Input value={password} onChange={setPassword} label="Hasło" type="password" required />

                <p> Nie masz konta? To je <Link to="/register" className="underline">załóż</Link>.  </p>

                <Button>Zaloguj się</Button>
            </div>
        </form>
    </div>
}