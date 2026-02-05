import { useEffect } from "react";

export default function Select(props) {
    useEffect(() => {
        if (props.onChange) {
            props.onChange(props.options[0]?.value || "none");
        }
    }, [props.options]);


    return <div className="flex flex-col gap-0.5 w-full">
        <label>{props.label}</label>
        <select name={props.name} value={props.value} onChange={handleChange} className="px-3 py-2 bg-[var(--mgreen)] text-[14px] border-1 border-[#000] rounded-[0px] w-full">
            <option value={"none"}>Wybierz...</option>
            {props.options.map((item, index) => {
                return <option key={index} value={item.value}>
                    {item.label}
                </option>
            })}
        </select>
    </div>

    function handleChange(e) {
        props.onChange(e.target.value);
    }
}