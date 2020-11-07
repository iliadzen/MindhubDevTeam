import React from 'react';

import { makeStyles } from '@material-ui/core/styles';
import { TextField } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
    card: {
        margin: "5px 20px 5px 20px",
    },
}));

const ScaleForm = ({handler, value}) => {
    return (
        <div>
            <h3>Add scale:</h3>
            <TextField
            type="number"
            name="Scale"
            variant="outlined"
            fullWidth
            label="Scale"
            value={value}
            onChange={(e) => handler(parseFloat(e.target.value))}
            placeholder={"Scale"}
            />
        </div>
    );
}
export default ScaleForm;