using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.CompilerServices;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://annieakraft.github.io/CSE445Assignment4/NationalParks.xml";
        public static string xmlErrorURL = "https://annieakraft.github.io/CSE445Assignment4/NationalParksErrors.xml";
        public static string xsdURL = "https://annieakraft.github.io/CSE445Assignment4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            // Collect schema validation and XML parsing errors
            System.Text.StringBuilder errors = new System.Text.StringBuilder();

            try
            {
                //Configure the XML reader to validate against the XSD
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, xsdUrl);

                settings.ValidationEventHandler += (sender, e) =>
                {
                    errors.AppendLine(e.Message);
                };

                //Read the remote XML and validate it
                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read())
                    {
                        //Reading the entire document triggers validation
                    }
                }

                //Return the required message when there are no errors
                if (errors.Length == 0)
                {
                    return "No errors are found";
                }

                return errors.ToString();
            }
            catch (Exception ex)
            {
                if (errors.Length > 0)
                {
                    errors.AppendLine(ex.Message);
                    return errors.ToString().Trim();
                }

                return ex.Message;
            }
            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
            //discrepancy between template and Assignment instructions
        }

        public static string Xml2Json(string xmlUrl)
        {
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            try
            {
                //Download the XML document from the URL
                string xmlText = DownloadContent(xmlUrl);

                //Convert XML to a JSON-compatible format
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlText);

                //Remove the XML declaration so the JSON has one root object
                if (xmlDoc.FirstChild is XmlDeclaration)
                {
                    xmlDoc.RemoveChild(xmlDoc.FirstChild);
                }

                string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented, false);

//test                XmlDocument testDoc = JsonConvert.DeserializeXmlNode(jsonText);

                return jsonText;
            }
             catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }

}