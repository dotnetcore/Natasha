namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        public LINK MakerHeader()
        {
            _script.Append(_using);
            if (_namespace != null)
            {
                _script.Append($"namespace {_namespace}{{");
            }
            _script.Append($"public class {_class_name} {_inheritance}");
            return _link;
        }

        public LINK MakerStaticHeader()
        {
            _script.Append(_using);
            if (_namespace != null)
            {
                _script.Append($"namespace {_namespace}{{");
            }
            _script.Append($"public static class {_class_name}");
            return _link;
        }

        public LINK MakerContent(string content)
        {
            _script.Append($"{{{content}}}");
            if (_namespace != null)
            {
                _script.Append("}");
            }
            return _link;
        }

    }
}
