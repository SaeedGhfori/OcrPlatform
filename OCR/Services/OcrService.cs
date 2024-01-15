using Microsoft.AspNetCore.Mvc;
using Syncfusion.OCRProcessor;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;

namespace OCR.Services
{
    public static class OcrService
    {
        public static FileStreamResult Execute(string pathPdf)
        {
            //Load an existing PDF document.
            FileStream docStream = new FileStream(pathPdf, FileMode.Open, FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);
            //Initialize the OCR processor.
            using (OCRProcessor processor = new OCRProcessor())
            {
                //Language to process the OCR.
                processor.Settings.Language = Languages.English;
                FileStream fontStream = new FileStream("ARIALUNI.ttf", FileMode.Open, FileAccess.Read);
                processor.UnicodeFont = new PdfTrueTypeFont(fontStream, 8);
                //Process OCR by providing the loaded PDF document, Data dictionary, and language.
                processor.PerformOCR(loadedDocument);
            }

            //Save a PDF to the MemoryStream.
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            //Close a PDF document.
            loadedDocument.Close(true);
            //Set the position as '0.'
            stream.Position = 0;
            //Download a PDF document in the browser.
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = "OCR.pdf";
            return fileStreamResult;
        }
    }
}
