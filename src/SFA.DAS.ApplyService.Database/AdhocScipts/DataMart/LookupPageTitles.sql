﻿DROP TABLE IF EXISTS LookupPageTitles
GO

CREATE TABLE LookupPageTitles
(
	[SequenceNumber] INT NOT NULL,
	[SectionNumber] INT NOT NULL,
	[PageId] NVARCHAR(50) NOT NULL,
	[Title] NVARCHAR(255)
)
GO

INSERT INTO LookupPageTitles ([SequenceNumber],[SectionNumber],[PageId],[Title]) VALUES
(0, 1, '1', 'Preamble'),
(0, 1, '2', 'Provider route'),
(0, 1, '3', 'Conditions of acceptance'),

(1, 1, '10', 'Your organisation'),
(1, 1, '11', 'Your organisation'),
(1, 1, '12', 'Your organisation'),

(1, 2, '20', 'Does your organisation have an ultimate parent company in the UK?'),
(1, 2, '21', 'Enter your organisation''s ultimate parent company details'),
(1, 2, '30', 'What is your organisation''s Information Commissioner''s Office (ICO) registration number?'),
(1, 2, '40', 'Does your organisation have a website?'),

(1, 3, '70', 'Confirm who''s in control - Companies House'),
(1, 3, '80', 'Confirm who''s in control - Trustees'),
(1, 3, '90', 'Confirm who''s in control - Add Trustees'),
(1, 3, '100', 'Tell us your organisation''s type'),
(1, 3, '101', 'What is your organisation''s partner?'),
(1, 3, '110', 'Enter the individual''s details'),
(1, 3, '111', 'Enter the organisation''s details'),
(1, 3, '120', 'What is individual''s date of birth?'),
(1, 3, '130', 'Confirm who''s in control - Manual Entry'),

(1, 4, '140', 'What is your organisation?'),
(1, 4, '150', 'What is your organisation?'),
(1, 4, '160', 'What type of educational institute is your organisation?'),
(1, 4, '170', 'What type of public body is your organisation?'),
(1, 4, '180', 'What type of school is your organisation?'),
(1, 4, '200', 'Is your organisation registered and receiving funding from ESFA?'),
(1, 4, '210', 'Is your organisation receiving funding from ESFA?'),
(1, 4, '220', 'Is your organisation monitored and supported by the Office of Students?'),
(1, 4, '230', 'How would you describe your organisation?'),
(1, 4, '50', 'How long has your organisation been actively trading?'),
(1, 4, '60', 'How long has your organisation been actively trading?'),
(1, 4, '10001', 'Your organisation is not eligible to apply to join RoATP'),
(1, 4, '10002', 'Your organisation is not eligible to apply to join RoATP'),
(1, 4, '10003', 'Your organisation is not eligible to apply to join RoATP'),

(1, 5, '235', 'Is your organisation funded by the Office for Students?'),
(1, 5, '240', 'Does your organisation offer initial teacher training?'),
(1, 5, '250', 'Is the postgraduate teaching apprenticeship the only apprenticeship your organisation intends to deliver?'),
(1, 5, '260', 'Has your organisation had a full Ofsted inspection in further education and skills?'),
(1, 5, '270', 'Did your organisation get a grade for apprenticeships in this full Ofsted inspection?'),
(1, 5, '280', 'What grade did your organisation get for overall effectiveness in this full Ofsted inspection?'),
(1, 5, '290', 'Has your organisation had a monitoring visit for apprenticeships in further education and skills?'),
(1, 5, '292', 'Has your organisation had 2 consecutive monitoring visits with the grade ''insufficient progress''?'),
(1, 5, '294', 'Was the most recent monitoring visit within the last 18 months?'),
(1, 5, '300', 'What grade did your organisation get for apprenticeships in this full Ofsted inspection?'),
(1, 5, '301', 'What grade did your organisation get for apprenticeships in this full Ofsted inspection?'),
(1, 5, '310', 'Did your organisation get this grade within the last 3 years?'),
(1, 5, '311', 'Did your organisation get this grade within the last 3 years?'),
(1, 5, '320', 'Has your organisation maintained funding from an education agency since its full Ofsted inspection?'),
(1, 5, '330', 'Has your organisation had a short Ofsted inspection within the last 3 years?'),
(1, 5, '340', 'Has your organisation maintained the grade it got in its full Ofsted inspection in its short Ofsted inspection?'),
(1, 5, '350', 'Has your organisation delivered apprenticeship training as a subcontractor in the last 12 months?'),
(1, 5, '360', 'Upload a copy of a legally binding contract between your organisation and a main or employer provider'),
(1, 5, '10004', 'Your organisation is not eligible to apply to join RoATP'),
(1, 5, '10010', 'Your organisation is not eligible to apply to join RoATP'),

