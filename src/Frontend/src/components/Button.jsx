export default function Button(props) {
    return <button {...props} className={`text-black rounded-[0px] py-1 pt-1.5 px-4 bg-[var(--accent)] cursor-pointer font-semibold border-1 ${props.className}`} />
}