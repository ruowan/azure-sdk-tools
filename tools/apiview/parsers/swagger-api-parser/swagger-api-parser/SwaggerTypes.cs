using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace swagger_api_parser
{
    public class SwaggerSpec
    {
        public string swagger { get; set; }
        public Dictionary<string, Dictionary<string, Operation> > paths { get; set; }
        public Info info { get; set; }
        
        
    }

    public class Info
    {
        public string title { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string termsOfService { get; set; }
    }


    public class Operation
    {

        public string description { get; set; }
        public string operationId { get; set; }
        public List<Parameter> parameters { get; set; }
        public Dictionary<string, Response> responses { get; set; }

    }

    public class Parameter
    {
        public string name { get; set; }
        public bool required { get; set; }
        public string description { get; set; }
        
        [JsonPropertyName("in")]
        public string In { get; set; }

    }
    

    public class Response
    {
        public string description { get; set; }
        public Dictionary<string, Header> headers { get; set; }
    }

    public class Header : BaseSchema
    { }

    public class BaseSchema
    {
        public string description { get; set; }
        public string type { get; set; }
        public string format { get; set; }

        [JsonPropertyName("$ref")]
        public string _ref { get; set; }

        public List<BaseSchema> items { get; set; }
    }
    
}
