// Decompiled with JetBrains decompiler
// Type: HidLibrary.HidDevice
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace HidLibrary;

public class HidDevice : IHidDevice, IDisposable
{
  private readonly string _description;
  private readonly string _devicePath;
  private readonly HidDeviceAttributes _deviceAttributes;
  private readonly HidDeviceCapabilities _deviceCapabilities;
  private DeviceMode _deviceReadMode = DeviceMode.NonOverlapped;
  private DeviceMode _deviceWriteMode = DeviceMode.NonOverlapped;
  private ShareMode _deviceShareMode = ShareMode.ShareRead | ShareMode.ShareWrite;
  private readonly HidDeviceEventMonitor _deviceEventMonitor;
  private bool _monitorDeviceEvents;

  public event InsertedEventHandler Inserted;

  public event RemovedEventHandler Removed;

  internal HidDevice(string devicePath, string description = null)
  {
    this._deviceEventMonitor = new HidDeviceEventMonitor(this);
    this._deviceEventMonitor.Inserted += new HidDeviceEventMonitor.InsertedEventHandler(this.DeviceEventMonitorInserted);
    this._deviceEventMonitor.Removed += new HidDeviceEventMonitor.RemovedEventHandler(this.DeviceEventMonitorRemoved);
    this._devicePath = devicePath;
    this._description = description;
    try
    {
      IntPtr num = HidDevice.OpenDeviceIO(this._devicePath, 0U);
      this._deviceAttributes = HidDevice.GetDeviceAttributes(num);
      this._deviceCapabilities = HidDevice.GetDeviceCapabilities(num);
      HidDevice.CloseDeviceIO(num);
    }
    catch (Exception ex)
    {
      throw new Exception($"Error querying HID device '{devicePath}'.", ex);
    }
  }

  public IntPtr Handle { get; private set; }

  public bool IsOpen { get; private set; }

  public bool IsConnected => HidDevices.IsConnected(this._devicePath);

  public string Description => this._description;

  public HidDeviceCapabilities Capabilities => this._deviceCapabilities;

  public HidDeviceAttributes Attributes => this._deviceAttributes;

  public string DevicePath => this._devicePath;

  public bool MonitorDeviceEvents
  {
    get => this._monitorDeviceEvents;
    set
    {
      if (value & !this._monitorDeviceEvents)
        this._deviceEventMonitor.Init();
      this._monitorDeviceEvents = value;
    }
  }

  public override string ToString()
  {
    return $"VendorID={this._deviceAttributes.VendorHexId}, ProductID={this._deviceAttributes.ProductHexId}, Version={this._deviceAttributes.Version}, DevicePath={this._devicePath}";
  }

  public void OpenDevice()
  {
    this.OpenDevice(DeviceMode.NonOverlapped, DeviceMode.NonOverlapped, ShareMode.ShareRead | ShareMode.ShareWrite);
  }

  public void OpenDevice(DeviceMode readMode, DeviceMode writeMode, ShareMode shareMode)
  {
    if (this.IsOpen)
      return;
    this._deviceReadMode = readMode;
    this._deviceWriteMode = writeMode;
    this._deviceShareMode = shareMode;
    try
    {
      this.Handle = HidDevice.OpenDeviceIO(this._devicePath, readMode, 3221225472U /*0xC0000000*/, shareMode);
    }
    catch (Exception ex)
    {
      this.IsOpen = false;
      throw new Exception("Error opening HID device.", ex);
    }
    this.IsOpen = this.Handle.ToInt32() != -1;
  }

  public void CloseDevice()
  {
    if (!this.IsOpen)
      return;
    HidDevice.CloseDeviceIO(this.Handle);
    this.IsOpen = false;
  }

  public HidDeviceData Read() => this.Read(0);

  public HidDeviceData Read(int timeout)
  {
    if (!this.IsConnected)
      return new HidDeviceData(HidDeviceData.ReadStatus.NotConnected);
    if (!this.IsOpen)
      this.OpenDevice(this._deviceReadMode, this._deviceWriteMode, this._deviceShareMode);
    try
    {
      return this.ReadData(timeout);
    }
    catch
    {
      return new HidDeviceData(HidDeviceData.ReadStatus.ReadError);
    }
  }

  public void Read(ReadCallback callback) => this.Read(callback, 0);

