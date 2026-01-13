// Decompiled with JetBrains decompiler
// Type: HID.SP_DEVINFO_DATA
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

using System;
using System.Runtime.InteropServices;

namespace HID;

[StructLayout(LayoutKind.Sequential)]
public class SP_DEVINFO_DATA
{
  public int cbSize = Marshal.SizeOf(typeof (SP_DEVINFO_DATA));
  public Guid classGuid = Guid.Empty;
  public int devInst;
  public int reserved;
}
