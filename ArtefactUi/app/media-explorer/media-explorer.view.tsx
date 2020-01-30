import React from 'react';
import {
  Text, FlatList, TouchableOpacity, Alert,
} from 'react-native';
import { MockMediaExplorerModule, IMediaLocation } from './media-explorer.mock';

interface IState {
  mediaLocations: IMediaLocation[];
  selectedLocationId?: string;
}

export class MediaExplorerView extends React.Component<{}, IState> {
  private readonly mediaExplorerModule = new MockMediaExplorerModule();
  constructor(props: {}) {
    super(props);
    this.state = { mediaLocations: [] };
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
        <FlatList
          data={this.state.mediaLocations}
          renderItem={({ item }) => (
            <MediaLocationListItem
              location={item}
              isSelected={this.state.selectedLocationId === item.id}
              onSelect={(id) => {
                this.setState({ ...this.state, selectedLocationId: id });
              }}
            />
          )}
          keyExtractor={item => item.id.toString()}
          extraData={this.state.selectedLocationId}
        >
        </FlatList>
        <Text>Folders</Text>
        <FlatList
          data={this.state.mediaLocations}
          renderItem={({ item }) => (
            <MediaLocationListItem
              location={item}
              isSelected={this.state.selectedLocationId === item.id}
              onSelect={(id) => {
                this.setState({ ...this.state, selectedLocationId: id });
              }}
            />
          )}
          keyExtractor={item => item.id.toString()}
          extraData={this.state.selectedLocationId}
        >
        </FlatList>
        <Text>Files</Text>
        <FlatList
          data={this.state.mediaLocations}
          renderItem={({ item }) => (
            <MediaLocationListItem
              location={item}
              isSelected={this.state.selectedLocationId === item.id}
              onSelect={(id) => {
                this.setState({ ...this.state, selectedLocationId: id });
              }}
            />
          )}
          keyExtractor={item => item.id.toString()}
          extraData={this.state.selectedLocationId}
        >
        </FlatList>
      </>
    )
  }
}

const MediaLocationListItem: React.FC<{
  location: IMediaLocation,
  isSelected: boolean,
  onSelect: (id: string) => void
}> = props => (
  <TouchableOpacity
    onPress={() => {
      props.onSelect(props.location.id);
    }}
    style={{ backgroundColor: props.isSelected ? '#6e3b6e' : '#f9c2ff' }}>
    <Text>{props.location.name}</Text>
  </TouchableOpacity>
);
