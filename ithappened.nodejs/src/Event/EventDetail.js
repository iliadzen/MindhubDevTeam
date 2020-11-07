import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import Container from '@material-ui/core/Container';
import Grid from '@material-ui/core/Grid';

import EventCard from '../Components/Event/EventCard';
import LinkButton from '../Components/Common/LinkButton';
import { getEvent } from '../Api.js';

import EventPreview from '../Components/Event/EventPreview';


export default function EventDetail() {
    const history = useHistory();
    const {trackerId, eventId} = useParams();
    const [event, setEvent] = useState({
        title: "LOADING...",
        customizations: []
    });

    const fetchEvent = async (trackerId, eventId) => {
        const eventWait = await getEvent(trackerId, eventId);
        setEvent(eventWait);
    }
    useEffect(() => {
        fetchEvent(trackerId, eventId);
    }, []);

    return (
        <Container component="main" maxWidth="xs">
            <LinkButton url={`/trackers/${trackerId}`} text="Go Back!" />
            <EventCard {...event} trackerId={trackerId}/>
        </Container>
    );
}