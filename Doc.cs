using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalQ3
{
    public class Doc
    {
        public List<Term> termList;
        public int DocId;
        public string DocName;
        public Doc() {
            termList = new List<Term>();
        }

    }

    public class Term{
        public int TermId;
        public int count = 0;
        public Term(int tid, int cnt) {
            this.TermId = tid;
            this.count = cnt;
        }
    }
}
