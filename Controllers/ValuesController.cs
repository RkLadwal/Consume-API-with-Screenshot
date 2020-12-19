using Newtonsoft.Json;
using ServerSide.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ServerSide.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public string Post()
        {
            EmailAPIModel emailAPIModel = new EmailAPIModel();
            ApprovalModel approvalModel = new ApprovalModel();
            var httpPostedFile = HttpContext.Current.Request.Files;
            var data = httpPostedFile.Count;
            var fdata = HttpContext.Current.Request.Form;
            var model = fdata["mail"];
            if (model == null)
            {
                model = fdata["approval"];
            }

            //emailAPIModel = JsonConvert.DeserializeObject<EmailAPIModel>(model);
            approvalModel = JsonConvert.DeserializeObject<ApprovalModel>(model);
            string str = "";
            int count = 0;
            HttpFileCollection uploads = HttpContext.Current.Request.Files;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/images")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/images"));
            }
            for (int i = 0; i < uploads.Count; i++)
            {
                HttpPostedFile upload = uploads[i];
                string targetFolder = HttpContext.Current.Server.MapPath("~/images");
                string targetPath = Path.Combine(targetFolder, upload.FileName);
                upload.SaveAs(targetPath);
            }
            string files = count + " Uploaded Files From:" + str;
            return "Success";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
