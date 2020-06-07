import React, { } from 'react';
import Box from '@material-ui/core/Box';
import './app.css';
import { ItemListComponent } from './features/explorer/item-list/item-list.component';
import { ItemDetailsComponent } from './features/explorer/item-details.component';

interface IState {
  selectedCatalogItemId?: number;
}

export class App extends React.Component<{}, IState> {

  /**
   * Ctor.
   */
  constructor(props: any) {
    super(props);
    this.state = {};
  }

  /**
   * React.Component.
   */
  public render(): React.ReactNode {
    return (
      <div className="App">
        <Box
          display="flex"
          flexDirection="row">
          <ItemListComponent onItemSelected={(catalogItemId) => {
            this.setState({ ...this.state, selectedCatalogItemId: catalogItemId });
          }} />
          {this.state.selectedCatalogItemId !== undefined && (
            <ItemDetailsComponent catalogItemId={this.state.selectedCatalogItemId} />
          )}
        </Box>
      </div>
    );
  }
}

export default App;
