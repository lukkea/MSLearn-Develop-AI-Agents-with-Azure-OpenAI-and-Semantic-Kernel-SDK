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

// Build the kernel
Kernel kernel = builder.Build();

// Get chat completion service.
IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Create a chat history object
ChatHistory chatHistory = [];

void AddMessage(string msg) {
    Console.WriteLine(msg);
    chatHistory.AddAssistantMessage(msg);
}

void GetInput() {
    string input = Console.ReadLine()!;
    chatHistory.AddUserMessage(input);
}

async Task GetReply() {
    ChatMessageContent reply = await chatCompletionService.GetChatMessageContentAsync(
        chatHistory,
        kernel: kernel
    );
    Console.WriteLine(reply.ToString());
    chatHistory.AddAssistantMessage(reply.ToString());
}

// Prompt the LLM
chatHistory.AddSystemMessage("You are a helpful travel assistant.");
chatHistory.AddSystemMessage("Recommend a destination to the traveler based on their background and preferences.");

// Get information about the user's plans
AddMessage("Tell me about your travel plans.");
GetInput();
await GetReply();

// Offer recommendations
AddMessage("Would you like some activity recommendations?");
GetInput();
await GetReply();

// Offer language tips
AddMessage("Would you like some helpful phrases in the local language?");
GetInput();
await GetReply();

Console.WriteLine("Chat Ended.\n");
Console.WriteLine("Chat History:");

for (int i = 0; i < chatHistory.Count; i++) {
    Console.WriteLine($"{chatHistory[i].Role}: {chatHistory[i]}");
}
Console.ReadLine();
