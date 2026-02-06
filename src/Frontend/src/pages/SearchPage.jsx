import Attraction from "../components/Attraction";
import Input from "../components/Input";
import { useContext, useEffect, useState } from "react";
import getAttraction from "../api/requests/getAttraction";
import AuthContext from "../contexts/AuthContext";
import FeatherIcon from "feather-icons-react";
import LocationSelector from "../components/LocationSelector";
import getRegion from "../api/requests/getRegion";


export default function SearchPage() {
    const auth = useContext(AuthContext)
    const [isHidden, setHidden] = useState(false);

    const [data, setData] = useState([]);
    const [baseData, setBaseData] = useState([]);
    const [region, setRegion] = useState(false);

    const [search, setSearch] = useState(null);
    const [sName, setSName] = useState("");

    useEffect(() => {
        handleLoad();
    }, [auth.value]);

    async function handleLoad() {
        const r = await getAttraction(auth.value, "");
        setBaseData(r);
        setData(r);
        console.log(r);

        try {
            const l = await getRegion();
            setRegion(l);
        } catch (error) {
            alert("Sorki coś poszło nie tak");
        }
    }

    useEffect(() => {
        handleLoad();
    }, []);

    useEffect(() => {
        if (!search) return;
        console.log(search);

        let d = baseData;

        if (search.type) {
            const typeName = region.attractionTypes.find((item) => item.id == search.type).name;
            d = d.filter((item) => item.typeName == typeName);
        }

        if (search.continent) {
            d = d.filter((item) => item.continentId == search.continent);
        }

        if (search.country) {
            d = d.filter((item) => item.countryId == search.country);
        }

        if (search.region) {
            d = d.filter((item) => item.regionId == search.region);
        }


        if (search.city) {
            d = d.filter((item) => item.cityId == search.city);
        }

        if (sName.trim() !== "") {
            d = d.filter((item) => item.title.toLowerCase().trim().includes(sName.toLowerCase().trim()));
        }

        setData(d);
    }, [search, sName]);



    return <div className="h-[calc(100vh-var(--nav-h))] grid grid-cols-[1fr]">
        <div className="h-[calc(100vh-var(--nav-h))] overflow-y-auto">
            <div className={
                "p-3 lg:p-8 grid-rows-[auto] " +
                "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 lg:gap-4 gap-y-4"
            }>
                {data.map((item, index) => {
                    return <Attraction data={item} key={index} />
                })}

            </div>
        </div>

        <div className={`${isHidden ? "translate-x-[100%] w-0 " : ""} transition-transform bg-white w-[400px] max-w-[calc(100vw-40px)] absolute top-[var(--nav-h)] right-0 p-4 md:p-8 border-l-1 border-[var(--border-color)] h-[calc(100vh-var(--nav-h))] `}>
            <div onClick={() => setHidden(!isHidden)} className="absolute right-[100%] top-[var(--nav-h)] w-10 h-10 bg-white z-5 border-1 border-[var(--border-color)] rounded-l-[var(--rounded)] flex items-center justify-center text-[#555]" >
                <FeatherIcon icon={isHidden ? "chevron-left" : "chevron-right"} size={24} />
            </div>

            <div className={isHidden ? "hidden" : ""}>
                <h2 className="font-medium text-[24px] md:text-[32px]">Wyszukiwarka</h2>

                <div className="my-4 grid gap-3">
                    <LocationSelector
                        data={region}
                        value={search}
                        onChange={setSearch}
                    />

                    <Input value={sName} onChange={setSName} label="Nazwa atrakcji" placeholder="Wpisz nazwę atrakcji" />
                </div>
            </div>
        </div>
    </div>
}