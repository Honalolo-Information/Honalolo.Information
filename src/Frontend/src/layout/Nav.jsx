import { Link, useNavigate } from "react-router";
import logo from "../assets/logo.png";
import Button from "../components/Button";
import { useContext } from "react";
import AuthContext from "../contexts/AuthContext";
import FeatherIcon from "feather-icons-react";

export default function Nav() {
    const auth = useContext(AuthContext);
    const navigate = useNavigate();

    return <nav className="w-full p-2 py-1 bg-white border-b-1">
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
                        <Button>Logowanie</Button>
                    </Link>
                    <Link to="/register">
                        <Button className="!bg-[#ddd]">Rejestracja</Button>
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