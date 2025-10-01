# TooliRent
TooliRent är ett uthyrningssystem för verktyg byggt med .NET 8 och ASP.NET Core Web Api. Det här backend APIet är designat för att hantera uthyrning av verktyg men även för att ge stöd till användarregistrering,inloggning,kategorisering av verktyg och hantering av tillgänglighet.
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Arkitektur

Projektet är organiserat efter N-tier arkitektur med tydlig seperation:

    Presentation(Controllers) - Api controllers som hanterar HTTP-förfrågningar.
      -AuthsController
      -ToolsController
      -BookingsController
      -CategoriesController
    
    Application(Services) - Hanterar affärslogik och validering
      -DTOs( för ex. Booking,Tool,Category
      -Services(ex. BookingService, ToolService, AuthService)
      -Validators(FluentValidation)
      -Mapping(AutoMapper)
      
    Domain(Entities, Interfaces) - hanterar entities och interfaces.
    
    Infrastructure(Repositories, DbContext) - Hanterarkomminikationen med databasen genom EF Core.
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Funktionalitet

1. Autentisering & Roller

	-Registrera och logga in användare
		
	-JWT-token används för autentisering
		
	-Token refresh

2. Admin

    . Crud-operationer på verktyg och kategory

    - Hantera användare(aktivera/inaktivera)

4. Verktyg

    -Lista alla verktyg
    
    -Filtrera genom status,tillgänglighet och kategori
    
    -Hämta detaljer om ett verktyg

5. Kategorier

    -Lista alla kategorier
   
    -Se verktyg per kategori

    -CRUD-operationer (endast Admin)

6. Bokningar
   
    -Boka ett eller flera verktyg under en period (start-slut)
  
    -Skapa och avboka bokningar

    -Se egna bokningar

    -Statistik över bokningar

    -markera som hämtad/returnerad
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Kör lokalt
vad som krävs
- .NET 8
- SQL Server(connectionstring)

Följ dessa steg:

1. Klona projektet
        
        git clone https://github.com/shobah24/TooliRent.git
        
        cd TooliRent

2. Skapa databas
   
        Ändra appsettings.json till rätt connectionstring och kör:

        dotnet restore
        dotnet ef database update


4. Starta API

       ex. via dotnet run eller CTRL+F5
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Inloggning:

Member:
  
    {
      "email": "member@toolirent.local", 
      "password": "Member123!"
    }

Admin:

    {
      "email": "admin@toolirent.local",
      "password": "Admin123!"
    }

      
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
# Api Endpoints
Autentisering:

      Metod   Endpoint                       Roller   Beskrivning                                      
       -----   ----------------------------   ------   ------------------------------------------------ 
       POST    `/api/auths/register`          Båda     Registrera en ny användare (default = Member).   
       POST    `/api/auths/registerAdmin`     Admin    Registrera en ny Admin-användare.                
       POST    `/api/auths/login`             Båda     Logga in och få JWT-token + refresh token.       
       POST    `/api/auths/refresh`           Båda     Förnya access-token med refresh token.           
       POST    `/api/auths/revoke`            Båda     Revokera (ogiltigförklara) en refresh token.     
       PATCH   `/api/auths/{id}/deactivate`   Admin    Avaktivera en användare (sätter `LockoutEnd`).   
       PATCH   `/api/auths/{id}/activate`     Admin    Återaktivera en användare (`LockoutEnd = null`). 

Verktyg:

      Metod    Endpoint                                            Roller          Beskrivning                                    
      ------   -------------------------------------------------   ------   ---------------------------------------------- 
      GET      `/api/tools`                                        Båda     Lista alla verktyg.                            
      GET      `/api/tools/{id}`                                   Båda     Hämta detaljer om ett specifikt verktyg.       
      GET      `/api/tools/available`                              Båda     Hämta alla lediga verktyg.                     
      GET      `/api/tools/category/{categoryId}`                  Båda     Lista verktyg i en viss kategori.              
      GET      `/api/tools/filter?status=Available&categoryId=1`   Båda     Filtrera verktyg på status och/eller kategori. 
      POST     `/api/tools`                                        Admin    Skapa nytt verktyg.                            
      PUT      `/api/tools/{id}`                                   Admin    Uppdatera ett verktyg.                         
      DELETE   `/api/tools/{id}`                                   Admin    Radera ett verktyg.                            


Kategori:

        Metod    Endpoint                     Roller   Beskrivning                      
        ------   --------------------------   ------   --------------------------------- 
        GET      `/api/category`              Båda     Lista alla kategorier.             
        GET      `/api/category/{id}`         Båda     Hämta en kategori med ID.         
        GET      `/api/category/{id}/tools`   Båda     Lista verktyg i en viss kategori.  
        POST     `/api/category`              Admin    Skapa en ny kategori.             
        PUT      `/api/category/{id}`         Admin    Uppdatera en kategori.            
        DELETE   `/api/category/{id}`         Admin    Ta bort en kategori.              


Bokning: 

        Metod    Endpoint                             Roller          Beskrivning                                   
        ------   ----------------------------------   -------------   --------------------------------------------- 
        GET      `/api/bookings`                      Admin           Lista alla bokningar.                         
        GET      `/api/bookings/{id}`                 Admin           Hämta en bokning med ID.                      
        GET      `/api/bookings/tool/{id}`            Admin           Lista bokningar för ett visst verktyg.        
        GET      `/api/bookings/user/{userId}`        Båda            Lista bokningar för en specifik användare.    
        GET      `/api/bookings/overall-stats`        Admin           Statistik över bokningar.                     
        POST     `/api/bookings`                      Båda            Skapa en ny bokning (kan boka flera verktyg). 
        PUT      `/api/bookings/{id}`                 Båda            Uppdatera en bokning (ändra datum/verktyg).   
        PATCH    `/api/bookings/{id}/pickup-status`   Båda            Markera bokning som hämtad.                   
        PATCH    `/api/bookings/{id}/return`          Båda            Markera bokning som returnerad.               
        DELETE   `/api/bookings/{id}`                 Admin           Ta bort en bokning.                           