(2, 1, '2000', 'Financial health assessment'),
(2, 1, '2010', 'Financial health assessment'),

(2, 2, '2100', 'Was your organisation''s total annual turnover over £75 million for the latest reported financial year?'),
(2, 2, '2110', 'Does your organisation expect its funding from ESFA to be less than 5% of its total annual turnover?'),
(2, 2, '2120', 'Financial evidence'),
(2, 2, '2130', 'Financial evidence'),
(2, 2, '2140', 'Financial evidence'),
(2, 2, '2150', 'Has your organisation produced its latest full financial statements covering a minimum of 12 months?'),
(2, 2, '2160', 'Upload your organisation''s latest full financial statements covering the last 12 months'),
(2, 2, '2170', 'Has your organisation produced financial statements covering any period within the last 12 months?'),
(2, 2, '2180', 'What you need to upload'),
(2, 2, '2190', 'Upload your organisation''s financial statements covering any period within the last 12 months'),
(2, 2, '2195', 'What you need to upload'),
(2, 2, '2200', 'Upload your organisation''s management accounts covering the remaining period to date'),
(2, 2, '2210', 'Has your organisation produced full management accounts covering the last 12 months?'),
(2, 2, '2220', 'What you need to upload'),
(2, 2, '2225', 'Upload your organisation''s full management accounts for the last 12 months'),
(2, 2, '2230', 'What you need to upload'),
(2, 2, '2240', 'Upload your organisation''s management accounts covering at least 3 months within the last 12 months'),
(2, 2, '2250', 'Upload your organisation''s financial projections covering the remaining period'),
(2, 2, '2260', 'Who prepared the answers and uploads in this section?'),
(2, 2, '2270', 'What''s the accounting reference date for the financial information you are submitting?'),
(2, 2, '2280', 'How many months does the accounting period cover for the financial information you are submitting?'),

(2, 3, '2500', 'Does your UK ultimate parent company have consolidated financial statements?'),
(2, 3, '2510', 'Upload your UK ultimate parent company''s consolidated financial statements'),
(2, 3, '2520', 'Does your UK ultimate parent company have other active subsidiary companies?'),
(2, 3, '2530', 'Upload a financial statement for all your UK ultimate parent company''s active subsidiary companies'),
(2, 3, '2540', 'Upload your UK ultimate parent company''s full financial statements covering the last 12 months'),

(3, 1, '3000', 'Criminal and compliance checks on your organisation'),

