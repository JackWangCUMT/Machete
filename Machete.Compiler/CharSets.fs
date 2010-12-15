﻿namespace Machete.Compiler

module CharSets =

    open System
    open System.Globalization

    let whiteSpaceCharSet =  
        set [ 
            '\u0009';'\u000b';
            '\u000c';'\u0020';
            '\u00a0';'\u1680';
            '\u180e';'\u2000';
            '\u2001';'\u2002';
            '\u2003';'\u2004';
            '\u2005';'\u2006';
            '\u2007';'\u2008';
            '\u2009';'\u200a';
            '\u202f';'\u205f';
            '\u3000';'\ufeff';
        ]

    let lineTerminatorCharSet = 
        set [ 
            '\u000a';'\u000d';
            '\u2028';'\u2029'; 
        ]

    let isUnicodeLetter c =
        match Char.GetUnicodeCategory(c) with
        | UnicodeCategory.UppercaseLetter
        | UnicodeCategory.LowercaseLetter 
        | UnicodeCategory.TitlecaseLetter 
        | UnicodeCategory.ModifierLetter
        | UnicodeCategory.OtherLetter
        | UnicodeCategory.LetterNumber -> true
        | _ -> false
          

//    let unicodeLetterCharSet = 
//        set [
//            for c in [Char.MinValue..Char.MaxValue] do 
//                match Char.GetUnicodeCategory(c) with
//                | UnicodeCategory.UppercaseLetter
//                | UnicodeCategory.LowercaseLetter 
//                | UnicodeCategory.TitlecaseLetter 
//                | UnicodeCategory.ModifierLetter
//                | UnicodeCategory.OtherLetter
//                | UnicodeCategory.LetterNumber -> yield c
//                | _ -> ()
//        ]

    let isUnicodeCombiningMark c =
        match Char.GetUnicodeCategory c with
        | UnicodeCategory.NonSpacingMark
        | UnicodeCategory.SpacingCombiningMark -> true
        | _ -> false  

//    let unicodeCombiningMarkCharSet = 
//        set [
//            for c in [Char.MinValue..Char.MaxValue] do 
//                match Char.GetUnicodeCategory(c) with
//                | UnicodeCategory.NonSpacingMark
//                | UnicodeCategory.SpacingCombiningMark -> yield c
//                | _ -> ()
//        ]

    let isUnicodeDigit c =
        match Char.GetUnicodeCategory c with
        | UnicodeCategory.DecimalDigitNumber -> true
        | _ -> false 

//    let unicodeDigitCharSet = 
//        set [
//            for c in [Char.MinValue..Char.MaxValue] do 
//                match Char.GetUnicodeCategory(c) with
//                | UnicodeCategory.DecimalDigitNumber -> yield c
//                | _ -> ()
//        ]

    let isUnicodeConnectorPunctuation c =
        match Char.GetUnicodeCategory c with
        | UnicodeCategory.ConnectorPunctuation -> true
        | _ -> false 

//    let unicodeConnectorPunctuationCharSet = 
//        set [
//            for c in [Char.MinValue..Char.MaxValue] do 
//                match Char.GetUnicodeCategory(c) with
//                | UnicodeCategory.ConnectorPunctuation -> yield c
//                | _ -> ()
//        ]

    
//    let nonLineTerminatorCharSet = 
//        set [
//            for c in [Char.MinValue..Char.MaxValue] do 
//                if not (lineTerminatorCharSet.Contains c) then 
//                    yield c
//        ]

    let isNonLineTerminator c = 
        not (lineTerminatorCharSet.Contains c)

    let decimalDigitCharSet = 
        set ['0'..'9']

    let isNonDecimalDigit c = 
        not (decimalDigitCharSet.Contains c)

//    let nonDecimalDigitCharSet = 
//        set [
//            for c in [Char.MinValue..Char.MaxValue] do 
//                if not (decimalDigitCharSet.Contains c) then 
//                    yield c
//        ]

    let nonZeroDigitCharSet = 
        set ['1'..'9']

    let hexDigitCharSet = 
        set (['0'..'9'] @ ['a'..'f'] @ ['A'..'F'])

    let keyWordSet =
        set [
            "break"; 
            "case"; "catch"; "continue";
            "debugger"; "default"; "delete"; "do";  
            "else"; 
            "finally"; "for"; "function"; 
            "if"; "in"; "instanceof";
            "new";
            "return";
            "switch";
            "this"; "throw"; "try"; "typeof";
            "var"; "void";
            "while"; "with";
        ]

    let futureReservedWordSet = 
        set [
            "class"; "const"; 
            "enum"; "export"; "extends";
            "implements"; "import"; "interface";
            "let";
            "package"; "private"; "protected"; "public";
            "static"; "super";
            "yield";
        ]

    let reservedWordSet =
        Set.union keyWordSet futureReservedWordSet 
//        Set.ofList [
//            "break"
//            "case"
//            "catch"
//            "continue"
//            "debugger"
//            "default"
//            "delete"
//            "do"
//            "else"
//            "finally"
//            "for"
//            "function"
//            "if"
//            "in"
//            "instanceof"
//            "new"
//            "return"
//            "switch"
//            "this"
//            "throw"
//            "try"
//            "typeof"
//            "var"
//            "void"
//            "while"
//            "with"
//            "class"
//            "const"
//            "enum"
//            "export"
//            "extends"
//            "implements"
//            "import"
//            "interface"
//            "let"
//            "package"
//            "private"
//            "protected"
//            "public"
//            "static"
//            "super"
//            "yield"
//            "null"
//            "true"
//            "false"
//        ]
