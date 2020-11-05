import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import Container from '@material-ui/core/Container';
import Button from '@material-ui/core/Button';

import TrackerCard from '../Components/Tracker/TrackerCard';
import { getTracker } from '../Api.js';


export default function TrackerDetail() {
    const history = useHistory();
    const {trackerId} = useParams();
    const [tracker, setTracker] = useState({});

    const fetchTracker = async (trackerId) => {
        const trackerWait = await getTracker(trackerId);
        console.log(trackerWait.customizations);
        setTracker(trackerWait);
    }
    useEffect(() => {
        fetchTracker(trackerId);
    }, []);

    return (
        <Container component="main" maxWidth="xs">
            <Button
                style={{marginTop: "10px",}}
                variant="contained"
                color="primary"
                onClick={() => {
                    history.push(`/trackers`);
                }}
            >
                Go Back
            </Button>
            <TrackerCard {...tracker} />
        </Container>
    );
}