(3, 2, '3100', 'Does your organisation have any composition with creditors?'),
(3, 2, '3110', 'Has your organisation failed to pay back funds in the last 3 years?'),
(3, 2, '3120', 'Has your organisation had a contract terminated early by a public body in the last 3 years?'),
(3, 2, '3130', 'Has your organisation withdrawn from a contract with a public body in the last 3 years?'),
(3, 2, '3135', 'Has your organisation been removed from the Register of Training Organisations (RoTO) in the last 3 years?'),
(3, 2, '3140', 'Has your organisation had funding removed from any education bodies in the last 3 years?'),
(3, 2, '3150', 'Has your organisation been removed from any professional or trade registers in the last 3 years?'),
(3, 2, '3160', 'Has your organisation been involuntarily withdrawn from Initial Teacher Training accreditation in the last 3 years?'),
(3, 2, '3170', 'Has your organisation been removed from any charity register?'),
(3, 2, '3180', 'Has your organisation been investigated due to safeguarding issues in the last 3 months?'),
(3, 2, '3190', 'Is your organisation currently, or has within the last 5 years been, subject to an investigation by the ESFA or other public body or regulator?'),
(3, 2, '3199', 'Has your organisation been subject to insolvency or winding up proceedings in the last 3 years?'),

(3, 3, '3200', 'Criminal and compliance checks on who''s in control of your organisation'),

(3, 4, '3300', 'Does anyone who''s in control of your organisation have any unspent criminal convictions?'),
(3, 4, '3301', 'Do you have any unspent criminal convictions?'),
(3, 4, '3310', 'Has anyone who''s in control of your organisation failed to pay back funds in the last 3 years?'),
(3, 4, '3311', 'Have you failed to pay back funds in the last 3 years?'),
(3, 4, '3320', 'Has anyone who''s in control of your organisation been investigated for fraud or irregularities in the last 3 years?'),
(3, 4, '3321', 'Have you been investigated for fraud or irregularities in the last 3 years?'),
(3, 4, '3330', 'Does anyone who''s in control of your organisation have any ongoing investigations for fraud or irregularities?'),
(3, 4, '3331', 'Do you have any ongoing investigations for fraud or irregularities?'),
(3, 4, '3340', 'Has anyone who''s in control of your organisation had a contract terminated by a public body in the last 3 years?'),
(3, 4, '3341', 'Have you had a contract terminated by a public body in the last 3 years?'),
(3, 4, '3350', 'Has anyone who''s in control of your organisation withdrawn from a contract with a public body in the last 3 years?'),
(3, 4, '3351', 'Have you withdrawn from a contract with a public body in the last 3 years?'),
(3, 4, '3360', 'Has anyone who''s in control of your organisation breached tax payments or social security contributions in the last 3 years?'),
(3, 4, '3361', 'Have you breached tax payments or social security contributions in the last 3 years?'),
(3, 4, '3370', 'Is anyone who''s in control of your organisation on the Register of Removed Trustees?'),
(3, 4, '3371', 'Are you on the Register of Removed Trustees?'),
(3, 4, '3380', 'Has anyone who''s in control of your organisation or any of its partner organisations been made bankrupt in the last 3 years?'),
(3, 4, '3381', 'Have you been made bankrupt in the last 3 years?'),
(3, 4, '3390', 'Has anyone who''s in control of your organisation, or any of its partner organisations, been subject to a prohibition order from the Teaching Regulation Agency on behalf of the Secretary of State for Education?'),
(3, 4, '3391', 'Have you been subject to a prohibition order from the Teaching Regulation Agency on behalf of the Secretary of State for Education?'),
(3, 4, '3395', 'Has anyone who''s in control of your organisation, or any of its partner organisations, been subject to a ban from management or governance of schools?'),
(3, 4, '3396', 'Have you been subject to a ban from  management or governance of schools?'),

(4, 1, '4000', 'Protecting your apprentices'),
(4, 1, '4001', 'Protecting your apprentices'),
(4, 1, '4002', 'Protecting your apprentices'),

(4, 2, '4010', 'Upload your organisation''s continuity plan for apprenticeship training'),

(4, 3, '4020', 'Upload your organisation''s equality and diversity policy'),

(4, 4, '4030', 'Upload your organisation''s safeguarding policy'),
(4, 4, '4035', 'Tell us who has overall responsibility for safeguarding in your organisation'),
(4, 4, '4037', 'Does your organisation''s safeguarding policy include its responsibilities towards the Prevent duty?'),
(4, 4, '4038', 'Upload your organisation''s Prevent duty policy'),

