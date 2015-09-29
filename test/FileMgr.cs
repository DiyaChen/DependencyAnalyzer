///////////////////////////////////////////////////////////////////////
// FileMgr.cs -  managing files rules specific to an application     //
// ver 2.2                                                          //
// Language:    C#, 2014, .Net Framework 4.0                         //
// Platform:    Dell Precision T7400, Win7, SP1                      //
// Application: Demonstration for CSE681, Project #2, Fall 2014      //
// Author:      Diya Chen,Syracuse University,dchen04@syr.edu        //
///////////////////////////////////////////////////////////////////////
/*
 * Maintenance History
 * ===================
 * ver 2.2 : 05 Oct 2014
 * -modified class FileMgr, added a control statement to search subdirectories 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeAnalysis
{
    public class FileMgr
    {
        private List<string> files = new List<string>();
        private List<string> patterns = new List<string>();
        //private bool recurse = true;

        public void findFiles(string path, bool sub_flag)
        {
            if (patterns.Count == 0)
                addPattern("*.*");
            foreach(string pattern in patterns)
            {
                string[] newFiles = Directory.GetFiles(path, pattern);
                for (int i = 0; i < newFiles.Length; ++i)
                    newFiles[i] = Path.GetFullPath(newFiles[i]);
                files.AddRange(newFiles);
            }
            if(sub_flag)                 //if command line argument contains /S, search all files including subdirectories
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                    findFiles(dir,sub_flag);
            }
        }

        public void addPattern(string pattern)
        {
            patterns.Add(pattern);
        }

        public List<string> getFiles()
        {
            return files;
        }

#if(TEST_FILEMGR)
        static void Main(string[] args)
        {
            Console.Write("\n  Testing FileMgr Class");
            Console.Write("\n =======================\n");
            FileMgr fm = new FileMgr();
            fm.addPattern("*.cs");
            fm.findFiles("../../");
            List<string> files = fm.getFiles();
            foreach (string file in files)
            Console.Write("\n  {0}", file);
            Console.Write("\n\n");
            
        }
#endif
    }
}
