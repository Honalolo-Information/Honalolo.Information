import { Link } from "react-router";
import image from "../assets/home.png";
import Attraction from "../components/Attraction";
import Button from "../components/Button";

export default function HomePage() {
    return <div>
        <div className="relative">
            <img className="w-full h-[calc(100svh-58px)] object-cover brightness-[40%]" src={image} />

            <div className={
                "p-4 w-full absolute left-[50%] top-[50%] translate-[-50%] text-white sm:text-center "
                + "flex flex-col gap-4 items-start sm:items-center"
            }>
                <h1 className="font-medium text-[42px] md:text-[48px] lg:text-[64px]"> Odkryj uroki twojego regionu </h1>
                <p className="text-[14px] sm:text-[16px]">Poznaj najciekawsze miejsca, szlaki i wydarzenia w okolicy. Znajdziesz tu praktyczne informacje o atrakcjach, restauracjach i możliwościach aktywnego wypoczynku — wszystko, czego potrzebujesz, by zaplanować udaną podróż.</p>

                <div className="flex gap-2">
                    <Link to="/register">
                        <Button>Dołącz do społeczności</Button>
                    </Link>
                </div>
            </div>
        </div>

    </div>
}