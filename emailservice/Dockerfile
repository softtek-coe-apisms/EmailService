FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src

RUN ls
COPY ["emailservice.csproj", "emailservice/"]
RUN dotnet restore "emailservice/emailservice.csproj"
COPY . "/src/emailservice"
WORKDIR "/src/emailservice"
RUN ls
RUN dotnet build "emailservice.csproj" -c Release -o /app
COPY "/wwwroot/Assets" "/app/Assets"

FROM build AS publish
RUN dotnet publish "emailservice.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN ls
COPY --from=build /app/Assets /app/Assets
RUN ls
ENTRYPOINT ["dotnet", "emailservice.dll"]
