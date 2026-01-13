// Decompiled with JetBrains decompiler
// Type: HID.SP_DEVICE_INTERFACE_DETAIL_DATA
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

using System.Runtime.InteropServices;

namespace HID;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
{
  internal int cbSize;
  internal short devicePath;
}
