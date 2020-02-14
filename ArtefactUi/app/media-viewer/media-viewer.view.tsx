import React, { } from 'react';
import {
  Text, View, ViewProps, StyleSheet
} from 'react-native';

interface IProps {
  filePath?: string;
}

export const MediaViewerView: React.FC<IProps & ViewProps> = props => {
  return (
    <View {...props}>
      <View style={styles.container}>
        {!props.filePath &&
          <Text style={styles.label1}>No file selected</Text>
        }
        {props.filePath && (
          <Text style={styles.label1}>Playing file: {props.filePath}</Text>
        )}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignContent: 'center'
  },
  label1: {
    textAlign: 'center'
  }
});
