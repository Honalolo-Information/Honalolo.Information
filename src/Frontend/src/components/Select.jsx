export default function Select(props) {
    return <div className="flex flex-col gap-0.5 w-full">
        <label>{props.label}</label>
        <select value={props.value} onChange={handleChange} className="px-3 py-2 bg-[var(--mgreen)] text-[14px] border-1 border-[#000] rounded-[0px] w-full">
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