using System;
using System.Collections.Generic;

namespace CSharpSnippets.CSharpLanguage.Reflection
{
    class SillyModelStore
    {
        private static readonly List<SillyModel> _sillyModels = new List<SillyModel>
        {
            new SillyModel { id = 1, name = "bert", isGood = false, numCars = null, dateGraduated = null, dateCreated = DateTime.Now },
            new SillyModel { id = 2, name = "ernie", isGood = true, numCars = 5, dateGraduated = null, dateCreated = DateTime.Now }
        };

        public static IEnumerable<Dictionary<string, string>> GetSillyModelsAsDicts()
        {
            foreach (var model in _sillyModels)
            {
                yield return new Dictionary<string, string>
                {
                    { "id", model.id.ToString() },
                    { "name", model.name },
                    { "isGood", model.isGood.ToString() },
                    { "numCars", model.numCars?.ToString() },
                    { "dateGraduated", model.dateGraduated?.ToString() },
                    { "dateCreated", model.dateCreated.ToString() }
                };
            }
        }
    }
}
