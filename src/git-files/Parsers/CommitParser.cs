using System.Collections.Generic;
using System.Linq;
using git_files.Domain;
using git_fukd;
using LibGit2Sharp;

namespace git_files.Parsers
{
    public class CommitParser
    {
        public List<string> FindFilesOfTypeChangedBetweenTags(Arguments arguments, Repository repo)
        {
            var spinner = new ConsoleSpiner();

            var commits = repo.Commits; 
            Commit lastCommit = null;

            var isLooking = false;
            var isLast = false;
            var files = new List<string>();

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

            return files;
        }
    }
}