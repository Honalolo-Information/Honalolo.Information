import { useState } from "react"
import Button from "./Button"

export default function FileUpload() {
    const [files, setFiles] = useState([])

    const handleChange = (event) => {
        const fileList = event.target.files
        const arr = fileList ? Array.from(fileList) : []
        setFiles(arr)
        // tu masz dostęp do plików w JS:
        // console.log(arr) // [File, File, ...]
    }

    const filesCount = files.length

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
