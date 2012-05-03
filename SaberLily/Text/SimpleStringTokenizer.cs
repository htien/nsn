using System.Collections;

namespace SaberLily.Text
{
    public class SimpleStringTokenizer : IEnumerable, IEnumerator
    {
        private string[] tokensizedStrs;
        private int index = -1;

        public SimpleStringTokenizer(string target, char[] tokens)
        {
            tokensizedStrs = target.Split(tokens);
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public int CountTokens()
        {
            return tokensizedStrs.Length;
        }

        object IEnumerator.Current
        {
            get { return tokensizedStrs[index]; }
        }

        bool IEnumerator.MoveNext()
        {
            return !(++index >= tokensizedStrs.Length);
        }

        void IEnumerator.Reset()
        {
            index = -1;
        }
    }
}
