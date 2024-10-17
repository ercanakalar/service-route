import React, { useState } from 'react';
import { View, TextInput, Button, StyleSheet, Alert } from 'react-native';

const AddWaypointScreen = ({ navigation }: { navigation: any }) => {
  const [latitude, setLatitude] = useState('39.945301446178874');
  const [longitude, setLongitude] = useState('32.77263918817639');

  const addWaypoint = () => {
    const lat = parseFloat(latitude);
    const lon = parseFloat(longitude);

    if (!isNaN(lat) && !isNaN(lon)) {
      const newWaypoint = { latitude: lat, longitude: lon };
      navigation.navigate('Map', { newWaypoint });
    } else {
      Alert.alert(
        'Invalid input',
        'Please enter valid latitude and longitude.'
      );
    }
  };
  return (
    <View style={styles.container}>
      <TextInput
        style={styles.input}
        placeholder='Latitude'
        value={latitude}
        onChangeText={setLatitude}
        keyboardType='numeric'
      />
      <TextInput
        style={styles.input}
        placeholder='Longitude'
        value={longitude}
        onChangeText={setLongitude}
        keyboardType='numeric'
      />
      <Button title='Add Waypoint' onPress={addWaypoint} />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  input: {
    height: 40,
    borderColor: '#ccc',
    borderWidth: 1,
    marginBottom: 20,
    paddingHorizontal: 10,
    borderRadius: 5,
  },
});

export default AddWaypointScreen;
