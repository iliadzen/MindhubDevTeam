import React from 'react';

import { makeStyles } from '@material-ui/core/styles';
import { TextField } from '@material-ui/core';

const PhotoForm = ({handler, value}) => {
    const toBinary = file => new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });

    const waitAndSubmit = async (file) => {
        var result = await file;
        handler(result);
    }

    return (
        <div>
            <h3>Add photo:</h3>
            <TextField
                type="file"
                variant="outlined"
                fullWidth
                value={value}
                onChange={(e) => {
                    if (e.target.files.length != 0) {
                        const file = Array.from(e.target.files)[0]
                        waitAndSubmit(toBinary(file))
                    }                    
                }}
            />
        </div>
    );
}
export default PhotoForm;