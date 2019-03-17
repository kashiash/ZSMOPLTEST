  MEMBER('WssSignedXmlClarionTest.clw')

  PRAGMA('define(init_priority=>3)')

  MAP
    MODULE('WssSignedXmlClarionTest_BC0.CLW')
WssSignedXmlClarionTest_BC0:DctInit             PROCEDURE()
WssSignedXmlClarionTest_BC0:DctKill             PROCEDURE()
WssSignedXmlClarionTest_BC0:FilesInit           PROCEDURE()
    END
  END

DctInit PROCEDURE()
  CODE
  WssSignedXmlClarionTest_BC0:DctInit
  WssSignedXmlClarionTest_BC0:FilesInit


DctKill PROCEDURE()
  CODE
  WssSignedXmlClarionTest_BC0:DctKill

