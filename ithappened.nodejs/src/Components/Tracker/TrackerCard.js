import React from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import { deleteTracker } from '../../Api';


const useStyles = makeStyles((theme) => ({
    card: {
        margin: "50px 20px 5px 20px",
    },
}));

const TrackerCard = ({id, title, creationDate, modificationDate, customizations}) => {
    const history = useHistory();
    const classes = useStyles();
    return (
        <Card className={classes.card}>
            <CardContent>
                <h3>{title}</h3>
                <p>Creation date: {creationDate}</p>
                <p>Modification date: {modificationDate}</p>
                <p>Modifications: {customizations === [] ? customizations.join(" ") : "No Customisations"}</p>
            </CardContent>
            <CardActions>
            <Button
                    fullWidth
                    variant="contained"
                    color="primary"
                    className={classes.submit}
                    onClick={() => {
                        history.push(`/trackers/${id}/edit`);
                    }}
                >
                    Edit
                </Button>
                <Button
                    fullWidth
                    variant="contained"
                    color="secondary"
                    className={classes.submit}
                    onClick={() => {
                        deleteTracker(id);
                        history.push(`/trackers`);
                    }}
                >
                    Delete
                </Button>
            </CardActions>
        </Card>
        
    );
}

export default TrackerCard;