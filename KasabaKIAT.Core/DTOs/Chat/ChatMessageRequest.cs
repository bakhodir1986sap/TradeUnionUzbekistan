namespace KasabaKIAT.Core.DTOs.Chat;

public record ChatMessageRequest(string Message, List<ChatTurnDto>? History);

public record ChatTurnDto(string Role, string Content);
