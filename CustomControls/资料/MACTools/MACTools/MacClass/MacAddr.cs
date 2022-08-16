using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;

namespace ISEAGE.May610.Diagrammer
{
    /// <summary>
    /// Data type for holding MAC Addresses
    /// </summary>
    [Serializable]
    public class MacAddr
    {
        #region Constructor
        /************************************************/
        /// <summary>
        /// Initially sets all bytes to zero
        /// </summary>
        public MacAddr()
        {
            firstByte = 0;
            secondByte = 0;
            thirdByte = 0;
            fourthByte = 0;
            fifthByte = 0;
            sixthByte = 0;

        }
        /// <summary>
        /// Sets all bytes according to string addy
        /// </summary>
        /// <param name="addy">string in a MAC address format (colon seperators)</param>
        public MacAddr(string addy)
        {
            this.Address = addy;
        }


        /************************************************/
        #endregion

        #region Fields
        /************************************************/

        /// <summary>
        /// The bytes of the MAC address
        /// </summary>
        private byte firstByte;
        private byte secondByte;
        private byte thirdByte;
        private byte fourthByte;
        private byte fifthByte;
        private byte sixthByte;


        /************************************************/
        #endregion

        #region Properties
        /************************************************/

        /// <summary>
        /// Gets or sets first byte in mac address
        /// </summary>
        public byte FirstByte
        {
            get { return firstByte; }
            set { firstByte = value; }
        }
        /// <summary>
        /// Gets or sets second byte in mac address
        /// </summary>
        public byte SecondByte
        {
            get { return secondByte; }
            set { secondByte = value; }
        }
        /// <summary>
        /// Gets or sets third byte in mac address
        /// </summary>
        public byte ThirdByte
        {
            get { return thirdByte; }
            set { thirdByte = value; }
        }
        /// <summary>
        /// Gets or sets fourth byte in mac address
        /// </summary>
        public byte FourthByte
        {
            get { return fourthByte; }
            set { fourthByte = value; }
        }
        /// <summary>
        /// Gets or sets fifth byte in mac address
        /// </summary>
        public byte FifthByte
        {
            get { return fifthByte; }
            set { fifthByte = value; }
        }

        /// <summary>
        /// Gets or sets the sixth byte in the mac address
        /// </summary>
        public byte SixthByte
        {
            get { return sixthByte; }
            set { sixthByte = value; }
        }
        /// <summary>
        /// Gets or sets the string value of the entire Mac Address. 
        /// Note: includes colon seperators, e.g. "1a:2b:3c:4d:5f
        /// </summary>
        public string Address
        {
            get
            {
                string temp;

                temp = string.Format("{0:x2}:{1:x2}:{2:x2}:{3:x2}:{4:x2}:{5:x2}", firstByte, secondByte, thirdByte, fourthByte, fifthByte, sixthByte);

                return temp;
            }
            set
            {
                string[] temp;
                byte[] byTemp;
                char[] sep = new char[1];
                sep[0] = ':';
                temp = value.Split(sep, 6);
                
                //format A-F as 10-15 decimal
                byTemp = FormatByte(temp);

                firstByte = byTemp[0];
                secondByte = byTemp[1];
                thirdByte = byTemp[2];
                fourthByte = byTemp[3];
                fifthByte = byTemp[4];
                sixthByte = byTemp[5];
                
            }
        }

        /************************************************/
        #endregion

        #region Methods
        /************************************************/


        /// <summary>
        /// This essentially overrides the assignment operator (=)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static implicit operator MacAddr(string input)
        {
            return new MacAddr(input);
        }

        public override string ToString()
        {
            return this.Address;
        }

        /// <summary>
        /// Verifies that the byte of MAC Address is valid
        /// </summary>
        /// <param name="value">byte to verify</param>
        /// <returns>value if it is a valid hex value</returns>
        private byte verifyByte(byte value)
        {
            if (value < 0x0 || value > 0xff)
                throw new Exception("A byte of the MAC Address was out of bounds");
            else
                return value;
        }

        /// <summary>
        /// Converts each part of sTemp between A-F and converts to decimal 10-15, respectively
        /// </summary>
        /// <param name="sTemp"></param>
        /// <returns>sTemp</returns>
        private byte[] FormatByte(string[] sTemp)
        {
            byte[] sRetValue = new byte[6];
            char[] cAtoF = { 'A', 'B', 'C', 'D', 'E', 'F', 'a', 'b', 'c', 'd', 'e', 'f' };
            char[] c0to9 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            // iterate through each byte of the mac address
            for (int i = 0; i < 6; i++)
            {
                // iterate through both cAtoF and c0to9
                for (int j = 0; j < 12; j++)
                {
                    // shift ASCII numbers down to decimal equivilent ( 0 (48) = 0, 1 (49) = 1 ... )
                    if (j < 10)
                        sTemp[i] = new string(sTemp[i].Replace(c0to9[j], (char)((int)c0to9[j] - 48)).ToCharArray());
                    // shift ASCII uppercase A-F down to hex equivilant decimal value ( A (65) = 10 = 0xA )
                    if( j < 6 )
                        sTemp[i] = new string(sTemp[i].Replace(cAtoF[j], (char)((int)cAtoF[j] - 55)).ToCharArray());
                    // shift ASCII lowercase a-f down to hex equivilant decimal value ( A (97) = 10 = 0xA )
                    else
                        sTemp[i] = new string(sTemp[i].Replace(cAtoF[j], (char)((int)cAtoF[j] - 87)).ToCharArray());
                }
            }

            // go through the byte array and assign it to the appopriate shifted pair
            for (int i = 0; i < 6; i++)
            {
                sRetValue[i] = (byte)(sTemp[i][0] << 4 | sTemp[i][1]);
            }

            return sRetValue;
        }


        /************************************************/
        #endregion
    }
}