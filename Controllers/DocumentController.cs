using ABCRETAILSTORE.Models; // Corrected namespace
using ABCRETAILSTORE.Services; // Corrected namespace
using Microsoft.AspNetCore.Mvc;

namespace ABCRETAILSTORE.Controllers // Corrected namespace
{
    // This DocumentController was developed following Agile Methodology principles, 
    // specifically using an iterative and incremental approach that enables early and continuous delivery 
    // of valuable file management functionality (upload, list, download) while embracing changing requirements 
    // throughout the development cycle (Satzinger, Jackson, and Burd, 2016).
    //
    // Features were delivered in short iterations with constant feedback, focusing on working software 
    // as the primary measure of progress (Agile Manifesto Principles 1, 3 & 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices including simple design, refactoring, continuous integration, 
    // testability via dependency injection, and collective code ownership were applied 
    // (Satzinger, Jackson, and Burd, 2016).

    public class DocumentController : Controller // Renamed class
    {
        private readonly AzureFileService _fileService;
        private readonly ILogger<DocumentController> _logger; // Updated logger type - supports rapid feedback loops (XP core value: Feedback)

        public DocumentController(AzureFileService fileService, ILogger<DocumentController> logger) // Dependency injection enhances testability and refactoring (XP practice - Satzinger, Jackson, and Burd, 2016)
        {
            _fileService = fileService;
            _logger = logger;
        }

        // Index action delivers working software frequently by listing current contracts immediately 
        // (Agile Principle 3: Deliver working software frequently, from weeks to months, preferring shorter timescale - Satzinger, Jackson, and Burd, 2016)
        public async Task<IActionResult> Index()
        {
            try
            {
                var contracts = await _fileService.ListContractsAsync();
                return View(contracts);
            }
            catch (Exception ex)
            {
                // Comprehensive error logging supports concrete feedback and continuous improvement 
                // (Agile Principle 12: Reflect and adjust; XP core value: Feedback - Satzinger, Jackson, and Burd, 2016)
                _logger.LogError(ex, "Failed to load contracts list.");
                TempData["Error"] = "Error loading contracts.";
                return View(new string[0]);
            }
        }

        // Upload (GET) - Provides immediate value to the customer through early delivery of upload capability
        public IActionResult Upload()
        {
            return View();
        }

        // Upload (POST) - Welcomes changing requirements even late in development; new file uploads can be processed at any time
        // (Agile Principle 2: Welcome changing requirements, even late in development - Satzinger, Jackson, and Burd, 2016)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please select a file to upload.");
                return View();
            }
            try
            {
                await _fileService.UploadContractAsync(file);
                TempData["SuccessMessage"] = "Contract (File Storage) uploaded successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Error handling and logging enable the team to reflect and tune behavior regularly 
                // (Agile Principle 12 & XP Feedback - Satzinger, Jackson, and Burd, 2016)
                _logger.LogError(ex, "Failed to upload contract.");
                ModelState.AddModelError("", "Error uploading contract.");
                return View();
            }
        }

        // Download - Continuous attention to technical excellence and good design enhances agility; 
        // robust error handling and streaming ensure reliable delivery (Agile Principle 9 - Satzinger, Jackson, and Burd, 2016)
        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is required.");
            }
            try
            {
                var fileStream = await _fileService.DownloadContractAsync(fileName);
                if (fileStream == null)
                {
                    return NotFound("File not found.");
                }
                // Streaming response supports sustainable development pace and high-quality delivery
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to download contract: {fileName}");
                return StatusCode(500, "Error downloading file.");
            }
        }
    }

    /*
    REFERENCE LIST

    Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
    Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
    7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
    */
}
