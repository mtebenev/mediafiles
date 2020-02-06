import React, { ComponentType } from 'react';
import {
  Text
} from 'react-native';
import { withDataConnectorProps } from '../common/data-connector';
import { IMediaLocation } from './media-explorer.mock';
import { SelectableList } from '../common/selectable-list.component';

interface IProps {
  data: IMediaLocation[];
  onSelectionChanged: (mediaLocation: IMediaLocation) => void;
}

const MediaLocationListImpl: React.FC<IProps> = props => (
  <SelectableList
    data={props.data}
    renderItem={({ item }) => (
      <MediaLocationListItem location={item} />
    )}
    keyExtractor={item => item.id.toString()}
    onSelectionChanged={(item) => {
      props.onSelectionChanged(item);
    }}
  />
);

const MediaLocationListItem: React.FC<{
  location: IMediaLocation
}> = props => (
  <Text>{props.location.name}</Text>
);

export const MediaLocationList = withDataConnectorProps(MediaLocationListImpl);
