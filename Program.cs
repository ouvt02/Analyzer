using System;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;



namespace classes
{
    class Analyzer : CSharpSyntaxWalker
    {
        int length_class_name = 3;
        int length_field_name = 2;
        int length_property_name = 5;
        int length_function_name = 1;
        int length_local_var_name = 3;

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            String class_name = node.Identifier.ValueText;

            if (class_name != null && class_name.Length > length_class_name)
                Console.WriteLine($"Length of name of class \"{class_name}\" more than acceptable");

            base.VisitClassDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            String method_name = node.Identifier.ValueText;

            if (method_name != null && method_name.Length > length_function_name)
                Console.WriteLine($"Length of name of function \"{method_name}\" more than acceptable");

            base.VisitMethodDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            String property_name = node.Identifier.ValueText;

                if (property_name != null && property_name.Length > length_property_name)
        Console.WriteLine($"Length of name of property \"{property_name}\" more than acceptable");

            base.VisitPropertyDeclaration(node);
        }

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            if (node.Parent is VariableDeclarationSyntax)
            {
                if (node.Parent.Parent is FieldDeclarationSyntax)
                {
                    String field_name = node.Identifier.ValueText;

                    if (field_name != null && field_name.Length > length_field_name)
                        Console.WriteLine($"Length of name of field \"{field_name}\" more than acceptable");
                }

                else
                {
                    String local_var_name = node.Identifier.ValueText;

                    if (local_var_name != null && local_var_name.Length > length_local_var_name)
                        Console.WriteLine($"Length of name of local variable \"{local_var_name}\" more than acceptable");
                }
            }

            base.VisitVariableDeclarator(node);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            VisualStudioInstance[] test = MSBuildLocator.QueryVisualStudioInstances().ToArray();
            VisualStudioInstance inst = test[0];
            MSBuildLocator.RegisterInstance(inst);

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Project project = workspace.OpenProjectAsync(@"C:\Users\Ульяна\source\repos\ForTesting\ForTesting.csproj").Result;
            IEnumerable<Document> documents = project.Documents;

            Document[] Documents = documents.ToArray();

            Analyzer analyzer = new Analyzer();

            foreach (Document document in documents)
                analyzer.Visit(document.GetSyntaxTreeAsync().Result.GetRoot());
        }
    }
}

