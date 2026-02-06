import { useContext, useEffect, useState } from "react";
import getRegion from "../api/requests/getRegion";
import Select from "../components/Select";
import Input from "../components/Input";
import Button from "../components/Button";
import getReport from "../api/requests/getReport";
import AuthContext from "../contexts/AuthContext";
import downloadReport from "../api/requests/downloadReport";
import getAllReports from "../api/requests/getAllReports";
import getAttraction from "../api/requests/getAttraction";
import { Link } from "react-router";
import deleteAttraction from "../api/requests/deleteAttraction";

export default function AdminPage() {
    return <div className="p-4 md:p-8 grid gap-4">
        <h1 className="mb-8">Panel administratora</h1>
        <Attractions />
        <Reports />
    </div>
}

function Attractions() {

    const [count, setCount] = useState(0);
    const auth = useContext(AuthContext)

    const [data, setData] = useState([]);

    useEffect(() => {
        handleLoad();
    }, [auth.value, count]);

    async function handleLoad() {
        const r = await getAttraction(auth.value, "");
        console.log(r);

        setData(r);
    }

    async function handleDelete(id) {
        const token = localStorage.getItem("authToken");
        try {
            const r = await deleteAttraction(token, id);
        } catch (err) {

        }
        setCount(count + 1);
    }


    return <>
        <h2>Zarządzanie atrakcjami</h2>
        <div className="w-full border-1 border-[var(--border-color)] rounded-[var(--rounded)] overflow-hidden">
            <table className="w-full ">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>ID Autora</th>
                        <th>Nazwa</th>
                        <th>Type</th>
                        <th>Akcja</th>
                    </tr>
                </thead>

                <tbody>
                    {data.reverse().map((item) => {
                        return <tr key={item.id}>
                            <td>{item.id}</td>
                            <td>{item.authorId}</td>
                            <td>{item.title}</td>
                            <td>{item.typeName}</td>
                            <td>
                                <div className="flex gap-1">
                                    <Link to={`/edit/${item.id}`}>
                                        <Button className="!px-3 !py-1 !text-[14px]" >Edytuj</Button>
                                    </Link>
                                    <Button onClick={() => handleDelete(item.id)} className="!px-3 !py-1 !text-[14px] !bg-[#e60000] !border-[#ff0000]" >Usuń</Button>
                                </div>
                            </td>
                        </tr>
                    })}
                </tbody>
            </table>
        </div >
    </>
}

function Reports() {
    const auth = useContext(AuthContext);
    const [reports, setReports] = useState([]);

    const [count, setCount] = useState(0);

    useEffect(() => {
        handleLoad();
    }, [count]);


    async function handleLoad() {

        const token = localStorage.getItem("authToken");
        const r = await getAllReports(token);
        setReports(r);
    }


    return <>
        <h2 className="mt-8">Raporty</h2>
        <div className="grid lg:grid-cols-[2fr_1fr] gap-8 ">
            <div className="w-full border-1 border-[var(--border-color)] rounded-[var(--rounded)] overflow-hidden">
                <table className="w-full ">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Nazwa raportu</th>
                            <th>Akcja</th>
                        </tr>
                    </thead>

                    <tbody>
                        {reports.map((item) => {
                            return <tr key={item.id}>
                                <td>{item.id}</td>
                                <td>{item.title}</td>
                                <td>
                                    <Button
                                        onClick={() => handleDownload(item.id)}
                                        className="!px-3 !py-1 !text-[14px]"
                                    >Pobierz</Button>
                                </td>
                            </tr>
                        })}
                    </tbody>

                </table>
            </div>
            <Form onSubmit={handleSubmit} />
        </div>
    </>

    async function handleSubmit(e) {
        e.preventDefault();
        const formData = new FormData(e.target);

        var object = {};
        formData.forEach((value, key) => object[key] = value);

        // object["StartDate"] = new Date(object["StartDate"]);
        // object["EndDate"] = new Date(object["EndDate"]);
        object["Type"] = parseInt(object["Type"]);
        object["MinPrice"] = parseFloat(object["MinPrice"]);
        object["MaxPrice"] = parseFloat(object["MaxPrice"]);

        // var json = JSON.stringify(object);

        const r = await getReport(auth.value, object);

        setTimeout(() => {
            setCount(count + 1);
        }, 1000);
    }

    async function handleDownload(id) {
        const blob = await downloadReport(auth.value, id);

        const url = URL.createObjectURL(blob);
        const filename = "raport.pdf";

        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();

        document.body.removeChild(link);
        URL.revokeObjectURL(url);  // Clean up memory
    }
}

function Form(props) {
    const [data, setData] = useState(null);

    const [types, setTypes] = useState([]);
    const [typeVal, setTypeVal] = useState(0);

    const [cities, setCities] = useState([]);
    const [cityVal, setCityVal] = useState(0)

    async function handleLoad() {
        try {
            const r = await getRegion();
            setData(r);

            setCities(r.cities.map((item) => ({ label: item.name, value: item.name })));
            setTypes([
                { label: "W przedziałach kwotowych", value: 0, },
                { label: "W przedziałach datowych", value: 1, },
            ]);
        } catch (error) {
            alert("Sorki coś poszło nie tak");
        }
    }

    useEffect(() => {
        handleLoad();
    }, []);


    return <form onSubmit={props.onSubmit} className="p-4 border-1 border-[var(--border-color)] rounded-[var(--rounded)] flex flex-col gap-4">
        <h2>Generuj raport</h2>
        <Select name="Type" value={typeVal} onChange={setTypeVal} label="Kategoria" options={types} />
        {typeVal == 1 ?
            <>
                <Input name="StartDate" type="date" label="Data rozpoczęcia" />
                <Input name="EndDate" type="date" label="Data zakończenia" />
            </>
            : null}

        {typeVal == 0 ?
            <>
                <Input name="MinPrice" type="number" label="Cena minimalna" />
                <Input name="MaxPrice" type="number" label="Cena Maksymalna" />
            </>
            : null}
        <Select name="CityName" value={cityVal} onChange={setCityVal} label="Miasto" options={cities} />
        <Button className="mt-auto">Generuj</Button>
    </form>
}