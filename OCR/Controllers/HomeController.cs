using Microsoft.AspNetCore.Mvc;
using OCR.Models;
using System.Diagnostics;
using Syncfusion.OCRProcessor;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;


namespace OCR.Controllers
{
    public class HomeController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;

        public HomeController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult PerformOCR()
        {
            string docPath = _hostingEnvironment.WebRootPath + "/Data/Input.pdf";
            FileStream docStream = new FileStream(docPath, FileMode.Open, FileAccess.Read);

            //Load the PDF document 
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);

            //Initialize the OCR processor by providing the path of tesseract binaries
            using (OCRProcessor processor = new OCRProcessor())
            {
                //Language to process the OCR
                processor.Settings.Language = Languages.English;

                string fontPath = _hostingEnvironment.WebRootPath + "/Data/ARIALUNI.ttf";
                FileStream fontStream = new FileStream(fontPath, FileMode.Open, FileAccess.Read);
                processor.UnicodeFont = new PdfTrueTypeFont(fontStream, 8);

                //Process OCR by providing loaded PDF document, Data dictionary and language
                processor.PerformOCR(loadedDocument);
            }

            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);

            //Close the PDF document 
            loadedDocument.Close(true);

            //Set the position as '0'
            stream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

            fileStreamResult.FileDownloadName = "OCR.pdf";

            return fileStreamResult;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
