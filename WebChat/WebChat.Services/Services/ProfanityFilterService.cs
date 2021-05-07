using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebChat.Services.Interfaces;

namespace WebChat.Services.Services
{
    public class ProfanityFilterService : IProfanityFilter
    {
        IEnumerable<Regex> ForbiddenWordsRegex { get; }
        string ForbiddenWordsTextFile { get; }
        public IConfiguration Configuration { get; }
        public ILogger<ProfanityFilterService> Logger { get; }

        public ProfanityFilterService(IConfiguration configuration, ILogger<ProfanityFilterService> logger)
        {
            Configuration = configuration;
            Logger = logger;

            List<Regex> forbiddenWordsRegex = new List<Regex>();

            ForbiddenWordsTextFile = Path.GetFullPath(Configuration["ForbiddenWordsTextFile"]);

            try
            {
                int loadedWords = 0;

                using (StreamReader reader = File.OpenText(ForbiddenWordsTextFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string forbiddenRegex = reader.ReadLine();

                        if (!string.IsNullOrWhiteSpace(forbiddenRegex))
                        {
                            Regex regex = new Regex(forbiddenRegex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                            forbiddenWordsRegex.Add(regex);
                            loadedWords++;
                        }
                    }
                }

                ForbiddenWordsRegex = forbiddenWordsRegex.AsEnumerable();

                Logger.LogInformation("{0} palavras do arquivo de palavras proibidas em {1} foram carregadas com sucesso.", loadedWords, ForbiddenWordsTextFile);
            }
            catch (FileNotFoundException)
            {
                Logger.LogError("Não foi possível carregar o arquivo de palavras proibidas em {0}. O filtro está desativado.", ForbiddenWordsTextFile);
            }
        }
        public string Filter(string text)
        {
            string filteredText = text;

            if (ForbiddenWordsRegex != null)
            {
                foreach (Regex forbiddenWord in ForbiddenWordsRegex)
                {
                    filteredText = forbiddenWord.Replace(filteredText, "******");
                }
            }

            return filteredText;
        }
    }
}
