using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Microsoft.CodeAnalysis.Emit
{
    public static class CompilerExtension
    {
        public static EmitResult UnixEmit(
            this Compilation compilation,
            string outputPath,
            string pdbPath = null,
            string xmlDocPath = null,
            string win32ResourcesPath = null,
            IEnumerable<ResourceDescription> manifestResources = null,
            CancellationToken cancellationToken = default)
        {
            if (compilation == null)
            {
                throw new ArgumentNullException(nameof(compilation));
            }

            using (var outputStream = FileUtilities.CreateFileStreamChecked(File.Create, outputPath, nameof(outputPath)))
            using (var pdbStream = (pdbPath == null ? null : FileUtilities.CreateFileStreamChecked(File.Create, pdbPath, nameof(pdbPath))))
            using (var xmlDocStream = (xmlDocPath == null ? null : FileUtilities.CreateFileStreamChecked(File.Create, xmlDocPath, nameof(xmlDocPath))))
            using (var win32ResourcesStream = (win32ResourcesPath == null ? null : FileUtilities.CreateFileStreamChecked(File.OpenRead, win32ResourcesPath, nameof(win32ResourcesPath))))
            {
                return compilation.Emit(
                    outputStream,
                    pdbStream: pdbStream,
                    xmlDocumentationStream: xmlDocStream,
                    win32Resources: win32ResourcesStream,
                    manifestResources: manifestResources,
                    options: new EmitOptions(pdbFilePath: pdbPath,debugInformationFormat: DebugInformationFormat.PortablePdb),
                    cancellationToken: cancellationToken);
            }
        }
    }

    public static class FileUtilities
    {
        internal static Stream CreateFileStreamChecked(Func<string, Stream> factory, string path, string paramName = null)
        {
            try
            {
                return factory(path);
            }
            catch (ArgumentNullException)
            {
                if (paramName == null)
                {
                    throw;
                }
                else
                {
                    throw new ArgumentNullException(paramName);
                }
            }
            catch (ArgumentException e)
            {
                if (paramName == null)
                {
                    throw;
                }
                else
                {
                    throw new ArgumentException(e.Message, paramName);
                }
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
}
