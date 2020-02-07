import React, { } from 'react';
import {
  Text, Alert
} from 'react-native';
import { IDirectoryContent, IFileSystemItem } from './media-explorer.mock';
import { SelectableList } from '../common/selectable-list.component';

interface IProps {
  directory: IDirectoryContent;
  //onSelectionChanged: (mediaLocation: IMediaLocation) => void;
}

const FolderListImpl: React.FC<IProps> = props => (
  <SelectableList
    data={props.directory.items.filter(i => i.isDirectory)}
    renderItem={({ item }) => (
      <FolderListItem item={item} />
    )}
    keyExtractor={item => item.name}
    onSelectionChanged={(item) => {
      Alert.alert('Folder changed');
    }}
  />
);

const FolderListItem: React.FC<{
  item: IFileSystemItem
}> = props => (
  <Text>{props.item.name}</Text>
);

export const FolderList = FolderListImpl;
