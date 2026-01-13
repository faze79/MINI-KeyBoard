// Decompiled with JetBrains decompiler
// Type: HID.DEV_BROADCAST_HDR
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

namespace HID;

public struct DEV_BROADCAST_HDR
{
  public int dbcc_size;
  public int dbcc_devicetype;
  public int dbcc_reserved;
}
