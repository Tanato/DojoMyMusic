# Dojo MyMusic

# Especificação de Negócio

O cliente deseja uma aplicação que permita o usuário buscar músicas e adicionar músicas do retorno da busca em sua playlist de favoritos.

Esta versão MVP da aplicação consiste basicamente em listar músicas a partir de uma busca realizada pelo usuário e permitir que o usuário adicione músicas do resultado em sua playlist de Favoritos.

O Cliente já possúi um banco de dados SQL Server com a estrutura de tabelas, lista de músicas e usuários, e o modelo está na especificação técnica desta história.

# Especificação Técnica

No levantamento de necessidades com o cliente, foi identificado a necessidade de dois módulos principais no backend:

* Modulo de busca de músicas
* Módulo de controle de playlists

## Módulo de Buscas de Músicas

Este módulo tem como objetivo principal retornar uma lista de músicas a partir de uma busca realizada pelo usuário.

Esta busca deve ser realizada com um mínimo de 3 caracteres, e que deve ser validado no servidor. O servidor por sua vez deve retornar um erro HTTP 400, informando ao usuário que um mínimo de 3 caracteres deve ser digitado para realizar uma busca.

Há uma necessidade grande de desempenho deste módulo e para alcançar este objetivo foi acordado com o cliente que será feito o uso de duas técnicas principais:

* Micro-serviços: Para atingir este objetivo é necessário que o cliente possa levantar diversas instâncias desta API, de forma independente do módulo de controle de Playlists.
* Cache: Para atingir este objetivo, deverá ser utilizado o controle de cache das buscas recentes utilizando o serviço de Redis, evitando a necessidade de busca no banco de dados para consultas repetidas em um intervalo menor ou igual a 10 minutos.

Esta API de buscas deve ser construido como uma API pública, permitindo que mesmo um usuário não autenticado realize uma busca.

Deve haver uma documentação da API utilizando o **Swagger** na "home" da API, que será exibida sempre que o usuário digitar a URL base.

## Módulo de controle de Playlist 

O Cliente necessita que

Estas APIs devem ser privadas, permitindo a consulta e edição de músicas da playlist apenas para um usuário logado com o Token válido. O Cliente já possúi uma API de autenticação, que utiliza JWT e Oauth para gerar um token Bearer, que é passado pela aplicação de FrontEnd automaticamente no header de cada request autenticado. Caso o header de autenticação não seja válido, a API deve retornar um erro HTTP 401 *Unauthorized*.

Deve haver uma documentação da API utilizando o **Swagger** na "home" da API, que será exibida sempre que o usuário digitar a URL base.




# Tarefas

* Criar projeto de API para músicas
* Adicionar middleware de conexão com banco de dados
* Criar classe de Música mapeada com o banco de dados
* Criar método get
* Adicionar middleware de Swagger
* Adicionar middleware de Redis
* Adicionar controle de cache na Action get
* Criar projeto de Testes unitários para método get
* Criar middleware de log (console.log)

* Criar projeto de API para playlist
* Adicionar middleware de conexão com banco de dados
* Criar classe de Música, Playlist, Usuário mapeada com o banco de dados
* Criar Actions de GET, POST e DELETE
* Adicionar middleware de Swagger
* Adicionar middleware de autenticação
* Criar projeto de testes unitários
* Criar "filter" para log em Post e Delete via Attribute