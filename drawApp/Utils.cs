using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace drawApp
{
    public static class Utils
    {
        public static int HexToInt(string hexStr)
        {
            return int.Parse(hexStr, System.Globalization.NumberStyles.HexNumber);
        }

        public static byte IntToByte(int intVal)
        {
            return Convert.ToByte(intVal);
        }

        public static byte HexToByte(string hexStr)
        {
            return IntToByte(HexToInt(hexStr));
        }

        public static string IntToHex(int intVal)
        {
            string hexVal = intVal.ToString("X");
            if (hexVal.Length == 1) hexVal = $"0{hexVal}";
            return hexVal;
        }
    }
}
