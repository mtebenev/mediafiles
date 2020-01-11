using LibVLCSharp.Platforms.UWP;
using LibVLCSharp.Shared;
using System;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ArtefactUiUwpLib
{
  public sealed class VlcControl : Control
  {
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    public VideoView videoView;

    public static DependencyProperty LabelProperty { get; private set; }

    public string Label
    {
      get
      {
        return (string)GetValue(LabelProperty);
      }
      set
      {
        SetValue(LabelProperty, value);
      }
    }

    static VlcControl()
    {
      LabelProperty = DependencyProperty.Register(
          nameof(Label),
          typeof(string),
          typeof(VlcControl),
          new PropertyMetadata(default(string))
          );
    }

    public VlcControl()
    {
      DefaultStyleKey = typeof(VlcControl);
    }

    internal void InitPlayer()
    {
      Core.Initialize();
      videoView = (VideoView)GetTemplateChild("videoView");
      _libVLC = new LibVLC(videoView.SwapChainOptions);
      _mediaPlayer = new MediaPlayer(_libVLC);
      videoView.MediaPlayer = _mediaPlayer;

      OpenMedia();
    }

    private async void OpenMedia()
    {
      // Open media item
      FileOpenPicker openPicker = new FileOpenPicker();
      openPicker.ViewMode = PickerViewMode.Thumbnail;
      openPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
      openPicker.FileTypeFilter.Add(".avi");
      openPicker.FileTypeFilter.Add(".flv");
      openPicker.FileTypeFilter.Add(".wmv");

      StorageFile file = await openPicker.PickSingleFileAsync();

      var fileStream = await file.OpenReadAsync();
      var readStream = fileStream.AsStreamForRead();
      var media = new Media(_libVLC, readStream);
      _mediaPlayer.Play(media);

    }
  }
}
