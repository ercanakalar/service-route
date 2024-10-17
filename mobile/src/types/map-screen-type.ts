import { NativeStackNavigationProp } from '@react-navigation/native-stack';

export type RootStackParamList = {
  MapScreen: undefined;
  AddWaypointScreen: undefined;
};

export type MapScreenNavigationProp = NativeStackNavigationProp<
  RootStackParamList,
  'MapScreen'
>;

export type MapScreenProps = {
  navigation: MapScreenNavigationProp;
};

export type Waypoint = {
  latitude: number;
  longitude: number;
};

export type RouteCoordinate = {
  latitude: number;
  longitude: number;
};

export type Route = {
  key: string;
  name: string;
  params?: {
    newWaypoint?: Waypoint;
  };
};
