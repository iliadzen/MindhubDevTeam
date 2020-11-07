import { useState } from 'react';
import { YMaps, Map, Placemark } from 'react-yandex-maps';

const GeotagForm = ({handler}) => {
  const [mapState, setMapState] = useState({ center: [55.750579, 37.611781], zoom: 8 });

  const [placeMark, setPlaceMark] = useState({
    geometry: [55.750579, 37.611781],
  });

  return (
      <YMaps>
          <h3>Select geotag:</h3>
          <Map
            state={mapState}
            onClick={(e) => {
              var coords = e.get('coords');
              let a = coords[0].toPrecision(8);
              let b = coords[1].toPrecision(8);
              let newPlaceMark = {
                geometry: [a, b],
              }
              setPlaceMark(newPlaceMark);

              let newMapState = { center: [a, b]}
              let geotag = {
                Latitude: parseFloat(a),
                Longitude: parseFloat(b)
              }
              handler(geotag)
            }}
          >
              <Placemark {...placeMark} />
          </Map>
      </YMaps>
  );
};

export default GeotagForm;