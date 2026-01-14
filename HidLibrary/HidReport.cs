using System;

#nullable disable
namespace HidLibrary;

public class HidReport
{
  private byte _reportId;
  private byte[] _data = new byte[0];
  private readonly HidDeviceData.ReadStatus _status;

  public HidReport(int reportSize) => Array.Resize<byte>(ref this._data, reportSize - 1);

  public HidReport(int reportSize, HidDeviceData deviceData)
  {
    this._status = deviceData.Status;
    Array.Resize<byte>(ref this._data, reportSize - 1);
    if (deviceData.Data != null)
    {
      if (deviceData.Data.Length != 0)
      {
        this._reportId = deviceData.Data[0];
        this.Exists = true;
        if (deviceData.Data.Length <= 1)
          return;
        int length = reportSize - 1;
        if (deviceData.Data.Length < reportSize - 1)
          length = deviceData.Data.Length;
        Array.Copy((Array) deviceData.Data, 1, (Array) this._data, 0, length);
      }
      else
        this.Exists = false;
    }
    else
      this.Exists = false;
  }

  public bool Exists { get; private set; }

  public HidDeviceData.ReadStatus ReadStatus => this._status;

  public byte ReportId
  {
    get => this._reportId;
    set
    {
      this._reportId = value;
      this.Exists = true;
    }
  }

  public byte[] Data
  {
    get => this._data;
    set
    {
      this._data = value;
      this.Exists = true;
    }
  }

  public byte[] GetBytes()
  {
    byte[] array = (byte[]) null;
    Array.Resize<byte>(ref array, this._data.Length + 1);
    array[0] = this._reportId;
    Array.Copy((Array) this._data, 0, (Array) array, 1, this._data.Length);
    return array;
  }
}
