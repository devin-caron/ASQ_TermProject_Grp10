IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'fdms_db')
	BEGIN
		CREATE DATABASE [fdms_db]
	END

	GO
		USE [fdms_db]
	GO
	

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AttitudeParameters' and xtype='U')
BEGIN

    CREATE TABLE [dbo].[AttitudeParameters](
	[AircraftID] [nchar](6) NOT NULL,
	[Timestamp] [datetime] NULL,
	[Altitude] [float] NOT NULL,
	[Pitch] [float] NOT NULL,
	[Bank] [float] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AircraftID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

END


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='GForceParameters' and xtype='U')
BEGIN
    
	CREATE TABLE [dbo].[GForceParameters](
	[AircraftID] [nchar](6) NOT NULL,
	[Timestamp] [datetime] NULL,
	[AccelX] [float] NOT NULL,
	[AccelY] [float] NOT NULL,
	[AccelZ] [float] NOT NULL,
	[Weight] [float] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AircraftID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

END