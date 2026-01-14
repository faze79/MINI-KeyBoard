using System;

#nullable disable
namespace HidLibrary;

[Flags]
public enum ShareMode
{
  Exclusive = 0,
  ShareRead = 1,
  ShareWrite = 2,
}
