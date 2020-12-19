using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSide.Models
{
    public partial class ApprovalModel
    {
        public Apln[] Aplns { get; set; }
        public string Contents { get; set; }
        public string ContentsType { get; set; }
        public string NotifyOption { get; set; }
        public string UrgnYn { get; set; }
        public string SbmDt { get; set; }
        public string Subject { get; set; }
        public string SbmLang { get; set; }
        public string ApInfId { get; set; }
        public string DocMngSaveCode { get; set; }
        public string TimeZone { get; set; }
    }

    public partial class Apln
    {
        public string EpId { get; set; }
        public string Seq { get; set; }
        public string Role { get; set; }
        public string AplnStatsCode { get; set; }
        public string ArbPmtYn { get; set; }
        public string ContentsMdfyPmtYn { get; set; }
        public string AplnMdfyPmtYn { get; set; }
    }
}