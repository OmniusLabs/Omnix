using System;
using System.Collections.Generic;
using System.Linq;
using Omnius.Core.Serialization.RocketPack.DefinitionCompiler.Extensions;
using Sprache;

namespace Omnius.Core.Serialization.RocketPack.DefinitionCompiler
{
    internal static class FormatParser
    {
        // 「"」で囲まれた文字列を抽出するパーサー
        private static readonly Parser<string> _stringLiteralParser =
            from leading in Parse.WhiteSpace.Many()
            from openQuote in Parse.Char('\"')
            from fragments in Parse.Char('\\').Then(_ => Parse.AnyChar.Select(c => $"\\{c}")).Or(Parse.CharExcept("\\\"").Many().Text()).Many()
            from closeQuote in Parse.Char('\"')
            from trailing in Parse.WhiteSpace.Many()
            select string.Join(string.Empty, fragments).Replace("\\\"", "\"");

        // 英数字と'_'の文字列を抽出するパーサー
        private static readonly Parser<string> _nameParser =
            from name in Parse.Char(x => ('0' <= x && x <= '9') || ('A' <= x && x <= 'Z') || ('a' <= x && x <= 'z') || x == '_', "Name").AtLeastOnce().Text()
            select name;

        // example: syntax = v1.0
        private static readonly Parser<string> _syntaxParser =
            from keyword in Parse.String("syntax").TokenWithSkipComment()
            from equal in Parse.Char('=').TokenWithSkipComment()
            from type in Parse.String("v1.0").TokenWithSkipComment().Text()
            from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
            select type;

        // example: option csharp_namespace = "RocketPack.Messages";
        private static readonly Parser<OptionDefinition> _optionParser =
            from keyword in Parse.String("option").TokenWithSkipComment()
            from name in _nameParser.TokenWithSkipComment()
            from equal in Parse.Char('=').TokenWithSkipComment()
            from value in _stringLiteralParser.TokenWithSkipComment()
            from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
            select new OptionDefinition(name, value);

        // example: using "RocketPack.Messages";
        private static readonly Parser<UsingDefinition> _usingParser =
            from keyword in Parse.String("using").TokenWithSkipComment()
            from value in _stringLiteralParser.TokenWithSkipComment()
            from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
            select new UsingDefinition(value);

        // example: namespace "RocketPack.Messages";
        private static readonly Parser<NamespaceDefinition> _namespaceParser =
            from keyword in Parse.String("namespace").TokenWithSkipComment()
            from value in _stringLiteralParser.TokenWithSkipComment()
            from semicolon in Parse.Char(';').Or(Parse.Return(';')).TokenWithSkipComment()
            select new NamespaceDefinition(value);

        // example: [csharp_recyclable]
        private static readonly Parser<string> _attributeParser =
            from beginTag in Parse.Char('[').TokenWithSkipComment()
            from name in Parse.CharExcept(']').AtLeastOnce().TokenWithSkipComment().Text()
            from endTag in Parse.Char(']').TokenWithSkipComment()
            select name;

        // example: capacity: 1024,
        private static readonly Parser<(string key, string value)> _parametersElementParser =
            from key in _nameParser.TokenWithSkipComment()
            from colon in Parse.Char(':').TokenWithSkipComment()
            from value in _nameParser.TokenWithSkipComment()
            from comma in Parse.Char(',').Or(Parse.Return(',')).TokenWithSkipComment()
            select (key, value);

        // example: (capacity: 1024, recyclable: true)
        private static readonly Parser<Dictionary<string, string>> _parametersParser =
            from beginTag in Parse.Char('(').TokenWithSkipComment()
            from elements in _parametersElementParser.Many().TokenWithSkipComment()
            from endTag in Parse.Char(')').TokenWithSkipComment()
            select new Dictionary<string, string>(elements.Select(n => new KeyValuePair<string, string>(n.key, n.value)));

        private static readonly Parser<IntType> _intTypeParser =
            from isSigned in Parse.Char('u').Then(n => Parse.Return(false)).Or(Parse.Return(true))
            from type in Parse.String("int")
            from size in Parse.Decimal
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new IntType(isSigned, int.Parse(size), isOptional, parameters);

