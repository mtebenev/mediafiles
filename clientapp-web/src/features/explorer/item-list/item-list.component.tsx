import React, { } from 'react';
import { List, ListItem } from '@material-ui/core'
import { IRestCatalogItem } from '../irest-catalog-item';
import { ItemComponent } from './item.component';

interface IProps {
  /**
   * Callback invoked when a catalog item selected.
   */
  onItemSelected: (catalogItemId: number) => void;
}

interface IState {
  items: IRestCatalogItem[];
}

/**
 * The item list component.
 */
export class ItemListComponent extends React.Component<IProps, IState> {

  /**
   * Ctor.
   */
  constructor(props: IProps) {
    super(props);
    this.state = { items: [] };
  }

  /**
   * React.Component.
   */
  public render(): React.ReactNode {
    return (
      <>
        <div>Items list:</div>
        <List>
          {this.state.items.map(i => (
            <ListItem
              button
              onClick={(event) => this.props.onItemSelected(i.catalogItemId)}
            >
              <ItemComponent item={i} />
            </ListItem>
          ))}
        </List>
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