(4, 5, '4040', 'Upload your organisation''s health and safety policy'),
(4, 5, '4045', 'Tell us who has overall responsibility for health and safety in your organisation'),

(4, 6, '4050', 'When acting as a subcontractor, will your organisation follow its lead provider''s policies and plans?'),

(5, 1, '5000', 'Readiness to engage'),
(5, 1, '5010', 'Readiness to engage'),
(5, 1, '5020', 'Readiness to engage'),

(5, 2, '5100', 'Has your organisation engaged with employers to deliver apprenticeship training to their employees?'),
(5, 2, '5110', 'How will your organisation manage its relationship with employers?'),
(5, 2, '5120', 'Tell us who''s responsible for managing relationships with employers'),
(5, 2, '5130', 'How will your organisation promote apprenticeships to employers?'),

(5, 3, '5200', 'Upload your organisation''s complaints policy'),
(5, 3, '5210', 'Enter the website link for your organisation''s complaints policy'),

(5, 4, '5300', 'Upload your organisation''s contract for services template with employers'),

(5, 5, '5400', 'Upload your organisation''s commitment statement template'),

(5, 6, '5500', 'What is your organisation''s process for initial assessments to recognise prior learning?'),
(5, 6, '5510', 'What is your organisation''s process to assess English and maths qualifications for apprentices?'),

(5, 7, '5550', 'How will you deliver the assessments in English and maths?'),
(5, 7, '5560', 'Where will you deliver the assessments in English and maths?'),
(5, 7, '5570', 'How will you continue to deliver English and maths training and assessments if there''s a significant event?'),

(5, 8, '5600', 'Does your organisation expect to use subcontractors in the first 12 months of joining RoATP?'),
(5, 8, '5610', 'How will your organisation carry out due diligence on its subcontractors?'),

(6, 1, '6000', 'Planning apprenticeship training'),
(6, 1, '6001', 'Planning apprenticeship training'),
(6, 1, '6002', 'Planning apprenticeship training'),

(6, 2, '6201', 'What type of apprenticeship training will your organisation offer?'),
(6, 2, '6202', 'What type of apprenticeship training will your organisation offer?'),
(6, 2, '6203', 'What type of apprenticeship training will your organisation offer?'),
(6, 2, '6204', 'What type of apprenticeship training will your organisation offer?'),
(6, 2, '6230', 'Tell us how your organisation is ready to deliver training in apprenticeship standards'),
(6, 2, '6250', 'Does your organisation have a plan to transition from apprenticeship frameworks to apprenticeship standards?'),
(6, 2, '6252', 'How will your organisation transition from apprenticeship frameworks to apprenticeship standards?'),
(6, 2, '6253', 'How will your organisation transition from apprenticeship frameworks to apprenticeship standards?'),
(6, 2, '6254', 'Why will your organisation only deliver apprenticeship frameworks?'),
(6, 2, '6260', 'Does your organisation have a plan to transition from apprenticeship frameworks to apprenticeship standards?'),
(6, 2, '6262', 'How will your organisation transition from apprenticeship frameworks to apprenticeship standards?'),
(6, 2, '6264', 'Why will your organisation only deliver apprenticeship frameworks?'),
(6, 2, '6270', 'How will your organisation engage with end-point assessment organisations (EPAOs)?'),
(6, 2, '6280', 'How will your organisation plan to engage and work with awarding bodies?'),

(6, 3, '6300', 'How will your organisation train its apprentices?'),

(6, 4, '6400', 'How will your organisation ensure apprentices are supported during their apprenticeship training?'),
(6, 4, '6405', 'How will your organisation ensure apprentices are supported during their apprenticeship training?'),
(6, 4, '6410', 'How will your organisation support its apprentices?'),
(6, 4, '6420', 'What other ways will your organisation use to support its apprentices?'),

