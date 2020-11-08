import React from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import { deleteTracker } from '../../Api';
import LinkButton from '../Common/LinkButton';

const useStyles = makeStyles((theme) => ({
    card: {
        margin: "50px 20px 5px 20px",
    },
}));

const TrackerCard = ({id, title, creationDate, modificationDate, customizations}) => {
    const history = useHistory();
    const classes = useStyles();

    const handleDelete = async (trackerId) => {
        await deleteTracker(id);
        history.push(`/trackers`);
    }

    return (
        <div>
        <Card className={classes.card}>
            <CardContent>
                <h3>{title}</h3>
                <p>Created at: <i>{creationDate}</i></p>
                <p>Last modification: <i>{modificationDate}</i></p>
                <p>Customizations: <i>{customizations.join(", ")}</i></p>
            </CardContent>
            <CardActions>
                <LinkButton url={`/trackers/${id}/edit`} text="Edit" fullWidth={true}/>
                <Button
                    fullWidth
                    variant="contained"
                    color="secondary"
                    style={{marginTop: "10px",}}
                    onClick={() => {
                        handleDelete(id);
                    }}
                >
                    Delete
                </Button>
            </CardActions>
        </Card>
        <LinkButton url={`/trackers/${id}/events/create`} text="Create Event!" fullWidth={true}/>
        </div>
    );
}

export default TrackerCard;