using System;
using System.ComponentModel;

namespace Mt.MediaMan.ArtefactUi.Uwp.Core
{
  public class MmAppContext : INotifyPropertyChanged
  {
    private MediaItem _currentItem;

    public event PropertyChangedEventHandler PropertyChanged;

    public MediaItem CurrentItem
    {
      get
      {
        return _currentItem;
      }
      set
      {
        this._currentItem = value;
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentItem)));
      }
    }
  }
}
