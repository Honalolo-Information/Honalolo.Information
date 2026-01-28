import Attraction from "../components/Attraction";
import Button from "../components/Button";
import Select from "../components/Select";
import Input from "../components/Input";

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
    return <div className="h-[calc(100vh-58px)] grid grid-cols-[1fr_400px]">
        <div className={
            "p-8 h-[calc(100vh-58px)] " +
            "grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-3 gap-y-8 " +
            "overflow-y-auto "
        }>
            {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12].map((item, index) => {
                return <Attraction key={index} />
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