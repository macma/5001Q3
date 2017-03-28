using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalQ3
{
    class MinHash
    {
        //4613,737----,85576
        int rowCount = 4613;
        int numHashes = 50;
        int primeNum = 4621;
        //int rowCount = 7;
        //int numHashes = 1;
        //int primeNum = 7;
        int colCount;
        //int rowCount = 5;
        //int numHashes = 50;
        //int primeNum = 5;
        List<Doc> DocList;

        public MinHash(List<Doc> dl, int hashCount)
        {
            this.DocList = dl;
            this.colCount = this.DocList.Count;
            this.numHashes = hashCount;
        }

        public int[][] GenerateMinHash(out string strHash)
        {
            StringBuilder sb = new StringBuilder();
            int[][] hMatrix = new int[numHashes][];
            int[] coeffA = GenerateSingatures();
            int[] coeffB = GenerateSingatures();
            StringBuilder sba = new StringBuilder();
            StringBuilder sbb = new StringBuilder();
            sba.AppendLine("HashA:");
            sbb.AppendLine("HashB:");
            for (int i = 0; i < coeffA.Length; i++)
            {
                sba.Append(coeffA[i].ToString() + ",");
                sbb.Append(coeffB[i].ToString() + ",");
            }
            Console.WriteLine(sba.ToString());
            Console.WriteLine(sbb.ToString());
            sb.AppendLine(sba.ToString());
            sb.AppendLine();
            sb.AppendLine(sbb.ToString());
            strHash = sb.ToString();
            for (int h = 0; h < numHashes; h++)
            {
                hMatrix[h] = new int[colCount];
                for (int i = 0; i < colCount; i++)
                {
                    hMatrix[h][i] = 10000;
                }
                int[] hash = new int[rowCount];
                //hash[i] = (coeffA[h] * h + coeffB[h]) % primeNum;
                for (int i = 0; i < rowCount; i++)
                {
                    hash[i] = (coeffA[h] * i + coeffB[h]) % primeNum;
                }

                //hash[0] = 1;
                //hash[1] = 3;
                //hash[2] = 7;
                //hash[3] = 6;
                //hash[4] = 2;
                //hash[5] = 5;
                //hash[6] = 4;
                //hash[0] = 3;
                //hash[1] = 4;
                //hash[2] = 7;
                //hash[3] = 6;
                //hash[4] = 1;
                //hash[5] = 2;
                //hash[6] = 5;

                //hash[0] = 4;
                //hash[1] = 2;
                //hash[2] = 1;
                //hash[3] = 3;
                //hash[4] = 6;
                //hash[5] = 7;
                //hash[6] = 5;

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        if(DocList[j].termList[i].count > 0)
                        //var li = (from a in DocList[j].termList where a.TermId == i + 1 select a).ToList();
                        //if (li.Count > 0)
                            hMatrix[h][j] = Math.Min(hash[i], hMatrix[h][j]);
                    }
                }

            }
            return hMatrix;
        }

        int[] GenerateSingatures() {
            int[] result = new int[numHashes];
            for (int i = 0; i < numHashes; i++)
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                result[i] = rand.Next(1, rowCount);
            }
            return result;
        }
    }
}
