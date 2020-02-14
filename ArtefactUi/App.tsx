/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * Generated with the TypeScript template
 * https://github.com/react-native-community/react-native-template-typescript
 *
 * @format
 */

import React, { useState } from 'react';
import {
  StyleSheet,
  View,
  Button,
  Text,
} from 'react-native';

import {
  Colors,
} from 'react-native/Libraries/NewAppScreen';
import { MediaViewerView } from './app/media-viewer/media-viewer.view';
import { MediaExplorerView } from './app/media-explorer/media-explorer.view';

const App = () => {
  const [selectedFileName, setSelectedFileName] = useState<string | undefined>();
  return (
    <View style={styles.body}>
      <View style={styles.mainContainer}>
        <MediaExplorerView
          style={styles.explorerPane}
          onFileSelected={(fsItem) => {
            setSelectedFileName(fsItem.path);
          }}
        />
        <MediaViewerView style={styles.viewerPane} filePath={selectedFileName} />
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  body: {
    backgroundColor: Colors.white,
    flex: 1
  },
  mainContainer: {
    flexDirection: 'row',
    flex: 1,
  },
  explorerPane: {
    flex: 1,
    borderWidth: 1,
    borderColor: 'red',
  },
  viewerPane: {
    flex: 1,
    borderWidth: 1,
    borderColor: 'red',
  }
});

export default App;