  public void Read(ReadCallback callback, int timeout)
  {
    HidDevice.ReadDelegate callerDelegate = new HidDevice.ReadDelegate(this.Read);
    HidAsyncState hidAsyncState = new HidAsyncState((object) callerDelegate, (object) callback);
    callerDelegate.BeginInvoke(timeout, new AsyncCallback(HidDevice.EndRead), (object) hidAsyncState);
  }

  public async Task<HidDeviceData> ReadAsync(int timeout = 0)
  {
    HidDevice.ReadDelegate readDelegate = new HidDevice.ReadDelegate(this.Read);
    HidDeviceData hidDeviceData = await Task<HidDeviceData>.Factory.FromAsync<int>(new Func<int, AsyncCallback, object, IAsyncResult>(readDelegate.BeginInvoke), new Func<IAsyncResult, HidDeviceData>(readDelegate.EndInvoke), timeout, (object) null);
    readDelegate = (HidDevice.ReadDelegate) null;
    return hidDeviceData;
  }

  public HidReport ReadReport() => this.ReadReport(0);

  public HidReport ReadReport(int timeout)
  {
    return new HidReport((int) this.Capabilities.InputReportByteLength, this.Read(timeout));
  }

  public void ReadReport(ReadReportCallback callback) => this.ReadReport(callback, 0);

  public void ReadReport(ReadReportCallback callback, int timeout)
  {
    HidDevice.ReadReportDelegate callerDelegate = new HidDevice.ReadReportDelegate(this.ReadReport);
    HidAsyncState hidAsyncState = new HidAsyncState((object) callerDelegate, (object) callback);
    callerDelegate.BeginInvoke(timeout, new AsyncCallback(HidDevice.EndReadReport), (object) hidAsyncState);
  }

  public async Task<HidReport> ReadReportAsync(int timeout = 0)
  {
    HidDevice.ReadReportDelegate readReportDelegate = new HidDevice.ReadReportDelegate(this.ReadReport);
    HidReport hidReport = await Task<HidReport>.Factory.FromAsync<int>(new Func<int, AsyncCallback, object, IAsyncResult>(readReportDelegate.BeginInvoke), new Func<IAsyncResult, HidReport>(readReportDelegate.EndInvoke), timeout, (object) null);
    readReportDelegate = (HidDevice.ReadReportDelegate) null;
    return hidReport;
  }

  public HidReport ReadReportSync(byte reportId)
  {
    byte[] numArray = new byte[(int) this.Capabilities.InputReportByteLength];
    numArray[0] = reportId;
    bool inputReport = NativeMethods.HidD_GetInputReport(this.Handle, numArray, numArray.Length);
    return new HidReport((int) this.Capabilities.InputReportByteLength, new HidDeviceData(numArray, inputReport ? HidDeviceData.ReadStatus.Success : HidDeviceData.ReadStatus.NoDataRead));
  }

  public bool ReadFeatureData(out byte[] data, byte reportId = 0)
  {
    if (this._deviceCapabilities.FeatureReportByteLength <= (short) 0)
    {
      data = new byte[0];
      return false;
    }
    data = new byte[(int) this._deviceCapabilities.FeatureReportByteLength];
    byte[] featureOutputBuffer = this.CreateFeatureOutputBuffer();
    featureOutputBuffer[0] = reportId;
    IntPtr num = IntPtr.Zero;
    bool flag = false;
    try
    {
      num = !this.IsOpen ? HidDevice.OpenDeviceIO(this._devicePath, 0U) : this.Handle;
      flag = NativeMethods.HidD_GetFeature(num, featureOutputBuffer, featureOutputBuffer.Length);
      if (flag)
        Array.Copy((Array) featureOutputBuffer, 0, (Array) data, 0, Math.Min(data.Length, (int) this._deviceCapabilities.FeatureReportByteLength));
    }
    catch (Exception ex)
    {
      throw new Exception($"Error accessing HID device '{this._devicePath}'.", ex);
    }
    finally
    {
      if (num != IntPtr.Zero && num != this.Handle)
        HidDevice.CloseDeviceIO(num);
    }
    return flag;
  }

