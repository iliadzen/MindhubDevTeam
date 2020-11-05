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
        <Card className={classes.card}>
            <CardContent>
                <h3>{title}</h3>
                <p>Creation date: {creationDate}</p>
                <p>Modification date: {modificationDate}</p>
                <p>Customizations: {customizations.join(", ")}</p>
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
        
    );
}

export default TrackerCard;