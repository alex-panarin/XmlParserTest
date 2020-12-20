namespace XmlParserTest
{
    internal class LimitSetNode : ParentNode
    {
        public LimitSetNode(string line)
            : base(line)
        {

        }

        protected override string Template => "cim:OperationalLimitSet";

        //protected override string ToJson()
        //{
        //    return $"\"{Name}\"";
        //}
    }
}