// Decompiled with JetBrains decompiler
// Type: HIDTester.HidLib
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

using HidLibrary;
using System.Collections.Generic;
using System.Linq;

namespace HIDTester;

internal class HidLib
{
  private bool Dev_Sta;
  private List<HidDevice> DeviceList = new List<HidDevice>();
  private HidDevice wDevice;

  public bool Get_Dev_Sta() => this.Dev_Sta;

  public bool Connect_Device()
  {
    this.wDevice = HidDevices.Enumerate(4489).FirstOrDefault<HidDevice>();
    if (this.wDevice != null)
    {
      foreach (HidDevice hidDevice in HidDevices.Enumerate(4489).ToList<HidDevice>())
      {
        if (hidDevice.DevicePath.IndexOf("mi_01") != -1)
        {
          this.DeviceList.Add(hidDevice);
          this.wDevice = hidDevice;
          this.wDevice.OpenDevice();
          this.wDevice.MonitorDeviceEvents = true;
          this.Dev_Sta = true;
          return true;
        }
      }
    }
    return false;
  }

  public bool Check_Disconnect()
  {
    if (this.wDevice.IsConnected)
      return true;
    this.wDevice.CloseDevice();
    this.Dev_Sta = false;
    return false;
  }

  public bool WriteDevice(byte id, byte[] arrayBuff)
  {
    HidReport report = this.wDevice.CreateReport();
    report.ReportId = id;
    report.Data[0] = arrayBuff[0];
    report.Data[1] = arrayBuff[1];
    report.Data[2] = arrayBuff[2];
    report.Data[3] = arrayBuff[3];
    report.Data[4] = arrayBuff[4];
    report.Data[5] = arrayBuff[5];
    report.Data[6] = arrayBuff[6];
    report.Data[7] = arrayBuff[7];
    return this.wDevice.WriteReport(report, 500);
  }
}
