export default function Input(props) {
    if (props.type === "textarea") {
        return <div className="flex flex-col gap-2 w-full">
            <label>{props.label}</label>
            <textarea name={props.name} value={props.value} onChange={handleChange} className="w-full px-3 py-1 text-[14px] bg-[var(--mgreen)] rounded-[0px] border-1 resize-y h-[150px] " required={props.required} />
        </div>
    }

    return <div className="flex flex-col gap-2 w-full">
        <label>{props.label}</label>
        <input name={props.name} type={props.type} value={props.value} onChange={handleChange} className="w-full px-3 py-1 text-[14px] bg-[var(--mgreen)] rounded-[0px] border-1 " required={props.required} />
    </div>

    function handleChange(e) {
        if (props.onChange) {
            props.onChange(e.target.value);
        }
    }
}