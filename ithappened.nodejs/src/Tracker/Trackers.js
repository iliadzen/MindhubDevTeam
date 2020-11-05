import React, { useEffect, useState } from 'react';
import {Link, useHistory} from 'react-router-dom';
import { makeStyles } from '@material-ui/core/styles';
import { getTrackers } from '../Api.js'
import Container from '@material-ui/core/Container';
import Button from '@material-ui/core/Button';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  paper: {
      margin: "5px",
      padding: "5px",
  },
  control: {
    padding: theme.spacing(2),
  },
}));

export default function TrackersList() {
    const history = useHistory();
    const classes = useStyles();

    const [trackersList, setTrackersList] = useState([]);

    const fetchTrackersList = async () => {
        const trackers = await getTrackers();
        console.log(trackers);
        setTrackersList(trackers);
    }
    useEffect(() => {
        fetchTrackersList();
    }, []);

    const [spacing, setSpacing] = React.useState(2);

    return (
        <Container component="main" spacing="9">
            <Grid container className={classes.root} spacing={2}>
                {trackersList.map((tracker) => (
                    <Grid item>
                        <Paper className={classes.paper}>
                            <p><b>{tracker.title}</b></p>
                            <Button
                                variant="contained"
                                color="secondary"
                                className={classes.submit}
                                onClick={() => {
                                    history.push("/trackers/"+tracker.id);
                                }}
                            >
                                See Detail
                            </Button>
                        </Paper>
                    </Grid>
                ))}
            </Grid>
        </Container>
    );
}