        private static readonly Parser<BoolType> _boolTypeParser =
            from type in Parse.String("bool").TokenWithSkipComment()
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new BoolType(isOptional, parameters);

        private static readonly Parser<FloatType> _floatTypeParser =
            from type in Parse.String("float").TokenWithSkipComment()
            from size in Parse.Decimal.TokenWithSkipComment()
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new FloatType(int.Parse(size), isOptional, parameters);

        private static readonly Parser<StringType> _stringTypeParser =
            from type in Parse.String("string").TokenWithSkipComment()
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new StringType(isOptional, parameters);

        private static readonly Parser<TimestampType> _timestampTypeParser =
            from type in Parse.String("timestamp")
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new TimestampType(isOptional, parameters);

        private static readonly Parser<BytesType> _memoryTypeParser =
            from type in Parse.String("bytes").TokenWithSkipComment()
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new BytesType(isOptional, parameters);

        private static readonly Parser<CustomType> _customTypeParser =
            from type in _nameParser.Text()
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new CustomType(type, isOptional, parameters);

        private static readonly Parser<VectorType> _vectorTypeParser =
            from type in Parse.String("vector").TokenWithSkipComment()
            from beginType in Parse.String("<").TokenWithSkipComment()
            from elementType in _boolTypeParser
                .Or<TypeBase>(_intTypeParser)
                .Or(_floatTypeParser)
                .Or(_stringTypeParser)
                .Or(_timestampTypeParser)
                .Or(_memoryTypeParser)
                .Or(_customTypeParser).TokenWithSkipComment()
            from endType in Parse.String(">").TokenWithSkipComment()
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new VectorType(elementType, isOptional, parameters);

        private static readonly Parser<MapType> _mapTypeParser =
            from type in Parse.String("map").TokenWithSkipComment()
            from beginType in Parse.Char('<').TokenWithSkipComment()
            from keyType in _boolTypeParser
                .Or<TypeBase>(_intTypeParser)
                .Or(_floatTypeParser)
                .Or(_stringTypeParser)
                .Or(_timestampTypeParser)
                .Or(_memoryTypeParser)
                .Or(_customTypeParser).TokenWithSkipComment()
            from comma in Parse.Char(',').TokenWithSkipComment()
            from valueType in _boolTypeParser
                .Or<TypeBase>(_intTypeParser)
                .Or(_floatTypeParser)
                .Or(_stringTypeParser)
                .Or(_timestampTypeParser)
                .Or(_memoryTypeParser)
                .Or(_customTypeParser).TokenWithSkipComment()
            from endType in Parse.Char('>').TokenWithSkipComment()
            from isOptional in Parse.Char('?').Then(n => Parse.Return(true)).Or(Parse.Return(false)).TokenWithSkipComment()
            from parameters in _parametersParser.Or(Parse.Return(new Dictionary<string, string>())).TokenWithSkipComment()
            select new MapType(keyType, valueType, isOptional, parameters);

        private static readonly Parser<EnumElement> _enumElementParser =
            from attributes in _attributeParser.Many().TokenWithSkipComment()
            from name in _nameParser.TokenWithSkipComment()
            from equal in Parse.Char('=').TokenWithSkipComment()
            from id in Parse.Decimal.TokenWithSkipComment()
            from comma in Parse.Char(',').TokenWithSkipComment()
            select new EnumElement(attributes.ToList(), name, int.Parse(id));

        private static readonly Parser<EnumDefinition> _enumDefinitionParser =
            from attributes in _attributeParser.Many().TokenWithSkipComment()
            from keyword in Parse.String("enum").TokenWithSkipComment()
            from name in _nameParser.TokenWithSkipComment()
            from colon in Parse.Char(':').TokenWithSkipComment()
            from type in _intTypeParser
            from beginTag in Parse.Char('{').TokenWithSkipComment()
            from enumProperties in _enumElementParser.Except(Parse.Char('}')).Many().TokenWithSkipComment()
            from endTag in Parse.Char('}').TokenWithSkipComment()
            select new EnumDefinition(attributes.ToList(), name, type, enumProperties.ToList());

