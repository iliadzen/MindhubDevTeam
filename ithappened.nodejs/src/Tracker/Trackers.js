import React, { useEffect, useState } from 'react';
import {useHistory} from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import { getTrackers } from '../Api.js'
import Container from '@material-ui/core/Container';
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';

import Tracker from '../Components/Tracker/TrackerPreview';

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        marginTop: "6vh",
    },
}));

export default function TrackersList() {
    const history = useHistory();
    const classes = useStyles();

    const [trackersList, setTrackersList] = useState([]);

    const fetchTrackersList = async () => {
        const trackers = await getTrackers();
        console.log(trackers);
        setTrackersList(trackers);
    }
    useEffect(() => {
        fetchTrackersList();
    }, []);

    return (
        <Container component="main" spacing="9">
            <Button
                style={{marginTop: "10px",}}
                variant="contained"
                color="primary"
                className={classes.submit}
                onClick={() => {
                    history.push(`/`);
                }}
            >
                Go Back
            </Button>
            <Grid container className={classes.root} spacing={2}>
                {
                    trackersList.map((tracker) => <Tracker {...tracker} />)
                }
            </Grid>
        </Container>
    );
}