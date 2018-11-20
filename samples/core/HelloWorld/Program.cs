using System.Diagnostics;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.Rendering;
using System.IO;

namespace HelloWorld
{
    /// <summary>
    /// This sample is the obligatory Hello World program for MigraDoc documents.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create a MigraDoc document.
            var document = CreateDocument();

#if DEBUG


			MigraDoc.DocumentObjectModel.IO.Xml.DdlWriter.WriteToFile(document, "MigraDoc.xml");

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
#endif


			// ----- Unicode encoding and font program embedding in MigraDoc is demonstrated here. -----

			// A flag indicating whether to create a Unicode PDF or a WinAnsi PDF file.
			// This setting applies to all fonts used in the PDF document.
			// This setting has no effect on the RTF renderer.
			const bool unicode = true;

            // Create a renderer for the MigraDoc document.
            var pdfRenderer = new PdfDocumentRenderer(unicode);

            // Associate the MigraDoc document with a renderer.
            pdfRenderer.Document = document3;

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();

            // Save the document...
            const string filename = "HelloWorld.pdf";
            pdfRenderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

        /// <summary>
        /// Creates an absolutely minimalistic document.
        /// </summary>
        static Document CreateDocument()
        {
            // Create a new MigraDoc document.
            var document = new Document();

            document.Styles[StyleNames.Normal].Font.Name = "Lucida Sans";

            // Add a section to the document.
            var section = document.AddSection();

            // Add a paragraph to the section.
            var paragraph = section.AddParagraph();

            // Set font color.
            //paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            paragraph.Format.Font.Color = Colors.DarkBlue;

            // Add some text to the paragraph.
            paragraph.AddFormattedText("Hello, World!", TextFormat.Bold);

            // Create the primary footer.
            var footer = section.Footers.Primary;

            // Add content to footer.
            paragraph = footer.AddParagraph();
            paragraph.Add(new DateField() { Format = "yyyy/MM/dd HH:mm:ss" });
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            return document;
        }
    }
}
