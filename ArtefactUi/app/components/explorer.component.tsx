import React from 'react';
import {View, StyleSheet, Text, Button, NativeModules} from 'react-native';

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
 * Displays artifacts explorer.
 */
export class ExplorerComponent extends React.Component {
  public render(): React.ReactNode {
    return (
      <View style={styles.container}>
        <Text>Explorer</Text>
        <Button
          title="Test"
          onPress={() => {
            NativeModules.AppModule.getItems((res: any) => {
              let bb = 3;
              bb++;
            });
          }} />
      </View>
    );
  }
}

