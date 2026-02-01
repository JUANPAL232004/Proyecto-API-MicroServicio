**Prueba Técnica: API de Gestión de Productos** 
Este proyecto consiste en un microservicio desarrollado en .NET 8 que gestiona un inventario de productos. Incluye una API RESTful, seguridad con JWT, y un frontend integrado.

_**--Tecnologías y Requisitos**_
Backend: .NET 8 (Web API)

Base de Datos: SQL Server (Sin ORM, usando ADO.NET)

Seguridad: Autenticación por Token (JWT)

Frontend: Vanilla JavaScript, HTML5 y CSS (Bootstrap 5)

Contenedores: Docker (para la base de datos)

_**-- Configuración del Proyecto**_

**1.** Base de Datos (SQL Server)
He adjuntado un archivo llamado script.sql. Para restaurar la base de datos:

Ejecute el script en su instancia de SQL Server.

El script creará las tablas Producto, Estado y el procedimiento almacenado sp_CrearProducto.

(Opcional) Si usa Docker, puede levantar la imagen oficial:

Bash

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=SqlServer@123" -p 1433:1433 --name sql_server -d mcr.microsoft.com/mssql/server:2022-latest


**2.** Backend (API)
Vaya al archivo appsettings.json y actualice la cadena de conexión en el objeto cadenaConexionSqlServer con sus credenciales del servidor de base de datos.

En la terminal, ejecute:

Bash

(Esto permite que el proyecto pueda correr y usar sin problemas)
dotnet restore
dotnet run



**3.** Frontend
El cliente web está integrado en la carpeta wwwroot. Una vez que el backend esté corriendo, simplemente abra en su navegador: http://localhost:5000/index.html (o el puerto que le asigne .NET).

Documentación de la API (Swagger)
Para facilitar la revisión técnica, se ha implementado Swagger. Esta herramienta permite ver y probar todos los endpoints de forma interactiva.

URL de Documentación: http://localhost:5068/swagger


_**Cómo probar:**_
**1.** Use el endpoint de Login para obtener un Token. 2. Haga clic en el botón "Authorize" arriba a la derecha. 3. Pegue el token y podrá ejecutar las peticiones POST, PUT y DELETE.

-- Puntos Clave Implementados
ADO.NET Nativo: No se utilizaron ORMs (Entity Framework o Dapper) para cumplir con el requerimiento de SQL directo.

Relaciones: Consultas con INNER JOIN para obtener nombres de estados.

Procedimientos Almacenados: Implementados para la creación de registros.

Validaciones: Control de errores y códigos de respuesta HTTP adecuados, rangos, formatos de cada dato.

_**Funcionalidad**_
**1.** Se debe ingresar por la direccion http://localhost:5068.
**2.** Se debe registrar un usuario para poder acceder a las funcionalidades.
**3.** Posteriormente loguearse con los datos previamente registrados.
**4** Se puede realizar creación, actualización y eliminación de datos.
