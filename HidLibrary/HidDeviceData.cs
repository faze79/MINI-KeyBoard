// Decompiled with JetBrains decompiler
// Type: HidLibrary.HidDeviceData
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

#nullable disable
namespace HidLibrary;

public class HidDeviceData
{
  public HidDeviceData(HidDeviceData.ReadStatus status)
  {
    this.Data = new byte[0];
    this.Status = status;
  }

  public HidDeviceData(byte[] data, HidDeviceData.ReadStatus status)
  {
    this.Data = data;
    this.Status = status;
  }

  public byte[] Data { get; private set; }

  public HidDeviceData.ReadStatus Status { get; private set; }

  public enum ReadStatus
  {
    Success,
    WaitTimedOut,
    WaitFail,
    NoDataRead,
    ReadError,
    NotConnected,
  }
}
