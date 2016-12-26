if ((Get-PSSnapin -Name Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue) -eq $null ) {
	Add-PSSnapin Microsoft.SharePoint.Powershell
}

$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2("C:\Certificates\root.cer")
New-SPTrustedRootAuthority -Name "CA" -Certificate $cert
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2("C:\Certificates\subroot.cer")
New-SPTrustedRootAuthority -Name "SubCA" -Certificate $cert
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2("C:\Certificates\pltmuep.erp.petrochina.cer")
New-SPTrustedRootAuthority -Name "STS" -Certificate $cert

$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn" -IncomingClaimTypeDisplayName "账号" -SameAsIncoming
$signInURL = "http://pltmuep.erp.petrochina/MCSWebApp/PassportService/SecurityTokenService/default.aspx"
$realm = "http://hr.erp.cnpc/_trust/"
$ap = New-SPTrustedIdentityTokenIssuer -Name "基础平台" -Description "基于基础平台提供的安全令牌服务的声明认证" -realm $realm -ClaimsMappings $map -SignInUrl $signInURL -IdentifierClaim $map.InputClaimType -ImportTrustCertificate $cert 
$ap.UseWReplyParameter = 1;
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "基础平台"
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/displayname");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/appcategory");
$ap.ClaimTypes.add("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/department");
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "基础平台"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/displayname" -IncomingClaimTypeDisplayName "姓名" -SameAsIncoming
Add-SPClaimTypeMapping -Identity $map -TrustedIdentityTokenIssuer $ap
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "基础平台"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress" -IncomingClaimTypeDisplayName "邮箱" -SameAsIncoming
Add-SPClaimTypeMapping -Identity $map -TrustedIdentityTokenIssuer $ap
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "基础平台"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/appcategory" -IncomingClaimTypeDisplayName "组织" -SameAsIncoming
Add-SPClaimTypeMapping -Identity $map -TrustedIdentityTokenIssuer $ap
$ap.update();

$ap = Get-SPTrustedIdentityTokenIssuer -Identity "基础平台"
$map = New-SPClaimTypeMapping -IncomingClaimType "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/department" -IncomingClaimTypeDisplayName "部门" -SameAsIncoming
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
New-SPClaimProvider -Type "PTR.Apps.Providers.SiteClaimProvider" -AssemblyName "PTR.Apps, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b9251a74853a87d6" -DisplayName "基础平台" -Description "基于基础平台提供的安全令牌服务的声明认证" 
New-SPClaimProvider -Type "Practices.SharePoint.Providers.SiteClaimProvider" -Scope (Get-SPWebApplication http://test)

$cp = Get-SPClaimProvider | where { $_.TypeName -eq "PTR.Apps.Providers.SiteClaimProvider" }

# Set-SPTrustedIdentityTokenIssuer
# https://technet.microsoft.com/en-us/library/ff607792.aspx
# https://social.technet.microsoft.com/Forums/sharepoint/en-US/b0323a2a-2ecd-4a22-925b-915d370bed7d/powershell-setsptrustedidentitytokenissuer-error?forum=sharepointadmin
# Set-SPTrustedIdentityTokenIssuer -Identity "基础平台-人力资源" -ClaimProvider f4dba9e5-6294-4a01-b81f-81b257a4b49e
# Set-SPTrustedIdentityTokenIssuer -Identity "基础平台-人力资源" -ImportTrustCertificate $cert


$ap = Get-SPTrustedIdentityTokenIssuer -Identity "基础平台"