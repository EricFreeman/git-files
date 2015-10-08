using System.Linq;
using git_files.Domain;
using LibGit2Sharp;

namespace git_files.Parsers
{
    public class ArgumentParser
    {
        public Arguments ParseArguments(string[] args, TagCollection tags)
        {
            if (args.Length == 2)
            {
                return Generate(tags, args[0], args[1]);
            }
            else if (args.Length == 3)
            {
                return Generate(tags, args[0], args[1], args[2]);
            }
            else if (args.Length == 4)
            {
                return Generate(tags, args[0], args[1], args[2], args[3]);
            }

            return Generate(tags);
        }

        private Arguments Generate(TagCollection tags, string startTag = "", string endTag = "", string fileExtension = ".sql", string fileName = "updated-sql.csv")
        {
            var start = tags.FirstOrDefault(x => x.Name == startTag);
            var end = tags.FirstOrDefault(x => x.Name == endTag);

            return new Arguments
            {
                StartTag = start != null ? start.Target.Sha : tags.Last().Target.Sha,
                EndTag = end != null ? end.Target.Sha : tags.Reverse().Skip(1).First().Target.Sha,
                FileExtension = fileExtension,
                ExportFileName = fileName
            };
        }
    }
}
