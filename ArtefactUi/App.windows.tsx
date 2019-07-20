import React, {Component} from 'react';
import {AppRegistry, StyleSheet, View, } from 'react-native';

import {ExplorerComponent} from './app/components/explorer.component';
import {MediaViewerComponent} from './app/components/media-viewer.component';

const styles = StyleSheet.create({
  container: {
    flex: 1,
    flexDirection: 'row',
    alignContent: 'stretch',
    justifyContent: 'center',
    alignItems: 'stretch',
    backgroundColor: '#F5FCFF',
    borderColor: 'blue',
    borderWidth: 1
  },
});

class ArtefactUi extends Component {
  render() {
    return (
      <View style={styles.container}>
        <ExplorerComponent/>
        <MediaViewerComponent/>
      </View>
    );
  }
}

AppRegistry.registerComponent('ArtefactUi', () => ArtefactUi);
