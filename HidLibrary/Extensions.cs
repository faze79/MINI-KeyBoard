using System.Text;

#nullable disable
namespace HidLibrary;

public static class Extensions
{
  public static string ToUTF8String(this byte[] buffer)
  {
    string str = Encoding.UTF8.GetString(buffer);
    return str.Remove(str.IndexOf(char.MinValue));
  }

  public static string ToUTF16String(this byte[] buffer)
  {
    string str = Encoding.Unicode.GetString(buffer);
    return str.Remove(str.IndexOf(char.MinValue));
  }
}
