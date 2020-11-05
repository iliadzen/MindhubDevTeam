import React, { useState } from 'react';
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

const TrackerCard = ({tracker, onSubmit}) => {
    const {id, title, creationDate, modificationDate, customizations} = tracker;
    const history = useHistory();
    const classes = useStyles();
    const [trackerName, setTrackerName] = useState("");
    return (
        <Card className={classes.card}>
            <CardContent>
            <TextField
                placeholder={`Name: ${title}`}
                value={trackerName}
                onChange={(e) => setTrackerName(e.target.value)}
            />
            </CardContent>
            <CardActions>
            <Button
                fullWidth
                variant="contained"
                color="primary"
                className={classes.submit}
                onClick={() => {
                    onSubmit(trackerName, id);
                    history.push(`/trackers/${id}`);
                }}
            >
                Submit
            </Button>
            </CardActions>
        </Card>
    );
}

export default TrackerCard;