import { useEffect, useState } from "react";
import Select from "./Select";

export default function LocationSelector(props) {
    const [typeVal, setTypeVal] = useState("none");
    const [continentVal, setContinentVal] = useState("none");
    const [countryVal, setCountryVal] = useState("none");
    const [regionVal, setRegionVal] = useState("none");
    const [cityVal, setCityVal] = useState("none");

    const [baseCountries, baseSetCountries] = useState([]);
    const [baseRegions, baseSetRegions] = useState([]);
    const [baseCities, baseSetCities] = useState([]);

    const [continents, setContinents] = useState([]);
    const [countries, setCountries] = useState([]);
    const [regions, setRegions] = useState([]);
    const [cities, setCities] = useState([]);

    const [types, setTypes] = useState([]);

    useEffect(() => {
        props.onChange({
            type: parseInt(typeVal),
            continent: parseInt(continentVal),
            country: parseInt(countryVal),
            region: parseInt(regionVal),
            city: parseInt(cityVal)
        });
    }, [typeVal, continentVal, countryVal, regionVal, cityVal]);

    useEffect(() => {
        if (!props.data) {
            return
        }

        baseSetCountries(props.data.countries.map((item) => ({ label: item.name, value: item.id, parentId: item.parentId })));
        baseSetRegions(props.data.regions.map((item) => ({ label: item.name, value: item.id, parentId: item.parentId })));
        baseSetCities(props.data.cities.map((item) => ({ label: item.name, value: item.id, parentId: item.parentId })));
        setContinents(props.data.continents.map((item) => ({ label: item.name, value: item.id })));

        setTypes(props.data.attractionTypes.map((item) => ({ label: item.name, value: item.id })));
    }, [props.data]);

    useEffect(() => {
        setCountryVal("none");
        setRegionVal("none");
        setCityVal("none");

        const c = baseCountries.filter((item) => item.parentId == continentVal);
        setCountries(c);
    }, [continentVal]);


    useEffect(() => {
        setRegionVal("none");
        setCityVal("none");

        const c = baseRegions.filter((item) => item.parentId == countryVal);
        setRegions(c);
    }, [countryVal]);

    useEffect(() => {
        setCityVal("none");

        const c = baseCities.filter((item) => item.parentId == regionVal);
        setCities(c);
    }, [regionVal]);

    useEffect(() => {
        if (!props.defaultValue) return;
        handleDefaultValue();

    }, [props.defaultValue]);

    async function handleDefaultValue() {
        setTypeVal(props.defaultValue.type);
        setContinentVal(props.defaultValue.continent);
        await sleep(100);
        setCountryVal(props.defaultValue.country);
        await sleep(100);
        setRegionVal(props.defaultValue.region);
        await sleep(100);
        setCityVal(props.defaultValue.city);
    }

    return <>
        <Select value={typeVal} onChange={setTypeVal} label="Kategoria" options={types} disabled={props.lockTypes} />
        <Select value={continentVal} onChange={setContinentVal} label="Kontynent" options={continents} />
        <Select value={countryVal} onChange={setCountryVal} label="Kraj" options={countries} />
        <Select value={regionVal} onChange={setRegionVal} label="Region" options={regions} />
        <Select value={cityVal} onChange={setCityVal} label="Miasto" options={cities} />
    </>
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
