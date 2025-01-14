using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;
using System.Text.Unicode;

namespace theTranslator.Google.Translate
{
//    You can use 108 language in target and source,details view LANGUAGES.
//    Target language: like 'en'、'zh'、'th'...

//    :param url_suffix: The source text(s) to be translated.Batch translation is supported via sequence input.
//                       The value should be one of the url_suffix listed in : `DEFAULT_SERVICE_URLS`
//    :type url_suffix: UTF-8 :class:`str`; :class:`unicode`; string sequence(list, tuple, iterator, generator)

//    :param text: The source text(s) to be translated.
//    :type text: UTF-8 :class:`str`; :class:`unicode`;

//    :param lang_tgt: The language to translate the source text into.
//                     The value should be one of the language codes listed in : `LANGUAGES`
//    :type lang_tgt: :class:`str`; :class:`unicode`

//    :param lang_src: The language of the source text.
//                    The value should be one of the language codes listed in :const:`googletrans.LANGUAGES`
//                    If a language is not specified,
//                    the system will attempt to identify the source language automatically.
//    :type lang_src: :class:`str`; :class:`unicode`

//    :param timeout: Timeout Will be used for every request.
//    :type timeout: number or a double of numbers

//    :param proxies: proxies Will be used for every request.
//    :type proxies: class : dict; like: {'http': 'http:171.112.169.47:19934/', 'https': 'https:171.112.169.47:19934/'}

public class GoogleTranslator
    {
        private static readonly List<string> URLS_SUFFIX = Constants.DEFAULT_SERVICE_URLS
            .Select(url => Regex.Match(url.Trim(), @"translate\.google\.(.*)").Groups[1].Value)
            .ToList();

        private const string URL_SUFFIX_DEFAULT = "com";
        private readonly string url;
        private readonly int timeout;
        private readonly Dictionary<string, string> proxies;

        public GoogleTranslator(string urlSuffix = "com", int timeout = 5, Dictionary<string, string>? proxies = null)
        {
            this.proxies = proxies ?? new Dictionary<string, string>();

            if (!URLS_SUFFIX.Contains(urlSuffix))
            {
                urlSuffix = URL_SUFFIX_DEFAULT;            
            }

            string urlBase = $"https://translate.google.{urlSuffix}";

            this.url = $"{urlBase}/_/TranslateWebserverUi/data/batchexecute";
            this.timeout = timeout;
        }

        private static string PackageRpc(string text, string langSrc = "auto", string langTgt = "auto")
        {
            var googleTtsRpc = new List<string> { "MkEWBc" };
            var parameter = new List<object> { new List<object> { text.Trim(), langSrc, langTgt, true }, new List<int> { 1 } };
            string escapedParameter = JsonSerializer.Serialize(parameter);
            var rpc = new List<object> { new List<object> { googleTtsRpc[new Random().Next(googleTtsRpc.Count)], escapedParameter, null, "generic" } };
            string escapedRpc = JsonSerializer.Serialize(rpc);
            string freqInitial = $"f.req={Uri.EscapeDataString(escapedRpc)}&";

            return freqInitial;
        }

        public async Task<string> TranslateAsync(string text, string langTgt = "auto", string langSrc = "auto", bool pronounce = false)
        {
            if (text.Length >= 5000)
            {
                return "Warning: Can only detect less than 5000 characters";
            }

            if (text.Length == 0)
            {
                return "Warning: Cannot process empty text";
            }

            var headers = new Dictionary<string, string>
            {
                { "Referer", $"http://translate.google.{url}/" },
                { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36" },
                //{ "Content-Type", "application/x-www-form-urlencoded;charset=utf-8" }
            };

            string freq = PackageRpc(text, langSrc, langTgt);

            using (var client = new HttpClient())
            {
                var content = new StringContent(freq, Encoding.UTF8, "application/x-www-form-urlencoded");

                try
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    var response = await client.PostAsync(url, content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseBody))
                    {
                        foreach (var line in responseBody.Split('\n'))
                        {
                            if (line.Contains("MkEWBc"))
                            {
                                try
                                {
                                    var jsonResponse = JsonSerializer.Deserialize<List<object>>(line);
                                    var responseList = JsonSerializer.Deserialize<List<object>>(jsonResponse[0].ToString());
                                    var responseContent = JsonSerializer.Deserialize<List<object>>(responseList[2].ToString());
                                    var sentences = responseContent[1] as List<object>;

                                    if (sentences.Count == 1)
                                    {
                                        var sentence = sentences[0] as List<object>;

                                        if (sentence.Count > 5)
                                        {
                                            var sentenceList = sentence[5] as List<object>;
                                            var translateText = string.Join(" ", sentenceList.Select(s => s.ToString().Trim()));

                                            return pronounce ? $"{translateText}, {responseContent[0]}, {responseContent[1]}" : translateText;
                                        }
                                        else
                                        {
                                            var singleSentence = sentence[0].ToString();

                                            return pronounce ? $"{singleSentence}, null, null" : singleSentence;
                                        }
                                    }
                                    else if (sentences.Count == 2)
                                    {
                                        var sentenceList = sentences.Select(s => s.ToString()).ToList();

                                        return pronounce ? $"{string.Join(" ", sentenceList)}, {responseContent[0]}, {responseContent[1]}" : string.Join(" ", sentenceList);
                                    }
                                }
                                catch (Exception)
                                {
                                    return "Error: Processing translation response";
                                }
                            }
                        }
                    }
                    else
                        return "Error: No translation result";
                }
                catch (HttpRequestException)
                {
                    return "Error: Request failed";
                }
                catch (Exception e)
                {
                    return "Error: Request failed";
                }
            }

            return string.Empty;
        }
    }
}

