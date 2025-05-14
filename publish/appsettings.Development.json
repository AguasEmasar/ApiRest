{
  "Jwt": {
    "ValidIssuer": "http://localhost:5173",
    "ValidAudience": "https://localhost:5088",
    "Secret": "QDNm%dDQ!pH5VDVGWEM51Y4LYMRdh6sWUEML9dwL!P2vn3hhWMWkgDb",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiry": 1440
  },
  "Firebase": {
    "CredentialPath": "firebase-config.json"
  },
  "FrontendURL": "http://localhost:5173",
  "ConnectionStrings": {
    //"DefaultConnection": "Server=localhost;Database=databaseaguas;User=root;Password=frt123;"
    "DefaultConnection": "Server=192.168.10.138,3306;Database=dbaqs;User=angel;Password=yx4SBK"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "aguassantarosa6@gmail.com",
    "SmtpPassword": "tpcixaxfxwvscobc"
  },
  "ApiConfig": {
    "BaseUrl": "http://192.168.10.254:8180/simafiws/consulta/publicos",
    "BaseUrlComment": "http://192.168.10.254:8180/simafiws2025/consulta/comentario",
    "BaseUrlHistory": "http://192.168.10.254:8180/simafiws2025/historial/publicos",
    "AuthId": "011",
    "AuthKey": "924703863DFB32C4AF1436B17B63ED1A"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console" // ver los logs en la consola
      },
      {
        "Name": "MySQL",
        "Args": {
          "connectionString": "Server=192.168.10.138,3306;Database=dbaqs;User=angel;Password=yx4SBK",
          //"connectionString": "Server=localhost;Database=Aquasystem;User=root;Password=frt123;",
          "tableName": "Logs",
          "restrictedToMinimumLevel": "Information"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://*:5088"
      }
    }
  }
}
