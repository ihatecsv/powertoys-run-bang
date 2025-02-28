using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using Wox.Plugin;  // Provided by PowerToys (via Wox infrastructure)
using ManagedCommon; // For Logging, if needed

namespace Community.PowerToys.Run.Plugin.Bang
{
    /// <summary>
    /// A minimal plugin that takes queries prefixed by "?" and opens:
    /// https://unduck.link?q=!<query-with-plus-spaces>
    /// </summary>
    public class Main : IPlugin, IPluginI18n
    {
        private PluginInitContext _context;

        public string Name => "Bang (Unduck)";
        public string Description => "Query '?something' => https://unduck.link?q=!something";

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Result> Query(Query query)
        {
            // The user typed "?some text" => query.Search is "some text".
            string userInput = query?.Search?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(userInput))
            {
                // If there's no input after '?', do nothing.
                return new List<Result>();
            }

            // Prepend '!' and replace spaces with '+' for the final query.
            // For example: "gh react" -> "!gh+react"
            string transformed = "!" + userInput.Replace(" ", "+");

            // URL-encode in case of special characters, then build final link
            string url = "https://unduck.link?q=" + Uri.EscapeDataString(transformed);

            // Return a single result for the user to open.
            var result = new Result
            {
                Title = "Open: " + transformed,
                SubTitle = "Go to " + url,
                IcoPath = "Images\\Search.light.png",
                Action = _ =>
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception e)
                    {
                        // If you'd like, log an error here
                        // Log.Exception("Failed to open URL", e, GetType());
                        return false;
                    }
                    return true;
                }
            };

            return new List<Result> { result };
        }

        // No settings panel used
        public Control CreateSettingPanel() => throw new NotImplementedException();

        // Plugin translations:
        public string GetTranslatedPluginTitle() => Name;
        public string GetTranslatedPluginDescription() => Description;
    }
}
