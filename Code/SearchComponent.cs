using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BlazorDemo.Code
{
    public class SearchComponent : ComponentBase
    {
        public string SearchValue { get; set; }

        public string PressedKey { get; set; }

        public string SearchResult { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        public SearchComponent()
        {
        }

        public async Task Search()
        {
            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync($"https://www.google.com/search?q={SearchValue}");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();

                    Match m = Regex.Match(content, $"<div class=\"sd\"{@"\s*(.+?)\s*"}</div>");

                    if (m.Success)
                    {
                        SearchResult = HttpUtility.HtmlDecode(m.Groups[0].Value.Replace("<div class=\"sd\" id=\"resultStats\">", "").Replace("</div>", ""));
                    }
                }
            }
        }

        public async Task OnKeyPressed(UIKeyboardEventArgs eventArgs)
        {
            PressedKey += eventArgs.Key;

            if (eventArgs.Key == "1")
            {
              var someResult = await JSRuntime?.InvokeAsync<string>("DoSomeWarning");
            }

        }
    }
}
