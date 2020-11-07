import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';

import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import { TextField } from '@material-ui/core';
import CommentForm from './Customization/CommentForm';
import GeotagForm from './Customization/GeotagForm';
// import PhotoForm from './Customization/PhotoForm';
import RatingForm from './Customization/RatingForm';
import ScaleForm from './Customization/ScaleForm';



const useStyles = makeStyles((theme) => ({
    card: {
        margin: "5px 20px 5px 20px",
    },
}));

const TrackerCreateForm = ({tracker, onSubmit}) => {
    const history = useHistory();
    const classes = useStyles();
    const [trackerLoaded, setTracker] = useState({
        id: "Loading...",
        title: "Loading...",
        customizations: [
            "Comment"
        ]
    });
    const fetchTracker = async (tracker) => {
        await tracker;
    }
    useEffect(() => {
        fetchTracker(tracker);
    }, []);

    const [eventTitle, setEventTitle] = useState("");
    const [eventCustomizations, setCustomizations] = useState({
        Comment: {
            Content: ""
        },
        Rating: {
            Stars: 0
        },
        Scale: {
            Value: 0
        },
        Geotag: {
            Longitude: 0,
            Latitude: 0,
        },
        // Photo: {
        //     Photo: ""
        // },
    });

    const handleCommentChange = (value) => {
        setCustomizations({...eventCustomizations, Comment: {Content: value}})
    };
    const handleGeotagChange = (geotag) => {
        setCustomizations({...eventCustomizations, Geotag: geotag})
    };
    // const handlePhotoChange = (binary) => {
    //     setCustomizations({...eventCustomizations, Photo: {Photo: binary}})
    // };
    const handleRatingChange = (rating) => {
        setCustomizations({...eventCustomizations, Rating: {Stars: rating}})
    };
    const handleScaleChange = (scale) => {
        setCustomizations({...eventCustomizations, Scale: {Value: scale}})
    };
    return (
        <Card className={classes.card}>
            <CardContent>
                <h2>Create Event</h2>
                <TextField
                    name="Title"
                    variant="outlined"
                    fullWidth
                    label="Title"
                    value={eventTitle}
                    onChange={(e) => setEventTitle(e.target.value)}
                    placeholder={"Event Title"}

                />
            </CardContent>
            <CardContent>
                
            {tracker.customizations.includes("Comment") ? <CommentForm handler={handleCommentChange} value={eventCustomizations.Comment.Content}/> : [] }
            {tracker.customizations.includes("Geotag") ? <GeotagForm handler={handleGeotagChange}/> : [] }
            {/* tracker.customizations.includes("Photo") ? <PhotoForm handler={handlePhotoChange}/> : [] } */}
            {tracker.customizations.includes("Rating") ? <RatingForm value={eventCustomizations.Rating.Stars} handler={handleRatingChange}/> : [] }
            {tracker.customizations.includes("Scale") ? <ScaleForm value={eventCustomizations.Scale.Value} handler={handleScaleChange}/> : [] }
                
            </CardContent>
            <CardActions>
            <Button
                fullWidth
                variant="contained"
                color="primary"
                className={classes.submit}
                onClick={() => {
                    onSubmit(tracker, eventTitle, eventCustomizations);
                }}
            >
                Submit
            </Button>
            </CardActions>
        </Card>
    );
}

export default TrackerCreateForm;