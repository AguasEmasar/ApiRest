# Imagen base de .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia los archivos publicados
COPY ./publish .

# Expone el puerto que usa la API 
EXPOSE 4000

# Comando de inicio
ENTRYPOINT ["dotnet", "LOGIN.dll"]
