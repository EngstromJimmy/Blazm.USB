
namespace Blazm.Usb.Server.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public class RisingSunProtocol : Protocol
    {

        private string GetStringSelflearning(string id, MethodEnum method)
        {
            int house = 0;
            int code = 0;
            var ids = id.Split(',');
            house = Convert.ToInt32(ids[0]);
            code = Convert.ToInt32(ids[1]);

            string[] code_on = { "110110", "001110", "100110", "010110", "111001", "000101", "101001", "011001", "110000", "001000", "100000", "010000", "111100", "000010", "101100", "011100" };
            string[] code_off = { "111110", "000001", "101110", "011110", "110101", "001101", "100101", "010101", "111000", "000100", "101000", "011000", "110010", "001010", "100010", "010010" };
            const sbyte l = 120;
            const sbyte s = 51;

            StringBuilder strCode = new StringBuilder("10");
            code = (code < 0 ? 0 : code);
            code = (code > 15 ? 15 : code);
            if (method == MethodEnum.TURNON)
            {
                strCode.Append(code_on[code]);
            }
            else if (method == MethodEnum.TURNOFF)
            {
                strCode.Append(code_off[code]);
            }
            else if (method == MethodEnum.LEARN)
            {
                strCode.Append(code_on[code]);
            }
            else
            {
                return "";
            }

            for (int i = 0; i < 25; ++i)
            {
                if ((house & 1) != 0)
                {
                    strCode.Insert(1, "1");
                }
                else
                {
                    strCode.Insert(1, "0");
                }
                house >>= 1;
            }

            StringBuilder strReturn = new StringBuilder();
            for (int i = 0; i < strCode.Length; ++i)
            {
                if (strCode[i] == '1')
                {
                    strReturn.Insert(1, l);
                    strReturn.Insert(1, s);
                }
                else
                {
                    strReturn.Insert(1, s);
                    strReturn.Insert(1, l);
                }
            }

            StringBuilder prefix = new StringBuilder("P");
            prefix.Insert(1, 5);
            if (method == MethodEnum.LEARN)
            {
                prefix.Append("R");
                prefix.Insert(1, 50);
            }
            prefix.Append("S");
            strReturn = strReturn.Insert(0, prefix);
            strReturn.Insert(1, '+');
            return strReturn.ToString();
        }

        public string GetString(string id, MethodEnum method)
        {
            int house = 0;
            int unit = 0;
            var ids = id.Split(',');
            house = Convert.ToInt32(ids[0]);
            unit = Convert.ToInt32(ids[1]);
            StringBuilder strReturn = new StringBuilder("S.e");
            strReturn.Append(getCodeSwitchTuple(house - 1));
            strReturn.Append(getCodeSwitchTuple(unit - 1));
            if (method == MethodEnum.TURNON)
            {
                strReturn.Append("e..ee..ee..ee..e+");
            }
            else if (method == MethodEnum.TURNOFF)
            {
                strReturn.Append("e..ee..ee..e.e.e+");
            }
            else
            {
                return "";
            }
            return strReturn.ToString();
        }


        private string getCodeSwitchTuple(int intToConvert)
        {
            string strReturn = "";
            for (int i = 0; i < 4; ++i)
            {
                if (i == intToConvert)
                {
                    strReturn += ".e.e";
                }
                else
                {
                    strReturn += "e..e";
                }
            }
            return strReturn;
        }

        public override string GetString(string id, TypeEnum type, MethodEnum method, char data)
        {
            if (type == TypeEnum.CodeSwitch)
                return GetString(id, method);
            else
                return GetStringSelflearning(id, method);
        }



        public override string DecodeData(string data, string model)
        {
            return "";
        }
    }

