import React, { } from 'react';
import { GridList, GridListTile } from '@material-ui/core';


interface IProps {
  catalogItemId: number;
}

interface IState {
  thumbnailUrls: string[];
}

/**
 * The item details component.
 */
export class ItemDetailsComponent extends React.Component<IProps, IState> {

  /**
   * Ctor.
   */
  constructor(props: IProps) {
    super(props);
    this.state = { thumbnailUrls: [] };
  }

  /**
   * React.Component.
   */
  public render(): React.ReactNode {
    return this.state.thumbnailUrls && (
      <GridList>
        {this.state.thumbnailUrls.map(url => (
          <GridListTile key={url}>
            <img
              src={url}
              width="100"
              height="100"
            />
          </GridListTile>
        ))}
      </GridList>
    );
  }

  /**
   * NewLifecycle.
   */
  public componentDidUpdate(prevProps: Readonly<IProps>, prevState: Readonly<IState>): void {
    if(prevProps.catalogItemId !== this.props.catalogItemId) {
      this.loadThumbnails();
    }
  }

  /**
   * Loads thumbnails for the catalog item.
   */
  private async loadThumbnails(): Promise<void> {
    console.error('Starting request.');
    var url1 = `http://localhost:9001/api/item/${this.props.catalogItemId}/thumbnails`;
    const requestThumbnails = new Request(url1);
    const responseThumbnails = await fetch(requestThumbnails);
    const thumbnailIds: number[] = await responseThumbnails.json();

    var thumbnailUrls = thumbnailIds.map(id => `http://localhost:9001/api/thumbnails/${id}`);
    this.setState({ ...this.state, thumbnailUrls: thumbnailUrls })

    console.error(`Thumbnail urls: ${JSON.stringify(thumbnailUrls)}`);
  }
}
