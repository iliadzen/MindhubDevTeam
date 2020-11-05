import React, { useEffect, useState } from 'react';
import { getTracker } from '../Api.js'
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import Container from '@material-ui/core/Container';
import { makeStyles } from '@material-ui/core/styles';

import TrackerEditForm from '../Components/Tracker/TrackerEditForm';
import { editTracker } from '../Api'
import LinkButton from '../Components/Common/LinkButton'

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        marginTop: "6vh",
    },
}));

export default function TrackerEdit() {
    const history = useHistory();
    const classes = useStyles();
    const {trackerId} = useParams();
    const [tracker, setTracker] = useState({});

    const handleEdit = async (trackerName, trackerId) => {
        await editTracker(trackerName, trackerId);
        history.push(`/trackers/${trackerId}`);
    }

    const fetchTracker = async (trackerId) => {
        const trackerWait = await getTracker(trackerId);
        setTracker(trackerWait);
    }

    useEffect(() => {
        fetchTracker(trackerId);
    }, []);

    return (
        <Container component="main" maxWidth="xs">
            <LinkButton url={`/trackers/${trackerId}`} text="Go Back!" />
            <TrackerEditForm className={classes.root} tracker={tracker} onSubmit={handleEdit}/>
        </Container>
    );
}