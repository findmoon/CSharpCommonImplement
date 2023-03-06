**C#如何将应用程序的icon图标分配给程序中的所有Form窗体？**

> [Assigning an application’s icon to all forms in the application](https://www.codeproject.com/articles/29923/assigning-an-application-s-icon-to-all-forms-in-th)

It is a handy thing if all the windows in an application, by default, have the same icon like the application’s executable. Here is a tip: how to easily assign your application’s icon to all the hosted forms.

- [Download source code - 45.3 KB](https://www.codeproject.com/KB/dialog/AssigningDefaultIcon/_code.zip)

## Introduction

While developing an application, often, it is a handy thing if all the windows of the application have, by default, the same icon like the application’s executable. However, if you want to do so, you have to manually assign the icon for each form in Visual Studio. If the executable icon is changed, you have to repeat this work again. This article describes a tip for how to assign your application’s icon to all the forms hosted by the application with code.

It is written in C#, and can be used in any .NET language as a DLL.

The most important code for the icon extraction was gotten from [here](http://www.vbaccelerator.com/home/NET/Utilities/Icon_Extractor/article.asp). It was only slightly fixed and enhanced.

## Solution

All you have to do is:

1. Assign an icon to your project in Visual Studio.
2. Add the classes contained in the folder _IconRoutines_ to your project, or reference _IconRoutines.dll_ in your project.
3. Add the following line to each form constructor in your app where you want to set the same icon as the application’s icon:

C#

this.icon = Cliver.IconRoutines.HostIcon;

This will make the windows of your app have the same icon like the app itself. That’s all.

## How it works

This section is addressed to inquisitive guys, and can be omitted.

The first step is getting the icon of the executable.

After some time wasted with the .NET `Icon` class, I understood that correctly extracting the icon from an EXE file (or any file containing icons) is not a simple thing. Why? Because of several hacks there:

- `Icon.ExtractAssociatedIcon` extracts only a small image from an icon stored in the file.
- `Icon.Save` saves icons in a 16-color format only.

That means the code for extracting icons from files must be written from scratch. Fortunately, such a code was done already by Steve McMahon, and only needed some upgrade. It fetches an icon’s images from an executable. You can find it in the attached sources.

The second step is creating an `Icon` type object from the fetched images. The following code does it:

C#

/// <summary\> /// Extract all icon images from the library file like .exe, .dll, etc. /// </summary\> /// <param name="file"\>file where icon is extracted from</param\> /// <returns\>extracted icon</returns\> public static Icon ExtractIconFromLibrary(string file)
{
    // .NET Icon Explorer (http://www.vbaccelerator.com/home/NET/
    // Utilities/Icon\_Extractor/article.asp) is used here.
    Icon icon = null;
    try
    {
        vbAccelerator.Components.Win32.GroupIconResources gir = 
              new vbAccelerator.Components.Win32.GroupIconResources(file);
        if (gir.Count < 0)
            return icon;

        vbAccelerator.Components.Win32.IconEx i = 
              new vbAccelerator.Components.Win32.IconEx();
        if (gir\[0\].IdIsNumeric)
            i.FromLibrary(file, gir\[0\].Id);
        else
            i.FromLibrary(file, gir\[0\].Name);

        icon = i.GetIcon();
    }
    catch (Exception e)
    {
        throw (new Exception("Could not extract the host icon.", e));
        icon = null;
    }
    return icon;
}

/// <summary\> /// Icon of the hosting application. /// </summary\> public static Icon HostIcon
{
    get
    {
        if (\_HostIcon == null)
            \_HostIcon = IconRoutines.ExtractIconFromLibrary(
                        Application.ExecutablePath);
        return \_HostIcon;
    }
}
static Icon \_HostIcon = null;

Now, we only have to assign `IconRoutines.HostIcon` to the form’s icons.

## Using the code

In the attached code, you can find:

- A project _IconRoutines_ that contains the code for the icon management. You can compile it as a DLL and link to your project; else, add the code of _IconRoutines_ to your code.
- Project _Test_ that references _IconRoutines.dll_ and uses it.

Be happy!