import React, { } from 'react';
import { IRestCatalogItem } from './irest-catalog-item';

interface IState {
  items: IRestCatalogItem[];
}

/**
 * The item list component.
 */
export class ItemListComponent extends React.Component<{}, IState> {

  /**
   *
   */
  constructor(props: any) {
    super(props);
    this.state = {items: []};
  }

  /**
   * React.Component.
   */
  public render(): React.ReactNode {
    return (
      <>
        <div>Items list:</div>
        <div>
          {this.state.items.map(i => (
            <div key={i.catalogItemId}>
              <div>{i.catalogItemId}</div>
              <div>{i.path}</div>
            </div>
          ))}
        </div>
        <div>
          <button onClick={() => {
            this.loadData();
          }}>
            Load Data
          </button>
        </div>
        <div>

        </div>
      </>
    );
  }

  /**
   * Loads catalog items.
   */
  private async loadData(): Promise<void> {

    console.error('Starting request.');
    var url = 'http://localhost:9001/api/items';
    const request = new Request(url);
    const response = await fetch(request);
    const items = await response.json();

    console.error(`Got data: ${JSON.stringify(items)}`);

    this.setState({ ...this.state, items: items });
  }
}
