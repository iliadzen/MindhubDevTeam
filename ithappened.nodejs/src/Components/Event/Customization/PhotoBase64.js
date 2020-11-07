import React from 'react';

const PhotoBase64 = ({data}) => {
    return (
        <div>
            <h4>Your photo:</h4>
            <p>
                <img
                    src={data}
                    style={{
                        width: "100%",
                    }}
                />
                </p>
        </div>
    );
}
export default PhotoBase64;