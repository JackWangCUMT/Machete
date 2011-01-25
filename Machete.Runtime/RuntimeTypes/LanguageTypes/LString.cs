﻿using System;
using Machete.Runtime.NativeObjects;
using Machete.Core;

namespace Machete.Runtime.RuntimeTypes.LanguageTypes
{
    public sealed class LString : LType, IString
    {
        public string BaseValue { get; private set; }

        public override LanguageTypeCode TypeCode
        {
            get { return LanguageTypeCode.String; }
        }

        public override bool IsPrimitive
        {
            get { return true; }
        }

        public LString(IEnvironment environment, string value)
            : base(environment)
        {
            BaseValue = value;
        }
                
        public override IDynamic Op_Equals(IDynamic other)
        {
            switch (other.TypeCode)
            {
                case LanguageTypeCode.String:
                    return Environment.CreateBoolean(this.BaseValue == ((LString)other).BaseValue);
                case LanguageTypeCode.Number:
                    return this.ConvertToNumber().Op_Equals(other);
                case LanguageTypeCode.Object:
                    return this.ConvertToPrimitive(null).Op_Equals(other);
                default:
                    return Environment.False;
            }
        }

        public override IDynamic Op_StrictEquals(IDynamic other)
        {
            switch (other.TypeCode)
            {
                case LanguageTypeCode.String:
                    return Environment.CreateBoolean(this.BaseValue == ((LString)other).BaseValue);
                default:
                    return Environment.False;
            }
        }        

        public override IDynamic Op_Typeof()
        {
            return Environment.CreateString("string");
        }
                
        public override IDynamic ConvertToPrimitive(string preferredType)
        {
            return this;
        }

        public override IBoolean ConvertToBoolean()
        {
            return Environment.CreateBoolean(BaseValue.Length > 0);
        }

        public override INumber ConvertToNumber()
        {
            return Environment.CreateNumber(Machete.Compiler.StringNumericLiteral.eval(BaseValue));
        }

        public override IString ConvertToString()
        {
            return this;
        }

        public override IObject ConvertToObject()
        {
            return ((IConstructable)Environment.StringConstructor).Construct(Environment, Environment.CreateArgs(new IDynamic[] { this }));
        }
    }
    //public struct LString : IString, IEquatable<LString>
    //{
    //    private readonly IEnvironment _environment;
    //    private readonly string _value;


    //    public LString(IEnvironment environment, string value)
    //    {
    //        _environment = environment;
    //        _value = value;
    //    }


    //    public string BaseValue
    //    {
    //        get { return _value; }
    //    }

    //    public LanguageTypeCode TypeCode
    //    {
    //        get { return LanguageTypeCode.String; }
    //    }

    //    public bool IsPrimitive
    //    {
    //        get { return true; }
    //    }

    //    public IDynamic Value
    //    {
    //        get { return this; }
    //        set { }
    //    }


    //    public IDynamic Op_LogicalNot()
    //    {
    //        return LType.Op_LogicalNot(_environment, this);
    //    }

    //    public IDynamic Op_LogicalOr(IDynamic other)
    //    {
    //        return LType.Op_LogicalOr(_environment, this, other);
    //    }

    //    public IDynamic Op_LogicalAnd(IDynamic other)
    //    {
    //        return LType.Op_LogicalAnd(_environment, this, other);
    //    }

    //    public IDynamic Op_BitwiseNot()
    //    {
    //        return LType.Op_BitwiseNot(_environment, this);
    //    }

    //    public IDynamic Op_BitwiseOr(IDynamic other)
    //    {
    //        return LType.Op_BitwiseOr(_environment, this, other);
    //    }

    //    public IDynamic Op_BitwiseXor(IDynamic other)
    //    {
    //        return LType.Op_BitwiseXor(_environment, this, other);
    //    }

    //    public IDynamic Op_BitwiseAnd(IDynamic other)
    //    {
    //        return LType.Op_BitwiseAnd(_environment, this, other);
    //    }

    //    public IDynamic Op_Equals(IDynamic other)
    //    {
    //        switch (other.TypeCode)
    //        {
    //            case LanguageTypeCode.String:
    //                return _environment.CreateBoolean(this._value == ((LString)other)._value);
    //            case LanguageTypeCode.Number:
    //                return this.ConvertToNumber().Op_Equals(other);
    //            case LanguageTypeCode.Object:
    //                return this.ConvertToPrimitive(null).Op_Equals(other);
    //            default:
    //                return _environment.False;
    //        }
    //    }

    //    public IDynamic Op_DoesNotEquals(IDynamic other)
    //    {
    //        return LType.Op_DoesNotEquals(_environment, this, other);
    //    }

    //    public IDynamic Op_StrictEquals(IDynamic other)
    //    {
    //        switch (other.TypeCode)
    //        {
    //            case LanguageTypeCode.String:
    //                return _environment.CreateBoolean(this._value == ((LString)other)._value);
    //            default:
    //                return _environment.False;
    //        }
    //    }

    //    public IDynamic Op_StrictDoesNotEquals(IDynamic other)
    //    {
    //        return LType.Op_StrictDoesNotEquals(_environment, this, other);
    //    }

    //    public IDynamic Op_Lessthan(IDynamic other)
    //    {
    //        return LType.Op_Lessthan(_environment, this, other);
    //    }

    //    public IDynamic Op_Greaterthan(IDynamic other)
    //    {
    //        return LType.Op_Greaterthan(_environment, this, other);
    //    }

    //    public IDynamic Op_LessthanOrEqual(IDynamic other)
    //    {
    //        return LType.Op_LessthanOrEqual(_environment, this, other);
    //    }