        private static readonly Parser<ObjectElement> _structElementParser =
            from attributes in _attributeParser.Many().TokenWithSkipComment()
            from name in _nameParser.TokenWithSkipComment()
            from colon in Parse.Char(':').TokenWithSkipComment()
            from type in _boolTypeParser
                .Or<TypeBase>(_intTypeParser)
                .Or(_floatTypeParser)
                .Or(_stringTypeParser)
                .Or(_timestampTypeParser)
                .Or(_memoryTypeParser)
                .Or(_vectorTypeParser)
                .Or(_mapTypeParser)
                .Or(_customTypeParser).TokenWithSkipComment()
            from comma in Parse.Char(',').TokenWithSkipComment()
            select new ObjectElement(attributes.ToList(), name, type, null);

        private static readonly Parser<ObjectDefinition> _structDefinitionParser =
            from attributes in _attributeParser.Many().TokenWithSkipComment()
            from keyword in Parse.String("struct").TokenWithSkipComment()
            from name in _nameParser.TokenWithSkipComment()
            from beginTag in Parse.Char('{').TokenWithSkipComment()
            from elements in _structElementParser.Except(Parse.Char('}')).Many().TokenWithSkipComment()
            from endTag in Parse.Char('}').TokenWithSkipComment()
            select new ObjectDefinition(attributes.ToList(), name, MessageFormatType.Struct, elements.ToList());

        private static readonly Parser<ObjectElement> _tableElementParser =
            from attributes in _attributeParser.Many().TokenWithSkipComment()
            from name in _nameParser.TokenWithSkipComment()
            from colon in Parse.Char(':').TokenWithSkipComment()
            from type in _boolTypeParser
                .Or<TypeBase>(_intTypeParser)
                .Or(_floatTypeParser)
                .Or(_stringTypeParser)
                .Or(_timestampTypeParser)
                .Or(_memoryTypeParser)
                .Or(_vectorTypeParser)
                .Or(_mapTypeParser)
                .Or(_customTypeParser).TokenWithSkipComment()
            from equal in Parse.Char('=').Or(Parse.Return('=')).TokenWithSkipComment()
            from id in Parse.Decimal.TokenWithSkipComment()
            from comma in Parse.Char(',').TokenWithSkipComment()
            select new ObjectElement(attributes.ToList(), name, type, int.Parse(id));

        private static readonly Parser<ObjectDefinition> _tableDefinitionParser =
            from attributes in _attributeParser.Many().TokenWithSkipComment()
            from keyword in Parse.String("table").TokenWithSkipComment()
            from name in _nameParser.TokenWithSkipComment()
            from beginTag in Parse.Char('{').TokenWithSkipComment()
            from elements in _tableElementParser.Except(Parse.Char('}')).Many().TokenWithSkipComment()
            from endTag in Parse.Char('}').TokenWithSkipComment()
            select new ObjectDefinition(attributes.ToList(), name, MessageFormatType.Table, elements.ToList());

        private static readonly Parser<RocketPackDefinition> _omniPackDefinitionParser =
            from syntax in _syntaxParser.Once().TokenWithSkipComment()
            from usings in _usingParser.Many().TokenWithSkipComment()
            from @namespace in _namespaceParser.Once().TokenWithSkipComment()
            from options in _optionParser.Many().TokenWithSkipComment()
            from contents in _enumDefinitionParser.Or<object>(_structDefinitionParser).Or(_tableDefinitionParser).Many().TokenWithSkipComment()
            select new RocketPackDefinition(
                usings,
                @namespace.First(),
                options,
                contents.OfType<EnumDefinition>().ToList(),
                contents.OfType<ObjectDefinition>().ToList()
            );

        public static RocketPackDefinition ParseV1_0(string text)
        {
            var result = _omniPackDefinitionParser.Parse(text);

            // struct形式のメッセージはOptional型は認めない。
            foreach (var objectDefinition in result.Objects.Where(n => n.FormatType == MessageFormatType.Struct))
            {
                if (objectDefinition.Elements.Any(n => n.Type.IsOptional))
                {
                    throw new Exception();
                }
            }

            return result;
        }
    }
}
