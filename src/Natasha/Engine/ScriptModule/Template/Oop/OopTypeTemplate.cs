namespace Natasha.Template
{
    public class OopTypeTemplate<T>: OopModifierTemplate<T>
    {
        public OopType OopTypeEnum;
        public string OopTypeScript;
        public OopTypeTemplate()
        {

            OopTypeScript = "class ";
            OopTypeEnum = OopType.Class;

        }




        public T ChangeToClass()
        {

            OopTypeScript = "class ";
            OopTypeEnum = OopType.Class;
            return Link;

        }




        public T ChangeToStruct()
        {

            OopTypeScript = "struct ";
            OopTypeEnum = OopType.Struct;
            return Link;

        }



        public T ChangeToInterface()
        {

            OopTypeScript = "interface ";
            OopTypeEnum = OopType.Interface;
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(OopTypeScript);
            return Link;

        }

    }

}

namespace Natasha
{
    public enum OopType
    {

        Class,
        Struct,
        Interface

    }
}
