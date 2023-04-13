**Get the CPU Usage in C#**

[toc]

> [Get the CPU Usage in C#](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/)

![Get the CPU Usage in C#](/img/Csharp/ag%20feature%20image%20-%20csharp%20get%20cpu%20usage.png?ezimgfmt=rs%3Adevice%2Frscb5-1)

This tutorial thoroughly explains the performance monitoring parameters that affect your programs. We will also demonstrate CPU and memory usage methods, particularly in C#.

디톡스 다이어트 식단 8가�...

Please enable JavaScript

[디톡스 다이어트 식단 8가지, 굶지 않고 할 수 있어요](https://humix.com/redirect?url=https%3A%2F%2Fktong.kr%2Fhumix%2Fvideo%2Fddaeea18d74f0cc3c82186f51eb2ddfddfdf4f603dc920d6cc0e85f0729621d5)

## [Performance Monitoring in `C#`](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/#performance-monitoring-in-c)

The performance of your application/program matters a lot as it consumes the memory and processor of your system. The program’s performance can be optimized by tracking and customizing these parameter values efficiently.

The processor of the system intimates mostly the CPU of the system. In C#, the CPU usage by a single process and the whole processor can be tracked.

Memory counters can be used to track memory management issues—the namespace `System.Diagnostics` provides the class `PerformanceCounter`, which implements all the performance counter matrices.

We can import this namespace and use the `PerformanceCounter` class methods to diagnose our system usage. Following are some performance counters and their descriptions.

We use the format `Object Name: Counter - Description` in enlisting the following counters where `Object` is Performance Monitor, and the Counter is the name of the counter supported by the `PerformanceCounter` class.

### [Processor Performance](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/#processor-performance)

1. `Processor`: `% Processor Time` - This counter indicates the time the processor actively devotes to operating on useful threads and how frequently it was occupied with responding to requests.
    
    This counter calculates the percentage of time the system is inactive divided by `100%`. It is easy for the processor to calculate this parameter as it never sits idle; it always has something to do.
    
2. `Processor`: `% User Time` - This counter’s value aids in identifying the type of processing impacting the system. The outcome is the entire value of productive time used for `User` mode operations. Typically, this refers to program code.
    
3. `Processor`: `% Interrupt Time` - This shows how much of the processor’s time is being used to handle `Interrupts`.
    
4. `Process`: `% Processor Time` - This counter gives us the value of time that this particular process uses the processor.
    
5. `System`: `% Total Processor Time` - This counter will give you the whole system’s performance by joining the activities of all the processors. This value will equal the `%Processor Time` in a single processor system.
    

### [Memory Performance](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/#memory-performance)

1. `Memory`: `Available Memory` - This number shows how much memory is still available after paged pool allocations, nonpaged pool allocations, process working sets, and file system cache have finished.
2. `Memory`: `Committed Bytes` - This counter shows the allocated amount of memory solely for usage by any Windows NT processes or services.
3. `Memory`: `Page Faults/sec` - This counter specifies the number of page faults. Page fault is when the required information is missing in the cache or virtual memory.
4. `Memory`: `Pages/sec` - This counter shows how often the system accessed the hard drive to store or retrieve memory-associated data.
5. `Process`: `Working Set` - This shows the current memory area size that the process utilizes for code, threads, and data.

## [Demonstrate the Performance Monitoring Counters in `C#`](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/#demonstrate-the-performance-monitoring-counters-in-c)

We will create a C# example code to demonstrate the performance monitoring counters. Let’s create a C# empty project either using Visual Studio or Visual Studio Code.

The first step is to import the `Diagnostics` namespace into your project. Import it using the following code:

C

c#Copy`using System.Diagnostics;` 

We have to create objects of the class `System.Diagnostics.PerformanceCounter`. Create two objects, one for memory usage recording and the other for CPU usage, such as follows:

C

c#Copy`protected static PerformanceCounter CPUCounter;
protected static PerformanceCounter memoryCounter;` 

The next step is to initialize these objects. We can initialize them using two ways in C#.

1. You can initialize an object by passing the parameter values at the time of its creation. In this method, you don’t need to call the default constructor.
    
    For example:
    
    C
    
    c#Copy`protected static PerformanceCounter CPUCounter = new PerformanceCounter(Object Name, Counter, Instance);
    protected static PerformanceCounter memoryCounter = new PerformanceCounter(Category Name, Counter);` 
    
2. The other way is to initialize an object using a `new` keyword and default constructor. The parameter values are set using the dot identifier to access the variables.
    
    For example:
    
    C
    
    c#Copy`//Initialize the objects
    CPUCounter = new PerformanceCounter();
    memoryCounter = new PerformanceCounter();
    
    //Assign values
    CPUCounter.CategoryName = Object Name;
    CPUCounter.CounterName = Counter;
    CPUCounter.InstanceName = Instance;
    
    memoryCounter.CategoryName = Object Name;
    memoryCounter.CounterName = Counter;` 
    
    After initialization, the next step is to calculate values. The `NextValue()` method is used to get the value of counters.
    
    We need to call these methods using the respective objects created above.
    
    C
    
    c#Copy`Console.WriteLine(CPUCounter.NextValue());` 
    

### [Retrieve Total CPU and Memory Usage](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/#retrieve-total-cpu-and-memory-usage)

Following is the complete code to get the entire CPU and memory usage.

C

c#Copy`using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
  class Program
  {
	  //create objects
	  protected static PerformanceCounter CPUCounter;
	  protected static PerformanceCounter memoryCounter;
	  static void Main(string[] args)
	  {

		  //initialize objects
		  CPUCounter = new PerformanceCounter();
		  memoryCounter = new PerformanceCounter();

		  //assign parameters
		  CPUCounter.CategoryName = "Processor";
		  CPUCounter.CounterName = "% Processor Time";
		  CPUCounter.InstanceName = "_Total";

		  memoryCounter.CategoryName = "Memory";
		  memoryCounter.CounterName = "Available MBytes";

		  //invoke the following monitoring methods
		  getCpuUsage();
		  getMemory();
		  Console.ReadKey();
	  }

	  //prints the value of total processor usage time
	  public static void getCpuUsage()
	  {

		  Console.WriteLine(CPUCounter.NextValue() + "%");
	  }

	  //prints the value of available memory
	  public static void getMemory()
	  {

		Console.WriteLine(memoryCounter.NextValue()+"MB");
	  }
   }
}` 

The output of the above code snippet will be the CPU usage in `%` and available memory in `MB` of your system.

### [Retrieve CPU and Memory Usage of the Current Process](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/#retrieve-cpu-and-memory-usage-of-the-current-process)

You can use the following code snippet to get the CPU and memory usage for the current process.

C

c#Copy`using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
  class Program
  {
	  protected static PerformanceCounter CPUCounter;
	  protected static PerformanceCounter memoryCounter;
	  static void Main(string[] args)
	  {

		  CPUCounter =new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

		  memoryCounter = new PerformanceCounter("Memory", "Available MBytes");

		  getCpuUsage();
		  getMemory();
		  Console.ReadKey();
	  }

	  public static void getCpuUsage()
	  {
		  Console.WriteLine(CPUCounter.NextValue() + "%");
	  }

	  public static void getMemory()
	  {
		Console.WriteLine(memoryCounter.NextValue()+"MB");
	  }
   }
}` 

The output of the above code snippet will be the processor used by the current process and your system’s available memory.

### [Retrieve CPU and Memory Usage of a Specific Application](https://www.delftstack.com/howto/csharp/csharp-get-cpu-usage/#retrieve-cpu-and-memory-usage-of-a-specific-application)

If you want to get the CPU usage by a specific application, you can use the application name as an instance in the `PerformanceCounter` constructor.

C

c#Copy`PerformanceCounter("Process", "% Processor Time", app_name);` 

Here is the complete example code to get the CPU usage by any specific app.

C

c#Copy`using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
  class Program
  {

	  static void Main(string[] args)
	  {

		  CPUUsagebyApp("chrome");

	  }

	  private static void CPUUsagebyApp(string app_name)
	  {

		  PerformanceCounter totalCPUUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		  PerformanceCounter processCPUUsage = new PerformanceCounter("Process", "% Processor Time", app_name);

		  //loop will be executed while you don't press the enter key from the keyboard
		  //you have to press space or any other key to iterate through the loop.
		  do
		  {
			  float total = totalCPUUsage.NextValue();
			  float process = processCPUUsage.NextValue();

			  Console.WriteLine(String.Format("App CPU Usage = {0} {1}%", total, process, process / total * 100));

			  System.Threading.Thread.Sleep(1000);

		  } while (Console.ReadKey().Key != ConsoleKey.Enter);
	  }
   }
}` 

The output of this code snippet will be Chrome’s total CPU usage and CPU usage as we passed the `"chrome"` as the parameter. You can read [Performance Monitoring in C#](https://learn.microsoft.com/en-us/previous-versions//cc768048(v=technet.10)?redirectedfrom=MSDN) to understand in depth.