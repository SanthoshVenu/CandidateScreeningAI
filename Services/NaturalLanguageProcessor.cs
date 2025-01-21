using Python.Runtime;
using CandidateScreeningAI.Interface;

namespace CandidateScreeningAI.Services
{
    public class NaturalLanguageProcessor : INaturalLanguageProcessor
    {
        public Dictionary<string, object> AnalyzeResponse(string responseText)
        {
            string pythonFilePath = @"C:\MyProject\Scripts";
            PythonEngine.Initialize();
            using (Py.GIL())
            {
                dynamic sys = Py.Import("sys");
                sys.path.append(pythonFilePath);

                dynamic nlpModule = Py.Import("response_analysis");
                dynamic result = nlpModule.analyze_response(responseText);

                var dictionary = new Dictionary<string, object>();
                foreach (var item in result.items())
                {
                    dictionary.Add(item[0].ToString(), item[1].ToString());
                }
                return dictionary;
            }
        }
    }
}
