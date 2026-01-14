using System;
using System.Threading;

#nullable disable
namespace HidLibrary;

internal class HidDeviceEventMonitor
{
  private readonly HidDevice _device;
  private bool _wasConnected;

  public event HidDeviceEventMonitor.InsertedEventHandler Inserted;

  public event HidDeviceEventMonitor.RemovedEventHandler Removed;

  public HidDeviceEventMonitor(HidDevice device) => this._device = device;

  public void Init()
  {
    Action action = new Action(this.DeviceEventMonitor);
    action.BeginInvoke(new AsyncCallback(HidDeviceEventMonitor.DisposeDeviceEventMonitor), (object) action);
  }

  private void DeviceEventMonitor()
  {
    bool isConnected = this._device.IsConnected;
    if (isConnected != this._wasConnected)
    {
      if (isConnected && this.Inserted != null)
        this.Inserted();
      else if (!isConnected && this.Removed != null)
        this.Removed();
      this._wasConnected = isConnected;
    }
    Thread.Sleep(500);
    if (!this._device.MonitorDeviceEvents)
      return;
    this.Init();
  }

  private static void DisposeDeviceEventMonitor(IAsyncResult ar)
  {
    ((Action) ar.AsyncState).EndInvoke(ar);
  }

  public delegate void InsertedEventHandler();

  public delegate void RemovedEventHandler();
}
