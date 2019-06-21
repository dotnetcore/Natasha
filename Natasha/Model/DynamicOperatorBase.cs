using System;

namespace Natasha
{
    public abstract class DynamicOperatorBase
    {


        internal string _current_name;
        public DynamicOperatorBase this[string key]
        {
            get
            {
                _current_name = key;
                return this;
            }
        }


        public bool _bool;
        public bool BoolValue
        {
            get
            {
                Get(_current_name);
                return _bool;
            }
            set
            {
                _bool = value;
                Set(_current_name);
            }
        }


        public byte _byte;
        public byte ByteValue
        {
            get
            {
                Get(_current_name);
                return _byte;
            }
            set
            {
                _byte = value;
                Set(_current_name);
            }
        }


        public sbyte _sbyte;
        public sbyte SByteValue
        {
            get
            {
                Get(_current_name);
                return _sbyte;
            }
            set
            {
                _sbyte = value;
                Set(_current_name);
            }
        }


        public short _short;
        public short ShortValue
        {
            get
            {
                Get(_current_name);
                return _short;
            }
            set
            {
                _short = value;
                Set(_current_name);
            }
        }


        public ushort _ushort;
        public ushort UShortValue
        {
            get
            {
                Get(_current_name);
                return _ushort;
            }
            set
            {
                _ushort = value;
                Set(_current_name);
            }
        }


        public int _int;
        public int IntValue
        {
            get
            {
                Get(_current_name);
                return _int;
            }
            set
            {
                _int = value;
                Set(_current_name);
            }
        }


        public uint _uint;
        public uint UIntValue
        {
            get
            {
                Get(_current_name);
                return _uint;
            }
            set
            {
                _uint = value;
                Set(_current_name);
            }
        }


        public long _long;
        public long LongValue
        {
            get
            {
                Get(_current_name);
                return _long;
            }
            set
            {
                _long = value;
                Set(_current_name);
            }
        }


        public ulong _ulong;
        public ulong ULongValue
        {
            get
            {
                Get(_current_name);
                return _ulong;
            }
            set
            {
                _ulong = value;
                Set(_current_name);
            }
        }


        public float _float;
        public float FloatValue
        {
            get
            {
                Get(_current_name);
                return _float;
            }
            set
            {
                _float = value;
                Set(_current_name);
            }
        }


        public double _double;
        public double DoubleValue
        {
            get
            {
                Get(_current_name);
                return _double;
            }
            set
            {
                _double = value;
                Set(_current_name);
            }
        }


        public decimal _decimal;
        public decimal DecimalValue
        {
            get
            {
                Get(_current_name);
                return _decimal;
            }
            set
            {
                _decimal = value;
                Set(_current_name);
            }
        }


        public string _string;
        public string StringValue
        {
            get
            {
                Get(_current_name);
                return _string;
            }
            set
            {
                _string = value;
                Set(_current_name);
            }
        }


        public DateTime _DateTime;
        public DateTime DateTimeValue
        {
            get
            {
                Get(_current_name);
                return _DateTime;
            }
            set
            {
                _DateTime = value;
                Set(_current_name);
            }
        }

        public char _char;
        public char CharValue
        {
            get
            {
                Get(_current_name);
                return _char;
            }
            set
            {
                _char = value;
                Set(_current_name);
            }
        }


        public Delegate _delegate;
        public Delegate DelegateValue
        {
            get
            {
                Get(_current_name);
                return _delegate;
            }
            set
            {
                _delegate = value;
                Set(_current_name);
            }
        }


        private DynamicOperatorBase _operator;
        public DynamicOperatorBase OperatorValue
        {
            get
            {
                Get(_current_name);
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }


        public virtual void Set(string name)
        {
        }
        public virtual void Get(string name)
        {
        }
        public virtual void Set<T>(T value)
        {

        }
        public virtual T Get<T>()
        {
            return default(T);
        }
    }
}
