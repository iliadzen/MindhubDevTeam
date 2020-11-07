import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import Container from '@material-ui/core/Container';
import TextField from '@material-ui/core/TextField';
import Grid from '@material-ui/core/Grid';

import TrackerCard from '../Components/Tracker/TrackerCard';
import LinkButton from '../Components/Common/LinkButton';
import { getTracker, getEvents } from '../Api.js';

import EventPreview from '../Components/Event/EventPreview';
import { Button } from '@material-ui/core';


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

    const [eventsList, setEventsList] = useState([{
        title: "Loading..."
    }]);

    const fetchEventsList = async (trackerId) => {
        let queryString = "?";
        for (const [key, value] of Object.entries(queryList)) {
            if (value != null) queryString+=`${key}=${value}&`
        }
        const events = await getEvents(trackerId, queryString);
        setEventsList(events);
    }
    useEffect(() => {
        fetchEventsList(trackerId);
    }, []);

    const [queryList, setQueryList] = useState({
        period: null,
        ratingMin: null,
        ratingMax: null,
        fromDate: null,
        toDate: null,
        scaleMin: null,
        scaleMax: null,
    });

    return (
        <Container component="main" maxWidth="xs">
            <LinkButton url={`/trackers`} text="Go Back!" />
            <TrackerCard {...tracker} />
            <hr></hr>
            <Button
                style={{margin: "3px"}}
                variant="contained"
                color="primary"
                onClick={() => {
                    setQueryList({
                        period: null,
                        ratingMin: null,
                        ratingMax: null,
                        fromDate: null,
                        toDate: null,
                        scaleMin: null,
                        scaleMax: null,
                    });
                    fetchEventsList(trackerId);
                }}
            >
                Reset Filters - All Time
            </Button>
            <Button
                style={{margin: "3px"}}
                variant="contained"
                color="primary"
                onClick={() => {
                    setQueryList({...queryList, period: "thisWeek"});
                    fetchEventsList(trackerId);
                }}
            >
                This Week
            </Button>
            <TextField
                style={{backgroundColor:"white", borderRadius:"3px", margin: "3px", padding: "3px"}}
                label="From Date"
                type="date"
                defaultValue="2020-11-01"
                onChange={(e) => {
                    setQueryList({...queryList, fromDate: `${e.target.value}`});
                    fetchEventsList(trackerId);
                }}
                InputLabelProps={{
                shrink: true,
                }}
            />
            <TextField
                style={{backgroundColor:"white", borderRadius:"3px", margin: "3px", padding: "3px"}}
                label="To Date"
                type="date"
                defaultValue="2020-11-01"
                onChange={(e) => {
                    setQueryList({...queryList, toDate: `${e.target.value}`});
                    fetchEventsList(trackerId);
                }}
                InputLabelProps={{
                shrink: true,
                }}
            />
            <hr></hr>
            {tracker.customizations.includes("Rating")
            ? <div><TextField
                style={{backgroundColor:"white", borderRadius:"3px", margin: "3px", padding: "3px", width:"30%"}}
                label="Min Rating"
                type="number"
                InputProps={{ inputProps: { min: 0, max: 5 } }}
                defaultValue="0"
                onChange={(e) => {
                    setQueryList({...queryList, ratingMin: `${e.target.value}`});
                    fetchEventsList(trackerId);
                }}
                InputLabelProps={{
                shrink: true,
                }}
            />
            <TextField
                style={{backgroundColor:"white", borderRadius:"3px", margin: "3px", padding: "3px", width:"30%"}}
                label="Max Rating"
                type="number"
                InputProps={{ inputProps: { min: 0, max: 5 } }}
                defaultValue="5"
                onChange={(e) => {
                    setQueryList({...queryList, ratingMax: `${e.target.value}`});
                    fetchEventsList(trackerId);
                }}
                InputLabelProps={{
                shrink: true,
                }}
            />
            <hr></hr>
            </div>
            : []}
            {tracker.customizations.includes("Scale")
            ? <div><TextField
                style={{backgroundColor:"white", borderRadius:"3px", margin: "3px", padding: "3px", width:"30%"}}
                label="Min Scale"
                type="number"
                defaultValue="0"
                onChange={(e) => {
                    setQueryList({...queryList, scaleMin: `${e.target.value}`});
                    fetchEventsList(trackerId);
                }}
                InputLabelProps={{
                shrink: true,
                }}
            />
            <TextField
                style={{backgroundColor:"white", borderRadius:"3px", margin: "3px", padding: "3px", width:"30%"}}
                label="Max Rating"
                type="number"
                defaultValue="100"
                onChange={(e) => {
                    setQueryList({...queryList, scaleMax: `${e.target.value}`});
                    fetchEventsList(trackerId);
                }}
                InputLabelProps={{
                shrink: true,
                }}
            />
            <hr></hr>
            </div>
            : []}
            <Grid container spacing={2}>
                {
                    eventsList.map((event) => <EventPreview {...event} trackerId={trackerId}/>)
                }
            </Grid>
        </Container>
    );
}