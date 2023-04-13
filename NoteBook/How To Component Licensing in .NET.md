How To: Component Licensing in .NET

> [How To: Component Licensing in .NET](https://www.codeproject.com/Articles/599509/How-To-Component-Licensing-in-NET)
>
> source code 已下载

Developing a component licensing system in .NET.

- [Download source code - 163.1 KB](https://www.codeproject.com/KB/dotnet/599509/ComponentLicensing.zip)

![Image 1](/KB/dotnet/599509/Marketing.png)

## Introduction

So you've created the next super-widget that hundreds of developers want, but you have a problem, how do you make sure that your customers purchase licenses and don't just share the DLL? You need to implement a licensing system... Here we will explore how to implement a licensing system and to effectively protect your intellectual property.

## First Steps - Decisions 

There are a couple of decisions you need to make about your licensing system, and the first is how you are going to handle the generation of license codes. There are a number of methods out there, on my website I chose to go with Wordpress + WooCommerce + Software Plugin. The software plugin generates license codes that can be used to validate licenses later on and can handle activations for use in my application based (another article) licensing. 

After that, you will need to use a combination of methods to effectively protect your software. After you get the licensing system implemented, you will need to protect against reverse engineering. As a disclaimer I will say that no obfuscation tool is 100% effective, but the goal here as I see it is to make it require more effort to reverse engineer and crack your code than it is to just purchase a license key. At the end of this article I'll list out some obfuscation tools and my impressions of them for reference. 

## Designing a Licensing System 

.NET has a built-in licensing system that is very useful, although without modifying the default implementations or the provided license provider ([LicFileLicenseProvider](http://msdn.microsoft.com/en-us/library/system.componentmodel.licfilelicenseprovider.aspx)), its almost comically easy to break. The `LicFileLicenseProvider` simply holds a license file that has the full type name and version in a text file. If the text file contains this text, then the component is licensed. 

So what are the pieces of licensing that we will need to work with? 

- [LicenseManager](http://msdn.microsoft.com/en-us/library/system.componentmodel.licensemanager.aspx)  
    - The `LicenseManager` is a utility class that lets us validate and work with the licensing system. The main methods we will use on this are the `Validate()` methods.
- [LicenseProvider](http://msdn.microsoft.com/en-us/library/system.componentmodel.licenseprovider.aspx)  
    - This is an abstract class that we will derive from to implement our provider. The provider is responsible for  (oddly enough) providing a license to the `LicenseManager`. You will use the [`LicenseProviderAttribute`](http://msdn.microsoft.com/en-us/library/system.componentmodel.licenseproviderattribute.aspx) on your licensed component to direct the `LicenseManager` to the correct provider.
- [License](http://msdn.microsoft.com/en-us/library/system.componentmodel.license.aspx)
    - Another abstract class that we will derive from for the license. There is only one property that has to be overridden, which is the `LicenseKey` property (when implementing it will also require you implement the `Dispose` method, but unless you are using something that must be disposed this can be left blank). 

You might be wondering just how all these pieces fit together, here is a basic diagram of how the pieces interact:

![Image 2](data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==)

Basically what we have is the License-able component in blue. You decorate this class with the `LicenseProviderAttribute` and the type of your implemented license provider. Somewhere in your class (probably in the constructor), you call the `LicenseManager.Validate` method to validate the license. The license manager returns a context of License (which you can cast to your implemented License type) and check license specific properties.

## Implementing the License 

It may seem counter intuitive to start at the very end, but we will need this component later on in the License Provider, so we need to start with the License now. 

So let's dive right into the code:

C#

/// <summary\> /// License granted to components. /// </summary\> public class ComponentLicense : License
{
} 

Here we derive the class from the `License` base type. In the implementation that I provide, I made the constructors private and added internal static methods to allow different types of licenses to be created, but for now lets take a look at the meat of the class: 

C#

private bool VerifyKey()
{
    //Implement your own key verification here. For now we will
    //just verify that the last 8 characters are a valid CRC.
    //The key is a simple Guid with an extended 8 characters for
    //the CRC.

    if (string.IsNullOrEmpty(\_licKey))
        return false;

    //Guid's are in the following format:
    //F2A7629C-5AAF-4E86-8EC2-64F73B6A4FE3
    //Developer keys are an extension of that, like:
    //F2A7629C-5AAF-4E86-8EC2-64F73B6A4FE3-XXXXXXXX

    //So a developer key MUST be 45 characters long

    if (\_licKey.Length != 45)
        return false;

    //It must also contain -'s
    if (!\_licKey.Contains('\-'))
        return false;

    //Now split it
    string\[\] splitKey = \_licKey.Split('\-');

    //It has to have 6 parts or its invalid
    if (splitKey.Length != 6)
        return false;

    //Join elements 1 through 5, then convert to a byte array
    string baseKey = string.Join("\-", splitKey, 0, 5);
    byte\[\] asciiBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(baseKey);

    //Get the CRC
    uint baseCRC = Crc32.Compute(asciiBytes);

    //Now let's compare the CRC to make sure its a valid key
    if (string.Equals(splitKey\[5\], baseCRC.ToString("X8"), 
                        StringComparison.OrdinalIgnoreCase))
        return true;

    return false;
} 

This is just an example, you can modify this method any way you see fit to create as complicated of a licensing system as you want, but here is how my licensing system works: 

A license key is simply a `GUID` that has a `CRC` appended to it. The `CRC` is calculated by taking the `GUID` formatted string, getting the ASCII bytes, and running it through a `CRC32` algorithm. The `CRC` is appended to the license key as 8 hex characters.

The validation routine does some basic checks to verify that there is a valid `GUID` formatted string, and then calculates the `CRC` of the string portion and compares that with the license key. If the keys match then the license is valid, if they don't then a demo license will be returned.

As I said above, this is only an example of what you can do. You can make developer codes that verify against a web service to lock the developer to a single computer, only allow development on Wednesdays, use a web service to allow development based on a subscription, etc. The actual implementation is up to you and the more complicated you can make it, the better off you can be. You can even embed data in the license key to enable different features.

## Creating the License Provider 

The next step is implementing the custom license provider for your components.  The license provider tells the license manager how to get a license. You could add verification code here as well, the implementation is really up to you.

C#

/// <summary\> /// Gets a license for an instance or type of component. /// </summary\> /// <param name="context"\>A <see cref="LicenseContext"/\> // that specifies where you can use the licensed object.</param>
/// <param name="type"\>A <see cref="System.Type"/\> // that represents the component requesting the license.</param>
/// <param name="instance"\>An object that is requesting the license.</param\> /// <param name="allowExceptions"\>true if a /// <see cref="LicenseException"/\> should be thrown /// when the component cannot be granted a license; otherwise, false.</param\> /// <returns\>A valid <see cref="License"/\>.</returns\> public override License GetLicense(LicenseContext context, 
       Type type, object instance, bool allowExceptions)
{
    //Here you can check the context to see if it is
    //running in Design or Runtime. You can also do more
    //fun things like limit the number of instances
    //by tracking the keys (only allow X number of controls
    //on a form for example). You can add additional
    //data to the instance if you want, etc. 

    try
    {
        string devKey = GetDeveloperKey(type);

        return ComponentLicense.CreateLicense(devKey);
        //Returns a demo license if no key.
    }
    catch (LicenseException le)
    {
        if (allowExceptions)
            throw le;
        else
            return ComponentLicense.CreateDemoLicense();
    }
}

/// <summary\> /// Returns the string value of the DeveloperKey static property on the control. /// </summary\> /// <param name="type"\>Type of licensed /// component with a DeveloperKey property.</param\> /// <returns\>String value of the developer key.</returns\> /// <exception cref=""\></\> private string GetDeveloperKey(Type type)
{
    PropertyInfo pInfo = type.GetProperty("DeveloperKey");

    if (pInfo == null)
        throw new LicenseException(type, null, 
           "The licensed control does not contain " + 
           "a DeveloperKey static field. Contact the developer for assistance.");

    string value = pInfo.GetValue(null, null) as string;

    return value;
} 

There is only one method that you can override in this class, the `GetLicense` method. In the implementation that I provide, I'm using reflection to get a developer key that is stored as a static property on the control. This key is then passed to the `ComponentLicense.CreateLicense` static method.  

You can also use the context and instance data passed into the `GetLicense` to check additional data or to use it in the licensing system. In the implementation above, all we do is create the license from the developer key property on the licensed component. Its also important to note the use of the `allowExceptions` property to know if you are allowed to throw exceptions or not. I'm really not sure when each one is used, but its important to implement the method properly.

And that's really all you need to implement to create your own component licensing system. Next we will explore how to use it.

## Using Your Licensing System 

Using your licensing system is not very difficult. In the example project I've included there is a licensed control that implements our license system. Here is the full code of the component:

C#

\[LicenseProvider(typeof(Ingenious.Licensing.ComponentLicenseProvider))\]
public partial class LicensableControl : UserControl
{

    #region Fields 
    private bool \_isDemo = true;
    private static string \_licKey = "";

    #endregion 
    #region Properties 
    /// <summary\>    /// Gets/sets the developer key for use on this control.    /// </summary\>    \[Category("License"), 
      Description("Sets the license key for use on the control.")\]
    public string LicenseKey 
    {
        get { return LicensableControl.DeveloperKey; }
        set 
        {
            LicensableControl.DeveloperKey = value;
            CheckLicense();
            this.Invalidate();
        }
    }

    \[Browsable(false)\]
    public static string DeveloperKey { get; set; }

    #endregion 
    #region Construction / Deconstruction 
    public LicensableControl()
    {
        InitializeComponent();
        SetStyle(ControlStyles.ResizeRedraw, true);
        SetStyle(ControlStyles.UserPaint, true);
        
        CheckLicense();
    }

    #endregion 
    #region Protected Methods 
    protected override void OnPaint(PaintEventArgs e)
    {
        string text = (\_isDemo ? "This control is running in demo mode." : 
                          "This is a fully licensed control.");

        StringFormat sf = new StringFormat();
        sf.LineAlignment = StringAlignment.Center;
        sf.Alignment = StringAlignment.Center;

        e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
        e.Graphics.DrawString(text, Font, Brushes.Black, ClientRectangle, sf);
    }

    #endregion 
    #region Private Methods 
    private void CheckLicense()
    {
        //If we want to only grant a license for design time, we can check like this:
        /\* if (Site != null && Site.DesignMode)
        {
            //Check license here
        }
        else
        {
            \_isDemo = false;
            //Allow runtime usage.
        }
        \*/

        ComponentLicense lic = 
          LicenseManager.Validate(typeof(LicensableControl), this) as ComponentLicense;

        if (lic != null)
        {
            \_isDemo = lic.IsDemo;
        }
    }

    #endregion }

You can see that there isn't much to get the system to work. The first thing you do is decorate your class with the `LicenseProvider` attribute that uses the `ComponentLicenseProvider` type as the provider. Then in the constructor we can check the license and set a flag (or save the whole license if more metadata is included in it). 

In my implementation I'm allowing the developer to place the license key into the property window of one of the controls and it will propagate to the rest of them through the static property. It's worth noting however, that if you do it this way, the control will not be licensed until that form is opened that you originally pasted the developer key into. The way to get around this is in your _Program.cs_ file, set the `LicensableControl.DeveloperKey` to the developer key. The only problem with this is that the designer will not recognize this and it will look like an unlicensed component in the designer. I didn't spend a lot of time on this since it was beyond the scope of the article.

## Some Things Worth Saying 

Its very important that you pick a licensing method and **stick with it**. The problem comes in when you change your licensing system and alienate your existing user base. Changing your licensing system half way through the game can be an expensive lesson, and a painful one for your users.

## Obfuscation

I promised I'd touch on this subject and here it is: Obfuscation is the process of taking otherwise readable code and turning it into incomprehensible junk. The main goal here is to take an otherwise easily decompilable program into something that even if it is decompiled, that its meaning is hidden. 

There are ways around even the most  brilliant obfuscation methods, but as I said earlier in the article the goal here is to make it more expensive to decompile than it is for the user to just buy a license. A truly determined user can turn your compiled obfuscated code back into source no matter what you do.

I've spent some time evaluating some of the obfuscation tools and here are my thoughts on some of the ones I've found: 

- [Redgate SmartAssembly](http://www.red-gate.com/products/dotnet-development/smartassembly/)

- From the company that brought you the best decompilation tool I've used (Redgate Reflector), there is a tool to avoid decompilation, SmartAssembly. I did not evaluate this tool because the price point was high, but I was reluctant anyway from the promise to the .NET community to keep Reflector free, then doubling back and turning it into a rather expensive tool.

- [Preemptive Solutions Dotfuscator](http://www.preemptive.com/products/dotfuscator/overview)

- The only one trusted to ship in the box with MS VS. Actually I think this was a (somewhat) clever marketing ploy as DotFuscator's "Community" edition is a joke that only uses simple variable renaming, which is easily cracked. The full version of DotFuscator is way out of reach of most budding developers. 

- [SecureTeam Agile 6.0 Code Protection (CliSecure)](http://www.secureteam.net/index.aspx) 
    - Of the ones I tested this was the better of them. There are a ton of options that can really make it difficult to reverse engineer. This is the one that I bought, although it was rather expensive I found that it produced usable assemblies (some products mangled them so bad they wouldn't even run). 
-  [Eziriz .NET Reactor](http://www.eziriz.com/dotnet_reactor.htm) 
    - This is one of the truly affordable obfuscation tools out there at $179 and it does a good job. I've used the .NET Licensing product from the same company with good results before deciding to roll my own.  

There are a lot more out there, and I've just touched on some of the bigger name ones so do a Google search and decide for yourself. Just remember, the harder you make it for the end user to circumvent your licensing, the more revenue you can hold. You could also bypass licensing completely and just sell support contracts, but we can leave the concept of "free" software for another article.

## Points of Interest  

Licensing can be as complicated or as easy as you want, and .NET has a very powerful set of base classes to build on. Extending these classes the right way allows you to reap the benefits of selling your software for a profit rather than just fun. In a future article I will explore how to do application level licensing using many of the same concepts applied here.

## History

- May 28, 2013 - Initial release.

## License

This article, along with any associated source code and files, is licensed under [The Code Project Open License (CPOL)](http://www.codeproject.com/info/cpol10.aspx)