using System.Collections.Generic;

namespace TLab1.Parser
{
    public sealed class SearchResult
    {
        public SearchResult()
        {
            Matches = new List<SearchMatch>();
        }

        public List<SearchMatch> Matches { get; private set; }

        public int MatchCount
        {
            get { return Matches.Count; }
        }
    }
}