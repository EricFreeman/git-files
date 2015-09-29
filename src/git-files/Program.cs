using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using git_files;
using LibGit2Sharp;

namespace git_fukd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var files = new List<string>();
            var spinner = new ConsoleSpiner();
            Console.Write("Working....");

            var directory = Directory.GetCurrentDirectory();
            directory = "C:\\home\\projects\\fuck";
            using (var repo = new Repository(directory))
            {
                var argumentParser = new ArgumentParser();
                var arguments = argumentParser.ParseArguments(args, repo.Tags);

                var commits = repo.Commits;
                Commit lastCommit = null;

                var isLooking = false;
                var isLast = false;

                foreach (var commit in commits)
                {
                    spinner.Turn();

                    var tree = commit.Tree;

                    if (lastCommit == null)
                    {
                        lastCommit = commit;
                        continue;
                    }

                    var parentCommitTree = lastCommit.Tree;
                    lastCommit = commit;

                    if (commit.Sha == arguments.StartTag || commit.Sha == arguments.EndTag)
                    {
                        isLooking = !isLooking;
                        isLast = !isLooking;
                    }

                    if (isLooking)
                    {
                        var c = repo.Diff
                            .Compare<TreeChanges>(parentCommitTree, tree)
                            .ToList();

                        c.ForEach(commitChanges =>
                        {
                            var file = commitChanges.Path;
                            if (file.EndsWith(".sql") && !files.Contains(file))
                            {
                                files.Add(file);
                            }
                        });
                    }

                    if (isLast)
                    {
                        break;
                    }
                }

                Console.WriteLine();

                foreach (var change in files)
                {
                    Console.WriteLine(change);
                }
            }
        }
    }
}