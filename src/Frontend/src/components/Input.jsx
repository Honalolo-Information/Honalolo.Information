export default function Input(props) {
    if (props.type === "textarea") {
        return <div className="flex flex-col w-full">
            <label className="input-label">{props.label}</label>
            <textarea name={props.name} value={props.value} onChange={handleChange} required={props.required} defaultValue={props.defaultValue} />
        </div>
    }

    return <div className="flex flex-col w-full">
        <label className="input-label">{props.label}</label>
        <input name={props.name} type={props.type} value={props.value} onChange={handleChange} required={props.required} defaultValue={props.defaultValue} />
    </div>

    function handleChange(e) {
        if (props.onChange) {
            props.onChange(e.target.value);
        }
    }
}