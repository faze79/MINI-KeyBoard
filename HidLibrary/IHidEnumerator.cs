// Decompiled with JetBrains decompiler
// Type: HidLibrary.HidEnumerator
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace HidLibrary;

public class HidEnumerator : IHidEnumerator
{
  public bool IsConnected(string devicePath) => HidDevices.IsConnected(devicePath);

  public IHidDevice GetDevice(string devicePath) => (IHidDevice) HidDevices.GetDevice(devicePath);

  public IEnumerable<IHidDevice> Enumerate()
  {
    return HidDevices.Enumerate().Select<HidDevice, IHidDevice>((Func<HidDevice, IHidDevice>) (d => (IHidDevice) d));
  }

  public IEnumerable<IHidDevice> Enumerate(string devicePath)
  {
    return HidDevices.Enumerate(devicePath).Select<HidDevice, IHidDevice>((Func<HidDevice, IHidDevice>) (d => (IHidDevice) d));
  }

  public IEnumerable<IHidDevice> Enumerate(int vendorId, params int[] productIds)
  {
    return HidDevices.Enumerate(vendorId, productIds).Select<HidDevice, IHidDevice>((Func<HidDevice, IHidDevice>) (d => (IHidDevice) d));
  }

  public IEnumerable<IHidDevice> Enumerate(int vendorId)
  {
    return HidDevices.Enumerate(vendorId).Select<HidDevice, IHidDevice>((Func<HidDevice, IHidDevice>) (d => (IHidDevice) d));
  }
}
