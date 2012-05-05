using System;

namespace NewSocialNetwork.Website.Controllers.Helper
{
    [Serializable]
    public class ResponseMessage
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }

        public ResponseMessage() { }

        public ResponseMessage(string name, int status, string message)
        {
            this.Name = name;
            this.Status = status;
            this.Message = message;
        }

        public void SetStatusAndMessage(int status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}