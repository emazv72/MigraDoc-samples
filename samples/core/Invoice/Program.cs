using System;
using System.Diagnostics;
using System.IO;
using MigraDoc.Rendering;

namespace Invoice
{
    /// <summary>
    /// This sample shows how to create a simple invoice of a fictional book store. The invoice document
    /// is created with the MigraDoc document object model and then rendered to PDF with PDFsharp.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // try
            // {
            // Create an invoice form with the sample invoice data.
            var invoice = new InvoiceForm("../../../../assets/xml/invoice.xml");

            // Create the document using MigraDoc.
            var document = invoice.CreateDocument();
            document.UseCmykColor = true;

#if DEBUG
            // For debugging only...
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
            //MigraDoc.DocumentObjectModel.IO.Xml.DdlWriter.WriteToFile(document, "MigraDoc.xml");

            var document2 = MigraDoc.DocumentObjectModel.IO.DdlReader.DocumentFromFile("MigraDoc.mdddl");

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
						sw.WriteLine("{0}:{1} {2} {3}",  error.SourceLine, error.SourceColumn, error.ErrorLevel, error.ErrorMessage);

					}

				}

			}

			//var document3 = MigraDoc.DocumentObjectModel.IO.Xml.DdlReader.DocumentFromFile("MigraDoc.xml");
			//document = document2;
			// With PDFsharp 1.50 beta 3 there is a known problem: the blank before "by" gets lost while persisting as MDDDL.
#endif

			// Create a renderer for PDF that uses Unicode font encoding.
			var pdfRenderer = new PdfDocumentRenderer(true);

            // Set the MigraDoc document.
            pdfRenderer.Document = document3;

            // Create the PDF document.
            pdfRenderer.RenderDocument();

            // Save the PDF document...
            var filename = "Invoice.pdf";
#if DEBUG
            // I don't want to close the document constantly...
            filename = "Invoice-" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
#endif
            pdfRenderer.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
            //}
            //catch (Exception ex)
            //{
            //  Console.WriteLine(ex.Message);
            //Console.ReadLine();
            //}
        }
    }
}
