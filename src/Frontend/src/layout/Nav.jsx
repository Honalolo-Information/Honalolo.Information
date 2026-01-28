import { Link } from "react-router";
import logo from "../assets/logo.png";
import Button from "../components/Button";
import { useContext } from "react";
import AuthContext from "../contexts/AuthContext";
import FeatherIcon from "feather-icons-react";

export default function Nav() {
    const auth = useContext(AuthContext);

    return <nav className="w-full p-2 py-1 bg-white border-b-1">
        <div className="md:px-7 mx-auto flex justify-between items-center ">
            <Link to="/">
                <img src={logo} className="h-[50px]" />
            </Link>

            {/* <div className="flex gap-4 text-[15px]">
                <Link to="/">Strona główna</Link>
                <Link to="/search">Wyszukiwarka</Link>
                <Link>O nas</Link>
                <Link>Kontakt</Link>
            </div> */}

            {!auth.value ? (
                <div className="pr-2 flex gap-2">
                    <Link to="/login">
                        <Button>Logowanie</Button>
                    </Link>
                    <Link to="/register">
                        <Button className="!bg-[#ddd]">Rejestracja</Button>
                    </Link>
                </div>) : <Link to="/profile" className="flex items-center gap-2">
                <FeatherIcon icon="user" size={24} />
                Nazwa użytkownika
            </Link>}
        </div>
    </nav>

}