import React, { ComponentType } from 'react';
import {
  Text, Alert, Button, NativeModules
} from 'react-native';
import { MockMediaExplorerModule, IMediaLocation, IMediaExplorerModule, IFileSystemItem, IDirectoryContent } from './media-explorer.mock';
import { IDataConnector } from '../common/data-connector';
import { MediaLocationList } from './media-location-list.component';
import { FolderList } from './folder-list.component';
import { FileList } from './file-list.component';

interface IState {
  mediaLocations: IMediaLocation[];
  currentDirectory?: IDirectoryContent;
  selectedFile?: IFileSystemItem;
}

export class MediaExplorerView extends React.Component<{}, IState> {
  private readonly mediaExplorerModule: IMediaExplorerModule;
  private readonly realModule: IMediaExplorerModule;
  private readonly mediaExplorerConnector: IDataConnector<IMediaLocation[]>;
  private readonly folderConnector: IDataConnector<IDirectoryContent>;

  constructor(props: {}) {
    super(props);
    this.state = {mediaLocations: []};
    this.realModule = NativeModules.MediaExplorerModule;

    this.mediaExplorerModule = new MockMediaExplorerModule();
    this.mediaExplorerConnector = {
      fetch: () => {
        return this.mediaExplorerModule.getMediaLocations();
      }
    };
    this.folderConnector = {
      fetch: () => {
        return this.mediaExplorerModule.getRoot('location1');
      }
    };
  }

  public async componentDidMount(): Promise<void> {
  }

  public render(): React.ReactNode {
    return (
      <>
        <Text>I am explorer view</Text>
        <Button title="Add Location"
          onPress={() => {
            this.realModule.addLocation();
          }}
        />
        <Button title="Refresh Locations"
          onPress={() => {
            this.loadLocations();
          }}
        />
        <Text>Media locations:</Text>
        <MediaLocationList
          data={this.state.mediaLocations}
          onSelectionChanged={(ml: IMediaLocation) => {
            this.loadRootDirectory(ml);
          }}
        />
        {this.state.currentDirectory && (
          <>
            <Text>Folders</Text>
            <FolderList
              directory={this.state.currentDirectory}
            />
            <Text>Files</Text>
            <FileList
              directory={this.state.currentDirectory}
            />
          </>
        )}
      </>
    )
  }

  private async loadRootDirectory(mediaLocation: IMediaLocation): Promise<void> {
    //const directory = await this.mediaExplorerModule.getRoot(mediaLocation.path);
    //this.setState({ ...this.state, currentDirectory: directory });
    const directory = await this.realModule.getRoot(mediaLocation.name);
    Alert.alert(`Got directory: ${JSON.stringify(directory)}`);
    this.setState({ ...this.state, currentDirectory: directory });
  }

  private async loadLocations(): Promise<void> {
    const locations = await this.realModule.getMediaLocations();
    Alert.alert(JSON.stringify(locations));
    this.setState({...this.state, mediaLocations: locations});
  }
}
