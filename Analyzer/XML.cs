///////////////////////////////////////////////////////////////////////
// XML.cs -     Create XML files for application                     //
// ver 2.1                                                           //
// Language:    C#, 2014, .Net Framework 4.0                         //
// Platform:    Dell Precision T7400, Win7, SP1                      //
// Application: Demonstration for CSE681, Project #2, Fall 2014      //
// Author:      Diya Chen,Syracuse University,dchen04@syr.edu        //
///////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * XML package defines one class and one function to build XML files.
 * class XML
 * public function XmlBuilder
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CodeAnalysis
{
    class XML
    {
        public void XmlBuilder(List<Elem> table, string FileName)
        {
            Console.Write("\n Create XML file using XDocument-----------------------------------\n");
            XDocument xml = new XDocument();

            foreach (Elem e in table)
            {
                if (e.type == "namespace")
                {
                    XElement ns = new XElement("namespace" + e.name + e.begin + e.end);            //if detected namespace, new a node to store it 
                    xml.Add(ns);
                    foreach (Elem el in table)
                    {
                        if (el.type == "class" && e.begin < el.begin && e.end >= el.end)  //if detected a class within a namespace, new a node for class
                        {
                            XElement cl = new XElement("class" + el.name + el.begin + el.end);
                            ns.Add(cl);                                           //add the class node under namespace node 
                            foreach (Elem ele in table)
                            {
                                if (ele.type == "function" && el.begin < ele.begin && el.end > ele.end)
                                {
                                    XElement func = new XElement("function" + ele.name + ele.begin + ele.end);
                                    cl.Add(func);//if detected a function with a class, add this node under class node 
                                }
                            }
                        }
                    }
                }

            }
            Console.Write(xml.ToString());
            Console.Write("\n");
            int index = FileName.LastIndexOf("\\");
            int index1 = FileName.LastIndexOf(".");
            xml.Save(FileName.Substring((index + 1), index1 - index - 1) + ".xml");
        }

#if (TEST_XML)
    static void Main(string[] args)
    {
        Console.Write("\n  Testing XML - XmlBuilder ");
        Console.Write("\n ============================\n");
        List<Elem> test = new List<Elem>();

        Elem ns = new Elem();
        ns.name = "testNameSpace";
        ns.type = "namespace";
        ns.begin = 10;
        ns.end = 100;

        Elem cl = new Elem();
        cl.name = "testClass";
        cl.type = "class";
        cl.begin = 20;
        cl.end = 80;

        Elem func = new Elem();
        func.name = "testFunction";
        func.type = "function";
        func.begin = 30;
        func.end = 60;

        test.Add(ns);
        test.Add(cl);
        test.Add(func);

        XML xb = new XML();
        xb.XmlBuilder(test);

    }
#endif
    }

}