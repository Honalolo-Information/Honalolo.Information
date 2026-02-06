import { useContext, useEffect, useState } from "react";
import FileUpload from "../components/FileUpload";
import Input from "../components/Input";
import Select from "../components/Select";
import Button from "../components/Button";
import getRegion from "../api/requests/getRegion";
import LocationSelector from "../components/LocationSelector";
import createAttraction from "../api/requests/createAttraction";
import AuthContext from "../contexts/AuthContext";
import uploadImage from "../api/requests/uploadImage";
import getAttraction from "../api/requests/getAttraction";
import { useParams } from "react-router";
import editAttraction from "../api/requests/editAttraction";

export default function EditAttraction() {
    const { id } = useParams();

    const [atData, setAtData] = useState(false);

    const [data, setData] = useState(false);
    const [locationVal, setLocationVal] = useState({});
    const [images, setImages] = useState([]);
    const [defLocationVal, setDefLocation] = useState(false);

    const auth = useContext(AuthContext);

    async function loadAttraction() {
        const token = localStorage.getItem("authToken");
        const r = await getAttraction(token, id);
        setAtData(r);

    }


    async function handleLoad() {
        try {
            const r = await getRegion();
            setData(r);
        } catch (error) {
            alert("Sorki coś poszło nie tak");
        }
    }

    useEffect(() => {
        if (!data || !atData) return;

        const typeId = data.attractionTypes.find((item) => item.name == atData.typeName).id;

        const loc = {
            type: typeId,
            continent: atData.continentId,
            country: atData.countryId,
            region: atData.regionId,
            city: atData.cityId,
        };

        setDefLocation(loc);



    }, [data, atData])

    useEffect(() => {
        handleLoad();
        loadAttraction();
    }, [auth.value]);

    if (!data || !atData) return null;

    async function handleSubmit(e) {
        try {
            e.preventDefault();
            const formData = new FormData(e.target);
            const obj = Object.fromEntries(formData.entries()); // or just formData

            // Type
            const type = data.attractionTypes.find((item) => item.id == locationVal.type);
            if (!type) {
                throw "Wybierz poprawną kategorię atrakcji";
            }
            obj['TypeName'] = type.name;


            const continent = data.continents.find((item) => item.id == locationVal.continent);
            if (!type) {
                throw "Wybierz poprawny kontynent";
            }
            obj['ContinentName'] = continent.name;


            const country = data.countries.find((item) => item.id == locationVal.country);
            if (!country) {
                throw "Wybierz poprawny kraj";
            }
            obj['CountryName'] = country.name;


            const region = data.regions.find((item) => item.id == locationVal.region);
            if (!region) {
                throw "Wybierz poprawny region";
            }
            obj['RegionName'] = region.name;


            const city = data.cities.find((item) => item.id == locationVal.city);
            if (!city) {
                throw "Wybierz poprawny region";
            }
            obj['CityName'] = city.name;

            obj['Price'] = parseFloat(obj['Price']) || 0;
            obj['Languages'] = obj['Languages'].split(" ");
            obj['OpeningHours'] = obj['OpeningHours'].split("\n");

            obj['TrailDetails'] = {};
            obj['TrailDetails']['DistanceMeters'] = parseInt(obj['DistanceMeters']) || 0;
            obj['TrailDetails']['DifficultyLevelId'] = parseInt(obj['DifficultyLevelId']) || 0;

            obj['EventDetails'] = {};
            obj['EventDetails']['StartDate'] = obj['StartDate'];
            obj['EventDetails']['EndDate'] = obj['EndDate'];

            obj['HotelDetails'] = {};
            obj['HotelDetails']['Amenities'] = obj['Amenities'];

            obj['FoodDetails'] = {};
            obj['FoodDetails']['FoodType'] = obj['FoodType'];


            const r = await editAttraction(auth.value, id, obj);
            // const id = r.id;

            // Upload images
            const imagesForm = new FormData();
            images.forEach((file) => {
                imagesForm.append("files", file);
            });

            await uploadImage(auth.value, id, imagesForm, "PUT");
            window.location.reload();

        } catch (error) {
            console.error(error);
            alert("Sorki, coś poszło nie tak");
            // alert(error);
        }

    }

    return <form onSubmit={handleSubmit} className="flex items-center justify-center p-4 py-8">
        <div className="max-w-[600px] w-full flex flex-col gap-4">
            <h1>Edytuj atrakcje</h1>

            <Input defaultValue={atData.title} name="Title" label="Nazwa atrakcji" />
            <Input defaultValue={atData.description} name="Description" label="Opis" type="textarea" />

            <LocationSelector
                defaultValue={defLocationVal}
                data={data}
                value={locationVal}
                onChange={setLocationVal}
                lockTypes={true}
            />

            <h2 className="mt-[32px] text-[32px]">Opcjonalne</h2>
            <Input defaultValue={atData.price} name="Price" type="number" label="Koszt atrakcji (opcjonalnie)" />
            <Input name="Languages" defaultValue={atData.languages} label="Języki (opcjonalnie)" />
            {/* <Input defaultValue={atData.} label="Czas trwania (opcjonalnie)" /> */}

            <Input name="OpeningHours" defaultValue={atData.openingHours} label="Godziny otwarcia (opcjonalnie)" type="textarea" />


            {locationVal.type == 51 ? <TrailInputs defaultValues={atData} data={data} /> : null}
            {locationVal.type == 53 ? <FoodInputs defaultValues={atData} /> : null}
            {locationVal.type == 52 ? <HotelInputs defaultValues={atData} /> : null}
            {locationVal.type == 50 ? <EventInputs defaultValues={atData} /> : null}

            <h2 className="mt-[32px] text-[32px]">Zdjęcia</h2>
            <FileUpload value={images} onChange={setImages} />

            <Button>Edytuj atrakcje</Button>
        </div>
    </form>
}

