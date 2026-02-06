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
import { useNavigate } from "react-router";

export default function CreateAttraction() {
    const [data, setData] = useState(false);
    const [locationVal, setLocationVal] = useState({});
    const [images, setImages] = useState([]);
    const navigate = useNavigate();

    const auth = useContext(AuthContext);

    async function handleLoad() {
        try {
            const r = await getRegion();
            setData(r);
        } catch (error) {
            alert("Sorki coś poszło nie tak");
        }
    }

    useEffect(() => {
        handleLoad();
    }, []);

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


            const r = await createAttraction(auth.value, obj);
            const id = r.id;

            // Upload images
            const imagesForm = new FormData();
            images.forEach((file) => {
                imagesForm.append("files", file);
            });

            await uploadImage(auth.value, id, imagesForm);
            navigate(`/attraction/${id}`);


        } catch (error) {
            console.error(error);
            alert("Sorki, coś poszło nie tak");
            // alert(error);
        }

    }

    return <form onSubmit={handleSubmit} className="flex items-center justify-center p-4 py-8">
        <div className="max-w-[600px] w-full flex flex-col gap-4">
            <h1>Nowa atrakcja</h1>

            <Input name="Title" label="Nazwa atrakcji" />
            <Input name="Description" label="Opis" type="textarea" />

            <LocationSelector
                data={data}
                value={locationVal}
                onChange={setLocationVal}
            />

            <h2 className="mt-[32px] text-[32px]">Opcjonalne</h2>
            <Input name="Price" type="float" label="Koszt atrakcji (opcjonalnie)" />
            <Input name="Languages" label="Języki (opcjonalnie)" />
            {/* <Input name="Duration" label="Czas trwania (opcjonalnie)" /> */}

            <Input name="OpeningHours" label="Godziny otwarcia (opcjonalnie)" type="textarea" />


            {locationVal.type == 51 ? <TrailInputs data={data} /> : null}
            {locationVal.type == 53 ? <FoodInputs /> : null}
            {locationVal.type == 52 ? <HotelInputs /> : null}
            {locationVal.type == 50 ? <EventInputs /> : null}

            <h2 className="mt-[32px] text-[32px]">Zdjęcia</h2>
            <FileUpload value={images} onChange={setImages} />

            <Button>Utwórz atrakcje</Button>
        </div>
    </form>
}

function TrailInputs(props) {
    const [levels, setLevels] = useState([]);

    useEffect(() => {
        if (!props.data) return
        setLevels(props.data.difficultyLevels.map((item) => ({ label: item.name, value: item.id })));
    }, [props.data]);

    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły szlaku</h2>
        <Select name="DifficultyLevelId" label="Poziom trudności" options={levels} />
        <Input name="DistanceMeters" label="Dystans (m)" type="number" />
    </>
}

function FoodInputs() {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły jedzenia</h2>
        <Input name="FoodType" label="Rodzaj jedzenia" />
    </>
}

function HotelInputs() {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły hotelu</h2>
        <Input name="Amenities" label="Udogodnienia" type="textarea" />
    </>
}

function EventInputs() {
    return <>
        <h2 className="mt-[32px] text-[32px]">Szczegóły wydarzenia</h2>
        {/* <Input label="Rodzaj wydarzenia" /> */}
        <Input name="StartDate" label="Data i godzina startu" type="datetime-local" />
        <Input name="EndDate" label="Data i godzina końca" type="datetime-local" />
    </>
}