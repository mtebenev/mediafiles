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

interface IState<TData> {
  data?: TData;
}

interface IDataConnectorProps<TData> {
  connector: IDataConnector<TData>;
}

interface IDataConsumerProps<TData> {
  data: TData;
}

/**
 * HoC accepting connector with props
 * TODO: get rid of any
 */
export function withDataConnectorProps<
  TData,
  TDataConnectorProps extends IDataConnectorProps<TData> & any
>(
  WrappedComponent: React.ComponentType<IDataConsumerProps<TData> & any>,
) {
  return class extends React.Component<TDataConnectorProps, IState<TData>> {
    constructor(props: TDataConnectorProps) {
      super(props);
      this.state = {};
    }
    public async componentDidMount(): Promise<void> {
      const data = await this.props.connector.fetch();
      this.setState({ ...this.state, data });
    }
    public render(): React.ReactNode {
      return this.state.data
        ? <WrappedComponent {...this.props} data={this.state.data} />
        : <Text>Loading data...</Text>
    }
  };
}

/**
 * Connector HoC (function argument)
 */
export function withDataConnector<TData, Props extends { data: TData }>(
  connector: IDataConnector<TData>,
  WrappedComponent: React.ComponentType<Props>
) {
  return class extends React.Component<Omit<Props, "data">, IState<TData>> {
    constructor(props: Omit<Props, "data">) {
      super(props);
      this.state = {};
    }
    public async componentDidMount(): Promise<void> {
      const data = await connector.fetch();
      this.setState({ ...this.state, data });
    }
    public render(): React.ReactNode {
      return this.state.data
        ? <WrappedComponent  {...this.props as Props} data={this.state.data} />
        : <Text>Loading data...</Text>
    }
  };
}
