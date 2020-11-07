import React, { useEffect, useState } from 'react';
import {useHistory} from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import { getTrackers } from '../Api.js'
import Container from '@material-ui/core/Container';
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';

import TrackerPreview from '../Components/Tracker/TrackerPreview';
import LinkButton from '../Components/Common/LinkButton'

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
        setTrackersList(trackers);
    }
    useEffect(() => {
        fetchTrackersList();
    }, []);

    return (
        <Container component="main" spacing="9">
            <LinkButton url="/" text="Go Back!"/>
            <Grid container className={classes.root} spacing={2}>
                {
                    trackersList.map((tracker) => <TrackerPreview {...tracker} />)
                }
            </Grid>
        </Container>
    );
}