(6, 5, '6500', 'How many starts does your organisation forecast in the first 12 months of joining the RoATP?'),
(6, 5, '6510', 'When will your organisation be ready to deliver training against this forecast?'),
(6, 5, '6520', 'Will your organisation recruit new staff to deliver training against these forecasts?'),
(6, 5, '6530', 'What is the typical ratio of the staff who deliver training to the apprentices?'),
(6, 5, '6540', 'How does your proposed staff to learner ratio offer quality support to your apprentices?'),

(6, 6, '6600', 'What teaching methods will your organisation use to deliver 20% off the job training?'),
(6, 6, '6610', 'How will your organisation ensure 20% off the job training is relevant to the specific apprenticeship being delivered?'),

(6, 7, '6700', 'Where will your apprentices be trained?'),

(7, 1, '7000', 'Delivering apprenticeship training'),
(7, 1, '7001', 'Delivering apprenticeship training'),
(7, 1, '7002', 'Delivering apprenticeship training'),

(7, 2, '7100', 'Tell us who has overall accountability for apprenticeships in your organisation'),

(7, 3, '7200', 'Confirm management hierarchy'),

(7, 4, '7300', 'Upload your management hierarchy''s expectations for quality and high standards in apprenticeship training'),
(7, 4, '7305', 'Tell us how these expectations for quality and high standards in apprenticeship training are monitored and evaluated'),
(7, 4, '7310', 'Tell us who''s responsible for maintaining these expectations for quality and high standards in apprenticeship training'),
(7, 4, '7320', 'How are these expectations for quality and high standards in apprenticeship training communicated to employees'),

(7, 5, '7500', 'Does your organisation have a team responsible for developing and delivering training?'),
(7, 5, '7510', 'Does your organisation have someone responsible for developing and delivering training?'),
(7, 5, '7520', 'Does your organisation have someone responsible for developing and delivering training?'),
(7, 5, '7530', 'Who has the team worked with to develop and deliver training?'),
(7, 5, '7540', 'Tell us who''s the overall manager for this team'),
(7, 5, '7560', 'Who has this person worked with to develop and deliver training?'),
(7, 5, '7570', 'How has the team worked with other organisations to develop and deliver training?'),
(7, 5, '7590', 'Tell us who''s the overall manager for this team'),
(7, 5, '7591', 'How has the team worked with other organisations and employers to develop and deliver training?'),
(7, 5, '7592', 'How has the team worked with employers to develop and deliver training?'),
(7, 5, '7593', 'How has this person worked with other organisations to develop and deliver training?'),
(7, 5, '7594', 'How has this person worked with other organisations and employers to develop and deliver training?'),
(7, 5, '7595', 'How has this person worked with employers to develop and deliver training?'),

