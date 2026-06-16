#define AppName "InovaConecta"
#define AppVersion "2.0"
#define AppPublisher "InovaFarma"
#define AppExeName "InovaConecta.exe"
#define SourceDir "InovaConecta\bin\Release"

[Setup]
AppId={{A3A25DE5-10FB-441F-BA05-B1A1E7159ABC}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppName}
OutputDir=Installer
OutputBaseFilename=InovaConecta_Setup_{#AppVersion}
SetupIconFile=C:\Users\jeang\source\repos\JeanGabrielSilva\InovaConecta-master\InovaConecta\img\Logo-InovaConecta.ico
; Ícone que aparece no Painel de Controle > Programas e Recursos
UninstallDisplayIcon={app}\img\Logo-InovaConecta.ico
UninstallDisplayName={#AppName}
VersionInfoVersion={#AppVersion}
VersionInfoCompany={#AppPublisher}
VersionInfoProductName={#AppName}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
; Requer .NET 4.7.2
MinVersion=6.1

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na Área de Trabalho"; GroupDescription: "Ícones adicionais:"; Flags: unchecked
Name: "startupicon"; Description: "Iniciar com o Windows"; GroupDescription: "Inicialização:"; Flags: unchecked

[Files]
; Executável principal e dependências
Source: "{#SourceDir}\InovaConecta.exe";        DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\InovaConecta.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#SourceDir}\Newtonsoft.Json.dll";     DestDir: "{app}"; Flags: ignoreversion

; NOTA: config.json e bancos_titulos.json NÃO são empacotados de propósito.
; O app cria esses arquivos automaticamente em runtime quando o usuário
; configura a instância. Empacotá-los espalharia dados/senhas do dev
; pra todas as máquinas instaladas (já aconteceu — não fazer).

; Imagens e ícones
Source: "InovaConecta\img\*"; DestDir: "{app}\img"; Flags: ignoreversion recursesubdirs

[Icons]
; Atalho no menu Iniciar
Name: "{group}\{#AppName}";           Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\img\Logo-InovaConecta.ico"
Name: "{group}\Desinstalar {#AppName}"; Filename: "{uninstallexe}"

; Atalho na Área de Trabalho (opcional)
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{app}\img\Logo-InovaConecta.ico"; Tasks: desktopicon

; Atalho na inicialização do Windows (opcional)
Name: "{autostartup}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: startupicon

[Run]
; Abre o app após instalar
Filename: "{app}\{#AppExeName}"; Description: "Abrir {#AppName}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Remove config ao desinstalar (opcional — comente se quiser preservar)
; Type: files; Name: "{app}\config.json"
; Type: files; Name: "{app}\bancos_titulos.json"
