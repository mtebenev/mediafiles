using System;
using Windows.UI.Xaml.Controls;
using LibVLCSharp.Shared;
using System.IO;
using Mt.MediaMan.ArtefactUi.Uwp.Core;
using Windows.UI.Xaml;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Mt.MediaMan.ArtefactUi.Uwp.Components
{
  public sealed partial class MediaViewer : UserControl
  {
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;

    private static readonly DependencyProperty _dpMediaItem =
    DependencyProperty.Register(nameof(MediaItem), typeof(MediaItem), typeof(MediaViewer), new PropertyMetadata(0));

    public MediaViewer()
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