(7, 6, '7600', 'What sectors will your organisation deliver apprenticeship training in?'),
(7, 6, '7610AA', 'What standards do you intend to deliver within the ''Agriculture, environmental and animal care'' sector?'),
(7, 6, '7610A', 'How many starts does your organisation forecast in the ''Agriculture, environmental and animal care'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7610B', 'How many employees will deliver training in the ''Agriculture, environmental and animal care'' sector?'),
(7, 6, '7610', 'Who''s the most experienced employee in the ''Agriculture, environmental and animal care'' sector?'),
(7, 6, '7611', 'Most Experienced Employee''s experience in the ''Agriculture, environmental and animal care'' sector'),
(7, 6, '7612', 'Most Experienced Employee''s experience in the ''Agriculture, environmental and animal care'' sector'),
(7, 6, '7613', 'Most Experienced Employee''s experience in the ''Agriculture, environmental and animal care'' sector'),
(7, 6, '7615AA', 'What standards do you intend to deliver within the ''Business and administration'' sector?'),
(7, 6, '7615A', 'How many starts does your organisation forecast in the ''Business and administration'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7615B', 'How many employees will deliver training in the ''Business and administration'' sector?'),
(7, 6, '7615', 'Who''s the most experienced employee in the ''Business and administration'' sector?'),
(7, 6, '7616', 'Most Experienced Employee''s experience in the ''Business and administration'' sector'),
(7, 6, '7617', 'Most Experienced Employee''s experience in the ''Business and administration'' sector'),
(7, 6, '7618', 'Most Experienced Employee''s experience in the ''Business and administration'' sector'),
(7, 6, '7620AA', 'What standards do you intend to deliver within the ''Care services'' sector?'),
(7, 6, '7620A', 'How many starts does your organisation forecast in the ''Care services'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7620B', 'How many employees will deliver training in the ''Care services'' sector?'),
(7, 6, '7620', 'Who''s the most experienced employee in the ''Care services'' sector?'),
(7, 6, '7621', 'Most Experienced Employee''s experience in the ''Care services'' sector'),
(7, 6, '7622', 'Most Experienced Employee''s experience in the ''Care services'' sector'),
(7, 6, '7623', 'Most Experienced Employee''s experience in the ''Care services'' sector'),
(7, 6, '7625AA', 'What standards do you intend to deliver within the ''Catering and hospitality'' sector?'),
(7, 6, '7625A', 'How many starts does your organisation forecast in the ''Catering and hospitality'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7625B', 'How many employees will deliver training in the ''Catering and hospitality'' sector?'),
(7, 6, '7625', 'Who''s the most experienced employee in the ''Catering and hospitality'' sector?'),
(7, 6, '7626', 'Most Experienced Employee''s experience in the ''Catering and hospitality'' sector'),
(7, 6, '7627', 'Most Experienced Employee''s experience in the ''Catering and hospitality'' sector'),
(7, 6, '7628', 'Most Experienced Employee''s experience in the ''Catering and hospitality'' sector'),
(7, 6, '7630AA', 'What standards do you intend to deliver within the ''Construction'' sector?'),
(7, 6, '7630A', 'How many starts does your organisation forecast in the ''Construction'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7630B', 'How many employees will deliver training in the ''Construction'' sector?'),
(7, 6, '7630', 'Who''s the most experienced employee in the ''Construction'' sector?'),
(7, 6, '7631', 'Most Experienced Employee''s experience in the ''Construction'' sector'),
(7, 6, '7632', 'Most Experienced Employee''s experience in the ''Construction'' sector'),
(7, 6, '7633', 'Most Experienced Employee''s experience in the ''Construction'' sector'),
(7, 6, '7635AA', 'What standards do you intend to deliver within the ''Creative and design'' sector?'),
(7, 6, '7635A', 'How many starts does your organisation forecast in the ''Creative and design'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7635B', 'How many employees will deliver training in the ''Creative and design'' sector?'),
(7, 6, '7635', 'Who''s the most experienced employee in the ''Creative and design'' sector?'),
(7, 6, '7636', 'Most Experienced Employee''s experience in the ''Creative and design'' sector'),
(7, 6, '7637', 'Most Experienced Employee''s experience in the ''Creative and design'' sector'),
(7, 6, '7638', 'Most Experienced Employee''s experience in the ''Creative and design'' sector'),
(7, 6, '7640AA', 'What standards do you intend to deliver within the ''Digital'' sector?'),
(7, 6, '7640A', 'How many starts does your organisation forecast in the ''Digital'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7640B', 'How many employees will deliver training in the ''Digital'' sector?'),
(7, 6, '7640', 'Who''s the most experienced employee in the ''Digital'' sector?'),
(7, 6, '7641', 'Most Experienced Employee''s experience in the ''Digital'' sector'),
(7, 6, '7642', 'Most Experienced Employee''s experience in the ''Digital'' sector'),
(7, 6, '7643', 'Most Experienced Employee''s experience in the ''Digital'' sector'),
(7, 6, '7645AA', 'What standards do you intend to deliver within the ''Education and childcare'' sector?'),
(7, 6, '7645A', 'How many starts does your organisation forecast in the ''Education and childcare'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7645B', 'How many employees will deliver training in the ''Education and childcare'' sector?'),
(7, 6, '7645', 'Who''s the most experienced employee in the ''Education and childcare'' sector?'),
(7, 6, '7646', 'Most Experienced Employee''s experience in the ''Education and childcare'' sector'),
(7, 6, '7647', 'Most Experienced Employee''s experience in the ''Education and childcare'' sector'),
(7, 6, '7648', 'Most Experienced Employee''s experience in the ''Education and childcare'' sector'),
(7, 6, '7650AA', 'What standards do you intend to deliver within the ''Engineering and manufacturing'' sector?'),
(7, 6, '7650A', 'How many starts does your organisation forecast in the ''Engineering and manufacturing'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7650B', 'How many employees will deliver training in the ''Engineering and manufacturing'' sector?'),
(7, 6, '7650', 'Who''s the most experienced employee in the ''Engineering and manufacturing'' sector?'),
(7, 6, '7651', 'Most Experienced Employee''s experience in the ''Engineering and manufacturing'' sector'),
(7, 6, '7652', 'Most Experienced Employee''s experience in the ''Engineering and manufacturing'' sector'),
(7, 6, '7653', 'Most Experienced Employee''s experience in the ''Engineering and manufacturing'' sector'),
(7, 6, '7655AA', 'What standards do you intend to deliver within the ''Hair and Beauty'' sector?'),
(7, 6, '7655A', 'How many starts does your organisation forecast in the ''Hair and Beauty'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7655', 'How many employees will deliver training in the ''Hair and Beauty'' sector?'),
(7, 6, '7655B', 'Who''s the most experienced employee in the ''Hair and Beauty'' sector?'),
(7, 6, '7656', 'Most Experienced Employee''s experience in the ''Hair and Beauty'' sector'),
(7, 6, '7657', 'Most Experienced Employee''s experience in the ''Hair and Beauty'' sector'),
(7, 6, '7658', 'Most Experienced Employee''s experience in the ''Hair and Beauty'' sector'),
(7, 6, '7660AA', 'What standards do you intend to deliver within the ''Health and science'' sector?'),
(7, 6, '7660A', 'How many starts does your organisation forecast in the ''Health and science'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7660B', 'How many employees will deliver training in the ''Health and science'' sector?'),
(7, 6, '7660', 'Who''s the most experienced employee in the ''Health and science'' sector?'),
(7, 6, '7661', 'Most Experienced Employee''s experience in the ''Health and science'' sector'),
(7, 6, '7662', 'Most Experienced Employee''s experience in the ''Health and science'' sector'),
(7, 6, '7663', 'Most Experienced Employee''s experience in the ''Health and science'' sector'),
(7, 6, '7665AA', 'What standards do you intend to deliver within the ''Legal, finance and accounting'' sector?'),
(7, 6, '7665A', 'How many starts does your organisation forecast in the ''Legal, finance and accounting'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7665B', 'How many employees will deliver training in the ''Legal, finance and accounting'' sector?'),
(7, 6, '7665', 'Who''s the most experienced employee in the ''Legal, finance and accounting'' sector?'),
(7, 6, '7666', 'Most Experienced Employee''s experience in the ''Legal, finance and accounting'' sector'),
(7, 6, '7667', 'Most Experienced Employee''s experience in the ''Legal, finance and accounting'' sector'),
(7, 6, '7668', 'Most Experienced Employee''s experience in the ''Legal, finance and accounting'' sector'),
(7, 6, '7670AA', 'What standards do you intend to deliver within the ''Protective services'' sector?'),
(7, 6, '7670A', 'How many starts does your organisation forecast in the ''Protective services'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7670B', 'How many employees will deliver training in the ''Protective services'' sector?'),
(7, 6, '7670', 'Who''s the most experienced employee in the ''Protective services'' sector?'),
(7, 6, '7671', 'Most Experienced Employee''s experience in the ''Protective services'' sector'),
(7, 6, '7672', 'Most Experienced Employee''s experience in the ''Protective services'' sector'),
(7, 6, '7673', 'Most Experienced Employee''s experience in the ''Protective services'' sector'),
(7, 6, '7675AA', 'What standards do you intend to deliver within the ''Sales, marketing and procurement'' sector?'),
(7, 6, '7675A', 'How many starts does your organisation forecast in the ''Sales, marketing and procurement'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7675B', 'How many employees will deliver training in the ''Sales, marketing and procurement'' sector?'),
(7, 6, '7675', 'Who''s the most experienced employee in the ''Sales, marketing and procurement'' sector?'),
(7, 6, '7676', 'Most Experienced Employee''s experience in the ''Sales, marketing and procurement'' sector'),
(7, 6, '7677', 'Most Experienced Employee''s experience in the ''Sales, marketing and procurement'' sector'),
(7, 6, '7678', 'Most Experienced Employee''s experience in the ''Sales, marketing and procurement'' sector'),
(7, 6, '7680AA', 'What standards do you intend to deliver within the ''Transport and logistics'' sector?'),
(7, 6, '7680A', 'How many starts does your organisation forecast in the ''Transport and logistics'' sector in the first 12 months of joining the RoATP?'),
(7, 6, '7680B', 'How many employees will deliver training in the ''Transport and logistics'' sector?'),
(7, 6, '7680', 'Who''s the most experienced employee in the ''Transport and logistics'' sector?'),
(7, 6, '7681', 'Most Experienced Employee''s experience in the ''Transport and logistics'' sector'),
(7, 6, '7682', 'Most Experienced Employee''s experience in the ''Transport and logistics'' sector'),
(7, 6, '7683', 'Most Experienced Employee''s experience in the ''Transport and logistics'' sector'),

(7, 7, '7700', 'Upload your organisation''s contract for services template with employers'),
(7, 7, '7710', 'Give an example of how your organisation used this policy to improve employee sector expertise'),
(7, 7, '7720', 'Give an example of how your organisation used this policy to maintain employee teaching and training knowledge'),

(8, 1, '8000', 'Evaluating apprenticeship training'),
(8, 1, '8010', 'Evaluating apprenticeship training'),

(8, 2, '8100', 'What is your organisation''s process for evaluating the quality of training delivered?'),
(8, 2, '8110', 'How has your organisation made improvements using its process for evaluating the quality of training delivered?'),

(8, 3, '8200', 'Does your organisation''s process for evaluating the quality of training delivered include apprenticeship training?'),
(8, 3, '8210', 'How will your organisation evaluate the quality of apprenticeship training?'),
(8, 3, '8220', 'How will your organisation evaluate the quality of apprenticeship training?'),
(8, 3, '8230', 'How will your organisation review its process for evaluating the quality of training delivered?'),

(8, 4, '8300', 'Does your organisation have systems and processes in place to collect apprenticeship data?'),
(8, 4, '8310', 'Does your organisation have the resources to submit Individualised Learner Record (ILR) data?'),
(8, 4, '8320', 'Who is the individual accountable for submitting ILR data for your organisation?'),

(9, 1, '9000', 'Do you have permission from everyone named in this application to use their details?'),
(9, 1, '9010', 'Have you checked with everyone named in this application that the details provided for them are accurate?'),
(9, 1, '9020', 'Do you have permission from your organisation to submit this application?'),
(9, 1, '10005', 'Before you submit your application'),

(9, 2, '9100', 'Do you understand that your organisation must develop and deliver apprenticeship training in line with the Institute for Apprenticeships and Technical Education''s ''quality statement''?'),
(9, 2, '100055', 'Before you submit your application'),

(9, 3, '9200', 'Do you understand that your organisation will not join the RoATP until it completes all post application tasks?'),
(9, 3, '10006', 'Before you submit your application');