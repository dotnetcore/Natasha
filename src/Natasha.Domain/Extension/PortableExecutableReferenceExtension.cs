using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace Natasha.Domain.Extension
{
    internal class PortableExecutableReferenceExtension
    {
        internal static unsafe PortableExecutableReference? CreateFromAssemblyOrNullInternal(
            this Assembly assembly,
            MetadataReferenceProperties properties,
            DocumentationProvider? documentation = null,
            bool throwOnFailure = false)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (assembly.IsDynamic)
            {
                if (throwOnFailure)
                {
                    throw new NotSupportedException("不能为动态程程序集创建PE引用!");
                }
                else
                {
                    return null;
                }
            }

            if (properties.Kind != MetadataImageKind.Assembly)
            {
                throw new ArgumentException("不能为动态程程序集创建模块引用!");
            }

            AssemblyMetadata metadata;
            var location = assembly.IsDynamic ? null : assembly.Location;

#if NETCOREAPP
            if (assembly.TryGetRawMetadata(out var blob, out var length))
            {
                metadata = AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length));
            }
            else
#endif
            {
                if (string.IsNullOrEmpty(location))
                {
                    if (throwOnFailure)
                    {
                        throw new NotSupportedException("不能为Location属性为空的程序集创建PE引用!");
                    }
                    else
                    {
                        return null;
                    }
                }

                Stream peStream = StandardFileSystem.Instance.OpenFileWithNormalizedException(location, FileMode.Open, FileAccess.Read, FileShare.Read);

                // The file is locked by the CLR assembly loader, so we can create a lazily read metadata, 
                // which might also lock the file until the reference is GC'd.
                metadata = AssemblyMetadata.CreateFromStream(peStream);
            }
            PortableExecutableReference.c
            return new MetadataImageReference(metadata, properties, documentation, location, display: null);
        }
    }
    /// <summary>
    /// Abstraction over the file system that is useful for test hooks
    /// </summary>
    internal interface ICommonCompilerFileSystem
    {
        bool FileExists(string filePath);

        Stream OpenFile(string filePath, FileMode mode, FileAccess access, FileShare share);

        Stream OpenFileEx(string filePath, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, out string normalizedFilePath);
    }

    internal static class CommonCompilerFileSystemExtensions
    {
        /// <summary>
        /// Open a file and ensure common exception types are wrapped to <see cref="IOException"/>.
        /// </summary>
        internal static Stream OpenFileWithNormalizedException(this ICommonCompilerFileSystem fileSystem, string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            try
            {
                return fileSystem.OpenFile(filePath, fileMode, fileAccess, fileShare);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (DirectoryNotFoundException e)
            {
                throw new FileNotFoundException(e.Message, filePath, e);
            }
            catch (IOException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new IOException(e.Message, e);
            }
        }
    }
    internal sealed class StandardFileSystem : ICommonCompilerFileSystem
    {
        public static StandardFileSystem Instance { get; } = new StandardFileSystem();

        private StandardFileSystem()
        {
        }

        public bool FileExists(string filePath) => File.Exists(filePath);

        public Stream OpenFile(string filePath, FileMode mode, FileAccess access, FileShare share)
            => new FileStream(filePath, mode, access, share);

        public Stream OpenFileEx(string filePath, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options, out string normalizedFilePath)
        {
            var fileStream = new FileStream(filePath, mode, access, share, bufferSize, options);
            normalizedFilePath = fileStream.Name;
            return fileStream;
        }
    }
}
