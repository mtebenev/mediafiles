import React from 'react';
import {View, StyleSheet, Text} from 'react-native';

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
    borderColor: 'red',
    borderWidth: 1
  },
});

/**
 * Displays media viewer.
 */
export class MediaViewerComponent extends React.Component {
  public render(): React.ReactNode {
    return (
      <View style={styles.container}>
        <Text>Media viewer</Text>
      </View>
    );
  }
}
