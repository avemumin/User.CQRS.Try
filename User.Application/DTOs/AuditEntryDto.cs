namespace User.Application.DTOs;

public record AuditEntryDto(
 string UserId,
 string Action,
 string EntityType,
 string EntityId,
 string TraceId,
 string? Details
);
