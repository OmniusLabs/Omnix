using System;
using System.Collections.Generic;
using System.Linq;
using Omnius.Core.RocketPack.DefinitionCompiler.Configuration;
using Omnius.Core.RocketPack.DefinitionCompiler.Models;

namespace Omnius.Core.RocketPack.DefinitionCompiler
{
    internal partial class CodeGenerator
    {
        private readonly RocketPackDefinition _rootDefinition;
        private readonly List<RocketPackDefinition> _externalDefinitions;

        public CodeGenerator(RocketPackDefinition rootDefinition, IEnumerable<RocketPackDefinition> externalDefinitions)
        {
            _rootDefinition = rootDefinition;
            _externalDefinitions = externalDefinitions.ToList();
        }

        public string Generate()
        {
            var b = new CodeWriter();

            // 自動生成されたと宣言する
            b.WriteLine("// <auto-generated/>");

            // Nullableを有効化する
            b.WriteLine("#nullable enable");
            b.WriteLine();

            // namespaceの宣言を行う。
            {
                b.WriteLine($"namespace {_rootDefinition.CSharpNamespace}");
                b.WriteLine("{");

                using (b.Indent())
                {
                    var accessLevel = this.GetAccessLevel();

                    var enumWriter = new EnumWriter();
                    foreach (var enumDefinition in _rootDefinition.Enums)
                    {
                        enumWriter.Write(b, enumDefinition, accessLevel);
                    }

                    var objectWriter = new ObjectWriter(_rootDefinition, _externalDefinitions);
                    foreach (var objectDefinition in _rootDefinition.Objects)
                    {
                        objectWriter.Write(b, objectDefinition, accessLevel);
                    }

                    var serviceWriter = new ServiceWriter(_rootDefinition, _externalDefinitions);
                    foreach (var serviceDefinition in _rootDefinition.Services)
                    {
                        serviceWriter.Write(b, serviceDefinition, accessLevel);
                    }
                }

                b.WriteLine("}");
            }

            return b.ToString();
        }

        private string GetAccessLevel()
        {
            var accessLevelOption = _rootDefinition.Options.FirstOrDefault(n => n.Name == "csharp_access_level");
            return accessLevelOption?.Value as string ?? "public";
        }

        private static string GenerateTypeFullName(string name, params string[] types)
        {
            var result = name switch
            {
                "AsyncDisposableBase" => "Omnius.Core.AsyncDisposableBase",
                "ArgumentNullException" => "System.ArgumentNullException",
                "ArgumentOutOfRangeException" => "System.ArgumentOutOfRangeException",
                "Array" => "System.Array",
                "BytesOperations" => "Omnius.Core.BytesOperations",
                "CancellationToken" => $"System.Threading.CancellationToken",
                "CollectionHelper" => "Omnius.Core.Helpers.CollectionHelper",
                "Dictionary<,>" => $"System.Collections.Generic.Dictionary<{types[0]}, {types[1]}>",
                "FormatException" => "System.FormatException",
                "HashCode" => "System.HashCode",
                "IBufferWriter<>" => $"System.Buffers.IBufferWriter<{types[0]}>",
                "IBytesPool" => "Omnius.Core.IBytesPool",
                "IConnection" => $"Omnius.Core.Net.Connections.IConnection",
                "IDisposable" => "System.IDisposable",
                "IMemoryOwner<>" => $"System.Buffers.IMemoryOwner<{types[0]}>",
                "IRocketPackObjectFormatter<>" => $"Omnius.Core.RocketPack.IRocketPackObjectFormatter<{types[0]}>",
                "IRocketPackObject<>" => $"Omnius.Core.RocketPack.IRocketPackObject<{types[0]}>",
                "Lazy<>" => $"System.Lazy<{types[0]}>",
                "Memory<>" => $"System.Memory<{types[0]}>",
                "MemoryOwner<>" => $"Omnius.Core.MemoryOwner<{types[0]}>",
                "ObjectHelper" => "Omnius.Core.Helpers.ObjectHelper",
                "ReadOnlyDictionarySlim<,>" => $"Omnius.Core.Collections.ReadOnlyDictionarySlim<{types[0]}, {types[1]}>",
                "ReadOnlyListSlim<>" => $"Omnius.Core.Collections.ReadOnlyListSlim<{types[0]}>",
                "ReadOnlyMemory<>" => $"System.ReadOnlyMemory<{types[0]}>",
                "ReadOnlySequence<>" => $"System.Buffers.ReadOnlySequence<{types[0]}>",
                "ReadOnlySpan<>" => $"System.ReadOnlySpan<{types[0]}>",
                "RocketPackObjectReader" => "Omnius.Core.RocketPack.RocketPackObjectReader",
                "IRocketPackRpc" => $"Omnius.Core.Remoting.IRocketPackRpc",
                "RocketPackRpc" => $"Omnius.Core.Remoting.RocketPackRpc",
                "RocketPackRpcFunction" => $"Omnius.Core.Remoting.RocketPackRpcFunction",
                "RocketPackRpcMessenger" => $"Omnius.Core.Remoting.RocketPackRpcMessenger",
                "RocketPackObjectWriter" => "Omnius.Core.RocketPack.RocketPackObjectWriter",
                "Span<>" => $"System.Span<{types[0]}>",
                "Task" => $"System.Threading.Tasks.Task",
                "Task<>" => $"System.Threading.Tasks.Task<{types[0]}>",
                "Timestamp" => "Omnius.Core.RocketPack.Timestamp",
                "ValueTask" => $"System.Threading.Tasks.ValueTask",
                "ValueTask<>" => $"System.Threading.Tasks.ValueTask<{types[0]}>",
                _ => throw new KeyNotFoundException(name)
            };

            return "global::" + result;
        }
    }
}
