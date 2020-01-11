import React, {Fragment} from 'react';
import {
  SafeAreaView,
  StyleSheet,
  ScrollView,
  View,
  Text,
  StatusBar,
  Button,
  Alert,
  UIManager
} from 'react-native';
import {NativeModules, NativeEventEmitter, requireNativeComponent, findNodeHandle} from 'react-native';
//let VideoPlayer = requireNativeComponent('VideoPlayer');
const CustomUserControlCS = requireNativeComponent('CustomUserControlCS');

import {
  Header,
  LearnMoreLinks,
  Colors,
  DebugInstructions,
  ReloadInstructions,
} from 'react-native/Libraries/NewAppScreen';

export class App extends React.Component {
  private _ctrlRef: any;

  constructor(props) {
    super(props);
    this.state = {};
  }
  public render() {
    return (
      <Fragment>
        <View style={styles.body}>
          <Button title="TEST" onPress={() => {
            // Invoke component command
            const tag = findNodeHandle(this._ctrlRef);
            UIManager.dispatchViewManagerCommand(tag, UIManager.getViewManagerConfig('CustomUserControlCS').Commands.CustomCommand, ['some_arg']);


          }}>
          </Button>
        </View>
        <View style={{ backgroundColor: 'Gray', flex: 1 }}>
          <CustomUserControlCS
            style={styles.customcontrol}
            label="CustomUserControlCS!"
            ref={(ref) => {
              this._ctrlRef = ref;
            }}
          />
        </View>
      </Fragment>
    );
  }
}

const styles = StyleSheet.create({
  customcontrol: {
    color: '#333333',
    backgroundColor: '#006666',
    flex: 1,
    margin: 10,
  },
  scrollView: {
    backgroundColor: Colors.lighter,
  },
  engine: {
    position: 'absolute',
    right: 0,
  },
  body: {
    backgroundColor: Colors.white,
  },
  sectionContainer: {
    marginTop: 32,
    paddingHorizontal: 24,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: '600',
    color: Colors.black,
  },
  sectionDescription: {
    marginTop: 8,
    fontSize: 18,
    fontWeight: '400',
    color: Colors.dark,
  },
  highlight: {
    fontWeight: '700',
  },
  footer: {
    color: Colors.dark,
    fontSize: 12,
    fontWeight: '600',
    padding: 4,
    paddingRight: 12,
    textAlign: 'right',
  },
});

export default App;
