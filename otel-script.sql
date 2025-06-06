USE [oteldb]
GO
/****** Object:  Table [dbo].[Durum]    Script Date: 22.04.2025 16:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Durum](
	[DurumID] [int] IDENTITY(1,1) NOT NULL,
	[Durum1] [nvarchar](20) NULL,
 CONSTRAINT [PK_Durum_1] PRIMARY KEY CLUSTERED 
(
	[DurumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Musteriler]    Script Date: 22.04.2025 16:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Musteriler](
	[MusteriID] [int] IDENTITY(1,1) NOT NULL,
	[TC] [varchar](11) NOT NULL,
	[Ad] [nvarchar](50) NOT NULL,
	[Soyad] [nvarchar](50) NOT NULL,
	[SehirID] [int] NOT NULL,
	[GelisTarihi] [datetime] NOT NULL,
	[CikisTarihi] [datetime] NOT NULL,
	[DogumTarihi] [datetime] NOT NULL,
	[Telefon] [nvarchar](20) NOT NULL,
	[OdaTuruID] [int] NOT NULL,
	[DurumID] [int] NOT NULL,
 CONSTRAINT [PK__Musteril__7262447134287F8C] PRIMARY KEY CLUSTERED 
(
	[MusteriID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sehirler]    Script Date: 22.04.2025 16:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sehirler](
	[SehirID] [int] IDENTITY(1,1) NOT NULL,
	[SehirAd] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[SehirID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Odalar]    Script Date: 22.04.2025 16:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Odalar](
	[OdaID] [int] IDENTITY(1,1) NOT NULL,
	[OdaTuru] [nvarchar](20) NOT NULL,
	[Dolu] [char](10) NULL,
	[OdaDurumu] [varchar](20) NULL,
	[OdaTuruID] [int] NULL,
 CONSTRAINT [PK__Odalar__190B1E09DE8D89EC] PRIMARY KEY CLUSTERED 
(
	[OdaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OdaTurleri]    Script Date: 22.04.2025 16:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OdaTurleri](
	[OdaTuruID] [int] IDENTITY(1,1) NOT NULL,
	[OdaTuru] [nvarchar](20) NULL,
 CONSTRAINT [PK_OdaTurleri] PRIMARY KEY CLUSTERED 
(
	[OdaTuruID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[View_1]    Script Date: 22.04.2025 16:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_1]
AS
SELECT dbo.Musteriler.TC, dbo.Musteriler.Ad, dbo.Musteriler.Soyad, dbo.Sehirler.SehirAd, dbo.Musteriler.GelisTarihi, dbo.Musteriler.CikisTarihi, dbo.Musteriler.DogumTarihi, dbo.Musteriler.Telefon, dbo.OdaTurleri.OdaTuru, 
                  dbo.Durum.Durum
FROM     dbo.Musteriler INNER JOIN
                  dbo.Odalar ON dbo.Musteriler.OdaID = dbo.Odalar.OdaID INNER JOIN
                  dbo.OdaTurleri ON dbo.Musteriler.OdaTuruID = dbo.OdaTurleri.OdaTuruID AND dbo.Odalar.OdaTuruID = dbo.OdaTurleri.OdaTuruID INNER JOIN
                  dbo.Sehirler ON dbo.Musteriler.SehirID = dbo.Sehirler.SehirID INNER JOIN
                  dbo.Durum ON dbo.Musteriler.DurumID = dbo.Durum.DurumID
GO
ALTER TABLE [dbo].[Musteriler] ADD  CONSTRAINT [DF__Musterile__Durum__37A5467C]  DEFAULT ('Aktif') FOR [DurumID]
GO
ALTER TABLE [dbo].[Odalar] ADD  CONSTRAINT [DF__Odalar__Dolu__3A81B327]  DEFAULT ((0)) FOR [Dolu]
GO
ALTER TABLE [dbo].[Odalar] ADD  CONSTRAINT [DF__Odalar__OdaDurum__3B75D760]  DEFAULT ('Aktif') FOR [OdaDurumu]
GO
ALTER TABLE [dbo].[Musteriler]  WITH CHECK ADD  CONSTRAINT [FK_Musteriler_Durum] FOREIGN KEY([DurumID])
REFERENCES [dbo].[Durum] ([DurumID])
GO
ALTER TABLE [dbo].[Musteriler] CHECK CONSTRAINT [FK_Musteriler_Durum]
GO
ALTER TABLE [dbo].[Musteriler]  WITH CHECK ADD  CONSTRAINT [FK_Musteriler_OdaTurleri] FOREIGN KEY([OdaTuruID])
REFERENCES [dbo].[OdaTurleri] ([OdaTuruID])
GO
ALTER TABLE [dbo].[Musteriler] CHECK CONSTRAINT [FK_Musteriler_OdaTurleri]
GO
ALTER TABLE [dbo].[Musteriler]  WITH CHECK ADD  CONSTRAINT [FK_Musteriler_Sehirler] FOREIGN KEY([SehirID])
REFERENCES [dbo].[Sehirler] ([SehirID])
GO
ALTER TABLE [dbo].[Musteriler] CHECK CONSTRAINT [FK_Musteriler_Sehirler]
GO
ALTER TABLE [dbo].[Odalar]  WITH CHECK ADD  CONSTRAINT [FK_Odalar_OdaTurleri] FOREIGN KEY([OdaTuruID])
REFERENCES [dbo].[OdaTurleri] ([OdaTuruID])
GO
ALTER TABLE [dbo].[Odalar] CHECK CONSTRAINT [FK_Odalar_OdaTurleri]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Musteriler"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 295
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "Odalar"
            Begin Extent = 
               Top = 7
               Left = 290
               Bottom = 245
               Right = 484
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OdaTurleri"
            Begin Extent = 
               Top = 7
               Left = 532
               Bottom = 186
               Right = 726
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Sehirler"
            Begin Extent = 
               Top = 7
               Left = 774
               Bottom = 176
               Right = 968
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Durum"
            Begin Extent = 
               Top = 127
               Left = 1016
               Bottom = 246
               Right = 1210
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_1'
GO
