import React, { useEffect, useState } from 'react';
import { Button, StyleSheet, Text, View } from 'react-native';
import MapView, { Marker, Polyline, PROVIDER_GOOGLE } from 'react-native-maps';
import polyline from '@mapbox/polyline';

import {
  MapScreenProps,
  Route,
  RouteCoordinate,
  Waypoint,
} from 'types/map-screen-type';
import { RouteLeg } from 'types/map';

const MapScreen = ({ navigation }: MapScreenProps) => {
  const [routeCoordinates, setRouteCoordinates] = useState<RouteCoordinate[]>([]);
  const [waypoints, setWaypoints] = useState<Waypoint[]>([
    { latitude: 39.9373193, longitude: 32.8775523 },
    { latitude: 39.9283269, longitude: 32.8654476 },
    { latitude: 39.9409097, longitude: 32.8377803 },
    { latitude: 39.9582303, longitude: 32.8172487 },
    { latitude: 39.9620885, longitude: 32.8110612 },
    { latitude: 39.9569244, longitude: 32.7950965 },
    { latitude: 39.945225, longitude: 32.7375667 },
    { latitude: 39.9795356, longitude: 32.65884 },
  ]);

  const destination = { latitude: 40.0689064, longitude: 32.5902646 };

  const decodePolyline = (encoded: string) => {
    const decodedPoints = polyline.decode(encoded);
    const coordinates = decodedPoints.map((point: any) => ({
      latitude: point[0],
      longitude: point[1],
    }));
    return coordinates;
  };

  const fetchRoute = async () => {
    try {
      const waypointsString = waypoints
        .map((point) => `${point.latitude},${point.longitude}`)
        .join('|');

      const url = `https://maps.googleapis.com/maps/api/directions/json?origin=${waypoints[0].latitude},${waypoints[0].longitude}&destination=${destination.latitude},${destination.longitude}&waypoints=${waypointsString}&key=${process.env.REACT_APP_MAP_API}`;

      fetch(url)
        .then((response) => response.json())
        .then((data) => {
          const overviewPolyline = data.routes[0].overview_polyline.points;
          
          data.routes[0].legs.forEach((leg: RouteLeg) => {
            console.log(leg.end_address);
          });

          const decodedPoints = decodePolyline(overviewPolyline);
          setRouteCoordinates(decodedPoints);
        });
    } catch (error) {
      console.error('Error fetching directions:', error);
    }
  };

  useEffect(() => {
    fetchRoute();
  }, [waypoints]);

  useEffect(() => {
    const unsubscribe = navigation.addListener('focus', () => {
      const state = navigation.getState();
      const routeWithWaypoint = state.routes.find(
        (route: Route) => route.params?.newWaypoint
      ) as Route | undefined;

      if (routeWithWaypoint && routeWithWaypoint.params?.newWaypoint) {
        const newWaypoint = routeWithWaypoint.params.newWaypoint as Waypoint;
        setWaypoints((prevWaypoints) => [...prevWaypoints, newWaypoint]);
      }
    });

    return unsubscribe;
  }, [navigation]);

  return (
    <View style={styles.container}>
      <MapView
        style={styles.map}
        initialRegion={{
          latitude: waypoints[0].latitude,
          longitude: waypoints[0].longitude,
          latitudeDelta: 0.1,
          longitudeDelta: 0.1,
        }}
      >
        {Array.isArray(waypoints) && waypoints.length > 0 && waypoints.map((waypoint: Waypoint, index) => (
          <Marker
            key={`Waypoint ${index + 1}`}
            coordinate={waypoint}
            title={`Waypoint ${index + 1}`}
          />
        ))}

        <Marker coordinate={destination} title="Destination" />

        {Array.isArray(routeCoordinates) && routeCoordinates.length > 0 && (
          <Polyline coordinates={routeCoordinates} strokeWidth={5} strokeColor="blue" />
        )}

      </MapView>

      <Button
        title="Add New Waypoint"
        onPress={() => navigation.navigate('AddWaypointScreen')}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  map: {
    flex: 1,
  },
});

export default MapScreen;
