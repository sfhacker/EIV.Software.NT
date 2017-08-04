/// <summary>
/// 
/// </summary>
namespace EIV.UI.ServiceContext.Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using EIV.UI.ServiceContext.Interface;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public sealed class ServiceResponse : IServiceResponse
    {
        private int statusCode = 0;
        private string queryUri = string.Empty;
        private string reasonPhrase = string.Empty;
        private string responseContent = string.Empty;

        // it throws an exception here:
        // 'El valor de Count no forma parte del flujo de respuesta.'
        // this.totalCount = response.TotalCount;
        private long totalCount = -1;

        private Type entityType = null;
        private IEnumerator items = null;
        private IEnumerable<object> itemList = null;

        private string entityState = string.Empty;

        private string errorMessage = string.Empty;
        private string errorExtraMessage = string.Empty;

        //private IList<IEnumerable> queries = null;

        // TODO: internal
        public ServiceResponse(InvalidOperationException response)
        {
            if (response == null)
            {
                return;
            }

            this.statusCode = 400;  // Bad request
            this.reasonPhrase = "Invalid Operation Exception";
            this.errorMessage = response.Message;
        }

        public ServiceResponse(Exception response)
        {
            if (response == null)
            {
                return;
            }

            this.statusCode = 400;   // Bad request
            this.errorMessage = response.Message;
        }

        // here when DataServiceRequestException
        public ServiceResponse(int statusCode, string message, Exception response)
        {
            this.statusCode = statusCode;
            this.reasonPhrase = message;

            if (response == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(response.Message))
            {
                try
                {
                    JObject errorJSon = JObject.Parse(response.Message);
                    if (errorJSon != null)
                    {
                        HttpErrorResponseJSon errorObject = errorJSon.ToObject<HttpErrorResponseJSon>();
                        if (errorObject != null)
                        {
                            this.errorMessage = errorObject.Error.Message;
                            if (errorObject.Error.InnerMessage != null)
                            {
                                string innerMsg = errorObject.Error.InnerMessage.Message;
                                if (innerMsg.EndsWith("\r\n"))
                                {
                                    char[] removeChars = new char[2] { '\r', '\n' };

                                    this.errorExtraMessage = innerMsg.TrimEnd(removeChars);
                                }
                                else
                                {
                                    this.errorExtraMessage = innerMsg;
                                }
                            }
                        }
                    }
                }
                catch (Exception jsonEx)
                {
                    this.errorMessage = response.Message;
                    if (response.InnerException != null)
                    {
                        this.errorExtraMessage = response.InnerException.Message;
                    }
                }
            }
        }

        public int StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }

        public Type EntityType
        {
            get
            {
                return this.entityType;
            }
            internal set
            {
                this.entityType = value;
            }
        }

        public string EntityState
        {
            get
            {
                return this.entityState;
            }
            internal set
            {
                this.entityState = value;
            }
        }

        public IEnumerator Items
        {
            get
            {
                return this.items;
            }
            internal set
            {
                this.items = value;
            }
        }

        public IEnumerable<object> ItemList
        {
            get
            {
                return this.itemList;
            }
            internal set
            {
                this.itemList = value;
            }
        }

        public string Error
        {
            get
            {
                return this.errorMessage;
            }
            internal set
            {
                this.errorMessage = value;
            }
        }

        public string ExtraError
        {
            get
            {
                return this.errorExtraMessage;
            }
            internal set
            {
                this.errorExtraMessage = value;
            }
        }
        public string QueryUri
        {
            get
            {
                return this.queryUri;
            }
            internal set
            {
                this.queryUri = value;
            }
        }

        public string Message
        {
            get
            {
                return this.reasonPhrase;
            }
            internal set
            {
                this.reasonPhrase = value;
            }
        }

        public string Content
        {
            get
            {
                return this.responseContent;
            }
        }

        public void SetContent(string content)
        {
            this.responseContent = content;
        }
    }

    internal sealed class HttpErrorResponseJSon
    {
        [JsonProperty(PropertyName = "error")]
        public HttpErrorCodeJSon Error { get; set; }
    }

    internal sealed class HttpErrorCodeJSon
    {
        [JsonProperty(PropertyName = "code")]
        //public int Code { get; set; } // Orig. ; Int or String?
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        // Added at home
        [JsonProperty(PropertyName = "innererror")]
        public HttpErrorInnerMessageJSon InnerMessage { get; set; }
    }

    internal sealed class HttpErrorInnerMessageJSon
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "stacktrace")]
        public string StackTrace { get; set; }
    }
}