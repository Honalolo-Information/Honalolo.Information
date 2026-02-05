import { useContext, useEffect, useState } from "react";
import image from "../assets/register.jpg";
import Attraction, { Point } from "../components/Attraction";
import { useParams } from "react-router";
import AuthContext from "../contexts/AuthContext";
import getAttraction from "../../api/requests/getAttraction";

export default function AttractionPage() {
    const {id} = useParams();

    const auth = useContext(AuthContext)
    const [data, setData] = useState([]);
    const [more, setMore] = useState([]);

    useEffect(() => {
        handleLoad();
    }, [auth.value]);

    async function handleLoad() {
        const r = await getAttraction(auth.value, id);
        console.log(r);
        setData(r);

        let m = await getAttraction(auth.value, "");
        m = m.filter(item => item.id != id);
        m = m.slice(0,4);
        setMore(m);
    }


    return <div className="mx-auto p-4 md:p-8 flex flex-col gap-4">
        <Gallery />

        <div className="border-1 rounded-[var(--rounded)] bg-white p-4 mt-4 grid md:grid-cols-[2fr_1fr] gap-4">
            <div className="rounded-[var(--rounded)]">
                <Point
                    className="text-[#333]"
                    icon="map-pin"
                    label="Chłopia, Chłopowice"
                />
                <h1 className="mt-2 font-medium text-[24px] md:text-[36px] ">{data.title}</h1>

                <p className="mt-2 !max-w-[60ch]">{data.description}</p>
            </div>

            <div className="rounded-[var(--rounded)] flex flex-col gap-3">
                <h2 className="font-medium text-[24px] md:text-[32px]">Szczegóły</h2>

                <div className="flex flex-col gap-2">
                    <Point icon="watch" label="Średni czas trwania: 5 godzin" />
                    <Point icon="life-buoy" label="Poziom trudności: Trudny" />
                    <Point icon="map" label="Dystans: 5400 m" />

                    <Point
                        icon="clock"
                        label={`Pn-Pt: 08:00 - 16:00
                        Sob: 10:00 - 14:00`}
                    />

                    <Point
                        icon="box"
                        label={`Kuchnia meksykańska`}
                    />

                    <Point
                        icon="globe"
                        label="Angielski, Polski, Murckowski"
                    />
                </div>
            </div>

        </div>


        <div className="mt-7 flex flex-col gap-4">
            <h2 className="font-medium text-[24px] md:text-[32px]">Zobacz więcej</h2>
            <div className="m-auto grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-3">
                {more.map((m) => <Attraction key={m.id} data={m} />)}
            </div>
        </div>
    </div>
}

function Gallery() {
    const [currentImage, setCurrentImage] = useState(image);
    return <div className="grid md:grid-cols-[1fr_100px] lg:grid-cols-[1fr_150px] gap-2 md:h-[80vh] overflow-hidden ">
        <img src={currentImage} className="rounded-[var(--rounded)] w-full md:h-[80vh] object-cover aspect-[16/9] flex border-1" />

        <div className="flex md:flex-col gap-2 h-full overflow-x-auto md:overflow-y-auto">
            {[1, 2, 3, 4, 5, 6, 7, 8].map((i) => {
                return <img key={i} src={image} onClick={() => setCurrentImage(image)} className="w-[80px] md:w-[100px] lg:w-[150px] aspect-[1] object-cover rounded-[var(--rounded)] border-1" />
            })}
        </div>
    </div>
}