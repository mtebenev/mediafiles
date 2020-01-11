// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using LibVLCSharp.Platforms.UWP;
using LibVLCSharp.Shared;
using System;
using System.IO;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExtLib
{
  public sealed class CustomUserControlCS : Control
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

    static CustomUserControlCS()
    {
      LabelProperty = DependencyProperty.Register(
          nameof(Label),
          typeof(string),
          typeof(CustomUserControlCS),
          new PropertyMetadata(default(string))
          );
    }

    public CustomUserControlCS()
    {
      DefaultStyleKey = typeof(CustomUserControlCS);
    }

    internal void InitPlayer()
    {
      Core.Initialize();
      this.videoView = (VideoView) this.GetTemplateChild("videoView");
      _libVLC = new LibVLC(this.videoView.SwapChainOptions);
      _mediaPlayer = new MediaPlayer(_libVLC);
      this.videoView.MediaPlayer = _mediaPlayer;

      this.OpenMedia();
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
      this._mediaPlayer.Play(media);

    }
  }
}
