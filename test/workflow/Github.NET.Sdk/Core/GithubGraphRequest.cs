using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Github.NET.Sdk
{

    /// <summary>
    /// https://docs.github.com/en/graphql/guides/forming-calls-with-graphql
    /// </summary>
    public static class GithubGraphRequest
    {
        private static readonly HttpClient _client;
        private static readonly JsonSerializerOptions _jsonSerializer;
        static GithubGraphRequest()
        {

            _jsonSerializer = new JsonSerializerOptions();
            _jsonSerializer.PropertyNameCaseInsensitive = true;
            _client = new();
            _client.BaseAddress = new Uri("https://api.github.com/graphql");
            _client.DefaultRequestHeaders.Add("X-Github-Next-Global-ID", "1");
            _client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.bane-preview+json");
            _client.DefaultRequestHeaders.Add("User-Agent", "Awesome-Octocat-App");

        }

        public static void SetSecretByEnvKey(string envKey = "GITHUB_TOKEN")
        {
            var key = Environment.GetEnvironmentVariable(envKey);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
        }
#if DEBUG
        public static void SetSecret(string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
#endif



        //public static async Task<string> Test()
        //{
        //    var requestBody = new StringContent("{\"query\":\"query{organization(login:\"night-moon-studio\"){login id}}\"");
        //    Console.WriteLine(requestBody.ReadAsStringAsync().Result);
        //    var response = await _client.PostAsync(string.Empty, requestBody);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return await response.Content.ReadAsStringAsync();
        //    }
        //    Console.WriteLine(await response.Content.ReadAsStringAsync());
        //    return string.Empty;
        //}


        //        public static async Task<string> GetStringAsync(string requestJson, bool isMutation)
        //        {
        //            var requestBody = new StringContent($"{{\"query\":\"{(isMutation? "mutation" : "query")}{{{requestJson}}}\"}}");
        //#if DEBUG
        //            Console.WriteLine("============================================");
        //            Console.WriteLine(await requestBody.ReadAsStringAsync());
        //#endif
        //            var response = await _client.PostAsync(string.Empty, requestBody);
        //#if DEBUG
        //            Console.WriteLine("---------------------------------------------");
        //            Console.WriteLine(await response.Content.ReadAsStringAsync());
        //            Console.WriteLine("============================================");
        //#endif
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return await response.Content.ReadAsStringAsync();
        //            }
        //#if DEBUG
        //            Console.WriteLine(await response.Content.ReadAsStringAsync());
        //#endif
        //            return string.Empty;
        //        }

        public static async Task<(T?,string)> GetResultAsync<T>(string requestJson, bool isMutation)
        {
            var requestBody = new StringContent($"{{\"query\":\"{(isMutation? "mutation" : "query")}{{{requestJson}}}\"}}");
#if DEBUG
            Console.WriteLine("============================================");
            Console.WriteLine(await requestBody.ReadAsStringAsync());
#endif

            var response = await _client.PostAsync(string.Empty, requestBody);
            try
            {
#if DEBUG
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                Console.WriteLine("============================================");
#endif
                if (response.IsSuccessStatusCode)
                {
                    return (await response.Content.ReadFromJsonAsync<T>(_jsonSerializer),string.Empty);
                }
                else
                {
#if DEBUG
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
#endif
                    return (default(T?), await response.Content.ReadAsStringAsync());
                }

            }
            catch (Exception ex)
            {
                return (default, $"异常错误:{ex.Message} API 报错:{await response.Content.ReadAsStringAsync()}");
            }

        }



        public static GraphEntity Query()
        {
            return new GraphEntity();
        }
        public static GraphEntity Mutation()
        {
            return new GraphEntity(true);
        }

        public static Task<(T?,string)> GraphResultAsync<T>(this GraphEntity requestEntity)
        {
            return GetResultAsync<T>(requestEntity.ToString(), requestEntity.IsMutation);
        }
        //public static Task<string> GraphStringAsync(this GraphEntity requestEntity)
        //{
        //    return GetStringAsync(requestEntity.ToString(), requestEntity.IsMutation);
        //}
    }


    public class GraphBase
    {
        protected readonly StringBuilder _body;
        public GraphBase()
        {
            _body = new StringBuilder();
        }
        public override string ToString()
        {
            return _body.ToString();
        }
    }

    public sealed class GraphEntity : GraphBase
    {
        public readonly bool IsMutation;
        public GraphEntity(bool isMutation = false)
        {
            IsMutation = isMutation;
        }
        private string _nodeName = string.Empty;
        private string _methodParameter = string.Empty;
        private bool _hasChild;
        public GraphEntity DefineWithStrongType(string methodOrEntityName, Action<GraphParameter>? paramAction = null)
        {
            return Define($"...on {methodOrEntityName}", paramAction);
        }
        public GraphEntity Define(string methodOrEntityName, Action<GraphParameter>? paramAction = null)
        {
            _nodeName = methodOrEntityName;
            if (paramAction != null)
            {
                var paramObj = new GraphParameter(IsMutation);
                paramAction(paramObj);
                _methodParameter = paramObj.ToString();
            }
            return this;
        }


        public override string ToString()
        {
            if (_hasChild)
            {
                return $"{_nodeName}{_methodParameter}{{{_body}}}";
            }
            else
            {
                return _nodeName;
            }
            
        }


        //public GraphEntity Child(string methodOrEntityName)
        //{
        //    _body.Append(methodOrEntityName);
        //    return this;
        //}
        public GraphEntity Child(params string[] fields)
        {
            _hasChild = true;
            for (int i = 0; i < fields.Length; i++)
            {
                _body.Append($"{fields[i]} ");
            }
            return this;
        }
        public GraphEntity Child(string methodOrEntityName,  Action<GraphEntity> nodeAction)
        {

            _hasChild = true;
            GraphEntity entity = new();
            entity._hasChild = true;
            entity.Define(methodOrEntityName, null);
            nodeAction(entity);
            _body.Append(entity.ToString());
            return this;

        }
        public GraphEntity Child(string methodOrEntityName, Action<GraphParameter> paramAction , Action<GraphEntity> nodeAction)
        {

            _hasChild = true;
            GraphEntity entity = new();
            entity._hasChild = true;
            entity.Define(methodOrEntityName, paramAction);
            nodeAction(entity);
            _body.Append(entity.ToString());
            return this;

        }

        public GraphEntity ChildWithStrongType(string methodOrEntityName, Action<GraphEntity> nodeAction)
        {

            _hasChild = true;
            GraphEntity entity = new();
            entity._hasChild = true;
            entity.DefineWithStrongType(methodOrEntityName, null);
            nodeAction(entity);
            _body.Append(entity.ToString());
            return this;

        }
        public GraphEntity ChildWithStrongType(string methodOrEntityName, Action<GraphParameter> paramAction, Action<GraphEntity> nodeAction)
        {

            _hasChild = true;
            GraphEntity entity = new();
            entity._hasChild = true;
            entity.DefineWithStrongType(methodOrEntityName, paramAction);
            nodeAction(entity);
            _body.Append(entity.ToString());
            return this;

        }

    }

    public sealed class GraphParameter : GraphBase
    {
        private readonly bool _isMutation;

        public GraphParameter(bool isMutation = false)
        {
            _isMutation = isMutation;
        }
        public GraphParameter WithParameter(string param_name, string[] param_value)
        {
            return AddParameter($"{param_name}:[\\\"{string.Join("\\\",\\\"", param_value)}\\\"]");
        }
        public GraphParameter WithParameter(string param_name, int param_value)
        {
            return AddParameter($"{param_name}:{param_value}");
        }
        public GraphParameter WithParameter(string param_name, bool param_value)
        {
            return AddParameter($"{param_name}:{param_value.ToString().ToLowerInvariant()}");
        }
        public GraphParameter WithParameter(string param_name, string param_value, bool autoTransfer = true)
        {
            if (autoTransfer)
            {
                return AddParameter($"{param_name}:\\\"{param_value.Replace("\n", "\\n").Replace("\r", "\\r")}\\\"");
            }
            return AddParameter($"{param_name}:{param_value}");
        }

        private GraphParameter AddParameter(string parameter)
        {
            _body.Append($"{parameter},");
            return this;
        }

        public override string ToString()
        {
            if (_body.Length != 0)
            {
                _body.Length -= 1;
                if (_isMutation)
                {
                    return $"(input:{{{_body}}})";
                }
                else
                {
                    return $"({_body})";
                }
            }
            return string.Empty;
        }
    }
}
