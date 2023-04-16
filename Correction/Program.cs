using System;
using System.Collections.Generic;
using System.IO;
using DeepMorphy;
using System.Linq;

namespace Correction
{
    class Program
    {
        static void Main(string[] args)
        {

            StreamReader sr = new("chist_nouns.txt");

            List<string> nouns = new();

            string stroke;


            while (!sr.EndOfStream)
            {
                stroke = sr.ReadLine().Split("	")[1];
                nouns.Add(stroke);
            }

            sr.Close();

            MorphAnalyzer morph = new();


            sr = new StreamReader("Tezaurus.txt");
            Queue<List<string>> words = new();
            string dop;

            while (!sr.EndOfStream)
            {
                List<string> assass = new();
                foreach (var word in sr.ReadLine().Split(' '))
                {
                    if (word.Length > 0)
                    {
                        dop = word;
                        while ((dop[^1] == ';') | (dop[^1] == ',') | (dop[^1] == '»') | (dop[^1] == '!') | (dop[^1] == ':') | (dop[^1] == '?'))
                            dop = dop.Remove(dop.IndexOf(dop[^1]));
                        if ((dop.Length > 0) && (dop[0] == '«'))
                            dop = dop.Remove(dop.IndexOf(dop[0]));
                        if (dop.Length > 0)
                            assass.Add(dop);
                    }
                }
                words.Enqueue(assass);

            }

            sr.Close();
            StreamWriter sw = new("Assass.txt", false);

            List<DeepMorphy.Model.MorphInfo> info;
            string[] posts = new string[8] { "цифра", "част", "мест", "предл", "союз", "пункт", "неизв", "числ" };

            foreach (var ass in words)
            {
                info = morph.Parse(ass).ToList();
                foreach (var word in info)
                {
                    if (word["чр"].BestGramKey == "сущ")
                    {
                        if (nouns.Contains(word.Text))
                        {
                            Console.Write(word.Text + ';');
                            sw.Write(word.Text + ';');
                        }
                    }
                    else if (!posts.Contains(word["чр"].BestGramKey))
                    {
                        Console.Write(word.Text + ';');
                        sw.Write(word.Text + ';');
                    }

                }
                Console.WriteLine('\n');
                sw.WriteLine();
            }

            sw.Close();
        }
    }
}
