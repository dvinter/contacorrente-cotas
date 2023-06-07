FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

WORKDIR /app

COPY . .

RUN dotnet restore

RUN dotnet publish -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

# Change timezone to local time
ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

WORKDIR /app

COPY --from=build /app/published-app /app/

EXPOSE 80

ENTRYPOINT ["dotnet", "/app/AlesCotas.Presentation.dll"]