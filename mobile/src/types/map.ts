type MapLocation = {
  lat: number;
  lng: number;
};

type DistanceOrDuration = {
  text: string;
  value: number;
};

export type RouteLeg = {
  distance: DistanceOrDuration;
  duration: DistanceOrDuration;
  end_address: string;
  end_location: MapLocation;
  start_address: string;
  start_location: MapLocation;
  traffic_speed_entry: any[];
  via_waypoint: any[];
};