  public bool ReadProduct(out byte[] data)
  {
    data = new byte[254];
    IntPtr num = IntPtr.Zero;
    bool flag = false;
    try
    {
      num = !this.IsOpen ? HidDevice.OpenDeviceIO(this._devicePath, 0U) : this.Handle;
      flag = NativeMethods.HidD_GetProductString(num, ref data[0], data.Length);
    }
    catch (Exception ex)
    {
      throw new Exception($"Error accessing HID device '{this._devicePath}'.", ex);
    }
    finally
    {
      if (num != IntPtr.Zero && num != this.Handle)
        HidDevice.CloseDeviceIO(num);
    }
    return flag;
  }

  public bool ReadManufacturer(out byte[] data)
  {
    data = new byte[254];
    IntPtr num = IntPtr.Zero;
    bool flag = false;
    try
    {
      num = !this.IsOpen ? HidDevice.OpenDeviceIO(this._devicePath, 0U) : this.Handle;
      flag = NativeMethods.HidD_GetManufacturerString(num, ref data[0], data.Length);
    }
    catch (Exception ex)
    {
      throw new Exception($"Error accessing HID device '{this._devicePath}'.", ex);
    }
    finally
    {
      if (num != IntPtr.Zero && num != this.Handle)
        HidDevice.CloseDeviceIO(num);
    }
    return flag;
  }

  public bool ReadSerialNumber(out byte[] data)
  {
    data = new byte[254];
    IntPtr num = IntPtr.Zero;
    bool flag = false;
    try
    {
      num = !this.IsOpen ? HidDevice.OpenDeviceIO(this._devicePath, 0U) : this.Handle;
      flag = NativeMethods.HidD_GetSerialNumberString(num, ref data[0], data.Length);
    }
    catch (Exception ex)
    {
      throw new Exception($"Error accessing HID device '{this._devicePath}'.", ex);
    }
    finally
    {
      if (num != IntPtr.Zero && num != this.Handle)
        HidDevice.CloseDeviceIO(num);
    }
    return flag;
  }

  public bool Write(byte[] data) => this.Write(data, 0);

  public bool Write(byte[] data, int timeout)
  {
    if (!this.IsConnected)
      return false;
    if (!this.IsOpen)
      this.OpenDevice(this._deviceReadMode, this._deviceWriteMode, this._deviceShareMode);
    try
    {
      return this.WriteData(data, timeout);
    }
    catch
    {
      return false;
    }
  }

  public void Write(byte[] data, WriteCallback callback) => this.Write(data, callback, 0);

  public void Write(byte[] data, WriteCallback callback, int timeout)
  {
    HidDevice.WriteDelegate callerDelegate = new HidDevice.WriteDelegate(this.Write);
    HidAsyncState hidAsyncState = new HidAsyncState((object) callerDelegate, (object) callback);
    callerDelegate.BeginInvoke(data, timeout, new AsyncCallback(HidDevice.EndWrite), (object) hidAsyncState);
  }

  public async Task<bool> WriteAsync(byte[] data, int timeout = 0)
  {
    HidDevice.WriteDelegate writeDelegate = new HidDevice.WriteDelegate(this.Write);
    bool flag = await Task<bool>.Factory.FromAsync<byte[], int>(new Func<byte[], int, AsyncCallback, object, IAsyncResult>(writeDelegate.BeginInvoke), new Func<IAsyncResult, bool>(writeDelegate.EndInvoke), data, timeout, (object) null);
    writeDelegate = (HidDevice.WriteDelegate) null;
    return flag;
  }

  public bool WriteReport(HidReport report) => this.WriteReport(report, 0);

  public bool WriteReport(HidReport report, int timeout) => this.Write(report.GetBytes(), timeout);

  public void WriteReport(HidReport report, WriteCallback callback)
  {
    this.WriteReport(report, callback, 0);
  }

  public void WriteReport(HidReport report, WriteCallback callback, int timeout)
  {
    HidDevice.WriteReportDelegate callerDelegate = new HidDevice.WriteReportDelegate(this.WriteReport);
    HidAsyncState hidAsyncState = new HidAsyncState((object) callerDelegate, (object) callback);
    callerDelegate.BeginInvoke(report, timeout, new AsyncCallback(HidDevice.EndWriteReport), (object) hidAsyncState);
  }

  public bool WriteReportSync(HidReport report)
  {
    byte[] lpReportBuffer = report != null ? report.GetBytes() : throw new ArgumentException("The output report is null, it must be allocated before you call this method", nameof (report));
    return NativeMethods.HidD_SetOutputReport(this.Handle, lpReportBuffer, lpReportBuffer.Length);
  }

