if ((Get-PSSnapin -Name Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue) -eq $null ) {
	Add-PSSnapin Microsoft.SharePoint.Powershell
}

$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2("C:\Certificates\root.cer")
New-SPTrustedRootAuthority -Name "CA" -Certificate $cert
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2("C:\Certificates\subroot.cer")
New-SPTrustedRootAuthority -Name "SubCA" -Certificate $cert
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2("C:\Certificates\pltmuep.erp.petrochina.cer")
New-SPTrustedRootAuthority -Name "STS" -Certificate $cert

$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn" -IncomingClaimTypeDisplayName "�˺�" -SameAsIncoming
$signInURL = "http://pltmuep.erp.petrochina/MCSWebApp/PassportService/SecurityTokenService/default.aspx"
$realm = "http://hr.erp.cnpc/_trust/"
$ap = New-SPTrustedIdentityTokenIssuer -Name "����ƽ̨" -Description "���ڻ���ƽ̨�ṩ�İ�ȫ���Ʒ����������֤" -realm $realm -ClaimsMappings $map -SignInUrl $signInURL -IdentifierClaim $map.InputClaimType -ImportTrustCertificate $cert 
$ap.UseWReplyParameter = 1;
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨"
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/displayname");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/appcategory");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/department");
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/displayname" -IncomingClaimTypeDisplayName "����" -SameAsIncoming
Add-SPClaimTypeMapping -Identity $map -TrustedIdentityTokenIssuer $ap
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" -IncomingClaimTypeDisplayName "����" -SameAsIncoming
Add-SPClaimTypeMapping -Identity $map -TrustedIdentityTokenIssuer $ap
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/appcategory" -IncomingClaimTypeDisplayName "��֯" -SameAsIncoming
Add-SPClaimTypeMapping -Identity $map -TrustedIdentityTokenIssuer $ap
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/department" -IncomingClaimTypeDisplayName "����" -SameAsIncoming
Add-SPClaimTypeMapping -Identity $map -TrustedIdentityTokenIssuer $ap
$ap.update();

$uri = new-object System.Uri("http://hr.erp.cnpc/")
$ap.ProviderRealms.Add($uri, "http://hr.erp.cnpc/_trust/")
$ap.update();

# New-SPClaimProvider
# https://technet.microsoft.com/en-us/library/ff607616.aspx
# Get-SPClaimProvider
# https://technet.microsoft.com/en-us/library/ff607629.aspx
# Set-SPClaimProvider
# https://technet.microsoft.com/en-us/library/ff607918.aspx
New-SPClaimProvider -Type "PTR.Apps.Providers.SiteClaimProvider" -AssemblyName "PTR.Apps, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b9251a74853a87d6" -DisplayName "����ƽ̨" -Description "���ڻ���ƽ̨�ṩ�İ�ȫ���Ʒ����������֤" 
New-SPClaimProvider -Type "Practices.SharePoint.Providers.SiteClaimProvider" -Scope (Get-SPWebApplication http://test)

$cp = Get-SPClaimProvider | where { $_.TypeName -eq "PTR.Apps.Providers.SiteClaimProvider" }

# Set-SPTrustedIdentityTokenIssuer
# https://technet.microsoft.com/en-us/library/ff607792.aspx
# https://social.technet.microsoft.com/Forums/sharepoint/en-US/b0323a2a-2ecd-4a22-925b-915d370bed7d/powershell-setsptrustedidentitytokenissuer-error?forum=sharepointadmin
# Set-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨-������Դ" -ClaimProvider f4dba9e5-6294-4a01-b81f-81b257a4b49e
# Set-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨-������Դ" -ImportTrustCertificate $cert


$ap = Get-SPTrustedIdentityTokenIssuer -Identity "����ƽ̨"