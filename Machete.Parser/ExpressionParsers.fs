﻿namespace Machete.Parser

open FParsec.OperatorPrecedenceParser
open FParsec.CharParsers
open FParsec.Primitives
open InputElementParsers

module ExpressionParsers =

    let skippable = [
        parseWhiteSpace
        parseLineTerminator
        parseMultiLineComment
        parseSingleLineComment
    ]

    let skipOver state = 
        (skipMany (choice skippable)) state

    let skipOverThen parser state = 
        (skipMany (choice skippable) .>> parser) state
        
    module private Operators = 
        let postfixIncrement = PostfixOp ("++", skipOver, 17, true, fun x -> PostfixExpression (x, Some PostfixIncrement))
        let postfixDecrement = PostfixOp ("--", skipOver, 17, true, fun x -> PostfixExpression (x, Some PostfixDecrement))
        let delete = PrefixOp ("delete", skipOver, 16, true, fun x -> UnaryExpression (Some Delete, x))
        let typeof = PrefixOp ("typeof", skipOver, 16, true, fun x -> UnaryExpression (Some Typeof, x))
        let void' = PrefixOp ("void", skipOver, 16, true, fun x -> UnaryExpression (Some Void, x))
        let prefixIncrement = PrefixOp ("++", skipOver, 15, true, fun x -> UnaryExpression (Some PrefixIncrement, x))
        let prefixDecrement = PrefixOp ("--", skipOver, 15, true, fun x -> UnaryExpression (Some PrefixDecrement, x))
        let plus = PrefixOp ("+", skipOver, 15, true, fun x -> UnaryExpression (Some Plus, x))
        let minus = PrefixOp ("-", skipOver, 15, true, fun x -> UnaryExpression (Some Minus, x))
        let bitwiseNot = PrefixOp ("~", skipOver, 15, true, fun x -> UnaryExpression (Some BitwiseNot, x))
        let logicalNot = PrefixOp ("!", skipOver, 15, true, fun x -> UnaryExpression (Some LogicalNot, x))         
        let multiply = InfixOp("*", skipOver, 14, Assoc.Left, fun x y -> MultiplicativeExpression (x, Some (Multiply, y)))
        let divide = InfixOp("/", skipOver, 14, Assoc.Left, fun x y -> MultiplicativeExpression (x, Some (Divide, y)))
        let remainder = InfixOp("%", skipOver, 14, Assoc.Left, fun x y -> MultiplicativeExpression (x, Some (Remainder, y)))          
        let add = InfixOp("+", skipOver, 13, Assoc.Left, fun x y -> AdditiveExpression (x, Some (Add, y)))
        let subtract = InfixOp("-", skipOver, 13, Assoc.Left, fun x y -> AdditiveExpression (x, Some (Subtract, y)))
        let leftShift = InfixOp("<<", skipOver, 12, Assoc.Left, fun x y -> ShiftExpression (x, Some (LeftShift, y)))  
        let signedRightShift = InfixOp(">>", skipOver, 12, Assoc.Left, fun x y -> ShiftExpression (x, Some (SignedRightShift, y)))
        let unsignedRightShift = InfixOp(">>>", skipOver, 12, Assoc.Left, fun x y -> ShiftExpression (x, Some (UnsignedRightShift, y)))
        let lessThan = InfixOp("<", skipOver, 11, Assoc.Left, fun x y -> RelationalExpression (x, Some (LessThan, y)))
        let greaterThan = InfixOp(">", skipOver, 11, Assoc.Left, fun x y -> RelationalExpression (x, Some (GreaterThan, y)))
        let lessThanOrEqual = InfixOp("<=", skipOver, 11, Assoc.Left, fun x y -> RelationalExpression (x, Some (LessThanOrEqual, y)))
        let greaterThanOrEqual = InfixOp(">=", skipOver, 11, Assoc.Left, fun x y -> RelationalExpression (x, Some (GreaterThanOrEqual, y)))
        let instanceof = InfixOp("instanceof", skipOver, 11, Assoc.Left, fun x y -> RelationalExpression (x, Some (Instanceof, y)))
        let in' = InfixOp("in", skipOver, 11, Assoc.Left, fun x y -> RelationalExpression (x, Some (In, y)))        
        let equals = InfixOp("==", skipOver, 10, Assoc.Left, fun x y -> EqualityExpression (x, Some (Equals, y)))
        let doesNotEquals = InfixOp("!=", skipOver, 10, Assoc.Left, fun x y -> EqualityExpression (x, Some (DoesNotEquals, y)))
        let strictEquals = InfixOp("===", skipOver, 10, Assoc.Left, fun x y -> EqualityExpression (x, Some (StrictEquals, y)))
        let strictDoesNotEquals = InfixOp("!==", skipOver, 10, Assoc.Left, fun x y -> EqualityExpression (x, Some (StrictDoesNotEquals, y)))
        let bitwiseAnd = InfixOp("&", skipOver, 9, Assoc.Left, fun x y -> BitwiseANDExpression (x, Some y))
        let bitwiseXor = InfixOp("^", skipOver, 8, Assoc.Left, fun x y -> BitwiseXORExpression (x, Some y))
        let bitwiseOr = InfixOp("|", skipOver, 7, Assoc.Left, fun x y -> BitwiseORExpression (x, Some y))
        let logicalAnd = InfixOp("&&", skipOver, 6, Assoc.Left, fun x y -> LogicalANDExpression (x, Some y))
        let logicalOr = InfixOp("||", skipOver, 5, Assoc.Left, fun x y -> LogicalORExpression (x, Some y))
        let conditional = TernaryOp("?", skipOver, ":", skipOver, 4, Assoc.Right, fun x y z -> ConditionalExpression (x, Some (y, z)))
        let simpleAssignment = InfixOp("=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (Simple, y)))
        let compoundMultiplyAssignment = InfixOp("*=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundMultiply, y)))
        let compoundDivideAssignment = InfixOp("/=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundDivide, y)))
        let compoundRemainderAssignment = InfixOp("%=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundRemainder, y)))
        let compoundAddAssignment = InfixOp("+=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundAdd, y)))
        let compoundSubtractAssignment = InfixOp("-=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundSubtract, y)))        
        let compoundLeftShiftAssignment = InfixOp("<<=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundLeftShift, y)))
        let compoundSignedRightShiftAssignment = InfixOp(">>=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundSignedRightShift, y)))
        let compoundUnsignedRightShiftAssignment = InfixOp(">>>=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundUnsignedRightShift, y)))
        let compoundBitwiseAndAssignment = InfixOp("&=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundBitwiseAnd, y)))
        let compoundBitwiseXorAssignment = InfixOp("^=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundBitwiseXor, y)))
        let compoundBitwiseOrAssignment = InfixOp("|=", skipOver, 2, Assoc.Right, fun x y -> AssignmentExpression (x, Some (CompoundBitwiseOr, y)))
        let comma = InfixOp(",", skipOver, 1, Assoc.Left, fun x y -> Expression (x, Some y))

        let expressionOperators = [ 
            postfixIncrement
            postfixDecrement
            delete 
            typeof
            void'
            prefixIncrement
            prefixDecrement
            plus
            minus
            bitwiseNot
            logicalNot 
            multiply
            divide
            remainder
            add
            subtract
            leftShift
            signedRightShift
            unsignedRightShift
            lessThan
            greaterThan
            lessThanOrEqual
            greaterThanOrEqual
            instanceof
            in'
            equals
            doesNotEquals
            strictEquals
            strictDoesNotEquals
            bitwiseAnd 
            bitwiseXor 
            bitwiseOr
            logicalAnd 
            logicalOr
            conditional
            simpleAssignment       
            compoundMultiplyAssignment
            compoundDivideAssignment
            compoundRemainderAssignment
            compoundAddAssignment
            compoundSubtractAssignment
            compoundLeftShiftAssignment
            compoundSignedRightShiftAssignment
            compoundUnsignedRightShiftAssignment
            compoundBitwiseAndAssignment
            compoundBitwiseXorAssignment
            compoundBitwiseOrAssignment
            comma
        ]

        let expressionNoInOperators () = 
            expressionOperators |> List.filter (fun op -> op <> in')

        let assignmentExpressionOperators () = 
            expressionOperators |> List.filter (fun op -> op <> comma)

        let assignmentExpressionNoInOperators () = 
            expressionOperators |> List.filter (fun op -> op <> comma && op <> in')
                
    open Operators
    
    let skipIdentifierName name state =
        (skipOverThen (parseSpecificIdentifierName name) |>> ignore) state

    let skipPunctuator name state =
        (skipOverThen (pstring name) |>> ignore) state
    
    let rec parseTerm state = 
        (skipOver >>. parseLeftHandSideExpression .>> skipOver) state
        
    and parsePrimaryExpression state = 
        (parse {        
            let! r =  skipOver >>. parseLiteral .>> skipOver
            return PrimaryExpression r
        }) state
        
    and parseFunctionExpression state = 
        (parse {        
            let! r = parseLiteral .>> skipOver
            return PrimaryExpression r
        }) state
        
    and parseLambdaExpression state = 
        (parse {        
            let! r = parseLiteral .>> skipOver
            return PrimaryExpression r
        }) state

    and parseMemberExpression state = 
        (
        (attempt <| parse {
            do! skipIdentifierName "new"        
            let! r = parseMemberExpression
            let! a = opt parseArguments
            return MemberExpression (r, a)
        }) <|> parse {        
            let! r = parsePrimaryExpression
            let! t = opt parseMemberExpressionTail
            return MemberExpression (r, t)
        }
        ) state

    and parseMemberExpressionTail state = 
        (parse {
            let! c = skipOver >>. anyOf ".["    
            match c with
            | '.' ->
                let! e = parseIdentifierName
                match e with
                | IdentifierName (s, d) ->
                    let e = PrimaryExpression (StringLiteral (s, d))
                    let! t = opt parseMemberExpressionTail
                    return MemberExpressionTail (e, t)
            | '[' ->
                let! e = skipOver >>. parseExpression .>> skipOver .>> skipChar ']'
                let! t = opt parseMemberExpressionTail
                return MemberExpressionTail (e, t)
        }
        ) state

    and parseNewExpression state = 
        (
        parse {
            do! skipIdentifierName "new"        
            let! r = parseNewExpression
            return NewExpression r
        } <|> parse {        
            let! r = parseMemberExpression
            return NewExpression r
        }
        ) state

    and parseCallExpression state = 
        (parse {        
            let! r = parseLiteral .>> skipOver
            return PrimaryExpression r
        }) state

    and parseCallExpressionTail state = 
        (parse {        
            let! r = parseLiteral .>> skipOver
            return PrimaryExpression r
        }) state

    and parseArguments state = 
        (attempt ((skipPunctuator "(" >>. opt parseArgumentList .>> skipPunctuator ")") |>> Arguments)) state

    and parseArgumentList state = 
        (parse {        
            let! r = parseAssignmentExpression
            let! t = opt parseArgumentListTail
            return ArgumentList (r, t)
        }) state

    and parseArgumentListTail state = 
        (parse {        
            do! skipPunctuator ","
            let! r = parseAssignmentExpression
            let! t = opt parseArgumentListTail
            return ArgumentList (r, t)
        }) state

    and parseLeftHandSideExpression state = 
        ( 
        parse {        
            let! r = parseMemberExpression
            let! a = opt parseArguments
            match a with
            | Some _ ->
                let! t = opt parseCallExpressionTail                
                return CallExpression (r, a, t)
            | None ->
                return r
        } <|> parse {
            do! skipIdentifierName "new"        
            let! r = parseNewExpression
            return NewExpression r
        }
        ) state

    and parseExpression =
        let p = new OperatorPrecedenceParser<SourceElement, unit>()
        let e = p.ExpressionParser            
        p.TermParser <- parseTerm
        p.AddOperators expressionOperators
        fun state -> 
            (skipOver >>. e) state

    and parseExpressionNoIn =
        let p = new OperatorPrecedenceParser<SourceElement, unit>()
        let e = p.ExpressionParser            
        p.TermParser <- parseTerm
        p.AddOperators (expressionNoInOperators ())
        fun state -> 
            (skipOver >>. e) state

    and parseAssignmentExpression =
        let p = new OperatorPrecedenceParser<SourceElement, unit>()
        let e = p.ExpressionParser            
        p.TermParser <- parseTerm
        p.AddOperators (assignmentExpressionOperators ())
        fun state -> 
            (skipOver >>. e) state

    and parseAssignmentExpressionNoIn =
        let p = new OperatorPrecedenceParser<SourceElement, unit>()
        let e = p.ExpressionParser            
        p.TermParser <- parseTerm
        p.AddOperators (assignmentExpressionNoInOperators ())
        fun state -> 
            (skipOver >>. e) state


        

