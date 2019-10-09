set datestr=%date%
set result=%datestr:/=_%
exp LEGALTECH/LEGALTECH file=C:/inetpub/wwwroot/Dropbox/BackUp_DB/%result%LEGALTECH.dmp
xcopy C:\inetpub\wwwroot\Dropbox\BackUp_DB C:\BackUp\ /D /Y
exit