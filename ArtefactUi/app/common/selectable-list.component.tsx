import React from 'react';
import { FlatList, ListRenderItem, TouchableOpacity, ListRenderItemInfo } from 'react-native';

interface IProps<TItem> {
  data: TItem[];
  renderItem: ListRenderItem<TItem>;
  keyExtractor: (item: TItem, index: number) => string;
  onSelectionChanged: (item: TItem) => void;
}

interface IState {
  selectedItemId?: string;
}

/**
 * List component based on flat list
 */
export class SelectableList<T> extends React.Component<IProps<T>, IState> {

  constructor(props: IProps<T>) {
    super(props);
    this.state = {};
  }

  public render(): React.ReactNode {
    return (
      <>
        <FlatList
          data={this.props.data}
          keyExtractor={this.props.keyExtractor}
          extraData={this.state.selectedItemId}
          renderItem={item =>
            (
              <SelectableItem
                item={item}
                renderItem={this.props.renderItem}
                isSelected={this.state.selectedItemId === this.props.keyExtractor(item.item, item.index)}
                onSelect={item => {
                  const itemId = this.props.keyExtractor(item.item, item.index);
                  this.setState({ ...this.state, selectedItemId: itemId });
                  this.props.onSelectionChanged(item.item);
                }}
              />
            )}
        >
        </FlatList>
      </>
    );
  }
}

interface IItemProps<TItem> {
  item: ListRenderItemInfo<TItem>;
  renderItem: ListRenderItem<TItem>;
  isSelected: boolean;
  onSelect: (item: ListRenderItemInfo<TItem>) => void;
}

const SelectableItem = <T extends {}>(props: IItemProps<T>) => (
  <TouchableOpacity
    onPress={() => {
      props.onSelect(props.item);
    }}
    style={{ backgroundColor: props.isSelected ? '#6e3b6e' : '#f9c2ff' }}
  >
    {props.renderItem(props.item)}
  </TouchableOpacity>
);
