#nullable disable
namespace HidLibrary;

public class HidDeviceAttributes
{
  internal HidDeviceAttributes(NativeMethods.HIDD_ATTRIBUTES attributes)
  {
    this.VendorId = (int) attributes.VendorID;
    this.ProductId = (int) attributes.ProductID;
    this.Version = (int) attributes.VersionNumber;
    this.VendorHexId = "0x" + attributes.VendorID.ToString("X4");
    this.ProductHexId = "0x" + attributes.ProductID.ToString("X4");
  }

  public int VendorId { get; private set; }

  public int ProductId { get; private set; }

  public int Version { get; private set; }

  public string VendorHexId { get; set; }

  public string ProductHexId { get; set; }
}