  public async Task<bool> WriteReportAsync(HidReport report, int timeout = 0)
  {
    HidDevice.WriteReportDelegate writeReportDelegate = new HidDevice.WriteReportDelegate(this.WriteReport);
    bool flag = await Task<bool>.Factory.FromAsync<HidReport, int>(new Func<HidReport, int, AsyncCallback, object, IAsyncResult>(writeReportDelegate.BeginInvoke), new Func<IAsyncResult, bool>(writeReportDelegate.EndInvoke), report, timeout, (object) null);
    writeReportDelegate = (HidDevice.WriteReportDelegate) null;
    return flag;
  }

  public HidReport CreateReport() => new HidReport((int) this.Capabilities.OutputReportByteLength);

  public bool WriteFeatureData(byte[] data)
  {
    if (this._deviceCapabilities.FeatureReportByteLength <= (short) 0)
      return false;
    byte[] featureOutputBuffer = this.CreateFeatureOutputBuffer();
    Array.Copy((Array) data, 0, (Array) featureOutputBuffer, 0, Math.Min(data.Length, (int) this._deviceCapabilities.FeatureReportByteLength));
    IntPtr num = IntPtr.Zero;
    bool flag = false;
    try
    {
      num = !this.IsOpen ? HidDevice.OpenDeviceIO(this._devicePath, 0U) : this.Handle;
      flag = NativeMethods.HidD_SetFeature(num, featureOutputBuffer, featureOutputBuffer.Length);
    }
    catch (Exception ex)
    {
      throw new Exception($"Error accessing HID device '{this._devicePath}'.", ex);
    }
    finally
    {
      if (num != IntPtr.Zero && num != this.Handle)
        HidDevice.CloseDeviceIO(num);
    }
    return flag;
  }

  protected static void EndRead(IAsyncResult ar)
  {
    HidAsyncState asyncState = (HidAsyncState) ar.AsyncState;
    HidDevice.ReadDelegate callerDelegate = (HidDevice.ReadDelegate) asyncState.CallerDelegate;
    ReadCallback callbackDelegate = (ReadCallback) asyncState.CallbackDelegate;
    HidDeviceData data = callerDelegate.EndInvoke(ar);
    if (callbackDelegate == null)
      return;
    callbackDelegate(data);
  }

  protected static void EndReadReport(IAsyncResult ar)
  {
    HidAsyncState asyncState = (HidAsyncState) ar.AsyncState;
    HidDevice.ReadReportDelegate callerDelegate = (HidDevice.ReadReportDelegate) asyncState.CallerDelegate;
    ReadReportCallback callbackDelegate = (ReadReportCallback) asyncState.CallbackDelegate;
    HidReport report = callerDelegate.EndInvoke(ar);
    if (callbackDelegate == null)
      return;
    callbackDelegate(report);
  }

  private static void EndWrite(IAsyncResult ar)
  {
    HidAsyncState asyncState = (HidAsyncState) ar.AsyncState;
    HidDevice.WriteDelegate callerDelegate = (HidDevice.WriteDelegate) asyncState.CallerDelegate;
    WriteCallback callbackDelegate = (WriteCallback) asyncState.CallbackDelegate;
    bool success = callerDelegate.EndInvoke(ar);
    if (callbackDelegate == null)
      return;
    callbackDelegate(success);
  }

  private static void EndWriteReport(IAsyncResult ar)
  {
    HidAsyncState asyncState = (HidAsyncState) ar.AsyncState;
    HidDevice.WriteReportDelegate callerDelegate = (HidDevice.WriteReportDelegate) asyncState.CallerDelegate;
    WriteCallback callbackDelegate = (WriteCallback) asyncState.CallbackDelegate;
    bool success = callerDelegate.EndInvoke(ar);
    if (callbackDelegate == null)
      return;
    callbackDelegate(success);
  }

  private byte[] CreateInputBuffer()
  {
    return HidDevice.CreateBuffer((int) this.Capabilities.InputReportByteLength - 1);
  }

  private byte[] CreateOutputBuffer()
  {
    return HidDevice.CreateBuffer((int) this.Capabilities.OutputReportByteLength - 1);
  }

  private byte[] CreateFeatureOutputBuffer()
  {
    return HidDevice.CreateBuffer((int) this.Capabilities.FeatureReportByteLength - 1);
  }

