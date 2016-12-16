if ((Get-PSSnapin -Name Microsoft.SharePoint.PowerShell -ErrorAction SilentlyContinue) -eq $null ) {
	Add-PSSnapin Microsoft.SharePoint.Powershell
}

# New-SPClaimProvider
# https://technet.microsoft.com/en-us/library/ff607616.aspx
# Get-SPClaimProvider
# https://technet.microsoft.com/en-us/library/ff607629.aspx
New-SPClaimProvider -Type "PTR.Apps.Providers.SiteClaimProvider" -AssemblyName "Practices.SharePoint.Fabric, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a3abb3c7af314c99" -DisplayName "员工编号" -Description "基础平台" 
New-SPClaimProvider -Type "Practices.SharePoint.Providers.SiteClaimProvider" -Scope (Get-SPWebApplication http://test)

$cp = Get-SPClaimProvider | where { $_.TypeName -eq "PTR.Apps.Providers.SiteClaimProvider" }

# Set-SPTrustedIdentityTokenIssuer
# https://technet.microsoft.com/en-us/library/ff607792.aspx
# https://social.technet.microsoft.com/Forums/sharepoint/en-US/b0323a2a-2ecd-4a22-925b-915d370bed7d/powershell-setsptrustedidentitytokenissuer-error?forum=sharepointadmin
# Set-SPTrustedIdentityTokenIssuer -Identity "基础平台-人力资源" -ClaimProvider f4dba9e5-6294-4a01-b81f-81b257a4b49e
# Set-SPTrustedIdentityTokenIssuer -Identity "基础平台-人力资源" -ImportTrustCertificate $cert


$ap = Get-SPTrustedIdentityTokenIssuer -Identity "基础平台-人力资源"