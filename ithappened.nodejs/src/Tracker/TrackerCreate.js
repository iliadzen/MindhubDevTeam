import React, { useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import { useHistory } from 'react-router-dom';

import Container from '@material-ui/core/Container';
import { makeStyles } from '@material-ui/core/styles';

import TrackerCreateForm from '../Components/Tracker/TrackerCreateForm';
import LinkButton from '../Components/Common/LinkButton';

import { createTracker } from '../Api'

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
    
    const handleCreate = async (title, customizationsString) => {
        await createTracker(title, customizationsString);
        history.push(`/trackers`);
    }

    return (
        <Container component="main" maxWidth="xs">
            <LinkButton url={`/`} text="Go Back!" />
            <TrackerCreateForm className={classes.root} tracker={tracker} onSubmit={handleCreate}/>
        </Container>
    );
}