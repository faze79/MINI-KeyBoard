using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace HidLibrary;

public class HidFastReadEnumerator : IHidEnumerator
{
  public bool IsConnected(string devicePath) => HidDevices.IsConnected(devicePath);

  public IHidDevice GetDevice(string devicePath)
  {
    return this.Enumerate(devicePath).FirstOrDefault<IHidDevice>();
  }

  public IEnumerable<IHidDevice> Enumerate()
  {
    return HidDevices.EnumerateDevices().Select<HidDevices.DeviceInfo, IHidDevice>((Func<HidDevices.DeviceInfo, IHidDevice>) (d => (IHidDevice) new HidFastReadDevice(d.Path, d.Description)));
  }

  public IEnumerable<IHidDevice> Enumerate(string devicePath)
  {
    return HidDevices.EnumerateDevices().Where<HidDevices.DeviceInfo>((Func<HidDevices.DeviceInfo, bool>) (x => x.Path == devicePath)).Select<HidDevices.DeviceInfo, IHidDevice>((Func<HidDevices.DeviceInfo, IHidDevice>) (d => (IHidDevice) new HidFastReadDevice(d.Path, d.Description)));
  }

  public IEnumerable<IHidDevice> Enumerate(int vendorId, params int[] productIds)
  {
    return HidDevices.EnumerateDevices().Select<HidDevices.DeviceInfo, HidFastReadDevice>((Func<HidDevices.DeviceInfo, HidFastReadDevice>) (d => new HidFastReadDevice(d.Path, d.Description))).Where<HidFastReadDevice>((Func<HidFastReadDevice, bool>) (f => f.Attributes.VendorId == vendorId && ((IEnumerable<int>) productIds).Contains<int>(f.Attributes.ProductId))).Select<HidFastReadDevice, IHidDevice>((Func<HidFastReadDevice, IHidDevice>) (d => (IHidDevice) d));
  }

  public IEnumerable<IHidDevice> Enumerate(int vendorId)
  {
    return HidDevices.EnumerateDevices().Select<HidDevices.DeviceInfo, HidFastReadDevice>((Func<HidDevices.DeviceInfo, HidFastReadDevice>) (d => new HidFastReadDevice(d.Path, d.Description))).Where<HidFastReadDevice>((Func<HidFastReadDevice, bool>) (f => f.Attributes.VendorId == vendorId)).Select<HidFastReadDevice, IHidDevice>((Func<HidFastReadDevice, IHidDevice>) (d => (IHidDevice) d));
  }
}
