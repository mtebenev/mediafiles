import React from 'react';
import {
  Text, FlatList, TouchableOpacity, Alert,
} from 'react-native';
import { MockMediaExplorerModule, IMediaLocation } from './media-explorer.mock';
import { SelectableList } from '../selectable-list.component';

interface IState {
  mediaLocations: IMediaLocation[];
  folders: string[];
  files: string[];
  selectedLocationId?: string;
}

export class MediaExplorerView extends React.Component<{}, IState> {
  private readonly mediaExplorerModule = new MockMediaExplorerModule();
  constructor(props: {}) {
    super(props);
    this.state = {
      mediaLocations: [],
      folders: ['folder 1', 'folder 2', 'folder 3'],
      files: ['file 1', 'file 2', 'file 3']
    };
  }

  public async componentDidMount(): Promise<void> {
    const mediaLocations = await this.mediaExplorerModule.getMediaLocations();
    this.setState({ ...this.state, mediaLocations });
  }
  public render(): React.ReactNode {
    return (
      <>
        <Text>I am explorer view</Text>
        <Text>Media locations:</Text>
        <SelectableList
          data={this.state.mediaLocations}
          renderItem={({ item }) => (
            <MediaLocationListItem location={item} />
          )}
          keyExtractor={item => item.id.toString()}
        />
        <Text>Folders</Text>
        <SelectableList
          data={this.state.folders}
          renderItem={({ item }) => (
            <FolderListItem folder={item} />
          )}
          keyExtractor={item => item}
        />
        <Text>Files</Text>
        <SelectableList
          data={this.state.files}
          renderItem={({ item }) => (
            <FileListItem file={item} />
          )}
          keyExtractor={item => item}
        />
      </>
    )
  }
}

const MediaLocationListItem: React.FC<{
  location: IMediaLocation
}> = props => (
  <Text>{props.location.name}</Text>
);

const FolderListItem: React.FC<{
  folder: string
}> = props => (
  <Text>{props.folder}</Text>
);

const FileListItem: React.FC<{
  file: string
}> = props => (
  <Text>{props.file}</Text>
);
