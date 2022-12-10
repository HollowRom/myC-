using System;
using System.Text;

namespace noThingObject
{
    public class Class1
    {
        public static bool LinkCard() {
            return true;
        }

        public static bool UnlinkCard()
        {
            return true;
        }

        public static int ReadCardID(StringBuilder sCardId)
        {
            sCardId.Append("ReadCardID.sCardId");
            return 999;
        }

        public static bool WriteCardData(int nBlock, StringBuilder sData, int sPassType, StringBuilder sPassWord)
        {
            sData.Append("WriteCardData.sData");
            sData.Append("WriteCardData.sPassWord");
            return true;
        }

        public static bool ReadCardData(StringBuilder sData, int nBlock, int sPassType, StringBuilder sPassWord)
        {
            sData.Append("ReadCardData.sData");
            sPassWord.Append("ReadCardData.sPassWord");
            return true;
        }

        public static void GetErr(StringBuilder ErrStr)
        {
            ErrStr.Append("GetErr"); 
        }
    }
}
