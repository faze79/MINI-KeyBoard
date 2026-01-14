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