  private static byte[] CreateBuffer(int length)
  {
    byte[] array = (byte[]) null;
    Array.Resize<byte>(ref array, length + 1);
    return array;
  }

  private static HidDeviceAttributes GetDeviceAttributes(IntPtr hidHandle)
  {
    NativeMethods.HIDD_ATTRIBUTES attributes = new NativeMethods.HIDD_ATTRIBUTES();
    attributes.Size = Marshal.SizeOf((object) attributes);
    NativeMethods.HidD_GetAttributes(hidHandle, ref attributes);
    return new HidDeviceAttributes(attributes);
  }

  private static HidDeviceCapabilities GetDeviceCapabilities(IntPtr hidHandle)
  {
    NativeMethods.HIDP_CAPS capabilities = new NativeMethods.HIDP_CAPS();
    IntPtr preparsedData = new IntPtr();
    if (NativeMethods.HidD_GetPreparsedData(hidHandle, ref preparsedData))
    {
      NativeMethods.HidP_GetCaps(preparsedData, ref capabilities);
      NativeMethods.HidD_FreePreparsedData(preparsedData);
    }
    return new HidDeviceCapabilities(capabilities);
  }

  private bool WriteData(byte[] data, int timeout)
  {
    if (this._deviceCapabilities.OutputReportByteLength <= (short) 0)
      return false;
    byte[] outputBuffer = this.CreateOutputBuffer();
    uint lpNumberOfBytesWritten = 0;
    Array.Copy((Array) data, 0, (Array) outputBuffer, 0, Math.Min(data.Length, (int) this._deviceCapabilities.OutputReportByteLength));
    if (this._deviceWriteMode == DeviceMode.Overlapped)
    {
      NativeMethods.SECURITY_ATTRIBUTES securityAttributes = new NativeMethods.SECURITY_ATTRIBUTES();
      NativeOverlapped lpOverlapped = new NativeOverlapped();
      int dwMilliseconds = timeout <= 0 ? -1 : timeout;
      securityAttributes.lpSecurityDescriptor = IntPtr.Zero;
      securityAttributes.bInheritHandle = true;
      securityAttributes.nLength = Marshal.SizeOf((object) securityAttributes);
      lpOverlapped.OffsetLow = 0;
      lpOverlapped.OffsetHigh = 0;
      lpOverlapped.EventHandle = NativeMethods.CreateEvent(ref securityAttributes, Convert.ToInt32(false), Convert.ToInt32(true), "");
      try
      {
        NativeMethods.WriteFile(this.Handle, outputBuffer, (uint) outputBuffer.Length, out lpNumberOfBytesWritten, ref lpOverlapped);
      }
      catch
      {
        return false;
      }
      switch (NativeMethods.WaitForSingleObject(lpOverlapped.EventHandle, dwMilliseconds))
      {
        case 0:
          return true;
        case 258:
          return false;
        case uint.MaxValue:
          return false;
        default:
          return false;
      }
    }
    else
    {
      try
      {
        NativeOverlapped lpOverlapped = new NativeOverlapped();
        return NativeMethods.WriteFile(this.Handle, outputBuffer, (uint) outputBuffer.Length, out lpNumberOfBytesWritten, ref lpOverlapped);
      }
      catch
      {
        return false;
      }
    }
  }

