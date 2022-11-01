
using HotChocolate;
using HotChocoServer;
using ZeroQL.Bootstrap;
using Path = System.IO.Path;

if (args.Length != 1)
{
    Console.WriteLine("You must supply the output folder for the generated client to be created.");
    return;
}

var outputFolder = args[0];

const string clientName = "GeneratedClient";

var executor = await ConfigureServices.GetGraphQLExecutorAsync();
var documentNode = SchemaSerializer.SerializeSchema(executor.Schema);
var graphqlSchema = documentNode.ToString();

var csharpClient = GraphQLGenerator.ToCSharp(graphqlSchema, "HotChocoServer.Generated", clientName);

if (!Directory.Exists(outputFolder))
{
    Directory.CreateDirectory(outputFolder);
}

var output = Path.Combine(outputFolder, $"{clientName}.cs");
File.WriteAllText(output, csharpClient);


Console.WriteLine($"Generated {output} from GraphQL schema.");
