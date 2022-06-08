using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SwaggerApiParser;

public class SwaggerSpec
{
    public string swagger { get; set; }

    public Info info { get; set; }
    public string host { get; set; }

    public string basePath { get; set; }
    public List<string> schemes { get; set; }
    public List<string> consumes { get; set; }
    public List<string> produces { get; set; }

    public List<object> security { get; set; }

    public Dictionary<string, object> securityDefinitions { get; set; }
    public Dictionary<string, Dictionary<string, Operation>> paths { get; set; }

    public Dictionary<string, Parameter> parameters { get; set; }

    public Dictionary<string, Response> responses { get; set; }

    [JsonPropertyName("x-ms-paths")] public Dictionary<string, Dictionary<string, Operation>> xMsPaths { get; set; }

    public Dictionary<string, Definition> definitions { get; set; }

    public object ResolveRefObj(string Ref)
    {
        if (Ref.Contains("parameters"))
        {
            var key = Ref.Split("/").Last();
            this.parameters.TryGetValue(key, out var ret);
            return ret;
        }

        if (Ref.Contains("definitions"))
        {
            var key = Ref.Split("/").Last();
            this.definitions.TryGetValue(key, out var ret);
            return ret;
        }

        return null;
    }
}
