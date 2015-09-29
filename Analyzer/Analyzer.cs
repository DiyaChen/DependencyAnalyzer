///////////////////////////////////////////////////////////////////////
// Analyzer.cs - Analyzer rules specific to an application           //
// ver 2.1                                                           //
// Language:    C#, 2014, .Net Framework 4.0                         //
// Platform:    Dell Precision T7400, Win7, SP1                      //
// Application: Demonstration for CSE681, Project #2, Fall 2014      //
// Author:      Diya Chen,Syracuse University,dchen04@syr.edu        //
///////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    class Analyzer
    {
        static public string[] getFiles(string path, List<string> patterns, bool sub_flag)
        {
            FileMgr fm = new FileMgr();
            foreach (string pattern in patterns)
                fm.addPattern(pattern);
            fm.findFiles(path,sub_flag);                //control files will be searched 
            return fm.getFiles().ToArray();
        }

        static void doAnalysis(string[] files, bool relation_flag, bool xml_flag)
        {
            List<Elem> TABLE = new List<Elem>();
            foreach (object file in files)
            {
                CSsemi.CSemiExp semi = new CSsemi.CSemiExp();
                semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", file);
                    return;
                }
                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi);
                Parser parser = builder.build();
                try
                {
                    while (semi.getSemi())
                        parser.parse(semi);
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }
                Repository rep = Repository.getInstance();
                TABLE.AddRange(rep.locations);          // Build a TABLE to store all the elements, provided to be searched for relationshipAnalysis class 
                semi.close();
            }

            Console.Write("\n  Demonstrating Parser");
            Console.Write("\n ======================\n");

            // parse all files and store the elements into table for each file
            foreach (object file in files)
            {
                Console.Write("\n Processing file {0}\n", file as string);

                CSsemi.CSemiExp semi = new CSsemi.CSemiExp();
                semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", file);
                    return;
                }

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi);
                Parser parser = builder.build();

                try
                {
                    while (semi.getSemi())
                        parser.parse(semi);
                    Console.Write("\n\n locations table contains:");
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }
                Repository rep = Repository.getInstance();
                List<Elem> table = rep.locations;

                // display basic information for elements including size and complexity 
                Console.Write("\n    type                 name          begin      end       size     complexity");
                foreach (Elem e in table)
                {
                    if (e.type == "function")
                    {
                        ComplexityAnalysis ca = new ComplexityAnalysis();
                        e.complexity = ca.ComplexityAnalyze(e, file);
                    }
                        Console.Write("\n  {0,9}, {1,20}, {2,8}, {3,8}, {4,8}, {5,8}", e.type, e.name, e.begin, e.end, e.end - e.begin + 1, e.complexity);
                    
                }
                //display the relationships between classes 
                if (relation_flag == true)
                {
                    foreach (Elem e in table)
                    {
                        e.AggregationFlag = false;
                        e.CompositionFlag = false;
                        e.UsingFlag = false;
                        if (e.type == "class" || e.type == "struct" || e.type == "interface")
                        {
                            Console.Write("\n\n {0} {1}", e.type, e.name);
                            Console.Write("------------------------------------------------------\n");
                            RelationshipAnalysis ra = new RelationshipAnalysis();
                            ra.RelationshipAnalyzer(e, file, TABLE);
                            foreach (Elem ele in TABLE)
                            {
                                ele.AggregationFlag = false;
                                ele.CompositionFlag = false;
                                ele.UsingFlag = false;
                            }
                        }
                    }
                }
                Console.WriteLine();

                //display xml files for each file 
                if (xml_flag == true)
                {
                    XML x = new XML();
                    x.XmlBuilder(table, (string)file);
                }
            }
            Console.Write("\n");
            DisplaySummary(TABLE);
        }
         static void DisplaySummary(List<Elem> TABLE)
         {
            Console.Write("============Summary================");
            Console.Write("\n    type                 name        size");
            foreach (Elem e in TABLE)
                {
                    Console.Write("\n {0,9}, {1,20},{2,8}", e.type, e.name, e.end - e.begin + 1);
                }
            Console.Write("\n");
         }
         
        static void Main(string[] args)
        {
            //string path = "../../";
            string path = args[0];
            List<string> patterns = new List<string>();   
            bool sub_flag = false;               //set a flag for searching subdirectory 
            bool relation_flag = false;          //set a flag for dispalying relationship of classes 
            bool xml_flag = false;               //set a flag for displaying xml of each file

            for(int i = 1; i<args.Length ; i++)
            {
                if (args[i].Contains("*"))         //set the pattern for command line arguments
                    patterns.Add(args[i]);         
                else if (args[i].Contains("/S"))
                    sub_flag = true;
                else if (args[i].Contains("/R"))
                    relation_flag = true;
                else if (args[i].Contains("/X"))
                    xml_flag = true;
            }
            //patterns.Add("*.cs");
            string[] files = Analyzer.getFiles(path, patterns, sub_flag);
            doAnalysis(files, relation_flag, xml_flag);
        }
    }
}
