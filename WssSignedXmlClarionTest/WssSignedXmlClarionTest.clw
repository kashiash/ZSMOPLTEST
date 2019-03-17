   PROGRAM


StringTheory:TemplateVersion equate('2.90')
WinEvent:TemplateVersion      equate('5.26')

   INCLUDE('ABERROR.INC'),ONCE
   INCLUDE('ABUTIL.INC'),ONCE
   INCLUDE('ERRORS.CLW'),ONCE
   INCLUDE('KEYCODES.CLW'),ONCE
   INCLUDE('ABFUZZY.INC'),ONCE
  include('StringTheory.Inc'),ONCE
    Include('WinEvent.Inc'),Once

   MAP
     MODULE('WSSSIGNEDXMLCLARIONTEST_BC.CLW')
DctInit     PROCEDURE                                      ! Initializes the dictionary definition module
DctKill     PROCEDURE                                      ! Kills the dictionary definition module
     END
!--- Application Global and Exported Procedure Definitions --------------------------------------------
     MODULE('WSSSIGNEDXMLCLARIONTEST001.CLW')
Main                   PROCEDURE   !Wizard Application for 
     END
      MODULE('Gabos.Ezla.dll')
     		Zaloguj(bstring, bstring),bstring,PASCAL, RAW
           	Wyloguj(bstring),bstring,PASCAL,RAW
             NadajMyIdSeji(),bstring,PASCAL, RAW
             PobierzListeBiezacychZlaLekarz(bstring),bstring,PASCAL,RAW
             PobierzMiejsceWykonywaniaZawodu(bstring),bstring,PASCAL,RAW
           	PobierzDaneLekarza(bstring),bstring,PASCAL,RAW
           	PobierzPlatnikowUbezpieczonego(bstring,bstring),bstring,PASCAL,RAW
           	PobierzListeZlaUbezpieczonego(bstring,bstring,bstring),bstring,PASCAL,RAW    
             PobierzDaneUbezpieczonego(bstring,bstring,bstring),bstring,PASCAL,RAW
             PobierzDanePlatnika(bstring,bstring,bstring,bstring ),bstring,PASCAL,RAW
             RezerwujSeriaNumerZla(bstring,bstring),bstring,PASCAL,RAW 
             PobierzLiterowyKodChoroby(bstring,bstring ,bstring,bstring,bstring),bstring,PASCAL,RAW
             PobierzCzlonkowRodzinyUbezpieczonego(bstring,bstring,bstring),bstring,PASCAL,RAW 
             SprawdzMozliwoscElektronizacji(bstring,bstring,bstring),bstring,PASCAL,RAW 
             SprawdzMozliwoscAnulowania(bstring,bstring,bstring),bstring,PASCAL,RAW 
             SprawdzMozliwoscUniewaznienia(bstring,bstring,bstring),bstring,PASCAL,RAW 
             PobierzSzczegolyZlaBiezace(bstring,bstring, bstring),bstring,PASCAL,RAW    
             !********
             WyslijDokumentyZla(bstring ,bstring ,bstring),bstring,PASCAL,RAW  
             WalidujDokumentyZla(bstring ,bstring ,bstring),bstring,PASCAL,RAW  
             !********
             PobierzDokument(bstring,bstring,bstring,bstring),bstring,PASCAL,RAW 
     	END
     
     MODULE('ZsmoplWssBuilder.dll')
     !	GetWssSignedXml(bstring,bstring, bstring),bstring,PASCAL,RAW 
     	GetWssSignedXml(bstring,bstring, bstring)bstring,PASCAL,RAW 
         GetWssSignedXmlX(bstring,bstring, bstring)bstring,PASCAL,RAW 
     		END
       MyOKToEndSessionHandler(long pLogoff),long,pascal
       MyEndSessionHandler(long pLogoff),pascal
   END

  include('StringTheory.Inc'),ONCE
glo:KEDU             CSTRING(100000)
SilentRunning        BYTE(0)                               ! Set true when application is running in 'silent mode'

!region File Declaration
!endregion

WE::MustClose       long
WE::CantCloseNow    long

FuzzyMatcher         FuzzyClass                            ! Global fuzzy matcher
GlobalErrorStatus    ErrorStatusClass,THREAD
GlobalErrors         ErrorClass                            ! Global error manager
INIMgr               INIClass                              ! Global non-volatile storage manager
GlobalRequest        BYTE(0),THREAD                        ! Set when a browse calls a form, to let it know action to perform
GlobalResponse       BYTE(0),THREAD                        ! Set to the response from the form
VCRRequest           LONG(0),THREAD                        ! Set to the request from the VCR buttons

Dictionary           CLASS,THREAD
Construct              PROCEDURE
Destruct               PROCEDURE
                     END


  CODE
  GlobalErrors.Init(GlobalErrorStatus)
  FuzzyMatcher.Init                                        ! Initilaize the browse 'fuzzy matcher'
  FuzzyMatcher.SetOption(MatchOption:NoCase, 1)            ! Configure case matching
  FuzzyMatcher.SetOption(MatchOption:WordOnly, 0)          ! Configure 'word only' matching
  INIMgr.Init('.\WssSignedXmlClarionTest.INI', NVD_INI)    ! Configure INIManager to use INI file
  DctInit()
    ds_SetOKToEndSessionHandler(address(MyOKToEndSessionHandler))
    ds_SetEndSessionHandler(address(MyEndSessionHandler))
  Main
  INIMgr.Update
  INIMgr.Kill                                              ! Destroy INI manager
  FuzzyMatcher.Kill                                        ! Destroy fuzzy matcher
    
! ------ winevent -------------------------------------------------------------------
MyOKToEndSessionHandler procedure(long pLogoff)
OKToEndSession    long(TRUE)
! Setting the return value OKToEndSession = FALSE
! will tell windows not to shutdown / logoff now.
! If parameter pLogoff = TRUE if the user is logging off.

  code
  return(OKToEndSession)

! ------ winevent -------------------------------------------------------------------
MyEndSessionHandler procedure(long pLogoff)
! If parameter pLogoff = TRUE if the user is logging off.

  code


Dictionary.Construct PROCEDURE

  CODE
  IF THREAD()<>1
     DctInit()
  END


Dictionary.Destruct PROCEDURE

  CODE
  DctKill()

