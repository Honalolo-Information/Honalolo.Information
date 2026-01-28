import FeatherIcon from "feather-icons-react";
import image from "../assets/register.jpg";
import Badge from "./Badge";
import Button from "./Button";
import { Link } from "react-router";

export default function Attraction() {
    return <Link to="/attraction/elo" className="rounded-[var(--rounded)] bg-white border-1">
        <div className="relative border-b-1">
            <img className="w-full aspect-[16/9] rounded-t-[var(--rounded)] object-cover" src={image} />

            <Badge className="!border-0 absolute left-2 top-2 bg-[var(--lgreen)]">SZLAK</Badge>
        </div>

        <div className="p-3 flex flex-col gap-1 items-start">
            <div className="w-full flex justify-start gap-3 items-center">
                <Point
                    className="text-[#333]"
                    icon="map-pin"
                    label="Chłopia, Chłopowice"
                />
                <Point icon="watch" label="5 godzin" />
                <Point icon="life-buoy" label="Trudny" />
            </div>

            <h3 className="mt-1 text-[24px]">Nazwa atrakcji</h3>
        </div>
    </Link>
}

export function Point(props) {
    return <div className={`flex gap-2 ${props.className}`}>
        <FeatherIcon icon={props.icon} size={20} />
        <p className="text-[14px] whitespace-pre-line">{props.label}</p>
    </div>

}