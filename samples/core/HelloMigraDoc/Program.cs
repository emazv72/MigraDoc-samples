using System;
using System.Diagnostics;
using System.IO;
using MigraDoc.Rendering;

namespace HelloMigraDoc
{
    /// <summary>
    /// This sample shows some features of MigraDoc.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create a MigraDoc document.
            var document = Documents.CreateDocument();

			//var ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
			//MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
			
			//MigraDoc.DocumentObjectModel.IO.Xml.DdlWriter.WriteToFile(document, "MigraDoc.xml");

			MigraDoc.DocumentObjectModel.Document document3 = null;

			using (StreamReader sr = File.OpenText("MigraDoc.xml"))
			{
				var errors = new MigraDoc.DocumentObjectModel.IO.DdlReaderErrors();
				var reader = new MigraDoc.DocumentObjectModel.IO.Xml.DdlReader(sr, errors);

				document3 = reader.ReadDocument();

				using (StreamWriter sw = new StreamWriter("MigraDoc.xml.error"))
				{
					foreach (MigraDoc.DocumentObjectModel.IO.DdlReaderError error in errors)
					{
						sw.WriteLine("{0}:{1} {2} {3}", error.SourceLine, error.SourceColumn, error.ErrorLevel, error.ErrorMessage);

					}

				}

			}

			var renderer = new PdfDocumentRenderer(true);
            renderer.Document = document3;

            renderer.RenderDocument();

            // Save the document...
#if DEBUG
            var filename = Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
#else
            var filename = "HelloMigraDoc.pdf";
#endif
            renderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}
