namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        public LINK MakerUsing()
        {
            _script.Append(_using);
            if (_namespace != null)
            {
                _script.Append($"namespace {_namespace}{{");
            }
            return _link;
        }

        public LINK MakerClass()
        {
            _script.Append($"{_level}{_modifier}class {_class_name}{_inheritance}{{");            
            return _link;
        }

        public LINK MakerContent()
        {
            _script.Append($"{_fields}{_text}}}");
            if (_namespace != null)
            {
                _script.Append("}");
            }
            return _link;
        }

        public string Builder()
        {
            MakerUsing();
            MakerClass();
            MakerContent();
            return _script.ToString();
        }
    }
}
