// Decompiled with JetBrains decompiler
// Type: HidLibrary.HidFastReadDevice
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

using System;
using System.Threading.Tasks;

#nullable disable
namespace HidLibrary;

public class HidFastReadDevice : HidDevice
{
  internal HidFastReadDevice(string devicePath, string description = null)
    : base(devicePath, description)
  {
  }

  public HidDeviceData FastRead() => this.FastRead(0);

  public HidDeviceData FastRead(int timeout)
  {
    try
    {
      return this.ReadData(timeout);
    }
    catch
    {
      return new HidDeviceData(HidDeviceData.ReadStatus.ReadError);
    }
  }

  public void FastRead(ReadCallback callback) => this.FastRead(callback, 0);

  public void FastRead(ReadCallback callback, int timeout)
  {
    HidDevice.ReadDelegate callerDelegate = new HidDevice.ReadDelegate(this.FastRead);
    HidAsyncState hidAsyncState = new HidAsyncState((object) callerDelegate, (object) callback);
    callerDelegate.BeginInvoke(timeout, new AsyncCallback(HidDevice.EndRead), (object) hidAsyncState);
  }

  public async Task<HidDeviceData> FastReadAsync(int timeout = 0)
  {
    HidDevice.ReadDelegate readDelegate = new HidDevice.ReadDelegate(this.FastRead);
    HidDeviceData hidDeviceData = await Task<HidDeviceData>.Factory.FromAsync<int>(new Func<int, AsyncCallback, object, IAsyncResult>(readDelegate.BeginInvoke), new Func<IAsyncResult, HidDeviceData>(readDelegate.EndInvoke), timeout, (object) null);
    readDelegate = (HidDevice.ReadDelegate) null;
    return hidDeviceData;
  }

  public HidReport FastReadReport() => this.FastReadReport(0);

  public HidReport FastReadReport(int timeout)
  {
    return new HidReport((int) this.Capabilities.InputReportByteLength, this.FastRead(timeout));
  }

  public void FastReadReport(ReadReportCallback callback) => this.FastReadReport(callback, 0);

  public void FastReadReport(ReadReportCallback callback, int timeout)
  {
    HidDevice.ReadReportDelegate callerDelegate = new HidDevice.ReadReportDelegate(this.FastReadReport);
    HidAsyncState hidAsyncState = new HidAsyncState((object) callerDelegate, (object) callback);
    callerDelegate.BeginInvoke(timeout, new AsyncCallback(HidDevice.EndReadReport), (object) hidAsyncState);
  }

  public async Task<HidReport> FastReadReportAsync(int timeout = 0)
  {
    HidDevice.ReadReportDelegate readReportDelegate = new HidDevice.ReadReportDelegate(this.FastReadReport);
    HidReport hidReport = await Task<HidReport>.Factory.FromAsync<int>(new Func<int, AsyncCallback, object, IAsyncResult>(readReportDelegate.BeginInvoke), new Func<IAsyncResult, HidReport>(readReportDelegate.EndInvoke), timeout, (object) null);
    readReportDelegate = (HidDevice.ReadReportDelegate) null;
    return hidReport;
  }
}
