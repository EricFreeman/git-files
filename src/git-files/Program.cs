using System;
using System.Collections.Generic;
using System.IO;
using git_files.Parsers;
using LibGit2Sharp;

namespace git_fukd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Working....");

            var directory = Directory.GetCurrentDirectory();
            directory = "C:\\home\\projects\\test-repo-for-git-files";
            using (var repo = new Repository(directory))
            {
                var argumentParser = new ArgumentParser();
                var arguments = argumentParser.ParseArguments(args, repo.Tags);

                var commitParse = new CommitParser();
                var files = commitParse.FindFilesOfTypeChangedBetweenTags(arguments, repo);

                Console.WriteLine();

                foreach (var change in files)
                {
                    Console.WriteLine(change);
                }
            }
        }
    }
}