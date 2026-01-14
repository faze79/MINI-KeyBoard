// Decompiled with JetBrains decompiler
// Type: HidLibrary.HidDeviceCapabilities
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

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
