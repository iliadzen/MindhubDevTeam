import { useState } from 'react';
import { YMaps, Map, Placemark } from 'react-yandex-maps';

const GeotagForm = ({geotag}) => {
    const mapState = { center: [geotag.latitude, geotag.longitude], zoom: 8 };
    const placeMark = {
        geometry: [geotag.latitude, geotag.longitude],
    };
    return (
        <YMaps>
            <h4>Your geotag:</h4>
            <Map width={"100%"} state={mapState}>
                <Placemark {...placeMark} />
            </Map>
        </YMaps>
    );
};

export default GeotagForm;