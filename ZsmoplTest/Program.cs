using Gabos.Zsmolp.Client;
using System;
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

            //Chilkat.StringBuilder request = new Chilkat.StringBuilder();
            //request.Clear();
            //var result = request.Append(PrepareEnvelopeXml());
            //var resultWss = SignXml(request);

            //var resultWss = SignXml();
            //success = resultWss.WriteFile($"c:\\apps\\zsmoplSignedXml{DateTime.Now.ToLongTimeString()}.xml", "utf-8", false);
            //var xmlRequest = resultWss.GetAsString();
            //var response = SendRequest(xmlRequest);
            //Console.ReadLine();


            //var xBody = LoadMediqusXml();
            //var xmlRequest2 = ZsmoplFactory.SignXml(certWss, passWss, xBody);
            //var response2 = SendRequest(xmlRequest2);
            //Console.ReadLine();

            //var xBody3 = LoadMediqusXmlFull();
            //string bodyPrefix = "obs";
            //string nameSpace = @"http://csioz.gov.pl/zsmopl/ws/obslugakomunikatow/";
            //var xmlRequest3 = ZsmoplFactory.SignXmlFull(certWss, passWss, xBody3, bodyPrefix, nameSpace);
            //var response3 = SendRequest(xmlRequest3);
            //Console.ReadLine();

            var xBody4 = LoadMediqusXmlKomunikat();
            var bodyPrefix = "stat";
            var nameSpace = @"http://csioz.gov.pl/zsmopl/ws/statuskomunikatudmz/";
            string ID = "155290997373964462";
            var xmlRequest4 = ZsmoplFactory.GetSignedStatusRequest(certWss, passWss, ID);
          //  var Odp = ZsmoplFactory.GetSignedStatusRequest(certificate, certPassword, ID);
            string soapAction = "";
            string path = "/cxf/statuskomunikatudmz/";
            var response4 = SendRequest(xmlRequest4, soapAction,path);
            Console.ReadLine();


        }

        private static string LoadMediqusXmlFull()
        {
            string rec = File.ReadAllText(@"c:\Apps\ww5065981.xml");
            return $"<obs:zapiszKomunikatOS>{rec} </obs:zapiszKomunikatOS>";
        }

        private static string LoadMediqusXmlKomunikat()
        {
            return "<stat:zapytajOStatusKomunikatu><komunikat><identyfikatorKomunikatu>154686776898112672</identyfikatorKomunikatu></komunikat></stat:zapytajOStatusKomunikatu>";
        }

        private static string LoadMediqusXml()
        {
            string rec = File.ReadAllText(@"c:\Apps\ww5065981.xml");
            return rec;
        }

        public static Chilkat.StringBuilder SignXml()
        {
            bool success = true;
            // Create the above XML to be signed...
            Chilkat.Xml xmlToSign = new Chilkat.Xml();
            xmlToSign.Tag = "soapenv:Envelope";
            xmlToSign.AddAttribute("xmlns:obs", "http://csioz.gov.pl/zsmopl/ws/obslugakomunikatow/");
            xmlToSign.AddAttribute("xmlns:soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlToSign.UpdateAttrAt("soapenv:Header|wsse:Security", true, "xmlns:wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            xmlToSign.UpdateAttrAt("soapenv:Header|wsse:Security", true, "xmlns:wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            xmlToSign.UpdateAttrAt("soapenv:Header|wsse:Security", true, "soapenv:mustUnderstand", "1");
            xmlToSign.UpdateAttrAt("soapenv:Header|wsse:Security|wsse:BinarySecurityToken", true, "EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            xmlToSign.UpdateAttrAt("soapenv:Header|wsse:Security|wsse:BinarySecurityToken", true, "ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509PKIPathv1");
            xmlToSign.UpdateAttrAt("soapenv:Header|wsse:Security|wsse:BinarySecurityToken", true, "wsu:Id", "X509-02BF0107214FC61449FD0013DF68F0359");
            // Note: The content of this XML node is a placeholder that will be updated below with the X509PKIPathv1 for the signing certificate.
            xmlToSign.UpdateChildContent("soapenv:Header|wsse:Security|wsse:BinarySecurityToken", "BinarySecurityToken_Base64Binary_Content");
            xmlToSign.UpdateAttrAt("soapenv:Body", true, "xmlns:wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            xmlToSign.UpdateAttrAt("soapenv:Body", true, "wsu:Id", "id-396BB6026342EB5C0E1EA73593B3CC098");
            xmlToSign.UpdateChildContent("soapenv:Body|obs:zapiszKomunikatOS", "Body_Content");


            Chilkat.XmlDSigGen gen = new Chilkat.XmlDSigGen();

            gen.SigLocation = "soapenv:Envelope|soapenv:Header|wsse:Security";
            gen.SigLocationMod = 0;
            gen.SigId = "SIG-BB965DFC3C8AAF87903C0ED898B8D2A8D";
            gen.SigNamespacePrefix = "ds";
            gen.SigNamespaceUri = "http://www.w3.org/2000/09/xmldsig#";
            gen.SignedInfoCanonAlg = "EXCL_C14N";
            gen.SignedInfoDigestMethod = "sha1";


            // Set the KeyInfoId before adding references..
            gen.KeyInfoId = "KI-9D95C38916099AD2EE87DDAC1A76E97E4";

            // -------- Reference 1 --------
            gen.AddSameDocRef("id-396BB6026342EB5C0E1EA73593B3CC098", "sha1", "EXCL_C14N", "obs", "");

            // The reference to be produced in the Signature should look like this:

            // <ds:Reference URI="#id-396BB6026342EB5C0E1EA73593B3CC098">
            //     <ds:Transforms><ds:Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#">
            //         <ec:InclusiveNamespaces xmlns:ec="http://www.w3.org/2001/10/xml-exc-c14n#" PrefixList="obs"></ec:InclusiveNamespaces>
            //     </ds:Transform></ds:Transforms>
            //     <ds:DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"></ds:DigestMethod>
            //     <ds:DigestValue>2e9hZYj/CN2nPsgQqUraU43k3ds=</ds:DigestValue>
            // </ds:Reference>
            // 

            // Provide a certificate + private key. (PFX password is test123)
            Chilkat.Cert cert = new Chilkat.Cert();
            success = cert.LoadPfxFile(certWss, passWss);
            if (success != true)
            {
                Debug.WriteLine(cert.LastErrorText);
                //  return;
            }

            gen.SetX509Cert(cert, true);

            gen.KeyInfoType = "Custom";

            // Create the custom KeyInfo XML..
            Chilkat.Xml xmlCustomKeyInfo = new Chilkat.Xml();
            xmlCustomKeyInfo.Tag = "wsse:SecurityTokenReference";
            xmlCustomKeyInfo.AddAttribute("wsse11:TokenType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509PKIPathv1");
            xmlCustomKeyInfo.AddAttribute("xmlns:wsse11", "http://docs.oasis-open.org/wss/oasis-wss-wssecurity-secext-1.1.xsd");
            xmlCustomKeyInfo.AddAttribute("wsu:Id", "STR-FF238E7C061332C5B19752C2FBC8CDEF2");
            xmlCustomKeyInfo.UpdateAttrAt("wsse:Reference", true, "URI", "#X509-02BF0107214FC61449FD0013DF68F0359");
            xmlCustomKeyInfo.UpdateAttrAt("wsse:Reference", true, "ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509PKIPathv1");

            xmlCustomKeyInfo.EmitXmlDecl = false;
            gen.CustomKeyInfoXml = xmlCustomKeyInfo.GetXml();

            // Load XML to be signed...
            Chilkat.StringBuilder sbXml = new Chilkat.StringBuilder();
            xmlToSign.GetXmlSb(sbXml);

            // Update BinarySecurityToken_Base64Binary_Content with the actual X509PKIPathv1 of the signing cert.
            int nReplaced = sbXml.Replace("BinarySecurityToken_Base64Binary_Content", cert.X509PKIPathv1());

            int nReplacedBody = sbXml.Replace("Body_Content", LoadMediqusXml());

            gen.Behaviors = "IndentedSignature";

            // Sign the XML...
            success = gen.CreateXmlDSigSb(sbXml);
            if (success != true)
            {
                Debug.WriteLine(gen.LastErrorText);
                // return;
            }

            // -----------------------------------------------

            // Save the signed XML to a file.
            success = sbXml.WriteFile(@"c:\apps\SignedRequest.xml", "utf-8", false);

            Debug.WriteLine(sbXml.GetAsString());

            success = VerifySignature(sbXml);

            return sbXml;
        }

        private static bool VerifySignature(Chilkat.StringBuilder sbXml)
        {
            bool success;
            // ----------------------------------------
            // Verify the signatures we just produced...
            Chilkat.XmlDSig verifier = new Chilkat.XmlDSig();
            success = verifier.LoadSignatureSb(sbXml);
            if (success != true)
            {
                Debug.WriteLine(verifier.LastErrorText);
                //  return;
            }

            int numSigs = verifier.NumSignatures;
            int verifyIdx = 0;
            while (verifyIdx < numSigs)
            {
                verifier.Selector = verifyIdx;
                bool verified = verifier.VerifySignature(true);
                if (verified != true)
                {
                    Debug.WriteLine(verifier.LastErrorText);
                    //    return;
                }

                verifyIdx = verifyIdx + 1;
            }

            Debug.WriteLine("All signatures were successfully verified.");
            return success;
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
            req.AddHeader("SOAPAction", $"\"zapiszKomunikatOS\"");
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
        public static string SendRequest(string xmlRequest,string action,string path)
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
            req.AddHeader("SOAPAction", $"\"{action}\"");
            req.Path = path;


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

