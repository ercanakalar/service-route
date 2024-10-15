import { View, StyleSheet } from 'react-native';
import React, { ReactNode } from 'react';

const Container = ({ children }: { children: ReactNode }) => {
  return (
    <>
      <View style={styles.topContainer}></View>
      {children}
    </>
  );
};

export default Container;

const styles = StyleSheet.create({
  topContainer: {
    flexDirection: 'column', // Stack the logo and search bar vertically
    alignItems: 'center',
    backgroundColor: '#ffffff',
    paddingVertical: 20,
    elevation: 3, // Adds a shadow for Android
    shadowColor: '#000', // Shadow for iOS
    shadowOpacity: 0.1,
    shadowRadius: 5,
  },
});
