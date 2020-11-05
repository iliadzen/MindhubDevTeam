

import React from 'react';
import { useHistory } from 'react-router-dom';
import Button from '@material-ui/core/Button';

const LinkButton = ({url, color = "primary", text, fullWidth=false}) => {
    const history = useHistory();
    return (
        <Button
            fullWidth={fullWidth}
            style={{marginTop: "10px",}}
            variant="contained"
            color={color}
            onClick={() => {
                history.push(url);
            }}
        >
            {text}
        </Button>
    );
}

export default LinkButton;