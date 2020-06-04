using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace WebApiCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // GET: api/Files
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "file1", "file2" };
        }

        // GET: api/Files/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        //// POST: api/Files
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // POST: api/Files
        [HttpPost("upload-file")]
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
        [HttpPost("upload-file")]
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


        // PUT: api/Files/5
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
