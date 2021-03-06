﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Runtime.RuntimeTypes.LanguageTypes;
using Machete.Core;

namespace Machete.Runtime.NativeObjects.BuiltinObjects
{
    public sealed class BJson : LObject
    {
        public BJson(IEnvironment environment)
            : base(environment)
        {

        }

        public override void Initialize()
        {
            Class = "JSON";
            Extensible = true;
            Prototype = Environment.ObjectPrototype;
            base.Initialize();
        }

        [BuiltinFunction("parse", "text", "reviver"), DataDescriptor(true, false, true)]
        internal static IDynamic Parse(IEnvironment environment, IArgs args)
        {
            var text = args[0];
            var reviver = args[1];
            return Machete.Compiler.JsonParser.Parse(environment, text, reviver);
        }

        [BuiltinFunction("stringify", "value", "replacer", "space"), DataDescriptor(true, false, true)]
        internal static IDynamic Stringify(IEnvironment environment, IArgs args)
        {
            var value = args[0];
            var replacer = args[1];
            var space = args[2];
            return Machete.Compiler.JsonParser.Stringify(environment, value, replacer, space);
        }
    }
}
