#Set-location "c:\root"            
[System.Reflection.Assembly]::Load("System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")            
$publish = New-Object System.EnterpriseServices.Internal.Publish            
$publish.GacInstall("c:\root\PTR.Apps.dll")
#$publish.GacInstall("c:\root\PTR.Portal.dll")
iisreset