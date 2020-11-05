import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import Container from '@material-ui/core/Container';

import TrackerCard from '../Components/Tracker/TrackerCard';
import LinkButton from '../Components/Common/LinkButton';
import { getTracker } from '../Api.js';

import EventPreview from '../Components/Event/EventPreview';


export default function TrackerDetail() {
    const history = useHistory();
    const {trackerId} = useParams();
    const [tracker, setTracker] = useState({
        title: "LOADING...",
        customizations: ["Loading..."]
    });

    const fetchTracker = async (trackerId) => {
        const trackerWait = await getTracker(trackerId);
        setTracker(trackerWait);
    }
    useEffect(() => {
        fetchTracker(trackerId);
    }, []);

    return (
        <Container component="main" maxWidth="xs">
            <LinkButton url={`/trackers`} text="Go Back!" />
            <TrackerCard {...tracker} />
        </Container>
    );
}