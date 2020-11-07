import React from 'react';

import { makeStyles } from '@material-ui/core/styles';
import { TextField } from '@material-ui/core';

const useStyles = makeStyles((theme) => ({
    card: {
        margin: "5px 20px 5px 20px",
    },
}));

const CommentForm = ({value}) => {
    return (
        <div>
            <h4>Your comment:</h4>
            <p>{value}</p>
        </div>
    );
}
export default CommentForm;