Rem {VERSION..VER_DX_NT_1_0}
Rem Deinstallation: Place Name of Paket here
Rem Hersteller: Place Name of maker here
Rem Version: Place Version of number here
Rem Datum: 03.09.2013 18:53:22
Rem Autor: DX-Union Paket Wizard 7.1.4.44

Option Explicit

Dim sLastReport As String
Dim iLastReportID As Integer

Const sSetupDir = """{ENVIRONMENT..TEMP}\{package..name}\"""
Const sInstallationskommando = """{ENVIRONMENT..TEMP}\{package..name}\DEINTALLNAMEHERE.exe"""
Const sInstallationsparameter = "parameter switchs"

Sub Main

	On Error GoTo Fehler

	EnableReportDXDist
	ReportDXDist (1, "Starte DeInstallation")

	Dim ExitCode As Integer
	ExitCode = KopiereMedium

	If ExitCode <> 0 Then
		Exit Sub
	End If

	Dim lRet As Long

	If IsFileExisting(sInstallationskommando) Then
		If sInstallationskommando <> "" Then
			lRet = wexec32(sInstallationskommando, sInstallationsparameter, SW_NORMAL)
		End If

		If lRet <> 0 Then
			FehlerSetzen("Installation", "Fehler während der Deinstallation. Der Rückgabewert ist: " & lRet)
        Else
            ReportDXDist (1, "Deinstallation abgeschlossen")
		End If
	Else
		FehlerSetzen("Die Datei wurde nicht gefunden.", sInstallationskommando)
	End If

	LoescheMedium

	ReportDXDist (1, "DeInstallation abgeschlossen")

	Goto EndeOK:

	Fehler:
	FehlerSetzen("Fehler", "Es ist ein Fehler aufgetreten: " + err.description + " " + err.source + " " + str(err.number))


	EndeOK:

End Sub

Function KopiereMedium

	'Kopiere das Softwaremedium auf den Client
	On Error GoTo Fehler
	Dim oDXCopyObj As Object
	Set oDXCopyObj = CreateObject("DxftClient.DxftClient.1")

	EnableReportDXDist
	ReportDXDist (1, "Kopiere Herstellerdateien auf den Client")
	ReportDXDist (1, "Evtl. alte Setup-Dateien entfernen")

	'Evtl. alte Setup-Daten entfernen
	rm32 ("-q -r -f", sSetupDir)

	KopiereMedium = 0
	If ForcePkgCD("{package..name}") = 0 Then
		oDXCopyObj.DxCopy("-v -f -r ""{dynamic..depotrechner}:{dynamic..verzeichnis}/pkg""" + " " + sSetupDir)
	    ReportDXDist (1, "Kopieren abgeschlossen")
	Else
		'Der Benutzer kann das Medium bei CD Installation nicht zur Verfügung stellen
		KopiereMedium = 1
		FehlerSetzen("Fehler", "Der Benutzer kann das Medium bei CD Installation nicht zur Verfügung stellen.")
	    ReportDXDist (1, "Kopieren fehlerhaft")
	End If

	Set oDXCopyObj = Nothing

	GoTo EndeOK:

	Fehler:
	FehlerSetzen("Fehler", "Es ist ein Fehler aufgetreten: " + Err.Description + " " + Err.Source + " " + Str(Err.Number))
	KopiereMedium = 1

	EndeOK:


End Function

Sub LoescheMedium

	'Lösche das Softwaremedium wieder vom Client
	On Error GoTo Fehler
	EnableReportDXDist

	'Lösche das Installationsmedium
	rm32 ("-q -r -f", sSetupDir)

	GoTo EndeOK:

	Fehler:
	FehlerSetzen("Fehler", "Es ist ein Fehler aufgetreten: " + Err.Description + " " + Err.Source + " " + Str(Err.Number))

	EndeOK:

End Sub

Function FehlerSetzen(cmd$, errmsg$)

	' Message-Ids
	Dim QP_EXEC_ERR_EXECUTING
	Dim QP_EXEC_ERR_EXECCMD
	QP_EXEC_ERR_EXECUTING = 17460176
	QP_EXEC_ERR_EXECCMD = 17725734

	' Initialisierung von PET
	Dim oPetComHelper As Object
	Set oPetComHelper = CreateObject("PetcomHelper.PetcomHelper.1")
	oPetComHelper.UnloadRegistry()
	oPetComHelper.LoadRegistry("DX-Union", "DX-Script", "DX-Union")

	Dim oPetComError As Object
	Set oPetComError = CreateObject("PetcomError.PetcomError.1")

	' Fehlermeldung auf Fehlerstack ablegen
	oPetComError.Error(0, QP_EXEC_ERR_EXECUTING, cmd$)
	oPetComError.Error(0, QP_EXEC_ERR_EXECCMD , errmsg$)

	' Fehlermeldungsbox mit den Meldungen vom Fehlerstack anzeigen
	' oPetComError.MessageBox()

End Function

Sub EnableReportDXDist

	' Report-File auf ausreichende Größe setzen
	Dim obDxRegistry As Object
	Set obDxRegistry = CreateObject("DxRegistry.DxRegistry.1")

	obDxRegistry.sKey = "HKEY_LOCAL_MACHINE"
	obDxRegistry.sSubkey = "SOFTWARE\Dr. Materna GmbH\Tracing\DX-UNION\DX-Distribute"
	obDxRegistry.Write 2, "ReportLevel"
	obDxRegistry.Write &H500000, "ReportMaxSize"

End Sub

Function ReportDXDist(ByVal ReportID%, ByVal ReportMsg$)

	Dim oPetComHelperRep As Object
	Dim oPetComReport As Object
	Dim iCompIDRep

	' PET-Report fuer DX-Dist initialisieren
	Set oPetComHelperRep = CreateObject("PetcomHelper.PetcomHelper.1")
	oPetComHelperRep.UnloadRegistry()

	'Beispiel-Einstellungen für Report nach DX-Script
	'oPetComHelperRep.LoadRegistry("DX-Union", "DX-Script", "DX-Union")
	'iCompIDRep = 267        ' DX-Script

	'Report für Komponente DX-Distribute erzeugen
	oPetComHelperRep.LoadRegistry("DX-Union", "DX-Distribute", "DX-Union")
	iCompIDRep = 259        ' DX-Distribute

	Set oPetComReport = CreateObject("PetcomReport.PetcomReport.1")
	oPetComReport.Report( iCompIDRep, ReportID%, "SWP '{package..name}'- " & ReportMsg$ )

	iLastReportID = ReportID%
	sLastReport   = ReportMsg$

	' Aufräumen
	oPetComHelperRep.UnloadRegistry()
	Set oPetComHelperRep = Nothing
	Set oPetComReport = Nothing

End Function

Function isFileExisting(path As String) As Boolean
	Dim obFso
	Set obFso = CreateObject("Scripting.FileSystemObject")
	isFileExisting = obFso.FileExists( unQuote(path) )
	Set obFso = Nothing
End Function

Function unQuote(source as String) As String
	Dim result as String
	Dim i
	For i = 1 to len(source)
		If mid(source,i,1) <> """" Then result = result + mid(source,i,1)
	Next i
	unQuote = result
End Function

