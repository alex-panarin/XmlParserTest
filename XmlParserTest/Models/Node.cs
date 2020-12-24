using System;
using System.IO;
using System.Threading.Tasks;

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

        public async Task Parse(StreamReader reader, NodesRepository repository)
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                line = line.TrimStart();

                if (line.StartsWith($"<{nameTemplate}>"))
                {
                    Name = ParseValue(line);
                    continue;
                }

                if (!string.IsNullOrEmpty(ParentTemplate)
                    && line.StartsWith($"<{ParentTemplate} "))
                {
                    Parent = ParseKey(line);
                    continue;
                }

                ParseInternal(line);

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

        protected virtual void ParseInternal(string line)
        {

        }
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
