﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

BEGIN
	DELETE FROM WhitelistedProviders;

-- APR-2424 Whitelisted Providers May Refresh
	INSERT INTO WhitelistedProviders ([UKPRN], [StartDateTime], [EndDateTime]) 
	VALUES (10002085, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10007165, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10007177, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10023326, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10024636, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10027061, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10027216, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10029699, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10032315, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10032663, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10033950, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10036126, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10040392, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10040411, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10049431, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10061312, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10062335, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10063769, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10000565, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10000831, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10001113, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10001156, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10001777, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10004486, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10005204, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10019581, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10024704, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10029952, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10031982, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10034969, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10037391, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10061524, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),
		   (10013516, N'2021-05-17 08:00:00', N'2021-06-30 23:59:59'),

-- APR-2530 Whitelisted Providers July Refresh (UTC times used)
		   (10034146, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10037203, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10039772, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10047354, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10048055, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10056832, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10057290, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10063352, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10063869, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10067387, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10003375, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10004643, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10025998, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10027272, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10028038, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10034309, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10035789, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10038023, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10038772, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10039527, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10040525, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10042570, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10046692, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10048177, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10052858, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10056912, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10057050, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10061219, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10061407, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10061826, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10061842, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10063274, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10063309, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10065535, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10065578, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10065628, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10065960, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10084622, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10007784, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10019812, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10044886, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10045282, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10047122, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10056428, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10057055, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10065872, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10066838, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10067528, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10040187, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10048569, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),
		   (10055542, N'2021-07-01 08:00:00', N'2021-07-31 22:59:59'),

-- APR-2558 Whitelisted Providers August Refresh (UTC times used)
		   (10044008, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10084912, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10003526, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10004977, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10005897, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10036952, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10036956, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10037445, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10040915, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10041057, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10042593, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10043208, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10043813, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10044321, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10044729, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10044792, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10045955, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10046315, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10046679, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10046797, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10046979, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10047356, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10048110, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10049392, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10056500, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10062612, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10064332, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10065654, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10026268, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10034300, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10042221, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10043715, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10045354, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10045538, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10055025, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10056013, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10062157, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10062759, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10065933, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10066561, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10081982, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10082136, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10005781, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10044407, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10048284, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10053948, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10056839, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10065657, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59'),
		   (10082053, N'2021-07-31 23:00:00', N'2021-08-31 22:59:59')

:r .\APR-2311.sql

END