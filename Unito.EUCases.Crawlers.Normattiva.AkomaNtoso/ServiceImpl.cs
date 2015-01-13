using mshtml;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Crawlers.Normattiva.AkomaNtoso
{
    public class ServiceImpl
    {
        public void GetAllFile(string sourceFolderPath, string destinationFolderPath)
        {
            foreach (var file in Directory.GetFiles(sourceFolderPath))
            {
                string nakedFile = string.Empty;
                nakedFile = GetSingleFileNaked(Path.Combine(sourceFolderPath, file));
                File.WriteAllText(Path.Combine(destinationFolderPath, file), nakedFile);
            }
        }

        public string GetSingleFileNaked(string FilePath)
        {
            //
            string nakedText = string.Empty;
            string rawHtmlText;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                String line = String.Empty;
                rawHtmlText = reader.ReadToEnd();                
            }

            // Obtain the document interface
            IHTMLDocument2 htmlDocument = (IHTMLDocument2)new mshtml.HTMLDocument();

            // Construct the document
            htmlDocument.write(rawHtmlText);            
            // Extract all elements
            IHTMLElementCollection allElements = htmlDocument.all;

            return nakedText;
        }

        public void GetAkomaNtosoFromOriginalText(string FilePath, string destinationFolderPath)
        {
            using (var client = new WebClient())
            {

                NameValueCollection formData = new NameValueCollection();
                //formData.Add("mesePubblicazioneDa", pMesePubblicazioneDa);

                string akomaNtosoDoc; 
                byte[] responseBytes = client.UploadValues("", "POST", formData);

                akomaNtosoDoc = Encoding.ASCII.GetString(responseBytes);
                client.CancelAsync();

            }
        }

        // deve prendere da un folder tutti i file che trova
        // deve togliergli gli HTML tag prendendolo "nudo"
        // deve fare la submit del dato con determinati parametri
        // deve memorizzare il file ricevuto su folder
    }
}
