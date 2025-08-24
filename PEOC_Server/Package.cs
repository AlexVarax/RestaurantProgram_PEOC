using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEOC_Server
{
    public enum TypeReq
    {
        OpenTable,
        CreateOrder,
        AddOrder,
        AddItem,
        PrintingReceipt,
        GiveDiscount,
        SplitOrder
    }

    static public class Package
    {
        public static string Packaging(TypeReq typeReq, int id)
        {
            return "";
        }
    }
}
