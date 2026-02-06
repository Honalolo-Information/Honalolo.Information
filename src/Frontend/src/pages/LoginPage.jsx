import { useContext, useState } from "react";
import image from "../assets/register.jpg";
import Button from "../components/Button";
import Input from "../components/Input";
import AuthContext from "../contexts/AuthContext";
import loginRequest from "../api/requests/loginRequest";
import { Link, useNavigate } from "react-router";

import logo from "../assets/pineapple.png";

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
        <img src={image} className="brightness-80 hidden md:flex w-full h-full object-cover" />

        <form onSubmit={handleLogin} className="w-full flex items-center justify-center">
            <div className="p-4 sm:p-8 w-[100vw] md:w-[500px] flex flex-col gap-4 ">

                <img src={logo} className="w-24 mx-auto" />
                <div className="mb-4 text-center">
                    <h1>Zaloguj się na&nbsp;swoje&nbsp;konto</h1>
                    <p>Witamy ponownie! Prosze wpisz dane logowania.</p>
                </div>

                <Input value={email} onChange={setEmail} label="Email" type="email" required />
                <Input value={password} onChange={setPassword} label="Hasło" type="password" required />


                <Button>Zaloguj się</Button>

                <p className="text-center mt-3">  
                    <span>Nie masz konta? </span>
                    <Link to="/register" className="underline text-[var(--accent-dark)]">To se załóż.</Link>  
                </p>
            </div>
        </form>
    </div>
}