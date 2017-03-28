using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalQ3
{
    public partial class Form1 : Form
    {
        string[] docNames;
        int docNum = 737;
        int termCount = 4613;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            docNames = System.IO.File.ReadAllLines(@"bbcsport.docs");
        }

        private List<Doc> readDocList1()
        {
            string[] lines = System.IO.File.ReadAllLines(@"bbcsport.mtx");
            List<Doc> docList = new List<Doc>();
            for (int i = 0; i < docNum; i++)
            {
                Doc doc = new Doc();
                doc.DocId = i + 1;
                doc.DocName = docNames[doc.DocId - 1];
                
                for (int j = 0; j < termCount; j++)
                {
                    Term term = new Term(j + 1, 0);
                    doc.termList.Add(term);
                }
                docList.Add(doc);
            }
            for (int i = 2; i < lines.Length; i++)
            {
                string strLine = lines[i];

                if (Convert.ToInt32(strLine.Split(' ')[1]) == 414)
                {
                }
                if (strLine.Split(' ').Length == 3)
                {
                    int docId = Convert.ToInt32(strLine.Split(' ')[1]);
                    int termId = Convert.ToInt32(strLine.Split(' ')[0]);
                    int cnt = Convert.ToInt32(Convert.ToDouble(strLine.Split(' ')[2]));
                    docList[docId - 1].termList[termId - 1].count = cnt;
                }
            }
            return docList;
        }

        private List<Doc> readDocList()
        {
            string[] lines = System.IO.File.ReadAllLines(@"bbcsport.mtx");
            List<Doc> docList = new List<Doc>();
            for (int i = 0; i < docNum; i++)
            {
                Doc doc = new Doc();
                doc.DocId = i + 1;
                doc.DocName = docNames[doc.DocId - 1];
                docList.Add(doc);
            }
            for (int i = 2; i < lines.Length; i++)
            {
                string strLine = lines[i];

                if (Convert.ToInt32(strLine.Split(' ')[1]) == 414)
                {
                }
                if (strLine.Split(' ').Length == 3)
                {
                    var li = (from a in docList where a.DocId == Convert.ToInt32(strLine.Split(' ')[1]) select a).ToList();
                    //if (li.Count == 0)
                    //{
                    //    Doc doc = new Doc();
                    //    doc.DocId = Convert.ToInt32(strLine.Split(' ')[1]);
                    //    doc.DocName = docNames[doc.DocId - 1];
                    //    Term term = new Term(Convert.ToInt32(strLine.Split(' ')[0]), Convert.ToInt32(Convert.ToDouble(strLine.Split(' ')[2])));
                    //    doc.termList.Add(term);
                    //    docList.Add(doc);
                    //}
                    //else
                    {
                        Term term = new Term(Convert.ToInt32(strLine.Split(' ')[0]), Convert.ToInt32(Convert.ToDouble(strLine.Split(' ')[2])));
                        li[0].termList.Add(term);
                    }
                }
            }
            return docList;
        
        } 
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("starttime: " + DateTime.Now.ToString("HH:mm:ss"));
            List<Doc> docList = readDocList1();
            int count = 0;

            //double dou = CalculateJaccardSimilarity(docList[0], docList[1]);
            for (int i = 0; i < docList.Count; i++)
            {
                for (int j = i + 1; j < docList.Count; j++)
                {
                    if (docList[i].DocName == "tennis.012" && docList[j].DocName == "tennis.018")
                    { 
                    }
                    double dou = CalculateJaccardSimilarity(docList[i], docList[j]);
                    if (dou >= 0.5) {
                        count++;
                        //Console.WriteLine(string.Format("{0} and {1}, similarity: {2}", docList[i].DocName, docList[j].DocName, dou));
                        sb.AppendLine(string.Format("{0}:{1}:{2};", docList[i].DocId, docList[j].DocId, dou));
                        textBox1.Text = sb.ToString();
                        Console.WriteLine(string.Format("{0}:{1}:{2};", docList[i].DocId, docList[j].DocId, dou));
                    }
                }
            }
            sb.AppendLine("endtime: " + DateTime.Now.ToString("HH:mm:ss"));
            textBox1.Text = sb.ToString();
        }

        double CalculateJaccardSimilarity(Doc doc1, Doc doc2) {
            int totalCount = 0;
            int similarCount = 0;
            for (int i = 0; i < termCount; i++)
            {
                if (doc1.termList[i].count > 0 || doc2.termList[i].count > 0) {
                    totalCount++;
                    if (doc1.termList[i].count > 0 && doc2.termList[i].count > 0) {
                        similarCount++;
                    }
                }
            }
            double similarity = (double)similarCount / totalCount;
            return similarity;
        }

        double CalculateSimpleJaccardSimilarity(Doc doc1, Doc doc2)
        {
            int totalCount = 0;
            int similarCount = 0;
            if (doc1.DocId == 414 && doc2.DocId == 644) { 
            
            }
            for (int i = 0; i < doc1.termList.Count; i++)
            {
                var li = (from a in doc2.termList where a.TermId == doc1.termList[i].TermId select a).ToList();
                if (li.Count > 0)
                {
                    similarCount += 1;
                    totalCount += 1;
                }
                else { totalCount += 1; }
            }
            for (int i = 0; i < doc2.termList.Count; i++)
            {
                var li = (from a in doc1.termList where a.TermId == doc2.termList[i].TermId select a).ToList();
                if (li.Count == 0)
                    totalCount += 1;
            }
            double similarity = (double)similarCount / totalCount;
            return similarity;
        }


        double CalculateMinHashSimilarity(int[][] hMatrix, int col1, int col2)
        {
            int hashCount = hMatrix.Length;//should be 50
            int docCount = hMatrix[0].Length;//should be 737


            int simCount = 0;
            for (int i = 0; i < hashCount; i++)
            {
                if (hMatrix[i][col1] == hMatrix[i][col2])
                    simCount++;
            }
            double similarity = (double)simCount / hashCount;
            return similarity;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //List<Doc> docList = new List<Doc>();// readDocList();
            //MinHash mh = new MinHash(docList);
            //int[][] hMatrix = mh.GenerateMinHash();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("starttime: " + DateTime.Now.ToString("HH:mm:ss"));
            List<Doc> docList = readDocList1();

            #region test
            
            //List<Doc> docList = new List<Doc>();
            //Doc d = new Doc();
            //d.DocId = 1;
            //List<Term> lt = new List<Term>();
            //lt.Add(new Term(1, 1));
            //lt.Add(new Term(2, 1));
            //lt.Add(new Term(3, 0));
            //lt.Add(new Term(4, 0));
            //lt.Add(new Term(5, 0));
            //lt.Add(new Term(6, 1));
            //lt.Add(new Term(7, 1));
            //d.termList = lt;
            //docList.Add(d);

            //d = new Doc();
            //d.DocId = 2;
            //lt = new List<Term>();
            //lt.Add(new Term(3, 0));
            //lt.Add(new Term(3, 0));
            //lt.Add(new Term(3, 1));
            //lt.Add(new Term(4, 1));
            //lt.Add(new Term(5, 1));
            //lt.Add(new Term(6, 0));
            //lt.Add(new Term(7, 0));
            //d.termList = lt;
            //docList.Add(d);

            //d = new Doc();
            //d.DocId = 3;
            //lt = new List<Term>();
            //lt.Add(new Term(1, 1));
            //lt.Add(new Term(2, 0));
            //lt.Add(new Term(3, 0));
            //lt.Add(new Term(4, 0));
            //lt.Add(new Term(5, 0));
            //lt.Add(new Term(6, 1));
            //lt.Add(new Term(7, 1));
            //d.termList = lt;
            //docList.Add(d);

            //d = new Doc();
            //d.DocId = 4;
            //lt = new List<Term>();
            //lt.Add(new Term(1, 0));
            //lt.Add(new Term(2, 1));
            //lt.Add(new Term(3, 1));
            //lt.Add(new Term(4, 1));
            //lt.Add(new Term(5, 1));
            //lt.Add(new Term(6, 0));
            //lt.Add(new Term(7, 0));
            //d.termList = lt;
            //docList.Add(d);

            
            #endregion
            int minHashCount = Convert.ToInt32(comboBox1.SelectedItem.ToString());
            MinHash mh = new MinHash(docList, minHashCount);
            string strHash = string.Empty;
            int[][] hMatrix = mh.GenerateMinHash(out strHash);

            int count = 0;
            //double dou = CalculateJaccardSimilarity(docList[0], docList[1]);
            sb.AppendLine(strHash);
            for (int i = 0; i < docList.Count; i++)
            {
                for (int j = i + 1; j < docList.Count; j++)
                {
                    double dou = CalculateMinHashSimilarity(hMatrix, i, j);
                    if (dou >= 0.5)
                    {
                        count++;
                        //Console.WriteLine(string.Format("{0} and {1}, similarity: {2}", docList[i].DocName, docList[j].DocName, dou));
                        sb.AppendLine(string.Format("{0}:{1}:{2};", docList[i].DocId, docList[j].DocId, dou));
                        //Console.WriteLine(string.Format("{0}:{1}:{2};", docList[i].DocId, docList[j].DocId, dou));
                    }
                }
            }
            sb.AppendLine("endtime: " + DateTime.Now.ToString("HH:mm:ss"));
            textBox1.Text = sb.ToString();
        }
    }
}

