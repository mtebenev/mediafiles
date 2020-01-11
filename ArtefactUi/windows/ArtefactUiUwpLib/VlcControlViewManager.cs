using System.Diagnostics;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

using Microsoft.ReactNative.Managed;

namespace ArtefactUiUwpLib
{
  internal class VlcControlViewManager : AttributedViewManager<VlcControl>
  {
    public override FrameworkElement CreateView()
    {
      var view = new VlcControl();
      view.RegisterPropertyChangedCallback(VlcControl.LabelProperty, (obj, prop) =>
      {
        if(obj is VlcControl c)
        {
          LabelChanged(c, c.Label);
        }
      });

      return view;
    }

    [ViewManagerProperty("label")]
    public void SetLabel(VlcControl view, string value)
    {
      if(null != value)
      {
        view.SetValue(VlcControl.LabelProperty, value);
      }
      else
      {
        view.ClearValue(VlcControl.LabelProperty);
      }
    }

    [ViewManagerProperty("color")]
    public void SetColor(VlcControl view, Brush value)
    {
      if(null != value)
      {
        view.SetValue(Control.ForegroundProperty, value);
      }
      else
      {
        view.ClearValue(Control.ForegroundProperty);
      }
    }

    [ViewManagerProperty("backgroundColor")]
    public void SetBackgroundColor(VlcControl view, Brush value)
    {
      if(null != value)
      {
        view.SetValue(Control.BackgroundProperty, value);
      }
      else
      {
        view.ClearValue(Control.BackgroundProperty);
      }
    }

    [ViewManagerCommand]
    public void CustomCommand(VlcControl view, string arg)
    {
      Debug.WriteLine($"{Name}.{nameof(CustomCommand)}({view.Tag}, \"{arg}\")");
      view.InitPlayer();

    }

    [ViewManagerExportedDirectEventTypeConstant]
    public ViewManagerEvent<VlcControl, string> LabelChanged;
  }
}
