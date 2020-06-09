using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using WebApiCoreApp.Models;

namespace WebApiCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        // GET: api/Images
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Images/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }        

        // POST: api/Files
        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult UploadImages()
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var result = new List<string>();
                bool isSuccess = false;

                var files = Request.Form.Files;

                if (files.Count == 0)
                    return BadRequest();

                foreach (var file in files)
                {
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        result.Add(dbPath);
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                    }
                }

                if (isSuccess)
                    return Ok(new { result });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // POST: api/Files
        [HttpPost("base64-file")]
        [DisableRequestSizeLimit]
        public IActionResult UploadImages(UploadFile model)
        {
            try
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                bool isSuccess = false;

                if (model != null && !string.IsNullOrWhiteSpace(model.ImageData) && !string.IsNullOrWhiteSpace(model.FileName))
                {
                    byte[] imageBytes = Convert.FromBase64String(model.ImageData);
                    var fullPath = Path.Combine(pathToSave, model.FileName);
                    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                    {
                        System.IO.File.WriteAllBytes(fullPath, imageBytes);
                    }

                    isSuccess = true;
                }
                
                if (isSuccess)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // POST: api/images/uriFiles
        [HttpPost("url-file")]
        [DisableRequestSizeLimit]
        public IActionResult UploadImages(UrlFile urlFile)
        {      
            try
            {
                var webClient = new WebClient();
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                bool isSuccess = false;

                if (urlFile != null)
                {
                    var fullPath = Path.Combine(pathToSave, urlFile.FileName);
                    webClient.DownloadFile(urlFile.UploadUrl, fullPath);

                    if (System.IO.File.Exists(fullPath))
                        isSuccess = true;
                }

                if (isSuccess)
                    return Ok();
                else
                    return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }            
        }

        public static async void DownloadFile(string url, string fullPath)
        {
            using (var client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                var data = await content.ReadAsByteArrayAsync();
                using (FileStream file = System.IO.File.Create(fullPath))
                {
                    file.Write(data, 0, data.Length);
                }
            }
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
