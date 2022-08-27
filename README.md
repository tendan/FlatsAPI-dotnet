# Flats of Blocks RESTful API

##### RESTful API that imitates the e-commerce platform for blocks of flats and flats managing

This API handles every CRUD requests with some bit of authentication and user authorization.

Available features for now:
- Adding new accounts validation
- Account authentication and authorization
- Updating account credentials
- Getting account information
- Deleting accounts
- Specified permissions for authorizing users
- Managing blocks of flats, such as creating, getting, deleting them.
- Managing flats, such as creating, getting, deleting, just like with blocks of flats.
- Applying rents for accounts with hosted services, which work independently from other different synchonous services.
- Invoices generating based on account rents

Dependencies used in this project:
- AutoMapper.Extensions.Microsoft.DependencyInjection
- FluentValidation
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- itext7
