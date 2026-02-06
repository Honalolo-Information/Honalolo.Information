import { useContext, useEffect, useState } from "react";
import image from "../assets/register.jpg";
import Attraction, { Point } from "../components/Attraction";
import { useLocation, useParams } from "react-router";
import AuthContext from "../contexts/AuthContext";
import getAttraction from "../api/requests/getAttraction";
import FeatherIcon from "feather-icons-react";
import config from "../api/config.js";
import getRegion from "../api/requests/getRegion.js";

export default function AttractionPage() {
    const { id } = useParams();

    const auth = useContext(AuthContext)
    const [data, setData] = useState(false);
    const [more, setMore] = useState([]);
    const [region, setRegion] = useState("");

    useEffect(() => {
        handleLoad();
    }, [auth.value]);

    useEffect(() => {
        // console.log("ELO"); 
        handleLoad();
    }, [id]);

    async function handleLoad() {
        const r = await getAttraction(auth.value, id);


        let m = await getAttraction(auth.value, "");
        m = m.filter(item => item.id != id);

        // Shuffle function (Fisher-Yates)
        const shuffle = (array) => {
            const shuffled = [...array];  // Copy to avoid mutating original
            let currentIndex = shuffled.length;
            while (currentIndex != 0) {
                let randomIndex = Math.floor(Math.random() * currentIndex);
                currentIndex--;
                [shuffled[currentIndex], shuffled[randomIndex]] =
                    [shuffled[randomIndex], shuffled[currentIndex]];
            }
            return shuffled;
        };

        m = shuffle(m).slice(0, 4);
        setMore(m);



        const reg = await getRegion();
        setData(r);
        const continent = reg.continents.find((item) => item.id == r.continentId).name;
        const country = reg.countries.find((item) => item.id == r.countryId).name;
        const regname = reg.regions.find((item) => item.id == r.regionId).name;
        const city = reg.cities.find((item) => item.id == r.cityId).name;

        const address = continent + ", " + country + ", " + regname + ", " + city;
        setRegion(address);

    }

    if (!data) return null;

    return <div className="mx-auto p-4 md:p-8 flex flex-col gap-4">
        <div className="grid grid-cols-[1fr] md:grid-cols-[3fr_1fr] gap-4">
            <Gallery images={data.images} />

            <div className="border-1 border-[var(--border-color)] rounded-[var(--rounded)] bg-white p-4 flex flex-col xs:grid grid-cols-[2fr_1fr] md:flex md:flex-col gap-4">
                <div className="rounded-[var(--rounded)]">
                    <Point
                        className="text-[#333]"
                        icon="map-pin"
                        label={region}
                    />
                    <h1 className="mt-2 font-medium text-[24px] md:text-[36px] ">{data.title}</h1>

                    <p className="mt-2 !max-w-[60ch]">{data.description}</p>
                </div>

                <div className="rounded-[var(--rounded)] flex flex-col gap-3">
                    <h3 className="font-medium text-[24px] md:text-[32px]">Szczegóły</h3>

                    <div className="flex flex-col gap-2">
                        <Point icon="square" label={data.typeName} />
                        {data.price != 0 ?
                            <Point icon="dollar-sign" label={data.price.toFixed(2) + " zł"} /> : null}


                        {data.languages && data.languages.length > 0 && data.languages[0].trim() != "" ? <Point icon="globe" label={data.languages.join(", ")} /> : null}
                        {data.openingHours && data.openingHours.length > 0 && data.openingHours[0].trim() != "" ? <Point icon="clock" label={data.openingHours.join("\n")} /> : null}

                        {data.trailDetails && data.trailDetails.difficulty ?
                            <Point
                                icon="anchor"
                                label={data.trailDetails.difficulty + " - Dystans " + data.trailDetails.distanceMeters + " m"}
                            /> : null}

                        {data.eventDetails && data.eventDetails.startDate ?
                            <Point
                                icon="watch"
                                label={(new Date(data.eventDetails.startDate)).toLocaleString() + " - " + (new Date(data.eventDetails.endDate)).toLocaleString()}
                            /> : null}

                        {data.hotelDetails && data.hotelDetails.amenities ?
                            <Point
                                icon="award"
                                label={data.hotelDetails.amenities}
                            /> : null}

                        {data.foodDetails && data.foodDetails.foodType ?
                            <Point
                                icon="tag"
                                label={data.foodDetails.foodType}
                            /> : null}

                    </div>
                </div>

            </div>
        </div>

        <div className="mt-7 flex flex-col gap-4">
            <h2 className="font-medium text-[24px] md:text-[32px]">Zobacz więcej</h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-3">
                {more.map((m) => <Attraction key={m.id} data={m} />)}
            </div>
        </div>
    </div>
}

function Gallery(props) {
    const [currentImage, setCurrentImage] = useState(0);
    const images = props.images || [];  // Unikaj błędów jeśli brak images

    const goPrev = () => {
        setCurrentImage((prev) => (prev > 0 ? prev - 1 : images.length - 1));
    };

    const goNext = () => {
        setCurrentImage((prev) => (prev < images.length - 1 ? prev + 1 : 0));
    };

    return (
        <div className="grid max-h-[80vh] overflow-hidden relative rounded-[var(--rounded)] border-1 border-[var(--border-color)]">
            <img
                src={config.url + images[currentImage]}
                className="w-full object-cover aspect-[16/9] flex "
                alt={`Zdjęcie ${currentImage + 1}`}
            />

            {props.images.length > 1 ?
                <>
                    <div onClick={goPrev} className="absolute left-[1rem] top-[50%] translate-y-[-50%] bg-white rounded-[100%] p-1 flex items-center justify-center">
                        <FeatherIcon icon="chevron-left" className="translate-x-[-1px]" />
                    </div>
                    <div onClick={goNext} className="absolute right-[1rem] top-[50%] translate-y-[-50%] bg-white rounded-[100%] p-1 flex items-center justify-center">
                        <FeatherIcon icon="chevron-right" className="translate-x-[1px]" />
                    </div>
                </> : null}
        </div>
    );
}
