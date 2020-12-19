using Newtonsoft.Json;
using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace ServerSide.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult SendMailAsync()
        {
            if (TempData["Response"] != null)
            {
                ViewBag.Response = TempData["Response"].ToString();
            }
            return View();
        }


        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SendMailAsync(EmailModel emailModel)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
    Request.ApplicationPath.TrimEnd('/') + "/";

            EmailAPIModel emailAPIModel = new EmailAPIModel();

            emailAPIModel.subject = emailModel.subject;
            emailAPIModel.content = emailModel.content;

            emailAPIModel.sender = new Sender();
            emailAPIModel.sender.emailAddress = emailModel.sender;

            emailAPIModel.recipients = new List<Recipient>();
            if (!string.IsNullOrEmpty(emailModel.RecipientsTO))
            {
                foreach (var item in emailModel.RecipientsTO.Split(','))
                {
                    emailAPIModel.recipients.Add(
                        new Recipient
                        {
                            emailAddress = item.Trim(),
                            recipientType = "TO",
                        });
                }
            }
            HttpResponseMessage result = new HttpResponseMessage();

            using (var content = new MultipartFormDataContent())
            {
                HttpClient httpClient = new HttpClient();

                var json = JsonConvert.SerializeObject(emailAPIModel);
                var stringContent = new StringContent(json);

                //NOTE: convert this JSON object to the required model at the backend side
                content.Add(new StringContent(json), "mail");
                //NOTE: convert this JSON object to the required model at the backend side

                foreach (var item in emailModel.Files)
                {
                    if (item != null)
                    {
                        //content.Headers.Remove("Content-Type");
                        //content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data;");
                        content.Add(CreateFileContent(item.InputStream, item.FileName, item.ContentType), "mail");
                    }
                }

                //NOTE: change this with you API end point
                //var response = await httpClient.PostAsync("/mails/send?userID=" + emailModel.UserId, content);
                //NOTE: change this with you API end point

                var response = await httpClient.PostAsync(baseUrl + "/api/values", content);

                result = response.EnsureSuccessStatusCode();
            }
            TempData["Response"] = result.Content.ReadAsStringAsync().Result;
            return RedirectToAction("SendMailAsync");
        }

        public ActionResult ApprovalAsync()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ApprovalPostAsync()
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
    Request.ApplicationPath.TrimEnd('/') + "/";

            var httpPostedFile = Request.Files;
            var imageData = Request.Form["screenshot"].ToString();
            var data = httpPostedFile.Count;

            //var bytes = Convert.FromBase64String(imageData);

            //using (var img = new FileStream(filePath, FileMode.Create))
            //{
            //    img.Write(bytes, 0, bytes.Length);
            //    img.Flush();
            //}

            var fdata = Request.Form;
            var json = fdata["obj"];
            HttpResponseMessage result = new HttpResponseMessage();
            using (var content = new MultipartFormDataContent())
            {
                HttpClient httpClient = new HttpClient();

                var stringContent = new StringContent(json);

                //NOTE: convert this JSON object to the required model at the backend side
                content.Add(new StringContent(json), "approval");
                //NOTE: convert this JSON object to the required model at the backend side

                //files uploaded using file browser
                foreach (var item in httpPostedFile)
                {
                    HttpPostedFileBase file = Request.Files[item.ToString()];
                    if (file != null)
                    {
                        content.Add(CreateFileContent(file.InputStream, file.FileName, file.ContentType));
                    }
                }
                //files uploaded using file browser

                //files uploaded using file browser
                if (!string.IsNullOrEmpty(imageData))
                {
                    imageData = imageData.Substring(imageData.IndexOf(",") + 1);
                    Byte[] bytes = Convert.FromBase64String(imageData);
                    
                    Stream sstream = new MemoryStream(bytes);
                    content.Add(CreateFileContent(sstream, "screenshot.png", ""));
                }
                //files uploaded using file browser
                
                var response = await httpClient.PostAsync(baseUrl + "/api/values", content);

                result = response.EnsureSuccessStatusCode();
            }

            return Json(result.Content.ReadAsStringAsync().Result, JsonRequestBehavior.AllowGet);
        }

        private StreamContent CreateFileContent(Stream stream, string fileName, string contentType)
        {
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"files\"",
                FileName = "\"" + fileName + "\""
            };
            try
            {
                if (contentType == "")
                {
                    contentType = "multipart/form-data";
                }
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            }
            catch (Exception e)
            {
                throw;
            }
            return fileContent;
        }
    }
}
