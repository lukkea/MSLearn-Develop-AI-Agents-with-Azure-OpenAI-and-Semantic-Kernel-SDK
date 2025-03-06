using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

string filePath = Path.GetFullPath("../../../../../appsettings.json");
IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile(filePath)
    .Build();

// Set your values in appsettings.json
string modelId = config["modelId"]!;
string endpoint = config["endpoint"]!;
string apiKey = config["apiKey"]!;

// Create a kernel with Azure OpenAI chat completion
IKernelBuilder builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);

Kernel kernel = builder.Build();
IChatCompletionService _ = kernel.GetRequiredService<IChatCompletionService>();

//
// Add your code
//