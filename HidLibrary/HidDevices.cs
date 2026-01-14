using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace HidLibrary;

public class HidDevices
{
  private static Guid _hidClassGuid = Guid.Empty;

  public static bool IsConnected(string devicePath)
  {
    return HidDevices.EnumerateDevices().Any<HidDevices.DeviceInfo>((Func<HidDevices.DeviceInfo, bool>) (x => x.Path == devicePath));
  }

  public static HidDevice GetDevice(string devicePath)
  {
    return HidDevices.Enumerate(devicePath).FirstOrDefault<HidDevice>();
  }

  public static IEnumerable<HidDevice> Enumerate()
  {
    return HidDevices.EnumerateDevices().Select<HidDevices.DeviceInfo, HidDevice>((Func<HidDevices.DeviceInfo, HidDevice>) (x => new HidDevice(x.Path, x.Description)));
  }

  public static IEnumerable<HidDevice> Enumerate(string devicePath)
  {
    return HidDevices.EnumerateDevices().Where<HidDevices.DeviceInfo>((Func<HidDevices.DeviceInfo, bool>) (x => x.Path == devicePath)).Select<HidDevices.DeviceInfo, HidDevice>((Func<HidDevices.DeviceInfo, HidDevice>) (x => new HidDevice(x.Path, x.Description)));
  }

  public static IEnumerable<HidDevice> Enumerate(int vendorId, params int[] productIds)
  {
    return HidDevices.EnumerateDevices().Select<HidDevices.DeviceInfo, HidDevice>((Func<HidDevices.DeviceInfo, HidDevice>) (x => new HidDevice(x.Path, x.Description))).Where<HidDevice>((Func<HidDevice, bool>) (x => x.Attributes.VendorId == vendorId && ((IEnumerable<int>) productIds).Contains<int>(x.Attributes.ProductId)));
  }

  public static IEnumerable<HidDevice> Enumerate(int vendorId, int productId, ushort UsagePage)
  {
    return HidDevices.EnumerateDevices().Select<HidDevices.DeviceInfo, HidDevice>((Func<HidDevices.DeviceInfo, HidDevice>) (x => new HidDevice(x.Path, x.Description))).Where<HidDevice>((Func<HidDevice, bool>) (x => x.Attributes.VendorId == vendorId && productId == (int) (ushort) x.Attributes.ProductId && (int) (ushort) x.Capabilities.UsagePage == (int) UsagePage));
  }

  public static IEnumerable<HidDevice> Enumerate(int vendorId)
  {
    return HidDevices.EnumerateDevices().Select<HidDevices.DeviceInfo, HidDevice>((Func<HidDevices.DeviceInfo, HidDevice>) (x => new HidDevice(x.Path, x.Description))).Where<HidDevice>((Func<HidDevice, bool>) (x => x.Attributes.VendorId == vendorId));
  }

  internal static IEnumerable<HidDevices.DeviceInfo> EnumerateDevices()
  {
    List<HidDevices.DeviceInfo> deviceInfoList = new List<HidDevices.DeviceInfo>();
    Guid hidClassGuid = HidDevices.HidClassGuid;
    IntPtr classDevs = NativeMethods.SetupDiGetClassDevs(ref hidClassGuid, (string) null, 0, 18);
    if (classDevs.ToInt64() != -1L)
    {
      NativeMethods.SP_DEVINFO_DATA deviceInfoData = HidDevices.CreateDeviceInfoData();
      int memberIndex1 = 0;
      while (NativeMethods.SetupDiEnumDeviceInfo(classDevs, memberIndex1, ref deviceInfoData))
      {
        ++memberIndex1;
        NativeMethods.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
        deviceInterfaceData.cbSize = Marshal.SizeOf((object) deviceInterfaceData);
        int memberIndex2 = 0;
        while (NativeMethods.SetupDiEnumDeviceInterfaces(classDevs, ref deviceInfoData, ref hidClassGuid, memberIndex2, ref deviceInterfaceData))
        {
          ++memberIndex2;
          string devicePath = HidDevices.GetDevicePath(classDevs, deviceInterfaceData);
          string str = HidDevices.GetBusReportedDeviceDescription(classDevs, ref deviceInfoData) ?? HidDevices.GetDeviceDescription(classDevs, ref deviceInfoData);
          deviceInfoList.Add(new HidDevices.DeviceInfo()
          {
            Path = devicePath,
            Description = str
          });
        }
      }
      NativeMethods.SetupDiDestroyDeviceInfoList(classDevs);
    }
    return (IEnumerable<HidDevices.DeviceInfo>) deviceInfoList;
  }

  private static NativeMethods.SP_DEVINFO_DATA CreateDeviceInfoData()
  {
    NativeMethods.SP_DEVINFO_DATA structure = new NativeMethods.SP_DEVINFO_DATA();
    structure.cbSize = Marshal.SizeOf((object) structure);
    structure.DevInst = 0;
    structure.ClassGuid = Guid.Empty;
    structure.Reserved = IntPtr.Zero;
    return structure;
  }

  private static string GetDevicePath(
    IntPtr deviceInfoSet,
    NativeMethods.SP_DEVICE_INTERFACE_DATA deviceInterfaceData)
  {
    int requiredSize = 0;
    NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData = new NativeMethods.SP_DEVICE_INTERFACE_DETAIL_DATA()
    {
      Size = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8
    };
    NativeMethods.SetupDiGetDeviceInterfaceDetailBuffer(deviceInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0, ref requiredSize, IntPtr.Zero);
    return NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref deviceInterfaceData, ref deviceInterfaceDetailData, requiredSize, ref requiredSize, IntPtr.Zero) ? deviceInterfaceDetailData.DevicePath : (string) null;
  }

  private static Guid HidClassGuid
  {
    get
    {
      if (HidDevices._hidClassGuid.Equals(Guid.Empty))
        NativeMethods.HidD_GetHidGuid(ref HidDevices._hidClassGuid);
      return HidDevices._hidClassGuid;
    }
  }

  private static string GetDeviceDescription(
    IntPtr deviceInfoSet,
    ref NativeMethods.SP_DEVINFO_DATA devinfoData)
  {
    byte[] numArray = new byte[1024 /*0x0400*/];
    int requiredSize = 0;
    int propertyRegDataType = 0;
    NativeMethods.SetupDiGetDeviceRegistryProperty(deviceInfoSet, ref devinfoData, 0, ref propertyRegDataType, numArray, numArray.Length, ref requiredSize);
    return numArray.ToUTF8String();
  }

  private static string GetBusReportedDeviceDescription(
    IntPtr deviceInfoSet,
    ref NativeMethods.SP_DEVINFO_DATA devinfoData)
  {
    byte[] numArray = new byte[1024 /*0x0400*/];
    if (Environment.OSVersion.Version.Major > 5)
    {
      ulong propertyDataType = 0;
      int requiredSize = 0;
      if (NativeMethods.SetupDiGetDeviceProperty(deviceInfoSet, ref devinfoData, ref NativeMethods.DEVPKEY_Device_BusReportedDeviceDesc, ref propertyDataType, numArray, numArray.Length, ref requiredSize, 0U))
        return numArray.ToUTF16String();
    }
    return (string) null;
  }

  internal class DeviceInfo
  {
    public string Path { get; set; }

    public string Description { get; set; }
  }
}
