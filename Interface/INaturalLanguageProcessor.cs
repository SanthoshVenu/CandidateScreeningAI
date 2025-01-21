namespace CandidateScreeningAI.Interface
{
    public interface INaturalLanguageProcessor
    {   
        Dictionary<string, object> AnalyzeResponse(string responseText);
    }
}
