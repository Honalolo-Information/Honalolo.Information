import FeatherIcon from "feather-icons-react";
import Attraction from "../components/Attraction";
import Button from "../components/Button";
import { Link } from "react-router";
import { useContext, useEffect, useState } from "react";
import AuthContext from "../contexts/AuthContext";
import getAttraction from "../api/requests/getAttraction";
import getProfile from "../api/requests/getProfile";

export default function ProfilePage() {
    const auth = useContext(AuthContext)
    const [data, setData] = useState(null);

    useEffect(() => {
        handleLoad();
    }, [auth.value]);

    async function handleLoad() {
        const r = await getProfile(auth.value);
        setData(r);
        console.log(r);
    }

    if(!data) return null;

    return <div className="p-4 pt-8 mx-auto">
        <div className="flex items-center gap-8">
            {/* <Avatar /> */}
            <h1>{auth.username}</h1>
        </div>

        <div className="mt-7 flex flex-col gap-4">
            <div className="flex items-center justify-between">
                <h2 className="font-medium text-[24px] md:text-[32px]">Dodane atrakcje</h2>
                <Link to="/create">
                    <Button>Dodaj atrakcje</Button>
                </Link>
            </div>

            <div className="m-auto grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-3">
                {data.myAttractions.map((attraction) => (
                    <Attraction key={attraction.id} data={attraction} />
                ))}
            </div>
        </div>
    </div>
}

function Avatar() {
    return <div className="w-[75px] aspect-square bg-white border-1 rounded-[100%] flex items-center justify-center">
        <FeatherIcon icon="user" size={50} strokeWidth={1} />
    </div>

}