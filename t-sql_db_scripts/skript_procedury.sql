/*
	funkce 2. c)
	Vytvoreni rezervace
*/
CREATE PROCEDURE vytvoreniRezervace
(
	@p_jmeno nvarchar(30),
	@p_prijmeni nvarchar(30),
	@p_pocet_osob int,
	@p_rezervovane_datum datetime,
	@p_pruvodce bit
)
AS
	DECLARE @pocet_osob int = @p_pocet_osob;
	DECLARE @je_volne_datum bit;
	DECLARE @newDatetime datetime = @p_rezervovane_datum;
	
	DECLARE @pocet_rezervaci int;
	DECLARE @datetimeToString varchar(50) = convert(varchar(25), @newDatetime, 120);

BEGIN TRANSACTION [Tran1]
	BEGIN TRY
		SELECT 
			@pocet_rezervaci = COUNT(*) 
		FROM 
			Rezervace 
		WHERE Zarezervovane_datum = @datetimeToString;

		IF @pocet_rezervaci > 0
		BEGIN
			SET @je_volne_datum = 0;
		END
		ELSE
		BEGIN
			SET @je_volne_datum = 1;
		END
		
		IF @je_volne_datum = 0		-- Jestli není volne datum a cas
		BEGIN
			SELECT 'Datum neni volne';		END		ELSE
		BEGIN
			print 'Datum je volne';
			WHILE @pocet_osob > 30
			BEGIN
				print '@pocet_osob = ' + CAST(@pocet_osob AS CHAR(20));
				IF @p_pruvodce = 1
				BEGIN
					print 'Inserting with p_pruvodce = ' + CAST(@p_pruvodce AS CHAR(1));
					INSERT INTO Rezervace (Jmeno, Prijmeni, Pocet_osob, Zarezervovane_datum, datum_vytvoreni, Pruvodce_pID)
					VALUES (@p_jmeno, @p_prijmeni, 30, @newDatetime, CURRENT_TIMESTAMP, 1);
					print 'Konec insertu 1';
				END
				ELSE
				BEGIN
					--print 'Inserting with without p_pruvodce = ' + @p_pruvodce;
					print 'Inserting with without p_pruvodce = ' + CAST(@p_pruvodce AS CHAR(1));
					INSERT INTO Rezervace (Jmeno, Prijmeni, Pocet_osob, Zarezervovane_datum, Datum_vytvoreni, Pruvodce_pID)
					VALUES (@p_jmeno, @p_prijmeni, 30, @newDatetime, CURRENT_TIMESTAMP, 1);
					print 'Konec insertu 2';
				END
				SET @pocet_osob = @pocet_osob - 30;	   				  -- zmenšíme poèet osob o 30
				SET @newDatetime = DATEADD(MINUTE, 10, @newDatetime); -- $newDatetime += 10 minut
				print '@pocet_osob '  + CAST(@pocet_osob AS CHAR(10));
				print '@newDatetime '  + CAST(@newDatetime AS CHAR(10));
			END
			
			print 'While cycle ends';
			
			IF @pocet_osob > 0
			BEGIN
				print 'Second Statement: ';
				IF @p_pruvodce = 1
				BEGIN
					INSERT INTO Rezervace (Jmeno, Prijmeni, Pocet_osob, Zarezervovane_datum, Datum_vytvoreni, Pruvodce_pID)
					VALUES (@p_jmeno, @p_prijmeni, @pocet_osob, @newDatetime, CURRENT_TIMESTAMP, 1);
				END
				ELSE
				BEGIN
					INSERT INTO Rezervace (Jmeno, Prijmeni, Pocet_osob, Zarezervovane_datum, Datum_vytvoreni, Pruvodce_pID)
					VALUES (@p_jmeno, @p_prijmeni, @pocet_osob, @newDatetime, CURRENT_TIMESTAMP, 1);
				END
			END
		END
		COMMIT
	END TRY

BEGIN CATCH
	print 'CATCH BLOCK!'
	ROLLBACK TRANSACTION [Tran1];
END CATCH



/*
	funkce 2. e)
	Zmena rezervace
*/
CREATE PROCEDURE zmenaRezervace
(
	@p_rID int,
	@p_noveDatum datetime
)
AS
	DECLARE @pocet_zmen int;
	DECLARE @puvodni_datum_rezervace datetime;
	DECLARE @report_text varchar(120) = '';
	
	DECLARE @pocet_zaznamu_na_datum int;

