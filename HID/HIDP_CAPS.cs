// Decompiled with JetBrains decompiler
// Type: HID.HIDP_CAPS
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

using System.Runtime.InteropServices;

namespace HID;

public struct HIDP_CAPS
{
  public ushort Usage;
  public ushort UsagePage;
  public ushort InputReportByteLength;
  public ushort OutputReportByteLength;
  [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
  public ushort[] Reserved;
  public ushort NumberLinkCollectionNodes;
  public ushort NumberInputButtonCaps;
  public ushort NumberInputValueCaps;
  public ushort NumberInputDataIndices;
  public ushort NumberOutputButtonCaps;
  public ushort NumberOutputValueCaps;
  public ushort NumberOutputDataIndices;
  public ushort NumberFeatureButtonCaps;
  public ushort NumberFeatureValueCaps;
  public ushort NumberFeatureDataIndices;
}
