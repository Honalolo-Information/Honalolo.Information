export default function Badge(props) {
    return <h6 className={`px-2 py-0.5 font-semibold text-[13px] border-1 rounded-[100em] ${props.className}`}>{props.children}</h6>
}