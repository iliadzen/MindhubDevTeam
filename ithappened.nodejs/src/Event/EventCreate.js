import EventCreateForm from '../Components/Event/EventCreateForm';
import React, { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import LinkButton from '../Components/Common/LinkButton';
import {getTracker} from '../Api';
import {createEvent} from '../Api';


const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        marginTop: "6vh",
    },
}));

const EventCreate = () => {
    const history = useHistory();
    const classes = useStyles();
    const {trackerId} = useParams();
    const [tracker, setTracker] = useState({
        id: "Loading...",
        title: "Loading...",
        customizations: [
        ]
    });

    const fetchTracker = async (trackerId) => {
        const trackerWait = await getTracker(trackerId);
        setTracker(trackerWait);
    }

    useEffect(() => {
        fetchTracker(trackerId);
    }, []);
    
    const handleCreate = async(tracker, eventTitle, eventCustomizations) => {
        let request = {
            title: eventTitle,
            customizations: eventCustomizations
        }
        console.log(request);
        await eventCustomizations.Photo.DataUrl;
        await createEvent(tracker.id, request);
        history.push(`/trackers/${trackerId}`);

    }
    
    return (
        <Container component="main" maxWidth="xs">
            <LinkButton url={`/trackers/${trackerId}`} text="Go Back!" />
            <EventCreateForm className={classes.root} onSubmit={handleCreate} tracker={tracker}/>
        </Container>
    )
}

export default EventCreate