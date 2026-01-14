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

    // Log the HID write for debugging
    FormMain.LogHidWrite(id, report.GetBytes(), $"Key={arrayBuff[0]:X2} Type={arrayBuff[1]:X2}");

    return this.wDevice.WriteReport(report, 500);
  }

  // Device info methods
  public HidDeviceCapabilities GetCapabilities()
  {
    return this.wDevice?.Capabilities;
  }

  public HidDeviceAttributes GetAttributes()
  {
    return this.wDevice?.Attributes;
  }

  public string GetDevicePath()
  {
    return this.wDevice?.DevicePath;
  }

  public string GetProductString()
  {
    if (this.wDevice == null) return null;
    if (this.wDevice.ReadProduct(out byte[] data))
    {
      return System.Text.Encoding.Unicode.GetString(data).TrimEnd('\0');
    }
    return null;
  }

  public string GetManufacturerString()
  {
    if (this.wDevice == null) return null;
    if (this.wDevice.ReadManufacturer(out byte[] data))
    {
      return System.Text.Encoding.Unicode.GetString(data).TrimEnd('\0');
    }
    return null;
  }

  public string GetSerialNumber()
  {
    if (this.wDevice == null) return null;
    if (this.wDevice.ReadSerialNumber(out byte[] data))
    {
      return System.Text.Encoding.Unicode.GetString(data).TrimEnd('\0');
    }
    return null;
  }

  public bool ReadFeatureReport(byte reportId, out byte[] data)
  {
    data = null;
    if (this.wDevice == null) return false;
    return this.wDevice.ReadFeatureData(out data, reportId);
  }

  public bool WriteFeatureReport(byte[] data)
  {
    if (this.wDevice == null) return false;
    return this.wDevice.WriteFeatureData(data);
  }
}
