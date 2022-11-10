using Desafio.Contas.Api.Middlewares;
using Desafio.Contas.Application.Command.AddAccount;
using Desafio.Contas.Application.Command.UpdateAccount;
using Desafio.Contas.Application.Query.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.Contas.Api.Mappings
{
    public static class AccountApiEndpointMapping
    {
        public static void MapAccountEndpoints(this WebApplication application)
        {
            application.MapGet("/v1/account/{id}", async (IMediator mediator, [FromRoute] Guid? id) => 
            {
                var response = await mediator.Send(new GetAccountsRequest { Id = id });

                if (response.Accounts?.Any() ?? false)
                    return Results.Ok(response);
                else
                    return Results.NoContent();
            })
            .WithName("GetAccoutById")
            .Produces<GetAccountsResponse>(200)
            .Produces(204)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500);

            application.MapGet("/v1/account", async (IMediator mediator) =>
            {
                var response = await mediator.Send(new GetAccountsRequest());

                if (response.Accounts?.Any() ?? false)
                    return Results.Ok(response);
                else
                    return Results.NoContent();
            })
            .WithName("GetAccounts")
            .Produces<GetAccountsResponse>(200)
            .Produces(204)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500);

            application.MapPost("/v1/account", async (HttpContext context, AddAccountRequest request) =>
            {
                var mediator = context.RequestServices.GetRequiredService<IMediator>();
                var response = await mediator.Send(request);
                return Results.Created($"/account/{response.Id}", response);
            })
            .WithName("PostAccount")
            .Produces<AddAccountResponse>(201)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500);

            application.MapPut("/v1/account", async (HttpContext context, UpdateAccountRequest request) =>
            {
                var mediator = context.RequestServices.GetRequiredService<IMediator>();

                await mediator.Send(request);

                return Results.NoContent();
            })
            .WithName("PutAccount")
            .Produces(204)
            .Produces<ErrorResponse>(400)
            .Produces<ErrorResponse>(500);
        }
    }
}
