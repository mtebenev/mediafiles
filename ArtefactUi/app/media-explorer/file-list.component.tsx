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

const FileListImpl: React.FC<IProps> = props => (
  <SelectableList
    data={props.directory.items.filter(i => !i.isDirectory)}
    renderItem={({ item }) => (
      <FileListItem item={item} />
    )}
    keyExtractor={item => item.name}
    onSelectionChanged={(item) => {
      Alert.alert('File changed');
    }}
  />
);

const FileListItem: React.FC<{
  item: IFileSystemItem
}> = props => (
  <Text>{props.item.name}</Text>
);

export const FileList = FileListImpl;
