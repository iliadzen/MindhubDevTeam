import React from 'react';
import Rating from '@material-ui/lab/Rating';


const RatingForm = ({handler, value}) => {
    return (
        <div>
            <h3>Add rating:</h3>
            <Rating
                value={value}
                onChange={(event, newValue) => {
                    handler(parseInt(newValue))
                }}
            />
        </div>
    );
}
export default RatingForm;


