using System.Collections.Generic;
using System.Text;

namespace XmlParserTest
{
    class ParentNode : Node
    {
        public ParentNode(string line)
            : base(line)
        {
            Chldren = new List<Node>();
        }

        public List<Node> Chldren { get; set; }

        protected override string ToJson()
        {
            StringBuilder sb = new StringBuilder();

            if (Parent == null)
            {
                sb.Append("{")
                  .Append($"\"{Name}\":");

                return sb
                        .Append("{")
                        .AppendJoin(',', Chldren)
                        .Append("}")
                        .Append("}")
                        .ToString();
            }

            return sb
                    .Append($"\"{Name}\":")
                    .Append("[")
                    .AppendJoin(',', Chldren)
                    .Append("]")
                    .ToString();
        }

    }
}
