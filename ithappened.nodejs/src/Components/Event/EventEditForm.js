import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import { TextField } from '@material-ui/core';


const useStyles = makeStyles((theme) => ({
    card: {
        margin: "5px 20px 5px 20px",
    },
}));

const EventEditForm = ({trackerId, id, title, creationDate, modificationDate, customizations, onSubmit}) => {
    const history = useHistory();
    const classes = useStyles();
    const [eventTitle, setEventTitle] = useState("");
    return (
        <Card className={classes.card}>
            <CardContent>
            <TextField
                name="Title"
                variant="outlined"
                fullWidth
                label="Title"
                value={eventTitle}
                onChange={(e) => setEventTitle(e.target.value)}
                placeholder={`${title}`}
            />
            </CardContent>
            <CardActions>
            <Button
                fullWidth
                variant="contained"
                color="primary"
                className={classes.submit}
                onClick={() => {
                    onSubmit(trackerId, id, eventTitle);
                }}
            >
                Submit
            </Button>
            </CardActions>
        </Card>
    );
}

export default EventEditForm;