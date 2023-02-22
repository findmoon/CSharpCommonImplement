How to translate MS Windows OS version numbers into product names in .NET?

> https://stackoverflow.com/questions/545666/how-to-translate-ms-windows-os-version-numbers-into-product-names-in-net

[VB](http://www.vb-helper.com/howto_net_os_version.html):

```sql
Public Function GetOSVersion() As String
    Select Case Environment.OSVersion.Platform
        Case PlatformID.Win32S
            Return "Win 3.1"
        Case PlatformID.Win32Windows
            Select Case Environment.OSVersion.Version.Minor
                Case 0
                    Return "Win95"
                Case 10
                    Return "Win98"
                Case 90
                    Return "WinME"
                Case Else
                    Return "Unknown"
            End Select
        Case PlatformID.Win32NT
            Select Case Environment.OSVersion.Version.Major
                Case 3
                    Return "NT 3.51"
                Case 4
                    Return "NT 4.0"
                Case 5
                    Select Case _
                        Environment.OSVersion.Version.Minor
                        Case 0
                            Return "Win2000"
                        Case 1
                            Return "WinXP"
                        Case 2
                            Return "Win2003"
                    End Select
                Case 6
                    Select Case _
                        Environment.OSVersion.Version.Minor
                        Case 0
                            Return "Vista/Win2008Server"
                        Case 1
                            Return "Win7/Win2008Server R2"
                        Case 2
                            Return "Win8/Win2012Server"
                        Case 3
                            Return "Win8.1/Win2012Server R2"
                    End Select
                Case 10  //this will only show up if the application has a manifest file allowing W10, otherwise a 6.2 version will be used
                  Return "Windows 10"
                Case Else
                    Return "Unknown"
            End Select
        Case PlatformID.WinCE
            Return "Win CE"
    End Select
End Function
```

-

[C#](http://snippets.dzone.com/user/mstampar/tag/version)

```kotlin
public string GetOSVersion()
{
  switch (Environment.OSVersion.Platform) {
    case PlatformID.Win32S:
      return "Win 3.1";
    case PlatformID.Win32Windows:
      switch (Environment.OSVersion.Version.Minor) {
        case 0:
          return "Win95";
        case 10:
          return "Win98";
        case 90:
          return "WinME";
      }
      break;

    case PlatformID.Win32NT:
      switch (Environment.OSVersion.Version.Major) {
        case 3:
          return "NT 3.51";
        case 4:
          return "NT 4.0";
        case 5:
          switch (Environment.OSVersion.Version.Minor) {
            case 0:
              return "Win2000";
            case 1:
              return "WinXP";
            case 2:
              return "Win2003";
          }
          break;

        case 6:
          switch(Environment.OSVersion.Version.Minor) {
            case 0:
              return "Vista/Win2008Server";
            case 1:
              return "Win7/Win2008Server R2";
            case 2:
              return "Win8/Win2012Server";
            case 3:
              return "Win8.1/Win2012Server R2";
          }
          break;
        case 10:  //this will only show up if the application has a manifest file allowing W10, otherwise a 6.2 version will be used
          return "Windows 10";
      }
      break;

    case PlatformID.WinCE:
      return "Win CE";
  }

  return "Unknown";
}
```