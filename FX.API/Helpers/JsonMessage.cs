using Newtonsoft.Json;
using System.Collections.Generic;
using FX.DTO;

namespace FX.API.Helpers
{
    public class JsonMessage<T>
    {
        [JsonProperty("result")]
        public List<T> Results { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }

        [JsonProperty(PropertyName = "error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("success_message")]
        public string SuccessMessage { get; set; }

        [JsonProperty("meta_data")]
        public MetaData MetaData { get; set; }
        [JsonProperty("status_code")]
        public int StatusCode { get; internal set; }
    }
}
