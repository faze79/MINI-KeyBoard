using System;

namespace HID;

public class report : EventArgs
{
  public readonly byte reportID;
  public readonly byte[] reportBuff;

  public report(byte id, byte[] arrayBuff)
  {
    this.reportID = id;
    this.reportBuff = arrayBuff;
  }
}
