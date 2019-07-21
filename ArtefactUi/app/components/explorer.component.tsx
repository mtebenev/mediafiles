import React from 'react';
import {View, StyleSheet, Text, Button, FlatList, ListRenderItemInfo} from 'react-native';
import {ExplorerService} from '../core/services/explorer.service';

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

interface IState {
  items: string[] | null;
}

/**
 * Displays artifacts explorer.
 */
export class ExplorerComponent extends React.Component<{}, IState> {

  private _explorerService: ExplorerService;

  constructor(props: any) {
    super(props);
    this._explorerService = new ExplorerService();
    this.state = {items: null};
  }

  public render(): React.ReactNode {
    return (
      <View style={styles.container}>
        <Text>Explorer</Text>
        <Button
          title="Test"
          onPress={() => {
            this._explorerService.getItems().then((items) => {
              this.setState({...this.state, items: items});
            })
          }} />
        <FlatList
          data={this.state.items}
          renderItem={item => this.renderItem(item)} />
      </View>
    );
  }

  /**
   * ComponentLifecycle
   */
  public componentDidMount(): void {
  }

  private renderItem(itemInfo: ListRenderItemInfo<string>): React.ReactElement {
    return (
      <Text>{itemInfo.item}</Text>
    );
  }

  private loadItems(): void {
  }
}

