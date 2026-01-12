-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- [fabioribeiroaz-producao].dbo.[_versaoBD] definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.[_versaoBD];

CREATE TABLE [fabioribeiroaz-producao].dbo.[_versaoBD] (
	id int IDENTITY(1,1) NOT NULL,
	arquivo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataAplicacao datetime DEFAULT getdate() NOT NULL,
	CONSTRAINT PK__versaoBD PRIMARY KEY (id)
);


-- [fabioribeiroaz-producao].dbo.advAgeTiposCompromissos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advAgeTiposCompromissos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advAgeTiposCompromissos (
	idTipoCompromisso int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	recebimentoProcesso bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_advAgeTiposCompromissos PRIMARY KEY (idTipoCompromisso)
);


-- [fabioribeiroaz-producao].dbo.advAgeTiposTarefas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advAgeTiposTarefas;

CREATE TABLE [fabioribeiroaz-producao].dbo.advAgeTiposTarefas (
	idTipoTarefa int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	agendada bit DEFAULT 0 NOT NULL,
	pauta bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_advAgeTiposTarefas PRIMARY KEY (idTipoTarefa)
);
 CREATE NONCLUSTERED INDEX IX_advAgeTiposTarefas_pauta ON fabioribeiroaz-producao.dbo.advAgeTiposTarefas (  pauta ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advCliBairros definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliBairros;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliBairros (
	idBairro int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	cidade varchar(250) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT NULL NULL,
	estado varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT NULL NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK__advCliBa__86B592A14DA22A43 PRIMARY KEY (idBairro)
);


-- [fabioribeiroaz-producao].dbo.advCliCargos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliCargos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliCargos (
	idCargo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advCliCargos PRIMARY KEY (idCargo)
);


-- [fabioribeiroaz-producao].dbo.advCliComoChegou definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliComoChegou;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliComoChegou (
	idComoChegou int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advCliComoChegou PRIMARY KEY (idComoChegou)
);


-- [fabioribeiroaz-producao].dbo.advCliGrupos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliGrupos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliGrupos (
	idGrupo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advCliGrupos PRIMARY KEY (idGrupo)
);


-- [fabioribeiroaz-producao].dbo.advCliLocaisAtendido definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliLocaisAtendido;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliLocaisAtendido (
	idLocalAtendido int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advCliLocaisAtendido PRIMARY KEY (idLocalAtendido)
);


-- [fabioribeiroaz-producao].dbo.advCliLog definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliLog;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliLog (
	idLog int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	idUsuario int NOT NULL,
	acao int NOT NULL,
	idArea int NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idResponsavel int NULL,
	dataAgendamento datetime NULL,
	inssIdTipoBeneficio int NULL,
	CONSTRAINT PK_advCliLog PRIMARY KEY (idLog)
);


-- [fabioribeiroaz-producao].dbo.advCliPrioridades definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliPrioridades;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliPrioridades (
	idPrioridade int IDENTITY(1,1) NOT NULL,
	titulo varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cor varchar(6) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advCliPrioridades_idPrioridade PRIMARY KEY (idPrioridade)
);
 CREATE NONCLUSTERED INDEX IX_advPrioridades_ativo ON fabioribeiroaz-producao.dbo.advCliPrioridades (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advCliSituacoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliSituacoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliSituacoes (
	idSituacao int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advCliSituacoes PRIMARY KEY (idSituacao)
);


-- [fabioribeiroaz-producao].dbo.advCliTiposArquivos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliTiposArquivos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliTiposArquivos (
	idTipoArquivo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	checklist bit DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	pasta varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_advCliTiposArquivos PRIMARY KEY (idTipoArquivo)
);


-- [fabioribeiroaz-producao].dbo.advCliTiposHistoricos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCliTiposHistoricos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCliTiposHistoricos (
	idTipoHistorico int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advCliTiposHistoricos PRIMARY KEY (idTipoHistorico)
);


-- [fabioribeiroaz-producao].dbo.advClientesConvertidos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesConvertidos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesConvertidos (
	idRegistro int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	[data] smalldatetime NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	convertidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	CONSTRAINT PK_advClientesConvertidos PRIMARY KEY (idRegistro)
);


-- [fabioribeiroaz-producao].dbo.advClientesINSSStatus definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesINSSStatus;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesINSSStatus (
	idStatus int IDENTITY(1,1) NOT NULL,
	titulo varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK__advClien__01936F74AF543A56 PRIMARY KEY (idStatus)
);


-- [fabioribeiroaz-producao].dbo.advClientesModelos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesModelos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesModelos (
	idModelo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	conteudo text COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advClientesModelos_idModelo PRIMARY KEY (idModelo)
);


-- [fabioribeiroaz-producao].dbo.advClientes_bkp definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientes_bkp;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientes_bkp (
	idCliente int IDENTITY(1,1) NOT NULL,
	apelido varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	idGrupo int NULL,
	idSituacao int NULL,
	nome varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	email varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelular varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelularObs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixo varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixoObs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataNascimento smalldatetime NULL,
	cpf varchar(14) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	rg varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ctps varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	endereco varchar(75) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	numero varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	complemento varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bairro varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cep varchar(9) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	estado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataIngresso smalldatetime NULL,
	observacoes varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit NOT NULL,
	tsInclusao datetime NOT NULL,
	tsAlteracao datetime NULL,
	naturalEstado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	naturalCidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nomeDaMae varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dib bit NOT NULL,
	dibData smalldatetime NULL,
	dibIdTipoBeneficio int NULL,
	idCargo int NULL,
	telCelular2 varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelular2Obs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixo2 varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixo2Obs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cnpj varchar(18) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ie varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idFornecedor int NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	inssAgendado bit NOT NULL,
	inssData smalldatetime NULL,
	inssIdTipoBeneficio int NULL,
	inssIdPosto int NULL,
	inssResultado varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	prospect bit NOT NULL,
	idLocalAtendido int NULL,
	whatsapp bit NULL,
	pastaFTP varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	inssResponsavel int NULL,
	responsavelPendencia int NULL,
	comoChegou int NULL,
	inssProtocolo varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	inssTsInclusao datetime NULL,
	inssIdUsuarioInclusao int NULL,
	foto varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	followBloqueadoAte smalldatetime NULL,
	falecido bit NOT NULL,
	senhaINSSDigital varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idPrioridade int NULL,
	instagram varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	rgOrgaoExp varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nacionalidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	estadocivil varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dcb bit NULL,
	dcbData smalldatetime NULL
);


-- [fabioribeiroaz-producao].dbo.advFornecedores definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advFornecedores;

CREATE TABLE [fabioribeiroaz-producao].dbo.advFornecedores (
	idFornecedor int IDENTITY(1,1) NOT NULL,
	apelido varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	nome varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	email varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelular varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelularObs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixo varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixoObs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	endereco varchar(75) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	numero varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	complemento varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bairro varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cep varchar(9) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	estado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	observacoes varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	parceiroEmProcesso bit DEFAULT 0 NOT NULL,
	parceiroEmProcessoPerc decimal(5,2) DEFAULT 0 NOT NULL,
	idProfissional int NULL,
	foto varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_advFornecedores PRIMARY KEY (idFornecedor)
);


-- [fabioribeiroaz-producao].dbo.advPautaObs definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPautaObs;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPautaObs (
	idPautaObs int IDENTITY(1,1) NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	id int NULL,
	idTipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	observacao varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	CONSTRAINT PK_advPautaObs PRIMARY KEY (idPautaObs)
);


-- [fabioribeiroaz-producao].dbo.advPostosINSS definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPostosINSS;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPostosINSS (
	idPosto int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advPostosINSS PRIMARY KEY (idPosto)
);


-- [fabioribeiroaz-producao].dbo.advPreArquivosStatus definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreArquivosStatus;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreArquivosStatus (
	idStatus int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advPreArquivosStatus PRIMARY KEY (idStatus)
);


-- [fabioribeiroaz-producao].dbo.advPreCheckListsGrupos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreCheckListsGrupos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreCheckListsGrupos (
	idGrupo int IDENTITY(1,1) NOT NULL,
	titulo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advPreCheckListsGrupos PRIMARY KEY (idGrupo)
);


-- [fabioribeiroaz-producao].dbo.advPreLogStatus definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreLogStatus;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreLogStatus (
	idLog int IDENTITY(1,1) NOT NULL,
	idProcesso int NOT NULL,
	idStatus int NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	conversao bit DEFAULT 0 NOT NULL,
	tsConversao datetime NULL,
	perdido bit DEFAULT 0 NOT NULL,
	tsPerdido datetime NULL,
	diasCorridosDoAnterior int DEFAULT 0 NOT NULL,
	usuario varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_advPreLogStatus PRIMARY KEY (idLog)
);


-- [fabioribeiroaz-producao].dbo.advPreMetas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreMetas;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreMetas (
	idMeta int IDENTITY(1,1) NOT NULL,
	tipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idResponsavel int NULL,
	idEscritorio int NULL,
	qtde int DEFAULT 0 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	CONSTRAINT PK_advPreMetas PRIMARY KEY (idMeta)
);


-- [fabioribeiroaz-producao].dbo.advPreMotivosPerda definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreMotivosPerda;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreMotivosPerda (
	idMotivo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advPreMotivosPerda PRIMARY KEY (idMotivo)
);


-- [fabioribeiroaz-producao].dbo.advPreOrigens definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreOrigens;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreOrigens (
	idOrigem int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advPreOrigens PRIMARY KEY (idOrigem)
);


-- [fabioribeiroaz-producao].dbo.advPreProcessosCheckLists definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreProcessosCheckLists;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreProcessosCheckLists (
	idPreCheckList int IDENTITY(1,1) NOT NULL,
	idProcesso int NULL,
	idGrupo int NULL,
	idCheckList int NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	grupo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	item varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	concluido bit DEFAULT 0 NOT NULL,
	tsConclusao smalldatetime NULL,
	idResponsavel int NULL,
	ordem int DEFAULT 1 NOT NULL,
	CONSTRAINT PK_advPreProcessosCheckLists PRIMARY KEY (idPreCheckList)
);


-- [fabioribeiroaz-producao].dbo.advPreStatusTipos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreStatusTipos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreStatusTipos (
	idTipo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advPreStatusTipos_idTipo PRIMARY KEY (idTipo)
);


-- [fabioribeiroaz-producao].dbo.advProEscritorios definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProEscritorios;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProEscritorios (
	idEscritorio int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idCentroCusto int NULL,
	CONSTRAINT PK_advProEscritorios PRIMARY KEY (idEscritorio)
);


-- [fabioribeiroaz-producao].dbo.advProFases definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProFases;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProFases (
	idFase int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProFases PRIMARY KEY (idFase)
);


-- [fabioribeiroaz-producao].dbo.advProInstancias definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProInstancias;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProInstancias (
	idInstancia int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProInstancias PRIMARY KEY (idInstancia)
);


-- [fabioribeiroaz-producao].dbo.advProMeritos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProMeritos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProMeritos (
	idMerito int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	beneficioINSS bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_advProMeritos PRIMARY KEY (idMerito)
);


-- [fabioribeiroaz-producao].dbo.advProNaturezas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProNaturezas;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProNaturezas (
	idNatureza int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	mostraHistoricoNumeros bit DEFAULT 0 NOT NULL,
	recebeAcordo bit DEFAULT 0 NOT NULL,
	recebeRPV bit DEFAULT 0 NOT NULL,
	recebePrecatorio bit DEFAULT 0 NOT NULL,
	recebeAlvara bit DEFAULT 0 NOT NULL,
	idArea int NULL,
	CONSTRAINT PK_advProNaturezas PRIMARY KEY (idNatureza)
);


-- [fabioribeiroaz-producao].dbo.advProOrgaos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProOrgaos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProOrgaos (
	idOrgao int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProOrgaos PRIMARY KEY (idOrgao)
);


-- [fabioribeiroaz-producao].dbo.advProProbabilidades definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProProbabilidades;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProProbabilidades (
	idProbabilidade int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProProbabilidades PRIMARY KEY (idProbabilidade)
);


-- [fabioribeiroaz-producao].dbo.advProRelevancias definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProRelevancias;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProRelevancias (
	idRelevancia int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProRelevancias PRIMARY KEY (idRelevancia)
);


-- [fabioribeiroaz-producao].dbo.advProSentencas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProSentencas;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProSentencas (
	idSentenca int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProSentencas PRIMARY KEY (idSentenca)
);


-- [fabioribeiroaz-producao].dbo.advProStatus definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProStatus;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProStatus (
	idStatus int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProStatus PRIMARY KEY (idStatus)
);


-- [fabioribeiroaz-producao].dbo.advProTipos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProTipos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProTipos (
	idTipo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProTipos PRIMARY KEY (idTipo)
);


-- [fabioribeiroaz-producao].dbo.advProVaras definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProVaras;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProVaras (
	idVara int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProVaras PRIMARY KEY (idVara)
);


-- [fabioribeiroaz-producao].dbo.advTarefasAtualizacoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advTarefasAtualizacoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.advTarefasAtualizacoes (
	idAtualizacaoTarefa int IDENTITY(1,1) NOT NULL,
	idTarefa int NULL,
	idCompromisso int NULL,
	campo varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsAlteracao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	dadoAnterior varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idUsuario int NULL,
	CONSTRAINT PK_advTarefasAtualizacoes PRIMARY KEY (idAtualizacaoTarefa)
);
 CREATE NONCLUSTERED INDEX nci_msft_1_advTarefasAtualizacoes_5C8B7A46B0C9D2243B91AEF470702758 ON fabioribeiroaz-producao.dbo.advTarefasAtualizacoes (  idTarefa ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advVerTipos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advVerTipos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advVerTipos (
	idTipo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advVerTipos PRIMARY KEY (idTipo)
);


-- [fabioribeiroaz-producao].dbo.autoFTP definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.autoFTP;

CREATE TABLE [fabioribeiroaz-producao].dbo.autoFTP (
	id int IDENTITY(1,1) NOT NULL,
	arquivo varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	processado bit DEFAULT 0 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	CONSTRAINT PK_autoFTP PRIMARY KEY (id)
);


-- [fabioribeiroaz-producao].dbo.fabCondicoesPagamento definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabCondicoesPagamento;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabCondicoesPagamento (
	idCondicaoPagamento int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	parcelas tinyint DEFAULT 1 NOT NULL,
	p1 decimal(8,5) DEFAULT 0 NULL,
	d1 int DEFAULT 0 NOT NULL,
	p2 decimal(8,5) DEFAULT 0 NULL,
	d2 int DEFAULT 0 NOT NULL,
	p3 decimal(8,5) DEFAULT 0 NULL,
	d3 int DEFAULT 0 NOT NULL,
	p4 decimal(8,5) DEFAULT 0 NULL,
	d4 int DEFAULT 0 NOT NULL,
	p5 decimal(8,5) DEFAULT 0 NULL,
	d5 int DEFAULT 0 NOT NULL,
	p6 decimal(8,5) DEFAULT 0 NULL,
	d6 int DEFAULT 0 NOT NULL,
	p7 decimal(8,5) DEFAULT 0 NULL,
	d7 int DEFAULT 0 NOT NULL,
	p8 decimal(8,5) DEFAULT 0 NULL,
	d8 int DEFAULT 0 NOT NULL,
	p9 decimal(8,5) DEFAULT 0 NULL,
	d9 int DEFAULT 0 NOT NULL,
	p10 decimal(8,5) DEFAULT 0 NULL,
	d10 int DEFAULT 0 NOT NULL,
	p11 decimal(8,5) DEFAULT 0 NULL,
	d11 int DEFAULT 0 NOT NULL,
	p12 decimal(8,5) DEFAULT 0 NULL,
	d12 int DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	compras bit DEFAULT 0 NOT NULL,
	vendas bit DEFAULT 0 NOT NULL,
	valorMinimo decimal(11,2) DEFAULT 0 NULL,
	atendimento bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_fabCondicoesPagamento PRIMARY KEY (idCondicaoPagamento)
);


-- [fabioribeiroaz-producao].dbo.fabConfig definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabConfig;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabConfig (
	idConfig int IDENTITY(1,1) NOT NULL,
	imagemLogin varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	imagemLoginCentral varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	imagemLoginTickets varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsAlteracao datetime NULL,
	precoCombustivel decimal(11,2) DEFAULT 0 NOT NULL,
	dataBloqueioFinanceiro smalldatetime NULL,
	CONSTRAINT PK_fabConfig PRIMARY KEY (idConfig)
);


-- [fabioribeiroaz-producao].dbo.fabDatasEFeriados definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabDatasEFeriados;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabDatasEFeriados (
	idData int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	[data] smalldatetime NULL,
	feriado bit DEFAULT 0 NOT NULL,
	fixo bit DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_fabDatasEFeriados PRIMARY KEY (idData)
);


-- [fabioribeiroaz-producao].dbo.fabFormasPagamento definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabFormasPagamento;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabFormasPagamento (
	idFormaPagamento int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NOT NULL,
	ordem int DEFAULT 1 NOT NULL,
	padrao bit DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idCondicaoPagamento int NULL,
	contasPagar bit DEFAULT 0 NOT NULL,
	compras bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_fabFormasPagamento PRIMARY KEY (idFormaPagamento)
);


-- [fabioribeiroaz-producao].dbo.fabFormasRecebimento definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabFormasRecebimento;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabFormasRecebimento (
	idFormaRecebimento int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NOT NULL,
	ordem int DEFAULT 1 NOT NULL,
	padrao bit DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idCondicaoPagamento int NULL,
	online bit DEFAULT 0 NOT NULL,
	tipo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	emailPagSeguro varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	texto text COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	contasReceber bit DEFAULT 0 NOT NULL,
	vendas bit DEFAULT 0 NOT NULL,
	diasParaPrevisao int DEFAULT 0 NOT NULL,
	valorDesconto decimal(11,2) DEFAULT 0 NOT NULL,
	descontoTipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'R' NOT NULL,
	recebimentoFuturo bit DEFAULT 0 NOT NULL,
	recebimentoFuturoDias int DEFAULT 0 NOT NULL,
	recebimentoFuturoTaxa decimal(5,2) DEFAULT 0 NOT NULL,
	idConta int NULL,
	idPlanoConta int NULL,
	idCentroCusto int NULL,
	idContaPagar int NULL,
	idPlanoContaPagar int NULL,
	idCentroCustoPagar int NULL,
	idFormaPagar int NULL,
	CONSTRAINT PK_fabFormasRecebimento PRIMARY KEY (idFormaRecebimento)
);


-- [fabioribeiroaz-producao].dbo.fabHistoricoTipos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabHistoricoTipos;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabHistoricoTipos (
	idHistoricoTipo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	tipoMarcacoes char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_clientesHistoricoTipos PRIMARY KEY (idHistoricoTipo)
);


-- [fabioribeiroaz-producao].dbo.fabMotivosAproveitamento definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabMotivosAproveitamento;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabMotivosAproveitamento (
	idMotivo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_fabMotivosAproveitamento PRIMARY KEY (idMotivo)
);


-- [fabioribeiroaz-producao].dbo.fabMotivosPerda definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabMotivosPerda;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabMotivosPerda (
	idMotivo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_fabMotivosPerda PRIMARY KEY (idMotivo)
);


-- [fabioribeiroaz-producao].dbo.fabPaises definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabPaises;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabPaises (
	idPais int IDENTITY(1,1) NOT NULL,
	titulo varchar(90) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK__paises__BD2285E34BCC3ABA PRIMARY KEY (idPais)
);


-- [fabioribeiroaz-producao].dbo.fabPermissoesTipos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabPermissoesTipos;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabPermissoesTipos (
	idPermissaoTipo int IDENTITY(1,1) NOT NULL,
	descricao varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	CONSTRAINT PK_fabPermissoesTipos PRIMARY KEY (idPermissaoTipo)
);


-- [fabioribeiroaz-producao].dbo.fabRegioes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabRegioes;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabRegioes (
	idRegiao int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	estados varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK__fabRegio__82DCC78F6AFACD50 PRIMARY KEY (idRegiao)
);


-- [fabioribeiroaz-producao].dbo.fdtDevs definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fdtDevs;

CREATE TABLE [fabioribeiroaz-producao].dbo.fdtDevs (
	idDev int IDENTITY(1,1) NOT NULL,
	pacote varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	descricao varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	pendente bit DEFAULT 1 NOT NULL,
	aprovado bit DEFAULT 0 NOT NULL,
	reprovado bit DEFAULT 0 NOT NULL,
	finalizado bit DEFAULT 0 NOT NULL,
	comentariosRevisor varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsAlteracao datetime NULL,
	alteradoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	CONSTRAINT PK_fdtDevs_idDev PRIMARY KEY (idDev)
);
 CREATE NONCLUSTERED INDEX IX_fdtDevs_aprovado ON fabioribeiroaz-producao.dbo.fdtDevs (  aprovado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_fdtDevs_ativo ON fabioribeiroaz-producao.dbo.fdtDevs (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_fdtDevs_finalizado ON fabioribeiroaz-producao.dbo.fdtDevs (  finalizado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_fdtDevs_pendente ON fabioribeiroaz-producao.dbo.fdtDevs (  pendente ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_fdtDevs_reprovado ON fabioribeiroaz-producao.dbo.fdtDevs (  reprovado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finAreas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finAreas;

CREATE TABLE [fabioribeiroaz-producao].dbo.finAreas (
	idArea int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idCentroResultado int NULL,
	CONSTRAINT PK_finAreas PRIMARY KEY (idArea)
);
 CREATE NONCLUSTERED INDEX IX_finAreas_ativo ON fabioribeiroaz-producao.dbo.finAreas (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finAreas_idCentroResultado ON fabioribeiroaz-producao.dbo.finAreas (  idCentroResultado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finCentrosCusto definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finCentrosCusto;

CREATE TABLE [fabioribeiroaz-producao].dbo.finCentrosCusto (
	idCentroCusto int IDENTITY(1,1) NOT NULL,
	idUnidade int NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	padrao bit DEFAULT 0 NOT NULL,
	porcentagemRateio decimal(11,2) DEFAULT 0 NOT NULL,
	CONSTRAINT PK_finCentrosCusto PRIMARY KEY (idCentroCusto)
);
 CREATE NONCLUSTERED INDEX IX_finCentrosCusto_ativo ON fabioribeiroaz-producao.dbo.finCentrosCusto (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finCentrosResultado definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finCentrosResultado;

CREATE TABLE [fabioribeiroaz-producao].dbo.finCentrosResultado (
	idCentroResultado int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	padrao bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_finCentrosResultado PRIMARY KEY (idCentroResultado)
);
 CREATE NONCLUSTERED INDEX IX_finCentrosResultado_ativo ON fabioribeiroaz-producao.dbo.finCentrosResultado (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finContas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finContas;

CREATE TABLE [fabioribeiroaz-producao].dbo.finContas (
	idConta int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	banco varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	agencia varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	conta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	favorecido varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	limite decimal(11,2) DEFAULT 0 NOT NULL,
	padraoFluxo bit DEFAULT 1 NOT NULL,
	considerarIndicador bit DEFAULT 1 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	saldoInicial decimal(11,2) DEFAULT '0' NOT NULL,
	padrao bit DEFAULT 0 NOT NULL,
	codigo varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cor varchar(6) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_finContas PRIMARY KEY (idConta)
);
 CREATE NONCLUSTERED INDEX IX_finContas_ativo ON fabioribeiroaz-producao.dbo.finContas (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finContasClientes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finContasClientes;

CREATE TABLE [fabioribeiroaz-producao].dbo.finContasClientes (
	idContaCliente int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	cor varchar(6) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_finContasClientes_idContaCliente PRIMARY KEY (idContaCliente)
);


-- [fabioribeiroaz-producao].dbo.finGruposDRE definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finGruposDRE;

CREATE TABLE [fabioribeiroaz-producao].dbo.finGruposDRE (
	idGrupoDRE int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_finGrupoDRE PRIMARY KEY (idGrupoDRE)
);


-- [fabioribeiroaz-producao].dbo.finLancamentos_BKP definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finLancamentos_BKP;

CREATE TABLE [fabioribeiroaz-producao].dbo.finLancamentos_BKP (
	idLancamento int IDENTITY(1,1) NOT NULL,
	idConta int NOT NULL,
	idPlanoConta int NOT NULL,
	idCentroCusto int NOT NULL,
	operacao char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idForma int NOT NULL,
	modulo char(3) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idCadastro int NULL,
	idPedido int NULL,
	descricao varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nrDocumento varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	valor decimal(11,2) NOT NULL,
	dataEmissao smalldatetime NULL,
	dataVencimento smalldatetime NULL,
	dataQuitacao smalldatetime NULL,
	quitado bit NOT NULL,
	recorrente bit NOT NULL,
	recorrenteChave varchar(12) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	previsao bit NOT NULL,
	cobrancaEnviada bit NULL,
	parcelado bit NOT NULL,
	identificacao int NOT NULL,
	observacao varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit NOT NULL,
	tsInclusao datetime NOT NULL,
	tsAlteracao datetime NULL,
	idUsuarioInclusao int NULL,
	idUsuarioAlteracao int NULL,
	parcela int NOT NULL,
	parcelaMaxima int NOT NULL,
	dataVencimentoOriginal smalldatetime NULL,
	pagtoLiberado int NOT NULL,
	dataParaPrevisao smalldatetime NULL,
	recorrenteVencendoVisto bit NOT NULL,
	recebimentoFuturo bit NOT NULL,
	recebimentoFuturoRel bit NOT NULL,
	idTerceiro int NULL,
	arquivoDocumento varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	arquivoComprovante varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idClientePagar int NULL,
	idProcessoPagar int NULL,
	idArea int NULL,
	identificacaoPagar varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	identificacaoPagar2 varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	arquivoDocumento2 varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	arquivoComprovante2 varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	verba bit NULL,
	verbaDataDe smalldatetime NULL,
	verbaDataAte smalldatetime NULL,
	verbaEstado varchar(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	verbaCidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idCentroResultado int NULL,
	secundaria bit NOT NULL,
	geradoPeloProcesso bit NULL,
	sequenciaHerdeiro int NULL,
	idUnidade int NULL
);


-- [fabioribeiroaz-producao].dbo.finPlanoContasDet definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finPlanoContasDet;

CREATE TABLE [fabioribeiroaz-producao].dbo.finPlanoContasDet (
	idPlanoContasDet int IDENTITY(1,1) NOT NULL,
	idPlanoConta int NOT NULL,
	titulo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_finPlanoContasDet PRIMARY KEY (idPlanoContasDet)
);
 CREATE NONCLUSTERED INDEX IX_finPlanoContasDet_ativo ON fabioribeiroaz-producao.dbo.finPlanoContasDet (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finPlanoContasDet_idPlanoConta ON fabioribeiroaz-producao.dbo.finPlanoContasDet (  idPlanoConta ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finProcuracoesRPV definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finProcuracoesRPV;

CREATE TABLE [fabioribeiroaz-producao].dbo.finProcuracoesRPV (
	idProcuracao int IDENTITY(1,1) NOT NULL,
	idCliente int NULL,
	idProcesso int NULL,
	impressa bit DEFAULT 0 NOT NULL,
	tsImpressa datetime NULL,
	assinada bit DEFAULT 0 NOT NULL,
	tsAssinatura datetime NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_finProcuracoesRPV_idProcuracao PRIMARY KEY (idProcuracao)
);


-- [fabioribeiroaz-producao].dbo.finRateios definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finRateios;

CREATE TABLE [fabioribeiroaz-producao].dbo.finRateios (
	idRateio int IDENTITY(1,1) NOT NULL,
	idLancamento int NOT NULL,
	idCentroCusto int NOT NULL,
	idCentroResultado int NULL,
	percentualCC decimal(11,2) DEFAULT 0 NOT NULL,
	percentualCR decimal(11,2) NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	idUnidade int NULL,
	CONSTRAINT PK_finRateios PRIMARY KEY (idRateio)
);
 CREATE NONCLUSTERED INDEX IX_finRateios_ativo ON fabioribeiroaz-producao.dbo.finRateios (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finRateios_idCentroCusto ON fabioribeiroaz-producao.dbo.finRateios (  idCentroCusto ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finRateios_idCentroResultado ON fabioribeiroaz-producao.dbo.finRateios (  idCentroResultado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finRateios_idLancamento ON fabioribeiroaz-producao.dbo.finRateios (  idLancamento ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finRateiosPadrao definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finRateiosPadrao;

CREATE TABLE [fabioribeiroaz-producao].dbo.finRateiosPadrao (
	idPadrao int IDENTITY(1,1) NOT NULL,
	idUnidade int NOT NULL,
	idCentroResultado int NOT NULL,
	porcentagem decimal(11,2) NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL
);


-- [fabioribeiroaz-producao].dbo.finRecibos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finRecibos;

CREATE TABLE [fabioribeiroaz-producao].dbo.finRecibos (
	idRecibo int IDENTITY(1,1) NOT NULL,
	idLancamento int NULL,
	numero int NULL,
	referente varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_finRecibos_idRecibo PRIMARY KEY (idRecibo)
);
 CREATE NONCLUSTERED INDEX IX_finRecibos_numero ON fabioribeiroaz-producao.dbo.finRecibos (  numero ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finUnidades definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finUnidades;

CREATE TABLE [fabioribeiroaz-producao].dbo.finUnidades (
	idUnidade int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	percentual decimal(5,2) DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	tsAlteracao datetime NULL
);


-- [fabioribeiroaz-producao].dbo.flwAcoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.flwAcoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.flwAcoes (
	idAcao int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	diasReagendamento int DEFAULT 3 NOT NULL,
	CONSTRAINT PK_flwAcoes PRIMARY KEY (idAcao)
);


-- [fabioribeiroaz-producao].dbo.flwConfig definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.flwConfig;

CREATE TABLE [fabioribeiroaz-producao].dbo.flwConfig (
	idConfig int IDENTITY(1,1) NOT NULL,
	tipoMarcacoes char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK__flwConfi__C7E5C6EF6991A7CB PRIMARY KEY (idConfig)
);


-- [fabioribeiroaz-producao].dbo.logAcoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.logAcoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.logAcoes (
	idLog int IDENTITY(1,1) NOT NULL,
	area char(3) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	acao char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	usuario varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	motivo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	idCliente int NULL,
	idProcesso int NULL,
	idCompromisso int NULL,
	idTarefa int NULL,
	CONSTRAINT PK_logAcoes PRIMARY KEY (idLog)
);


-- [fabioribeiroaz-producao].dbo.opoSituacoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.opoSituacoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.opoSituacoes (
	idSituacao int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	ordem int DEFAULT 1 NOT NULL,
	considerarIndicador bit DEFAULT 1 NOT NULL,
	CONSTRAINT PK_opoStatus PRIMARY KEY (idSituacao)
);


-- [fabioribeiroaz-producao].dbo.opoTipos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.opoTipos;

CREATE TABLE [fabioribeiroaz-producao].dbo.opoTipos (
	idTipo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_opoTipos PRIMARY KEY (idTipo)
);


-- [fabioribeiroaz-producao].dbo.usuAreas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.usuAreas;

CREATE TABLE [fabioribeiroaz-producao].dbo.usuAreas (
	idArea int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_usuariosAreas PRIMARY KEY (idArea)
);


-- [fabioribeiroaz-producao].dbo.advClientes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientes;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientes (
	idCliente int IDENTITY(1,1) NOT NULL,
	apelido varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	idGrupo int NULL,
	idSituacao int NULL,
	nome varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	email varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelular varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelularObs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixo varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixoObs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataNascimento smalldatetime NULL,
	cpf varchar(14) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	rg varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ctps varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	endereco varchar(75) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	numero varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	complemento varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bairro varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cep varchar(9) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	estado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataIngresso smalldatetime NULL,
	observacoes varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	naturalEstado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	naturalCidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nomeDaMae varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dib bit DEFAULT 0 NOT NULL,
	dibData smalldatetime NULL,
	dibIdTipoBeneficio int NULL,
	idCargo int NULL,
	telCelular2 varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telCelular2Obs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixo2 varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	telFixo2Obs varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cnpj varchar(18) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ie varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idFornecedor int NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	inssAgendado bit DEFAULT 0 NOT NULL,
	inssData smalldatetime NULL,
	inssIdTipoBeneficio int NULL,
	inssIdPosto int NULL,
	inssResultado varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	prospect bit DEFAULT 0 NOT NULL,
	idLocalAtendido int NULL,
	whatsapp bit NULL,
	pastaFTP varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	inssResponsavel int NULL,
	responsavelPendencia int NULL,
	comoChegou int NULL,
	inssProtocolo varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	inssTsInclusao datetime NULL,
	inssIdUsuarioInclusao int NULL,
	foto varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	followBloqueadoAte smalldatetime NULL,
	falecido bit DEFAULT 0 NOT NULL,
	senhaINSSDigital varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idPrioridade int NULL,
	instagram varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	rgOrgaoExp varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nacionalidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT NULL NULL,
	estadocivil varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT NULL NULL,
	dcb bit DEFAULT 0 NULL,
	dcbData smalldatetime DEFAULT NULL NULL,
	finIdUnidade int NULL,
	finIdCentroCusto int NULL,
	CONSTRAINT PK_advClientes PRIMARY KEY (idCliente),
	CONSTRAINT FK_advClientes_advCliCargos FOREIGN KEY (idCargo) REFERENCES [fabioribeiroaz-producao].dbo.advCliCargos(idCargo) ON DELETE SET NULL,
	CONSTRAINT FK_advClientes_advCliGrupos FOREIGN KEY (idGrupo) REFERENCES [fabioribeiroaz-producao].dbo.advCliGrupos(idGrupo) ON DELETE CASCADE,
	CONSTRAINT FK_advClientes_advCliSituacoes FOREIGN KEY (idSituacao) REFERENCES [fabioribeiroaz-producao].dbo.advCliSituacoes(idSituacao) ON DELETE CASCADE,
	CONSTRAINT FK_advClientes_advFornecedores FOREIGN KEY (idFornecedor) REFERENCES [fabioribeiroaz-producao].dbo.advFornecedores(idFornecedor) ON DELETE SET NULL,
	CONSTRAINT FK_advClientes_advPostosINSS FOREIGN KEY (inssIdPosto) REFERENCES [fabioribeiroaz-producao].dbo.advPostosINSS(idPosto) ON DELETE SET NULL
);
 CREATE NONCLUSTERED INDEX IX_advClientes_ativo ON fabioribeiroaz-producao.dbo.advClientes (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advClientes_cnpj ON fabioribeiroaz-producao.dbo.advClientes (  cnpj ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advClientes_cpf ON fabioribeiroaz-producao.dbo.advClientes (  cpf ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advClientes_idSituacao ON fabioribeiroaz-producao.dbo.advClientes (  idSituacao ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientes_1B8F9380B38963DBC61A8C8C75E7821F ON fabioribeiroaz-producao.dbo.advClientes (  ativo ASC  , cpf ASC  , estado ASC  )  
	 INCLUDE ( dataNascimento , incluidoPor ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientes_3FDBFE1AA7D6F7E738199E4445A5F647 ON fabioribeiroaz-producao.dbo.advClientes (  ativo ASC  , inssAgendado ASC  , inssData ASC  )  
	 INCLUDE ( inssResultado ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientes_557F34D88F674C3BC0B16871385E0272 ON fabioribeiroaz-producao.dbo.advClientes (  ativo ASC  , estado ASC  )  
	 INCLUDE ( apelido , cidade , cnpj , cpf , dataNascimento , email , idCargo , idGrupo , idSituacao , inssData , nome , prospect , telCelular , telCelular2 , telFixo , telFixo2 , whatsapp ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientes_5B4B81C96CCC6859413ABB9FB1B7DD37 ON fabioribeiroaz-producao.dbo.advClientes (  ativo ASC  , idSituacao ASC  , estado ASC  )  
	 INCLUDE ( apelido , cidade , cnpj , cpf , dataNascimento , email , idCargo , idGrupo , nome , prospect , telCelular , telCelular2 , telFixo , telFixo2 , whatsapp ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advClientesArquivos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesArquivos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesArquivos (
	idArquivo int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	idTipoArquivo int NOT NULL,
	descricao varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	arquivo varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	alteradoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idProcesso int NULL,
	precisaRevisao bit DEFAULT 0 NOT NULL,
	idSolicitante int NULL,
	solicitanteComentario varchar(500) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idRevisor int NULL,
	revisorComentario varchar(500) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	reprovado int DEFAULT 0 NOT NULL,
	pendenteVisualizacaoAprovacao bit DEFAULT 0 NOT NULL,
	status varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	autoFTP bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_advClientesArquivos PRIMARY KEY (idArquivo),
	CONSTRAINT FK_advClientesArquivos_advCliTiposArquivos FOREIGN KEY (idTipoArquivo) REFERENCES [fabioribeiroaz-producao].dbo.advCliTiposArquivos(idTipoArquivo) ON DELETE CASCADE,
	CONSTRAINT FK_advClientesArquivos_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX nci_msft_1_advClientesArquivos_54ECFCA957E995731B48F4D42332C95B ON fabioribeiroaz-producao.dbo.advClientesArquivos (  arquivo ASC  , status ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientesArquivos_12CF88D13F9375A588DA55FBF8CE6203 ON fabioribeiroaz-producao.dbo.advClientesArquivos (  idRevisor ASC  , precisaRevisao ASC  , reprovado ASC  )  
	 INCLUDE ( idCliente ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientesArquivos_6384FD828313A8031740D5A8D81B76E8 ON fabioribeiroaz-producao.dbo.advClientesArquivos (  idSolicitante ASC  , precisaRevisao ASC  , reprovado ASC  )  
	 INCLUDE ( idCliente ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientesArquivos_99E1B93AAC3CE7FB09B99EC23169EB8B ON fabioribeiroaz-producao.dbo.advClientesArquivos (  idSolicitante ASC  , pendenteVisualizacaoAprovacao ASC  )  
	 INCLUDE ( idCliente ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientesArquivos_B4DB2095C47EF9CC898A4F9100BB360E ON fabioribeiroaz-producao.dbo.advClientesArquivos (  ativo ASC  , idCliente ASC  , idProcesso ASC  , reprovado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advClientesAtualizacoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesAtualizacoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesAtualizacoes (
	idAtualizacao int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	campo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	tsAlteracao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	dadoAnterior varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idUsuario int NULL,
	CONSTRAINT FK_advClientesAtualizacoes_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advClientesChecklist definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesChecklist;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesChecklist (
	idClienteChecklist int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	idTipoArquivo int NOT NULL,
	CONSTRAINT PK_advClientesChecklist PRIMARY KEY (idClienteChecklist),
	CONSTRAINT FK_advClientesChecklist_advCliTiposArquivos FOREIGN KEY (idTipoArquivo) REFERENCES [fabioribeiroaz-producao].dbo.advCliTiposArquivos(idTipoArquivo) ON DELETE CASCADE,
	CONSTRAINT FK_advClientesChecklist_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advClientesINSS definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesINSS;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesINSS (
	idInssAgendado int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	inssAgendado bit NULL,
	inssData smalldatetime NULL,
	inssIdTipoBeneficio int NULL,
	inssIdPosto int NULL,
	inssResultado varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsInclusao smalldatetime NOT NULL,
	tsAlteracao smalldatetime NULL,
	inssResultadoIndicadorOculto bit DEFAULT 0 NOT NULL,
	inssResponsavel int NULL,
	inssProtocolo varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	inssIdUsuarioInclusao int NULL,
	idStatus int NULL,
	dataFinalizacao smalldatetime NULL,
	CONSTRAINT FK_advClientesINSS_advClientesINSSStatus FOREIGN KEY (idStatus) REFERENCES [fabioribeiroaz-producao].dbo.advClientesINSSStatus(idStatus) ON DELETE SET NULL
);
 CREATE NONCLUSTERED INDEX nci_wi_advClientesINSS_7050530DA5ECCE64A2CE132F042C1369 ON fabioribeiroaz-producao.dbo.advClientesINSS (  idInssAgendado ASC  )  
	 INCLUDE ( idCliente , inssData , inssProtocolo , inssResponsavel ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advClientesINSS_8FCC706AF2763CE1EB0ED33D13F31965 ON fabioribeiroaz-producao.dbo.advClientesINSS (  idCliente ASC  , inssAgendado ASC  )  
	 INCLUDE ( dataFinalizacao , idInssAgendado , idStatus , inssData , inssIdPosto , inssIdTipoBeneficio , inssProtocolo , inssResponsavel , inssResultado , tsInclusao ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advPreCheckLists definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreCheckLists;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreCheckLists (
	idCheckList int IDENTITY(1,1) NOT NULL,
	idGrupo int NOT NULL,
	titulo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advPreCheckLists PRIMARY KEY (idCheckList),
	CONSTRAINT FK_advPreCheckLists_advPreCheckListsGrupos FOREIGN KEY (idGrupo) REFERENCES [fabioribeiroaz-producao].dbo.advPreCheckListsGrupos(idGrupo) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advPreStatus definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advPreStatus;

CREATE TABLE [fabioribeiroaz-producao].dbo.advPreStatus (
	idStatus int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	ordem int DEFAULT 9 NOT NULL,
	ultimo bit DEFAULT 0 NOT NULL,
	diasMaxParado int DEFAULT 0 NOT NULL,
	idTipo int NULL,
	CONSTRAINT PK_advPreStatus PRIMARY KEY (idStatus),
	CONSTRAINT FK_advPreStatus_advPreStatusTipos FOREIGN KEY (idTipo) REFERENCES [fabioribeiroaz-producao].dbo.advPreStatusTipos(idTipo)
);


-- [fabioribeiroaz-producao].dbo.advProcessos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProcessos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProcessos (
	idProcesso int IDENTITY(1,1) NOT NULL,
	idCliente int NULL,
	idUsuarioInclusao int NULL,
	idEscritorioOrigem int NULL,
	idEscritorioResponsavel int NULL,
	idAutorPeticao int NULL,
	idResponsavel int NULL,
	sintese text COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	numero varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataDistribuicao smalldatetime NULL,
	idStatus int NULL,
	idNatureza int NULL,
	idTipo int NULL,
	estado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idFase int NULL,
	idRelevancia int NULL,
	idProbabilidade int NULL,
	valorCausa decimal(18,2) DEFAULT 0 NOT NULL,
	valorHonorarios decimal(18,2) DEFAULT 0 NOT NULL,
	valorHonorariosTipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'r' NOT NULL,
	observacoes varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idSentenca int NULL,
	dataSentenca smalldatetime NULL,
	alvara bit NULL,
	valorDeferido decimal(18,2) DEFAULT 0 NOT NULL,
	dataEncerramento smalldatetime NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idOrgao int NULL,
	idInstancia int NULL,
	idVara int NULL,
	recurso bit DEFAULT 0 NOT NULL,
	recursoIdSentenca int NULL,
	recursoDataSentenca smalldatetime NULL,
	alvaraPendente bit NULL,
	alvaraPendenteDesde smalldatetime NULL,
	historicoNumeros text COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	recebeAcordo bit DEFAULT 0 NOT NULL,
	recebeRPV bit DEFAULT 0 NOT NULL,
	recebePrecatorio bit DEFAULT 0 NOT NULL,
	recebeAlvara bit DEFAULT 0 NOT NULL,
	recebeBanco varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	recebeDataLiberacao smalldatetime NULL,
	pendOutrosValores bit DEFAULT 0 NOT NULL,
	pendOutrosValoresDataEncerramento smalldatetime NULL,
	pendOutrosValoresDeferido bit DEFAULT 0 NOT NULL,
	pendOutrosValoresValorDeferido decimal(11,2) DEFAULT 0 NOT NULL,
	acaoColetiva bit DEFAULT 0 NOT NULL,
	temResponsavel bit DEFAULT 0 NOT NULL,
	nomeResponsavel varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cpfResponsavel varchar(14) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	imposto decimal(11,2) DEFAULT 0 NOT NULL,
	tarifa decimal(11,2) DEFAULT 0 NOT NULL,
	complementoPositivo decimal(11,2) DEFAULT 0 NOT NULL,
	RPV varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioBanco varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioTipoConta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioAgencia varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioConta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioFavorecido varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioCpf varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nomeReu varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	sucumbencia decimal(11,2) NULL,
	idConta int NULL,
	dataLiberacaoValorDeferido smalldatetime NULL,
	boleto bit DEFAULT 0 NOT NULL,
	precatorio varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	emitir varchar(10) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	emitido bit DEFAULT 0 NOT NULL,
	formaRecebimento varchar(60) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioBancoId int NULL,
	dataPrevisaoRepasseCliente smalldatetime NULL,
	honorariosTextoFicha varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nfComComplementoPositivo bit DEFAULT 0 NOT NULL,
	valorHonorariosDestaque decimal(11,2) DEFAULT 0 NOT NULL,
	valorHonorariosDestaqueTipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'r' NOT NULL,
	dataPrevisaoHonorariosDestaque smalldatetime NULL,
	idContaPagar int NULL,
	bancarioPerc decimal(11,2) NULL,
	dataPrevistaClienteReceber smalldatetime NULL,
	sucumbenciaAdd decimal(11,2) NULL,
	sucumbenciaAddData smalldatetime NULL,
	sucumbenciaAddIdBanco int NULL,
	saldoDevedor decimal(11,2) DEFAULT 0 NOT NULL,
	herdeirosTipoValor char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'p' NOT NULL,
	nrParcelasProcesso int DEFAULT 1 NOT NULL,
	nrParcelasSomenteSucumbencia int DEFAULT 1 NOT NULL,
	preProcesso bit DEFAULT 0 NOT NULL,
	preProcessoPasta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	preProcessoDataCriacao smalldatetime NULL,
	preProcessoDataPrevista smalldatetime NULL,
	preProcessoDataRealizada smalldatetime NULL,
	preProcessoIdStatus int NULL,
	tsConversao datetime NULL,
	perdido bit DEFAULT 0 NOT NULL,
	tsPerdido datetime NULL,
	idMotivoPerda int NULL,
	convertido bit DEFAULT 0 NOT NULL,
	clientePrimeiraVez bit NULL,
	preProcessoIdTipo int NULL,
	tarifaParcelas varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idOrigem int NULL,
	dataEntrada smalldatetime NULL,
	CONSTRAINT PK_advProcessos PRIMARY KEY (idProcesso),
	CONSTRAINT FK_advProcessos_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>] ON fabioribeiroaz-producao.dbo.advProcessos (  ativo ASC  , preProcesso ASC  , estado ASC  )  
	 INCLUDE ( acaoColetiva , cpfResponsavel , dataDistribuicao , dataEncerramento , idCliente , idNatureza , idResponsavel , idStatus , idTipo , nomeResponsavel , numero ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_ativo ON fabioribeiroaz-producao.dbo.advProcessos (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_cpfResponsavel ON fabioribeiroaz-producao.dbo.advProcessos (  cpfResponsavel ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_estado ON fabioribeiroaz-producao.dbo.advProcessos (  estado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_idResponsavel ON fabioribeiroaz-producao.dbo.advProcessos (  idResponsavel ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_numero ON fabioribeiroaz-producao.dbo.advProcessos (  numero ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_preProcesso ON fabioribeiroaz-producao.dbo.advProcessos (  preProcesso ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_preProcessoIdStatus ON fabioribeiroaz-producao.dbo.advProcessos (  preProcessoIdStatus ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessos_preProcessoPasta ON fabioribeiroaz-producao.dbo.advProcessos (  preProcessoPasta ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_msft_1_advProcessos_7172E5B55BFA9AA317B9500B4DFDD4A8 ON fabioribeiroaz-producao.dbo.advProcessos (  ativo ASC  , alvaraPendente ASC  , idNatureza ASC  , idResponsavel ASC  )  
	 INCLUDE ( convertido , dataDistribuicao , idCliente , idStatus , idTipo , numero , perdido , preProcesso , preProcessoIdStatus , preProcessoIdTipo , preProcessoPasta ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advProcessos_11954633388C8E0674A2A4C67368E0FF ON fabioribeiroaz-producao.dbo.advProcessos (  ativo ASC  , recurso ASC  , recursoIdSentenca ASC  , idNatureza ASC  , idResponsavel ASC  )  
	 INCLUDE ( cidade , estado ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advProcessos_2DF8802DE8A1116CF313E41F9FC30AED ON fabioribeiroaz-producao.dbo.advProcessos (  idNatureza ASC  , ativo ASC  , pendOutrosValores ASC  , pendOutrosValoresDataEncerramento ASC  , idResponsavel ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advProcessos_5870EB74A416810941CF9CC4C93E7435 ON fabioribeiroaz-producao.dbo.advProcessos (  idResponsavel ASC  , idNatureza ASC  , ativo ASC  , dataDistribuicao ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advProcessos_C18ABF319DF45116458FEECC5BB85835 ON fabioribeiroaz-producao.dbo.advProcessos (  idCliente ASC  , ativo ASC  , estado ASC  )  
	 INCLUDE ( acaoColetiva , dataDistribuicao , dataEncerramento , idNatureza , idResponsavel , idStatus , idTipo , numero ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advProcessosClientes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProcessosClientes;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProcessosClientes (
	idProcessoCliente int IDENTITY(1,1) NOT NULL,
	idProcesso int NOT NULL,
	idCliente int NOT NULL,
	CONSTRAINT PK_advProcessosClientes PRIMARY KEY (idProcessoCliente),
	CONSTRAINT FK_advProcessosClientes_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente),
	CONSTRAINT FK_advProcessosClientes_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advProcessosHonorarios definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProcessosHonorarios;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProcessosHonorarios (
	idHonorario int IDENTITY(1,1) NOT NULL,
	idProcesso int NOT NULL,
	dataPrevistaClienteReceber smalldatetime NULL,
	rpv varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	precatorio varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nrParcelasProcesso int DEFAULT 1 NOT NULL,
	valorHonorarios decimal(11,2) DEFAULT 0 NOT NULL,
	valorHonorariosTipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'p' NOT NULL,
	honorariosTextoFicha varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	valorHonorariosDestaque decimal(11,2) DEFAULT 0 NOT NULL,
	valorHonorariosDestaqueTipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'r' NOT NULL,
	dataPrevisaoHonorariosDestaque smalldatetime NULL,
	imposto decimal(11,2) DEFAULT 0 NOT NULL,
	complementoPositivo decimal(11,2) DEFAULT 0 NOT NULL,
	sucumbencia decimal(11,2) DEFAULT 0 NOT NULL,
	saldoDevedor decimal(11,2) DEFAULT 0 NOT NULL,
	valorDeferido decimal(11,2) DEFAULT 0 NOT NULL,
	dataLiberacaoValorDeferido smalldatetime NULL,
	idConta int NULL,
	dataPrevisaoRepasseCliente smalldatetime NULL,
	idContaPagar int NULL,
	formaRecebimento varchar(60) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nrParcelasSomenteSucumbencia int DEFAULT 0 NOT NULL,
	sucumbenciaAdd decimal(11,2) NULL,
	sucumbenciaAddData smalldatetime NULL,
	sucumbenciaAddIdBanco int NULL,
	boleto bit DEFAULT 0 NOT NULL,
	emitir varchar(10) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	emitido bit DEFAULT 0 NOT NULL,
	nfComComplementoPositivo bit DEFAULT 0 NOT NULL,
	bancarioCpf varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	herdeirosTipoValor char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	bancarioPerc decimal(11,2) DEFAULT 0 NOT NULL,
	tarifa decimal(11,2) DEFAULT 0 NOT NULL,
	tarifaParcelas varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioFavorecido varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioBancoId int NULL,
	bancarioTipoConta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioAgencia varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioConta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsAlteracao datetime NULL,
	alteradoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	valorHonorariosDestaqueSomente decimal(11,2) DEFAULT 0 NOT NULL,
	valorHonorariosDestaqueTipoSomente char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'r' NOT NULL,
	dataPrevisaoHonorariosDestaqueSomente smalldatetime NULL,
	idBancoDestaqueSomente int NULL,
	CONSTRAINT PK_advProcessosHonorarios_idHonorario PRIMARY KEY (idHonorario),
	CONSTRAINT FK_advProcessosHonorarios_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_advProcessosHonorarios_ativo ON fabioribeiroaz-producao.dbo.advProcessosHonorarios (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessosHonorarios_dataPrevisaoRepasseCliente ON fabioribeiroaz-producao.dbo.advProcessosHonorarios (  dataPrevisaoRepasseCliente ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessosHonorarios_dataPrevistaClienteReceber ON fabioribeiroaz-producao.dbo.advProcessosHonorarios (  dataPrevistaClienteReceber ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProcessosHonorarios_idProcesso ON fabioribeiroaz-producao.dbo.advProcessosHonorarios (  idProcesso ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advProcessosMeritos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProcessosMeritos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProcessosMeritos (
	idProcessoMerito int IDENTITY(1,1) NOT NULL,
	idProcesso int NOT NULL,
	idMerito int NOT NULL,
	CONSTRAINT PK_advProcessosMeritos PRIMARY KEY (idProcessoMerito),
	CONSTRAINT FK_advProcessosMeritos_advProMeritos FOREIGN KEY (idMerito) REFERENCES [fabioribeiroaz-producao].dbo.advProMeritos(idMerito) ON DELETE CASCADE,
	CONSTRAINT FK_advProcessosMeritos_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.fabEstados definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabEstados;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabEstados (
	idEstado int IDENTITY(1,1) NOT NULL,
	sigla char(2) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NULL,
	descricao varchar(90) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NULL,
	idPais int DEFAULT 1 NULL,
	CONSTRAINT PK__estados__62EA894A6A50C1DA PRIMARY KEY (idEstado),
	CONSTRAINT FK_estados_paises FOREIGN KEY (idPais) REFERENCES [fabioribeiroaz-producao].dbo.fabPaises(idPais) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.fabPermissoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabPermissoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabPermissoes (
	idPermissao int IDENTITY(1,1) NOT NULL,
	idPermissaoTipo int NOT NULL,
	modulo bit DEFAULT 0 NOT NULL,
	descricao varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	varSession varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	CONSTRAINT PK_fabPermissoes PRIMARY KEY (idPermissao),
	CONSTRAINT FK_fabPermissoes_fabPermissoesTipos FOREIGN KEY (idPermissaoTipo) REFERENCES [fabioribeiroaz-producao].dbo.fabPermissoesTipos(idPermissaoTipo) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.finExtrato definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finExtrato;

CREATE TABLE [fabioribeiroaz-producao].dbo.finExtrato (
	idExtrato int IDENTITY(1,1) NOT NULL,
	idConta int NOT NULL,
	idLancamento int NULL,
	transferencia bit DEFAULT 0 NOT NULL,
	idExtratoRel int NULL,
	[data] smalldatetime NULL,
	descricao varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	credito decimal(11,2) DEFAULT 0 NOT NULL,
	debito decimal(11,2) DEFAULT 0 NOT NULL,
	conferido bit DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idUsuarioInclusao int NULL,
	idUsuarioAlteracao int NULL,
	CONSTRAINT PK_finExtrato PRIMARY KEY (idExtrato),
	CONSTRAINT FK_finExtrato_finContas FOREIGN KEY (idConta) REFERENCES [fabioribeiroaz-producao].dbo.finContas(idConta) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_finExtrato ON fabioribeiroaz-producao.dbo.finExtrato (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finExtrato_1 ON fabioribeiroaz-producao.dbo.finExtrato (  idLancamento ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finExtrato_2 ON fabioribeiroaz-producao.dbo.finExtrato (  idConta ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_finExtrato_EF7BC32A49BCB8BB59FDBABD8FC2DCE3 ON fabioribeiroaz-producao.dbo.finExtrato (  ativo ASC  , idConta ASC  , data ASC  )  
	 INCLUDE ( credito , debito , descricao , idExtrato , idLancamento , transferencia ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finPlanoContasGrupos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finPlanoContasGrupos;

CREATE TABLE [fabioribeiroaz-producao].dbo.finPlanoContasGrupos (
	idGrupo int IDENTITY(1,1) NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'd' NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idGrupoDRE int NULL,
	ordem int DEFAULT 1 NOT NULL,
	CONSTRAINT PK_finPlanoContasGrupos PRIMARY KEY (idGrupo),
	CONSTRAINT FK_finPlanoContasGrupos_finGruposDRE FOREIGN KEY (idGrupoDRE) REFERENCES [fabioribeiroaz-producao].dbo.finGruposDRE(idGrupoDRE) ON DELETE SET NULL
);
 CREATE NONCLUSTERED INDEX IX_finPlanoContasGrupos_ativo ON fabioribeiroaz-producao.dbo.finPlanoContasGrupos (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finPlanoContasGrupos_idGrupoDRE ON fabioribeiroaz-producao.dbo.finPlanoContasGrupos (  idGrupoDRE ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finPlanoContasGrupos_tipo ON fabioribeiroaz-producao.dbo.finPlanoContasGrupos (  tipo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.flwConfigExcecoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.flwConfigExcecoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.flwConfigExcecoes (
	idConfig int IDENTITY(1,1) NOT NULL,
	tipoMarcacoes char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idHistoricoTipo int NOT NULL,
	[data] smalldatetime NULL,
	qtde int NULL,
	manhaQtde int NULL,
	tardeQtde int NULL,
	CONSTRAINT PK__flwConfi__C7E5C6EF6D6238AF PRIMARY KEY (idConfig),
	CONSTRAINT FK_flwConfigExcecoes_fabHistoricoTipos FOREIGN KEY (idHistoricoTipo) REFERENCES [fabioribeiroaz-producao].dbo.fabHistoricoTipos(idHistoricoTipo) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.flwGradeHorarios definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.flwGradeHorarios;

CREATE TABLE [fabioribeiroaz-producao].dbo.flwGradeHorarios (
	idGrade int IDENTITY(1,1) NOT NULL,
	idHistoricoTipo int NOT NULL,
	manhaHorarioInicial int NULL,
	manhaIntervalo int NULL,
	manhaQtde int NULL,
	tardeHorarioInicial int NULL,
	tardeIntervalo int NULL,
	tardeQtde int NULL,
	dom bit DEFAULT 0 NOT NULL,
	seg bit DEFAULT 0 NOT NULL,
	ter bit DEFAULT 0 NOT NULL,
	qua bit DEFAULT 0 NOT NULL,
	qui bit DEFAULT 0 NOT NULL,
	sex bit DEFAULT 0 NOT NULL,
	sab bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK__flwGrade__7AD7DF11731B1205 PRIMARY KEY (idGrade),
	CONSTRAINT FK_flwGradeHorarios_fabHistoricoTipos FOREIGN KEY (idHistoricoTipo) REFERENCES [fabioribeiroaz-producao].dbo.fabHistoricoTipos(idHistoricoTipo) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.logCampos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.logCampos;

CREATE TABLE [fabioribeiroaz-producao].dbo.logCampos (
	idLogCampo int IDENTITY(1,1) NOT NULL,
	idLog int NOT NULL,
	campo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	dadoAnterior varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dadoNovo varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_logCampos PRIMARY KEY (idLogCampo),
	CONSTRAINT FK_logCampos_logAcoes FOREIGN KEY (idLog) REFERENCES [fabioribeiroaz-producao].dbo.logAcoes(idLog) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.usuCargos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.usuCargos;

CREATE TABLE [fabioribeiroaz-producao].dbo.usuCargos (
	idCargo int IDENTITY(1,1) NOT NULL,
	idArea int NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_usuariosCargos PRIMARY KEY (idCargo),
	CONSTRAINT FK_usuariosCargos_usuariosAreas FOREIGN KEY (idArea) REFERENCES [fabioribeiroaz-producao].dbo.usuAreas(idArea) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.usuUsuarios definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.usuUsuarios;

CREATE TABLE [fabioribeiroaz-producao].dbo.usuUsuarios (
	idUsuario int IDENTITY(1,1) NOT NULL,
	nome varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	sobrenome varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	idArea int NOT NULL,
	idCargo int NOT NULL,
	[login] varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	senha varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	diaNascimento int DEFAULT 0 NOT NULL,
	mesNascimento int DEFAULT 0 NOT NULL,
	anoNascimento int DEFAULT 0 NOT NULL,
	email varchar(75) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	telCelular varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	telFixo varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	endereco varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	numero varchar(15) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	complemento varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	bairro varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	cep varchar(9) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	estado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	cidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	cpf varchar(14) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	banco varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	agencia varchar(10) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	conta varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	foto varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	cor varchar(30) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dashboardInicial varchar(3) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tokenPhoneApp varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	estadoCivil varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nrFilhos tinyint NULL,
	idadeFilhoMenor tinyint NULL,
	formacaoAcademica varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	regiao varchar(6) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idSuperior int NULL,
	master bit DEFAULT 0 NOT NULL,
	mediaConsumoLitro int DEFAULT 0 NOT NULL,
	distanciasIguais varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	chaveChamados varchar(32) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_usuarios PRIMARY KEY (idUsuario),
	CONSTRAINT FK_usuarios_usuariosCargos FOREIGN KEY (idCargo) REFERENCES [fabioribeiroaz-producao].dbo.usuCargos(idCargo) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advClientesHistoricos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advClientesHistoricos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advClientesHistoricos (
	idHistorico int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	idProcesso int NULL,
	idUsuario int NOT NULL,
	idTipoHistorico int NOT NULL,
	[data] smalldatetime NOT NULL,
	hora varchar(5) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ocorrencia varchar(4000) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	idOportunidade int NULL,
	depto char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'P' NOT NULL,
	prioritario bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_advClientesHistoricos PRIMARY KEY (idHistorico),
	CONSTRAINT FK_advClientesHistoricos_advCliTiposHistoricos FOREIGN KEY (idTipoHistorico) REFERENCES [fabioribeiroaz-producao].dbo.advCliTiposHistoricos(idTipoHistorico) ON DELETE CASCADE,
	CONSTRAINT FK_advClientesHistoricos_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente) ON DELETE CASCADE,
	CONSTRAINT FK_advClientesHistoricos_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX nci_wi_advClientesHistoricos_8A1DDD4E84F7168BDF7CC3FDA462FE47 ON fabioribeiroaz-producao.dbo.advClientesHistoricos (  idCliente ASC  , prioritario ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advCompromissos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advCompromissos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advCompromissos (
	idCompromisso int IDENTITY(1,1) NOT NULL,
	idTipoCompromisso int NOT NULL,
	idProcesso int NULL,
	dataPublicacao smalldatetime NULL,
	dataPrazoInterno smalldatetime NULL,
	dataPrazoFatal smalldatetime NULL,
	descricao varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	alteradoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idAgendamentoINSS int NULL,
	pauta bit DEFAULT 0 NOT NULL,
	pautaIdUsuarioResp int NULL,
	pautaRespAceite bit DEFAULT 0 NOT NULL,
	horarioInicial int DEFAULT 0 NOT NULL,
	horarioFinal int DEFAULT 0 NOT NULL,
	CONSTRAINT PK_advCompromissos PRIMARY KEY (idCompromisso),
	CONSTRAINT FK_advCompromissos_advAgeTiposCompromissos FOREIGN KEY (idTipoCompromisso) REFERENCES [fabioribeiroaz-producao].dbo.advAgeTiposCompromissos(idTipoCompromisso) ON DELETE CASCADE,
	CONSTRAINT FK_advCompromissos_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE SET NULL
);
 CREATE NONCLUSTERED INDEX IX_advCompromissos_ativo ON fabioribeiroaz-producao.dbo.advCompromissos (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advCompromissos_dataPublicacao ON fabioribeiroaz-producao.dbo.advCompromissos (  dataPublicacao ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advCompromissos_idAgendamentoINSS ON fabioribeiroaz-producao.dbo.advCompromissos (  idAgendamentoINSS ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advCompromissos_pauta ON fabioribeiroaz-producao.dbo.advCompromissos (  pauta ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advCompromissos_pautaIdUsuarioResp ON fabioribeiroaz-producao.dbo.advCompromissos (  pautaIdUsuarioResp ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advCompromissos_pautaRespAceite ON fabioribeiroaz-producao.dbo.advCompromissos (  pautaRespAceite ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advCompromissos_04486BEAB53F9D79A909AD9843D68D12 ON fabioribeiroaz-producao.dbo.advCompromissos (  ativo ASC  , idProcesso ASC  )  
	 INCLUDE ( idAgendamentoINSS ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advCompromissos_8B9D18F95349616F4BC53E8770730DC8 ON fabioribeiroaz-producao.dbo.advCompromissos (  ativo ASC  , idAgendamentoINSS ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advProcessosAlteracoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProcessosAlteracoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProcessosAlteracoes (
	idProcessoAlteracao int IDENTITY(1,1) NOT NULL,
	idProcesso int NOT NULL,
	idUsuario int NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	texto varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_advProcessosAlteracoes PRIMARY KEY (idProcessoAlteracao),
	CONSTRAINT FK_advProcessosAlteracoes_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE CASCADE,
	CONSTRAINT FK_advProcessosAlteracoes_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advProcessosDadosHerdeiros definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProcessosDadosHerdeiros;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProcessosDadosHerdeiros (
	idHerdeiro int IDENTITY(1,1) NOT NULL,
	idProcesso int NOT NULL,
	sequencia int NULL,
	bancarioBancoId int NULL,
	bancarioTipoConta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioAgencia varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioConta varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioFavorecido varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioCpf varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	bancarioPerc decimal(11,2) NULL,
	bancarioTarifa decimal(11,2) DEFAULT 0 NOT NULL,
	bancarioTarifaParcelas varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idHonorario int NULL,
	CONSTRAINT PK_advProcessosDadosHerdeiros_idHerdeiro PRIMARY KEY (idHerdeiro),
	CONSTRAINT FK_advProcessosDadosHerdeiros_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE CASCADE,
	CONSTRAINT FK_advProcessosDadosHerdeiros_advProcessosHonorarios FOREIGN KEY (idHonorario) REFERENCES [fabioribeiroaz-producao].dbo.advProcessosHonorarios(idHonorario)
);
 CREATE NONCLUSTERED INDEX nci_msft_1_advProcessosDadosHerdeiros_B8757DB855F6826E48B3287716162447 ON fabioribeiroaz-producao.dbo.advProcessosDadosHerdeiros (  idProcesso ASC  , sequencia ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advProfissionais definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProfissionais;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProfissionais (
	idProfissional int IDENTITY(1,1) NOT NULL,
	idUsuario int NOT NULL,
	nome varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	email varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_advProfissionais PRIMARY KEY (idProfissional),
	CONSTRAINT FK_advProfissionais_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_advProfissionais_ativo ON fabioribeiroaz-producao.dbo.advProfissionais (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advProfissionais_idUsuario ON fabioribeiroaz-producao.dbo.advProfissionais (  idUsuario ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advProfissionaisEstados definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProfissionaisEstados;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProfissionaisEstados (
	idProfissionalEstado int IDENTITY(1,1) NOT NULL,
	idProfissional int NOT NULL,
	estado char(2) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	CONSTRAINT PK_advProfissionaisEstados PRIMARY KEY (idProfissionalEstado),
	CONSTRAINT FK_advProfissionaisEstados_advProfissionais FOREIGN KEY (idProfissional) REFERENCES [fabioribeiroaz-producao].dbo.advProfissionais(idProfissional) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advProfissionaisNaturezas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advProfissionaisNaturezas;

CREATE TABLE [fabioribeiroaz-producao].dbo.advProfissionaisNaturezas (
	idProfissionalNatureza int IDENTITY(1,1) NOT NULL,
	idProfissional int NOT NULL,
	idNatureza int NOT NULL,
	CONSTRAINT PK_advProfissionaisNaturezas PRIMARY KEY (idProfissionalNatureza),
	CONSTRAINT FK_advProfissionaisNaturezas_advProNaturezas FOREIGN KEY (idNatureza) REFERENCES [fabioribeiroaz-producao].dbo.advProNaturezas(idNatureza) ON DELETE CASCADE,
	CONSTRAINT FK_advProfissionaisNaturezas_advProfissionais FOREIGN KEY (idProfissional) REFERENCES [fabioribeiroaz-producao].dbo.advProfissionais(idProfissional) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.advRevisaoDocumentos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advRevisaoDocumentos;

CREATE TABLE [fabioribeiroaz-producao].dbo.advRevisaoDocumentos (
	idRevisao int IDENTITY(1,1) NOT NULL,
	idUsuarioSolicitante int NOT NULL,
	idUsuarioRevisor int NOT NULL,
	localRede varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	pendente bit DEFAULT 0 NOT NULL,
	aprovado bit DEFAULT 0 NOT NULL,
	reprovado bit DEFAULT 0 NOT NULL,
	finalizado bit DEFAULT 0 NOT NULL,
	comentariosSolicitante varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	comentariosRevisor varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsAlteracao datetime NULL,
	alteradoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	CONSTRAINT PK_advRevisaoDocumentos_idRevisao PRIMARY KEY (idRevisao),
	CONSTRAINT FK_advRevisaoDocumentos_usuUsuarios_revisor FOREIGN KEY (idUsuarioRevisor) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario),
	CONSTRAINT FK_advRevisaoDocumentos_usuUsuarios_solicitante FOREIGN KEY (idUsuarioSolicitante) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario)
);
 CREATE NONCLUSTERED INDEX IX_advRevisaoDocumentos_aprovado ON fabioribeiroaz-producao.dbo.advRevisaoDocumentos (  aprovado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advRevisaoDocumentos_ativo ON fabioribeiroaz-producao.dbo.advRevisaoDocumentos (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advRevisaoDocumentos_finalizado ON fabioribeiroaz-producao.dbo.advRevisaoDocumentos (  finalizado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advRevisaoDocumentos_idUsuarioRevisor ON fabioribeiroaz-producao.dbo.advRevisaoDocumentos (  idUsuarioRevisor ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advRevisaoDocumentos_idUsuarioSolicitante ON fabioribeiroaz-producao.dbo.advRevisaoDocumentos (  idUsuarioSolicitante ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advRevisaoDocumentos_pendente ON fabioribeiroaz-producao.dbo.advRevisaoDocumentos (  pendente ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advRevisaoDocumentos_reprovado ON fabioribeiroaz-producao.dbo.advRevisaoDocumentos (  reprovado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advTarefas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advTarefas;

CREATE TABLE [fabioribeiroaz-producao].dbo.advTarefas (
	idTarefa int IDENTITY(1,1) NOT NULL,
	idTipoTarefa int NOT NULL,
	idCompromisso int NULL,
	idProcesso int NULL,
	dataCadastro smalldatetime NULL,
	dataParaFinalizacao smalldatetime NULL,
	descricao varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idResponsavel int NULL,
	idExecutor int NULL,
	finalizado bit DEFAULT 0 NOT NULL,
	tsFinalizacao datetime NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	alteradoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	agendada bit DEFAULT 0 NOT NULL,
	horarioInicial int DEFAULT 0 NOT NULL,
	horarioFinal int DEFAULT 0 NOT NULL,
	onde varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idCliente int NULL,
	idUsuarioFinalizou int NULL,
	lembreteQuandoFinalizarPara int NULL,
	tecnica bit DEFAULT 0 NOT NULL,
	coletivoOriginal bit DEFAULT 0 NOT NULL,
	coletivoIdOriginal int NULL,
	coletivoIdCliente int NULL,
	pauta bit DEFAULT 0 NOT NULL,
	pautaIdUsuarioResp int NULL,
	pautaRespAceite bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_advTarefas PRIMARY KEY (idTarefa),
	CONSTRAINT FK_advTarefas_advAgeTiposTarefas FOREIGN KEY (idTipoTarefa) REFERENCES [fabioribeiroaz-producao].dbo.advAgeTiposTarefas(idTipoTarefa) ON DELETE CASCADE,
	CONSTRAINT FK_advTarefas_advCompromissos FOREIGN KEY (idCompromisso) REFERENCES [fabioribeiroaz-producao].dbo.advCompromissos(idCompromisso) ON DELETE SET NULL,
	CONSTRAINT FK_advTarefas_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE SET NULL
);
 CREATE NONCLUSTERED INDEX IX_advTarefas_ativo ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_coletivoIdCliente ON fabioribeiroaz-producao.dbo.advTarefas (  coletivoIdCliente ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_coletivoOriginal ON fabioribeiroaz-producao.dbo.advTarefas (  coletivoOriginal ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_coletivoidOriginal ON fabioribeiroaz-producao.dbo.advTarefas (  coletivoIdOriginal ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_finalizado ON fabioribeiroaz-producao.dbo.advTarefas (  finalizado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_idCliente ON fabioribeiroaz-producao.dbo.advTarefas (  idCliente ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_idCompromisso ON fabioribeiroaz-producao.dbo.advTarefas (  idCompromisso ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_idProcesso ON fabioribeiroaz-producao.dbo.advTarefas (  idProcesso ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_idResponsavel ON fabioribeiroaz-producao.dbo.advTarefas (  idResponsavel ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_idTipoTarefa ON fabioribeiroaz-producao.dbo.advTarefas (  idTipoTarefa ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_pauta ON fabioribeiroaz-producao.dbo.advTarefas (  pauta ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_pautaIdUsuarioResp ON fabioribeiroaz-producao.dbo.advTarefas (  pautaIdUsuarioResp ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_pautaRespAceite ON fabioribeiroaz-producao.dbo.advTarefas (  pautaRespAceite ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_advTarefas_tecnica ON fabioribeiroaz-producao.dbo.advTarefas (  tecnica ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_msft_advTarefas_D0CA59FF727B1F4FE1275600491438C6 ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  , idExecutor ASC  , dataParaFinalizacao ASC  )  
	 INCLUDE ( agendada , descricao , finalizado , horarioFinal , horarioInicial , idResponsavel , idTipoTarefa ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advTarefas_21B1B05EBB13A1877D192CA2553D7CB5 ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  , finalizado ASC  , idResponsavel ASC  , dataParaFinalizacao ASC  )  
	 INCLUDE ( agendada , descricao , horarioFinal , horarioInicial , idCliente , idCompromisso , idExecutor , idProcesso , idTipoTarefa , incluidoPor , onde , tsFinalizacao ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advTarefas_2247D42A12849EF8E58B2E127B9D6981 ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  , finalizado ASC  , dataParaFinalizacao ASC  , idExecutor ASC  )  
	 INCLUDE ( agendada , descricao , horarioFinal , horarioInicial , idCliente , idCompromisso , idProcesso , idResponsavel , idTipoTarefa , incluidoPor , onde , tsFinalizacao ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advTarefas_575D902ADE74F04ADC02F4C742AD4DAF ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  , idCompromisso ASC  , finalizado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advTarefas_8BCC8CD4DEED8D5A639D1369C7AB4D90 ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  , finalizado ASC  , idExecutor ASC  , dataParaFinalizacao ASC  )  
	 INCLUDE ( agendada , descricao , horarioFinal , horarioInicial , idCliente , idCompromisso , idProcesso , idResponsavel , idTipoTarefa , incluidoPor , onde , tsFinalizacao ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advTarefas_C01038CE780B766BB3BE6D379185DEAD ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  , finalizado ASC  , dataParaFinalizacao ASC  )  
	 INCLUDE ( coletivoIdCliente , idCliente , idCompromisso , idProcesso ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_advTarefas_D6DDC044F6B18D5258726C1360CC178E ON fabioribeiroaz-producao.dbo.advTarefas (  ativo ASC  , finalizado ASC  , idExecutor ASC  , dataParaFinalizacao ASC  )  
	 INCLUDE ( agendada , coletivoIdCliente , coletivoIdOriginal , coletivoOriginal , descricao , horarioFinal , horarioInicial , idCliente , idCompromisso , idProcesso , idResponsavel , idTipoTarefa , incluidoPor , onde , tsFinalizacao ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.advVerbas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.advVerbas;

CREATE TABLE [fabioribeiroaz-producao].dbo.advVerbas (
	idVerba int IDENTITY(1,1) NOT NULL,
	idTipo int NOT NULL,
	idProfissional int NOT NULL,
	idProcesso int NULL,
	idLancamento int NULL,
	valor decimal(11,2) DEFAULT 0 NOT NULL,
	dataDe smalldatetime NOT NULL,
	dataAte smalldatetime NOT NULL,
	estado varchar(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	comprovante bit DEFAULT 0 NOT NULL,
	comprovanteArquivo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	solicitacao bit DEFAULT 0 NOT NULL,
	aceito bit DEFAULT 1 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	alteradoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	CONSTRAINT PK_advVerbas PRIMARY KEY (idVerba),
	CONSTRAINT FK_advVerbas_advProcessos FOREIGN KEY (idProcesso) REFERENCES [fabioribeiroaz-producao].dbo.advProcessos(idProcesso) ON DELETE SET NULL,
	CONSTRAINT FK_advVerbas_advProfissionais FOREIGN KEY (idProfissional) REFERENCES [fabioribeiroaz-producao].dbo.advProfissionais(idProfissional) ON DELETE CASCADE,
	CONSTRAINT FK_advVerbas_advVerTipos FOREIGN KEY (idTipo) REFERENCES [fabioribeiroaz-producao].dbo.advVerTipos(idTipo) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.fabCidades definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabCidades;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabCidades (
	idCidade int IDENTITY(1,1) NOT NULL,
	descricao varchar(90) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NULL,
	codigoIBGE varchar(10) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NULL,
	idEstado int NULL,
	CONSTRAINT PK__cidades__559AD0FE7D63964E PRIMARY KEY (idCidade),
	CONSTRAINT FK_cidades_estados FOREIGN KEY (idEstado) REFERENCES [fabioribeiroaz-producao].dbo.fabEstados(idEstado) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX nci_msft_1_fabCidades_0400DB523E172D9A937A9B77BAF3A9C1 ON fabioribeiroaz-producao.dbo.fabCidades (  idEstado ASC  )  
	 INCLUDE ( descricao ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.fabLembretes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.fabLembretes;

CREATE TABLE [fabioribeiroaz-producao].dbo.fabLembretes (
	idLembrete int IDENTITY(1,1) NOT NULL,
	idUsuario int NULL,
	mensagem varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	destino varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	incluidoPor varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	lido bit DEFAULT 0 NOT NULL,
	tipo char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'g' NOT NULL,
	CONSTRAINT PK_fabLembretes PRIMARY KEY (idLembrete),
	CONSTRAINT FK_fabLembretes_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX nci_wi_fabLembretes_A28FB0630D61315D37275ADA58E1A22B ON fabioribeiroaz-producao.dbo.fabLembretes (  idUsuario ASC  , lido ASC  , tipo ASC  )  
	 INCLUDE ( destino , mensagem , tsInclusao ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finPlanoContas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finPlanoContas;

CREATE TABLE [fabioribeiroaz-producao].dbo.finPlanoContas (
	idPlanoConta int IDENTITY(1,1) NOT NULL,
	idGrupo int NOT NULL,
	titulo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	codigo varchar(20) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	pagamentoSempreLiberado bit DEFAULT 0 NOT NULL,
	permiteLancamentoQuitado bit DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	padraoVendas bit DEFAULT 0 NOT NULL,
	antecipaVencimento bit DEFAULT 0 NOT NULL,
	terceiroNivel bit DEFAULT 0 NOT NULL,
	criarPeloFinanceiro bit DEFAULT 1 NOT NULL,
	valoresRestritos bit DEFAULT 0 NOT NULL,
	naoAbatePagtoDoSaldoDoCliente bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_finPlanoContas PRIMARY KEY (idPlanoConta),
	CONSTRAINT FK_finPlanoContas_finPlanoContasGrupos FOREIGN KEY (idGrupo) REFERENCES [fabioribeiroaz-producao].dbo.finPlanoContasGrupos(idGrupo) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_finPlanoContas_idGrupo ON fabioribeiroaz-producao.dbo.finPlanoContas (  idGrupo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.opoOportunidades definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.opoOportunidades;

CREATE TABLE [fabioribeiroaz-producao].dbo.opoOportunidades (
	idOportunidade int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	idUsuario int NOT NULL,
	idTipo int NOT NULL,
	idSituacao int NOT NULL,
	titulo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	numero char(15) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataInicio smalldatetime NULL,
	dataEstimada smalldatetime NULL,
	valorEstimado decimal(11,2) DEFAULT 0 NOT NULL,
	comentario varchar(500) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	aproveitada bit DEFAULT 0 NOT NULL,
	aproveitadaData smalldatetime NULL,
	cancelada bit DEFAULT 0 NOT NULL,
	canceladaMotivo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	canceladaData smalldatetime NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	indicadorCanceladoVisto bit DEFAULT 0 NOT NULL,
	valorEstimadoMensal decimal(11,2) DEFAULT 0 NOT NULL,
	deProcesso bit DEFAULT 1 NOT NULL,
	aproveitadaMotivo varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	numeroProcesso int DEFAULT 0 NOT NULL,
	CONSTRAINT PK_opoOportunidades PRIMARY KEY (idOportunidade),
	CONSTRAINT FK_opoOportunidades_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente) ON DELETE CASCADE,
	CONSTRAINT FK_opoOportunidades_opoTipos FOREIGN KEY (idTipo) REFERENCES [fabioribeiroaz-producao].dbo.opoTipos(idTipo) ON DELETE CASCADE,
	CONSTRAINT FK_opoOportunidades_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE,
	CONSTRAINT FK_opoSituacoes_opoStatus FOREIGN KEY (idSituacao) REFERENCES [fabioribeiroaz-producao].dbo.opoSituacoes(idSituacao) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.opoOrcamentos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.opoOrcamentos;

CREATE TABLE [fabioribeiroaz-producao].dbo.opoOrcamentos (
	idOrcamento int IDENTITY(1,1) NOT NULL,
	idOportunidade int NOT NULL,
	titulo varchar(100) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	dataCriacao smalldatetime NULL,
	valor decimal(11,2) DEFAULT 0 NOT NULL,
	arquivo varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	aceito bit DEFAULT 0 NOT NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	dataValidade smalldatetime NULL,
	valorMensal decimal(11,2) DEFAULT 0 NOT NULL,
	comArquivo bit DEFAULT 1 NOT NULL,
	comProduto bit DEFAULT 0 NOT NULL,
	comProdutoTerceiro bit DEFAULT 0 NOT NULL,
	comServico bit DEFAULT 0 NOT NULL,
	valorDesconto decimal(11,2) DEFAULT 0 NOT NULL,
	valorAcrescimo decimal(11,2) DEFAULT 0 NOT NULL,
	valorFrete decimal(11,2) DEFAULT 0 NOT NULL,
	informacoes varchar(4000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	descontoPercentual decimal(11,2) DEFAULT '0' NOT NULL,
	valorItens decimal(11,2) DEFAULT 0 NOT NULL,
	idCondicaoPagamento int NULL,
	dataPrevistaEntrega smalldatetime NULL,
	moeda char(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT 'R' NOT NULL,
	valorConversao decimal(12,3) DEFAULT 0 NOT NULL,
	imprimeMoedaAdd varchar(1) COLLATE SQL_Latin1_General_CP850_CI_AI DEFAULT '' NOT NULL,
	valorDescontoMensal decimal(11,2) DEFAULT 0 NOT NULL,
	valorAcrescimoMensal decimal(11,2) DEFAULT 0 NOT NULL,
	valorFreteMensal decimal(11,2) DEFAULT 0 NOT NULL,
	descontoPercentualMensal decimal(11,2) DEFAULT 0 NOT NULL,
	valorItensMensal decimal(11,2) DEFAULT 0 NOT NULL,
	CONSTRAINT PK_opoOrcamentos PRIMARY KEY (idOrcamento),
	CONSTRAINT FK_opoOrcamentos_fabCondicoesPagamento FOREIGN KEY (idCondicaoPagamento) REFERENCES [fabioribeiroaz-producao].dbo.fabCondicoesPagamento(idCondicaoPagamento) ON DELETE CASCADE,
	CONSTRAINT FK_opoOrcamentos_opoOportunidades FOREIGN KEY (idOportunidade) REFERENCES [fabioribeiroaz-producao].dbo.opoOportunidades(idOportunidade) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.usuAcessos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.usuAcessos;

CREATE TABLE [fabioribeiroaz-producao].dbo.usuAcessos (
	idAcesso int IDENTITY(1,1) NOT NULL,
	idUsuario int NOT NULL,
	[data] datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	ip varchar(32) COLLATE SQL_Latin1_General_CP850_CI_AI NOT NULL,
	CONSTRAINT PK_usuariosAcessos PRIMARY KEY (idAcesso),
	CONSTRAINT FK_usuariosAcessos_usuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.usuDistancias definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.usuDistancias;

CREATE TABLE [fabioribeiroaz-producao].dbo.usuDistancias (
	idDistancia int IDENTITY(1,1) NOT NULL,
	idUsuario int NOT NULL,
	estado varchar(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	cidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	km int DEFAULT 0 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_usuDistancias_idDistancia PRIMARY KEY (idDistancia),
	CONSTRAINT FK_usuDistancias_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.usuPermissoes definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.usuPermissoes;

CREATE TABLE [fabioribeiroaz-producao].dbo.usuPermissoes (
	idUsuarioPermissao int IDENTITY(1,1) NOT NULL,
	idUsuario int DEFAULT 0 NOT NULL,
	idPermissao int DEFAULT 0 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	CONSTRAINT PK_usuPermissoes PRIMARY KEY (idUsuarioPermissao),
	CONSTRAINT FK_usuPermissoes_fabPermissoes FOREIGN KEY (idPermissao) REFERENCES [fabioribeiroaz-producao].dbo.fabPermissoes(idPermissao) ON DELETE CASCADE,
	CONSTRAINT FK_usuPermissoes_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);


-- [fabioribeiroaz-producao].dbo.finLancamentos definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finLancamentos;

CREATE TABLE [fabioribeiroaz-producao].dbo.finLancamentos (
	idLancamento int IDENTITY(1,1) NOT NULL,
	idConta int NOT NULL,
	idPlanoConta int NOT NULL,
	idCentroCusto int NOT NULL,
	operacao char(1) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idForma int NOT NULL,
	modulo char(3) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idCadastro int NULL,
	idPedido int NULL,
	descricao varchar(255) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	nrDocumento varchar(25) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	valor decimal(11,2) DEFAULT 0 NOT NULL,
	dataEmissao smalldatetime NULL,
	dataVencimento smalldatetime NULL,
	dataQuitacao smalldatetime NULL,
	quitado bit DEFAULT 0 NOT NULL,
	recorrente bit DEFAULT 0 NOT NULL,
	recorrenteChave varchar(12) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	previsao bit DEFAULT 1 NOT NULL,
	cobrancaEnviada bit DEFAULT 0 NULL,
	parcelado bit DEFAULT 0 NOT NULL,
	identificacao int DEFAULT 0 NOT NULL,
	observacao varchar(1000) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idUsuarioInclusao int NULL,
	idUsuarioAlteracao int NULL,
	parcela int DEFAULT 1 NOT NULL,
	parcelaMaxima int DEFAULT 1 NOT NULL,
	dataVencimentoOriginal smalldatetime NULL,
	pagtoLiberado int DEFAULT 1 NOT NULL,
	dataParaPrevisao smalldatetime NULL,
	recorrenteVencendoVisto bit DEFAULT 0 NOT NULL,
	recebimentoFuturo bit DEFAULT 0 NOT NULL,
	recebimentoFuturoRel bit DEFAULT 0 NOT NULL,
	idTerceiro int NULL,
	arquivoDocumento varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	arquivoComprovante varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idClientePagar int NULL,
	idProcessoPagar int NULL,
	idArea int NULL,
	identificacaoPagar varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	identificacaoPagar2 varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	arquivoDocumento2 varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	arquivoComprovante2 varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	verba bit NULL,
	verbaDataDe smalldatetime NULL,
	verbaDataAte smalldatetime NULL,
	verbaEstado varchar(2) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	verbaCidade varchar(50) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	idCentroResultado int NULL,
	secundaria bit DEFAULT 0 NOT NULL,
	geradoPeloProcesso bit NULL,
	sequenciaHerdeiro int NULL,
	idUnidade int NULL,
	rateioFeito bit NULL,
	naoAbatePagtoDoSaldoDoCliente bit DEFAULT 0 NOT NULL,
	idHonorario int NULL,
	CONSTRAINT PK_finLancamentos PRIMARY KEY (idLancamento),
	CONSTRAINT FK_finLancamentos_finAreas FOREIGN KEY (idArea) REFERENCES [fabioribeiroaz-producao].dbo.finAreas(idArea) ON DELETE CASCADE,
	CONSTRAINT FK_finLancamentos_finCentrosCusto FOREIGN KEY (idCentroCusto) REFERENCES [fabioribeiroaz-producao].dbo.finCentrosCusto(idCentroCusto) ON DELETE CASCADE,
	CONSTRAINT FK_finLancamentos_finContas FOREIGN KEY (idConta) REFERENCES [fabioribeiroaz-producao].dbo.finContas(idConta) ON DELETE CASCADE,
	CONSTRAINT FK_finLancamentos_finPlanoContas FOREIGN KEY (idPlanoConta) REFERENCES [fabioribeiroaz-producao].dbo.finPlanoContas(idPlanoConta) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_finLancamentos_ativo ON fabioribeiroaz-producao.dbo.finLancamentos (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_dataEmissao ON fabioribeiroaz-producao.dbo.finLancamentos (  dataEmissao ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_dataQuitacao ON fabioribeiroaz-producao.dbo.finLancamentos (  dataQuitacao ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_dataVencimento ON fabioribeiroaz-producao.dbo.finLancamentos (  dataVencimento ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_idArea ON fabioribeiroaz-producao.dbo.finLancamentos (  idArea ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_idCadastro ON fabioribeiroaz-producao.dbo.finLancamentos (  idCadastro ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_idCentroCusto ON fabioribeiroaz-producao.dbo.finLancamentos (  idCentroCusto ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_idCentroResultado ON fabioribeiroaz-producao.dbo.finLancamentos (  idCentroResultado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_idPlanoConta ON fabioribeiroaz-producao.dbo.finLancamentos (  idPlanoConta ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_finLancamentos_quitado ON fabioribeiroaz-producao.dbo.finLancamentos (  quitado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_msft_1_finLancamentos_D9407E19941674E2B9792E6BF6AC9C4D ON fabioribeiroaz-producao.dbo.finLancamentos (  ativo ASC  , idPlanoConta ASC  , quitado ASC  , dataQuitacao ASC  )  
	 INCLUDE ( dataEmissao , dataVencimento , descricao , idCadastro , operacao , valor ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_finLancamentos_1C51B6681264548C2E4760E2A4DA5E73 ON fabioribeiroaz-producao.dbo.finLancamentos (  ativo ASC  , operacao ASC  , dataVencimento ASC  , idPlanoConta ASC  )  
	 INCLUDE ( arquivoComprovante , arquivoComprovante2 , arquivoDocumento , arquivoDocumento2 , cobrancaEnviada , dataEmissao , dataQuitacao , descricao , idCadastro , idClientePagar , idConta , identificacao , idForma , idPedido , modulo , nrDocumento , pagtoLiberado , parcela , parcelado , parcelaMaxima , previsao , quitado , recebimentoFuturo , recebimentoFuturoRel , recorrente , valor ) 
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 50   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.finPrestacaoContas definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.finPrestacaoContas;

CREATE TABLE [fabioribeiroaz-producao].dbo.finPrestacaoContas (
	idPrestacao int IDENTITY(1,1) NOT NULL,
	idLancamento int NOT NULL,
	levantado decimal(11,2) DEFAULT 0 NOT NULL,
	irpj decimal(11,2) DEFAULT 0 NOT NULL,
	carta decimal(11,2) DEFAULT 0 NOT NULL,
	honorarios decimal(11,2) DEFAULT 0 NOT NULL,
	tarifa decimal(11,2) DEFAULT 0 NOT NULL,
	liquidoRecebido decimal(11,2) DEFAULT 0 NOT NULL,
	tsInclusao datetime DEFAULT getdate() NOT NULL,
	CONSTRAINT PK_finPrestacaoContas_idPrestado PRIMARY KEY (idPrestacao),
	CONSTRAINT FK_finPrestacaoContas_finLancamentos FOREIGN KEY (idLancamento) REFERENCES [fabioribeiroaz-producao].dbo.finLancamentos(idLancamento)
);
 CREATE NONCLUSTERED INDEX IX_finPrestacaoContas_idLancamento ON fabioribeiroaz-producao.dbo.finPrestacaoContas (  idLancamento ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- [fabioribeiroaz-producao].dbo.flwFollows definição

-- Drop table

-- DROP TABLE [fabioribeiroaz-producao].dbo.flwFollows;

CREATE TABLE [fabioribeiroaz-producao].dbo.flwFollows (
	idFollow int IDENTITY(1,1) NOT NULL,
	idCliente int NOT NULL,
	idAcao int NOT NULL,
	idTipo int NOT NULL,
	idUsuario int NOT NULL,
	[data] smalldatetime NOT NULL,
	horario int DEFAULT 0 NOT NULL,
	comentario varchar(500) COLLATE SQL_Latin1_General_CP850_CI_AI NULL,
	finalizado bit DEFAULT 0 NOT NULL,
	dataFinalizacao smalldatetime NULL,
	horarioFinalizacao int NULL,
	ativo bit DEFAULT 1 NOT NULL,
	tsInclusao datetime DEFAULT dateadd(hour,(-3),getdate()) NOT NULL,
	tsAlteracao datetime NULL,
	idOportunidade int NULL,
	chegou bit DEFAULT 0 NOT NULL,
	tsChegou datetime NULL,
	naoComparecimento bit DEFAULT 0 NOT NULL,
	prioridade bit DEFAULT 0 NOT NULL,
	CONSTRAINT PK_flwFollows PRIMARY KEY (idFollow),
	CONSTRAINT FK_flwFollows_advClientes FOREIGN KEY (idCliente) REFERENCES [fabioribeiroaz-producao].dbo.advClientes(idCliente) ON DELETE CASCADE,
	CONSTRAINT FK_flwFollows_fabHistoricoTipos FOREIGN KEY (idTipo) REFERENCES [fabioribeiroaz-producao].dbo.fabHistoricoTipos(idHistoricoTipo) ON DELETE CASCADE,
	CONSTRAINT FK_flwFollows_flwAcoes FOREIGN KEY (idAcao) REFERENCES [fabioribeiroaz-producao].dbo.flwAcoes(idAcao) ON DELETE CASCADE,
	CONSTRAINT FK_flwFollows_opoOportunidades FOREIGN KEY (idOportunidade) REFERENCES [fabioribeiroaz-producao].dbo.opoOportunidades(idOportunidade),
	CONSTRAINT FK_flwFollows_usuUsuarios FOREIGN KEY (idUsuario) REFERENCES [fabioribeiroaz-producao].dbo.usuUsuarios(idUsuario) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_flwFollows_ativo ON fabioribeiroaz-producao.dbo.flwFollows (  ativo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_flwFollows_data ON fabioribeiroaz-producao.dbo.flwFollows (  data ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_flwFollows_finalizado ON fabioribeiroaz-producao.dbo.flwFollows (  finalizado ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_flwFollows_horario ON fabioribeiroaz-producao.dbo.flwFollows (  horario ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_flwFollows_idAcao ON fabioribeiroaz-producao.dbo.flwFollows (  idAcao ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_flwFollows_idCliente ON fabioribeiroaz-producao.dbo.flwFollows (  idCliente ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_flwFollows_idTipo ON fabioribeiroaz-producao.dbo.flwFollows (  idTipo ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_flwFollows_prioridade ON fabioribeiroaz-producao.dbo.flwFollows (  prioridade ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 80   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX nci_wi_flwFollows_D6AE7B8768B44C7A4C135E4061A8858B ON fabioribeiroaz-producao.dbo.flwFollows (  ativo ASC  , data ASC  , idTipo ASC  , prioridade ASC  , horario ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 25   ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = ON , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- dbo.apiProcessos fonte

ALTER VIEW dbo.apiPreProcessos
AS
SELECT        p.idCliente, p.acaoColetiva, c.apelido AS cliente, (CASE WHEN c.cnpj IS NOT NULL AND c.cnpj <> '' THEN c.cnpj ELSE c.cpf END) AS documento, c.telCelular, c.email, p.numero, rap.nome AS autorPeticao, 
                         rr.nome AS responsavel, ui.nome AS usuarioInclusao, eo.titulo AS escritorioOrigem, er.titulo AS escritorioResponsavel, p.sintese, p.dataDistribuicao, s.titulo AS status, n.titulo AS natureza, 
                         n.mostraHistoricoNumeros, t.titulo AS tipo, f.titulo AS fase, o.titulo AS orgao, p.estado, p.cidade, i.titulo AS instancia, v.titulo AS vara, r.titulo AS relevancia, b.titulo AS probabilidade, p.valorCausa, 
                         p.valorHonorarios, p.valorHonorariosTipo, p.observacoes, st.titulo AS sentenca, p.dataSentenca, p.alvara, p.valorDeferido, p.dataEncerramento, p.tsInclusao, p.recurso, stR.titulo AS recursoSentenca, 
                         p.recursoDataSentenca, p.alvaraPendente, p.alvaraPendenteDesde, p.historicoNumeros, fo.apelido AS fornecedor, p.recebeAcordo, p.recebeRPV, p.recebePrecatorio, p.recebeAlvara, p.recebeBanco, 
                         p.recebeDataLiberacao, p.pendOutrosValores, p.pendOutrosValoresDataEncerramento, p.pendOutrosValoresDeferido, p.pendOutrosValoresValorDeferido, p.temResponsavel, p.nomeResponsavel, 
                         p.cpfResponsavel, p.imposto,
                             (SELECT        SUM(bancarioTarifa) AS bancarioTarifa
                               FROM            dbo.advProcessosDadosHerdeiros
                               WHERE        (idProcesso = p.idProcesso)) AS tarifa, p.complementoPositivo, p.RPV, fcc.titulo AS bancarioBanco, p.bancarioTipoConta, p.bancarioAgencia, p.bancarioConta, p.bancarioFavorecido, p.bancarioCpf, 
                         p.nomeReu, p.sucumbencia, fc.titulo AS contaRecebimento, p.dataLiberacaoValorDeferido, p.boleto, p.precatorio, p.emitir, p.emitido, p.formaRecebimento, p.valorHonorariosDestaque, 
                         p.valorHonorariosDestaqueTipo, p.dataPrevisaoRepasseCliente, p.dataPrevisaoHonorariosDestaque, p.dataPrevistaClienteReceber, p.preProcessoPasta, p.preProcessoDataCriacao, 
                         rr.nome AS profissionalResponsavel, p.dataEntrada AS dataEntradaPreProcesso, p.preProcessoDataRealizada, p.preProcesso, p.perdido, p.tsPerdido, mp.titulo AS motivoPerda, p.convertido, p.tsConversao, 
                         p.clientePrimeiraVez, pt.titulo AS statusTipoPreProcesso
FROM            dbo.advProcessos AS p INNER JOIN
                         dbo.advClientes AS c ON p.idCliente = c.idCliente INNER JOIN
                         dbo.advProfissionais AS rap ON p.idAutorPeticao = rap.idProfissional INNER JOIN
                         dbo.advProfissionais AS rr ON p.idResponsavel = rr.idProfissional INNER JOIN
                         dbo.usuUsuarios AS ui ON p.idUsuarioInclusao = ui.idUsuario LEFT OUTER JOIN
                         dbo.advProEscritorios AS eo ON p.idEscritorioOrigem = eo.idEscritorio LEFT OUTER JOIN
                         dbo.advProEscritorios AS er ON p.idEscritorioResponsavel = er.idEscritorio LEFT OUTER JOIN
                         dbo.advProStatus AS s ON p.idStatus = s.idStatus LEFT OUTER JOIN
                         dbo.advProNaturezas AS n ON p.idNatureza = n.idNatureza LEFT OUTER JOIN
                         dbo.advProTipos AS t ON p.idTipo = t.idTipo LEFT OUTER JOIN
                         dbo.advProFases AS f ON p.idFase = f.idFase LEFT OUTER JOIN
                         dbo.advProOrgaos AS o ON p.idOrgao = o.idOrgao LEFT OUTER JOIN
                         dbo.advProInstancias AS i ON p.idInstancia = i.idInstancia LEFT OUTER JOIN
                         dbo.advProVaras AS v ON p.idVara = v.idVara LEFT OUTER JOIN
                         dbo.advProRelevancias AS r ON p.idRelevancia = r.idRelevancia LEFT OUTER JOIN
                         dbo.advProProbabilidades AS b ON p.idProbabilidade = b.idProbabilidade LEFT OUTER JOIN
                         dbo.advProSentencas AS st ON p.idSentenca = st.idSentenca LEFT OUTER JOIN
                         dbo.advProSentencas AS stR ON p.recursoIdSentenca = stR.idSentenca LEFT OUTER JOIN
                         dbo.advFornecedores AS fo ON c.idFornecedor = fo.idFornecedor LEFT OUTER JOIN
                         dbo.finContas AS fc ON p.idConta = fc.idConta LEFT OUTER JOIN
                         dbo.finContasClientes AS fcc ON p.bancarioBancoId = fcc.idContaCliente LEFT OUTER JOIN
                         dbo.usuUsuarios AS u ON p.idUsuarioInclusao = u.idUsuario LEFT OUTER JOIN
                         dbo.advPreMotivosPerda AS mp ON p.idMotivoPerda = mp.idMotivo LEFT OUTER JOIN
                         dbo.advPreStatusTipos AS pt ON p.preProcessoIdTipo = pt.idTipo
WHERE        (p.ativo = 1);