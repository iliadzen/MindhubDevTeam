import React, { useEffect, useState } from 'react';
import {Link, useHistory} from 'react-router-dom';
import { makeStyles } from '@material-ui/core/styles';
import { getTracker } from '../Api.js'
import { useParams } from "react-router-dom";
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import Container from '@material-ui/core/Container';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
    marginTop: "10vh",
  },
  paper: {
    height: 140,
    width: 150,
  },
  control: {
    padding: theme.spacing(2),
  },
}));

export default function TrackerDetail() {
    const {trackerId} = useParams();
    const history = useHistory();
    const classes = useStyles();
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
            <Card className={classes.root}>
                <CardContent>
                    <h3>{tracker.id}</h3>
                    <p>Creation date: {tracker.creationDate}</p>
                    <p>Creation date: {tracker.modificationDate}</p>
                    <p>Modifications: {tracker.customizations === [] ? tracker.customizations.join(" ") : "No Customisations"}</p>
                </CardContent>
                <CardActions>
                    <Button
                        variant="contained"
                        color="secondary"
                        className={classes.submit}
                        onClick={() => {
                            history.push(`/trackers/${tracker.id}/edit`);
                        }}
                    >
                        See Detail
                    </Button>
                </CardActions>
            </Card>
        </Container>
    );
}