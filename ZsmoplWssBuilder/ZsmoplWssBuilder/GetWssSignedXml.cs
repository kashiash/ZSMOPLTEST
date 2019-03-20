using Gabos.Zsmolp.Client;
using RGiesecke.DllExport;
using System.Runtime.InteropServices;

namespace ZsmoplWssBuilder
{
    public static class WssSignedXml
    {
        [DllExport(nameof(GetWssSignedXmlX), CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static string GetWssSignedXmlX([MarshalAs(UnmanagedType.BStr)] string certificate,
            [MarshalAs(UnmanagedType.BStr)]  string certPassword,
            [MarshalAs(UnmanagedType.BStr)]  string bodyXml)
        {
            var Odp = ZsmoplFactory.GetSignedRequest(certificate, certPassword, bodyXml);

            return Odp;
        }

        [DllExport(nameof(GetWssSignedXml), CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static string GetWssSignedXml([MarshalAs(UnmanagedType.BStr)] string certificate,
            [MarshalAs(UnmanagedType.BStr)] string certPassword,
            [MarshalAs(UnmanagedType.BStr)] string bodyXml
            )
        {
            var Odp = ZsmoplFactory.GetSignedRequest(certificate, certPassword, bodyXml);

            return Odp;
        }

        [DllExport(nameof(GetWssSignedFullXml), CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static string GetWssSignedFullXml([MarshalAs(UnmanagedType.BStr)] string certificate,
            [MarshalAs(UnmanagedType.BStr)] string certPassword,
            [MarshalAs(UnmanagedType.BStr)] string bodyXml
    )
        {
            var Odp = ZsmoplFactory.GetSignedRequestFullBody(certificate, certPassword, bodyXml);

            return Odp;
        }
    }
}