using AdventOfCode2022.SharedKernel;
using System.Dynamic;

namespace Day07
{
    internal class Program
    {
        internal enum FilesystemType
        {
            File,
            Dir
        }

        internal class FilesystemElement
        {
            public FilesystemElement? Parent { get; }
            public string Name { get; }
            public List<FilesystemElement> ChildElements { get; }
            public FilesystemType Type { get; }
            public int Size { get { return calculateSize(); } }

            private int fileSize;

            private int calculateSize()
            {
                if(Type == FilesystemType.File)
                {
                    return fileSize;
                }
                else
                {
                    int size = 0;
                    foreach(FilesystemElement child in ChildElements)
                    {
                        size += child.Size;
                    }
                    return size;
                }
            }

            public FilesystemElement(FilesystemElement? parent, string name, FilesystemType type, int size)
            {
                this.ChildElements = new List<FilesystemElement>();
                this.Parent = parent;
                this.Type = type;
                this.fileSize = size;
                this.Name = name;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 07: No Space Left On Device"));
            Console.WriteLine("Terminal output: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), false);

            FilesystemElement tree = buildFilesystemTree(puzzleInput);

            Console.WriteLine("Total size directories > 100000 size: {0}", GetTotalSize(tree, 100000));
        }

        private static FilesystemElement buildFilesystemTree(PuzzleInput puzzleInput)
        {
            FilesystemElement tree = new FilesystemElement(null, "/", FilesystemType.Dir, 0);
            FilesystemElement current = tree;

            foreach (string line in puzzleInput.Lines)
            {
                if (line.StartsWith("$ cd"))
                {
                    string dirName = line.Replace("$ cd", "").Trim();
                    if (dirName == "..")
                    {
                        current = current.Parent;
                    }
                    else if (dirName != "/")
                    {
                        FilesystemElement? dir = current.ChildElements.FirstOrDefault(d => d.Name == dirName);
                        if (dir == null)
                        {
                            dir = new FilesystemElement(current, dirName, FilesystemType.Dir, 0);
                            current.ChildElements.Add(dir);
                        }
                        current = dir;
                    }

                }
                else
                {
                    string[] elementProps = line.Split(' ');
                    int size = 0;
                    if (Int32.TryParse(elementProps[0], out size))
                    {
                        FilesystemElement file = new FilesystemElement(current, elementProps[1], FilesystemType.File, size);
                        current.ChildElements.Add(file);
                    }
                }
            }

            return tree;
        }

        private static int GetTotalSize(FilesystemElement tree, int maxSize)
        {
            int total = 0;
            foreach (FilesystemElement element in tree.ChildElements)
            {
                int size = element.Size;
                if (element.Type == FilesystemType.Dir)
                {
                    if ( size <= maxSize)
                    {
                        total += size;
                    }
                    total += GetTotalSize(element, maxSize);
                }
            }
            return total;
        }
    }
}