function TrailInputs(props) {
    const [levels, setLevels] = useState([]);
    const [defVal, setDefVal] = useState(false);

    const [selValue, setSelValue] = useState(0);

    useEffect(() => {
        if (!props.data) return
        setLevels(props.data.difficultyLevels.map((item) => ({ label: item.name, value: item.id })));
    }, [props.data]);

    useEffect(() => {
       
        if (!props.data || !props.defaultValues || !props.defaultValues.trailDetails || !props.defaultValues.trailDetails.difficulty) return;
        const id = props.data.difficultyLevels.find((item) => item.name == props.defaultValues.trailDetails.difficulty).id;
        // setVal(id);
        setDefVal(id);
        setSelValue(id);
    }, [props.data, props.defaultValues]);

    async function setVal(val) {
        // await sleep(100);
    }

    // if (defVal === false) return null;

    const defDistance = props.defaultValues.trailDetails && props.defaultValues.trailDetails.distanceMeters ? props.defaultValues.trailDetails.distanceMeters : "";

    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły szlaku</h2>
        <Select 
        name="DifficultyLevelId" 
        label="Poziom trudności" 
        options={levels} 
        value={selValue}
        onChange={setSelValue}
        />
        <Input defaultValue={defDistance} name="DistanceMeters" label="Dystans (m)" type="number" />
    </>
}

function FoodInputs(props) {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły jedzenia</h2>
        <Input defaultValue={props.defaultValues.foodDetails.foodType} name="FoodType" label="Rodzaj jedzenia" />
    </>
}

function HotelInputs(props) {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły hotelu</h2>
        <Input
            defaultValue={
                props.defaultValues.hotelDetails &&
                    props.defaultValues.hotelDetails.amenities ?
                    props.defaultValues.hotelDetails.amenities : ""
            }
            name="Amenities"
            label="Udogodnienia"
            type="textarea"
        />
    </>
}

function EventInputs(props) {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły wydarzenia</h2>
        {/* <Input label="Rodzaj wydarzenia" /> */}
        <Input
            defaultValue={
                props.defaultValues.eventDetails && props.defaultValues.eventDetails.startDate ?
                    props.defaultValues.eventDetails.startDate : ""
            }
            name="StartDate"
            label="Data i godzina startu"
            type="datetime-local"
        />
        <Input
            defaultValue={
                props.defaultValues.eventDetails && props.defaultValues.eventDetails.endDate ?
                    props.defaultValues.eventDetails.endDate : ""
            }
            name="EndDate"
            label="Data i godzina końca"
            type="datetime-local"
        />
    </>
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}