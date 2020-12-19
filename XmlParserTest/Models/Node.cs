using System;
using System.IO;

namespace XmlParserTest
{
    class Node
    {
        private const int validKeyLength = 36;
        private const string nameTemplate = "cim:IdentifiedObject.name";
        protected Node(string line)
        {
            Key = ParseKey(line);
        }

        public virtual void Parse(StreamReader reader, NodesRepository repository)
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().TrimStart();

                if (line.StartsWith($"<{nameTemplate}>"))
                {
                    Name = ParseValue(line);
                }

                if (!string.IsNullOrEmpty(ParentTemplate)
                    && line.StartsWith($"<{ParentTemplate} "))
                {
                    Parent = ParseKey(line);
                }

                if (line.StartsWith($"</{Template}>"))
                {
                    repository.AddNode(this);
                    return;
                }

            }
        }
        public override string ToString()
        {
            return ToJson();
        }

        public bool IsValid => !string.IsNullOrEmpty(Key);
        public bool HasParent => !string.IsNullOrEmpty(Parent);
        public string Key { get; protected set; }
        public string Name { get; protected set; }
        public string Parent { get; protected set; }

        protected virtual string ParentTemplate => null;
        protected virtual string Template => null;
        protected virtual string ParseKey(string line)
        {
            var index = line.IndexOf('#');

            return line.Substring(index + 2, validKeyLength);
        }
        protected virtual string ParseValue(string line)
        {
            int index = line.IndexOf('>');
            int stopIndex = line.IndexOf('<', index);

            return line.Substring(index + 1, stopIndex - index - 1);
        }
        protected virtual string ToJson()
        {
            throw new NotSupportedException(nameof(ToJson));
        }

    }

}
