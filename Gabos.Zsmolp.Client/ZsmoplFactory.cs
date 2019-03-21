﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabos.Zsmolp.Client
{
    public static class ZsmoplFactory
    {
        public static string GetSignedRequest(string certificate, string password, string body)
        {
            UnlockChilkat();
            return SignXml(certificate, password, body);
        }
        public static string GetSignedRequestFullBody(string certificate, string password, string body, string bodyPrefix, string nameSpace)
        {
            UnlockChilkat();
            return SignXmlFull(certificate, password, body, bodyPrefix, nameSpace);
        }
        private static void UnlockChilkat()
        {
            Chilkat.Global glob = new Chilkat.Global();
            var success = glob.UnlockBundle("GABOSP.CBX112020_9fFCSJMMnRBy");
            if (success != true)
            {
                Debug.WriteLine(glob.LastErrorText);
                return;
            }
        }

        public static string SignXml(string certWss, string passWss, string body)
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
                Debug.WriteLine(cert.LastErrorText);
            //  return;

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

            int nReplacedBody = sbXml.Replace("Body_Content", body);

            gen.Behaviors = "IndentedSignature";

            // Sign the XML...
            success = gen.CreateXmlDSigSb(sbXml);
            if (success != true)
                Debug.WriteLine(gen.LastErrorText);
            // return;

            // -----------------------------------------------

            // Save the signed XML to a file.
            //  success = sbXml.WriteFile(@"c:\apps\SignedRequest.xml", "utf-8", false);

            Debug.WriteLine(sbXml.GetAsString());

            success = VerifySignature(sbXml);

            return sbXml.ToString();
        }

        public static string WyslijOS(string certWss, string passWss, string xBody)
        {

            //  var xmlRequest2 = ZsmoplFactory.SignXml(certWss, passWss, xBody);
            string bodyPrefix = "obs";
            string nameSpace = @"http://csioz.gov.pl/zsmopl/ws/obslugakomunikatow/";
            var xmlRequest2 = ZsmoplFactory.SignXmlFull(certWss, passWss, xBody, bodyPrefix, nameSpace);

            var response2 = ZsmoplFactory.SendRequest(certWss, passWss, xmlRequest2, "zapiszKomunikatOS", "/cxf/zsmopl/ws");
            return response2;

        }

        public static string PobierzStatus(string certWss, string passWss, string id)
        {

            var xmlRequest2 = GetSignedStatusRequest(certWss, passWss, id);

            var response2 = ZsmoplFactory.SendRequest(certWss, passWss, xmlRequest2, "zapytajOStatusKomunikatu", "/cxf/statuskomunikatudmz/");
            return response2;

        }

        public static string GetSignedStatusRequest(string certificate, string certPassword, string iD)
        {

            string body = $"<stat:zapytajOStatusKomunikatu><komunikat><identyfikatorKomunikatu>{iD}</identyfikatorKomunikatu></komunikat></stat:zapytajOStatusKomunikatu>";
            string bodyPrefix = "stat";
            string nameSpace = @"http://csioz.gov.pl/zsmopl/ws/statuskomunikatudmz/";
            return SignXmlFull(certificate, certPassword, body, bodyPrefix, nameSpace);
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



        #region podpis pelnego body
        public static string SignXmlFull(string certWss, string passWss, string body, string bodyPrefix, string nameSpace)
        {
            bool success = true;
            // Create the above XML to be signed...
            Chilkat.Xml xmlToSign = new Chilkat.Xml();
            xmlToSign.Tag = "soapenv:Envelope";
            xmlToSign.AddAttribute($"xmlns:{bodyPrefix}", $"{nameSpace}");
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
            xmlToSign.UpdateChildContent("soapenv:Body", "Body_Content");


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
            gen.AddSameDocRef("id-396BB6026342EB5C0E1EA73593B3CC098", "sha1", "EXCL_C14N", bodyPrefix, "");

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
                Debug.WriteLine(cert.LastErrorText);
            //  return;

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

            int nReplacedBody = sbXml.Replace("Body_Content", body);

            gen.Behaviors = "IndentedSignature";

            // Sign the XML...
            success = gen.CreateXmlDSigSb(sbXml);
            if (success != true)
                Debug.WriteLine(gen.LastErrorText);
            // return;

            // -----------------------------------------------

            // Save the signed XML to a file.
            //  success = sbXml.WriteFile(@"c:\apps\SignedRequest.xml", "utf-8", false);
            success = sbXml.WriteFile($"c:\\apps\\zsmoplSignedXml{DateTime.Now.ToLongTimeString()}.xml", "utf-8", false);

            Debug.WriteLine(sbXml.GetAsString());

            success = VerifySignature(sbXml);

            return sbXml.ToString();
        }
        #endregion

        public static string SendRequest(string certWss, string passWss, string xmlRequest, string action, string path)
        {
            Console.WriteLine("************************************************Request************************************************");
            Console.WriteLine(xmlRequest);
            Chilkat.Http http = new Chilkat.Http();

            //  Set the certificate to be used for mutual TLS authentication
            //  (i.e. sets the client-side certificate for two-way TLS authentication)
            var success = http.SetSslClientCertPfx(certWss, passWss);
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
