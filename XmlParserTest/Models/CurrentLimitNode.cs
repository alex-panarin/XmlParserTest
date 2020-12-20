using System;
using System.Collections.Generic;
using System.Text;

namespace XmlParserTest
{
    class CurrentLimitNode : Node
    {
        public CurrentLimitNode(string line)
            : base(line)
        {

        }

        protected override string ParentTemplate => "me:IdentifiedObject.ParentObject";
        protected override string Template => "cim:CurrentLimit";
        public string Value { get; private set; }

        protected override string ToJson()
        {
            return $"\"{Name}\":\"{Value}\"";
        }

        protected override void ParseInternal(string line)
        {
            if(line.StartsWith("<cim:CurrentLimit.value>"))
            {
                Value = ParseValue(line);
;           }
        }
    }
}
