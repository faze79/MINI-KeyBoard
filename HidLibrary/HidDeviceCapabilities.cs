#nullable disable
namespace HidLibrary;

public class HidDeviceCapabilities
{
  internal HidDeviceCapabilities(NativeMethods.HIDP_CAPS capabilities)
  {
    this.Usage = capabilities.Usage;
    this.UsagePage = capabilities.UsagePage;
    this.InputReportByteLength = capabilities.InputReportByteLength;
    this.OutputReportByteLength = capabilities.OutputReportByteLength;
    this.FeatureReportByteLength = capabilities.FeatureReportByteLength;
    this.Reserved = capabilities.Reserved;
    this.NumberLinkCollectionNodes = capabilities.NumberLinkCollectionNodes;
    this.NumberInputButtonCaps = capabilities.NumberInputButtonCaps;
    this.NumberInputValueCaps = capabilities.NumberInputValueCaps;
    this.NumberInputDataIndices = capabilities.NumberInputDataIndices;
    this.NumberOutputButtonCaps = capabilities.NumberOutputButtonCaps;
    this.NumberOutputValueCaps = capabilities.NumberOutputValueCaps;
    this.NumberOutputDataIndices = capabilities.NumberOutputDataIndices;
    this.NumberFeatureButtonCaps = capabilities.NumberFeatureButtonCaps;
    this.NumberFeatureValueCaps = capabilities.NumberFeatureValueCaps;
    this.NumberFeatureDataIndices = capabilities.NumberFeatureDataIndices;
  }

  public short Usage { get; private set; }

  public short UsagePage { get; private set; }

  public short InputReportByteLength { get; private set; }

  public short OutputReportByteLength { get; private set; }

  public short FeatureReportByteLength { get; private set; }

  public short[] Reserved { get; private set; }

  public short NumberLinkCollectionNodes { get; private set; }

  public short NumberInputButtonCaps { get; private set; }

  public short NumberInputValueCaps { get; private set; }

  public short NumberInputDataIndices { get; private set; }

  public short NumberOutputButtonCaps { get; private set; }

  public short NumberOutputValueCaps { get; private set; }

  public short NumberOutputDataIndices { get; private set; }

  public short NumberFeatureButtonCaps { get; private set; }

  public short NumberFeatureValueCaps { get; private set; }

  public short NumberFeatureDataIndices { get; private set; }
}