BEGIN TRANSACTION [Tran2]
	BEGIN TRY
		SELECT 
			@pocet_zmen = COUNT(*) 
		FROM 
			zmena_rezervace WHERE rezervace_rid = @p_rID;
		
		IF @pocet_zmen < 3
		BEGIN
			SELECT
				@puvodni_datum_rezervace = Zarezervovane_datum
			FROM
				Rezervace 
			WHERE 
				rID = @p_rID
				
			SELECT
				@pocet_zaznamu_na_datum = COUNT(*)
			FROM
				 Rezervace
			WHERE
				rid = @p_rID
				AND Zarezervovane_datum = @p_noveDatum;
			
			IF @pocet_zaznamu_na_datum = 0
			BEGIN
				INSERT INTO Zmena_rezervace (Datum_puvodni_rezervace, Datum_nove_rezervace, Datum_zmeny, Rezervace_rID)
				VALUES (@puvodni_datum_rezervace, @p_noveDatum, CURRENT_TIMESTAMP, @p_rID)
				
				UPDATE Rezervace SET Zarezervovane_datum = @p_noveDatum WHERE rID = @p_rID;
			END
			ELSE
			BEGIN
				SET @report_text = 'Datum rezervace je obsazene. ';
			END
			
		END
		ELSE
		BEGIN
			SET @report_text = 'Rezervaci jiz vicekrat nelze zmenit (max 3 zmeny)!';
		END
		
		print @report_text;
		
	COMMIT
	END TRY
	
BEGIN CATCH
	print 'CATCH BLOCK!'
	ROLLBACK TRANSACTION [Tran2];
END CATCH



/*
	funkce 3. c)
	kontrola vytvorene vystavy
*/
CREATE PROCEDURE kontrolaVytvoreneVystavy
(
	@p_vID int
)
AS
	DECLARE @archeolog_email varchar(120);
	DECLARE @pocet_stazenych_artefaktu int = 0;
	
BEGIN TRANSACTION [Tran3]
	BEGIN TRY
	
	DECLARE 
    @artefakt_id_for_delete int

	DECLARE cursor_artefakt CURSOR
	FOR SELECT 
			Artefakt_aID
		FROM 
			Vystavene_artefakty
			LEFT JOIN Artefakt ON Artefakt_aID = Artefakt.aID
		WHERE 
			DATEDIFF(day, COALESCE(Artefakt.datum_posledni_kontroly, CONVERT(datetime, '2000-01-01')), CURRENT_TIMESTAMP) > 30
			AND vystava_vid = @p_vID;
		
	OPEN cursor_artefakt;

	FETCH NEXT FROM cursor_artefakt INTO 
		@artefakt_id_for_delete;

	WHILE @@FETCH_STATUS = 0
		BEGIN
			PRINT 'Stahuji artefakt s danym id z muzea do skladu: ' + CAST(@artefakt_id_for_delete AS varchar);
			-- smaze zaznam z Vystavene_artefakty podle ID artefaktu
			DELETE FROM vystavene_artefakty WHERE artefakt_aid = @artefakt_id_for_delete;
			SET @pocet_stazenych_artefaktu = @pocet_stazenych_artefaktu + 1;
			-- vybereme nahodneho naseho arecheologa a ulozime jeho email
			SELECT 
			TOP 1 @archeolog_email = Email 
			FROM 
				Archeolog 
			ORDER BY NEWID();
			-- send email
			EXEC SendEmail_kontrola_artefaktu @archeolog_email, @artefakt_id_for_delete;
		
			FETCH NEXT FROM cursor_artefakt INTO 
				@artefakt_id_for_delete;
		END;

	CLOSE cursor_artefakt;
	DEALLOCATE cursor_artefakt;

	COMMIT
	SELECT @pocet_stazenych_artefaktu as StazeneArtefakty
	END TRY
	
BEGIN CATCH
	print 'CATCH BLOCK!'
	ROLLBACK TRANSACTION [Tran3];
END CATCH



/*	funkce 3. d)
	send email procedura
*/
CREATE PROCEDURE SendEmail_kontrola_artefaktu (
	@p_archeolog_email varchar(60),
	@p_artefakt_id int
)
AS
BEGIN
	DECLARE @info_text varchar(250) = 'Odesilam email archeologovi s emailem: ' + @p_archeolog_email + 
	', aby provedl kontrolu artefaktu s id: ' + CAST(@p_artefakt_id AS varchar);
	
	SELECT @info_text AS Email;
	PRINT @info_text;
END



