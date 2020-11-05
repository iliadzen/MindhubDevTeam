import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import { TextField } from '@material-ui/core';
import FormLabel from '@material-ui/core/FormLabel';
import FormControl from '@material-ui/core/FormControl';
import FormGroup from '@material-ui/core/FormGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';


const useStyles = makeStyles((theme) => ({
    card: {
        margin: "5px 20px 5px 20px",
    },
}));

const TrackerCreateForm = ({onSubmit}) => {
    const history = useHistory();
    const classes = useStyles();
    const [trackerName, setTrackerName] = useState("");
    const [trackerCustomizations, setTtrackerCustomizations] = useState({
        comment: false,
        rating: false,
        scale: false,
        geotag: false,
        photo: false
    });

    const handleCheckboxChange = (event) => {
        setTtrackerCustomizations({...trackerCustomizations, [event.target.name]: event.target.checked})
    };

    const handleCreate = (trackerName, trackerCustomizations) => {
        let customizationsList = []
        if (trackerCustomizations.comment) customizationsList.push(2);
        if (trackerCustomizations.rating) customizationsList.push(4);
        if (trackerCustomizations.geotag) customizationsList.push(1);
        if (trackerCustomizations.scale) customizationsList.push(3);
        if (trackerCustomizations.photo) customizationsList.push(5);
        onSubmit(trackerName, `[${customizationsList.toString()}]`);
    }
    return (
        <Card className={classes.card}>
            <CardContent>
                <TextField
                    name="Title"
                    variant="outlined"
                    fullWidth
                    label="Title"
                    value={trackerName}
                    onChange={(e) => setTrackerName(e.target.value)}
                    placeholder={"Tracker Name"}

                />
                <hr></hr>
                <FormControl component="fieldset" className={classes.formControl}>
                    <FormLabel>Select Customizations:</FormLabel>
                    <FormGroup>
                        <FormControlLabel
                            control={<Checkbox checked={trackerCustomizations.comment} onChange={handleCheckboxChange} name="comment" />}
                            label="Comment"
                        />
                        <FormControlLabel
                            control={<Checkbox checked={trackerCustomizations.rating} onChange={handleCheckboxChange} name="rating" />}
                            label="Rating"
                        />
                        <FormControlLabel
                            control={<Checkbox checked={trackerCustomizations.scale} onChange={handleCheckboxChange} name="scale" />}
                            label="Scale"
                        />
                        <FormControlLabel
                            control={<Checkbox checked={trackerCustomizations.geotag} onChange={handleCheckboxChange} name="geotag" />}
                            label="Geotag"
                        />
                        <FormControlLabel
                            control={<Checkbox checked={trackerCustomizations.photo} onChange={handleCheckboxChange} name="photo" />}
                            label="Photo"
                        />
                    </FormGroup>
                </FormControl>
            </CardContent>
            <CardActions>
            <Button
                fullWidth
                variant="contained"
                color="primary"
                className={classes.submit}
                onClick={() => {
                    handleCreate(trackerName, trackerCustomizations);
                }}
            >
                Submit
            </Button>
            </CardActions>
        </Card>
    );
}

export default TrackerCreateForm;