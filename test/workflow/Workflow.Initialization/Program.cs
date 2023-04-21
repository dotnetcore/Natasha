using Github.NET.Sdk;
using Github.NET.Sdk.Model;
using Github.NMSAcion.NET.Sdk.Model;
using Microsoft.VisualBasic;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Workflow.Initialization.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Workflow.Template.Initialization
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ConfigRunner.UpdateIssueTemplateConfig();
            var newSolutionInfo = SolutionRecorder.GetNewestSolution();
            SolutionRecorder.Save(newSolutionInfo);
            ConfigRunner.UpdateCodecovYML(newSolutionInfo);
            ConfigRunner.UpdateDependencyYML(newSolutionInfo);
            ConfigRunner.UpdateUnitTestYML(newSolutionInfo);
            ConfigRunner.UpdateIssueTempalate(newSolutionInfo);
            //ConfigRunner.GenPackageId(newSolutionInfo);
            ConfigRunner.GenUsings(newSolutionInfo);

        }

    }

}