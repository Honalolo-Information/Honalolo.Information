import Button from "./Button"

export default function FileUpload(props) {

    const handleChange = (event) => {
        const fileList = event.target.files
        const arr = fileList ? Array.from(fileList) : []
        props.onChange(arr)
    }

    return (
        <label>
            <input
                type="file"
                multiple
                onChange={handleChange}
                className="file-input file-input-bordered w-full"
            />
        </label>
    )
}
