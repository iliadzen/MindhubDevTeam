import React, { useEffect, useState } from 'react';
import { editEvent, getEvent } from '../Api.js'
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import Container from '@material-ui/core/Container';
import { makeStyles } from '@material-ui/core/styles';

import EventEditForm from '../Components/Event/EventEditForm';
import LinkButton from '../Components/Common/LinkButton'

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
        marginTop: "6vh",
    },
}));

export default function EventEdit() {
    const history = useHistory();
    const classes = useStyles();
    const {trackerId, eventId} = useParams();
    const [event, setEvent] = useState({});

    const handleEdit = async (trackerId, eventId, eventTitle) => {
        await editEvent(trackerId, eventId, eventTitle);
        history.push(`/trackers/${trackerId}/events/${eventId}`);
    }

    const fetchEvent = async (trackerId, eventId) => {
        const eventWait = await getEvent(trackerId, eventId);
        setEvent(eventWait);
    }

    useEffect(() => {
        fetchEvent(trackerId, eventId);
    }, []);

    return (
        <Container component="main" maxWidth="xs">
            <LinkButton url={`/trackers/${trackerId}/events/${eventId}`} text="Go Back!" />
            <EventEditForm className={classes.root} {...event} onSubmit={handleEdit} trackerId={trackerId}/>
        </Container>
    );
}