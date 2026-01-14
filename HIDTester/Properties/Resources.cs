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
