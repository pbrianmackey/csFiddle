using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using System.Diagnostics;
using Roslyn.Services; 

namespace csFiddleTests
{
    
    [TestClass]
    public class RoslynTests
    {
        private string sourceText = @"using System;
 
                namespace HelloWorld
                {
                    class Program
                    {
                        static void Main(string[] args)
                        {
                            Console.WriteLine(""Hello, World!"");
                        }
                    }
                }";

        [TestMethod]
        public void CreateIsolatedWorkspace()
        {
            //IWorkspace workspace = Workspace.LoadSolution(@"HelloWorld.sln");
            //ISolution solution = workspace.CurrentSolution;

            //OR if you do not expect to make changes to the workspace
            //ISolution solution = Solution.Load(@"HelloWorld.sln");

            //Or whatever this is
            ProjectId pid1, pid2;
            DocumentId did1, did2;
            ISolution solution =
                Solution.Create(SolutionId.CreateNewId("Solution"))
                        .AddCSharpProject("Project1.dll", "Project1", out pid1)
                        .AddDocument(pid1, "A.cs", "public class A { }", out did1);
            //.AddCSharpProject("Project2.dll", "Project2", out pid2)
            //.AddDocument(pid2, "B.cs", "class B : A { }", out did2)
            //.AddProjectReference(pid2, pid1); 


        }
        [TestMethod]
        public void CreateCompilation()
        {
            SyntaxTree tree = SyntaxTree.ParseText(sourceText);
            MetadataReference mscorlib = MetadataReference.CreateAssemblyReference("mscorlib");

            //Compilation compilation = Compilation.Create(
            //            outputName: "HelloWorld",
            //            syntaxTrees: new[] { tree },
            //            references: new[] { mscorlib });

            Compilation compilation = Compilation.Create("HelloWorld")
                            .AddReferences(mscorlib)
                            .AddSyntaxTrees(tree);

            //obtain symbols
            NamespaceSymbol globalNamespace = compilation.GlobalNamespace;

            foreach (Symbol member in globalNamespace.GetMembers())
            {
                Trace.Write(member);
            }

        }

        [TestMethod]
        public void ParseText()
        {
            SyntaxTree tree = SyntaxTree.ParseText(sourceText);
        }

        [TestMethod]
        public void ParseExpression()
        {
            ExpressionSyntax myExpression = Syntax.ParseExpression("1+1");
        }

        [TestMethod]
        public void CreateANode()
        {
            //Create a namespace
            QualifiedNameSyntax namespaceName = Syntax.QualifiedName(left: Syntax.IdentifierName("Roslyn"),right: Syntax.IdentifierName("Compilers"));

            NamespaceDeclarationSyntax myNamespace = Syntax.NamespaceDeclaration(namespaceName);

            SyntaxToken namespaceToken = Syntax.Token(SyntaxKind.NamespaceKeyword,Syntax.Space);
            //namespaceToken.Value = myNamespace;

            NamespaceDeclarationSyntax formattedNamespace = myNamespace.NormalizeWhitespace(); 
        }
    }
}
