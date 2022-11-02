using System;
using System.Threading;

namespace SendInputsDemo
{
    /// <summary>
    /// SendInput 发送鼠标或键盘事件的函数，本Demo出自：[How to Send Inputs using C#](https://www.codeproject.com/Articles/5264831/How-to-Send-Inputs-using-Csharp)
    /// SendInput 是旧事件方法 mouse_event、keybd_event 的替代，因此，对于模拟win的鼠标和键盘，推荐使用SendInput
    /// 本demo实现了对 SendInput 的简单封装，基本直接调用即可，在用到时需要查看相关的 键盘Scancode值、 SetCursorPosition、GetCursorPosition 设置获取鼠标位置等
    /// </summary>
    class Program
    {
        static void Main()
        {
            // If we want to click a special (extended) key like Volume up
            // We need to send to inputs with ExtendedKey and Scancode flags
            // First is 0xe0 and the second is the special key scancode we want
            // You can read more on that here -> https://www.win.tue.nl/~aeb/linux/kbd/scancodes-6.html#microsoft
            InputSender.SendKeyboardInput(new InputSender.KeyboardInput[]
            {
                new InputSender.KeyboardInput
                {
                    wScan = 0xe0,
                    dwFlags = (uint)(InputSender.KeyEventF.ExtendedKey | InputSender.KeyEventF.Scancode),
                },
                new InputSender.KeyboardInput
                {
                    wScan = 0x30,
                    dwFlags = (uint)(InputSender.KeyEventF.ExtendedKey | InputSender.KeyEventF.Scancode)
                }
            });  // Volume +

            // Using our ClickKey wrapper to press W
            // To see more scancodes see this site -> https://www.win.tue.nl/~aeb/linux/kbd/scancodes-1.html
            InputSender.ClickKey(0x11); // W

            Thread.Sleep(1000);

            // Setting the cursor position
            InputSender.SetCursorPosition(100, 100);

            Thread.Sleep(1000);

            // Getting the cursor position
            var point = InputSender.GetCursorPosition();
            Console.WriteLine(point.X);
            Console.WriteLine(point.Y);

            Thread.Sleep(1000);

            // Setting the cursor position RELATIVE to the current position
            InputSender.SendMouseInput(new InputSender.MouseInput[]
            {
                new InputSender.MouseInput
                {
                    dx = 100,
                    dy = 100,
                    dwFlags = (uint)InputSender.MouseEventF.Move
                }
            });
        }
    }
}
