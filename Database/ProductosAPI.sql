
--CREACION BASE DE DATOS
CREATE DATABASE DB_GestionProductos;
GO

--USO DE SCRIPT SQL EN BASE DE DATOS
USE DB_GestionProductos;
GO

--Si existe las tablas se eliminan las tablas
DROP TABLE IF EXISTS Producto;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Estado;
GO


--CREACIÓN TABLA ESTADO
CREATE TABLE Estado (
    IdEstado INTEGER PRIMARY KEY,
    NombreEstado VARCHAR(45)
);

--CREACIÓN TABLA PRODUCTO
CREATE TABLE Producto (
    IdProducto INTEGER PRIMARY KEY IDENTITY(1,1),
    NombreProducto VARCHAR(75) NOT NULL,
    Cliente VARCHAR(45) NOT NULL,
    Precio REAL NOT NULL,
    Stock INTEGER NOT NULL,
    IdEstado INTEGER NOT NULL,
    Fecha DATE NOT NULL,
    CONSTRAINT FK_Producto_Estado FOREIGN KEY (IdEstado) REFERENCES Estado(IdEstado)
);

--CREACIÓN TABLA USUARIO
CREATE table Usuario (
    IdUsuario INTEGER PRIMARY KEY IDENTITY(1,1),
    nombreUsuario VARCHAR(45) NOT NULL,
    contraseña VARCHAR(245) NOT NULL,
    CorreoElectronico VARCHAR(100) NOT NULL
);

INSERT INTO Usuario (IdUsuario, nombreUsuario, contraseña, CorreoElectronico) VALUES 
('Prueba Usuario', 'VarChar45._{}', 'correoprueba@gmail.com')

GO
-- Creación procedimiento para insertar los datos


--INSERT DE LA TABLA ESTADO, SE VALIDA EL NÚMERO DE ESTADO
INSERT INTO Estado (idEstado, NombreEstado) VALUES (1, 'Aprobado'), (2, 'En proceso'), (3, 'Rechazado');

--INSERT DE ELEMENTOS EN LA TABLA PRODUCTOS, VISUALIZACIÓN CRUD
INSERT INTO Producto (NombreProducto, Cliente, Precio, Stock, IdEstado, Fecha) VALUES 
('CosmeticoFragancia', 'Transportado SLC S.A.S', 520000, 20, 1, '2026-01-31'),
('CosmeticoFragancia', 'Transportado SLC S.A.S', 300000, 42, 2, '2026-03-23'),
('Valeriana Perfume 60 MLG', 'Empres S.A.S Solutions', 800000, 12, 3, '2026-02-14'),
('Hoddie Chico 34 US', 'TYM ROPA', 350000, 34, 1, '2026-02-14'),
('Base de Maquillaje Mate', 'Distribuidora Belleza S.A.', 45000.0, 100, 1, '2026-01-15'),
('Serum Vitamina C', 'Farmacias Global', 85000.50, 15, 2, '2026-01-20'),
('Labial Larga Duración', 'Tiendas Makeup Pro', 22000.0, 50, 1, '2026-02-01'),
('Paleta de Sombras Neon', 'Trendy Store', 120000.0, 8, 3, '2026-02-05'),
('Protector Solar FPS 50', 'BioSkin Care', 65000.0, 200, 1, '2026-02-10'),
('Crema Hidratante Noche', 'Estética Integral', 55000.0, 30, 2, '2026-02-12'),
('Delineador de Ojos Gel', 'Fashion Retail', 18000.0, 0, 3, '2026-02-14'),
('Agua Micelar 400ml', 'Supermercados Éxito', 32000.0, 45, 1, '2026-02-16'),
('Mascara de Pestañas', 'Beauty Supply Co.', 28000.0, 60, 2, '2026-02-18'),
('Exfoliante Corporal Café', 'Natural Spa', 42000.0, 12, 3, '2026-02-20');

GO

CREATE PROCEDURE sp_CrearProducto
    @Nombre NVARCHAR(100),
    @Cliente NVARCHAR(100),
    @Precio DECIMAL(18,2),
    @Stock INT,
    @IdEstado INT
AS
BEGIN
    INSERT INTO [dbo].[Producto] (NombreProducto, Cliente, Precio, Stock, IdEstado, Fecha)
    VALUES (@Nombre, @Cliente, @Precio, @Stock, @IdEstado, GETDATE());

    SELECT SCOPE_IDENTITY() AS IdNuevo;
END