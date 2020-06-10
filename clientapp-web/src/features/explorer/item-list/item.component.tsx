import React, { } from 'react';
import { IRestCatalogItem } from '../irest-catalog-item';
import { Box } from '@material-ui/core';

interface IProps {
  item: IRestCatalogItem;
}

/**
 * The item component.
 */
export class ItemComponent extends React.Component<IProps> {

  /**
   * Ctor.
   */
  constructor(props: IProps) {
    super(props);
  }

  /**
   * React.Component.
   */
  public render(): React.ReactNode {
    return (
      <Box
        display="flex"
        flexDirection="row"
      >
        <div>{this.props.item.catalogItemId}</div>
        <div>{this.props.item.path}</div>
      </Box>
    );
  }
}
