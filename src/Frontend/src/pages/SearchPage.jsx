import Attraction from "../components/Attraction";
import Button from "../components/Button";
import Select from "../components/Select";
import Input from "../components/Input";
import { useContext, useEffect, useState } from "react";
import getAttraction from "../../api/requests/getAttraction";
import AuthContext from "../contexts/AuthContext";

const continents = [
    { label: "Chłopea" }
];

const countries = [
    { label: "Chłopia" }
];

const states = [
    { label: "Chłopskie" }
];

const cities = [
    { label: "Chłopowice" }
];

const types = [
    { label: "Szlak" },
    { label: "Punkt widokowy" },
    { label: "Jezioro" }
];

export default function SearchPage() {
    const auth = useContext(AuthContext)
    const [data, setData] = useState([]);

    useEffect(() => {
        handleLoad();
    }, [auth.value]);

    async function handleLoad() {
        const r = await getAttraction(auth.value, "");
        console.log(r);
        setData(r);
    }

    return <div className="h-[calc(100vh-58px)] grid grid-cols-[1fr_400px]">
        <div className={
            "p-8 h-[calc(100vh-58px)] " +
            "grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-3 gap-y-8 " +
            "overflow-y-auto "
        }>
            {data.map((item, index) => {
                return <Attraction data={item} key={index} />
            })}
        </div>

        <div className="p-8 border-l-1 h-full ">
            <h2 className="font-medium text-[24px] md:text-[32px]">Wyszukiwarka</h2>

            <div className="my-4 grid gap-3">
                <Select label="Typ atrakcji" options={types} />
                <Select label="Kontynent" options={continents} />
                <Select label="Kraj" options={countries} />
                <Select label="Region" options={states} />
                <Select label="Miasto" options={cities} />
                <Input label="Nazwa atrakcji" placeholder="Wpisz nazwę atrakcji" />
            </div>

            <Button className="mt-2">Szukaj</Button>
        </div>
    </div>
}