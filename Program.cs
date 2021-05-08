using System;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

int length_class_name = 3;
int length_field_name = 2;
int length_property_name = 5;
int length_function_name = 1;
int length_local_var_name = 3;


SyntaxNode FindVariableDeclarator(SyntaxNode node)
{
    if (node.Kind().ToString() == "VariableDeclarator")
        return node;

    foreach (SyntaxNode child_node in node.ChildNodes())
    {
        SyntaxNode new_node = FindVariableDeclarator(child_node);
        if (new_node != null)
            return new_node;
    }

    return null;
}


String GetNameOfToken(String declaration, SyntaxNode node)
{
    if (declaration == "FieldDeclaration" || declaration == "LocalDeclarationStatement")
        node = FindVariableDeclarator(node);


    IEnumerable<SyntaxToken> tokens = node.ChildTokens();
    foreach (SyntaxToken token in tokens)
    {
        if (token.Kind().ToString() == "IdentifierToken")
            return token.Text;
    }
    
    return null;
}

String Filter(SyntaxNode node, String declaration)
{
    
    if (node.Kind().ToString() == declaration)
        return GetNameOfToken(declaration, node);

    return null;

}

void Visit(SyntaxNode node)
{
    String class_name = Filter(node, "ClassDeclaration");
    if (class_name != null && class_name.Length > length_class_name)
            Console.WriteLine($"Length of name of class \"{class_name}\" more than acceptable");

    String property_name = Filter(node, "PropertyDeclaration");
    if (property_name != null && property_name.Length > length_property_name)
        Console.WriteLine($"Length of name of property \"{property_name}\" more than acceptable");

    String method_name = Filter(node, "MethodDeclaration");
    if (method_name != null && method_name.Length > length_function_name)
        Console.WriteLine($"Length of name of function \"{method_name}\" more than acceptable");

    String field_name = Filter(node, "FieldDeclaration");
    if (field_name != null && field_name.Length > length_field_name)
        Console.WriteLine($"Length of name of field \"{field_name}\" more than acceptable");

    String local_var_name = Filter(node, "LocalDeclarationStatement");
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
