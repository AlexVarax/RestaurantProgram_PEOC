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
        SplitOrder,
        UpdateTables
    }

    public class Package
    {
        public TypeReq typeRequest;
        public int id_sotr;


        public static string Packaging(TypeReq typeReq, int id)
        {
            return "";
        }
    }
}
