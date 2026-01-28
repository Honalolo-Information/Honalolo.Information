import { useContext } from "react";
import image from "../assets/login.png";
import Button from "../components/Button";
import Input from "../components/Input";
import AuthContext from "../contexts/AuthContext";
import { useNavigate } from "react-router";

export default function LoginPage() {
    const auth = useContext(AuthContext);
    const navigate = useNavigate("/search");

    return <div className="min-h-[calc(100vh-59px)] grid grid-cols-[2fr_1fr]">
        <img src={image} className="w-full h-full object-cover" />

        <div className="flex items-center justify-center">
            <div className="w-[400px] flex flex-col gap-4 ">
                <h1>Logowanie</h1>
                <Input label="Email" type="email" />
                <Input label="Hasło" type="password" />

                <Button onClick={handleLogin}>Zaloguj się</Button>
            </div>
        </div>
    </div>

    function handleLogin() {
        auth.setValue(true);
        navigate("/search");
    }
}