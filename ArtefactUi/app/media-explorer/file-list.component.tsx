import React, { } from 'react';
import {
  Text
} from 'react-native';
import { IDirectoryContent, IFileSystemItem } from './media-explorer.mock';
import { SelectableList } from '../common/selectable-list.component';

interface IProps {
  directory: IDirectoryContent;
  onSelectionChanged: (fsItem: IFileSystemItem) => void;
}

const FileListImpl: React.FC<IProps> = props => (
  <SelectableList
    data={props.directory.items.filter(i => !i.isDirectory)}
    renderItem={({ item }) => (
      <FileListItem item={item} />
    )}
    keyExtractor={item => item.name}
    onSelectionChanged={(item) => {
      props.onSelectionChanged(item);
    }}
  />
);

const FileListItem: React.FC<{
  item: IFileSystemItem
}> = props => (
  <Text>{props.item.name}</Text>
);

export const FileList = FileListImpl;
