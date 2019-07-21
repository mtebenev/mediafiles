import {NativeModules} from 'react-native';

/**
 * Provides artefact exploration data.
 */
export class ExplorerService {

  public getItems(): Promise<string[]> {
    return new Promise((resolve, reject) => {
      NativeModules.AppModule.getItems((result: string[]) => {
        resolve(result);
      });
    });
  }
}
