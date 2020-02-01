import React, { ComponentType } from 'react';
import {
  Text
} from 'react-native';
import { MockMediaExplorerModule, IMediaLocation, IMediaExplorerModule } from './media-explorer.mock';
import { SelectableList } from '../common/selectable-list.component';
import { withDataConnector, IDataConnector } from '../common/data-connector';

interface IState {
  mediaLocations: IMediaLocation[];
  folders: string[];
  files: string[];
  selectedLocationId?: string;
}

export class MediaExplorerView extends React.Component<{}, IState> {
  private readonly mediaExplorerModule: IMediaExplorerModule;
  private readonly mediaExplorerConnector: IDataConnector<IMediaLocation[]>;

  constructor(props: {}) {
    super(props);

    this.mediaExplorerModule = new MockMediaExplorerModule();
    this.mediaExplorerConnector = {
      fetch: () =>  {
        return this.mediaExplorerModule.getMediaLocations();
      }
    }


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
        <MediaLocationList connector={this.mediaExplorerConnector} />
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

const MediaLocationListImpl: React.FC<{
  connector: IDataConnector<IMediaLocation[]>;
  data?: IMediaLocation[];
}> = props => (
  <SelectableList
    data={props.data}
    renderItem={({ item }) => (
      <MediaLocationListItem location={item} />
    )}
    keyExtractor={item => item.id.toString()}
  />
);

const MediaLocationList = withDataConnector()(MediaLocationListImpl);

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
