import React from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import { deleteEvent } from '../../Api';
import LinkButton from '../Common/LinkButton';
import CommentReadonly from './Customization/CommentReadonly';
import GeotagReadonly from './Customization/GeotagReadonly';
import ScaleReadonly from './Customization/ScaleReadonly';
import RatingReadonly from './Customization/RatingReadonly';
// import PhotoBase64 from './Customization/PhotoBase64';
const useStyles = makeStyles((theme) => ({
    card: {
        margin: "50px 20px 5px 20px",
    },
}));

const EventCard = ({trackerId, id, title, creationDate, modificationDate, customizations}) => {
    const history = useHistory();
    const classes = useStyles();

    console.log(customizations);
    
    const handleDelete = async (id) => {
        await deleteEvent(id);
        history.push(`/trackers`);
    }

    return (
        <Card className={classes.card}>
            <CardContent>
                <h3>{title}</h3>
                <p>Creation date: {creationDate}</p>
                <p>Modification date: {modificationDate}</p>
            </CardContent>
            <CardContent>
                <h3>More about:</h3>
                {customizations.comment != null ? <CommentReadonly value={customizations.comment.content} /> : []}
                {customizations.geotag != null ? <GeotagReadonly geotag={customizations.geotag} /> : []}
                {/* {customizations.photo != null ? <PhotoBase64 data={customizations.photo.DataUrl} /> : []} */}
                {customizations.rating != null ? <RatingReadonly stars={customizations.rating.stars} /> : []}
                {customizations.scale != null ? <ScaleReadonly value={customizations.scale.value} /> : []}
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

export default EventCard;