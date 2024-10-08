USE [Aplicacion]
GO
/****** Object:  Table [dbo].[Administradores]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administradores](
	[IdAdmin] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Correo] [nvarchar](150) NOT NULL,
	[Contrasenia] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAdmin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Citas]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Citas](
	[IdCita] [int] IDENTITY(1,1) NOT NULL,
	[IdVehiculo] [int] NOT NULL,
	[FechaInicio] [datetime] NOT NULL,
	[FechaFin] [datetime] NOT NULL,
	[Estado] [bit] NULL,
	[IdCliente] [int] NULL,
	[FechaTerminacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CitasAdmin]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CitasAdmin](
	[IdCitaAdmin] [int] IDENTITY(1,1) NOT NULL,
	[IdCita] [int] NOT NULL,
	[IdAdmin] [int] NOT NULL,
	[ComentariosAdmin] [nvarchar](max) NOT NULL,
	[FechaAprobacion] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCitaAdmin] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[IdCliente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[CorreoElectronico] [nvarchar](150) NOT NULL,
	[Telefono] [int] NOT NULL,
	[Direccion] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Clientes__D5946642ABD83720] PRIMARY KEY CLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Clientes__531402F36E12F768] UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehiculos]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehiculos](
	[IdVehiculo] [int] IDENTITY(1,1) NOT NULL,
	[IdCliente] [int] NOT NULL,
	[Marca] [nvarchar](50) NOT NULL,
	[Modelo] [nvarchar](50) NOT NULL,
	[Anio] [int] NOT NULL,
	[Placa] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVehiculo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD FOREIGN KEY([IdVehiculo])
REFERENCES [dbo].[Vehiculos] ([IdVehiculo])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Clientes] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Clientes]
GO
ALTER TABLE [dbo].[CitasAdmin]  WITH CHECK ADD FOREIGN KEY([IdAdmin])
REFERENCES [dbo].[Administradores] ([IdAdmin])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CitasAdmin]  WITH CHECK ADD FOREIGN KEY([IdCita])
REFERENCES [dbo].[Citas] ([IdCita])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Vehiculos]  WITH CHECK ADD  CONSTRAINT [FK__Vehiculos__IdCli__3C69FB99] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Clientes] ([IdCliente])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Vehiculos] CHECK CONSTRAINT [FK__Vehiculos__IdCli__3C69FB99]
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarAdmin]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarAdmin]
	@IdAdmin INT,
    @Nombre NVARCHAR(100),
    @Correo NVARCHAR(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		-- Verificar si el correo ya está en uso por otro administrador
		IF EXISTS (SELECT 1 FROM Administradores WHERE Correo = @Correo AND IdAdmin <> @IdAdmin)
		BEGIN
			THROW 51000, 'Este correo ya está en uso por otro administrador.', 1;
		END

		-- Actualizar el administrador
		UPDATE Administradores
		SET Nombre = @Nombre, Correo = @Correo
		WHERE IdAdmin = @IdAdmin;

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarCita]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarCita]    
	@IdCita INT,
    @IdVehiculo INT,
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		IF EXISTS (
        SELECT 1
        FROM Citas
        WHERE (
              -- Conflicto si la nueva cita comienza dentro del rango de otra cita
              (FechaInicio <= @FechaInicio AND FechaFin > @FechaInicio)
              OR
              -- Conflicto si la nueva cita termina dentro del rango de otra cita
              (FechaInicio < @FechaFin AND FechaFin >= @FechaFin)
              OR
              -- Conflicto si la nueva cita contiene completamente otra cita
              (@FechaInicio <= FechaInicio AND @FechaFin >= FechaFin)
          )
		)
		BEGIN
			-- Lanzar un error si ya existe una cita en el mismo intervalo
			THROW 51000, 'Ya existe una cita programada en este horario.', 1;
		END


		UPDATE Citas
		SET IdVehiculo = @IdVehiculo,
			FechaInicio = @FechaInicio,
			FechaFin = @FechaFin
		WHERE IdCita = @IdCita;
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarCliente]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarCliente]
	@IdCliente INT,
	@Nombre NVARCHAR(100) = NULL,
    @Apellido NVARCHAR(100) = NULL,
    @CorreoElectronico NVARCHAR(150) = NULL,
    @Telefono INT = NULL,
    @Direccion NVARCHAR(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @existe INT;

	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		SELECT 
			@existe = COUNT(*)
		FROM 
			Administradores
		WHERE 
			Correo = @CorreoElectronico;

		IF @existe > 0
		BEGIN
			THROW 51000, 'Este usuario no puede ser actualizado, contacte al administrador', 1;
		END
		
		UPDATE Clientes
		SET 
			Nombre = ISNULL(@Nombre, Nombre),
			Apellido = ISNULL(@Apellido, Apellido),
			CorreoElectronico = ISNULL(@CorreoElectronico, CorreoElectronico),
			Telefono = ISNULL(@Telefono, Telefono),
			Direccion = ISNULL(@Direccion, Direccion)
		WHERE IdCliente = @IdCliente;
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarEstadoCita]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ActualizarEstadoCita]
	@IdCita INT,
	@Correo VARCHAR(100),
    @Estado BIT, 
    @Comentarios VARCHAR(MAX),
	@FechaTerminacion DATETIME = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		DECLARE @IdAdmin INT
		DECLARE @FechaInicio DATETIME;
        DECLARE @FechaFin DATETIME;

		SET @IdAdmin = (SELECT IdAdmin FROM Administradores WHERE Correo = @Correo);

		IF @IdAdmin < 0
		BEGIN
			THROW 51000, 'No puede realizar esta operacion', 1;
		END

        SELECT @FechaInicio = FechaInicio, @FechaFin = FechaFin
        FROM Citas
        WHERE IdCita = @IdCita;

        IF @FechaTerminacion < @FechaInicio OR (@FechaTerminacion >= @FechaInicio AND @FechaTerminacion <= @FechaFin)
        BEGIN
            THROW 51000, 'La fecha de terminación no puede ser menor o estar entre la fecha de inicio y la fecha de fin de la cita.', 1;
        END

		INSERT INTO CitasAdmin (ComentariosAdmin, IdCita, IdAdmin, FechaAprobacion) VALUES (@Comentarios, @IdCita, @IdAdmin, GETDATE());
		
		UPDATE Citas
		SET Estado = @Estado, FechaTerminacion = @FechaTerminacion
		WHERE IdCita = @IdCita;

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AgregarVehiculo]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_AgregarVehiculo]
	@IdCliente INT,
    @Marca NVARCHAR(100),
    @Modelo NVARCHAR(100),
    @Anio INT,
    @Placa NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		INSERT INTO Vehiculos (IdCliente, Marca, Modelo, Anio, Placa)
		VALUES (@IdCliente, @Marca, @Modelo, @Anio, @Placa);
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CrearCita]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CrearCita] 
	@IdVehiculo INT,
	@IdCliente INT,
    @FechaInicio DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		DECLARE @FechaFin DATETIME;
		SET @FechaFin = DATEADD(HOUR, 2, @FechaInicio);

		IF EXISTS (
        SELECT 1
        FROM Citas
        WHERE (
              -- Conflicto si la nueva cita comienza dentro del rango de otra cita
              (FechaInicio <= @FechaInicio AND FechaFin > @FechaInicio)
              OR
              -- Conflicto si la nueva cita termina dentro del rango de otra cita
              (FechaInicio < @FechaFin AND FechaFin >= @FechaFin)
              OR
              -- Conflicto si la nueva cita contiene completamente otra cita
              (@FechaInicio <= FechaInicio AND @FechaFin >= FechaFin)
          )
		)
		BEGIN
			-- Lanzar un error si ya existe una cita en el mismo intervalo
			THROW 51000, 'Ya existe una cita programada en este horario.', 1;
		END

		INSERT INTO Citas (IdVehiculo, FechaInicio, FechaFin, IdCliente)
		VALUES (@IdVehiculo, @FechaInicio, @FechaFin, @IdCliente);
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarAdmin]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EliminarAdmin]
	@IdAdmin INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		DELETE FROM Administradores WHERE IdAdmin = @IdAdmin;

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarCita]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EliminarCita]     
	@IdCita INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		DELETE FROM Citas
		WHERE IdCita = @IdCita;
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarCliente]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EliminarCliente]
	@IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		DELETE FROM Clientes
		WHERE IdCliente = @IdCliente;
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertarAdmin]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertarAdmin]
	@Nombre NVARCHAR(100),
    @Correo NVARCHAR(150),
    @Contrasenia NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		IF EXISTS (SELECT 1 FROM Administradores WHERE Correo = @Correo)
		BEGIN
			THROW 51000, 'Ya existe un administrador con este correo.', 1;
		END

		INSERT INTO Administradores (Nombre, Correo, Contrasenia)
		VALUES (@Nombre, @Correo, @Contrasenia);

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ObtenerAdminPorId]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ObtenerAdminPorId]
	@IdAdmin INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		SELECT IdAdmin, Nombre, Correo FROM Administradores WHERE IdAdmin = @IdAdmin;

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ObtenerCitaPorId]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ObtenerCitaPorId]  
	@IdCita INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT c.IdCita, c.IdVehiculo, c.FechaInicio, c.IdCliente, c.FechaFin, c.Estado, ca.ComentariosAdmin
		FROM Citas c
		LEFT JOIN CitasAdmin ca ON ca.IdCita = c.IdCita
		WHERE c.IdCita = @IdCita;
		
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ObtenerCitas]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ObtenerCitas]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT 
		c.IdCita, 
		c.IdVehiculo, 
		c.IdCliente, 
		c.FechaInicio, 
		c.FechaTerminacion, 
		c.Estado,
		v.Marca + ' ' + v.Modelo AS Vehiculo,
		ca.ComentariosAdmin AS Comentarios
		FROM Citas c
		LEFT JOIN CitasAdmin ca ON c.IdCita = ca.IdCita
		LEFT JOIN Vehiculos v ON v.IdVehiculo = c.IdVehiculo;

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ObtenerClientePorId]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ObtenerClientePorId]
	 @IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT IdCliente, Nombre, Apellido, CorreoElectronico, Telefono, Direccion
		FROM Clientes
		WHERE IdCliente = @IdCliente;
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ObtenerClientes]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Alfredo Rodriguez Coronado>
-- Create date: <Create Date, 06/09/2024>
-- Description:	<Description,Obtener clientes con vehiculos>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ObtenerClientes]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT IdCliente, Nombre, Apellido, CorreoElectronico, Telefono, Direccion FROM Clientes;
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ObtenerTodosLosAdmins]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ObtenerTodosLosAdmins]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		SELECT IdAdmin, Nombre, Correo FROM Administradores;

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ObtenerVehiculosPorCliente]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ObtenerVehiculosPorCliente]      
	@IdCliente INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT IdVehiculo, Marca, Modelo, Placa
		FROM Vehiculos
		WHERE IdCliente = @IdCliente;
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[spConsultaUsuarioEnDb]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Alfredo Rodriguez>
-- Create date: <Create Date, 05/09/2024>
-- Description:	<Description, Consultar usuario en db>
-- =============================================
CREATE PROCEDURE [dbo].[spConsultaUsuarioEnDb]
(
	@CorreoElectronico VARCHAR(100)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT 
			c.Nombre, 
			c.Apellido 
		FROM 
			Clientes c 
		WHERE 
			c.CorreoElectronico = @CorreoElectronico; 
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[spCrearNuevoCliente]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, Alfredo Rodriguez>
-- Create date: <Create Date, 05/09/2024>
-- Description:	<Description, Consultar usuario en db>
-- =============================================
CREATE PROCEDURE [dbo].[spCrearNuevoCliente]
(
	@CorreoElectronico NVARCHAR(100),
	@Telefono INT,
	@Apellido NVARCHAR(100),
	@Nombre NVARCHAR(100),
	@Contrasenia NVARCHAR(100),
	@Direccion NVARCHAR(100)

)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	DECLARE @existe INT;


	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY

		SELECT 
			@existe = COUNT(*)
		FROM 
			Administradores
		WHERE 
			Correo = @CorreoElectronico;

		IF @existe > 0
		BEGIN
			THROW 51000, 'Este usuario no puede ser registrado, contacte al administrador', 1;
		END

		SELECT 
			@existe = COUNT(*)
		FROM 
			Clientes
		WHERE 
			CorreoElectronico = @CorreoElectronico;

		IF @existe > 0
		BEGIN
			THROW 51000, 'Este usuario no puede ser registrado, contacte al administrador', 1;
		END
		
		INSERT INTO Clientes (Nombre, Apellido, CorreoElectronico, Telefono, Contrasenia, Direccion)
        VALUES (@Nombre, @Apellido, @CorreoElectronico, @Telefono, @Contrasenia, @Direccion);

	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[spValidarCredencialesAdmin]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spValidarCredencialesAdmin]
(
	@Correo NVARCHAR(100)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT
			Contrasenia
		FROM
			Administradores
		WHERE
			Correo = @Correo
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[spValidarCredencialesClientes]    Script Date: 08/09/2024 03:18:43 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spValidarCredencialesClientes]
(
	@Correo NVARCHAR(100)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Message VARCHAR(255);
	BEGIN TRY
		SELECT
			Contrasenia
		FROM
			Clientes
		WHERE
			CorreoElectronico = @Correo
	END TRY
	BEGIN CATCH
		SET @Message = ERROR_MESSAGE();
		THROW 51000, @Message, 1;
	END CATCH
END
GO
