import React from 'react';
import Rating from '@material-ui/lab/Rating';


const RatingForm = ({stars}) => {
    return (
        <div>
            <h4>Your rate:</h4>
            <Rating
                value={stars}
                readOnly
            />
        </div>
    );
}
export default RatingForm;


