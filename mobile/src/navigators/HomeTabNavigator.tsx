import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import Icon from 'react-native-vector-icons/MaterialCommunityIcons';

import HomeScreen from 'screens/HomeScreen';
import MapScreen from 'screens/MapScreen';
import ChatScreen from 'screens/ChatScreen';
import MenuScreen from 'screens/MenuScreen';
import { Button, Text, View } from 'react-native';

const Tab = createBottomTabNavigator();

const HomeTabNavigator = () => {
  return (
    <Tab.Navigator
      initialRouteName='Home'
      screenOptions={({ route }) => ({
        tabBarIcon: ({ color, size }) => {
          let iconName = '';

          if (route.name === 'Home') {
            iconName = 'home';
          } else if (route.name === 'Map') {
            iconName = 'map-marker';
          } else if (route.name === 'Chat') {
            iconName = 'chat';
          } else if (route.name === 'Menu') {
            iconName = 'menu';
          }

          return <Icon name={iconName} color={color} size={size} />;
        },
        tabBarActiveTintColor: '#e91e63',
        tabBarInactiveTintColor: '#000000',
        tabBarStyle: {
          backgroundColor: '#ffffff',
          borderTopWidth: 0,
          paddingVertical: 10,
          height: 60,
        },
        tabBarLabelStyle: {
          paddingBottom: 8,
          fontSize: 12,
          fontWeight: 'bold',
        },
        headerShown: false,
      })}
    >
      <Tab.Screen
        name='Home'
        component={HomeScreen}
        options={{ tabBarLabel: 'Ana Sayfa' }}
      />
      <Tab.Screen
        name='Map'
        component={MapScreen}
        options={{ tabBarLabel: 'Harita' }}
      />
      <Tab.Screen
        name='Chat'
        component={ChatScreen}
        options={{ tabBarLabel: 'Sohbet' }}
      />
      <Tab.Screen
        name='Menu'
        component={MenuScreen}
        options={{
          headerShown: true,
          header: () => (
            <View
              style={{
                backgroundColor: 'gray',
                padding: 15,
                paddingTop: 50,
                flexDirection: 'row',
                alignItems: 'center',
                justifyContent: 'center',
              }}
            >
              <Text style={{ fontSize: 20, color: 'white' }}>Settings</Text>
            </View>
          ),
        }}
      />
    </Tab.Navigator>
  );
};

export default HomeTabNavigator;
