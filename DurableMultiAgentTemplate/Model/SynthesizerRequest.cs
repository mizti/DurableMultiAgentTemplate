using DurableMultiAgentTemplate.Shared.Model;

namespace DurableMultiAgentTemplate.Model;

public class SynthesizerRequest
{
    public List<string> AgentCallResult { get; set; } = new List<string>();
    public AgentRequestDto AgentRequest { get; set; } = new AgentRequestDto();
    public List<string> CalledAgentNames { get; set; } = new List<string>();
}