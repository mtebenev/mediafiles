export interface IMediaLocation {
  name: string;
  path: string;
}

/**
 * File or folder.
 */
export interface IFileSystemItem {
  name: string;
  isDirectory: boolean;
  path: string;
}

/**
 * A directory content (files + folders).
 */
export interface IDirectoryContent {
  items: IFileSystemItem[];
}

export interface IMediaExplorerModule {

  /**
   * Adds another location.
   */
  addLocation(): Promise<void>;

  /**
   * Returns all known media locations.
   */
  getMediaLocations(): Promise<IMediaLocation[]>;

  /**
   * Returns the root content of the media location.
   */
  getRoot(mediaLocationName: string): Promise<IDirectoryContent>;
}

export class MockMediaExplorerModule implements IMediaExplorerModule {

  private readonly _mediaLocations: IMediaLocation[] = [
    { path: 'location1', name: 'location1' },
    //{ id: 'location2', name: 'location2' },
    //{ id: 'location3', name: 'location3' },
  ];

  private readonly _roots: { [idx: string]: IDirectoryContent } = {
    'location1': {
      items: [
        { name: 'file 1', isDirectory: false, path: '/file1' },
        { name: 'file 2', isDirectory: false, path: '/file2' },
        { name: 'file 3', isDirectory: false, path: '/file3'},
        { name: 'folder 1', isDirectory: true, path: '/folder1' },
        { name: 'folder 2', isDirectory: true, path: '/folder2' },
        { name: 'folder 3', isDirectory: true, path: '/folder3' },
      ]
    },
    /*
    'location2': {
      items: [
        { name: 'file 21', isDirectory: false },
        { name: 'file 22', isDirectory: false },
        { name: 'file 23', isDirectory: false },
        { name: 'folder 21', isDirectory: true },
        { name: 'folder 22', isDirectory: true },
        { name: 'folder 23', isDirectory: true },
      ]
    },
    'location3': {
      items: [
        { name: 'file 31', isDirectory: false },
        { name: 'file 32', isDirectory: false },
        { name: 'file 33', isDirectory: false },
        { name: 'folder 31', isDirectory: true },
        { name: 'folder 32', isDirectory: true },
        { name: 'folder 33', isDirectory: true },
      ]
    },
    */
  };

  public addLocation(): Promise<void> {
    return Promise.resolve();
  }

  public getMediaLocations(): Promise<IMediaLocation[]> {
    return Promise.resolve(this._mediaLocations);
  }

  public getRoot(mediaLocationId: string): Promise<IDirectoryContent> {
    return Promise.resolve(this._roots[mediaLocationId]);
  }
}

