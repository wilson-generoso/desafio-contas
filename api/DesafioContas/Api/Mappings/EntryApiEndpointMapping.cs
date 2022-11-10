using Desafio.Contas.Api.Middlewares;
using Desafio.Contas.Application.Command.AddEntry;
using Desafio.Contas.Application.Query.SearchEntries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Desafio.Contas.Api.Mappings
{
    public static class EntryApiEndpointMapping
    {
        public static void MapEntryEndpoints(this WebApplication application)
        {
            application.MapGet("/v1/entry/{accountId}", async (
                IMediator mediator,
                [FromRoute] Guid accountId, 
                [DefaultValue(7)] int? lastDaysCount, 
                string? initialDate, 
                string? endDate) =>
            {
                var response = await mediator.Send(new SearchEntriesRequest
                {
                    AccountId = accountId,
                    LastDaysCount = lastDaysCount,
                    InitialDate = initialDate,
                    EndDate = endDate
                });

                if (response.Days?.Any() ?? false)
                    return Results.Ok(response);
                else
                    return Results.NoContent();
            })
            .WithName("SearchEntries")
            .Produces<SearchEntriesResponse>()
            .Produces(204)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500);

            application.MapPost("/v1/entry", async (HttpContext context, AddEntryRequest request) =>
            {
                var mediator = context.RequestServices.GetRequiredService<IMediator>();

                await mediator.Send(request);

                return Results.Ok();
            })
            .WithName("AddEntry")
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500);
        }
    }
}
