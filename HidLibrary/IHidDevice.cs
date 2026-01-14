// Decompiled with JetBrains decompiler
// Type: HidLibrary.IHidDevice
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

using System;
using System.Threading.Tasks;

#nullable disable
namespace HidLibrary;

public interface IHidDevice : IDisposable
{
  event InsertedEventHandler Inserted;

  event RemovedEventHandler Removed;

  IntPtr Handle { get; }

  bool IsOpen { get; }

  bool IsConnected { get; }

  string Description { get; }

  HidDeviceCapabilities Capabilities { get; }

  HidDeviceAttributes Attributes { get; }

  string DevicePath { get; }

  bool MonitorDeviceEvents { get; set; }

  void OpenDevice();

  void OpenDevice(DeviceMode readMode, DeviceMode writeMode, ShareMode shareMode);

  void CloseDevice();

  HidDeviceData Read();

  void Read(ReadCallback callback);

  void Read(ReadCallback callback, int timeout);

  Task<HidDeviceData> ReadAsync(int timeout = 0);

  HidDeviceData Read(int timeout);

  void ReadReport(ReadReportCallback callback);

  void ReadReport(ReadReportCallback callback, int timeout);

  Task<HidReport> ReadReportAsync(int timeout = 0);

  HidReport ReadReport(int timeout);

  HidReport ReadReport();

  bool ReadFeatureData(out byte[] data, byte reportId = 0);

  bool ReadProduct(out byte[] data);

  bool ReadManufacturer(out byte[] data);

  bool ReadSerialNumber(out byte[] data);

  void Write(byte[] data, WriteCallback callback);

  bool Write(byte[] data);

  bool Write(byte[] data, int timeout);

  void Write(byte[] data, WriteCallback callback, int timeout);

  Task<bool> WriteAsync(byte[] data, int timeout = 0);

  void WriteReport(HidReport report, WriteCallback callback);

  bool WriteReport(HidReport report);

  bool WriteReport(HidReport report, int timeout);

  void WriteReport(HidReport report, WriteCallback callback, int timeout);

  Task<bool> WriteReportAsync(HidReport report, int timeout = 0);

  HidReport CreateReport();

  bool WriteFeatureData(byte[] data);
}
