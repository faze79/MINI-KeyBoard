// Decompiled with JetBrains decompiler
// Type: HIDTester.Properties.Resources
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace HIDTester.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Resources()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (HIDTester.Properties.Resources.resourceMan == null)
        HIDTester.Properties.Resources.resourceMan = new ResourceManager("HIDTester.Properties.Resources", typeof (HIDTester.Properties.Resources).Assembly);
      return HIDTester.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => HIDTester.Properties.Resources.resourceCulture;
    set => HIDTester.Properties.Resources.resourceCulture = value;
  }
}
