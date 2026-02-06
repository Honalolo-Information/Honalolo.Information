import FeatherIcon from "feather-icons-react";
import image from "../assets/register.jpg";
import Badge from "./Badge";
import Button from "./Button";
import { Link } from "react-router";
import config from "../api/config.js";

export default function Attraction(props) {
    if (!props.data) return null;
    if (!props.data.mainImage) return null;

    return <Link to={`/attraction/${props.data.id}`} className="rounded-[var(--rounded)] bg-white border-1 border-[var(--border-color)] shadow-sm hover:shadow-md
        flex flex-col overflow-hidden ">
        <div className="relative border-b-1 border-[var(--border-color)] w-full">
            <img className="w-full aspect-[16/9] rounded-t-[var(--rounded)] object-cover" src={config.url + props.data.mainImage} />

            <Badge className="!border-0 absolute left-2 top-2 bg-[var(--lgreen)]">{props.data.typeName}</Badge>
        </div>

        <div className="p-3 flex flex-col gap-1 items-start">
            <div className="w-full flex justify-start gap-3 items-center">
                <Point
                    className="text-[#333]"
                    icon="map-pin"
                    label={props.data.cityName}
                />
                {
                    props.data.typeName == "Event" ?
                        <>
                            <Point
                                className="text-[#333]"
                                icon="calendar"
                                label={
                                    (new Date(props.data.eventStartDate).toLocaleDateString())
                                    + " - " +
                                    (new Date(props.data.eventEndDate).toLocaleDateString())
                                }
                            />
                        </>

                        : null
                }

                {
                    props.data.typeName == "Trail" ?
                        <>
                            <Point
                                className="text-[#333]"
                                icon="life-buoy"
                                label={props.data.trailDifficulty}

                            />
                        </>

                        : null
                }

                {
                    props.data.typeName == "Restaurant" && props.data.foodType ?
                        <>
                            <Point
                                className="text-[#333]"
                                icon="disc"
                                label={props.data.foodType}

                            />
                        </>

                        : null
                }

                {/* <Point
                    className="text-[#333]"
                    icon="watch"
                    label="5 godzin"
                />
                <Point
                    className="text-[#333]"
                    icon="life-buoy"
                    label="Trudny"
                /> */}
            </div>

            <h3 className="mt-1 text-[24px]">{props.data.title}</h3>
        </div>
    </Link>
}

export function Point(props) {
    return <div className={`flex items-center gap-1.5 ${props.className}`}>
        <FeatherIcon icon={props.icon} size={16} />
        <p className="text-[12px] whitespace-pre-line">{props.label}</p>
    </div>

}