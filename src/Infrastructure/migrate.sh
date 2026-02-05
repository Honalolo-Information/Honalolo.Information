#!/bin/bash
dotnet ef migrations add AddReportsTable --project ./Honalolo.Information.Infrastructure --startup-project ./Honalolo.Information.WebApi

dotnet ef database update --project ./Honalolo.Information.Infrastructure --startup-project ./Honalolo.Information.WebApi