  protected HidDeviceData ReadData(int timeout)
  {
    byte[] numArray = new byte[0];
    HidDeviceData.ReadStatus status = HidDeviceData.ReadStatus.NoDataRead;
    if (this._deviceCapabilities.InputReportByteLength > (short) 0)
    {
      uint length = 0;
      numArray = this.CreateInputBuffer();
      IntPtr num = Marshal.AllocHGlobal(numArray.Length);
      if (this._deviceReadMode == DeviceMode.Overlapped)
      {
        NativeMethods.SECURITY_ATTRIBUTES securityAttributes = new NativeMethods.SECURITY_ATTRIBUTES();
        NativeOverlapped lpOverlapped = new NativeOverlapped();
        int dwMilliseconds = timeout <= 0 ? -1 : timeout;
        securityAttributes.lpSecurityDescriptor = IntPtr.Zero;
        securityAttributes.bInheritHandle = true;
        securityAttributes.nLength = Marshal.SizeOf((object) securityAttributes);
        lpOverlapped.OffsetLow = 0;
        lpOverlapped.OffsetHigh = 0;
        lpOverlapped.EventHandle = NativeMethods.CreateEvent(ref securityAttributes, Convert.ToInt32(false), Convert.ToInt32(true), string.Empty);
        try
        {
          if (NativeMethods.ReadFile(this.Handle, num, (uint) numArray.Length, out length, ref lpOverlapped))
          {
            status = HidDeviceData.ReadStatus.Success;
          }
          else
          {
            switch (NativeMethods.WaitForSingleObject(lpOverlapped.EventHandle, dwMilliseconds))
            {
              case 0:
                status = HidDeviceData.ReadStatus.Success;
                NativeMethods.GetOverlappedResult(this.Handle, ref lpOverlapped, out length, false);
                break;
              case 258:
                status = HidDeviceData.ReadStatus.WaitTimedOut;
                numArray = new byte[0];
                break;
              case uint.MaxValue:
                status = HidDeviceData.ReadStatus.WaitFail;
                numArray = new byte[0];
                break;
              default:
                status = HidDeviceData.ReadStatus.NoDataRead;
                numArray = new byte[0];
                break;
            }
          }
          Marshal.Copy(num, numArray, 0, (int) length);
        }
        catch
        {
          status = HidDeviceData.ReadStatus.ReadError;
        }
        finally
        {
          HidDevice.CloseDeviceIO(lpOverlapped.EventHandle);
          Marshal.FreeHGlobal(num);
        }
      }
      else
      {
        try
        {
          NativeOverlapped lpOverlapped = new NativeOverlapped();
          NativeMethods.ReadFile(this.Handle, num, (uint) numArray.Length, out length, ref lpOverlapped);
          status = HidDeviceData.ReadStatus.Success;
          Marshal.Copy(num, numArray, 0, (int) length);
        }
        catch
        {
          status = HidDeviceData.ReadStatus.ReadError;
        }
        finally
        {
          Marshal.FreeHGlobal(num);
        }
      }
    }
    return new HidDeviceData(numArray, status);
  }

  private static IntPtr OpenDeviceIO(string devicePath, uint deviceAccess)
  {
    return HidDevice.OpenDeviceIO(devicePath, DeviceMode.NonOverlapped, deviceAccess, ShareMode.ShareRead | ShareMode.ShareWrite);
  }

  private static IntPtr OpenDeviceIO(
    string devicePath,
    DeviceMode deviceMode,
    uint deviceAccess,
    ShareMode shareMode)
  {
    NativeMethods.SECURITY_ATTRIBUTES lpSecurityAttributes = new NativeMethods.SECURITY_ATTRIBUTES();
    int dwFlagsAndAttributes = 0;
    if (deviceMode == DeviceMode.Overlapped)
      dwFlagsAndAttributes = 1073741824 /*0x40000000*/;
    lpSecurityAttributes.lpSecurityDescriptor = IntPtr.Zero;
    lpSecurityAttributes.bInheritHandle = true;
    lpSecurityAttributes.nLength = Marshal.SizeOf((object) lpSecurityAttributes);
    return NativeMethods.CreateFile(devicePath, deviceAccess, (int) shareMode, ref lpSecurityAttributes, 3, dwFlagsAndAttributes, 0);
  }

  private static void CloseDeviceIO(IntPtr handle)
  {
    if (Environment.OSVersion.Version.Major > 5)
      NativeMethods.CancelIoEx(handle, IntPtr.Zero);
    NativeMethods.CloseHandle(handle);
  }

  private void DeviceEventMonitorInserted()
  {
    if (!this.IsOpen)
      this.OpenDevice(this._deviceReadMode, this._deviceWriteMode, this._deviceShareMode);
    if (this.Inserted == null)
      return;
    this.Inserted();
  }

  private void DeviceEventMonitorRemoved()
  {
    if (this.IsOpen)
      this.CloseDevice();
    if (this.Removed == null)
      return;
    this.Removed();
  }

  public void Dispose()
  {
    if (this.MonitorDeviceEvents)
      this.MonitorDeviceEvents = false;
    if (!this.IsOpen)
      return;
    this.CloseDevice();
  }

  protected delegate HidDeviceData ReadDelegate(int timeout);

  protected delegate HidReport ReadReportDelegate(int timeout);

  private delegate bool WriteDelegate(byte[] data, int timeout);

  private delegate bool WriteReportDelegate(HidReport report, int timeout);
}
