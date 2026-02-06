import { Link, useNavigate } from "react-router";
import logo from "../assets/logo.png";
import Button from "../components/Button";
import { useContext } from "react";
import AuthContext from "../contexts/AuthContext";
import FeatherIcon from "feather-icons-react";

export default function Nav() {
    const auth = useContext(AuthContext);
    const navigate = useNavigate();

    return <nav className="w-full p-3 pr-4 md:pr-auto py-1 bg-white border-[var(--border-color)] border-b-1">
        <div className="md:px-7 mx-auto flex justify-between items-center ">
            {auth.value == "" ?
                <Link to="/">
                    <img src={logo} className="h-[50px]" />
                </Link> :
                <Link to="/search">
                    <img src={logo} className="h-[50px]" />
                </Link>
            }

            {!auth.value ? (
                <div className="pr-2 flex gap-2">
                    <Link to="/login">
                        <Button className="!px-3 !py-1 !text-[14px]">Logowanie</Button>
                    </Link>
                    <Link className="hidden sm:flex" to="/register">
                        <Button className="!px-3 !py-1 !bg-[#eee] border-1 !border-[var(--border-color)] !text-[14px]">Rejestracja</Button>
                    </Link>
                </div>)
                :
                <div className="flex gap-5 items-center">
                    {/* <Button>Nowa atrakcja</Button> */}
                    <Link to="/create" className="flex items-center gap-2">
                        <FeatherIcon icon="plus" size={24} />
                    </Link>

                    <Link to="/profile" className="flex items-center gap-2">
                        <FeatherIcon icon="user" size={24} />
                    </Link>
                    <div onClick={handleLogout} className="flex items-center gap-2">
                        <FeatherIcon icon="log-out" size={24} />
                    </div>
                </div>
            }
        </div>
    </nav>

    function handleLogout() {
        auth.setValue("");
        auth.setUsername("");
        auth.setUserId("");
        localStorage.removeItem("authToken");
        localStorage.removeItem("username");
        localStorage.removeItem("userId");
        navigate("/login");
    }
}