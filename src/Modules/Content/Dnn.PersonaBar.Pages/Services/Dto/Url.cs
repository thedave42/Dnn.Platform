﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dnn.PersonaBar.Pages.Services.Dto
{
    [DataContract]
    public class Url
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        public KeyValuePair<int, string> SiteAlias { get; set; }
        [DataMember(Name = "path")]
        public string Path { get; set; }
        public string PathWithNoExtension { get; set; }
        public string QueryString { get; set; }
        [DataMember(Name = "locale")]
        public KeyValuePair<int, string> Locale { get; set; }
        [DataMember(Name = "statusCode")]
        public KeyValuePair<int, string> StatusCode { get; set; }
        public int SiteAliasUsage { get; set; }
        [DataMember(Name = "autoGenarated")]
        public bool IsSystem { get; set; }
    }
}