using System;
using Azure.AI.OpenAI;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;

namespace DurableMultiAgentTemplate
{
    public class SynthesizerActivity(AzureOpenAIClient openAIClient, AppConfiguration configuration)
    {
        private readonly AzureOpenAIClient _openAIClient = openAIClient;
        private readonly AppConfiguration _configuration = configuration;

        [Function(AgentActivityName.SynthesizerActivity)]
        public async Task<string> Run([ActivityTrigger] SynthesizerRequest req, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SynthesizerActivity");
            logger.LogInformation("Run SynthesizerActivity");
            var systemMessageTemplate = SynthesizerPrompt.SystemPrompt;
            var systemMessage = $"{systemMessageTemplate}¥n{string.Join("¥n", req.AgentCallResult)}";

            ChatMessage[] messages = {new SystemChatMessage(systemMessage)};
            var allMessages = messages.Concat(req.AgentReques.Messages.ConvertToChatMessageArray()).ToArray();
            
            var chatClient = _openAIClient.GetChatClient(_configuration.OpenAIDeploy);
            var chatResult = await chatClient.CompleteChatAsync(
                allMessages
            );

            return chatResult.Value.Content.First().Text;
        }
    }
}
