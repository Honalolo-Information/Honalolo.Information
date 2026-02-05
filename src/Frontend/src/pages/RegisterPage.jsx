import { useContext, useState } from "react";
import image from "../assets/register.jpg";
import Button from "../components/Button";
import Input from "../components/Input";
import registerRequest from "../../api/requests/registerRequest";
import AuthContext from "../contexts/AuthContext";
import { Link, useNavigate } from "react-router";

export default function RegisterPage() {
    const auth = useContext(AuthContext);
    const navigate = useNavigate();

    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    async function handleRegister(e) {
        e.preventDefault();

        if (password !== confirmPassword) {
            alert("Hasła nie są takie same");
            return;
        }

        try {
            const r = await registerRequest({
                UserName: name,
                Email: email,
                Password: password,
                Role: 1
            });

            auth.setValue(r.token);
            auth.setUsername(r.userName);

            navigate("/search");

        } catch (error) {
            alert(await error);
        }
    }

    return <div className="min-h-[calc(100vh-59px)] grid grid-cols-[2fr_1fr]">
        <img src={image} className="hidden sm:flex w-full h-full object-cover" />

        <form onSubmit={handleRegister} className="flex items-center justify-center">
            <div className="p-8 w-[100vw] sm:w-[400px] flex flex-col gap-4 ">
                <h1>Rejestracja</h1>
                <Input value={name} onChange={setName} label="Imię i nazwisko" type="text" required />
                <Input value={email} onChange={setEmail} label="Email" type="email" required />
                <Input value={password} onChange={setPassword} label="Hasło" type="password" required />
                <Input value={confirmPassword} onChange={setConfirmPassword} label="Potwierdź hasło" type="password" required />


                <p> Masz ju konto? To się <Link to="/register" className="underline">zaloguj</Link>.  </p>

                <Button>Zarejestruj się</Button>
            </div>
        </form>
    </div>
}