import React, { } from 'react';
import {
  Text
} from 'react-native';

/**
 * The data connector interface.
 */
export interface IDataConnector<TData> {

  /**
   * Fetches the data.
   */
  fetch(): Promise<TData>;
}

interface IDataState<TData> {
  data?: TData;
}

export interface IDataConnectorProps<TData> {
  connector: IDataConnector<TData>;
}

/**
 * Creates HoC for data fetching.
 */
export function withDataConnector<TData>():
  <P extends {} & IDataConnectorProps<TData>>(WrappedComponent: React.ComponentType<P>) => React.ComponentClass<P> {
  return <P extends {} & IDataConnectorProps<TData>>(WrappedComponent: React.ComponentType<P>) =>
    class extends React.Component<P, IDataState<TData>> {
      constructor(props: P) {
        super(props);
        this.state = {};
      }
      public async componentDidMount(): Promise<void> {
        const data = await this.props.connector.fetch();
        this.setState({...this.state, data});
      }
      public render(): React.ReactNode {
        return this.state.data
          ? <WrappedComponent data={this.state.data} {...this.props} />
          : <Text>Loading data...</Text>
      }
    }
}



