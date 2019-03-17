using Gabos.Zsmolp.Client;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gabos.Zsmopl.Clarion
{
    public static class WssSignedXml
    {
        [DllExport(nameof(GetWssSignedXml), CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static string GetWssSignedXml([MarshalAs(UnmanagedType.BStr)] string certificate,
            [MarshalAs(UnmanagedType.BStr)] string certPassword, [MarshalAs(UnmanagedType.BStr)] string bodyXml)
        {
            var res = ZsmoplFactory.GetSignedRequest(certificate, certPassword, bodyXml);
            return res;
        }
    }
}
