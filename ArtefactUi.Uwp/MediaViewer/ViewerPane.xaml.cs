using System;
using System.IO;
using LibVLCSharp.Shared;
using Mt.MediaMan.ArtefactUi.Uwp.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mt.MediaMan.ArtefactUi.Uwp.MediaViewer
{
  public sealed partial class ViewerPane : UserControl
  {
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;

    private static readonly DependencyProperty _dpMediaItem =
    DependencyProperty.Register(nameof(MediaItem), typeof(MediaItem), typeof(ViewerPane), new PropertyMetadata(0));

    public ViewerPane()
    {
      this.InitializeComponent();
      Loaded += (s, e) =>
      {
        _libVLC = new LibVLC(this.videoView.SwapChainOptions);
        _mediaPlayer = new MediaPlayer(_libVLC);
        this.videoView.MediaPlayer = _mediaPlayer;
      };

      Unloaded += (s, e) =>
      {
        this.videoView.MediaPlayer = null;
        this._mediaPlayer.Dispose();
        this._libVLC.Dispose();
      };
    }

    // MediaItem DP
    public static DependencyProperty MediaItemProperty => _dpMediaItem;
    public MediaItem MediaItem
    {
      get => (MediaItem)GetValue(MediaItemProperty);
      set
      {
        SetValue(MediaItemProperty, value);
        this.UpdateMedia(value);
      }
    }

    /// <summary>
    /// Opens the current media item.
    /// </summary>
    private async void UpdateMedia(MediaItem mediaItem)
    {
      if(mediaItem != null)
      {
        var fileStream = await mediaItem.StorageFile.OpenReadAsync();
        var readStream = fileStream.AsStreamForRead();
        var media = new Media(_libVLC, readStream);
        this._mediaPlayer.Play(media);
      }
    }

  }
}
