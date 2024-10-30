import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity } from 'react-native';
import DraggableFlatList, { RenderItemParams } from 'react-native-draggable-flatlist';

type Location = {
  key: string;
  address: string;
};

const DraggableList = () => {
  const initialData: Location[] = [
    { key: '1', address: 'Plevne Cd., Sakarya, 06230 Altındağ/Ankara' },
    { key: '2', address: 'Akbank ATM, Hacettepe, Celal Bayar Blv.' },
    { key: '3', address: 'Hipodrom Cd., Emniyet, 06070 Yenimahalle' },
    { key: '4', address: 'İvedik Cd., Gayret, 06170 Yenimahalle/Ankara' },
    { key: '5', address: 'Ahmet Refik Cd 1, Gayret, 06170 Yenimahalle' },
    { key: '6', address: 'Çınardibi Cd., 25 Mart, 06200 Yenimahalle' },
    { key: '7', address: 'Ankara, İnönü, 06370 Yenimahalle/Ankara' },
    { key: '8', address: 'Fatih Sultan Mehmet Blv 642, Yeni Batı, 06...' },
    { key: '9', address: 'TİTRA ÜRETİM TESİSİ VE AR-GE, Saray OSB' },
  ];
  const [data, setData] = useState<Location[]>(initialData);

  const renderItem = ({ item, drag, isActive }: RenderItemParams<Location>) => {
    return (
      <TouchableOpacity
        style={[
          styles.item,
          { backgroundColor: isActive ? 'green' : '#e0e0e0' },
        ]}
        onLongPress={drag}
      >
        <Text style={styles.addressText}>{item.address}</Text>
      </TouchableOpacity>
    );
  };

  return (
    <View style={styles.container}>
      <DraggableFlatList
        data={data}
        renderItem={renderItem}
        keyExtractor={(item) => item.key}
        onDragEnd={({ data }) => setData(data)}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 10,
  },
  item: {
    padding: 15,
    marginVertical: 5,
    borderRadius: 10,
    borderColor: '#d3d3d3',
    borderWidth: 1,
  },
  addressText: {
    fontSize: 16,
    color: '#333',
  },
});

export default DraggableList;
