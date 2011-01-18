﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Machete.Core
{
    public interface IEnvironment
    {
        Output Output { get; }

        IExecutionContext Context { get; }
        ILexicalEnvironment GlobalEnvironment { get; }
        IArgs EmptyArgs { get; }
        IUndefined Undefined { get; }
        INull Null { get; }
        IBoolean True { get; }
        IBoolean False { get; }
        IObject GlobalObject { get; }
        IObject ObjectConstructor { get; }
        IObject ObjectPrototype { get; }
        IObject FunctionConstructor { get; }
        IObject FunctionPrototype { get; }
        IObject ArrayConstructor { get; }
        IObject ArrayPrototype { get; }
        IObject StringConstructor { get; }
        IObject StringPrototype { get; }
        IObject BooleanConstructor { get; }
        IObject BooleanPrototype { get; }
        IObject NumberConstructor { get; }
        IObject NumberPrototype { get; }
        IObject MathObject { get; }
        IObject DateConstructor { get; }
        IObject DatePrototype { get; }
        IObject RegExpConstructor { get; }
        IObject RegExpPrototype { get; }
        IObject ErrorConstructor { get; }
        IObject ErrorPrototype { get; }
        IObject EvalErrorConstructor { get; }
        IObject EvalErrorPrototype { get; }
        IObject RangeErrorConstructor { get; }
        IObject RangeErrorPrototype { get; }
        IObject ReferenceErrorConstructor { get; }
        IObject ReferenceErrorPrototype { get; }
        IObject SyntaxErrorConstructor { get; }
        IObject SyntaxErrorPrototype { get; }
        IObject TypeErrorConstructor { get; }
        IObject TypeErrorPrototype { get; }
        IObject UriErrorConstructor { get; }
        IObject UriErrorPrototype { get; }
        IObject JsonObject { get; }

        IFunction ThrowTypeErrorFunction { get; } 

        IReference CreateReference(string name, IReferenceBase @base, bool strict);
        IBoolean CreateBoolean(bool value);
        IString CreateString(string value);
        INumber CreateNumber(double value);

        IArgs CreateArgs(IDynamic value);
        IArgs CreateArgs(IEnumerable<IDynamic> values);
        IArgs ConcatArgs(IArgs first, IArgs second);

        IDynamic FromPropertyDescriptor(IPropertyDescriptor desc);
        IPropertyDescriptor ToPropertyDescriptor(IObject obj);

        IObject CreateArray();
        IObject CreateObject();
        IObject CreateRegExp(string pattern, string flags);

        MacheteRuntimeException CreateError(string message);
        MacheteRuntimeException CreateEvalError(string message);
        MacheteRuntimeException CreateRangeError(string message);
        MacheteRuntimeException CreateReferenceError(string message);
        MacheteRuntimeException CreateSyntaxError(string message);
        MacheteRuntimeException CreateTypeError(string message);
        MacheteRuntimeException CreateUriError(string message);

        IObject CreateArguments(ReadOnlyList<string> formalParameterList, IArgs args, bool strict);

        //IFunction CreateFunction(ReadOnlyList<string> formalParameterList, bool strict, Lazy<Code> code);
        //IFunction CreateFunction(ReadOnlyList<string> formalParameterList, bool strict, Lazy<Code> code, ILexicalEnvironment scope);
        IObject CreateFunction(ExecutableCode executableCode, ReadOnlyList<string> formalParameters, ILexicalEnvironment scope);


        IPropertyDescriptor CreateGenericDescriptor();
        IPropertyDescriptor CreateGenericDescriptor(bool? enumerable);
        IPropertyDescriptor CreateGenericDescriptor(bool? enumerable, bool? configurable);

        IPropertyDescriptor CreateDataDescriptor(IDynamic value);
        IPropertyDescriptor CreateDataDescriptor(IDynamic value, bool? writable);
        IPropertyDescriptor CreateDataDescriptor(IDynamic value, bool? writable, bool? enumerable);
        IPropertyDescriptor CreateDataDescriptor(IDynamic value, bool? writable, bool? enumerable, bool? configurable);

        IPropertyDescriptor CreateAccessorDescriptor(IDynamic get, IDynamic set);
        IPropertyDescriptor CreateAccessorDescriptor(IDynamic get, IDynamic set, bool? enumerable);
        IPropertyDescriptor CreateAccessorDescriptor(IDynamic get, IDynamic set, bool? enumerable, bool? configurable);

        IExecutionContext EnterContext();

        void ThrowRuntimeException(IDynamic thrown);
        void CheckObjectCoercible(IDynamic value);

        bool Instanceof(IDynamic left, IDynamic right);
        IDynamic Execute(ExecutableCode executableCode);
        void BindFunctionDeclarations(ReadOnlyList<FunctionDeclaration> functionDeclarations, bool strict, bool configurableBindings);
        void BindVariableDeclarations(ReadOnlyList<string> variableDeclarations, bool strict, bool configurableBindings);

        void EnterGlobalCode(ReadOnlyList<string> variableDeclarations, ReadOnlyList<FunctionDeclaration> functionDeclarations, bool strict);
        void EnterEvalCode(ReadOnlyList<string> variableDeclarations, ReadOnlyList<FunctionDeclaration> functionDeclarations, bool strict);
        void EnterFunctionCode(ReadOnlyList<string> variableDeclarations, ReadOnlyList<FunctionDeclaration> functionDeclarations, IArgs args);
    }
}