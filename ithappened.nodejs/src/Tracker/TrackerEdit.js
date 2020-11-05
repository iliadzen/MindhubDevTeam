import React, { useEffect, useState } from 'react';
import { getTracker } from '../Api.js'
import { useParams } from "react-router-dom";

import Container from '@material-ui/core/Container';
import { makeStyles } from '@material-ui/core/styles';

import TrackerEditForm from '../Components/Tracker/TrackerEditForm';
import { editTracker } from '../Api'

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        marginTop: "6vh",
    },
}));

export default function TrackerEdit() {
    const classes = useStyles();
    const {trackerId} = useParams();
    const [tracker, setTracker] = useState({});

    const fetchTracker = async (trackerId) => {
        const trackerWait = await getTracker(trackerId);
        console.log(trackerWait.customizations);
        setTracker(trackerWait);
    }

    const trackerEdit = (trackerName, trackerId) => {
        editTracker(trackerName, trackerId)
    }
    useEffect(() => {
        fetchTracker(trackerId);
    }, []);

    return (
        <Container component="main" maxWidth="xs" className={classes.root}>
            <TrackerEditForm tracker={tracker} onSubmit={editTracker}/>
        </Container>
    );
}