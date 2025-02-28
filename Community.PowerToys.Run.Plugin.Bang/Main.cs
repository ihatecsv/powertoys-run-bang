using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Bang
{
    public class Main : IPlugin, IPluginI18n
    {
        private PluginInitContext _context;

        public string Name => "Bang (Unduck)";
        public string Description => "Type \"?? <query>\" => opens https://unduck.link?q=!<query>";

        public void Init(PluginInitContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Result> Query(Query query)
        {
            string userInput = query?.Search?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(userInput))
            {
                return new List<Result>();
            }

            string transformed = "!" + userInput.Replace(" ", "+");

            string url = "https://unduck.link?q=" + Uri.EscapeDataString(transformed);

            return new List<Result>
            {
                new Result
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
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            };
        }

        public Control CreateSettingPanel() => throw new NotImplementedException();

        public string GetTranslatedPluginTitle() => Name;
        public string GetTranslatedPluginDescription() => Description;
    }
}
