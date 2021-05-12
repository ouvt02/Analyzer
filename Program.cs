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

int length_class_name = 3;
int length_field_name = 2;
int length_property_name = 5;
int length_function_name = 1;
int length_local_var_name = 1;


void Visit(SyntaxNode node)
{
    String class_name = null;
    String property_name = null;
    String method_name = null;
    String field_name = null;
    String local_var_name = null;

    if (node is VariableDeclaratorSyntax)
    {
        if (node.Parent is VariableDeclarationSyntax)
        {
            if (node.Parent.Parent is FieldDeclarationSyntax)
            {
                VariableDeclaratorSyntax nameSyntax = (VariableDeclaratorSyntax)node;
                field_name = nameSyntax.Identifier.Text;
            }

            else if (node.Parent.Parent is LocalDeclarationStatementSyntax)
            {
                VariableDeclaratorSyntax nameSyntax = (VariableDeclaratorSyntax)node;
                local_var_name = nameSyntax.Identifier.Text;
            }

        }
    }

    else if (node is ClassDeclarationSyntax)
    {
        ClassDeclarationSyntax nameSyntax = (ClassDeclarationSyntax)node;
        class_name = nameSyntax.Identifier.Text;
    }

    else if (node is PropertyDeclarationSyntax)
    {
        PropertyDeclarationSyntax nameSyntax = (PropertyDeclarationSyntax)node;
        property_name = nameSyntax.Identifier.Text;
    }

    else if (node is MethodDeclarationSyntax)
    {
        MethodDeclarationSyntax nameSyntax = (MethodDeclarationSyntax)node;
        method_name = nameSyntax.Identifier.Text;
    }


    if (class_name != null && class_name.Length > length_class_name)
        Console.WriteLine($"Length of name of class \"{class_name}\" more than acceptable");

    if (property_name != null && property_name.Length > length_property_name)
        Console.WriteLine($"Length of name of property \"{property_name}\" more than acceptable");

    if (method_name != null && method_name.Length > length_function_name)
        Console.WriteLine($"Length of name of function \"{method_name}\" more than acceptable");

    if (field_name != null && field_name.Length > length_field_name)
        Console.WriteLine($"Length of name of field \"{field_name}\" more than acceptable");

    if (local_var_name != null && local_var_name.Length > length_local_var_name)
        Console.WriteLine($"Length of name of local variable \"{local_var_name}\" more than acceptable");

    IEnumerable < SyntaxNode > nodes = node.ChildNodes();

    foreach (SyntaxNode child_node in nodes)
        Visit(child_node);
}

VisualStudioInstance[] test = MSBuildLocator.QueryVisualStudioInstances().ToArray();
VisualStudioInstance inst = test[0];
MSBuildLocator.RegisterInstance(inst);

MSBuildWorkspace workspace = MSBuildWorkspace.Create();
Project project = workspace.OpenProjectAsync(@"C:\Users\Ульяна\source\repos\ForTesting\ForTesting.csproj").Result;
IEnumerable<Document> documents = project.Documents;

Document[] Documents = documents.ToArray();

foreach (Document document in documents)
    Visit(document.GetSyntaxTreeAsync().Result.GetRoot());
