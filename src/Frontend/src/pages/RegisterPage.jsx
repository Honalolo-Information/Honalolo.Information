import image from "../assets/register.jpg";
import Button from "../components/Button";
import Input from "../components/Input";

export default function RegisterPage() {
    return <div className="min-h-[calc(100vh-59px)] grid grid-cols-[2fr_1fr]">
        <img src={image} className="w-full h-full object-cover" />

        <div className="flex items-center justify-center">
            <div className="w-[400px] flex flex-col gap-4 ">
                <h1>Rejestracja</h1>
                <Input label="Imię i nazwisko" type="text" />
                <Input label="Email" type="email" />
                <Input label="Hasło" type="password" />
                <Input label="Potwierdź hasło" type="password" />
                <Button>Zaloguj się</Button>
            </div>
        </div>
    </div>
}