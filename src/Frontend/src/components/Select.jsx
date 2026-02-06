import { useEffect } from "react";

export default function Select(props) {
    // useEffect(() => {
    //     if (props.onChange) {
    //         props.onChange(props.options[0]?.value || "none");
    //     }
    // }, [props.options]);
    return <div className="flex flex-col w-full">
        <label className="input-label">{props.label}</label>
        <select name={props.name} value={props.value} defaultValue={props.defaultValue} onChange={handleChange} disabled={props.disabled}>
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