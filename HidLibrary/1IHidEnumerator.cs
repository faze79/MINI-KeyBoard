// Decompiled with JetBrains decompiler
// Type: HidLibrary.IHidEnumerator
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace HidLibrary;

public interface IHidEnumerator
{
  bool IsConnected(string devicePath);

  IHidDevice GetDevice(string devicePath);

  IEnumerable<IHidDevice> Enumerate();

  IEnumerable<IHidDevice> Enumerate(string devicePath);

  IEnumerable<IHidDevice> Enumerate(int vendorId, params int[] productIds);

  IEnumerable<IHidDevice> Enumerate(int vendorId);
}
