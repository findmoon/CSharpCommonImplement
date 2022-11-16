using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlRound
{
    class MyButton:Button
    {
        public override void NotifyDefault(bool value)
        {
            base.NotifyDefault(false);
        }
    }
}
