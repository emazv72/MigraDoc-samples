using System;
using System.Diagnostics;
using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.RtfRendering;
using PdfSharp.Pdf;

namespace Images
{
    /// <summary>
    /// This is a sample program for MigraDoc documents.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Create a MigraDoc document.
            var document = CreateDocument();

#if DEBUG
			MigraDoc.DocumentObjectModel.IO.Xml.DdlWriter.WriteToFile(document, "MigraDoc.xml");
			using (StreamReader sr = File.OpenText("MigraDoc.xml"))
			{
				var errors = new MigraDoc.DocumentObjectModel.IO.DdlReaderErrors();
				var reader = new MigraDoc.DocumentObjectModel.IO.Xml.DdlReader(sr, errors);

				document = reader.ReadDocument();

				using (StreamWriter sw = new StreamWriter("MigraDoc.xml.error"))
				{
					foreach (MigraDoc.DocumentObjectModel.IO.DdlReaderError error in errors)
					{
						sw.WriteLine("{0}:{1} {2} {3}", error.SourceLine, error.SourceColumn, error.ErrorLevel, error.ErrorMessage);

					}

				}

			}
#endif

			var ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);

#if true
            var renderer = new RtfDocumentRenderer();
            renderer.Render(document, "HelloWorld.rtf", null);
#endif
            // ----- Unicode encoding and font program embedding in MigraDoc is demonstrated here. -----

            // A flag indicating whether to create a Unicode PDF or a WinAnsi PDF file.
            // This setting applies to all fonts used in the PDF document.
            // This setting has no effect on the RTF renderer.
            const bool unicode = true;

            // Create a renderer for the MigraDoc document.
            var pdfRenderer = new PdfDocumentRenderer(unicode);

            // Associate the MigraDoc document with a renderer.
            pdfRenderer.Document = document;

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();

            // Save the document...
            var filename = Guid.NewGuid() + ".pdf";
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

            // Add a section to the document.
            var section = document.AddSection();

            // Add a paragraph to the section.
            var paragraph = section.AddParagraph();

            // Add some text to the paragraph.
            paragraph.AddFormattedText("Hello, MigraDoc!", TextFormat.Italic);
            paragraph.Format.Font.Size = 20;
            section.AddImage("../../../../assets/images/MigraDoc.png");

            return document;
        }
    }
}
