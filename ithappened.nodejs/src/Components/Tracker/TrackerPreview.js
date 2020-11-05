import React from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';


const useStyles = makeStyles((theme) => ({
    card: {
      margin: "5px 20px 5px 20px",
    },
}));

const TrackerPreview = ({id, title, creationDate, modificationDate, customizations}) => {
    const history = useHistory();
    const classes = useStyles();
    return (
        <Card className={classes.card}>
            <CardContent>
                <h3>{title}</h3>
            </CardContent>
            <CardActions>
                <Button
                    fullWidth
                    variant="contained"
                    color="primary"
                    className={classes.submit}
                    onClick={() => {
                        history.push(`/trackers/${id}`);
                    }}
                >
                    Edit
                </Button>
            </CardActions>
        </Card>
    );
}

export default TrackerPreview;