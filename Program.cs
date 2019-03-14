﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestZsmopl
{
    class Program
    {
        static string Namespaces = "xmlns:obs=\"http://csioz.gov.pl/zsmopl/ws/obslugakomunikatow/\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">";
        static string bodyId = "3920507AC6762568DB1539944643192202";
        static string certWss = @"c:\apps\cert\SZBcertyfikat.pfx";
        static string passWss = @"01042009";

        static string certTLS = @"c:\apps\cert\SZBcertyfikat.pfx";
        static string passTLS = @"01042009";


        static bool success = false;
        string LastErrorText = string.Empty;
        static void Main(string[] args)
        {



            Chilkat.Global glob = new Chilkat.Global();
            success = glob.UnlockBundle("Anything for 30-day trial");
            if (success != true)
            {
                Console.WriteLine(glob.LastErrorText);
                return;
            }

            Chilkat.StringBuilder request = new Chilkat.StringBuilder();
            request.Clear();
            var result = request.Append(PrepareEnvelopeXml());
            var resultWss = SignXml(request);
            //  Save the signed XMl to a file.
            success = resultWss.WriteFile($"c:\\apps\\zsmoplSignedXml{DateTime.Now.ToLongTimeString()}.xml", "utf-8", false);

            var xmlRequest = resultWss.GetAsString();

            var response = SendRequest(xmlRequest);

        }

        public static string PrepareEnvelopeXml()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"<soapenv:Envelope {Namespaces}>");
            sb.AppendLine("   <soapenv:Header>");
            sb.AppendLine(" <wsse:Security soapenv:mustUnderstand = \"1\"");
            sb.AppendLine("                  xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"");
            sb.AppendLine("                  xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">");
            sb.AppendLine("   <wsse:BinarySecurityToken");
            sb.AppendLine("                 EncodingType = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\"");
            sb.AppendLine("                 ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509PKIPathv1\"");
            sb.AppendLine("                 wsu:Id=\"x509-3920507AC6762568DB1539944643191199\">BASE64_CERT</wsse:BinarySecurityToken>");
            sb.AppendLine(" </wsse:Security>");

            sb.AppendLine(" </soapenv:Header>");
            sb.Append(PrepareBody());
            sb.AppendLine("</soapenv:Envelope>");

            return sb.ToString();
        }

        internal static string PrepareBody()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine($"<soapenv:Body xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" wsu:Id=\"id-{bodyId}\">");
            sb.AppendLine("   <obs:zapiszKomunikatOS>");
            sb.AppendLine(LoadMediqusXml());
            sb.AppendLine("   </obs:zapiszKomunikatOS>");
            sb.AppendLine("</soapenv:Body>");
            return sb.ToString();
        }

        private static string LoadMediqusXml()
        {
            string rec = File.ReadAllText(@"c:\Apps\bodybo796905559833.xml");
            return rec;
        }



        public static Chilkat.StringBuilder SignXml(Chilkat.StringBuilder requestXml)

        {
            Chilkat.Http http = new Chilkat.Http();


            if (success != true)
            {
                Debug.WriteLine(http.LastErrorText);
                //throw new Exception(http.LastErrorText);
            }


            //  -------------------------------------------------------------------------
            //  Step 2: Get the test certificate and private key stored in a .pfx
            // 
            Chilkat.BinData pfxData = new Chilkat.BinData();

            pfxData.LoadFile(certWss);
            if (success != true)
            {
                Debug.WriteLine(http.LastErrorText);
                // throw new Exception(http.LastErrorText);
            }

            Chilkat.Pfx pfx = new Chilkat.Pfx();
            string password = passWss;
            success = pfx.LoadPfxEncoded(pfxData.GetEncoded("base64"), "base64", password);
            if (success != true)
            {
                Debug.WriteLine(pfx.LastErrorText);
                throw new Exception(http.LastErrorText);
            }

            //  -------------------------------------------------------------------------
            //  Step 3: Get the certificate from the PFX.
            // 
            Chilkat.Cert cert = pfx.GetCert(0);
            if (pfx.LastMethodSuccess != true)
            {
                Debug.WriteLine(pfx.LastErrorText);
                //  throw new Exception(http.LastErrorText);
            }

            //  -------------------------------------------------------------------------
            //  Step 4: Replace "BASE64_CERT" with the actual base64 encoded certificate.
            // 
            var enc = cert.GetEncoded();
            var dec = "MIIFETCCBQ0wggL1oAMCAQICAgOTMA0GCSqGSIb3DQEBCwUAMIGTMQswCQYDVQQGEwJQTDEUMBIGA1UECAwLbWF6b3dpZWNraWUxETAPBgNVBAcMCFdhcnN6YXdhMQ4wDAYDVQQKDAVDU0lPWjENMAsGA1UECwwEV1JTVDERMA8GA1UEAwwIQ1NJT1ogQ0ExKTAnBgkqhkiG9w0BCQEWGnAuZ29sZWJpZXdza2lAY3Npb3ouZ292LnBsMB4XDTE3MTAyNTA5Mzk1MVoXDTE5MTExNDA5Mzk1MVowgaIxCzAJBgNVBAYTAlBMMRAwDgYDVQQIDAdTbGFza2llMREwDwYDVQQHDAhLYXRvd2ljZTE7MDkGA1UECgwyU3pwaXRhbCBaYWtvbnUgQm9uaWZyYXRyb3cgdyBLYXRvd2ljYWNoIHNwLiB6IG8uby4xMTAvBgNVBAMMKHVsLiBLcy4gTC4gTWFya2llZmtpIDg3LCA0MC0yMTEgS2F0b3dpY2UwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDAYnKptKAg5UD0l+TdllETLmFmdvPCnJEf2PiTTtt1c5k7UTgOCCKYeNIaSDUg4HMPqlmoBYD00B03WHjzD5Rev+NNmjdIEtuqbPin+JXp43Oe9NmrXaeBokE4n5OA5W4ctXeELeTSE4LYXr4N6OWnWO42XRlL2LrjqzGy+7CyrIiEP0UFt3EEvHpLcPY2phRvFvLrzHUPctKXA2wCTPm/REQmf5JcaBdpCoLJU8MMgT2PywFn1xxXhEWfbyBYsep8iAHW7yzKXtJfhPKEjNOU5v+G3vEzbyxX1FFYFuJ3NSZIsxp9GKDJ8As0u3QnlN/BS8aoCgAxiXsZHNMW3FCnAgMBAAGjWjBYMB0GA1UdDgQWBBRtLd4xo2eITXbAVs974mW6vnW4RDAfBgNVHSMEGDAWgBROzwzQpVHpGIt703+e7A34jpNG6zAJBgNVHRMEAjAAMAsGA1UdDwQEAwIFoDANBgkqhkiG9w0BAQsFAAOCAgEAWroFaNH/chJkFF73uwoOm3C+R47GF4Zt1ZAKZfnmtL7Fh7cOU2imippWhpRPy96Q9oExBKiWVqLgbEn/xsxkS7KwDWRpVtWHhqAdpqFAYM7VrIZh+Ei2ZjGul0cUK/adgzzSykDCcfDLxZJU3zMoVeZjQVdEgreayRKONMHLGMVsTJoWXwFhtWztsAUe9oDX9ItkPgMs3Z4Ba/DzcmmDeul5TQN4WC6YxFHnezit/2fk9h2mvNbWHkrY1a0HLQDKRiyH4gQhhO2gf0GSPcf/3fFQU/cPW1d+HALyc8lJ+QGI6KYL1xfxX8yu5gqd7leXnQf0AnPaKHSZVAZ0q737NuY35cqoasnLlz3YbBCBNtCcO8iOjFAtAp8mK2ONhiNHoto++jZGupzXkFuWXjy1Sp+vSH02o6Iuk6v23vKMB74XnxlDWNhvPW5xBowLqUuvx+hnd43dvFusvSQTrl5tgPJ6rQOe1nASWrzVDVaP5f/cUBd6CS9x+E0e7x61CVhrW23MQKLVin87Wy1Cjv1sxcCNDGH6RPAWoIJ7MmsaWJ4IjvGkBKmGltqLshw+qoYeZtpdZ0/c3R7JJYCG1O5T9HX1qvZXyiMIzxDcuVoEvH28SRNSgc2FxmVPmyvmsIp1LQu2D/+I0ML4s/X7GUYrKoytBFOKPU4FXi5ejQFLWYc=";

            int numReplaced = requestXml.Replace("BASE64_CERT", dec);

            //  -------------------------------------------------------------------------
            //  Step 5: Build the wsse:SecurityTokenReference XML.
            //  This will be the CustomKeyInfoXml (see below).
            // 
            Chilkat.Xml refXml = new Chilkat.Xml();
            //   refXml.UpdateAttrAt("ds:KeyInfo", false, "Id", "KI-3920507AC6762568DB1539944643192200");
            refXml.Tag = "wsse:SecurityTokenReference";

            refXml.AddAttribute("wsse11:TokenType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509PKIPathv1");
            refXml.AddAttribute("xmlns:wsse11", "http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd");
            refXml.AddAttribute("wsu:Id", "STR-FF238E7C061332C5B19752C2FBC8CDEF2");

            refXml.UpdateAttrAt("wsse:Reference", true, "URI", "#x509-3920507AC6762568DB1539944643191199");
            refXml.UpdateAttrAt("wsse:Reference", true, "ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509PKIPathv1");




            refXml.EmitXmlDecl = false;
            Debug.WriteLine(refXml.GetXml());

            //  -------------------------------------------------------------------------
            //  Step 6: Setup the XML Digital Signature Generator and add the XML Signature.
            // 
            Chilkat.XmlDSigGen gen = new Chilkat.XmlDSigGen();
            gen.SigId = "SIG-3920507AC6762568DB1539944643201204";
            gen.SigLocation = "soapenv:Envelope|soapenv:Header|wsse:Security";
            gen.SignedInfoPrefixList = "obs soapenv";
            gen.AddSameDocRef($"id-{bodyId}", "sha1", "C14N", "obs", "");

            gen.KeyInfoType = "Custom";
            refXml.EmitCompact = false;
         
            gen.CustomKeyInfoXml = refXml.GetXml();
            gen.SetX509Cert(cert, true);
            gen.SignedInfoDigestMethod = "sha1";
            gen.SignedInfoCanonAlg = "EXCL_C14N";// "C14N_11"; //"http://www.w3.org/TR/2001/REC-xml-c14n-20010315";// 

            success = gen.CreateXmlDSigSb(requestXml);
            if (success != true)
            {
                Debug.WriteLine(gen.LastErrorText);
                // throw new Exception(http.LastErrorText);
            }

            var xmlRequest = requestXml.GetAsString();
            requestXml.WriteFile(@"c:\apps\SignedRequest.xml", "utf-8", false);
            //  Examine the signed XML
            Debug.WriteLine(requestXml.GetAsString());
            return requestXml;
        }
        public static string SendRequest(string xmlRequest)
        {
            Console.WriteLine("************************************************Request************************************************");
            Console.WriteLine(xmlRequest);
            Chilkat.Http http = new Chilkat.Http();

            //  Set the certificate to be used for mutual TLS authentication
            //  (i.e. sets the client-side certificate for two-way TLS authentication)
            var success = http.SetSslClientCertPfx(certTLS, passTLS);
            if (success != true)
            {
                Console.WriteLine(http.LastErrorText);
                return string.Empty;
            }

            Chilkat.HttpRequest req = new Chilkat.HttpRequest();
            req.HttpVerb = "POST";
            req.ContentType = "text/xml";
            req.SendCharset = true;
            req.Charset = "utf-8";
            req.AddHeader("SOAPAction", $"\"urn:zapiszKomunikatOS\"");
            req.Path = "/cxf/zsmopl/ws";


            req.LoadBodyFromString(xmlRequest, "utf-8");



            http.FollowRedirects = true;

            //  Chilkat will automatically offer TLS 1.2.  It is the server that
            //  chooses the TLS protocol version.  Assuming the server wishes to use
            //  TLS 1.2, then that is what will be used.
            Chilkat.HttpResponse response = http.SynchronousRequest("ewa-zsmopl.ezdrowie.gov.pl", 443, true, req);
            if (http.LastMethodSuccess != true)
            {
                Console.WriteLine(http.LastErrorText);
                return string.Empty;
            }

            string xmlResponse = response.BodyStr;
            Console.WriteLine("************************************************Response************************************************");
            Console.WriteLine(xmlResponse);
            return xmlResponse;
        }
    }
}
