export interface IMediaLocation {
  id: string;
  name: string;
}

/**
 * A directory content (files + folders).
 */
export interface IDirectoryContent {

  /**
   * The folders in the directory.
   */
  folders: string[];

  /**
   * The files in the directory.
   */
  files: string[];
}

export interface IMediaExplorerModule {

  /**
   * Returns all known media locations.
   */
  getMediaLocations(): Promise<IMediaLocation[]>;

  /**
   * Returns the root content of the media location.
   */
  getRoot(mediaLocationId: string): Promise<IDirectoryContent>;
}

export class MockMediaExplorerModule implements IMediaExplorerModule {

  private readonly _mediaLocations: IMediaLocation[] = [
    { id: 'location1', name: 'location1' },
    { id: 'location2', name: 'location2' },
    { id: 'location3', name: 'location3' },
  ];

  private readonly _contents: { [idx: string]: IDirectoryContent } = {
    'location1': {
      folders: [
        'Folder 1',
        'Folder 2',
        'Folder 3',
        'Folder 4',
      ],
      files: [
        'File 1',
        'File 2',
        'File 3',
        'File 4',
        'File 5',
      ]
    },
    'location2': {
      folders: [
        'Folder 11',
        'Folder 12',
        'Folder 13',
        'Folder 14',
      ],
      files: [
        'File 11',
        'File 12',
        'File 13',
        'File 14',
        'File 15',
      ]
    },
    'location3': {
      folders: [
        'Folder 11',
        'Folder 12',
        'Folder 13',
        'Folder 14',
      ],
      files: [
        'File 11',
        'File 12',
        'File 13',
        'File 14',
        'File 15',
      ]
    },
  }

  public getMediaLocations(): Promise<IMediaLocation[]> {
    return Promise.resolve(this._mediaLocations);
  }

  public getRoot(mediaLocationId: string): Promise<IDirectoryContent> {
    return Promise.resolve(this._contents[mediaLocationId]);
  }
}

