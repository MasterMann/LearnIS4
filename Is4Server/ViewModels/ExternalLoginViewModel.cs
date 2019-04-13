using Newtonsoft.Json;

namespace Is4Server.ViewModels
{
    public class ExternalLoginViewModel
    {
        #region Properties

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("id_token")]
        public string id_token { get; set; }
        
        public string access_token { get; set; }

        #endregion

        #region Constructor

        public ExternalLoginViewModel()
        {
            
        }
        

        #endregion
    }
}