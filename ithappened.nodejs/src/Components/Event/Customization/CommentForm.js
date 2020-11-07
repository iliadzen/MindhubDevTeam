import React from 'react';

import { makeStyles } from '@material-ui/core/styles';
import { TextField } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
    card: {
        margin: "5px 20px 5px 20px",
    },
}));

const CommentForm = ({handler, value}) => {
    return (
        <div>
            <h3>Add comment:</h3>
            <TextField
            name="Comment"
            variant="outlined"
            fullWidth
            label="Comment"
            value={value}
            onChange={(e) => handler(e.target.value)}
            placeholder={"Comment"}
            />
        </div>
    );
}
export default CommentForm;