    //    public IDynamic Op_GreaterthanOrEqual(IDynamic other)
    //    {
    //        return LType.Op_GreaterthanOrEqual(_environment, this, other);
    //    }

    //    public IDynamic Op_Instanceof(IDynamic other)
    //    {
    //        return LType.Op_GreaterthanOrEqual(_environment, this, other);
    //    }

    //    public IDynamic Op_In(IDynamic other)
    //    {
    //        return LType.Op_In(_environment, this, other);
    //    }

    //    public IDynamic Op_LeftShift(IDynamic other)
    //    {
    //        return LType.Op_LeftShift(_environment, this, other);
    //    }

    //    public IDynamic Op_SignedRightShift(IDynamic other)
    //    {
    //        return LType.Op_SignedRightShift(_environment, this, other);
    //    }

    //    public IDynamic Op_UnsignedRightShift(IDynamic other)
    //    {
    //        return LType.Op_UnsignedRightShift(_environment, this, other);
    //    }

    //    public IDynamic Op_Addition(IDynamic other)
    //    {
    //        return LType.Op_Addition(_environment, this, other);
    //    }

    //    public IDynamic Op_Subtraction(IDynamic other)
    //    {
    //        return LType.Op_Subtraction(_environment, this, other);
    //    }

    //    public IDynamic Op_Multiplication(IDynamic other)
    //    {
    //        return LType.Op_Multiplication(_environment, this, other);
    //    }

    //    public IDynamic Op_Division(IDynamic other)
    //    {
    //        return LType.Op_Division(_environment, this, other);
    //    }

    //    public IDynamic Op_Modulus(IDynamic other)
    //    {
    //        return LType.Op_Modulus(_environment, this, other);
    //    }

    //    public IDynamic Op_Delete()
    //    {
    //        return _environment.True;
    //    }

    //    public IDynamic Op_Void()
    //    {
    //        return _environment.Undefined;
    //    }

    //    public IDynamic Op_Typeof()
    //    {
    //        return _environment.CreateString("string");
    //    }

    //    public IDynamic Op_PrefixIncrement()
    //    {
    //        throw _environment.CreateReferenceError("");
    //    }

    //    public IDynamic Op_PrefixDecrement()
    //    {
    //        throw _environment.CreateReferenceError("");
    //    }

    //    public IDynamic Op_Plus()
    //    {
    //        return ConvertToNumber();
    //    }

    //    public IDynamic Op_Minus()
    //    {
    //        return LType.Op_Minus(_environment, this);
    //    }

    //    public IDynamic Op_PostfixIncrement()
    //    {
    //        throw _environment.CreateReferenceError("");
    //    }

    //    public IDynamic Op_PostfixDecrement()
    //    {
    //        throw _environment.CreateReferenceError("");
    //    }

    //    public IDynamic Op_GetProperty(IDynamic name)
    //    {
    //        return LType.Op_GetProperty(_environment, this, name);
    //    }

    //    public void Op_SetProperty(IDynamic name, IDynamic value)
    //    {
    //        LType.Op_SetProperty(_environment, this, name, value);
    //    }

    //    public IDynamic Op_Call(IArgs args)
    //    {
    //        return LType.Op_Call(_environment, this, args);
    //    }

    //    public IObject Op_Construct(IArgs args)
    //    {
    //        return LType.Op_Construct(_environment, this, args);
    //    }

    //    public void Op_Throw()
    //    {
    //        LType.Op_Throw(_environment, this);
    //    }

    //    public IDynamic ConvertToPrimitive(string preferredType)
    //    {
    //        return this;
    //    }

    //    public IBoolean ConvertToBoolean()
    //    {
    //        return _environment.CreateBoolean(_value.Length > 0);
    //    }

    //    public INumber ConvertToNumber()
    //    {
    //        return _environment.CreateNumber(Machete.Compiler.StringNumericLiteral.eval(BaseValue));
    //    }

    //    public IString ConvertToString()
    //    {
    //        return this;
    //    }

    //    public IObject ConvertToObject()
    //    {
    //        return ((IConstructable)_environment.StringConstructor).Construct(_environment, _environment.CreateArgs(new IDynamic[] { this }));
    //    }

    //    public INumber ConvertToInteger()
    //    {
    //        return LType.ConvertToInteger(_environment, this);
    //    }

    //    public INumber ConvertToInt32()
    //    {
    //        return LType.ConvertToInt32(_environment, this);
    //    }

    //    public INumber ConvertToUInt32()
    //    {
    //        return LType.ConvertToUInt32(_environment, this);
    //    }

    //    public INumber ConvertToUInt16()
    //    {
    //        return LType.ConvertToUInt16(_environment, this);
    //    }
        

    //    IDynamic IReferenceBase.Get(string name, bool strict)
    //    {
    //        return LType.GetValue(_environment, this, name, strict);
    //    }

    //    void IReferenceBase.Set(string name, IDynamic value, bool strict)
    //    {
    //        LType.SetValue(_environment, this, name, value, strict);
    //    }


    //    public override bool Equals(object obj)
    //    {
    //        return obj is LString && Equals((LString)obj);
    //    }

    //    public bool Equals(LString other)
    //    {
    //        return this._value == other._value;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return _value.GetHashCode();
    //    }

    //    public override string ToString()
    //    {
    //        return _value;
    //    }

    //    public static bool operator ==(LString left, LString right)
    //    {
    //        return left.Equals(right);
    //    }

    //    public static bool operator !=(LString left, LString right)
    //    {
    //        return !left.Equals(right);
    //    }

    //    public static explicit operator string(LString value)
    //    {
    //        return value._value;
    //    }
    //}
}