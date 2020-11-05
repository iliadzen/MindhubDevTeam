import React from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';

import LinkButton from '../Common/LinkButton';

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
                <LinkButton url={`/trackers/${id}`} text="See Detail" fullWidth={true}/>
            </CardActions>
        </Card>
    );
}

export default TrackerPreview;