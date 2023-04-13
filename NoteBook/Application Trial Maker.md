**Application Trial Maker**

[toc]

其他：
[Make Your Application with Trial Version](https://www.c-sharpcorner.com/uploadfile/tanmayit08/make-your-application-with-trial-version/)

[C# How can I create a simple time based trial version of my software? [duplicate]](https://stackoverflow.com/questions/412833/c-sharp-how-can-i-create-a-simple-time-based-trial-version-of-my-software)

[Implementing a 30 day time trial [closed]](https://stackoverflow.com/questions/2021088/implementing-a-30-day-time-trial/2022237#2022237)

[How can I create a product key for my C# application?](https://stackoverflow.com/questions/453030/how-can-i-create-a-product-key-for-my-c-sharp-application)

[License Key Generation](https://www.codeproject.com/Articles/11012/License-Key-Generation)

[Code for creating trial version of software created in VB.Net 2005?](https://social.msdn.microsoft.com/Forums/en-US/ff61633b-9115-4161-be8d-d59c0b4aa51f/code-for-creating-trial-version-of-software-created-in-vbnet-2005?forum=vbgeneral)   里面提到的 C:\Windows\system32 似乎也很不错

https://github.com/Willy-Kimura/TrialMaker.Demo
https://github.com/atxaloisio/TrialMaker



> 应用程序试用制作工具 [Application Trial Maker](https://www.codeproject.com/Articles/15496/Application-Trial-Maker-2)


- [Download source files - 72 Kb](https://www.codeproject.com/KB/cs/Trial_Maker/Trial_Maker_src.zip)
- [Download Example.zip](https://www.codeproject.com/KB/cs/Trial_Maker/Example.zip)
- [Download Serial Maker files - 46 Kb](https://www.codeproject.com/KB/cs/Trial_Maker/Serial_Maker.zip)
- [Download SerialBox Control files - 34 Kb](https://www.codeproject.com/KB/cs/Trial_Maker/SerialBox.zip)
- [Download FilterTextBox Control files - 19 Kb](https://www.codeproject.com/KB/cs/Trial_Maker/FilterTextBox.zip)


## Introduction

![](/KB/cs/Trial_Maker/RegistrationDialog.jpg)

It's very common to want a trial version of your application. For example, you need your application to only run for 30 days, or the user can only run your application 200 times.

You can do this kind of task with this library.

If someone wants to buy your application, he must call you and read his computer ID. Then you will use the Serial Maker to make a serial key (password). After entering the password, the application always runs as the full version.

If the trial period is finished, reinstalling the application won't let user run it.

The computer ID is a 25 character string that came from hardware information. Processor ID, main board manufacturer, VGA name, hard disk serial number, etc. You can choose what hardware items you want to use to make the computer ID.

When you give the password to a customer, his password won't change until he changes his hardware.

### Computer ID

To make a Computer ID, you must indicate the name of your application. That's because if you have used Trial Maker to make a trial for more than one application, each application will have it's own Computer ID. In the source, Computer ID = "BaseString"

Trial Maker uses management objects to get hardware information.

### Serial

The password will be produced by changing the Computer ID. To make a password, you must have your own identifier. This identifier is a 3 character string. None of characters must be zero.

### Files

There are two files to work with in this class.

The first file is used to save computer information and remaining days of the trial period. This file must be in a secure place, and the user must not find it (`RegFile`). It's not a bad idea to save this file in application startup directory

The second file will be used if the user registers the software. It will contain the serial that the user entered (`HideFile`). You can save HideFile in, for example, C:\\Windows\\system32\\tmtst.dfg. I mean some obscure path.

Both files will be encrypted using Triple DES algorithm. The key of encryption is custom. It's better you choose your own key for encryption.

## How it works

### Getting System Information

`SystemInfo` class is for getting system information. It contains the `GetSystemInfo` function. This function takes the application name and appends to it the Processor ID, main board product, etc.

public static string GetSystemInfo(string SoftwareName)
{
    if (UseProcessorID == true)

        SoftwareName += RunQuery("Processor", "ProcessorId");

    if (UseBaseBoardProduct == true)

        SoftwareName += RunQuery("BaseBoard", "Product");

    if (UseBaseBoardManufacturer == true)

        SoftwareName += RunQuery("BaseBoard", "Manufacturer");
    // See more in source code

    SoftwareName = RemoveUseLess(SoftwareName);

    if (SoftwareName.Length < 25)

        return GetSystemInfo(SoftwareName);
    return SoftwareName.Substring(0, 25).ToUpper();
}

Then it removes unused characters from the string. Any characters outside the range A to Z and 0 to 9 are unused characters. If the string isn't long enough, call `GetSystemInfoAgain` to make it longer.

`RunQuery` function takes object name (`TableName`) and method name and returns defined method of first object

private static string RunQuery(string TableName, string MethodName)
{
    ManagementObjectSearcher MOS =
      new ManagementObjectSearcher("Select \* from Win32\_" + TableName);
    foreach (ManagementObject MO in MOS.Get())
    {
        try
        {
            return MO\[MethodName\].ToString();
        }
        catch (Exception e)
        {
            System.Windows.Forms.MessageBox.Show(e.Message);
        }
    }
    return "";
}

Once we have system information, we must make a Computer ID (`BaseString`) from it.

### Making Computer ID (BaseString)

C#

private void MakeBaseString()
{
    \_BaseString = 
        Encryption.Boring(Encryption.InverseByBase(
            SystemInfo.GetSystemInfo(\_SoftName),10));
}

To make `BaseString`, first we get system information, and then use `Encryption.InverseByBase` and `Encryption.Boring`.

`InverseByBase`: `Encryption.InverseByBase("ABCDEF",3)` will return CBAFED; it's so simple - it's inversing every 3 characters

Boring: move every character in the string with the formula:

NewPlace = (CurrentPlace \* ASCII(character)) % string.length

### Making Serial key (Password)

We use `Encryption.MakePassword`. this function takes `BaseString` and `Identifier`. It uses `InverseByBase` 3 times and `Boring` once, and then use `ChangeChar` function to change characters

static public string MakePassword(string st, string Identifier)
{
    if (Identifier.Length != 3)
        throw new ArgumentException("Identifier must be 3 character length");
    int\[\] num = new int\[3\];
    num\[0\] = Convert.ToInt32(Identifier\[0\].ToString(), 10);
    num\[1\] = Convert.ToInt32(Identifier\[1\].ToString(), 10);
    num\[2\] = Convert.ToInt32(Identifier\[2\].ToString(), 10);
    st = Boring(st);
    st = InverseByBase(st, num\[0\]);
    st = InverseByBase(st, num\[1\]);
    st = InverseByBase(st, num\[2\]);

    StringBuilder SB = new StringBuilder();
    foreach (char ch in st)
    {
        SB.Append(ChangeChar(ch, num));
    }
    return SB.ToString();
}        

### Check Password

After making `BaseString` and password, check if the user already registered the software. To do this, use `CheckRegister` Function. It will return true if registered before and false if not.

If `CheckRegister` returns false, open a registration dialog for the user to enter the password.

### Show Dialog

Create new `frmDialog` and show it to the user. If the dialog result is `<code>OK`, it means software is registered successfully, and if it is `Retry`, it means it is in trial mode. Any other `DialogResult` means cancel. The Dialog class takes BaseString, Password, days to end and Run times to end as arguments.

### Reading And Writing Files

There's a class named `FileReadWrite`. This class reads / writes files with Triple DES encryption algorithm.

`FileReadWrite.WriteFile` take a 2 strings. The first one is the file path and the second one is the data to write. After writing all data it write byte 0 as finish. It will use for reading

public static void WriteFile(string FilePath, string Data)
{
    FileStream fout = new FileStream(FilePath, FileMode.OpenOrCreate,
      FileAccess.Write);
    TripleDES tdes = new TripleDESCryptoServiceProvider();
    CryptoStream cs = new CryptoStream(fout, tdes.CreateEncryptor(key, iv),
       CryptoStreamMode.Write);
    byte\[\] d = Encoding.ASCII.GetBytes(Data);

    cs.Write(d, 0, d.Length);
    cs.WriteByte(0);

    cs.Close();
    fout.Close();
}

The key for writing and reading is the same one, it's custom and you can change it.

## How To Use

### Where to use

The best place that you can check for registration is before showing the main dialog. When you have created a Windows application project, first you must add SoftwareLocker.dll as reference

Then in program.cs, find the main function. I think it's the best place for checking registration. You can check registration when your main dialog loads, but before loading is better.

### Change Main Dialog

It's better to add one argument to your main dialog constructor. A Boolean value that indicates that this is a trial or a full version running. If it's a trial mode, disable some parts of your application as needed.

### How to change main()

Add namespace

C#

using SoftwareLocker;

C#

\[STAThread\]
static void Main()
{
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    TrialMaker t = new TrialMaker("TMTest1",
      Application.StartupPath + "\\\\RegFile.reg",
      Environment.GetFolderPath(Environment.SpecialFolder.System) +
        "\\\\TMSetp.dbf",
      "Phone: +98 21 88281536\\nMobile: +98 912 2881860",
      5, 10, "745");

    byte\[\] MyOwnKey = { 97, 250,  1,  5,  84, 21,   7, 63,
                         4,  54, 87, 56, 123, 10,   3, 62,
                         7,   9, 20, 36,  37, 21, 101, 57};
    t.TripleDESKey = MyOwnKey;
    // if you don't call this part the program will
    //use default key to encryption

    TrialMaker.RunTypes RT = t.ShowDialog();
    bool is\_trial;
    if (RT != TrialMaker.RunTypes.Expired)
    {
        if (RT == TrialMaker.RunTypes.Full)
            is\_trial = false;
        else
            is\_trial = true;

        Application.Run(new Form1(is\_trial));
    }
}

Don't move first two lines, after them, define `TrialMaker` class

#### Constructor

- `SoftwareName`: Your software's name, this will be used to make ComputerID
- `RegFilePath`: The file path that, if the user entered the registration code, will save it and check on every run.
- `HiddenFilePath`: This file will be used to save system information, days to finish trial mode, how many other times user can run application and current date.
- `Text`: It will show below the OK button on the Registration Dialog. Use this text for your phone number, etc.
- `DefaultDays`: How many days the user can run in trial mode.
- `DefaultTimes`: How many times user can run the application.
- `Identifier`: Three character string for making password. It's the password making identifier

#### Optional and recommended

In the constructor, you can change the default Triple DES key. It's a 24 byte key, and then you can customize Management Objects used to make the Computer ID (`BaseString`):

C#

t.UseBiosVersion = false // = true;

You can do this with any `t.UseXxx`

Don't disable all of them or none of them, enabling 3 or 4 of them is the best choice. Or you can leave it as its default.

## Serial Maker

Serial maker is another application that uses the Encryption class. It takes your identifier and generate a password (serial key)

Doesn't contain anything special.

## Other Controls

In the registration dialog, I have used a `SerialBox` control. It's only 5 text boxes that you can write your serial in.

`SerialBox` uses `FilterTextBox` itself.

Both of these controls are available to download.

## Notes

- All codes are written in Visual Studio 2005
- Serial Maker uses Encryption class from Trial Maker as a file link
- Writing with one encryption key and reading with another will cause an exception
- FilterTextBox Control is not the same that I wrote before in my articles
- Always Secure your identifiers and don't lose them

## License

This article, along with any associated source code and files, is licensed under [The Code Project Open License (CPOL)](http://www.codeproject.com/info/cpol10.aspx)

