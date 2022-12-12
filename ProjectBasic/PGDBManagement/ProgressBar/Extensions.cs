using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public static class ProgressBarExten
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);
        /// <summary>
        /// 设置进度条的颜色。执行后，仅在下一次进度重新开始或进度结束时颜色才会生效。因此最好先改变颜色，再开始进度
        /// 但是修改颜色后，进度总是不填满100%，最后差一点。
        /// </summary>
        /// <param name="pBar"></param>
        /// <param name="state">颜色状态，取值 1 = normal (green rgb(6, 176, 37)); 2 = error (red，rgb(218, 38, 38)); 3 = warning (yellow)</param>
        public static void SetState(this ProgressBar pBar, int state)
        {
            if (pBar == null)
            {
                throw new ArgumentNullException(nameof(pBar));
            }

            SendMessage(pBar.Handle, 1040, (IntPtr)state, IntPtr.Zero);
        }

        public enum Color { None, Green, Red, Yellow }

        public static void SetState(this ProgressBar pBar, Color newColor)
        {
            if (pBar.Value == pBar.Minimum)  // If it has not been painted yet, paint the whole thing using defualt color...
            {                                // Max move is instant and this keeps the initial move from going out slowly 
                pBar.Value = pBar.Maximum;   // in wrong color on first painting
                SendMessage(pBar.Handle, 1040, (IntPtr)(int)Color.Green, IntPtr.Zero);
            }
            //pBar.Value = newValue;
            // 先执行一次Green默认颜色，占满100%，再执行设置其他颜色，可以解决其他颜色最后进度不占满100的问题。
            SendMessage(pBar.Handle, 1040, (IntPtr)(int)Color.Green, IntPtr.Zero);     // run it out to the correct spot in default
            SendMessage(pBar.Handle, 1040, (IntPtr)(int)newColor, IntPtr.Zero);        // now turn it the correct color
        }

    }
}
