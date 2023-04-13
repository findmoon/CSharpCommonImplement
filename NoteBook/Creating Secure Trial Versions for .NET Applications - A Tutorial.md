Creating Secure Trial Versions for .NET Applications - A Tutorial

> [Creating Secure Trial Versions for .NET Applications - A Tutorial](https://www.codeproject.com/Articles/473278/Creating-Secure-Trial-Versions-for-NET-Application)
> 
> 已下载 Source Code

Implement trial licensing model for your .NET applications with minimal costs

- [Download Source Code - 37.5 KB](https://www.codeproject.com/KB/dotnet/473278/SampleTrialApp.zip) 

 ![Image 1](/KB/dotnet/473278/sampletrialapp.jpg)

## Introduction  

This article actually represents a second part of my previous article [Software Copy Protection for .NET Applications - A Tutorial](http://www.codeproject.com/Articles/398130/Software-Copy-Protection-for-Net-Applications-a-Tu). In that article I have presented several methods for achieving satisfactory software copy protection for .NET applications. In this article I will show you how to create trial versions of your products in a secure manner. Note that completely eliminating software piracy is not generally possible given the today's OS and hardware infrastructure, but this doesn't mean you cannot control it with careful consideration. 

Attached to this articles are C# and VB.NET samples (derived from my first article's samples but significantly different) showing how to implement trial versions of your products. The article does not contain source code (it's in the samples), because I would rather concentrate of explaining how to do it, rather than duplicating source code that can already be found in the samples.  

## Background  

One of the most successful models for selling software is the trial software model.  In this model, potential customers are allowed to download and use the software free of charge for a limited period of time, after which the software stops working unless a license is purchased. Unfortunately, this model is also one which can cause loss of revenue because it increases the risk of software piracy. 

## The Trial Model Flow 

After the user downloads your trial software, you must do the following:

- Determine the trial expiration date 
- Store the trial expiration date in such a way that the user cannot alter it 
- Prevent additional trial installations after the trial expires  
- Make sure the user does not manipulate the system clock such that your trial software thinks that the trial period is not yet over

As you can see, this is easier said than actually done. Some pressing questions appear: where do you store the expiration data such that the user cannot find and alter it ? How can you make sure the user didn't alter the system clock ? The answers below... 

## The Big Issue #1: Where to Store the Expiration Data ?  

The answer to this question is that if you have to hide data from the user, your licensing model is already compromised. A good licensing model should never be based on hiding or obscuring data from the user.  

The solution that I have found to this matter is to use an online licensing service in conjunction with digital signatures based on public key cryptography.  I chose to use SoftActivate Licensing SDK (from [www.softactivate.com](http://www.softactivate.com/)) because it is quite cheap, and it includes an ASP.NET licensing service which can issue short, human readable license keys (digitally signed using elliptic curve cryptography) and supports product activation which strongly resembles the trial licensing model. In addition to this, it can generate unique hardware id strings, which are very important (see below).  

Of course, you can build your own service for this purpose, for example using RSA public/private key pairs and digital signatures. The algorithm is as follows:

- After downloading and installing the product, the customer must press a "Start Trial" button. 
- Upon clicking this button, the product connects to an online licensing service (preferably via HTTPS in order to ensure that the server is the intended one by checking its SSL certificate) and sends a "hardware id" string unique to the customer's computer 
- The server checks against a database if the hardware id has already been used for trial purposes. If the hardware id has not been used before, the server sends back a digitally signed message (we name it "trial license" containing the trial expiration date (for security reasons this MUST be absolute, and not relative like a number of days). The trial license is signed using a private key known only by the server. The computer's hardware id should also be included in the license data. 
- After receiving the signed trial license from the server, the product saves it in a usual settings location and validates the digital signature at every startup and (optionally) at random times during the program execution, using the corresponding public key which is embedded into the product. Please note that the digitally signed message doesn't need to be hidden from the user; the user cannot alter it in any way because it is digitally signed and the signing (private) key is only known by the server. Also, if the trial license is deleted the software would not run, and if a trial version is requested again from the server, the server would not issue another trial license because the hardware id has already been used once for issuing a trial license.

This approach solves the "where to store the expiration data" problem. Basically, you can store this data anywhere you want. You don't need to hide anything, because this data cannot be altered without the product detecting it.   

### The Concept of  "Trial License Keys"

 Almost every software product uses license keys to "unlock" the software. A good idea is to embed a certain license key into the product, which is marked as a "trial license key". If the product detects the trial license key at startup, it behaves like a trial version, and if it detects a regular license key, it behaves like a registered product.

The concept of trial license keys is important when using online software activation with your products (see my previous article for details). When the product sends the license key to the activation server, the servers issues a license with a shorter expiration date if it receives a trial license key, and a license with a longer (or unlimited) expiration date if it receives a regular (purchased) product key. The samples in this article use this concept: they have an embedded "trial key" which is sent to the licensing service. The licensing service searches for this license key into its database, and issues a digitally signed expiration date according to what is specified for that particular trial key (30 days from the time of receiving the request). You can modify this value in the licensing service database.

## The Big Issue #2: How to detect system  clock manipulation ?  

System clock manipulation detection involves searching a computer for evidence that the system clock was turned back with the purpose of prolonging a software license past its expiration period.

To my knowledge there are no 100% bullet-proof methods of doing this, but usually this involves:

- Searching certain system files for file times (usually the "last modification time") that are in the future 
- Searching the event logs for evidence that some events have occurred in the future 
- Searching popular browser caches / cookies  for evidence of file times occurring in the future 
- Combining the evidence and drawing a conclusion

The included samples contain some simple clock manipulation detection routines searching the event logs for event dates that occur in the future.   See the _ClockManipulationDetector_ class in the sample projects. 

## Using the code   

The samples are created with Visual Studio 2010. Also, SQL Express 2005 or higher should be present on the development machine for the licensing service database. Also make sure that SoftActivate Licensing SDK (2.0 or higher) is installed on the development machine.  Before compiling, copy the LicensingService subfolder under the Bin folder from the SoftActivate Licensing SDK installation folder, inside the Tools folder created by unzipping the sample. 

### Running the samples   

First and foremost, you must start the licensing service by running the RunLicensingService.bat file from the Tools folder. Make sure that the paths in the .bat (to Visual Studio and to the sample folder) are set correctly ! Go to the \\bin\\Release folder and run the SampleAppCS.exe or SampleAppVB file. Click the "Start 30-day Trial" button to start the trial. The application will then connect to the licensing service and request a trial license. Please note that if you try to click it again, it will not work since a trial license has already been issued for your computer. In order to try again, you must connect to the licensing service database (found in the _Tools\\LicensingService\\App\_Data_ folder) and delete the record from the Activations table. 

## Points of Interest  

You should be aware that your trial product is not secure if you do not employ the typical software copy protection techniques explained in the first article of this series [here](http://www.codeproject.com/Articles/398130/Software-Copy-Protection-for-Net-Applications-a-Tu "Software Copy Protection for .Net Applications") . This is because if your application binaries can be altered, the trial checking mechanisms can be bypassed altogether.

## History 

October 9, 2012 - Initial publication.

October 16, 2012 - Small bug fixes to the samples.  

## License

This article, along with any associated source code and files, is licensed under [The Code Project Open License (CPOL)](http://www.codeproject.com/info/cpol10.aspx)
