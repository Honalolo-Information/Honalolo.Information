import { useState } from "react";
import FileUpload from "../components/FileUpload";
import Input from "../components/Input";
import Select from "../components/Select";
import Button from "../components/Button";

const categories = [
    { label: "Szlak", value: "trail" },
    { label: "Punkt widokowy", value: "viewpoint" },
    { label: "Woda", value: "water" },
    { label: "Jedzenie", value: "food" },
    { label: "Hotel", value: "hotel" },
    { label: "Wydarzenie", value: "event" },
];

const continents = [
    { label: "Europa", value: "eu" },
    { label: "Azja", value: "as" },
    { label: "Afryka", value: "af" },
];

const countries = [
    { label: "Polska", value: "pl" },
    { label: "Niemcy", value: "de" },
    { label: "Czechy", value: "cz" },
];

const regions = [
    { label: "Śląskie", value: "sl" },
    { label: "Małopolskie", value: "ml" },
    { label: "Łódzkie", value: "ld" },
];

const cities = [
    { label: "Katowice", value: "kat" },
    { label: "Kraków", value: "krk" },
]

const trailDiffculty = [
    { label: "Trudny", value: "kat" },
    { label: "Łatwy", value: "krk" },
]

export default function CreateAttraction() {
    const [type, setType] = useState("trail");

    return <div className="flex items-center justify-center p-4 pt-8">
        <div className="max-w-[600px] w-full flex flex-col gap-4">
            <h1>Nowa atrakcja</h1>

            <Input label="Nazwa atrakcji" />
            <Input label="Opis" type="textarea" />
            <Select value={type} onChange={setType} label="Kategoria" options={categories} />
            <Input type="float" label="Koszt atrakcji (opcjonalnie)" />
            <Input label="Języki (opcjonalnie)" />
            <Input label="Czas trwania (opcjonalnie)" />

            <Input label="Godziny otwarcia (opcjonalnie)" type="textarea" />

            <h2 className="mt-[32px] text-[32px]">Lokalizacja</h2>

            <Select label="Kontynent" options={continents} />
            <Select label="Kraj" options={countries} />
            <Select label="Region" options={regions} />
            <Select label="Miasto" options={cities} />

            {type === "trail" ? <TrailInputs /> : null}
            {type === "food" ? <FoodInputs /> : null}
            {type === "hotel" ? <HotelInputs /> : null}
            {type === "event" ? <EventInputs /> : null}

            <h2 className="mt-[32px] text-[32px]">Zdjęcia</h2>
            <FileUpload />

            <Button>Utwórz atrakcje</Button>
        </div>
    </div>
}

function TrailInputs() {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły szlaku</h2>
        <Select label="Poziom trudności" options={trailDiffculty} />
        <Input label="Dystans (m)" type="number" />
        <Input label="Wysokość (m)" type="number" />

        <Input label="Opis punktu startowego" />
        <Input label="Opis punktu końcowego" />
    </>
}

function FoodInputs() {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły jedzenia</h2>
        <Input label="Rodzaj jedzenia" />
    </>
}

function HotelInputs() {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły hotelu</h2>
        <Input label="Udogodnienia" type="textarea" />
    </>
}

function EventInputs() {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły wydarzenia</h2>
        <Input label="Rodzaj wydarzenia" />
        <Input label="Data i godzina startu" type="datetime-local" />
        <Input label="Data i godzina końca" type="datetime-local" />
    </>
}